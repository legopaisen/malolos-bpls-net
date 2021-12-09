using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.ContainerWithShadow;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;
using Amellar.Common.TaskManager;
using Amellar.Modules.AdditionalBusiness;
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.HealthPermit
{
    public partial class frmHealthPermit : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        HealthPermitClass HealthPermitClass = new HealthPermitClass();

        private string m_sBIN = string.Empty;   // RMC 20141228 modified permit printing (lubao)
        private bool m_bIsRenPUT = false;
        private string sBusinessAddress = string.Empty;
        private string id = string.Empty;
        private string m_sTmpBnsName = string.Empty;    // RMC 20150107

        //MCR 20150325 (s)
        private string m_sPermitType = string.Empty;
        public string PermitType
        {
            set { m_sPermitType = value; }
        }
        public string m_sOrNo = string.Empty;
        public string m_sOrDate = string.Empty;
        //MCR 20150325 (e)

        private DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();
    

        string TimeIN = string.Empty;

        public frmHealthPermit()
        {
            InitializeComponent();
            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
            bin2.txtTaxYear.Focus();
            this.ActiveControl = bin2.txtTaxYear;
            bin2.txtTaxYear.ReadOnly = true;    // RMC 20141228 modified permit printing (lubao)
            bin2.txtBINSeries.ReadOnly = true;  // RMC 20141228 modified permit printing (lubao)
        }

        private void ViewForm()
        {
            DisableText();
            //ViewHealthPermit();
            //btnNew.Enabled = false;
//            btnAdd.Enabled = false;
            //btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnDiscard.Enabled = false;
            //btnPrint.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
            //btnPrint.Enabled = false; // RMC 20141228 modified permit printing (lubao)
            btnDelete.Enabled = true;   // RMC 20141211 QA Health Permit module
            btnEdit.Enabled = true;    // RMC 20141211 QA Health Permit module
            btnNew.Enabled = true;  // RMC 20141211 QA Health Permit module
            txtTaxYear.Text = AppSettingsManager.GetConfigValue("12");  // RMC 20141211 QA Health Permit module

            // RMC 20141228 modified permit printing (lubao) (s)
            cboPlaceOfWork.Items.Clear();
            cboPlaceOfWork.Items.Add("");
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select * from brgy order by brgy_nm";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cboPlaceOfWork.Items.Add(pRec.GetString("brgy_nm"));
                }
            }
            pRec.Close();
            // RMC 20141228 modified permit printing (lubao) (e)

        }

        private void ViewHealthPermit()
        {
            DataTable table = new DataTable();
            //table = HealthPermitClass.HealthPermitView(bin2.GetBin());
            //table = HealthPermitClass.HealthPermitView(m_sBIN); // RMC 20141228 modified permit printing (lubao)
            table = HealthPermitClass.HealthPermitView(m_sBIN, txtBusinessName.Text); // RMC 20141228 modified permit printing (lubao)
            dtgRecord.DataSource = table;            
            dtgRecord.Columns[0].Visible = false;
            dtgRecord.Columns[14].Visible = false;

            OracleResultSet result = new OracleResultSet();
            
            //MCR 20150325 (s)
            if (m_sPermitType == "ARCS")
            {
                result.Query = "select bns_nm,bns_add from emp_names where bin = '" + m_sBIN + "'";
                if (result.Execute())
                    if (result.Read())
                    {
                        txtBusinessName.Text = result.GetString(0).Trim();
                        txtBusinessAddress.Text = result.GetString(1).Trim();
                    }
                result.Close();
            }
            //MCR 20150325 (e)

            result.Query = "Select distinct(emp_rh_no) from emp_names";
            if (result.Execute())
            {
                while (result.Read())
                {
                    for (int i = 0; i < cboRHNo.Items.Count; i++)
                    {
                        for (int y = 0; y < cboRHNo.Items.Count; y++)
                        {
                            if (y != i && cboRHNo.Items[i] == cboRHNo.Items[y])
                            {
                                cboRHNo.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    cboRHNo.Items.Add(result.GetString("emp_rh_no"));
                }
            }
            if (!result.Commit())
            {
                result.Rollback();
            }
        }

        private void GenderCountPlus()
        {
            //HealthPermitClass.UpdateGender(cboGender.Text, bin2.GetBin());
            //HealthPermitClass.UpdateGender(cboGender.Text, bin2.GetBin(), txtTaxYear.Text); // RMC 20141211 QA Health Permit module
            HealthPermitClass.UpdateGender(cboGender.Text, m_sBIN, txtTaxYear.Text); // RMC 20150104 addl mods
        }

        private void DisableText()
        {
            txtBusinessName.ReadOnly = true;
            txtBusinessAddress.ReadOnly = true;
            txtLastName.ReadOnly = true;
            cboOccType.Enabled = false; //MCR 20150115
            txtFirstName.ReadOnly = true;
            txtMiddleInitial.ReadOnly = true;
            txtOccupation.ReadOnly = true;
            txtAge.ReadOnly = true;
            dtpDateOfBirth.Enabled = false;
            cboGender.Enabled = false;
            cbNationality.Enabled = false;
            txtNationality.ReadOnly = true; OracleResultSet update = new OracleResultSet();
            //txtPlaceOfWork.ReadOnly = true;   // RMC 20141228 modified permit printing (lubao)
            cboPlaceOfWork.Enabled = false; // RMC 20141228 modified permit printing (lubao)
            txtAddress.ReadOnly = true;
            txtTIN.ReadOnly = true;
            txtCTCNo.ReadOnly = true;
            dtpIssuedOn.Enabled = false;
            txtIssuedAt.ReadOnly = true;
            cboStatus.Enabled = false;
            cboRHNo.Enabled = false;
            //rdoNew.Enabled = false;  // RMC 20141228 modified permit printing (lubao)
            //rdoRenewal.Enabled = false;  // RMC 20141228 modified permit printing (lubao)
            dtpIssuance.Enabled = false;
            dtpExpiration.Enabled = false;
            dtpXRay.Enabled = false;
            txtXRayResult.ReadOnly = true;
            dtpHepa.Enabled = false;
            txtHepaResult.ReadOnly = true;
            dtpDrugTest.Enabled = false;
            txtDrugTestResult.ReadOnly = true;
            dtpFecalysis.Enabled = false;
            txtFecalysisResult.ReadOnly = true;
            dtpUrinalysis.Enabled = false;
            txtUrinalysisResult.ReadOnly = true;
        }

        private void EnableText()
        {
            txtLastName.ReadOnly = false;
            txtFirstName.ReadOnly = false;
            txtMiddleInitial.ReadOnly = false;
            //txtOccupation.ReadOnly = false;   // RMC 20141228 modified permit printing (lubao)
            txtAge.ReadOnly = false;
            cboOccType.Enabled = true;
            dtpDateOfBirth.Enabled = true;
            cboGender.Enabled = true;
            txtNationality.ReadOnly = false;
            //txtPlaceOfWork.ReadOnly = false;  // RMC 20141228 modified permit printing (lubao)
            cboPlaceOfWork.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
            txtAddress.ReadOnly = false;
            txtTIN.ReadOnly = false;
            txtCTCNo.ReadOnly = false;
            dtpIssuedOn.Enabled = true;
            txtIssuedAt.ReadOnly = false;
            cboStatus.Enabled = true;
            cboRHNo.Enabled = true;
            dtpIssuance.Enabled = true;
            dtpExpiration.Enabled = true;
            dtpXRay.Enabled = true;
            txtXRayResult.ReadOnly = false;
            dtpHepa.Enabled = true;
            txtHepaResult.ReadOnly = false;
            dtpDrugTest.Enabled = true;
            txtDrugTestResult.ReadOnly = false;
            dtpFecalysis.Enabled = true;
            txtFecalysisResult.ReadOnly = false;
            dtpUrinalysis.Enabled = true;
            cbNationality.Enabled = true; //JARS 20160205
            txtUrinalysisResult.ReadOnly = false;
        }

        private void ClearText()
        {
            txtLastName.Text = string.Empty;
            cboOccType.SelectedIndex = 0; //MCR 20150115
            txtFirstName.Text = string.Empty;
            txtMiddleInitial.Text = string.Empty;
            txtOccupation.Text = string.Empty;
            txtAge.Text = string.Empty;
            dtpDateOfBirth.Text = string.Empty;
            cboGender.Text = string.Empty;
            txtNationality.Text = string.Empty;
            cbNationality.Text = string.Empty;
            //txtPlaceOfWork.Text = string.Empty;
            cboPlaceOfWork.Text = "";   // RMC 20141228 modified permit printing (lubao)
            txtAddress.Text = string.Empty;
            txtTIN.Text = string.Empty;
            txtCTCNo.Text = string.Empty;
            dtpIssuedOn.Text = string.Empty;
            txtIssuedAt.Text = string.Empty;
            cboStatus.Text = string.Empty;
            txtXRayResult.Text = string.Empty;
            txtHepaResult.Text = string.Empty;
            txtDrugTestResult.Text = string.Empty;
            txtFecalysisResult.Text = string.Empty;
            txtUrinalysisResult.Text = string.Empty;

            //txtBusinessName.Text = string.Empty;    // RMC 20150117
            //txtBusinessAddress.Text = string.Empty; // RMC 20150117
        }

        private void NewHealthPermit()
        {
            //btnNew.Enabled = false;   // RMC 20141211 QA Health Permit module, put rem
            //btnAdd.Enabled = true;    // RMC 20141211 QA Health Permit module, put rem
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            //btnPrint.Enabled = true;
            btnPrint.Enabled = false;   // RMC 20141211 QA Health Permit module
            btnDiscard.Enabled = true;
            EnableText();
            ClearText();
        }

        private void SaveHealthPermit()
        {
            if (txtLastName.Text == "")
            {
                MessageBox.Show("Please enter the last name");
                //txtLastName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if(txtFirstName.Text == "")
            {
                MessageBox.Show("Please enter the first name");
                //txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtOccupation.Text == "")
            {
                MessageBox.Show("Please enter the occupation");
                //txtOccupation.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtAge.Text == "")
            {
                MessageBox.Show("Please enter the age");
                //txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (cboGender.Text == "")
            {
                MessageBox.Show("Please choose the gender");
                //txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (cbNationality.Text == "") //JARS 20160205
            {
                MessageBox.Show("Please enter the nationality");
                //txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            //else if (txtNationality.Text == "")
            //{
            //    MessageBox.Show("Please enter the nationality");
            //    //txtFirstName.Focus();
            //    return; // RMC 20141211 QA Health Permit module
            //}
            //else if (txtPlaceOfWork.Text == "")
            else if (cboPlaceOfWork.Text == "")  // RMC 20141228 modified permit printing (lubao)
            {
                MessageBox.Show("Please enter the place of work");
                //txtPlaceOfWork.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter the address");
                //txtAddress.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtTIN.Text == "")
            {
                MessageBox.Show("Please enter the tin");
                //txtTIN.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            /*else if (txtCTCNo.Text == "")
            {
                MessageBox.Show("Please enter the CTC No.");
                return;
            }*/
            else if (cboStatus.Text == "")
            {
                MessageBox.Show("Please choose the status");
                //cboStatus.Focus();
                return; // RMC 20141211 QA Health Permit module
            }

            // RMC 20141228 modified permit printing (lubao) (s)
            if (txtBusinessName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter business name");
                return;
            }

            if (txtBusinessAddress.Text.Trim() == "")
            {
                MessageBox.Show("Please enter business address");
                return;
            }

            if (m_sBIN.Trim() == "")
            {
                if (MessageBox.Show("No BIN was selected.\nSystem will generate temporary BIN.\nContinue?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            // RMC 20141228 modified permit printing (lubao) (e)

            // RMC 20141211 QA Health Permit module (s)
            // validate duplication
            int iCnt = 0;
            pSet.Query = "select count(*) from emp_names where tax_year = '" + txtTaxYear.Text + "'";
            pSet.Query += " and EMP_LN = '" + StringUtilities.HandleApostrophe(txtLastName.Text.Trim()) + "'";
            pSet.Query += " and EMP_FN = '" + StringUtilities.HandleApostrophe(txtFirstName.Text.Trim()) + "'";
            pSet.Query += " and EMP_MI = '" + StringUtilities.HandleApostrophe(txtMiddleInitial.Text.Trim()) + "'";
            pSet.Query += " and (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";   // RMC 20150104 addl mods
            int.TryParse(pSet.ExecuteScalar(), out iCnt);
            if (iCnt > 0)
            {
                MessageBox.Show("Employee name already added", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int.TryParse(txtAge.Text.Trim(), out iCnt);
            if (iCnt == 0)
            {
                MessageBox.Show("Invalid age",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            // RMC 20141211 QA Health Permit module (e)


            //else  // RMC 20141211 QA Health Permit module, put rem
            {
                //HealthPermitClass.AddHealthPermit(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, txtNationality.Text, txtPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text, txtTaxYear.Text, bin2.GetBin(), cboRHNo.Text, dtpIssuance.Text, dtpExpiration.Text, dtpXRay.Text, txtXRayResult.Text, dtpHepa.Text, txtHepaResult.Text, dtpDrugTest.Text, txtDrugTestResult.Text, dtpFecalysis.Text, txtFecalysisResult.Text, dtpUrinalysis.Text, txtUrinalysisResult.Text, txtBusinessName.Text, txtBusinessAddress.Text);

                // RMC 20141228 modified permit printing (lubao) (s)
                //HealthPermitClass.AddHealthPermit(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, txtNationality.Text, cboPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text, txtTaxYear.Text, m_sBIN, cboRHNo.Text, dtpIssuance.Text, dtpExpiration.Text, dtpXRay.Text, txtXRayResult.Text, dtpHepa.Text, txtHepaResult.Text, dtpDrugTest.Text, txtDrugTestResult.Text, dtpFecalysis.Text, txtFecalysisResult.Text, dtpUrinalysis.Text, txtUrinalysisResult.Text, txtBusinessName.Text.Trim(), txtBusinessAddress.Text.Trim()); 
                HealthPermitClass.AddHealthPermit(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, cbNationality.Text, cboPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text, txtTaxYear.Text, m_sBIN, cboRHNo.Text, dtpIssuance.Text, dtpExpiration.Text, dtpXRay.Text, txtXRayResult.Text, dtpHepa.Text, txtHepaResult.Text, dtpDrugTest.Text, txtDrugTestResult.Text, dtpFecalysis.Text, txtFecalysisResult.Text, dtpUrinalysis.Text, txtUrinalysisResult.Text, txtBusinessName.Text.Trim(), txtBusinessAddress.Text.Trim(), txtCTCNo.Text, dtpIssuedOn.Text, txtIssuedAt.Text); //DJAC-20150107 mod permit
                if (m_sBIN == "")
                {
                    m_sBIN = HealthPermitClass.m_sTmpBIN;
                    txtTmpBin.Text = m_sBIN;
                }
                // RMC 20141228 modified permit printing (lubao) (e)

                // RMC 20141211 QA Health Permit module (s)
                OracleResultSet pTmp = new OracleResultSet();
                //string sObj = bin2.GetBin() + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;
                string sObj = m_sBIN + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;    // RMC 20141228 modified permit printing (lubao)
                if (AuditTrail.InsertTrail("ABHC-A", "emp_names", StringUtilities.HandleApostrophe(sObj)) == 0) //Add
                {

                    MessageBox.Show(pTmp.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            

                // RMC 20141211 QA Health Permit module (e)

                //PermitRecord();
                btnNew.Enabled = true;
                //btnAdd.Enabled = false;
                /*btnEdit.Enabled = false;
                btnDelete.Enabled = false;*/
                // RMC 20141211 QA Health Permit module, put rem
                btnEdit.Enabled = true; // RMC 20141211 QA Health Permit module
                btnDelete.Enabled = true;   // RMC 20141211 QA Health Permit module
                btnPrint.Enabled = true;
                btnDiscard.Enabled = false;
                //MessageBox.Show("Health Permit has been saved!");
                MessageBox.Show("Employee information has been saved", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);    // RMC 20141211 QA Health Permit module
                DisableText();
                ClearText();
                GenderCountPlus();
                btnNew.Text = "Add";    // RMC 20141211 QA Health Permit module
                dtgRecord.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
            }
        }

        private void EditHealthPermit()
        {
            if (txtLastName.Text == "")
            {
                MessageBox.Show("Please enter the last name");
               // txtLastName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if(txtFirstName.Text == "")
            {
                MessageBox.Show("Please enter the first name");
               // txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtOccupation.Text == "")
            {
                MessageBox.Show("Please enter the occupation");
               // txtOccupation.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtAge.Text == "")
            {
                MessageBox.Show("Please enter the age");
               // txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (cboGender.Text == "")
            {
                MessageBox.Show("Please choose the gender");
               // txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (cbNationality.Text == "") //JARS 20160205
            {
                MessageBox.Show("Please enter the nationality");
                //txtFirstName.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            //else if (txtNationality.Text == "")
            //{
            //    MessageBox.Show("Please enter the nationality");
            //   // txtFirstName.Focus();
            //    return; // RMC 20141211 QA Health Permit module
            //}
            //else if (txtPlaceOfWork.Text == "")
            else if (cboPlaceOfWork.Text == "") // RMC 20141228 modified permit printing (lubao)
            {
                MessageBox.Show("Please enter the place of work");
               // txtPlaceOfWork.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter the address");
               // txtAddress.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            else if (txtTIN.Text == "")
            {
                MessageBox.Show("Please enter the tin");
               // txtTIN.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            /*else if (txtCTCNo.Text == "")
            {
                MessageBox.Show("Please enter the CTC No.");
                return;
            }*/
            else if (cboStatus.Text == "")
            {
                MessageBox.Show("Please choose the status");
                //cboStatus.Focus();
                return; // RMC 20141211 QA Health Permit module
            }
            // RMC 20141228 modified permit printing (lubao) (s)
            if (txtBusinessName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter business name",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            if (txtBusinessAddress.Text.Trim() == "")
            {
                MessageBox.Show("Please enter business address", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (id == "")
            {
                MessageBox.Show("Please select record to edit", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20141228 modified permit printing (lubao) (e)

            // RMC 20141211 QA Health Permit module (s)
            // validate duplication
            int iCnt = 0;
            pSet.Query = "select count(*) from emp_names where tax_year = '" + txtTaxYear.Text + "'";
            pSet.Query += " and EMP_LN = '" + StringUtilities.HandleApostrophe(txtLastName.Text.Trim()) + "'";
            pSet.Query += " and EMP_FN = '" + StringUtilities.HandleApostrophe(txtFirstName.Text.Trim()) + "'";
            pSet.Query += " and EMP_MI = '" + StringUtilities.HandleApostrophe(txtMiddleInitial.Text.Trim()) + "'";
            pSet.Query += " and emp_id <> '" + id + "'";
            int.TryParse(pSet.ExecuteScalar(), out iCnt);
            //if (iCnt > 0) JARS 20160104 had to comment out temporarily, conflict with editing of health permit.
            //{
            //    MessageBox.Show("Employee name already added", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}
            // RMC 20141211 QA Health Permit module (e)

            //else  // RMC 20141211 QA Health Permit module, put rem
            {
                //HealthPermitClass.UpdateHealthPermit(id, txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, txtNationality.Text, txtPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text);
                //HealthPermitClass.UpdateHealthPermit(id, txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, txtNationality.Text, cboPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text, cboRHNo.Text, dtpIssuance.Text, dtpExpiration.Text, dtpXRay.Text, txtXRayResult.Text, dtpHepa.Text, txtHepaResult.Text, dtpDrugTest.Text, txtDrugTestResult.Text, dtpFecalysis.Text, txtFecalysisResult.Text, dtpUrinalysis.Text, txtUrinalysisResult.Text, txtBusinessName.Text.Trim(), txtBusinessAddress.Text.Trim());    // RMC 20141228 modified permit printing (lubao)
                HealthPermitClass.UpdateHealthPermit(id, txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text, txtOccupation.Text, txtAge.Text, dtpDateOfBirth.Text, cboGender.Text, cbNationality.Text, cboPlaceOfWork.Text, txtAddress.Text, txtTIN.Text, cboStatus.Text, cboRHNo.Text, dtpIssuance.Text, dtpExpiration.Text, dtpXRay.Text, txtXRayResult.Text, dtpHepa.Text, txtHepaResult.Text, dtpDrugTest.Text, txtDrugTestResult.Text, dtpFecalysis.Text, txtFecalysisResult.Text, dtpUrinalysis.Text, txtUrinalysisResult.Text, txtBusinessName.Text.Trim(), txtBusinessAddress.Text.Trim(), txtCTCNo.Text, dtpIssuedOn.Text, txtIssuedAt.Text); //DJAC-20150107 mod permit
                
                // RMC 20141211 QA Health Permit module (s)
                //string sObj = bin2.GetBin() + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;
                string sObj = m_sBIN + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;    // RMC 20141228 modified permit printing (lubao)
                if (AuditTrail.InsertTrail("ABHC-E", "emp_names", StringUtilities.HandleApostrophe(sObj)) == 0) //edit
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RMC 20141211 QA Health Permit module (e)

                btnNew.Enabled = true;
                /*btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = true;
                btnDiscard.Enabled = false;*/
                //MessageBox.Show("Health Permit has been updated!");
                btnEdit.Enabled = true; // RMC 20141211 QA Health Permit module 
                btnDelete.Enabled = true;   // RMC 20141211 QA Health Permit module 
                btnPrint.Enabled = true;    // RMC 20141211 QA Health Permit module 
                btnDiscard.Enabled = true;  // RMC 20141211 QA Health Permit module 
                MessageBox.Show("Employee information has been updated", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);    // RMC 20141211 QA Health Permit module
                DisableText();
                ClearText();
                GenderCountPlus();
                btnEdit.Text = "Edit";  // RMC 20141211 QA Health Permit module 
            }
        }

        private void DeleteHealthPermit()
        {
            HealthPermitClass.DeleteHealthPermit(id);

            // RMC 20141211 QA Health Permit module (s)
            OracleResultSet pTmp = new OracleResultSet();
            //string sObj = bin2.GetBin() + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;
            string sObj = m_sBIN + ": " + txtLastName.Text + ", " + txtFirstName.Text + " " + txtMiddleInitial.Text;    // RMC 20141228 modified permit printing (lubao)
            if (AuditTrail.InsertTrail("ABHC-D", "emp_names", StringUtilities.HandleApostrophe(sObj)) == 0) //delete
            {
                MessageBox.Show(pTmp.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // RMC 20141211 QA Health Permit module (e)

            btnNew.Enabled = true;
            //btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = true;
            btnPrint.Enabled = false;
            btnDiscard.Enabled = false;
            GenderCountPlus();
            MessageBox.Show("Health Permit has been deleted!");
            btnDelete.Enabled = true; //JARS 20160111 OFFSITE UPDATE 
            DisableText();
            ClearText();
        }

        private void PrintHealthPermit()
        {
            // RMC 20150107 (s)
            OracleResultSet pRecT = new OracleResultSet();
            

            int iCnt = 0;
            pRecT.Query = "select count(*) from emp_names where (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "')";
            pRecT.Query += " and tax_year = '" + txtTaxYear.Text + "'";// and emp_occupation = 'OWNER'";
            int.TryParse(pRecT.ExecuteScalar(), out iCnt);
            


            //if (iCnt == 0)
            //{
            //    MessageBox.Show("Add Owner's information first",this.Text, MessageBoxButtons.OK,MessageBoxIcon.Stop);
            //    return;
            //}
            // RMC 20150107 (e)

            //frmPrint frmPrint = new frmPrint();
            ////frmPrint.BIN = bin2.GetBin();
            //frmPrint.BIN = m_sBIN;  // RMC 20141228 modified permit printing (lubao)
            //frmPrint.TaxYear = txtTaxYear.Text;
            //frmPrint.ShowDialog();

            frmPrinting frmPrinting = new frmPrinting();
            frmPrinting.ReportType = "Health";
            frmPrinting.BIN = m_sBIN;
            frmPrinting.TaxYear = txtTaxYear.Text;
            frmPrinting.ShowDialog();
        }

        private void CancelHealthPermit()
        {
            DisableText();
            //btnNew.Enabled = false;   // RMC 20141228 modified permit printing (lubao)
            /*btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnDiscard.Enabled = false;
            btnPrint.Enabled = true;*/
            // RMC 20141211 QA Health Permit module, put rem
            ClearText();
            ClearControls();
            //btnAdd.Text = "Add";    // RMC 20141211 QA Health Permit module, put rem
            // RMC 20141211 QA Health Permit module (s)
            btnNew.Enabled = true;   // RMC 20141228 modified permit printing (lubao)
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnDiscard.Enabled = true;
            btnPrint.Enabled = true;
            btnNew.Text = "Add";
            btnEdit.Text = "Edit";  
            // RMC 20141211 QA Health Permit module (e)

            // RMC 20141228 modified permit printing (lubao) (s)
            btnSearch.Enabled = false;
            btnSearchTmp.Enabled = false;
            // RMC 20141228 modified permit printing (lubao) (e)
            if (m_sPermitType == "ARCS")
            {
                ViewHealthPermit();
            }
                 
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            TimeIN = AppSettingsManager.GetSystemDate().ToString();
            
            
            //NewHealthPermit();    // RMC 20141211 QA Health Permit module, put rem
            // RMC 20141211 QA Health Permit module (s)
            if (btnNew.Text == "Add")
            {
                EnableText(); //MCR 20150115
                NewHealthPermit();
                btnNew.Text = "Save";
                btnDiscard.Enabled = true;
                rdoNew.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
                rdoRenewal.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
                dtgRecord.Enabled = false;  // RMC 20141228 modified permit printing (lubao)

                if (m_sPermitType == "ARCS")
                {
                    if (dtgRecord.RowCount <= 0)
                    {
                        txtBusinessName.ReadOnly = false;
                        txtBusinessAddress.ReadOnly = false;
                    }
                }
            }
            else
            {
                cboRHNo.Items.Clear();
                SaveHealthPermit();
                ViewHealthPermit();
                
            }
            // RMC 20141211 QA Health Permit module (e)

        }

        /*private void btnAdd_Click(object sender, EventArgs e)
        {
            SaveHealthPermit();
            ViewHealthPermit();
        }*/
        // RMC 20141211 QA Health Permit module, put rem

        private void btnEdit_Click(object sender, EventArgs e)
        {
            TimeIN = AppSettingsManager.GetSystemDate().ToString();
            
            // RMC 20141211 QA Health Permit module (s)
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                EnableText();
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                btnDiscard.Enabled = true;
                rdoNew.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
                rdoRenewal.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
                dtgRecord.Enabled = true;  // RMC 20141228 modified permit printing (lubao)
            }// RMC 20141211 QA Health Permit module (e)
            else
            {
                cboRHNo.Items.Clear();
                EditHealthPermit();
                ViewHealthPermit();
                DisableText();  // RMC 20141211 QA Health Permit module 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteHealthPermit();
            ViewHealthPermit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintHealthPermit();
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            CancelHealthPermit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(m_sBIN, "HEALTH PERMIT", "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            this.Dispose();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(m_sBIN, "HEALTH PERMIT", "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            if (btnSearch.Text == "Search")
            {
                if (bin2.txtTaxYear.Text != "" || bin2.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin2.GetBin(), "HEALTH PERMIT", "ADD", "ASS"))
                    {
                        m_sBIN = bin2.GetBin(); // RMC 20150104 addl mods
                        string sBnsStat = "";
                        OracleResultSet pSet = new OracleResultSet(); //JARS 20170120 TO CHECK IF RETIRED
                        pSet.Query = "select bns_stat from businesses where bin = '" + m_sBIN + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                                sBnsStat = pSet.GetString(0);
                        pSet.Close();
                        if (sBnsStat == "RET")
                        {
                            if (MessageBox.Show(m_sBIN + " is RETIRED, Continue?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                btnSearch.Text = "Search";  // RMC 20150105 mods in permit printing
                                ClearControls();
                                return;
                            }
                        }
                        LoadValues();
                        ViewHealthPermit();
                        dtgRecord.Refresh(); //JARS 20151223
                        //m_sBIN = bin2.GetBin(); // RMC 20141228 modified permit printing (lubao)  // RMC 20150104 addl mods, put rem
                    }
                    else
                    {
                        bin2.txtTaxYear.Text = "";
                        bin2.txtBINSeries.Text = "";
                        m_sBIN = "";    // RMC 20141228 modified permit printing (lubao)
                    }
                }
                else
                {
                    btnPrintSelected.Enabled = false; //JARS 20160118
                    ClearControls();
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();
                    //frmSearchBns.ModuleCode = m_sFormStatus;
                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin2.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin2.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin2.GetBin(), "HEALTH PERMIT", "ADD", "ASS"))
                        {
                            m_sBIN = bin2.GetBin(); // RMC 20150104 addl mods
                            LoadValues();
                            ViewHealthPermit();
                            //m_sBIN = bin2.GetBin(); // RMC 20141228 modified permit printing (lubao)  // RMC 20150104 addl mods, put rem
                        }
                        else
                        {
                            bin2.txtTaxYear.Text = "";
                            bin2.txtBINSeries.Text = "";
                            m_sBIN = "";    // RMC 20141228 modified permit printing (lubao)
                        }
                    }
                }

                
            }
            else
            {
                ClearControls();
            }

            //JHB 20200121 added trail
        }
        
        private void LoadValues()
        {
            OracleResultSet result = new OracleResultSet();
            pSet.Query = string.Format("select * from business_que where bin = '{0}'", bin2.GetBin());
            pSet.Query += string.Format(" and tax_year = '{0}'", ConfigurationAttributes.CurrentYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_bIsRenPUT = true;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}'", bin2.GetBin());

                    m_bIsRenPUT = true;
                }
            }

            if (m_bIsRenPUT)
            {
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sBusinessAddress = 
                            pSet.GetString("BNS_HOUSE_NO").Trim() + " " +
                            pSet.GetString("BNS_STREET").Trim() + " " +
                            pSet.GetString("BNS_MUN").Trim() + " " +
                            pSet.GetString("BNS_DIST").Trim() + " " +
                            pSet.GetString("BNS_ZONE").Trim() + " " +
                            pSet.GetString("BNS_BRGY").Trim() + " " +
                            pSet.GetString("BNS_PROV").Trim() + " " +
                            pSet.GetString("BNS_ZIP").Trim();
                        //txtTaxYear.Text = DateTime.Now.Year.ToString();   // RMC 20141211 QA Health Permit module, put rem   
                        txtBusinessName.Text = pSet.GetString("BNS_NM").Trim();
                        txtBusinessAddress.Text = sBusinessAddress;
                        m_sTmpBnsName = txtBusinessName.Text;   // RMC 20150107
                        //btnNew.Enabled = true;    // RMC 20141228 modified permit printing (lubao)
                        //btnPrint.Enabled = true;    // RMC 20141228 modified permit printing (lubao)
                        result.Close();
                        btnSearch.Text = "Clear";
                        pSet.Close();
                        ////search own_names
                        //result.Query = "Insert into emp_names(bin, tax_year, emp_ln, emp_fn, emp_mi, emp_address, emp_id) select '" + bin2.GetBin() + "', '" + txtTaxYear.Text + "', ow.own_ln, ow.own_fn, ow.own_mi, own_house_no || ' ' || own_street || ' ' || own_dist || ' ' || own_zone || ' ' || own_brgy || ' ' || own_mun || ' ' || own_prov, emp_sequence.nextval from own_names ow left join businesses bn on bn.own_code = ow.own_code left join business_que bnque on bnque.own_code = ow.own_code where (bn.bin = '" + bin2.GetBin() + "' or bnque.bin = '" + bin2.GetBin() + "') and not exists(select e.bin from emp_names e where e.bin = '" + bin2.GetBin() + "')";
                        //result.ExecuteNonQuery();
                        //if (!result.Commit())
                        //{
                        //    result.Rollback();
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Record not found.");
                        ClearControls();
                        btnSearch.Text = "Search";
                        return;
                    }

                }
            }
            else
            {
                MessageBox.Show("Record not found.");
                ClearControls();
                btnSearch.Text = "Search";
                return;
            }
        }

        private void ClearControls()
        {
            if (TaskMan.IsObjectLock(bin2.GetBin(), "HEALTH PERMIT", "DELETE", "ASS"))
            {
            }

            // RMC 20141228 modified permit printing (lubao) (s)
            if (TaskMan.IsObjectLock(m_sBIN, "HEALTH PERMIT", "DELETE", "ASS"))
            {
            }
            if (m_sPermitType == "ARCS") //MCR 20150326
                m_sBIN = lblORNo.Text.Trim();
            else
                m_sBIN = "";
            txtTmpBin.Text = "";
            rdoNew.Checked = false;
            rdoRenewal.Checked = false;
            id = "";
            // RMC 20141228 modified permit printing (lubao) (e)

            bin2.txtTaxYear.Text = string.Empty;
            bin2.txtBINSeries.Text = string.Empty;
            txtBusinessAddress.Text = string.Empty;
            txtBusinessName.Text = string.Empty;
            //txtTaxYear.Text = string.Empty;   // RMC 20141211 QA Health Permit module
            bin2.txtTaxYear.Focus();
            btnSearch.Text = "Search";
            btnNew.Enabled = false;
            btnPrint.Enabled = false;
            dtgRecord.DataSource = null;
            dtgRecord.Enabled = true;
            dtgRecord.Rows.Clear();
            
            ViewHealthPermit();

            btnEditBnsName.Enabled = false;
        }

        private void dtgRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TimeIN = AppSettingsManager.GetSystemDate().ToString();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dtgRecord.Rows[e.RowIndex];
                txtLastName.Text = row.Cells["Last Name"].Value.ToString();
                txtFirstName.Text = row.Cells["First Name"].Value.ToString();
                txtMiddleInitial.Text = row.Cells["M.I"].Value.ToString();
                txtOccupation.Text = row.Cells["Occupation"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                dtpDateOfBirth.Text = row.Cells["Date of Birth"].Value.ToString();
                cboGender.Text = row.Cells["Gender"].Value.ToString();
                //txtNationality.Text = row.Cells["Nationality"].Value.ToString(); //JARS 20160205
                cbNationality.Text = row.Cells["Nationality"].Value.ToString();
                //txtPlaceOfWork.Text = row.Cells["Place of work"].Value.ToString();
                cboPlaceOfWork.Text = row.Cells["Place of work"].Value.ToString();  // RMC 20141228 modified permit printing (lubao)
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtTIN.Text = row.Cells["TIN"].Value.ToString();
                //DJAC-20150107 mod permit (
                txtCTCNo.Text = row.Cells["CTC No."].Value.ToString();
                dtpIssuedOn.Text = row.Cells["CTC Issued On"].Value.ToString();
                txtIssuedAt.Text = row.Cells["CTC Issued At"].Value.ToString();
                //DJAC-20150107 mod permit )
                cboStatus.Text = row.Cells["Status"].Value.ToString();
                id = row.Cells["Reg. No."].Value.ToString();
                cboRHNo.Text = row.Cells["RH No"].Value.ToString();
                // RMC 20141228 modified permit printing (lubao) (s)
                dtpIssuance.Text = row.Cells["ISSUANCE"].Value.ToString();
                dtpExpiration.Text = row.Cells["EXPIRATION"].Value.ToString();
                dtpXRay.Text= row.Cells["XRAY DATE"].Value.ToString();
                txtXRayResult.Text = row.Cells["XRAY RESULT"].Value.ToString();
                dtpHepa.Text = row.Cells["HEPA-B DATE"].Value.ToString();
                txtHepaResult.Text = row.Cells["HEPA-B RESULT"].Value.ToString();
                dtpDrugTest.Text = row.Cells["DRUG TEST DATE"].Value.ToString();
                txtDrugTestResult.Text = row.Cells["DRUG TEST RESULT"].Value.ToString();
                dtpFecalysis.Text = row.Cells["FECALYSIS DATE"].Value.ToString();
                txtFecalysisResult.Text = row.Cells["FECALYSIS RESULT"].Value.ToString();
                dtpUrinalysis.Text = row.Cells["URINALYSIS DATE"].Value.ToString();
                txtUrinalysisResult.Text = row.Cells["URINALYSIS RESULT"].Value.ToString();
                // RMC 20141228 modified permit printing (lubao) (e)
            }
            btnPrintSelected.Enabled = true;    //JARS 20160118 added for selected print.
            btnPrint.Enabled = true;
            /*btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnPrint.Enabled = true;
            btnPrint.Enabled = false;
            btnDiscard.Enabled = true;*/
            // RMC 20141211 QA Health Permit module, put rem
        }

        private void frmHealthPermit_Load(object sender, EventArgs e)
        {
            if (m_sPermitType == "ARCS")
            {
                pnlHealthArcs.BringToFront();
                pnlHealth.Enabled = false;
                m_sBIN = m_sOrNo;
                lblORNo.Text = m_sOrNo;
                lblORDate.Text = m_sOrDate;
                ViewHealthPermit();
                cntBnsName.Size = new Size(404, 89);
                btnEditBnsName.Visible = false;
            }
            else
            {
                pnlHealth.BringToFront();
                pnlHealthArcs.Enabled = false;
            }


            OracleResultSet loadNat = new OracleResultSet(); //JARS 20160205
            cbNationality.Items.Add("");
            loadNat.Query = "select nat_code,nat_desc from nationality_tbl order by nat_desc asc";
            if(loadNat.Execute())
            {
                while(loadNat.Read())
                {
                    cbNationality.Items.Add(loadNat.GetString(0));
                    
                }
            }
            loadNat.Close();
            ViewForm();
        }

        private void dtgRecord_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dtgRecord_CellContentClick(sender, e);
        }

        private void cboRHNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string str = e.KeyChar.ToString().ToUpper();
            char[] ch = str.ToCharArray();
            e.KeyChar = ch[0];
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {

        }

        private void rdoRenewal_CheckedChanged(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            if (rdoRenewal.Checked == true)
            {
                ClearText();    // RMC 20150102 mods in permit
                rdoNew.Checked = false;
                btnSearch.Enabled = true;
                bin2.txtTaxYear.ReadOnly = false;    // RMC 20150104 addl mods
                bin2.txtBINSeries.ReadOnly = false;  // RMC 20150104 addl mods
                btnEditBnsName.Enabled = true;  // RMC 20150107

                txtTmpBin.Text = string.Empty;  // RMC 20150117
                m_sBIN = string.Empty;  // RMC 20150117
                txtBusinessName.Text = string.Empty;    // RMC 20150117
                txtBusinessAddress.Text = string.Empty; // RMC 20150117
            }
            else
            {
                btnSearch.Enabled = false;
                bin2.txtTaxYear.ReadOnly = true;    // RMC 20150104 addl mods
                bin2.txtBINSeries.ReadOnly = true;  // RMC 20150104 addl mods
                btnEditBnsName.Enabled = false;  // RMC 20150107
            }
        }

        private void rdoNew_CheckedChanged(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            if (rdoNew.Checked == true)
            {
                ClearText();    // RMC 20150102 mods in permit
                rdoRenewal.Checked = false;
                btnSearchTmp.Enabled = true;
                txtBusinessName.ReadOnly = false;
                txtBusinessAddress.ReadOnly = false;
                bin2.txtTaxYear.Text = "";  // RMC 20150117
                bin2.txtBINSeries.Text = "";    // RMC 20150117
                m_sBIN = "";    // RMC 20150117
                txtBusinessName.Text = string.Empty;    // RMC 20150117
                txtBusinessAddress.Text = string.Empty; // RMC 20150117
            }
            else
            {
                btnSearchTmp.Enabled = false;
                txtBusinessName.ReadOnly = true;
                txtBusinessAddress.ReadOnly = true;
                txtBusinessAddress.Text = "";
                txtBusinessName.Text = "";
            }
        }

        private void btnSearchTmp_Click(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            if (TaskMan.IsObjectLock(m_sBIN, "HEALTH PERMIT", "DELETE", "ASS"))  // RMC 20141228 modified permit printing (lubao)
            {
            }

            frmSearchTmp form = new frmSearchTmp();
            form.TaxYear = txtTaxYear.Text;
            form.Permit = "Health"; // RMC 20150102 mods in permit
            form.Val = "false";
            form.ShowDialog();

            txtTmpBin.Text = form.BIN;

            m_sBIN = txtTmpBin.Text;
            if (!TaskMan.IsObjectLock(m_sBIN, "HEALTH PERMIT", "ADD", "ASS"))
            {
                txtBusinessName.Focus();
                pSet = new OracleResultSet();
                pSet.Query = "select * from emp_names where (bin = '" + m_sBIN + "' or temp_bin = '" + m_sBIN + "') and tax_year = '" + txtTaxYear.Text + "'";
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtBusinessName.Text = pSet.GetString("bns_nm");
                        txtBusinessAddress.Text = pSet.GetString("bns_add");
                    }
                }
               

                pSet.Close();

                int iCnt = 0;
                pSet.Query = "select count(*) from businesses where bin = '" + m_sBIN + "'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    txtBusinessAddress.ReadOnly = true;
                    txtBusinessName.ReadOnly = true;
                }
                else
                {
                    pSet.Query = "select count(*) from business_que where bin = '" + m_sBIN + "'";
                    int.TryParse(pSet.ExecuteScalar(), out iCnt);

                    if (iCnt > 0)
                    {
                        txtBusinessAddress.ReadOnly = true;
                        txtBusinessName.ReadOnly = true;
                    }
                }

                ViewHealthPermit();
            }
            else
            {
                m_sBIN = "";
                txtTmpBin.Text = "";
            }

            // RMC 20141228 modified permit printing (lubao) (s)
            if (btnNew.Text == "Save" && txtBusinessName.Text != "")
            {
                txtBusinessAddress.ReadOnly = true;
                txtBusinessName.ReadOnly = true;
            }
            // RMC 20141228 modified permit printing (lubao) (e)
            
        }

        private void cboOccType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            if (cboOccType.Text == "OWNER")
            {
                txtOccupation.Text = "OWNER";
                txtOccupation.ReadOnly = true;
            }
            else
            {
                txtOccupation.ReadOnly = false;
                txtOccupation.Text = "";
            }
        }

        private void cboPlaceOfWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            // RMC 20141228 modified permit printing (lubao)
            OracleResultSet pRec = new OracleResultSet();

            pRec.Query = "select emp_rh_no from emp_names where emp_place_of_work = '" + cboPlaceOfWork.Text + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    cboRHNo.Text = pRec.GetString(0);
                }
                else
                    cboRHNo.Text = "";  // RMC 20150102 mods in permit
            }
            pRec.Close();
        }

        private void btnEditBnsName_Click(object sender, EventArgs e)
        {
            string sQuery = "";
            
            OracleResultSet pCmd = new OracleResultSet();

            if (btnEditBnsName.Text == "Edit DTI Business Name")
            {
                txtBusinessName.ReadOnly = false;
                btnEditBnsName.Text = "Update DTI Business Name";
            }
            else
            {
                string sObj = "";

                if (txtBusinessName.Text.Trim() != "")
                {
                    if (MessageBox.Show("Save business name?\nPlease note: this edited business name is for printing of Permit only.", "Business Permit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string sDate = "";

                        pCmd.Query = "delete from dti_bns_name where bin = '" + m_sBIN + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = "insert into dti_bns_name values (";
                        pCmd.Query += "'" + m_sBIN + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(txtBusinessName.Text) + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        sDate = Convert.ToString(AppSettingsManager.GetCurrentDate());
                        pCmd.Query += " to_date('" + sDate + "', 'MM/dd/yyyy hh:mi:ss am')) ";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        sObj = m_sBIN + ": Edited Permit Business Name to " + txtBusinessName.Text;

                        if (AuditTrail.InsertTrail("ABBP-EBN", "dti_bns_name", sObj) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        txtBusinessName.Text = m_sTmpBnsName;
                    }
                }
                else
                {
                    pCmd.Query = "delete from dti_bns_name where bin = '" + m_sBIN + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (m_sTmpBnsName != "")
                    {
                        sObj = m_sBIN + ": Deleted edited Permit Business Name - " + txtBusinessName;

                        if (AuditTrail.InsertTrail("ABBP-EBN", "dti_bns_name", sObj) == 0)
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                txtBusinessName.ReadOnly = true;
                btnEditBnsName.Text = "Edit DTI Business Name";
            }
        }

        private void PermitRecord()
        {
            String m_sPermitNumber = "";
            #region GetNextSeries
            OracleResultSet result = new OracleResultSet();
            String sQuery = String.Empty;
            result.Query = "select coalesce(max(to_number(permit_number)),0)+1 as permit_number from permit_type where perm_type = 'WORKING' and current_year = '" + AppSettingsManager.GetConfigValue("12") + "'";
            if (result.Execute())
            {
                if (result.Read())
                    m_sPermitNumber = AppSettingsManager.GetConfigValue("12") + "-" + result.GetInt(0).ToString("0000");
            }
            result.Close();
            #endregion

            #region Saving
            string sCurrentYear = AppSettingsManager.GetConfigValue("12");
            string sPermType = "WORKING";
            string sPermitNumber = m_sPermitNumber.Substring(5);
            string sIssuedDate = AppSettingsManager.GetSystemDate().ToString("MM/dd/yyyy");
            string sUserCode = AppSettingsManager.SystemUser.UserCode;
            //string s_mBin = HealthPermitClass.m_sTmpBIN; 
            string s_mBin = bin2.GetBin(); //DJAC-20150107 mod permit, change bin

            result.Query = "insert into permit_type (current_year,perm_type,permit_number,issued_date,user_code,bin) values('" + sCurrentYear + "', '" + sPermType + "', '" + sPermitNumber + "', '" + sIssuedDate + "', '" + sUserCode + "', '" + s_mBin + "')";
            result.ExecuteNonQuery();
            #endregion
        }

        private void ButtonControls(bool blnEnable)
        {

        }

        #region ARCS
        private void ARCSPermit()
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select ";
            if (pSet.Execute())
                while (pSet.Read())
                { 
                
                }
            pSet.Close();
        }
        #endregion

        private void btnPrintSelected_Click(object sender, EventArgs e) //JARS 20160118
        {
            btnPrintSelected.Enabled = false;
            string sLast = txtLastName.Text;
            string sFirst = txtFirstName.Text;
            frmPrinting frmPrinting = new frmPrinting();
            frmPrinting.ReportType = "SelectedHealth";
            frmPrinting.BIN = m_sBIN;
            frmPrinting.TaxYear = txtTaxYear.Text;
            frmPrinting.EmpLn = sLast;
            frmPrinting.EmpFn = sFirst;
            frmPrinting.m_timeIN = TimeIN;
            frmPrinting.ShowDialog();
        }

        private void cbNationality_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpDateOfBirth_ValueChanged(object sender, EventArgs e)
        {
            ComputeAge();
        }

        private void ComputeAge() //JARS 20170201
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(dtpDateOfBirth.Value.ToString("yyyyMMdd"));
            int age = (now - dob) / 10000;

            if(age >= 100) //JARS 20170724
            {
                MessageBox.Show("Age must not reach more than 3 DIGITS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtAge.Text = age.ToString();
        }

        private void txtBusinessName_TextChanged(object sender, EventArgs e)
        {

        }
        //private void frmHealthPermit_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    ClearControls();
        //}
        //DJAC-20150108 mod permit
    }
}