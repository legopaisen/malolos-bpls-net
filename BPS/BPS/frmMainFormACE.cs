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
using BusinessRoll;
using Amellar.Modules.HealthPermit;

namespace BPLSBilling
{
    public partial class frmMainFormACE : Form
    {
        private DateTime m_dtValidDate;
        public string m_sUser;


        public frmMainFormACE()
        {
            InitializeComponent();
        }

        private void frmMainFormACE_Load(object sender, EventArgs e)
        {
            bool blnIsConnectionOpen = false;

            blnIsConnectionOpen = DataConnectorManager.Instance.OpenConnection();
            if (!blnIsConnectionOpen)
            {
                MessageBox.Show("error connection");
                Dispose();
            }

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
                    pSet.Query = "SELECT USR_LN, USR_FN, USR_MI, USR_POS FROM SYS_USERS WHERE TRIM(USR_CODE) = :1";
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

                        }

                    }

                    ValidateDueDateSetup();
                }


                OracleResultSet result = new OracleResultSet();
                OracleResultSet result1 = new OracleResultSet();



                m_dtValidDate = AppSettingsManager.GetSystemDate(); // RMC 20110725 modified getting current date

                lblDate.Text = string.Format("Today is {0:MMMM dd, yyyy}", m_dtValidDate);
                //lblDate.Text += "\nVERSION: 19200-00005";
                // RMC 20110725 modified getting current date (s)
                if (AppSettingsManager.bFreezedDate)
                    lblDate.Text += "; DATE FREEZED";

                // RMC 20141202 in connection to merged trailing (s)
                // this can be remarked if na-run na onsite
                try
                {
                    int iCnt = 0;
                    result.Query = "select count(*) from trail_table where usr_rights = 'AAAPUT-NB'";
                    int.TryParse(result.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        result.Close();
                        result.Query = "insert into trail_table values ('AAAPUT-NB','ASS-APPLICATION-ADD-PERMIT UPDATE TRANSACTION NO BILLING')";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                    }

                    iCnt = 0;
                    result.Query = "select count(*) from trail_table where usr_rights = 'AAE-NBP'";
                    int.TryParse(result.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        result.Close();
                        result.Query = "insert into trail_table values ('AAE-NBP','ASS-APPLICATION-PARTIAL RET-WITHOUT BILLING')";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                    }

                    iCnt = 0;
                    result.Query = "select count(*) from trail_table where usr_rights = 'AAE-WBP'";
                    int.TryParse(result.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        result.Close();
                        result.Query = "insert into trail_table values ('AAE-WBP','ASS-APPLICATION-PARTIAL RET-WITH BILLING')";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                    }

                    iCnt = 0;
                    result.Query = "select count(*) from trail_table where usr_rights = 'AAEPUT-NB'";
                    int.TryParse(result.ExecuteScalar(), out iCnt);
                    if (iCnt == 0)
                    {
                        result.Close();
                        result.Query = "insert into trail_table values ('AAEPUT-NB','ASS-APPLICATION-EDIT-PERMIT UPDATE TRANSACTION NO BILLING')";
                        if (result.ExecuteNonQuery() == 0)
                        { }
                    }
                }
                catch { }
                // RMC 20141202 in connection to merged trailing (e)

            }
            catch (NullReferenceException)
            {
                blnIsConnectionOpen = false;
            }

            AppSettingsManager.GetSystemType = "A";
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("ABA"))   // RMC 20110808
            {
                // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (s)
                bool bContinue = true;
                OracleResultSet pSet = new OracleResultSet();
                int intCnt = 0;
                string strInsNo = string.Empty;

                /*pSet.Query = "select count(*) from btm_temp_businesses where trim(old_bin) is not null and trim(bin) is null";
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
                else*/
                //bContinue = true; // RMC 20150114 put rem
                // RMC 20111128 consider Business mapping - unencoded in Business Records-Add (e)

                // RMC 20150114 (s)
                using (frmSearchTmp frmList = new frmSearchTmp())
                {
                    strInsNo = "";
                    bContinue = true;
                    frmList.TaxYear = AppSettingsManager.GetConfigValue("12");
                    frmList.Text = "Select New Business Owner Record";
                    frmList.Permit = "Application"; // RMC 20150102 mods in permit
                    frmList.ShowDialog();
                    strInsNo = frmList.BIN;
                    if (strInsNo != "")
                    {
                        if (MessageBox.Show("Continue adding Temporary BIN: " + strInsNo + "?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            strInsNo = "";
                    }

                    //if (strInsNo != "")
                    //    bContinue = true;
                    //else
                    //    bContinue = false;
                }
                // RMC 20150114 (e)

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

        private void btnFees_Click(object sender, EventArgs e)
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
                        frmScheduleOthers.ShowDialog();

                        if (TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }
            }
        }

        private void btnExempt_Click(object sender, EventArgs e)
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

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUS"))   // RMC 20110809
            {
                using (frmConfig frmConfig = new frmConfig())
                {
                    frmConfig.ShowDialog();
                }
            }
        }

        private void btnTrail_Click(object sender, EventArgs e)
        {
            using (frmAuditTrail AuditTrailFrm = new frmAuditTrail())
            {
                AuditTrailFrm.ShowDialog();
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            frmBussList frmBussList = new frmBussList();
            frmBussList.ShowDialog();
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            frmBusinessRoll frmBusinessRoll = new frmBusinessRoll();
            frmBusinessRoll.BusinessQueue = false;
            frmBusinessRoll.ShowDialog();
        }

        private void btnRetirement_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AAR"))   // RMC 20110809
            {
                using (frmRetirement frmRetirement = new frmRetirement())
                {
                    frmRetirement.ShowDialog();
                }
            }
        }

        private void btnDILG_Click(object sender, EventArgs e)
        {
            using (frmList frmList = new frmList())
            {
                frmList.ShowDialog();
            }
        }

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTTM"))
            {
                using (frmTaskManager frmTaskMan = new frmTaskManager())
                {
                    frmTaskMan.Source = "ASS";
                    frmTaskMan.ShowDialog();
                }
            }
        }

        private void btnReportManager_Click(object sender, EventArgs e)
        {
            using (frmReportTools frmReportTools = new frmReportTools())
            {
                frmReportTools.SystemName = "ASS";
                frmReportTools.ShowDialog();
            }
        }

        private void btnAppCancel_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AACA"))  // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Cancel Application";
                    //frmBussRec.FormState(0);
                    frmBussRec.FormState(1);    // RMC 20110809
                    frmBussRec.m_sFormStatus = "CANCEL-APP";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnAppRET_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AAR"))   // RMC 20110809
            {
                using (frmRetirement frmRetirement = new frmRetirement())
                {
                    frmRetirement.ShowDialog();
                }
            }
        }

        private void btnAppView_Click(object sender, EventArgs e)
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

        private void btnAppEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABE"))    // RMC 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Newly Applied Business";
                    frmBussRec.m_sFormStatus = "NEW-APP-EDIT";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        btnAppEdit_Click(sender, e);
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

                /*pSet.Query = "select count(*) from unofficial_info_tbl where trim(bin_settled) is null ";
                int.TryParse(pSet.ExecuteScalar(), out intCntInsp);

                pSet.Query = "select count(*) from btm_temp_businesses where trim(old_bin) is null ";
                int.TryParse(pSet.ExecuteScalar(), out intCntMapped);

                intCnt = intCntInsp + intCntMapped;

                if (intCnt > 0)
                {
                    using (frmUnOfficialList frmList = new frmUnOfficialList())
                    {
                        frmList.Source = "INSPECTION";
                        frmList.ShowDialog();
                        bContinue = frmList.Continue;  // RMC 20111128
                        strInsNo = frmList.InspectionNumber;
                    }
                }
                else*/
                

                // RMC 20141222 modified permit printing (s)
                pSet.Query = "select count(*) from zoning where trim(bin) is null ";
                int.TryParse(pSet.ExecuteScalar(), out intCnt);

                //if (intCnt > 0)   // RMC 20141228 modified permit printing (lubao), put rem, force new application to go to zoning first
                {
                    using (frmSearchTmp frmList = new frmSearchTmp())
                    {
                        frmList.TaxYear = AppSettingsManager.GetConfigValue("12");
                        frmList.Text = "Select New Business Owner Record";
                        frmList.Permit = "Application"; // RMC 20150102 mods in permit
                        frmList.ShowDialog();
                        strInsNo = frmList.BIN;
                        if (strInsNo != "")
                        {
                            if (MessageBox.Show("Continue applying Temporary BIN: " + strInsNo + "?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                strInsNo = "";
                        }

                        if (strInsNo != "")
                            bContinue = true;
                        else
                            bContinue = false;
                    }
                }
                /*else
                    bContinue = true;  // RMC 20111128*/    // RMC 20141228 modified permit printing (lubao), put rem, force new application to go to zoning first
                // RMC 20141222 modified permit printing (e)
                
                // RMC 20110816 (e)

                if (bContinue)  // RMC 20111128
                {
                    using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                    {
                        frmBussRec.Text = "Apply New Business";
                        frmBussRec.m_sFormStatus = "NEW-APP";
                        // RMC 20110816 (s)
                        if (strInsNo != "")
                            frmBussRec.m_strInspectionNo = strInsNo;
                        else
                            frmBussRec.m_strInspectionNo = "";
                        // RMC 20110816 (e)
                        frmBussRec.ShowDialog();
                        if (frmBussRec.m_bAddNew)   // RMC 20111128
                            btnAppNew_Click(sender, e);
                    }
                }
            }
        }

        private void btnUnrenewed_Click(object sender, EventArgs e)
        {
            frmBusinessRoll frmBusinessRoll = new frmBusinessRoll();
            frmBusinessRoll.Unrenewed = true;
            frmBusinessRoll.ShowDialog();
        }

        private void btnListofRetirement_Click(object sender, EventArgs e)
        {
            frmRetirementReport frmRetirementReport = new frmRetirementReport();
            frmRetirementReport.ShowDialog();
        }

        private void BtnOwner_Click(object sender, EventArgs e)
        {
            frmACEBuildUpTools frmACEBuildUpTools = new frmACEBuildUpTools();
            frmACEBuildUpTools.ShowDialog();
        }

        private void btnPermitUpdate_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AA-PUT"))    // RMC 20110809
            {
                using (frmPermitUpdate frmPermitUpdate = new frmPermitUpdate())
                {
                    frmPermitUpdate.ShowDialog();
                }
            }
        }

        private void btnHealthPermit_Click(object sender, EventArgs e)
        {
            frmHealthPermit frmHealthPermit = new frmHealthPermit();
            frmHealthPermit.ShowDialog();
        }

        private void btnPermits_Click(object sender, EventArgs e)
        {
            frmACEPermits frmACEPermits = new frmACEPermits();
            frmACEPermits.ShowDialog();
        }

        private void btnSpecialPermitAdd_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABA"))
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Apply Special Business";
                    frmBussRec.m_sFormStatus = "SPL-APP";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnSpecialPermitEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABE"))
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Applied Special Business";
                    frmBussRec.m_sFormStatus = "SPL-APP-EDIT";
                    frmBussRec.ShowDialog();
                }
            }
        }

        private void btnSpecialPermitView_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABV"))
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "SPL-APP-VIEW";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnRenewalEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABE"))
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Edit Renewal Business";
                    frmBussRec.FormState(0);
                    frmBussRec.m_sFormStatus = "REN-APP-EDIT";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)
                        btnRenewalEdit_Click(sender, e);
                }
            }
        }

        private void btnRenewalView_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABV"))
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "REN-APP-VIEW";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnPrintForm_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABA") || AppSettingsManager.Granted("AARAPF"))
            {
                using (frmBinSearch BinSearchFrm = new frmBinSearch())
                {
                    BinSearchFrm.ModuleCode = "REN-APP-FORM";
                    BinSearchFrm.ShowDialog();
                }
            }
        }

        private void btnInspectorsModule_Click(object sender, EventArgs e)
        {
            frmACEInspectorModule frmACEInspectorModule = new frmACEInspectorModule();
            frmACEInspectorModule.ShowDialog();
        }

        private void btnRenewalAdd_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AARABA"))    // rmc 20110808
            {
                using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                {
                    frmBussRec.Text = "Apply Renewal Business";
                    frmBussRec.FormState(0);
                    frmBussRec.m_sFormStatus = "REN-APP";
                    frmBussRec.ShowDialog();
                    if (frmBussRec.m_bAddNew)   // RMC 20111128
                        btnRenewalAdd_Click(sender, e);
                }
            }
        }

        private void btnAppReq_Click(object sender, EventArgs e)
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

        private void btnDiscoveryDelq_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AANABA"))
            {
                bool bContinue = true;
                OracleResultSet pSet = new OracleResultSet();
                int intCnt = 0;
                int intCntInsp = 0;
                int intCntMapped = 0;
                string strInsNo = string.Empty;

                bContinue = true;
                using (frmSearchTmp frmList = new frmSearchTmp())
                {
                    frmList.TaxYear = AppSettingsManager.GetConfigValue("12");
                    frmList.Text = "Select New Business Owner Record";
                    frmList.Permit = "Application"; // RMC 20150102 mods in permit
                    frmList.ShowDialog();
                    strInsNo = frmList.BIN;
                    if (strInsNo != "")
                    {
                        if (MessageBox.Show("Continue applying Temporary BIN: " + strInsNo + "?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            strInsNo = "";
                    }

                    if (strInsNo != "")
                        bContinue = true;
                    else
                        bContinue = false;
                }

                if (bContinue)
                {
                    using (frmBusinessRecord frmBussRec = new frmBusinessRecord())
                    {
                        frmBussRec.Text = "Apply New Business";
                        frmBussRec.m_sFormStatus = "NEW-APP";
                        if (strInsNo != "")
                            frmBussRec.m_strInspectionNo = strInsNo;
                        else
                            frmBussRec.m_strInspectionNo = "";
                        frmBussRec.NewDiscovery = true;
                        frmBussRec.ShowDialog();
                        if (frmBussRec.m_bAddNew)
                            btnAppNew_Click(sender, e);
                    }
                }
            }
        }

        private void frmMainFormACE_FormClosed(object sender, FormClosedEventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            OracleResultSet pCmd = new OracleResultSet();
            string sTmp = string.Empty;

            try
            {
                sTmp = AppSettingsManager.SystemUser.UserCode;
            }
            catch
            {
                sTmp = "";
            }

            if (sTmp != "")
            {
                pCmd.Query = "delete from a_trail where mod_code = 'ALI' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
        }

        private void frmMainFormACE_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnViolation_Click(object sender, EventArgs e)
        {

            
        }
    }
}