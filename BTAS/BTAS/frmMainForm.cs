
// RMC 20111228 added deletion in user trail where user logs-out 
// RMC 20111227 added Gross monitoring module for gross >= 200000
// RMC 20111226 added system code in Common:Message_Box
// RMC 20111226 added logout button in Main Frame of BTAS
// RMC 20111226 added exit button, deleted control box in Main Frame of BTAS
// RMC 20111226 added display if date freezed in BTAS
// RMC 20111226 added trailing of user login in BTAS
// RMC 20111226 merged validation of connection before login from BPS to BTAS 
// RMC 20111216 added validation of rights in soa-view
// MCR 20140106 added LiquidationReports
// MCR 20140217 added Payment
// MCR 20140224 added Delinquency
// MCR 20140227 added Collectibles
// MCR 20140321 added DebitCredit
// MCR 20140602 added ReportTools
// MCR 20140626 added CompromiseAgreement

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.LogIn;
using Amellar.BPLS.Billing;
using Amellar.Common.SOA;
using Amellar.Modules.PaymentHistory;
using Posting;
//using Amellar.Modules.Payment;
using ComponentFactory.Krypton.Ribbon;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.AppSettings;
using Amellar.Common.BPLSApp;
using Amellar.Common.BusinessType;
using Amellar.Common.DataConnector;
using TellerSetup;
using ReturnDeclareOR;
//using Bank;
using Amellar.Common.User;  // RMC 20110725
using Amellar.Common.ModuleRights;  // RMC 20110725
using Amellar.Modules.Utilities;    // RMC 20110725
using Amellar.Common.TaskManager;   // RMC 20110805
using Amellar.Common.AuditTrail;    // RMC 20110805
using Amellar.Common.Reports;
using Amellar.RPTA.Classes.Bank;
using Amellar.BPLS.TreasurersModule;    // RMC 20111221 added Btax Monitoring module
using Amellar.Common.Message_Box;   // RMC 20111226 added exit button, deleted control box in Main Frame of BTAS
using NewBankModule;
using Amellar.Modules.LiquidationReports; //MCR 20140106
using Amellar.Modules.Payment; //MCR 20140217
using Amellar.Modules.Delinquency; //MCR 20140224
using Amellar.Modules.Collectibles; //MCR 20140227
using Amellar.Modules.DebitCreditTransaction; //MCR 20140321
using Amellar.Common.Tools;
using Amellar.Common.ReportTools; // MCR 20140602 
using Amellar.Modules.CompromiseAgreement; //MCR 20140626
using Amellar.Modules.BadChecks;
using Amellar.Common.StringUtilities;
using Amellar.Modules.BusinessReports;  // JHMN 20170127
using TreasurersModule;
using System.Reflection;

namespace BTAS
{
    public partial class frmBTAS : Form
    {
        private string m_sVersion;
        frmSOA fSOA;
        private Image[] _images = new Image[] { global::BTAS.Properties.Resources.payment_records_add };

        private string[] _names = new string[] { "Online" };

        public string m_sUser;  // RMC 20111226 added trailing of user login in BTAS
        private DateTime m_dtValidDate; // RMC 20111226 added display if date freezed in BTAS

        public frmBTAS()
        {
            InitializeComponent();
            CenterToScreen();   // RMC 20111226 added exit button, deleted control box in Main Frame of BTAS
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

        private void frmBTAS_Load(object sender, EventArgs e)
        {
            //JARS 20181127 THIS SHOWS THE DATE THE SYSTEM WAS COMPILED, 
            //AND WILL NOT CHANGE UNTIL THERE ARE CHANGES MADE TO THE SYSTEM (IN ANY FORM)
            this.Text = "Business Permit System | Version: " + AssemblyCreationDate.Value.ToString();


            //MessageBox.Show("You are using test database!");

            // RMC 20111226 merged validation of connection before login from BPS to BTAS (s)
            bool blnIsConnectionOpen = false;
            blnIsConnectionOpen = DataConnectorManager.Instance.OpenConnection();
            if (!blnIsConnectionOpen)
            {
                MessageBox.Show("error connection");
                Dispose();
                return; //MCR ADD 03032014
            }
            // RMC 20111226 merged validation of connection before login from BPS to BTAS (e)

            // RMC 20150603 Created/modified version validation (s)
            m_sVersion = "2015-AME-BP-C-0001";

            //if (!ValidateVersion()) //JARS 20170710 COMMENT OUT
                //return;
            // RMC 20150603 Created/modified version validation (e)

            using (frmLogIn frmLogInOut = new frmLogIn())
            {
                frmLogInOut.ShowDialog();

                // RMC 20111226 added trailing of user login in BTAS (s)
                m_sUser = frmLogInOut.m_sUserCode;
                if (m_sUser == "")
                    this.Close();
                try //MCR 20140903 Error Immediately Close after Run
                {
                    if (m_sUser.Length > 0)
                    {
                        lblUser.Text = "USER: " + AppSettingsManager.GetUserName(m_sUser);
                        lblCode.Text = "USERCODE: " + m_sUser;


                        if (AuditTrail.InsertTrail("CULI", "trail_table/a_trail", AppSettingsManager.GetUserName(m_sUser)) == 0)
                        {
                            return;
                        }
                        if (AuditTrail.InsertTrail("CLI", "a_trail", AppSettingsManager.GetUserName(m_sUser)) == 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        lblUser.Text = "";
                        lblCode.Text = "";
                    }
                    // RMC 20111226 added trailing of user login in BTAS (e)
                }
                catch { }
            }

            // RMC 20111226 added display if date freezed in BTAS (s)
            m_dtValidDate = AppSettingsManager.GetSystemDate();

            lblDate.Text = string.Format("Today is {0:MMMM dd, yyyy}", m_dtValidDate);
            //lblDate.Text += "\nVERSION: 19200-00005"; // RMC 20140909 Migration QA, PUT REM
            if (AppSettingsManager.bFreezedDate)
                lblDate.Text += "; DATE FREEZED";

            // RMC 20111226 added display if date freezed in BTAS (e)

            AppSettingsManager.GetSystemType = "C"; // RMC 20150226 adjustment in blob configuration

        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AIB"))  // RMC 20150811 added checking of module rights in main form
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "Billing";
                    BillingForm.Text = "Billing";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private void btnRevEam_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AIBRE-P"))  // RMC 20150811 added checking of module rights in main form
            {
                using (frmBilling BillingForm = new frmBilling())
                {
                    BillingForm.SourceClass = "RevExam";
                    BillingForm.Text = "Revene Examination / Adjustment";
                    BillingForm.ShowDialog();
                    BillingForm.Dispose();
                }
            }
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AIBVS"))    // RMC 20111216 added validation of rights in soa-view
            {
                fSOA = new frmSOA();
                fSOA.iFormState = 1;
                fSOA.ShowDialog();
            }
        }

        private void btnPayHist_Click(object sender, EventArgs e)
        {
            frmPaymentHistory frmPayHist = new frmPaymentHistory();
            frmPayHist.ShowDialog();
        }

        private void btnPostAdd_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRPA")) // RMC 20150811 added checking of module rights in main form
            {
                frmPosting frmPost = new frmPosting();
                frmPost.m_sPostFormState = "ADD";
                frmPost.ShowDialog();
            }
        }

        private void btnPostEdit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRPE")) // RMC 20150811 added checking of module rights in main form
            {
                frmPosting frmPost = new frmPosting();
                frmPost.m_sPostFormState = "EDIT";
                frmPost.ShowDialog();
            }
        }

        private void btnPostDelete_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRPD")) // RMC 20150811 added checking of module rights in main form
            {
                frmPosting frmPost = new frmPosting();
                frmPost.m_sPostFormState = "DELETE";
                frmPost.ShowDialog();
            }
        }

        private void btnOnline_Click(object sender, EventArgs e)
        {
            try
            {
                if (AppSettingsManager.g_objSystemUser.UserCode != string.Empty)
                {
                    frmLogIn fLog = new frmLogIn();
                    fLog.sFormState = "ONL";
                    fLog.Text = "Tellers Log-In";
                    fLog.ShowDialog();
                    //if (fLog.objTeller.UserCode != string.Empty) 
                    if (fLog.m_sUserCode != string.Empty) //MCR 20140711
                    {
                        OracleResultSet result = new OracleResultSet();
                        result.Query = "select * from or_current where teller = '" + fLog.objTeller.UserCode.Trim() + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                fLog.Close();
                                //frmPayments fPayment = new frmPayment();
                                frmPayment fPayment = new frmPayment();
                                fPayment.m_sPaymentMode = "ONL"; // GDE 20111124
                                fPayment.ShowDialog();
                            }
                            else
                            {
                                //MessageBox.Show("No OR has been declared for this user");
                                MessageBox.Show("No OR no. declared for this teller"); // RMC 20150501 QA BTAS, synchronized with ARCS
                                return;
                            }
                        }
                        result.Close();
                    }
                }
                else
                {
                    frmLogIn fLog = new frmLogIn();
                    fLog.sFormState = "ONL";
                    fLog.Text = "Tellers Log-In";
                    fLog.ShowDialog();
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Please log-in to the system");
                return;
            }

        }

        private void btnOffline_Click(object sender, EventArgs e)
        {
            try
            {
                if (AppSettingsManager.g_objSystemUser.UserCode != string.Empty)
                {
                    frmLogIn fLog = new frmLogIn();
                    fLog.sFormState = "OFL";
                    fLog.Text = "Tellers Log-In";
                    fLog.ShowDialog();
                    //if (fLog.objTeller.UserCode != string.Empty) 
                    if (fLog.m_sUserCode != string.Empty) //MCR 20140711
                    {
                        OracleResultSet result = new OracleResultSet();
                        result.Query = "select * from or_current where teller = '" + fLog.objTeller.UserCode.Trim() + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                fLog.Close();
                                //frmPayments fPayment = new frmPayment();
                                frmPayment fPayment = new frmPayment();
                                fPayment.m_sPaymentMode = "OFL"; // GDE 20111124
                                fPayment.ShowDialog();
                            }
                            else
                            {
                                //MessageBox.Show("No OR has been declared for this user");
                                MessageBox.Show("No OR no. declared for this teller");   // RMC 20150501 QA BTAS, synchronized with ARCS
                                return;
                            }
                        }
                        result.Close();
                    }
                }
                else
                {
                    frmLogIn fLog = new frmLogIn();
                    fLog.sFormState = "OFL";
                    fLog.Text = "Tellers Log-In";
                    fLog.ShowDialog();
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Payment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("Please log-in to the system");
                return;
            }
        }

        private void menuOnline_Opening(object sender, CancelEventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cntPosting_Click(object sender, EventArgs e)
        {
            frmPosting fPosting = new frmPosting();
            fPosting.ShowDialog();
        }
        private void cntOnline_Click(object sender, EventArgs e)
        {
            frmPayment fPayment = new frmPayment();
            fPayment.ShowDialog();
        }
        private void cntSoa_Click(object sender, EventArgs e)
        {
            frmSOA fSoa = new frmSOA();
            fSoa.iFormState = 1;
            fSoa.ShowDialog();
        }

        private void btnCancePayment_Click(object sender, EventArgs e)
        {
            frmCancelPayment fCancel = new frmCancelPayment();
            fCancel.ShowDialog();
        }

        private void btnTeller_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CUT"))  // RMC 20150811 added checking of module rights in main form
            {
                frmTellerSetup fTellerSetup = new frmTellerSetup();
                fTellerSetup.ShowDialog();
            }
        }

        private void btnUtilOR_Click(object sender, EventArgs e)
        {
            frmReturnDeclareOR frmRetDec = new frmReturnDeclareOR();
            frmRetDec.ShowDialog();
        }

        private void btnSoa_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CTSOA"))    // RMC 20150811 added checking of module rights in main form
            {
                fSOA = new frmSOA();
                fSOA.iFormState = 1;
                fSOA.ShowDialog();
            }
        }

        private void btnPaymentHist_Click(object sender, EventArgs e)
        {
            frmPaymentHistory frmPayHist = new frmPaymentHistory();
            frmPayHist.ShowDialog();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            frmBank fBank = new frmBank();
            fBank.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAU")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110725 Added User, Module Rights & Utilities project in BTAS
                using (frmUser frmUser = new frmUser())
                {
                    frmUser.Source = "BTAS";
                    frmUser.ShowDialog();
                }
            }
        }

        private void btnModule_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAM"))
            {
                // RMC 20110725 Added User, Module Rights & Utilities project in BTAS
                using (frmModule frmModule = new frmModule())
                {
                    frmModule.Source = "BTAS";
                    frmModule.ShowDialog();
                }
            }
        }

        private void btnDueDate_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUDD")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110725 Added User, Module Rights & Utilities project in BTAS
                using (frmDueDate frmDueDate = new frmDueDate())
                {
                    frmDueDate.ShowDialog();
                }
            }
        }

        private void btnCancelBill_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AIBCB"))    // RMC 20150811 added checking of module rights in main form
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

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            //if (AppSettingsManager.Granted("AUTTM"))    // RMC 20150811 added checking of module rights in main form
            if (AppSettingsManager.Granted("CUTM")) // RMC 20151021 changed task manager module code
            {
                // RMC 20110805
                using (frmTaskManager frmTaskMan = new frmTaskManager())
                {
                    frmTaskMan.Source = "COL";
                    frmTaskMan.ShowDialog();
                }
            }
        }

        private void btnAuditTrail_Click(object sender, EventArgs e)
        {
            // RMC 20110805
            using (frmAuditTrail AuditTrailFrm = new frmAuditTrail())
            {
                AuditTrailFrm.ShowDialog();
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110805
                if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "COL"))
                {
                    using (frmSchedule frmSchedule = new frmSchedule())
                    {
                        frmSchedule.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "COL"))  // RMC 20150811 added checking of module rights in main form
                        {
                        }
                    }

                }
            }
        }

        private void btnOtherCharges_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110805
                //if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "COL"))
                if (!TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "ADD", "COL"))      // RMC 20150811 added checking of module rights in main form
                {
                    using (frmScheduleOthers frmScheduleOthers = new frmScheduleOthers())
                    {
                        frmScheduleOthers.ShowDialog();

                        if (TaskMan.IsObjectLock("OTHER SCHEDULE", "SCHEDULE", "DELETE", "COL"))  // RMC 20150811 added checking of module rights in main form
                        {
                        }
                    }
                }
            }
        }

        private void btnExemptBuss_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110805
                //if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "COL"))
                if (!TaskMan.IsObjectLock("EXEMPTION SCHEDULE", "SCHEDULE", "ADD", "COL"))
                {
                    using (frmExemptedBuss frmExemptedBuss = new frmExemptedBuss())
                    {
                        frmExemptedBuss.ShowDialog();

                        if (TaskMan.IsObjectLock("EXEMPTION SCHEDULE", "SCHEDULE", "DELETE", "COL"))  // RMC 20150811 added checking of module rights in main form
                        {
                        }
                    }
                }
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTS")) // RMC 20150811 added checking of module rights in main form
            {
                // RMC 20110805
                //if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "ASS")) // GDE 20130221 change ASS to COL
                //if (!TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "ADD", "COL"))
                if (!TaskMan.IsObjectLock("SCHEDULE SET DEFAULT", "SCHEDULE", "ADD", "COL"))
                {
                    using (frmDefaultValuesSet frmDefaultValuesSet = new frmDefaultValuesSet())
                    {
                        frmDefaultValuesSet.ShowDialog();

                        if (TaskMan.IsObjectLock("SCHEDULE SET DEFAULT", "SCHEDULE", "DELETE", "COL")) // RMC 20150811 added checking of module rights in main form
                        {
                        }
                    }
                }
            }
        }

        private void btnPrintScheds_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUTPS"))    // RMC 20150811 added checking of module rights in main form
            {
                if (!TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "ADD", "COL"))  // RMC 20150811 added checking of module rights in main form
                {
                    using (frmPrintSchedule frmPrintSchedule = new frmPrintSchedule())
                    {
                        frmPrintSchedule.ShowDialog();

                        if (TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "DELETE", "COL"))  // RMC 20150811 added checking of module rights in main form
                        {
                        }
                    }
                }
            }

            /*ReportClass rClass = new ReportClass();
            rClass.Knootsky();
            */
            // RMC 20140909 Migration QA, put rem
        }

        private void btnPermitUpdate_Click(object sender, EventArgs e)
        {
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

        private void btnTaxMonitoring_Click(object sender, EventArgs e)
        {
            // RMC 20111221 added Btax Monitoring module
            if (AppSettingsManager.Granted("AUTM"))
            {
                using (frmBTaxMonitoring MonitoringForm = new frmBTaxMonitoring())
                {
                    MonitoringForm.SourceClass = "Tax";
                    MonitoringForm.ShowDialog();
                    MonitoringForm.Dispose();
                }
            }
        }

        private void picExitBig_Click(object sender, EventArgs e)
        {
            // RMC 20111226 added exit button, deleted control box in Main Frame of BTAS (s)
            using (frmMsgBox msgbox = new frmMsgBox())
            {
                string sMsg = "Are you sure you want to exit?";
                msgbox.m_sMsg = "EXIT";
                msgbox.m_slbl = sMsg;
                msgbox.m_sModCode = "CLI";  // RMC 20111226 added system code in Common:Message_Box
                msgbox.ShowDialog();


            }
            // RMC 20111226 added exit button, deleted control box in Main Frame of BTAS (e)
        }

        private void picLogOutBig_Click(object sender, EventArgs e)
        {
            // RMC 20111226 added logout button in Main Frame of BTAS (s)
            using (frmLogIn frmLogInOut = new frmLogIn())
            {
                // RMC 20111228 added deletion in user trail where user logs-out (s)
                OracleResultSet pCmd = new OracleResultSet();

                pCmd.Query = "delete from a_trail where mod_code = 'CLI' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
                // RMC 20111228 added deletion in user trail where user logs-out (e)

                frmLogInOut.ShowDialog();
                m_sUser = frmLogInOut.m_sUserCode;

                // RMC 20110808
                if (m_sUser == "")
                    this.Close();
                // RMC 20110808

                if (m_sUser.Length > 0)
                {
                    lblUser.Text = AppSettingsManager.GetUserName(m_sUser);
                    lblCode.Text = m_sUser;

                    if (AuditTrail.InsertTrail("CULI", "trail_table/a_trail", AppSettingsManager.GetUserName(m_sUser)) == 0)
                    {
                        return;
                    }
                    if (AuditTrail.InsertTrail("CLI", "a_trail", AppSettingsManager.GetUserName(m_sUser)) == 0)
                    {
                        return;
                    }
                    AppSettingsManager.GetSystemType = "C"; // RMC 20151021 changed task manager module code

                }
                else
                {
                    lblUser.Text = "";
                    lblCode.Text = "";
                }



            }
            // RMC 20111226 added logout button in Main Frame of BTAS (e)
        }

        private void picMainForm_MouseMove(object sender, MouseEventArgs e)
        {
            // RMC 20111226 added logout button in Main Frame of BTAS 
            lblDesc.Text = "AMELLAR SOLUTIONS: INFORMATION IS THE BUSINESS. TECHNOLOGY IS THE TOOL. PEOPLE MAKE THE SYSTEM WORK.";

        }

        private void picLogOut_MouseMove(object sender, MouseEventArgs e)
        {
            // RMC 20111226 added logout button in Main Frame of BTAS 
            lblDesc.Text = "Log-out your account";
        }

        private void picExit_MouseMove(object sender, MouseEventArgs e)
        {
            // RMC 20111226 added logout button in Main Frame of BTAS 
            lblDesc.Text = "Close the BPLS System";
        }

        private void btnGrossMonitoring_Click(object sender, EventArgs e)
        {
            // RMC 20111227 added Gross monitoring module for gross >= 200000
            OracleResultSet pSet = new OracleResultSet();

            string sSystemUserName = "";
            string sSystemUserCode = "";
            sSystemUserName = AppSettingsManager.SystemUser.UserName;
            sSystemUserCode = AppSettingsManager.SystemUser.UserCode;

            frmLogIn fLog = new frmLogIn();
            fLog.sFormState = "LOGIN";
            fLog.Text = "Approving Officer";
            fLog.ShowDialog();

            if (fLog.m_objSystemUser.UserCode != string.Empty)
            {
                pSet.Query = "select * from approver_tbl where usr_code = '" + fLog.m_objSystemUser.UserCode + "'";
                pSet.Query += " and usr_group = 'TREASURER'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        using (frmBTaxMonitoring MonitoringForm = new frmBTaxMonitoring())
                        {
                            MonitoringForm.SourceClass = "Gross";
                            MonitoringForm.ShowDialog();
                            MonitoringForm.Dispose();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Access denied");
                        return;
                    }
                }

                //load back system user code
                SystemUser m_objSystemUser = new SystemUser();
                if (m_objSystemUser.Load(sSystemUserCode))
                {
                    AppSettingsManager.g_objSystemUser = m_objSystemUser;
                }
            }
        }

        private void btnDelq_Click(object sender, EventArgs e)
        {
            frmDelqBrgy fDelqBrgy = new frmDelqBrgy();
            fDelqBrgy.ShowDialog();
        }

        private void btnTellerTransaction_Click(object sender, EventArgs e)
        {
            frmTellerTransaction frmTellerTransaction = new frmTellerTransaction();
            frmTellerTransaction.ReportSwitch = 5; //teller transaction
            frmTellerTransaction.ShowDialog();
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            frmSummaryofCollection frmSummaryofCollection = new frmSummaryofCollection();
            frmSummaryofCollection.ShowDialog();
        }

        private void btnExcessofCheck_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 2;
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void btnExcessofTaxCredit_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 4;
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void btnDelqUnrenewedBusiness_Click(object sender, EventArgs e)
        {
            frmUnrenewedBusiness frmUnrenewedBusiness = new frmUnrenewedBusiness();
            frmUnrenewedBusiness.ShowDialog();
        }

        private void btnDelqSummary_Click(object sender, EventArgs e)
        {
            frmSummaryDelinquency frmSummaryDelinquency = new frmSummaryDelinquency();
            frmSummaryDelinquency.ShowDialog();
        }

        private void btnDelqList_Click(object sender, EventArgs e)
        {
            frmListDelinquency frmListDelinquency = new frmListDelinquency();
            frmListDelinquency.ShowDialog();
        }

        private void btnListofCollectibles_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under Construction!","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //return;

            frmCollectibles frmCollectibles = new frmCollectibles();
            frmCollectibles.OptionFormat = 0; //0 = list 1 = Summary
            frmCollectibles.ShowDialog();
        }

        private void btnSummaryofCollectibles_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under Construction!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //return;

            frmCollectibles frmCollectibles = new frmCollectibles();
            frmCollectibles.OptionFormat = 1; //0 = list 1 = Summary
            frmCollectibles.ShowDialog();
        }

        private void btnLiquidation_Click(object sender, EventArgs e)
        {
            //MCR 03032014
            frmLiquidation frmLiquidation = new frmLiquidation();
            frmLiquidation.ShowDialog();
        }

        private void btnDebitCredit_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRDC")) // RMC 20150811 added checking of module rights in main form
            {
                frmDebitCreditTransaction frmDebitCreditTransaction = new frmDebitCreditTransaction();
                frmDebitCreditTransaction.ShowDialog();
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
        
        private void btnOthers_Click(object sender, EventArgs e)
        {
            using (frmOtherReports frmOtherReports = new frmOtherReports())
            {
                frmOtherReports.ShowDialog();
            }
        }

        private void btnActiveReport_Click(object sender, EventArgs e)
        {
            using (frmReportTools frmReportTools = new frmReportTools())
            {
                frmReportTools.SystemName = "COL";
                frmReportTools.ShowDialog();
            }
        }

        private void btnReportFiles_Click(object sender, EventArgs e)
        {
            using (frmReportFiles frmReportFiles = new frmReportFiles())
            {
                frmReportFiles.SystemName = "COL";
                frmReportFiles.ShowDialog();
            }
        }

        private void btnCompromise_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRPC")) // RMC 20150811 added checking of module rights in main form
            {
                using (frmCompromise frmCompromise = new frmCompromise())
                {
                    frmCompromise.ShowDialog();
                }
            }
        }

        private void btnRetirement_Click(object sender, EventArgs e)
        {
            //if (AppSettingsManager.Granted("AIB-MS"))
            if (AppSettingsManager.Granted("AIBB")) // RMC 20140909 Migration QA
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

        private void btnReprintOR_Click(object sender, EventArgs e)
        {
            frmReprintOR frmreprintor = new frmReprintOR();
            frmreprintor.ShowDialog();
        }

        private void btnBadCheck_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("CRPBC"))
            {
                frmBadCheck form = new frmBadCheck();
                form.ShowDialog();
            }
        }

        /// <summary>
        /// AST 20150416 Added this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUtilORInv_Click(object sender, EventArgs e)
        {
            frmORInventory FormORInventoryONL = new frmORInventory();
            FormORInventoryONL.ShowDialog();
        }

        private bool ValidateVersion()
        {
            OracleResultSet resultx = new OracleResultSet();
            OracleResultSet pCmd = new OracleResultSet();
            string sLatestVersion;
            int iVersion, iLatestVersion;

            int.TryParse(StringUtilities.Right(m_sVersion, 4), out iVersion);

            resultx.Query = "select * from sec_ver_valid where version_code like '%-BP-C-%'";
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

        private void kryptonRibbonGroup2_DialogBoxLauncherClick(object sender, EventArgs e)
        {

        }



        private void btnApproverTbl_Click(object sender, EventArgs e)
        {
            if (AppSettingsManager.Granted("AUAU")) // RMC 20150811 added checking of module rights in main form
            {
                frmApprover obj = new frmApprover();
                obj.Show();
            }
        }

        private void btnDeleteDBCR_Click(object sender, EventArgs e)
        {
            // RMC 20160302 merged tool to delete added dbcr memo from Mati (s)
            if (AppSettingsManager.Granted("CTDC"))
            {
                using (frmDeleteDebitCredit CreditForm = new frmDeleteDebitCredit())
                {
                    CreditForm.ShowDialog();
                    CreditForm.Dispose();
                }
            }
            // RMC 20160302 merged tool to delete added dbcr memo from Mati (e)
        }

        private void kryptonRibbonGroupButton2_Click(object sender, EventArgs e)
        {
            frmListofDeclaredOR frmList = new frmListofDeclaredOR();
            frmList.Show();
        }

        private void btnVerifyBusinesses_Click(object sender, EventArgs e)
        {
            // JHMN 20170127 verify businesses module (s)
            using (frmVerifyBusinesses frmVerifyBuss = new frmVerifyBusinesses())
            {
                frmVerifyBuss.ShowDialog();
            }
            // JHMN 20170127 verify businesses module (e)
        }

        private void btnWaiveSurchPen_Click(object sender, EventArgs e)
        {
            using (frmPenaltyTagging frmpenaltytagging = new frmPenaltyTagging())
                frmpenaltytagging.ShowDialog();
        }

        private void kryptonRibbonGroup13_DialogBoxLauncherClick(object sender, EventArgs e)
        {

        }

        private void btnNTRC_Click(object sender, EventArgs e) //JARS 20170907
        {
            using (frmNTRC NTRC = new frmNTRC())
                NTRC.ShowDialog();
        }

        private void btnPostApproval_Click(object sender, EventArgs e) //JARS 20170922
        {
            if (AppSettingsManager.Granted("CUPA"))
            {
                using (frmBTaxMonitoring MonitoringForm = new frmBTaxMonitoring())
                {
                    MonitoringForm.SourceClass = "PostPayment";
                    MonitoringForm.ShowDialog();
                    MonitoringForm.Dispose();
                }
            }
        }

        private void btnDBCR_Click(object sender, EventArgs e)
        {
            using (frmDBCRConverter DBCR = new frmDBCRConverter())
            {
                DBCR.ShowDialog();
                DBCR.Dispose();
            }
        }
    }
}