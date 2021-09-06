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
using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Ribbon;
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
using Amellar.Modules.RCD;
using BIN;

namespace BTAS
{
    public partial class frmMainFormACE : Form
    {
        public string m_sUser;
        private DateTime m_dtValidDate;

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
                return; //MCR ADD 20140303
            }
            // RMC 20111226 merged validation of connection before login from BPS to BTAS (e)

            using (frmLogIn frmLogInOut = new frmLogIn())
            {
                frmLogInOut.ShowDialog();


                m_sUser = frmLogInOut.m_sUserCode;
                if (m_sUser == "")
                    this.Close();
                try
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

                }
                catch { }
            }


            m_dtValidDate = AppSettingsManager.GetSystemDate();

            lblDate.Text = string.Format("Today is {0:MMMM dd, yyyy}", m_dtValidDate);
            if (AppSettingsManager.bFreezedDate)
                lblDate.Text += "; DATE FREEZED";

            AppSettingsManager.GetSystemType = "C"; //MCR 20141209
        }

        private void btnPostingAdd_Click(object sender, EventArgs e)
        {
            frmPosting frmPost = new frmPosting();
            frmPost.m_sPostFormState = "ADD";
            frmPost.ShowDialog();
        }

        private void btnPostingEdit_Click(object sender, EventArgs e)
        {
            frmPosting frmPost = new frmPosting();
            frmPost.m_sPostFormState = "EDIT";
            frmPost.ShowDialog();
        }

        private void btnPostingDelete_Click(object sender, EventArgs e)
        {
            frmPosting frmPost = new frmPosting();
            frmPost.m_sPostFormState = "DELETE";
            frmPost.ShowDialog();
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            using (frmBilling BillingForm = new frmBilling())
            {
                BillingForm.SourceClass = "Billing";
                BillingForm.Text = "Billing";
                BillingForm.ShowDialog();
                BillingForm.Dispose();
            }
        }

        private void btnSOA_Click(object sender, EventArgs e)
        {
            frmSOA fSOA;
            if (AppSettingsManager.Granted("AIBVS"))    // RMC 20111216 added validation of rights in soa-view
            {
                fSOA = new frmSOA();
                fSOA.iFormState = 1;
                fSOA.ShowDialog();
            }
        }

        private void btnPaymentOnline_Click(object sender, EventArgs e)
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
                                MessageBox.Show("No OR has been declared for this user");
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

        private void btnPaymentOffline_Click(object sender, EventArgs e)
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
                                MessageBox.Show("No OR has been declared for this user");
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
                MessageBox.Show("Please log-in to the system");
                return;
            }
        }

        private void btnPaymentCancel_Click(object sender, EventArgs e)
        {
            frmCancelPayment fCancel = new frmCancelPayment();
            fCancel.ShowDialog();
        }

        private void btnRCD_Click(object sender, EventArgs e)
        {
            frmRCD frmRCD = new frmRCD();
            frmRCD.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            using (frmUser frmUser = new frmUser())
            {
                frmUser.Source = "BTAS";
                frmUser.ShowDialog();
            }
        }

        private void btnTeller_Click(object sender, EventArgs e)
        {
            frmTellerSetup fTellerSetup = new frmTellerSetup();
            fTellerSetup.ShowDialog();
        }

        private void btnOR_Click(object sender, EventArgs e)
        {
            frmReturnDeclareOR frmRetDec = new frmReturnDeclareOR();
            frmRetDec.ShowDialog();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            frmBank fBank = new frmBank();
            fBank.ShowDialog();
        }

        private void btnDueDate_Click(object sender, EventArgs e)
        {
            using (frmDueDate frmDueDate = new frmDueDate())
            {
                frmDueDate.ShowDialog();
            }
        }

        private void btnTrail_Click(object sender, EventArgs e)
        {
            using (frmAuditTrail AuditTrailFrm = new frmAuditTrail())
            {
                AuditTrailFrm.ShowDialog();
            }
        }

        private void btnDILG_Click(object sender, EventArgs e)
        {
            using (frmOtherReports frmOtherReports = new frmOtherReports())
            {
                frmOtherReports.ShowDialog();
            }
        }

        private void btnSummCol_Click(object sender, EventArgs e)
        {
            frmSummaryofCollection frmSummaryofCollection = new frmSummaryofCollection();
            frmSummaryofCollection.ShowDialog();
        }

        private void btnDelinq_Click(object sender, EventArgs e)
        {
            frmSummaryDelinquency frmSummaryDelinquency = new frmSummaryDelinquency();
            frmSummaryDelinquency.ShowDialog();
        }

        private void btnCol_Click(object sender, EventArgs e)
        {

        }

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            // RMC 20110805
            using (frmTaskManager frmTaskMan = new frmTaskManager())
            {
                frmTaskMan.Source = "COL";
                frmTaskMan.ShowDialog();
            }
        }

        private void btnReportManager_Click(object sender, EventArgs e)
        {
            using (frmReportTools frmReportTools = new frmReportTools())
            {
                frmReportTools.SystemName = "COL";
                frmReportTools.ShowDialog();
            }
        }

        private void btnAbstract_Click(object sender, EventArgs e)
        {
            frmAbstractCollect frmAbstractCollect = new frmAbstractCollect();
            frmAbstractCollect.ReportSwitch = 1; // Abstract of Collection
            frmAbstractCollect.ShowDialog();
        }

        private void btnPayHist_Click(object sender, EventArgs e)
        {
            frmPaymentHistory frmPayHist = new frmPaymentHistory();
            frmPayHist.ShowDialog();
        }

        private void btnBillCancel_Click(object sender, EventArgs e)
        {
            using (frmBilling BillingForm = new frmBilling())
            {
                BillingForm.SourceClass = "CancelBilling";
                BillingForm.Text = "Cancel Billing";
                BillingForm.ShowDialog();
                BillingForm.Dispose();
            }
        }

        private void btnRevAdjExam_Click(object sender, EventArgs e)
        {
            using (frmBilling BillingForm = new frmBilling())
            {
                BillingForm.SourceClass = "RevExam";
                BillingForm.Text = "Revene Examination / Adjustment";
                BillingForm.ShowDialog();
                BillingForm.Dispose();
            }
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

        private void btnAbstractCheck_Click(object sender, EventArgs e)
        {
            frmAbstractChecks frmAbstractChecks = new frmAbstractChecks();
            frmAbstractChecks.ReportSwitch = 3; // Check
            frmAbstractChecks.ShowDialog();
        }

        private void btnAbstractPostedOR_Click(object sender, EventArgs e)
        {
            frmAbstractCollect frmAbstractCollect = new frmAbstractCollect();
            frmAbstractCollect.ReportSwitch = 2; // Abstract of Posted OR
            frmAbstractCollect.ShowDialog();
        }

        private void btnAbstractCancelOR_Click(object sender, EventArgs e)
        {
            frmAbstractOfCancelledOR frmAbstractOfCancelledOR = new frmAbstractOfCancelledOR();
            frmAbstractOfCancelledOR.AbstractReportFormat = 1;
            frmAbstractOfCancelledOR.ShowDialog();
        }

        private void btnDBCRReport_Click(object sender, EventArgs e)
        {
            frmDebitCredit frmDebitCredit = new frmDebitCredit();
            frmDebitCredit.ShowDialog();
        }

        private void btnReprintOR_Click(object sender, EventArgs e)
        {
            frmReprintOR frmreprintor = new frmReprintOR();
            frmreprintor.ShowDialog();
        }

        private void btnDBCRTag_Click(object sender, EventArgs e)
        {
            frmDebitCreditTransaction frmDebitCreditTransaction = new frmDebitCreditTransaction();
            frmDebitCreditTransaction.ShowDialog();
        }

        private void btnBillRetire_Click(object sender, EventArgs e)
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

        private void btnNotofDelq_Click(object sender, EventArgs e)
        {
            frmDelqBrgy fDelqBrgy = new frmDelqBrgy();
            fDelqBrgy.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmReprint frmReprint = new frmReprint();
            frmReprint.ShowDialog();
        }

        private void btnBillPermitUpdate_Click(object sender, EventArgs e)
        {
            // RMC 20150108
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

        private void frmMainFormACE_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmMainFormACE_FormClosed(object sender, FormClosedEventArgs e)
        {
            // RMC 20150112 mods in retirement billing
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
                pCmd.Query = "delete from a_trail where mod_code = 'CLI' and usr_code = '" + AppSettingsManager.SystemUser.UserCode + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmModifyDues form = new frmModifyDues();
            form.ShowDialog();
        }

        private void btnACEAbstract_Click(object sender, EventArgs e)
        {
            frmAbstractACE frmAbstractACE = new frmAbstractACE();
            frmAbstractACE.ShowDialog();
        }
    }
}