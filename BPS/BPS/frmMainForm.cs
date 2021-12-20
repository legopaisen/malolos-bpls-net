// MODIFICATIONS:

// RMC 20111228 added deletion in user trail where user logs-out
// RMC 20111228 Added auto-add of due dates upon change of tax year
// RMC 20111226 added system code in Common:Message_Box
// RMC 20111128 consider Business mapping - unencoded in Business Records-Add
// RMC 20111021 transferred validation of connection
// RMC 20110809 added trailing of system login
// RMC 20110808
// RMC 20110725 modified getting current date
// MCR 20140602 addedd ReportTools

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.EncryptUtilities;
using Amellar.Common.DataConnector;
using Amellar.Common.Message_Box;
using Amellar.Common.NewDateTimeUtilities;
using Amellar.Common.LogIn;
using ComponentFactory.Krypton.Ribbon;
using Amellar.Common.frmBns_Rec;
using Amellar.Common.BinSearch;
using Amellar.BPLS.SearchAndReplace;
using Amellar.Common.User;
using Amellar.Common.ModuleRights;
using Amellar.Common.TaskManager;
using Amellar.Common.AuditTrail;
using Amellar.Modules.Utilities;
using Amellar.Common.DeficientRecords;
using Amellar.Modules.Retirement;
using Amellar.Modules.PermitUpdate;
using Amellar.Modules.BusinessPermit;
using Amellar.Modules.InspectorsDetails;
using Amellar.Modules.InspectionTool;
using Amellar.Modules.SpecialOrdinances;
using Amellar.Common.AppSettings;
using Amellar.Modules.ApplicationRequirements;
using Amellar.Modules.BusinessMapping;
//using Amellar.Modules.PaymentHistory;
using Amellar.Common.Reports;
using Amellar.Common.ProgressControl;
using Amellar.Modules.BusinessReports;
using Amellar.Common.Tools;
using Amellar.Modules.PaymentHistory;
using Amellar.Modules.OwnerProfile;
using CDOReport;
using ReAssessment;
using BusinessSummary;
using Amellar.Common.ReportTools; // MCR 20140602
using Amellar.BPLS.Billing;
using Amellar.Common.SOA;
using Amellar.Common.StringUtilities;
using Amellar.Modules.EPS;
using TreasurersModule;
using InspectionTool;
using Amellar.Modules.DILGReport;
using System.Reflection;
using Amellar.Modules.HealthPermit;



namespace BPLSBilling
{
    public partial class frmMainForm : Form
    {
        public string asd = "";
        private DateTime m_dtValidDate;
        public string m_sUser;
        private string m_sVersion;  // RMC 20150603 Created/modified version validation
        private bool m_bIsNewDiscovery = false; // ALJ 20130219 NEW Discovery - delinquent  // RMC 20161109 merged discovery-delinq

        public frmMainForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void picBnsRecord_MouseMove(object sender, MouseEventArgs e)
        {
            //lblDesc.Text = "Business Record: Add, Edit, Delete";
            lblDesc.Text = "Business Record: Add";
        }

        private void picMainForm_MouseMove(object sender, MouseEventArgs e)
        {
            lblDesc.Text = "AMELLAR SOLUTIONS: INFORMATION IS THE BUSINESS. TECHNOLOGY IS THE TOOL. PEOPLE MAKE THE SYSTEM WORK.";
        }

        private void picNewBns_MouseMove(object sender, MouseEventArgs e)
        {
            lblDesc.Text = "Apply New Business";
        }

        private void picRenBns_MouseMove(object sender, MouseEventArgs e)
        {
            lblDesc.Text = "Apply Renewal Business";
        }

        private void picBill_MouseMove(object sender, MouseEventArgs e)
        {
            //picBillBig.Show();
            //lblDesc.Text = "Billing Module";
        }

        private void picFileManager_MouseMove(object sender, MouseEventArgs e)
        {
            //picFileManagerBig.Show();
            //lblDesc.Text = "File Manager Viewer";
        }

        private void picLogOut_MouseMove(object sender, MouseEventArgs e)
        {
            lblDesc.Text = "Log-out your account";
        }

        private void picExit_MouseMove(object sender, MouseEventArgs e)
        {
            lblDesc.Text = "Close the BPLS System";
        }

        private void lblDesc_MouseMove(object sender, MouseEventArgs e)
        {
            //picBnsRecBig.Hide();
            //picNewBnsBig.Hide();
            //picRenBnsBig.Hide();
            ////picBillBig.Hide();
            ////picFileManagerBig.Hide();
            //picLogOutBig.Hide();
            //picExitBig.Hide();
        }
        //JARS 20181127 (S)
        //Version version = Assembly.GetExecutingAssembly().GetName().Version;
        //DateTime creationDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
        internal static class AssemblyCreationDate
        {
            public static readonly DateTime Value;
            public static readonly string version;

            static AssemblyCreationDate()
            {

                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Value = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
            }
        }
        //JARS 20181127 (E)

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            //JARS 20181127 THIS SHOWS THE DATE THE SYSTEM WAS COMPILED, 
            //AND WILL NOT CHANGE UNTIL THERE ARE CHANGES MADE TO THE SYSTEM (IN ANY FORM)
            this.Text = "Business Permit System | Version: " + AssemblyCreationDate.Value.ToString();

            
//            // AST 20150410 Added this block(s) // One-time use only
//            OracleResultSet result0 = new OracleResultSet();
//            result0.Query = @"select * from user_tab_cols
//            where (column_name like '%USR_CODE%' 
//            or column_name like '%USER_CODE%'
//            or column_name like '%USER%')
//            and data_length <= 10";
//            if (result0.Execute())
//            {
//                while (result0.Read())
//                {
//                    string strTable = result0.GetString("table_name");
//                    string strColumn = result0.GetString("column_name");

//                    OracleResultSet rs = new OracleResultSet();
//                    rs.Query = string.Format("alter table {0} modify {1} varchar2(20)", strTable, strColumn);
//                    if (rs.ExecuteNonQuery() == 0) { }
//                }
//            }
//            result0.Close();
//            // AST 20150410 Added this block(e) // One-time use only

            /*result0.Query = "SELECT * FROM User_Tables";
            if (result0.Execute()) 
            {
                while (result0.Read())
                {
                    
                }
            }
            result0.Close();*/
            bool blnIsConnectionOpen = false;

            // RMC 20111021 transferred validation of connection
            blnIsConnectionOpen = DataConnectorManager.Instance.OpenConnection();
            if (!blnIsConnectionOpen)
            {
                MessageBox.Show("error connection");
                Dispose();
            }
            // RMC 20111021 transferred validation of connection

            // RMC 20140724 added hiding of Business Mapping Menu if client has no GIS (s)
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();
            }
            catch
            {
                tabBusinessMapping.Visible = false;
            }
            // RMC 20140724 added hiding of Business Mapping Menu if client has no GIS (e)

            // RMC 20150603 Created/modified version validation (s)
            m_sVersion = "2015-AME-BP-A-0001";

            //if (!ValidateVersion()) //JARS 20170614 COMMENT OUT
            //    return;
            // RMC 20150603 Created/modified version validation (e)

            //DateTime tDateTime;
            string sUsrDiv = string.Empty;  // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)
            string sDateTime;
            try
            {
                using (frmLogIn frmLogInOut = new frmLogIn())
                {
                    frmLogInOut.ShowDialog();
                    m_sUser = frmLogInOut.m_sUserCode;
                    // RMC 20110808
                    if (m_sUser == "")
                        this.Close();
                    // RMC 20110808

                    OracleResultSet pSet = new OracleResultSet();
                    //pSet.Query = "SELECT USR_LN, USR_FN, USR_MI, USR_POS FROM SYS_USERS WHERE TRIM(USR_CODE) = :1";
                    pSet.Query = "SELECT USR_LN, USR_FN, USR_MI, USR_POS, usr_div FROM SYS_USERS WHERE TRIM(USR_CODE) = :1";    // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)
                    pSet.AddParameter(":1", m_sUser.Trim());
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            lblUser.Text = " ";
                            lblCode.Text = " ";
                            string sLn, sFn, sMi, sPos, sUser;
                            sLn = pSet.GetString("usr_ln").Trim();
                            sFn = pSet.GetString("usr_fn").Trim();
                            sMi = pSet.GetString("usr_mi").Trim();
                            sPos = pSet.GetString("usr_pos").Trim();
                            sUsrDiv = pSet.GetString("usr_div").Trim();  // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)

                            if (sMi.Length > 0)
                                sMi = sMi + ". ";
                            else
                                sMi = " ";

                            sUser = sFn + " " + sMi + sLn;
                            lblUser.Text = "USER: " + sUser;
                            lblCode.Text = "USERCODE: " + m_sUser;

                            // RMC 20110809 added trailing of system login (s)
                            if (AuditTrail.InsertTrail("AULI", "trail_table/a_trail", sUser) == 0)
                            {
                                return;
                            }
                            if (AuditTrail.InsertTrail("ALI", "a_trail", sUser) == 0)
                            {
                                return;
                            }
                            // RMC 20110809 added trailing of system login (e)
                        }

                    }

                    ValidateDueDateSetup();
                }



                //blnIsConnectionOpen = DataConnectorManager.Instance.OpenConnection();

                OracleResultSet result = new OracleResultSet();
                OracleResultSet result1 = new OracleResultSet();


                /*result.Query = "SELECT sysdate FROM DUAL";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_dtValidDate = result.GetDateTime("sysdate");
                        m_dtValidDate = SysDate.SystemDate.GetStartingDate(m_dtValidDate);

                        result1.Query = "UPDATE DATETIME_SETTING SET datetime = :1 where description = :2";
                        result1.AddParameter(":1", m_dtValidDate);
                        result1.AddParameter(":2", "current_datetime");
                        if (result1.ExecuteNonQuery() == 0)
                        {
                            result1.Close();
                        }

                    }
                }
                result.Close();

                result.Query = "SELECT DATETIME FROM DATETIME_SETTING";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        m_dtValidDate = result.GetDateTime(0);
                    }
                }
                result.Close();*/
                // RMC 20110725 modified getting current date

                m_dtValidDate = AppSettingsManager.GetSystemDate(); // RMC 20110725 modified getting current date

                lblDate.Text = string.Format("Today is {0:MMMM dd, yyyy}", m_dtValidDate);
                //lblDate.Text += "\nVERSION: 19200-00005"; // RMC 20150425 disabled viewing of version in interface
                // RMC 20110725 modified getting current date (s)
                if (AppSettingsManager.bFreezedDate)
                    lblDate.Text += "; DATE FREEZED";
                // RMC 20110725 modified getting current date (e)


                AppSettingsManager.GetSystemType = "A"; // RMC 20150226 adjustment in blob configuration

                result.Query = "select * from config where code = '66'";
                if (result.Execute())
                {
                    if (!result.Read())
                    {
                        result.Close();
                        result.Query = "insert into CONFIG (CODE, OBJECT, REMARKS, SECURITY) ";
                        result.Query += "values ('66', 'ENTERPRISE', 'VERSION (VALUES: ENTERPRISE; ACE)', 1)";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                    }
                } result.Close();

                btnRevYear.Visible = false; // RMC 20160109 ENABLE THIS ONLY UNTIL FURTHER NOTICE OF GR FR TUMAUINI
                //btnSpecialOrd.Visible = false; // RMC 20160109 SYS_PROG tool - enable only when needed (edit & delete no function yet as of 20160217)

                // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan) (s)
                 if (sUsrDiv == "ENGINEERING" || sUsrDiv == "ZONING" || sUsrDiv == "SANITARY" ||
                  sUsrDiv == "BFP" || sUsrDiv == "BENRO" || sUsrDiv == "CENRO" || sUsrDiv == "CHO" || sUsrDiv == "SANITARY" || sUsrDiv == "MAPUMA" || sUsrDiv == "HEALTH" || sUsrDiv == "MARKET")
                {
                    tabRecords.Visible = false;
                    tabApplication.Visible = false;
                    tabBilling.Visible = false;
                    tabInspectorMod.Visible = false;
                    tabReports.Visible = false;
                    tabTables.Visible = false;
                    tabUtilities.Visible = false;
                    if (sUsrDiv == "ENGINEERING" || sUsrDiv == "BFP" || sUsrDiv == "CHO" || sUsrDiv == "PESO" || sUsrDiv == "ZONING" || sUsrDiv == "SANITARY" || sUsrDiv == "MAPUMA")
                    {
                        tabEPS.Visible = true;
                        tabZoning.Visible = true;
                        tabSanitary.Visible = true; //AFM 20200107
                        tabApplication.Visible = true; //AFM 20200108
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                        tabCenro.Visible = false;
                    }
                    else
                    {
                        tabEPS.Visible = false;
                        tabZoning.Visible = false;
                        tabSanitary.Visible = false; //AFM 20200107
                    }

                    if (sUsrDiv == "CENRO")
                    {
                        tabCenro.Visible = true;
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                    }
                    else if(sUsrDiv == "HEALTH")
                    {
                        tabCenro.Visible = false;
                        tabHealth.Visible = true;
                        tabMarket.Visible = false;
                    }
                    else if (sUsrDiv == "MARKET")
                    {
                        tabCenro.Visible = false;
                        tabMarket.Visible = true;
                        tabHealth.Visible = false;
                    }
                    else
                    {
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                        tabCenro.Visible = false;
                    }


                    tabNegaList.Visible = true;
                }
                else
                {
                    tabRecords.Visible = true;
                    tabApplication.Visible = true;
                    tabBilling.Visible = true;
                    tabInspectorMod.Visible = true;
                    tabReports.Visible = true;
                    tabTables.Visible = true;
                    tabUtilities.Visible = true;
                    tabNegaList.Visible = true;
                    tabEPS.Visible = false;
                    tabZoning.Visible = false; //AFM 20191219
                    tabSanitary.Visible = false; //AFM 20200107
                }

                if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                {
                    tabEPS.Visible = true;
                    tabZoning.Visible = true; //AFM 20191219
                    tabSanitary.Visible = true; //AFM 20200107
                    tabHealth.Visible = true;
                    tabMarket.Visible = true;
                    tabCenro.Visible = true;
                }

                // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan) (e)

            }
            catch (NullReferenceException)
            {
                blnIsConnectionOpen = false;
            }

            /*if (!blnIsConnectionOpen)
            {
                MessageBox.Show("error");
                Dispose();
            }*/
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            using (frmMsgBox msgbox = new frmMsgBox())
            {
                string sMsg = "Are you sure you want to exit?";
                msgbox.m_sMsg = "EXIT";
                msgbox.m_slbl = sMsg;
                msgbox.m_sModCode = "ALI";  // RMC 20111226 added system code in Common:Message_Box
                msgbox.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABA"))   // RMC 20110808
            {
                // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (s)
                bool bContinue = true;
                OracleResultSet pSet = new OracleResultSet();
                int intCnt = 0;
                string strInsNo = string.Empty;

                pSet.Query = "select count(*) from btm_temp_businesses where trim(old_bin) is not null and trim(bin) is null";
                int.TryParse(pSet.ExecuteScalar(), out intCnt);

                if (intCnt > 0)
                {
                    using (frmUnOfficialList frmList = new frmUnOfficialList())
                    {
                        frmList.Source = "BUSINESS MAPPING";
                        frmList.ShowDialog();
                        bContinue = frmList.Continue;
                        strInsNo = frmList.InspectionNumber;
                    }
                }
                else
                    bContinue = true;
                // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (e)

                pSet.Close();   // RMC 20161121 corrections in adding new record button

                if (bContinue)
                {
                    using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                    {
                        frmBussRec.Text = "BUSINESS RECORD - ADD";
                        frmBussRec.FormState(0);
                        frmBussRec.m_sFormStatus = "BUSS-ADD-NEW";
                        // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (s)
                        if (strInsNo != "")
                            frmBussRec.m_strInspectionNo = strInsNo;
                        else
                            frmBussRec.m_strInspectionNo = "";
                        // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (e)
                        frmBussRec.ShowDialog();
                        if (frmBussRec.m_bAddNew)   // RMC 20111128
                            btnAdd_Click(sender, e);
                    }
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABV"))  // RMC 20110808
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABE"))   // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "BUSINESS RECORD - EDIT";
                    frmBussRec.FormState(1);
                    frmBussRec.m_sFormStatus = "BUSS-EDIT";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        btnEdit_Click(sender, e);
                }
            }
        }
        private void btnAppNew_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABA"))    // RMC 20110808
            {
                // RMC 20110816 (s)
                bool bContinue = true;  // RMC 20111128
                OracleResultSet pSet = new OracleResultSet();
                int intCnt = 0;
                int intCntInsp = 0;
                int intCntMapped = 0;
                string strInsNo = string.Empty;

                //pSet.Query = "select count(*) from unofficial_info_tbl where trim(bin_settled) is null ";
                //int.TryParse(pSet.ExecuteScalar(), out intCntInsp);

                //pSet.Query = "select count(*) from btm_temp_businesses where trim(old_bin) is null ";
                //int.TryParse(pSet.ExecuteScalar(), out intCntMapped);

                //intCnt = intCntInsp + intCntMapped;

                //if (intCnt > 0)
                //{
                //    using (frmUnOfficialList frmList = new frmUnOfficialList())
                //    {
                //        // RMC 20150426 QA corrections (s) temp disabled this for tumauini
                //        //if (AppSettingsManager.GetConfigValue("10") == "011")
                //        //{
                //        //    bContinue = true;
                //        //    strInsNo = "";
                //        //}// RMC 20150426 QA corrections (e)
                //        //else
                //        {
                //            frmList.Source = "INSPECTION";
                //            frmList.ShowDialog(this);
                //            bContinue = frmList.Continue;  // RMC 20111128
                //            strInsNo = frmList.InspectionNumber;
                //        }
                //    }
                //}
                //else
                    bContinue = true;  // RMC 20111128
                // RMC 20110816 (e)

                pSet.Close();   // RMC 20161121 corrections in adding new record button

                if (bContinue)  // RMC 20111128
                {
                    using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                    {
                        frmBussRec.ObjectPass(sender, e); //GMC 20150819 Send Object and EventArgs of this button
                        frmBussRec.Text = "Apply New Business";
                        frmBussRec.m_sFormStatus = "NEW-APP";
                        //// RMC 20110816 (s)
                        //if (strInsNo != "")
                        //    frmBussRec.m_strInspectionNo = strInsNo;
                        //else
                        //    frmBussRec.m_strInspectionNo = "";
                        //// RMC 20110816 (e)

                        // RMC 20161109 merged discovery-delinq (s)
                        frmBussRec.NewDiscovery = m_bIsNewDiscovery; // ALJ 20130219 NEW Discovery - delinquent - pass the value
                        m_bIsNewDiscovery = false; // ALJ 20130219 NEW Discovery - delinquent - then reset
                        // RMC 20161109 merged discovery-delinq (e)

                        frmBussRec.ShowDialog(this);
                        if (frmBussRec.m_bAddNew)   // RMC 20111128
                            btnAppNew_Click(sender, e);
                    }
                }
            }
        }

        private void tbnAppRen_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABA"))    // rmc 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.ObjectPass(sender, e); //GMC 20150819 Send Object and EventArgs of this button
                    frmBussRec.Text = "Apply Renewal Business";
                    frmBussRec.FormState(0);
                    frmBussRec.m_sFormStatus = "REN-APP";
                    frmBussRec.ShowDialog(this);
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        tbnAppRen_Click(sender, e);
                }
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAU"))  // RMC 20110809
            {
                using (frmUser frmUser = new frmUser())
                {
                    frmUser.Source = "BPS";
                    //frmUser.Source = "BTAS";
                    frmUser.ShowDialog();
                }
            }
        }

        private void btnModule_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAM"))  // RMC 20110809
            {
                using (frmModule frmModule = new frmModule())
                {
                    frmModule.Source = "BPS";
                    frmModule.ShowDialog();
                }
            }
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUS"))   // RMC 20110809
            {
                using (frmConfig frmConfig = new frmConfig())
                {
                    frmConfig.ShowDialog();
                }
            }
        }

        private void btnDueDate_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUDD"))  // RMC 20110809
            {
                //using (frmDueDate frmDueDate = new frmDueDate())
                using (frmDueDateNew frmDueDate = new frmDueDateNew())   // RMC 20170725 configured penalty computation, merged from Binan
                {
                    frmDueDate.ShowDialog();
                }
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS"))  // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "ASS"))
                {
                    using (frmSchedule frmSchedule = new frmSchedule())
                    {
                        frmSchedule.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "ASS"))
                        {
                        }
                    }
                }
            }
        }

        private void btnOtherCharges_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS"))  // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmScheduleOthers frmScheduleOthers = new frmScheduleOthers())
                    {
                        frmScheduleOthers.moduletype = "Others";
                        frmScheduleOthers.ShowDialog();

                        if (TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }
            }
        }

        private void btnBrgy_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUB"))   // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("BARANGAY", "BARANGAY", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmBarangay frmBarangay = new frmBarangay())
                    {
                        frmBarangay.ShowDialog();

                        if (TaskMan.IsObjectLock("BARANGAY", "BARANGAY", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABD"))  // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "BUSINESS RECORD - DELETE";
                    frmBussRec.FormState(1);
                    frmBussRec.m_sFormStatus = "BUSS-DELETE";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnDefTools_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUDT"))  // RMC 20110809
            {
                using (frmDeficientRecordsTool frmDefTool = new frmDeficientRecordsTool())
                {
                    frmDefTool.ShowDialog();
                }
            }
        }

        private void btnUpdateRec_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABUR"))  // RMC 20110809
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "BUSINESS RECORD - UPDATE";
                    frmBussRec.FormState(1);
                    frmBussRec.m_sFormStatus = "BUSS-UPDATE";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABCU")) // RMC 20110809
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "BUSINESS RECORD - CANCEL UPDATE";
                    frmBussRec.FormState(1);
                    frmBussRec.m_sFormStatus = "BUSS-CANCEL-UPDATE";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnBIN_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSRBR"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Replace owner of BIN";
                    frmSNR.iWindowState = 1;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSROR"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Replace owner codes";
                    frmSNR.iWindowState = 2;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnEditOwner_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABSROE"))    // RMC 20110809
            {
                using (frmSearchAndReplace frmSNR = new frmSearchAndReplace())
                {
                    frmSNR.Text = "Edit owner's Information";
                    frmSNR.iWindowState = 0;
                    frmSNR.bControlState = true;
                    frmSNR.m_sTrailDetail = "S&R";
                    frmSNR.ShowDialog();
                }
            }
        }

        private void btnAuditTrail_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAT"))    //JHB 20181114 add access grants
            {

                using (frmAuditTrail AuditTrailFrm = new frmAuditTrail())
                {
                    AuditTrailFrm.ShowDialog();
                }
            }
        }

        private void btnAppEditNew_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABE"))    // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Newly Applied Business";
                    frmBussRec.m_sFormStatus = "NEW-APP-EDIT";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        btnAppEditNew_Click(sender, e);
                }
            }
        }

        private void btnAppViewNew_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABV"))    // rmc 20110808
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "NEW-APP-VIEW";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnAppRenEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABE"))    // rmc 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Renewal Business";
                    frmBussRec.FormState(0);
                    frmBussRec.m_sFormStatus = "REN-APP-EDIT";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        btnAppRenEdit_Click(sender, e);
                }
            }
        }

        private void tbnAppRenView_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABV"))    // rmc 20110808
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "REN-APP-VIEW";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnAppCancelApp_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AACA"))  // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.ObjectPass(sender, e);
                    frmBussRec.Text = "Cancel Application";
                    //frmBussRec.FormState(0);
                    frmBussRec.FormState(1);    // RMC 20110809
                    frmBussRec.m_sFormStatus = "CANCEL-APP";
                    frmBussRec.ShowDialog(this);
                }
            }
        }

        private void btnAppRet_Click(object sender, EventArgs e)
        {
            using (frmRetirement frmRetirement = new frmRetirement())
            {
                frmRetirement.ShowDialog();
            }
        }

        private void btnPermitUpdate_Click(object sender, EventArgs e)
        {
            using (frmPermitUpdate frmPermitUpdate = new frmPermitUpdate())
            {
                frmPermitUpdate.ShowDialog();
            }
        }

        private void btnBnsPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABMP"))   // RMC 20110808
            {
                using (frmBusinessPermit frmBusinessPermit = new frmBusinessPermit())
                {
                    frmBusinessPermit.ShowDialog();
                }
            }
        }

        private void tsmDefBns_Click(object sender, EventArgs e)
        {
            using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
            {
                frmInspectorDetails.Source = "Deficient";
                frmInspectorDetails.ShowDialog();
            }
        }

        private void tsmUnofBns_Click(object sender, EventArgs e)
        {
            using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
            {
                frmInspectorDetails.Source = "Unofficial";
                frmInspectorDetails.ShowDialog();
            }
        }

        private void btnInspector_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUI"))   // RMC 20110809
            {
                using (frmInspector frmInspector = new frmInspector())
                {
                    frmInspector.ShowDialog();
                }
            }
        }

        private void btnViolationTable_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUV"))   // RMC 20110809
            {
                using (frmViolationTable frmViolation = new frmViolationTable())
                {
                    frmViolation.ShowDialog();
                }
            }
        }

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTTM")) // RMC 20110809
            {
                using (frmTaskManager frmTaskMan = new frmTaskManager())
                {
                    frmTaskMan.Source = "ASS";  // RMC 20110805
                    frmTaskMan.ShowDialog();
                }
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS"))  // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("SCHEDULE SET DEFAULT", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmDefaultValuesSet frmDefaultValuesSet = new frmDefaultValuesSet())
                    {
                        frmDefaultValuesSet.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE SET DEFAULT", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }
            }
        }

        private void btnPrintScheds_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTPS"))  // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmPrintSchedule frmPrintSchedule = new frmPrintSchedule())
                    {
                        frmPrintSchedule.ShowDialog();

                        if (TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }

                /*if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20110815
                {
                }*/
            }
        }

        private void btnExemptBuss_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS"))  // RMC 20110809
            {
                if (!TaskMan.IsObjectLock("EXEMPTION SCHEDULE", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmExemptedBuss frmExemptedBuss = new frmExemptedBuss())
                    {
                        frmExemptedBuss.ShowDialog();

                        if (TaskMan.IsObjectLock("EXEMPTION SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }
            }
        }

        private void btnPer_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AA-PUT"))    // RMC 20110809
            {
                using (frmPermitUpdate frmPermitUpdate = new frmPermitUpdate())
                {
                    frmPermitUpdate.ShowDialog();
                }
            }
        }

        private void btnRetApp_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AAR"))   // RMC 20110809
            {
                using (frmRetirement frmRetirement = new frmRetirement())
                {
                    frmRetirement.ShowDialog();
                }
            }
        }

        private void btnInspecDetails_Click(object sender, EventArgs e)
        {

        }

        private void btnDeficientBuss_Click(object sender, EventArgs e)
        {

            // RMC 20110725 Created separate Menu for Inspector's module
            if (AppSettingsManager.Granted("ABID"))  // RMC 20110809
            {
                using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
                {
                    frmInspectorDetails.Source = "Deficient";
                    frmInspectorDetails.ShowDialog();
                }
            }
        }

        private void btnUnofficialBuss_Click(object sender, EventArgs e)
        {
            // RMC 20110725 Created separate Menu for Inspector's module
            if (AppSettingsManager.Granted("ABUB"))  // RMC 20110809
            {
                using (frmInspectorDetails frmInspectorDetails = new frmInspectorDetails())
                {
                    frmInspectorDetails.Source = "Unofficial";
                    frmInspectorDetails.ShowDialog();
                }
            }
        }

        private void flmFileMenu_Initialized(object sender, EventArgs e)
        {
            //MessageBox.Show("Initialize");
        }

        private void flmFileMenu_TabIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("test");
        }

        private void ValidateDueDateSetup()
        {
            // RMC 20110825 added validation if due date setup exists
            OracleResultSet result = new OracleResultSet();

            result.Query = "select * from due_dates where due_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();

                }
                else
                {
                    result.Close();

                    //MessageBox.Show("Please set due dates at Due Date Set-up module.", "BPS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // RMC 20111228 Added auto-add of due dates upon change of tax year (s)
                    string sDueDate = "";
                    string sMonth = "";
                    string sMonthCode = "";
                    string sCurrentDate = Convert.ToString(AppSettingsManager.GetCurrentDate());

                    sDueDate = "01/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "JANUARY";
                    sMonthCode = "01";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "02/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "FEBRUARY";
                    sMonthCode = "02";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "03/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "MARCH";
                    sMonthCode = "03";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "04/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "APRIL";
                    sMonthCode = "04";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "05/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "MAY";
                    sMonthCode = "05";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "06/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "JUNE";
                    sMonthCode = "06";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "07/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "JULY";
                    sMonthCode = "07";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "08/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "AUGUST";
                    sMonthCode = "08";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "09/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "SEPTEMBER";
                    sMonthCode = "09";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "10/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "OCTOBER";
                    sMonthCode = "10";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "11/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "NOVEMBER";
                    sMonthCode = "11";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    sDueDate = "12/20/" + ConfigurationAttributes.CurrentYear;
                    sMonth = "DECEMBER";
                    sMonthCode = "12";

                    result.Query = "insert into due_dates (DUE_CODE, DUE_DATE, DUE_DESC, DUE_YEAR, DATE_SAVE, USER_CODE)";
                    result.Query += " values ('" + sMonthCode + "', to_date('" + sDueDate + "', 'MM/dd/yyyy'), '" + sMonth + "', ";
                    result.Query += " '" + ConfigurationAttributes.CurrentYear + "', to_date('" + sCurrentDate + "', 'MM/dd/yyyy hh:mi:ss am'), ";
                    result.Query += " '" + AppSettingsManager.SystemUser.UserCode + "')";
                    if (result.ExecuteNonQuery() == 0)
                    { }

                    // RMC 20111228 Added auto-add of due dates upon change of tax year (e)

                }
            }
            result.Close();
        }

        private void btnOwnQuery_Click(object sender, EventArgs e)
        {
            // RMC 20110831
            if (AppSettingsManager.Granted("ABSROQ"))
            {
                using (frmOwnerQuery frmOwnerQuery = new frmOwnerQuery())
                {
                    frmOwnerQuery.ShowDialog();
                }
            }

        }

        private void btnDeleteOwner_Click(object sender, EventArgs e)
        {
            // RMC 20110901
            if (AppSettingsManager.Granted("ABSROQ"))
            {
                using (frmDeleteOwner frmDeleteOwner = new frmDeleteOwner())
                {
                    frmDeleteOwner.ShowDialog();
                }
            }
        }

        private void btnSpecialOrd_Click(object sender, EventArgs e)
        {
            using (frmTagging frmTagging = new frmTagging())
            {
                frmTagging.Switch = "";
                frmTagging.ShowDialog();
            }
        }

        private void btnReqmt_Click(object sender, EventArgs e)
        {
            // RMC 20110902 Created project ApplicationRequirements 
            if (!TaskMan.IsObjectLock("APP REQMT", "APP REQMT", "ADD", "ASS"))
            {
                if (AppSettingsManager.Granted("AUAR"))
                {
                    using (frmAppRequirement frmAppRequirement = new frmAppRequirement())
                    {
                        frmAppRequirement.ShowDialog();
                    }
                }
            }
        }

        private void btnMapping_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmMapping frmMapping = new frmMapping())
                {
                    frmMapping.SourceClass = "Official";
                    frmMapping.Text = "Business Mapping - Official Business";
                    frmMapping.ShowDialog();
                }
            }
        }

        private void btnPayHist_Click(object sender, EventArgs e)
        {
            // RMC 20111006 Merged viewing of Payment hist in BPS from BTAS
            //if (AppSettingsManager.Granted("CCPH"))
            if (AppSettingsManager.Granted("ARPH")) //AFM 20200107 new module code for bps payment hist
            {
                frmPaymentHistory frmPayHist = new frmPaymentHistory();
                frmPayHist.ShowDialog();
            }
        }

        private void btnEncoderReport_Click(object sender, EventArgs e)
        {
            using (frmEncoderReport EncoderReport = new frmEncoderReport())
            {
                EncoderReport.ShowDialog();
            }
        }

        private void btnUndeclared_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmMapping frmMapping = new frmMapping())
                {
                    frmMapping.SourceClass = "UnOfficial";
                    frmMapping.Text = "Business Mapping - Unofficial Business";
                    frmMapping.ShowDialog();
                }
            }
        }

        private void btnAccomplishementForm_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmForm frmAccomplishmentFrm = new frmForm())
                {
                    frmAccomplishmentFrm.ShowDialog();
                }
            }
        }

        private void btnMappingDeficient_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmSummaryReport frmSummaryRepFrm = new frmSummaryReport())
                {
                    frmSummaryRepFrm.ShowDialog();
                }
            }
        }

        private void btnMappingUnofficial_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmSummaryReportsUnofficial frmSummaryRepFrmUnoff = new frmSummaryReportsUnofficial())
                {
                    frmSummaryRepFrmUnoff.Source = "Unofficial";
                    frmSummaryRepFrmUnoff.ShowDialog();
                }
            }
        }

        private void btnNotMapped_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmSummaryReportsUnofficial frmSummaryRepFrmUnoff = new frmSummaryReportsUnofficial())
                {
                    frmSummaryRepFrmUnoff.Source = "Not-Mapped";
                    frmSummaryRepFrmUnoff.ShowDialog();
                }
            }
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARN"))
            {
                using (frmNotices frmNotice = new frmNotices())
                {
                    frmNotice.ShowDialog();
                }
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmReportTest frmReport = new frmReportTest())
                {
                    frmReport.ReportName = "Summary";
                    frmReport.ReportSwitch = "";
                    frmReport.ShowDialog();
                }
            }
        }

        private void btnSummarySection_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmReportTest frmReport = new frmReportTest())
                {
                    frmReport.ReportName = "Summary";
                    frmReport.ReportSwitch = "Section";
                    frmReport.ShowDialog();
                }
            }
        }

        private void btnAppForm_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABA") || AppSettingsManager.Granted("AARAPF"))   // RMC 20120102 added user granting for printing of applicaiton form
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "REN-APP-FORM";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void picNewBnsBig_Click(object sender, EventArgs e)
        {
            btnAppNew_Click(sender, e);
        }

        private void picRenBnsBig_Click(object sender, EventArgs e)
        {
            tbnAppRen_Click(sender, e);
        }

        private void btnORF_Click(object sender, EventArgs e)
        {

        }

        private void btnCertification_Click(object sender, EventArgs e)
        {
            /*using (frmCertification CertificationFrm = new frmCertification())
            {
                CertificationFrm.ShowDialog();
            }*/

            using (frmCertifications CertificationsFrm = new frmCertifications()) //JHMN 20170103 options for different certificates
            {
                CertificationsFrm.ShowDialog();
            }
        }

        private void btnBMCleansingTool_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABM"))
            {
                using (frmCleansingTool CleansingToolFrm = new frmCleansingTool())
                {
                    CleansingToolFrm.ShowDialog();
                }
            }
        }

        private void btnHoldRecord_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTHR"))
            {
                using (frmHoldRecord HoldRecordFrm = new frmHoldRecord())
                {
                    HoldRecordFrm.ShowDialog();
                }
            }
        }

        private void btnDataQuery_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUDQM"))
            {
                using (frmDataQuery DataQueryFrm = new frmDataQuery())
                {
                    DataQueryFrm.ShowDialog();
                }
            }
        }

        private void btnCDO_Click(object sender, EventArgs e)
        {
            frmCDOReport fCDO = new frmCDOReport();
            fCDO.ShowDialog();
        }

        private void btnBussOnQue_Click(object sender, EventArgs e)
        {

        }

        private void btnMappedWithPermit_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITH PERMIT";
            fBrgy.ShowDialog();
        }

        private void btnMappedWOPermit_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITHOUT PERMIT";
            fBrgy.ShowDialog();
        }

        private void btnWithPermit_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITH PERMIT";
            fBrgy.ShowDialog();
        }

        private void btnWithoutPermit_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITHOUT PERMIT";
            fBrgy.ShowDialog();
        }

        private void btnWithPermit1_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITH PERMIT AND NOTICE";
            fBrgy.ShowDialog();
        }

        private void btnWithoutPermit2_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITHOUT PERMIT AND NOTICE";
            fBrgy.ShowDialog();
        }

        private void btnReassessment_Click(object sender, EventArgs e)
        {

        }

        private void btnRetirement_Click(object sender, EventArgs e)
        {
            frmRetCert fRetCert = new frmRetCert();
            fRetCert.ShowDialog();
        }

        private void kryptonRibbonGroupButton1_Click(object sender, EventArgs e)
        {
            frmSpecial fSpecial = new frmSpecial();
            fSpecial.ShowDialog();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            frmBrgyFilter fBrgy = new frmBrgyFilter();
            fBrgy.m_sReportName = "MAPPED WITHOUT PERMIT2";
            fBrgy.ShowDialog();
        }

        private void btnBSPReport_Click(object sender, EventArgs e)
        {
            frmBussReport fBussReport = new frmBussReport();
            fBussReport.ReportSwitch = "BSP Report";
            fBussReport.ShowDialog();
        }

        private void btnSpecialPermits_Click(object sender, EventArgs e)
        {

        }

        private void btnAddSplPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABA"))    // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Apply Special Business";
                    frmBussRec.m_sFormStatus = "SPL-APP";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnSpecialPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AASP"))    //JHB 20181114 add access grants
            {
                // RMC 20171124 customized special permit printing where payment was made at aRCS (s)
                if (AppSettingsManager.GetConfigValue("10") == "216")
                {
                    frmArcsSplPermit fArcsPermit = new frmArcsSplPermit();
                    fArcsPermit.ShowDialog();
                }
                else
                {
                    // RMC 20171124 customized special permit printing where payment was made at aRCS (e)

                    frmSpecialPermit fSpecial = new frmSpecialPermit();
                    fSpecial.ShowDialog();
                }
            }
        }

        private void btnEditSplPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABE"))    // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Applied Special Business";
                    frmBussRec.m_sFormStatus = "SPL-APP-EDIT";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnViewSplPermit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABV"))    // rmc 20110808
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "SPL-APP-VIEW";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnEmpGender_Click(object sender, EventArgs e)
        {
            // RMC 20140105 added report with employee gender (dilg report -requested by mati)
            using (frmEmpGender form = new frmEmpGender())
            {
                form.ShowDialog();
            }
        }

        private void btnGenerateReasses_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("APRA"))    //JHB 20181114 add access grants
            {
                frmReAssessment fReAss = new frmReAssessment();
                fReAss.ShowDialog();
            }
        }

        private void btnBnsOnQueue_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABRIQ"))    //JHB 20181114 add access grants
            {
                frmBusinessOnQue fBnsQue = new frmBusinessOnQue();
                fBnsQue.ShowDialog();
            }
        }

        private void btnSummaryofBusinesses_Click(object sender, EventArgs e)
        {
            //MCR 03032014
            if (AppSettingsManager.Granted("ARL"))    //JHB 20181114 add access grants
            {

                frmManagementReport frmManagementReport = new frmManagementReport();
                frmManagementReport.ShowDialog();
            }
        }

        private void btnSummaryRec_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARSB"))    //JHB 20181114 add access grants
            {
                frmBussSummary frmBussSummary = new frmBussSummary();
                {
                     frmBussSummary.Switch = "BasedOnRecords";
                     frmBussSummary.ShowDialog();
                }
            }
        }

        private void btnSummaryYear_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARA"))    //JHB 20181114 add access grants
            {

                frmBussSummary frmBussSummary = new frmBussSummary();
                {
                    frmBussSummary.Switch = "BasedOnYearRegistration";
                    frmBussSummary.ShowDialog();
                }
            }
        }

        private void btnAddBnsRecord_Click(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }

        private void btnAppNewBns_Click(object sender, EventArgs e)
        {
            btnAppNew_Click(sender, e);
        }

        private void btnAppRenBns_Click(object sender, EventArgs e)
        {
            tbnAppRen_Click(sender, e);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            using (frmLogIn frmLogInOut = new frmLogIn())
            {
                // RMC 20111228 added deletion in user trail where user logs-out (s)
                OracleResultSet pCmd = new OracleResultSet();

                pCmd.Query = "delete from a_trail where mod_code = 'ALI' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
                // RMC 20111228 added deletion in user trail where user logs-out (e)

                frmLogInOut.ShowDialog();
                m_sUser = frmLogInOut.m_sUserCode;

                // RMC 20110808
                if (m_sUser == "")
                    this.Close();
                // RMC 20110808

                string sUsrDiv = string.Empty;  // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)

                if (m_sUser.Length > 0)
                {
                    OracleResultSet pSet = new OracleResultSet();
                    //pSet.Query = "SELECT USR_LN, USR_FN, USR_MI, USR_POS FROM SYS_USERS WHERE TRIM(USR_CODE) = :1";
                    pSet.Query = "SELECT USR_LN, USR_FN, USR_MI, USR_POS, usr_div FROM SYS_USERS WHERE TRIM(USR_CODE) = :1";    // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)
                    pSet.AddParameter(":1", m_sUser.Trim());
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            lblUser.Text = " ";
                            lblCode.Text = " ";
                            string sLn, sFn, sMi, sPos, sUser;
                            sLn = pSet.GetString("usr_ln").Trim();
                            sFn = pSet.GetString("usr_fn").Trim();
                            sMi = pSet.GetString("usr_mi").Trim();
                            sPos = pSet.GetString("usr_pos").Trim();
                            sUsrDiv = pSet.GetString("usr_div").Trim();  // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan)

                            if (sMi.Length > 0)
                                sMi = sMi + ". ";
                            else
                                sMi = " ";

                            sUser = sFn + " " + sMi + sLn;
                            lblUser.Text = "USER: " + sUser;
                            lblCode.Text = "USERCODE: " + m_sUser;

                            // RMC 20110809 added trailing of system login (s)
                            if (AuditTrail.InsertTrail("AULI", "trail_table/a_trail", sUser) == 0)
                            {
                                return;
                            }
                            if (AuditTrail.InsertTrail("ALI", "a_trail", sUser) == 0)
                            {
                                return;
                            }
                            // RMC 20110809 added trailing of system login (e)
                        }
                    }

                    AppSettingsManager.GetSystemType = "A"; // RMC 20151021 changed task manager module code
                }
                else
                {
                    lblUser.Text = "";
                    lblCode.Text = "";
                }

                // RMC 20170322 added module Eng'g Tool for business, this is LGU-Specific and not configurable (Binan) (s)
                if (sUsrDiv == "ENGINEERING" || sUsrDiv == "ZONING" || sUsrDiv == "SANITARY" ||
                    sUsrDiv == "BFP" || sUsrDiv == "BENRO" || sUsrDiv == "CENRO" || sUsrDiv == "CHO" || sUsrDiv == "MAPUMA" || sUsrDiv == "HEALTH" || sUsrDiv == "MARKET")
                {
                    tabRecords.Visible = false;
                    tabApplication.Visible = false;
                    tabBilling.Visible = false;
                    tabInspectorMod.Visible = false;
                    tabReports.Visible = false;
                    tabTables.Visible = false;
                    tabUtilities.Visible = false;
                    if (sUsrDiv == "ENGINEERING" || sUsrDiv == "BFP" || sUsrDiv == "CHO" || sUsrDiv == "PESO" || sUsrDiv == "ZONING" || sUsrDiv == "SANITARY" || sUsrDiv == "MAPUMA") //AFM 20200102
                    {
                        tabEPS.Visible = true;
                        tabZoning.Visible = true; //AFM 20191219
                        tabSanitary.Visible = true; //AFM 20200107
                        tabApplication.Visible = true; //AFM 2020010
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                        tabCenro.Visible = false;
                    }
                    else
                    {
                        tabEPS.Visible = false;
                        tabZoning.Visible = false; //AFM 20191219
                        tabSanitary.Visible = false; //AFM 20200107
                    }

                    if (sUsrDiv == "CENRO")
                    {
                        tabCenro.Visible = true;
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                    }
                    else if (sUsrDiv == "HEALTH")
                    {
                        tabCenro.Visible = false;
                        tabHealth.Visible = true;
                        tabMarket.Visible = false;
                    }
                    else if (sUsrDiv == "MARKET")
                    {
                        tabCenro.Visible = false;
                        tabHealth.Visible = false;
                        tabMarket.Visible = true;
                    }
                    else
                    {
                        tabHealth.Visible = false;
                        tabMarket.Visible = false;
                        tabCenro.Visible = false;
                    }

                    tabNegaList.Visible = true;
                }
                else
                {
                    tabRecords.Visible = true;
                    tabApplication.Visible = true;
                    tabBilling.Visible = true;
                    tabInspectorMod.Visible = true;
                    tabReports.Visible = true;
                    tabTables.Visible = true;
                    tabUtilities.Visible = true;
                    tabNegaList.Visible = true;
                    tabEPS.Visible = false;
                    tabZoning.Visible = false; //AFM 20191219
                    tabSanitary.Visible = false; //AFM 20200107
                }

                if (AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                {
                    tabEPS.Visible = true;
                    tabZoning.Visible = true; //AFM 20191219
                    tabSanitary.Visible = true; //AFM 20200107
                    tabMarket.Visible = true;
                    tabHealth.Visible = true;
                    tabCenro.Visible = true;
                }
            }
        }

        private void kbtnExit_Click(object sender, EventArgs e)
        {
            using (frmMsgBox msgbox = new frmMsgBox())
            {
                string sMsg = "Are you sure you want to exit?";
                msgbox.m_sMsg = "EXIT";
                msgbox.m_slbl = sMsg;
                msgbox.m_sModCode = "ALI";  // RMC 20111226 added system code in Common:Message_Box
                msgbox.ShowDialog();
            }
        }

        private void btnDataQuery_Click_1(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUDQM"))
            {
                using (frmDataQuery DataQueryFrm = new frmDataQuery())
                {
                    DataQueryFrm.ShowDialog();
                }
            }
        }

        private void btnBnsSummary_Click(object sender, EventArgs e)
        {
            //MCR 03192014
            frmSummary frmSummary = new frmSummary();
            frmSummary.ShowDialog();
        }

        private void btnPaperTrail_Click(object sender, EventArgs e)
        {
            frmPaperTrail frmPaperTrail = new frmPaperTrail();
            frmPaperTrail.ShowDialog();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARO"))    //JHB 20181114 add access grants
            {

                using (frmList frmList = new frmList())
                {
                    frmList.ShowDialog();
                }
            }
        }

        private void btnReportManager_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTRM"))    //JHB 20181114 add access grants
            {

                using (frmReportTools frmReportTools = new frmReportTools())
                {
                    frmReportTools.SystemName = "ASS";
                    frmReportTools.ShowDialog();
                }
            }
        }

        private void btnReportFiles_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTRF"))    //JHB 20181114 add access grants
            {
                using (frmReportFiles frmReportFiles = new frmReportFiles())
                {
                    frmReportFiles.SystemName = "ASS";
                    frmReportFiles.ShowDialog();
                }
            }
        }

        private void btnCDO2_Click(object sender, EventArgs e)
        {
            // RMC 20140724 transferred Cease and Desist menu
            frmCDOReport fCDO = new frmCDOReport();
            fCDO.ShowDialog();
        }

        private void kryptonRibbonGroup23_DialogBoxLauncherClick(object sender, EventArgs e)
        {

        }

        private void btnBill_Click(object sender, EventArgs e)
        {   // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIB"))
            {
                string sBIN = string.Empty;
                if (AppSettingsManager.GetConfigValue("78") == "Y") //AFM 20211213 MAO-21-16149	OTHER OFFICES
                {
                    using (frmSearchTmp form = new frmSearchTmp())
                    {
                        form.Office = "BPLO";
                        form.ShowDialog();
                        sBIN = form.BIN;

                        if (string.IsNullOrEmpty(sBIN))
                            return;
                    }
                }
                else
                {
                    using (frmBilling BillingForm = new frmBilling())
                    {
                        BillingForm.SourceClass = "Billing";
                        BillingForm.Text = "Billing";
                        if (!string.IsNullOrEmpty(sBIN))
                            BillingForm.BIN = sBIN;
                        BillingForm.ShowDialog();
                        BillingForm.Dispose();
                    }
                }
            }
        }

        private void btnRevExam_Click(object sender, EventArgs e)
        {
            // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIBRE-P"))
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "RevExam";
                    //BillingForm.Text = "Revene Examination / Adjustment";
                    BillingForm.Text = "Revenue Examination / Adjustment";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIBVS"))    // RMC 20111216 added validation of rights in soa-view
            {
                frmSOA fSOA = new frmSOA();
                fSOA.iFormState = 1;
                fSOA.ShowDialog();
            }
        }

        private void btnBillPermitUpdate_Click(object sender, EventArgs e)
        {
            // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIB-PUT"))  // RMC 20140909 Migration QA
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "PermitUpdate";
                    BillingForm.Text = "Permit Update";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private void btnBillRetirement_Click(object sender, EventArgs e)
        {
            // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIB")) // RMC 20140909 Migration QA
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "RetirementBilling";
                    BillingForm.Text = "Retirement Billing";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private void btnCancelBill_Click(object sender, EventArgs e)
        {
            // RMC 20150426 QA corrections
            if (AppSettingsManager.Granted("AIBCB"))
            {
                // RMC 20110807 created CancelBilling module
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "CancelBilling";
                    BillingForm.Text = "Cancel Billing";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private bool ValidateVersion()
        {
            OracleResultSet resultx = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();
            string sLatestVersion;
            int iVersion, iLatestVersion;

            int.TryParse(StringUtilities.Right(m_sVersion, 4), out iVersion);

            resultx.Query = "select * from sec_ver_valid where version_code like '%-BP-A-%'";
            if (resultx.Execute())
            {
                if (resultx.Read())
                {
                    sLatestVersion = resultx.GetString("version_code");
                    int.TryParse(StringUtilities.Right(m_sVersion, 4), out iLatestVersion);

                    if (iVersion > iLatestVersion)
                    {
                        pCmd.Query = "delete from sec_ver_valid";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        sLatestVersion = StringUtilities.Right(m_sVersion, 15) + StringUtilities.Right(m_sVersion, 4);
                        pCmd.Query = "insert into sec_ver_valid values (";
                        pCmd.Query += "'" + sLatestVersion + "')";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                    }
                }
                else
                {
                    pCmd.Query = "insert into sec_ver_valid values ('" + m_sVersion + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
            }
            resultx.Close();

            resultx.Query = "select * from sec_ver_valid where version_code = '" + m_sVersion + "'";
            if (resultx.Execute())
            {
                if (!resultx.Read())
                {
                    MessageBox.Show("Invalid version", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    resultx.Close();
                    return false;
                }
            }
            resultx.Close();

            return true;
        }

        private void picMainForm_Click(object sender, EventArgs e)
        {

        }

        private void btnExempBrgy_Click(object sender, EventArgs e)
        {
            // RMC 20150806 merged exemption by brgy from Mati 
            OracleResultSet pCmd = new OracleResultSet();

            try
            {
                pCmd.Query = "create table TAX_AND_FEES_EXEMPTED_BRGY(FEES_CODE VARCHAR2(4) not null, BRGY_CODE VARCHAR2(4) not null, REV_YEAR  VARCHAR2(4) not null)";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
            catch { }

            if (AppSettingsManager.Granted("AUTS"))  // MCR 20140929
            {
                if (!TaskMan.IsObjectLock("EXEMPTION BRGY SCHEDULE", "SCHEDULE", "ADD", "COL"))
                {
                    using (frmExemptionFeebyBrgy frmExemptionFeebyBrgy = new frmExemptionFeebyBrgy())
                    {
                        frmExemptionFeebyBrgy.ShowDialog();

                        if (TaskMan.IsObjectLock("EXEMPTION BRGY SCHEDULE", "SCHEDULE", "DELETE", "COL"))  // MCR 20140929
                        {
                        }
                    }
                }
            }
        }

        private void btnPreGross_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTPG"))    // RMC 20150811 QA Presumptive gross module
            {
                frmGross formGross = new frmGross();
                formGross.Show();
            }
        }

        private void btnRevYear_Click(object sender, EventArgs e)
        {
            // RMC 20160109 customized special ordinance module for Tumauini
            if (AppSettingsManager.Granted("AUTS"))
            {
                if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "ASS"))
                {
                    using (frmRevYear form = new frmRevYear())
                    {
                        form.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "ASS"))
                        {
                        }
                    }

                }
            }
        }

        private void btnSplOrd_Click(object sender, EventArgs e)
        {
            // RMC 20160109 customized special ordinance module for Tumauini
            if (AppSettingsManager.Granted("AUTS"))
            {
                if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "ASS"))
                {
                    using (frmSplOrd form = new frmSplOrd())
                    {
                        form.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "ASS"))
                        {
                        }
                    }

                }
            }
        }

        private void btnSanitary_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUSC"))
            {
                using (frmSanitary frmsanitary = new frmSanitary())
                {
                    frmsanitary.ShowDialog();
                }
            }
        }

        //AFM 20191217
        private void btnZoningClearance_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUZZC"))
            {
                using (frmClearance frmclearance = new frmClearance())
                {
                    frmclearance.ClearanceMode = "Zoning";
                    frmclearance.ShowDialog();
                }
            }
        }

        //AFM 20191217
        private void btnZoningListReport_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUZZC"))
            {
                using (frmEPSClearance eps = new frmEPSClearance())
                {
                    eps.ClearanceMode = "Zoning";
                    eps.ShowDialog();
                }
            }
        }

        //AFM 20191217
        private void btnSeriesConfig_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUZZC"))
            {
                using (frmZoningSeriesConfig zoning = new frmZoningSeriesConfig())
                {
                    zoning.ShowDialog();
                }
            }
        }

        //AFM 20191217
        private void btnSignatoryConfig_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUBS"))
            {
                using (frmPermitSignatory sig = new frmPermitSignatory())
                {
                    sig.ShowDialog();
                }
            }
        }

        private void btnAppNewDelq_Click(object sender, EventArgs e)
        {
            // RMC 20161109 merged discovery-delinq
            m_bIsNewDiscovery = true;
            btnAppNew_Click(sender, e);
        }

        private void btnBlankForm_Click(object sender, EventArgs e)
        {
            //if (AppSettingsManager.Granted("APBF"))    //JHB 20181114 add access grants
            {

                // RMC 20161219 add printing of blank application form
                if (MessageBox.Show("Print Blank application form?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    frmAppForm PrintAppForm = new frmAppForm();
                    PrintAppForm.PrintBlank = true;
                    PrintAppForm.ShowDialog();
                }
            }
        }

        private void btnRevRetirement_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AAR"))
            {
                using (frmRetirementReversal RetReversal = new frmRetirementReversal())
                {
                    RetReversal.ShowDialog();
                }
            }
        }

        private void btnPermitMonitoring_Click(object sender, EventArgs e)
        {
            // RMC 20170202 customized permit monitoring
            if (AppSettingsManager.Granted("ABMP"))
            {
                using (frmPermitMonitoring form = new frmPermitMonitoring())
                {
                    form.ShowDialog();
                }
            }
        }

        private void btnCertStatus_Click(object sender, EventArgs e)
        {
            using (frmBinSearch frmBinSearch = new frmBinSearch())
            {
                frmBinSearch.ModuleCode = "CERT-STAT";
                frmBinSearch.ShowDialog();
            }
        }

        private void btnChecklist_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUEC"))
            {
                using (frmRequirement form = new frmRequirement())
                {
                    form.ShowDialog();
                }
            }
        }

        private void btnAssessment_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUEA"))
            {
                using (frmEPS frmeps = new frmEPS())
                {
                    frmeps.ShowDialog();
                }

            }
        }

        private void btnWaiveSurchPen_Click(object sender, EventArgs e)
        {
            //if (AppSettingsManager.Granted("WSPB"))
            using (frmPenaltyTagging frmpenaltytagging = new frmPenaltyTagging())
                frmpenaltytagging.ShowDialog();
        }

        private void kryptonRibbonGroupButton5_Click(object sender, EventArgs e) //JARS 20170907
        {
            if (AppSettingsManager.Granted("ABMP-A")) //JARS 20170911 ADDED
            {
                using (frmAdjPermit frmAdj = new frmAdjPermit())
                    frmAdj.ShowDialog();
            }
        }

        private void btnInspectionReport_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABIR")) //JARS 20170911
            {
                using (frmInspectionReport frmIns = new frmInspectionReport())
                    frmIns.ShowDialog();
            }
        }

        private void btnTransTrail_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARTT"))    //JHB 20181114 add access grants
            {

                using (frmTransactionTrail form = new frmTransactionTrail())
                {
                    form.ShowDialog();
                }
            }
        }

        private void kryptonRibbonGroupButton9_Click(object sender, EventArgs e)
        {

        }

        private void btnInspectionReport_Click_1(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABIR")) //JARS 20170911
            {
                using (frmInspectionReport frmIns = new frmInspectionReport())
                    frmIns.ShowDialog();
            }
        }

        private void btnViolationReport_Click(object sender, EventArgs e)
        {
            // RMC 20171115 Added history of untagged violations
            if (AppSettingsManager.Granted("ABIR"))
            {
                using (frmPrintOptions form = new frmPrintOptions())
                {
                    form.Source = "VIOLATION REPORT";
                    form.ShowDialog();
                }
            }
        }

        private void btnComparative_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARC"))
            {
                // RMC 20180126 added comparative report as requested by Malolos BPLO Head

                frmBussReport fBussReport = new frmBussReport();
                frmDatePeriod form = new frmDatePeriod();
                form.Datefrom = AppSettingsManager.GetCurrentDate().ToShortDateString();
                form.Dateto = AppSettingsManager.GetCurrentDate().ToShortDateString();
                form.ShowDialog();

                fBussReport.m_dFrom = DateTime.Parse(form.Datefrom);
                fBussReport.m_dTo = DateTime.Parse(form.Dateto);
                fBussReport.ReportSwitch = "Comparative Report";
                fBussReport.ShowDialog();
            }
        }

        private void btn_pfd_Click(object sender, EventArgs e) // JHB 20182603 help button for BPS Manual
        {
            if (AppSettingsManager.Granted("AUTH"))    //JHB 20181114 add access grants
            {
                string filename = "MalolosBPS.pdf";
                System.Diagnostics.Process.Start(@"C:\BPLS-GUI\BPS\MalolosBPS.pdf");
            }
        }

        private void btnVioList_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABNL"))
            {
                frmVioList frmviolist = new frmVioList();
                frmviolist.ShowDialog();
            }
        }

        private void btnNigList_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABNL"))
            {
                frmNigList frmniglist = new frmNigList();
                frmniglist.ShowDialog();
            }
        }

        private void btnEngChecklist_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUEC"))
            {
                using (frmRequirement form = new frmRequirement())
                {
                    form.ShowDialog();
                }
            }
        }

        private void btnEngAssessment_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUEA"))
            {
                /*using (frmEPS frmeps = new frmEPS())
                {
                    frmeps.ShowDialog();
                }*/

                using (frmSearchTmp form = new frmSearchTmp())
                {
                    form.Office = "ENGINEERING";
                    form.ShowDialog();
                }
            }
        }

        private void btnEngCertification_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUECR"))
            {
                using (frmClearance frmclearance = new frmClearance())
                {
                    frmclearance.ClearanceMode = "Engineering"; //JARS 20190130
                    frmclearance.ShowDialog();
                }
            }
        }

        private void btnVioReport_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABNL"))
            {
                frmNigReport frmnigreport = new frmNigReport();
                frmnigreport.ShowDialog();
            }
        }

        private void btnBrgyCharges_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS"))  // RMC 20110809
            {
                // AFM 20191226 removed temporarily for buildup
                //if (!TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                //{
                    using (frmScheduleOthers frmScheduleOthers = new frmScheduleOthers())
                    {
                        frmScheduleOthers.moduletype = "Brgy";
                        frmScheduleOthers.ShowDialog();

                        if (TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                //}
            }
        }

        private void btnEngReport_Click(object sender, EventArgs e) //AFM 20200121
        {
            if (AppSettingsManager.Granted("ARER"))
            {
                frmDatePeriod frmdateperiod = new frmDatePeriod();
                frmdateperiod.ReportModule = "EngineeringReport";
                frmdateperiod.ShowDialog();
            }
        }

        private void btnSanitaryReport_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARSR"))
            {
                frmDatePeriod frmdateperiod = new frmDatePeriod();
                frmdateperiod.ReportModule = "SanitaryReport";
                frmdateperiod.ShowDialog();
            }
        }

        private void btnComparativeListDate_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ARC"))
            {
                frmBussReport fBussReport = new frmBussReport();
                frmDatePeriod form = new frmDatePeriod();
                form.Datefrom = AppSettingsManager.GetCurrentDate().ToShortDateString();
                form.Dateto = AppSettingsManager.GetCurrentDate().ToShortDateString();
                form.ShowDialog();
                if (form.IsOK == false)
                    return;

                fBussReport.m_dFrom = DateTime.Parse(form.Datefrom);
                fBussReport.m_dTo = DateTime.Parse(form.Dateto);
                fBussReport.ReportSwitch = "Comparative Report List By Date";
                fBussReport.ShowDialog();
            }
        }

        private void btnSOAMonitoring_Click(object sender, EventArgs e) //AFM 20210624 new module requested by MALOLOS
        {
            if (AppSettingsManager.Granted("AISM"))
            {
                frmSOAMonitoring form = new frmSOAMonitoring();
                form.ShowDialog();
            }
        }

        private void btnEsignature_Click(object sender, EventArgs e)
        {
            frmSignatories form = new frmSignatories();
            form.ShowDialog();
        }

        private void btnBrgyClearance_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABBC"))
            {
                frmCertificateAndPermit frmCertificateAndPermit = new frmCertificateAndPermit();
                frmCertificateAndPermit.CertificatePermitType = "Barangay Clearance";
                frmCertificateAndPermit.ShowDialog();

            }
        }

        private void kryptonRibbonGroupButton10_Click(object sender, EventArgs e)
        {

        }

        private void btnZoningApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABZC"))
            {
                using (frmSearchTmp form = new frmSearchTmp())
                {
                    form.Office = "PLANNING";
                    form.ShowDialog();
                }
            }
        }

        private void btnZoningModify_Click(object sender, EventArgs e)
        {
            using (frmSearchTmp form = new frmSearchTmp())
            {
                form.Office = "PLANNING";
                form.Switch = "LISTAPPROVE";
                form.ShowDialog();
            }
        }

        private void btnZoningListApp_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ListApproved";
            listform.Office = "PLANNING";
            listform.ShowDialog();
        }

        private void btnHealthApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABHC"))
            {
                using (frmSearchTmp form = new frmSearchTmp())
                {
                    form.Office = "HEALTH";
                    form.ShowDialog();
                }
            }
        }

        private void btnHealthListApp_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ListApproved";
            listform.Office = "HEALTH";
            listform.ShowDialog();
        }

        private void btnHealthModify_Click(object sender, EventArgs e)
        {
            using (frmSearchTmp form = new frmSearchTmp())
            {
                form.Office = "HEALTH";
                form.Switch = "LISTAPPROVE";
                form.ShowDialog();
            }
        }

        private void btnCenroApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABCENRO"))
            {
                using (frmSearchTmp form = new frmSearchTmp())
                {
                    form.Office = "CENRO";
                    form.ShowDialog();
                }
            }
        }

        private void btnCenroListApp_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ListApproved";
            listform.Office = "CENRO";
            listform.ShowDialog();
        }

        private void btnCenroModify_Click(object sender, EventArgs e)
        {
            using (frmSearchTmp form = new frmSearchTmp())
            {
                form.Office = "CENRO";
                form.Switch = "LISTAPPROVE";
                form.ShowDialog();
            }
        }

        private void btnMarketApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABMARKET"))
            {
                using (frmSearchTmp form = new frmSearchTmp())
                {
                    form.Office = "MARKET";
                    form.ShowDialog();
                }
            }
        }

        private void btnMarketListApp_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ListApproved";
            listform.Office = "MARKET";
            listform.ShowDialog();
        }

        private void btnMarketModify_Click(object sender, EventArgs e)
        {
            using (frmSearchTmp form = new frmSearchTmp())
            {
                form.Office = "MARKET";
                form.Switch = "LISTAPPROVE";
                form.ShowDialog();
            }
        }

        private void btnEngListApp_Click(object sender, EventArgs e)
        {
            frmPrinting listform = new frmPrinting();
            listform.ReportType = "ListApproved";
            listform.Office = "ENGINEERING";
            listform.ShowDialog();
        }

        private void btnEngModify_Click(object sender, EventArgs e)
        {
            using (frmSearchTmp form = new frmSearchTmp())
            {
                form.Office = "ENGINEERING";
                form.Switch = "LISTAPPROVE";
                form.ShowDialog();
            }
        }

        private void btnAppTrail_Click(object sender, EventArgs e)
        {
            frmBinSearch form = new frmBinSearch();
            form.ModuleCode = "APP-TRAIL";
            form.ShowDialog();
        }

        private void btnPendingApp_Click(object sender, EventArgs e)
        {
            using (frmPendingApproval PendingApproval = new frmPendingApproval())
            {
                PendingApproval.ShowDialog();
            }
        }

        private void btnPaidBnsApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABBA"))
            {
                frmBusinessApproval form = new frmBusinessApproval();
                form.Module = "BUSINESS APPROVAL";
                form.ShowDialog();
            }
        }

        private void btnMayorApproval_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABMA"))
            {
                frmBusinessApproval form = new frmBusinessApproval();
                form.Module = "MAYOR APPROVAL";
                form.ShowDialog();
            }
        }

    }
}