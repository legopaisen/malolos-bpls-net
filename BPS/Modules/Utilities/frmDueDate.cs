using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Utilities
{
    public partial class frmDueDate : Form
    {
        // RMC 20110825 Added initial values of due dates
        private DateTime dtpOldJan = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldFeb = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldMar = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldApr = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldMay = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldJun = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldJul = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldAug = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldSept = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldOct = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldNov = AppSettingsManager.GetCurrentDate();
        private DateTime dtpOldDec = AppSettingsManager.GetCurrentDate();
        private int intCtr = 0;

        public frmDueDate()
        {
            InitializeComponent();
        }

        
        private void frmDueDate_Load(object sender, EventArgs e)
        {
            intCtr = 0;
            this.EnableControls(false);
            txtYear.Text = ConfigurationAttributes.CurrentYear;
            LoadCurrentDate();
            this.OnViewDueYear();
            intCtr++;
        }

        private void EnableControls(bool blnEnable)
        {
            dtpJan.Enabled = blnEnable;
            dtpFeb.Enabled = blnEnable;
            dtpMar.Enabled = blnEnable;
            dtpApr.Enabled = blnEnable;
            dtpMay.Enabled = blnEnable;
            dtpJun.Enabled = blnEnable;
            dtpJul.Enabled = blnEnable;
            dtpAug.Enabled = blnEnable;
            dtpSept.Enabled = blnEnable;
            dtpOct.Enabled = blnEnable;
            dtpNov.Enabled = blnEnable;
            dtpDec.Enabled = blnEnable;
        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                this.txtYear.Enabled = true;
                this.btnView.Enabled = true;
                this.OnViewDueYear();
                this.btnClose.Text = "Close";
                this.btnEdit.Text = "Edit";
                this.EnableControls(false);
            }
            else
                this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            this.OnViewDueYear();
            
        }

        private void OnViewDueYear()
        {
            OracleResultSet result = new OracleResultSet();

            
            result.Query = string.Format("select * from due_dates where due_year = '{0}'", txtYear.Text);
            if (result.Execute())
            {
                if (result.Read())
                {}
                else
                {
                    if(txtYear.Text == ConfigurationAttributes.CurrentYear)
                        MessageBox.Show("No Record Found. \nPlease setup the due dates now.", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No Record Found.", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            result.Close();

            result.Query = string.Format("select * from due_dates where due_year = '{0}' order by due_code", txtYear.Text);
            if (result.Execute())
            {
                string strCode = string.Empty;
                
                while (result.Read())
                {
                    strCode = result.GetString("due_code");

                    if(strCode == "01")
                        dtpJan.Value = result.GetDateTime("due_date");
                    if(strCode == "02")
                        dtpFeb.Value = result.GetDateTime("due_date");
                    if(strCode == "03")
                        dtpMar.Value = result.GetDateTime("due_date");
                    if(strCode == "04")
                        dtpApr.Value = result.GetDateTime("due_date");
                    if(strCode == "05")
                        dtpMay.Value = result.GetDateTime("due_date");
                    if(strCode == "06")
                        dtpJun.Value = result.GetDateTime("due_date");
                    if(strCode == "07")
                        dtpJul.Value = result.GetDateTime("due_date");
                    if(strCode == "08")
                        dtpAug.Value = result.GetDateTime("due_date");
                    if(strCode == "09")
                        dtpSept.Value = result.GetDateTime("due_date");
                    if(strCode == "10")
                        dtpOct.Value = result.GetDateTime("due_date");
                    if(strCode == "11")
                        dtpNov.Value = result.GetDateTime("due_date");       
                    if(strCode == "12")
                        dtpDec.Value = result.GetDateTime("due_date");
                }
            }
            result.Close();

            dtpOldJan = dtpJan.Value;
            dtpOldFeb = dtpFeb.Value;
            dtpOldMar = dtpMar.Value;
            dtpOldApr = dtpApr.Value;
            dtpOldMay = dtpMay.Value;
            dtpOldJun = dtpJun.Value;
            dtpOldJul = dtpJul.Value;
            dtpOldAug = dtpAug.Value;
            dtpOldSept = dtpSept.Value;
            dtpOldOct = dtpOct.Value;
            dtpOldNov = dtpNov.Value;
            dtpOldDec = dtpDec.Value;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if(this.btnEdit.Text == "Edit")
            {
                this.btnEdit.Text = "Update";
                this.btnClose.Text = "Cancel";
                this.btnView.Enabled = false;
                this.EnableControls(true);
                this.txtYear.Enabled = false;
            }
            else
            {
                if(txtYear.Text.Trim() != "")
                {
                    if (MessageBox.Show("Save changes?", "Due Dates Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.UpdateDueDate("01", dtpJan.Value, lblJan.Text);
                        this.UpdateDueDate("02", dtpFeb.Value, lblFeb.Text);
                        this.UpdateDueDate("03", dtpMar.Value, lblMar.Text);
                        this.UpdateDueDate("04", dtpApr.Value, lblApr.Text);
                        this.UpdateDueDate("05", dtpMay.Value, lblMay.Text);
                        this.UpdateDueDate("06", dtpJun.Value, lblJun.Text);
                        this.UpdateDueDate("07", dtpJul.Value, lblJul.Text);
                        this.UpdateDueDate("08", dtpAug.Value, lblAug.Text);
                        this.UpdateDueDate("09", dtpSept.Value, lblSept.Text);
                        this.UpdateDueDate("10", dtpOct.Value, lblOct.Text);
                        this.UpdateDueDate("11", dtpNov.Value, lblNov.Text);
                        this.UpdateDueDate("12", dtpDec.Value, lblDec.Text);

                        this.btnEdit.Text = "Edit";
                        this.btnClose.Text = "Close";
                        this.btnView.Enabled = true;
                        this.EnableControls(false);
                        this.txtYear.Enabled = true;
                    }
                }
            }	
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            int intYear = 0;

            int.TryParse(txtYear.Text, out intYear);

            if (intYear == 0)
            {
                MessageBox.Show("Invalid value.", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtYear.Focus();
                return;
            }
        }

        private void UpdateDueDate(string strMonthCode, DateTime dtpNewDueDate, string strMonth)
        {
            OracleResultSet result = new OracleResultSet();
            DateTime dtpOldDueDate = AppSettingsManager.GetCurrentDate();
            string strObject = string.Empty;
            string strNewDueDate = "";
            string strOldDueDate = "";
                       
            result.Query = string.Format("select * from due_dates where due_code = '{0}' and due_year = '{1}'", strMonthCode, txtYear.Text);
            if (result.Execute())
            {
                if (result.Read())
                {
                    dtpOldDueDate = result.GetDateTime("due_date");
                }
            }
            result.Close();

            strNewDueDate = string.Format("{0:MM/dd/yyyy}", dtpNewDueDate);
            strOldDueDate = string.Format("{0:MM/dd/yyyy}", dtpOldDueDate);

            if (strNewDueDate != strOldDueDate)
            {
                result.Query = string.Format("delete from due_dates where due_year = '{0}' and due_code = '{1}'", txtYear.Text, strMonthCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "insert into due_dates(due_code, due_date, due_desc, due_year, date_save, user_code) values (:1,:2,:3,:4,:5,:6)";
                result.AddParameter(":1", strMonthCode);
                result.AddParameter(":2", Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}",dtpNewDueDate)));
                result.AddParameter(":3", strMonth);
                result.AddParameter(":4", txtYear.Text);
                result.AddParameter(":5", AppSettingsManager.GetCurrentDate());
                result.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                strObject = strMonth + " OLD: " + strOldDueDate;
                strObject += " NEW: " + strNewDueDate;

                if (AuditTrail.InsertTrail("AUDD", "due_dates", StringUtilities.HandleApostrophe(strObject)) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }

        private bool ValidateDate(DateTime dtpDueDate, DateTime dtpSucceeding, DateTime dtpPreceeding, string strMonth)
        {
            dtpDueDate = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dtpDueDate));
            dtpSucceeding = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dtpSucceeding));
            dtpPreceeding = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy}", dtpPreceeding));

            if (dtpDueDate > dtpSucceeding)
            {
                //MessageBox.Show("Invalid due date for month " + strMonth + "Date should not be earlier than the succeeding months.", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("Invalid due date for the month of " + strMonth + ".", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (strMonth != "JANUARY" && strMonth != "DECEMBER")
            {
                if (dtpDueDate < dtpPreceeding)
                {
                    //MessageBox.Show("Invalid due date for month " + strMonth + "Date should not be earlier than the succeeding months.", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("Invalid due date for the month of " + strMonth + ".", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            return true;
        }

        
        private void dtpJan_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpJan.Value, dtpFeb.Value, dtpJan.Value, lblJan.Text))
                dtpJan.Value = dtpOldJan;

            string strTempYear = string.Empty;
            strTempYear = dtpJan.Value.Year.ToString();

            if(strTempYear != txtYear.Text)
            {
                MessageBox.Show("Invalid due date for the month of " + lblJan.Text + ".", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpJan.Value = dtpOldJan;
            }
	

        }

        private void dtpFeb_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpFeb.Value, dtpMar.Value, dtpJan.Value, lblFeb.Text))
                dtpFeb.Value = dtpOldFeb;
        }

        private void dtpMar_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpMar.Value, dtpApr.Value, dtpFeb.Value, lblMar.Text))
                dtpMar.Value = dtpOldMar;
        }

        private void dtpApr_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpApr.Value, dtpMay.Value, dtpMar.Value, lblApr.Text))
                dtpApr.Value = dtpOldApr;
        }

        private void dtpMay_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpMay.Value, dtpJun.Value, dtpApr.Value, lblMay.Text))
                dtpMay.Value = dtpOldMay;
        }

        private void dtpJun_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpJun.Value, dtpJul.Value, dtpMay.Value, lblJun.Text))
                dtpJun.Value = dtpOldJun;
        }

        private void dtpJul_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpJul.Value, dtpAug.Value, dtpJun.Value, lblJul.Text))
                dtpJul.Value = dtpOldJul;
        }

        private void dtpAug_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpAug.Value, dtpSept.Value, dtpJul.Value, lblAug.Text))
                dtpAug.Value = dtpOldAug;
        }

        private void dtpSept_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpSept.Value, dtpOct.Value, dtpAug.Value, lblSept.Text))
                dtpSept.Value = dtpOldSept;
        }

        private void dtpOct_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpOct.Value, dtpNov.Value, dtpSept.Value, lblOct.Text))
                dtpOct.Value = dtpOldOct;
        }

        private void dtpNov_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (!this.ValidateDate(dtpNov.Value, dtpDec.Value, dtpOct.Value, lblNov.Text))
                dtpNov.Value = dtpOldNov;
        }

        private void dtpDec_ValueChanged(object sender, EventArgs e)
        {
            if (intCtr == 0)
                return;

            if (dtpDec.Value < dtpNov.Value)
            {
                MessageBox.Show("Invalid due date for the month of " + lblDec.Text + ".", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpDec.Value = dtpOldDec;
            }

            string strTempYear = string.Empty;
            strTempYear = dtpDec.Value.Year.ToString();

            if (strTempYear != txtYear.Text)
            {
                MessageBox.Show("Invalid due date for the month of " + lblJan.Text + ".", "Due Dates Setup", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dtpDec.Value = dtpOldDec;
            }
        }

        private void LoadCurrentDate()
        {
            dtpDec.Value = AppSettingsManager.GetCurrentDate();
            dtpNov.Value = AppSettingsManager.GetCurrentDate();
            dtpOct.Value = AppSettingsManager.GetCurrentDate();
            dtpSept.Value = AppSettingsManager.GetCurrentDate();
            dtpAug.Value = AppSettingsManager.GetCurrentDate();
            dtpJul.Value = AppSettingsManager.GetCurrentDate();
            dtpJun.Value = AppSettingsManager.GetCurrentDate();
            dtpMay.Value = AppSettingsManager.GetCurrentDate();
            dtpApr.Value = AppSettingsManager.GetCurrentDate();
            dtpMar.Value = AppSettingsManager.GetCurrentDate();
            dtpFeb.Value = AppSettingsManager.GetCurrentDate();
            dtpJan.Value = AppSettingsManager.GetCurrentDate();
            
        }

        

        

        
    }
}