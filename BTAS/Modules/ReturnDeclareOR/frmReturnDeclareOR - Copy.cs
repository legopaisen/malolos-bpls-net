using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.BPLSApp;

namespace ReturnDeclareOR
{
    public partial class frmReturnDeclareOR : Form
    {
        Library LibraryClass = new Library();
        public frmReturnDeclareOR()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDeclare_Click(object sender, EventArgs e)
        {
            DateTime dtSystemDate;
            string sCurrentDate = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sCurrentDate = string.Format("{0:MM/dd/yyyy}", dtSystemDate);
            //dtSystemDate = DateTime.Parse(sCurrentDate);
            OracleResultSet result = new OracleResultSet();

            // RMC 20141203 corrected error in online payment (s)
            if (txtSeries.Text.Trim().Length < 2 && AppSettingsManager.GetConfigValue("56") == "Y") //MCR 20140904 OR Series Validation
            {
                MessageBox.Show("Invalid OR Series");
                return;
            }
            // RMC 20141203 corrected error in online payment (e)

            if(txtFrom.Text.Trim() == string.Empty || int.Parse(txtFrom.Text.Trim()) < 1 || txtTo.Text.Trim() == string.Empty || int.Parse(txtTo.Text.Trim()) < 1 || int.Parse(txtFrom.Text.Trim()) > int.Parse(txtTo.Text.Trim()))
            {
                MessageBox.Show("Invalid OR Range");
                return;
            }

            for(int i = int.Parse(txtFrom.Text.Trim()); i<= int.Parse(txtTo.Text.Trim()); i++)
            {
                //result.Query = "select * from or_used where or_no = '" + i + "' and trn_date = '" + string.Format("{0:dd-MMM-yyyy}", dtSystemDate) + "'";
                // RMC 20141203 corrected error in online payment (s)
                // this mod not to be merged to antipolo ver
                result.Query = "select * from or_used where or_no = '" + i + "'";
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    result.Query += " and or_series = '" + txtSeries.Text.Trim() + "'";
                // RMC 20141203 corrected error in online payment (e)
                if(result.Execute())
                {
                    if(result.Read())
                    {
                        MessageBox.Show("Conflict with OR Range.");
                        return;
                    }
                }
                result.Close();
            }

            for (int i = int.Parse(txtFrom.Text.Trim()); i <= int.Parse(txtTo.Text.Trim()); i++)
            {
                result.Query = "select * from or_assigned where from_or_no <= '" + i + "' and to_or_no >= '" + i + "'";
                // RMC 20141203 corrected error in online payment (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    result.Query += " and or_series = '" + txtSeries.Text.Trim() + "'"; //MCR 20140820 added or_series
                // RMC 20141203 corrected error in online payment (e)
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("OR is currently assigned to another teller.");
                        return;
                    }
                }
                result.Close();
            }

            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")   // RMC 20141203 corrected error in online payment, put rem
            {
                // SAVING TO OR_ASSIGNED TABLE
                result.Query = string.Format("insert into or_assigned values (:1, :2, :3, :4, :5, :6, '')");
                result.AddParameter(":1", txtTellerCode.Text.Trim());
                result.AddParameter(":2", txtSeries.Text.Trim());
                result.AddParameter(":3", txtFrom.Text.Trim());
                result.AddParameter(":4", txtTo.Text.Trim());
                result.AddParameter(":5", string.Format("{0:dd-MMM-yyyy}", dtSystemDate));
                result.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                // SAVING TO or_current TABLE
                result.Query = string.Format("insert into or_current values (:1, :2, :3, :4,:5, '')");
                result.AddParameter(":1", txtTellerCode.Text.Trim());
                result.AddParameter(":2", txtSeries.Text.Trim());
                result.AddParameter(":3", txtFrom.Text.Trim());
                result.AddParameter(":4", txtTo.Text.Trim());
                result.AddParameter(":5", txtFrom.Text.Trim());
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
            }// RMC 20141127 QA Tarlac ver (e)
            else
            {
                // SAVING TO OR_ASSIGNED TABLE
                result.Query = string.Format("insert into or_assigned values (:1, :2, :3, :4, :5, '')");
                result.AddParameter(":1", txtTellerCode.Text.Trim());
                result.AddParameter(":2", txtFrom.Text.Trim());
                result.AddParameter(":3", txtTo.Text.Trim());
                result.AddParameter(":4", string.Format("{0:dd-MMM-yyyy}", dtSystemDate));
                result.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                // SAVING TO or_current TABLE
                result.Query = string.Format("insert into or_current values (:1, :2, :3, :4, '')");
                result.AddParameter(":1", txtTellerCode.Text.Trim());
                result.AddParameter(":2", txtFrom.Text.Trim());
                result.AddParameter(":3", txtTo.Text.Trim());
                result.AddParameter(":4", txtFrom.Text.Trim());
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();
            }

            UpdateList();

            // AUDIT TRAIL
            string sObject = string.Empty;
            // RMC 20141203 corrected error in online payment (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
                sObject = txtTellerCode.Text + " " + txtSeries.Text + " " + txtFrom.Text + " " + txtTo.Text;
            else// RMC 20141203 corrected error in online payment (e) 
                sObject = txtTellerCode.Text + " " + txtFrom.Text + " " + txtTo.Text;  
            LibraryClass.AuditTrail("CDOR", "multiple table", sObject);
            // AUDIT TRAIL

            // RMC 20141203 corrected error in online payment (s)
            MessageBox.Show("O.R. range declared", "Declare/Return O.R.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string sTemp = txtTellerCode.Text.Trim() + txtFrom.Text.Trim() + txtTo.Text.Trim();
            CleanMe();
            // RMC 20141203 corrected error in online payment (e)

        }

        private void UpdateList()
        {
            OracleResultSet result = new OracleResultSet();
            dgvTellerOR.Rows.Clear();
            result.Query = "select * from or_assigned order by teller";
            if(result.Execute())
            {
                while(result.Read())
                {
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        dgvTellerOR.Rows.Add(result.GetString("teller").Trim(), result.GetString("or_series").Trim(), result.GetInt("from_or_no").ToString(), result.GetInt("to_or_no").ToString(), result.GetDateTime("trn_date").ToShortDateString(), result.GetString("assigned_by").Trim());
                    // RMC 20141127 QA Tarlac ver (e)
                    else
                        dgvTellerOR.Rows.Add(result.GetString("teller").Trim(), result.GetInt("from_or_no").ToString(), result.GetInt("to_or_no").ToString(), result.GetDateTime("trn_date").ToShortDateString(), result.GetString("assigned_by").Trim());
                }
            }
            result.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTeller();
        }

        private void SearchTeller()
        {

            bool bTellerFound = false;
            bool bTellerWithOR = false;

            string strButtonCaption = btnSearch.Text.Trim();

            if (strButtonCaption == "Search")
            {
                btnSearch.Text = "Clear";
                if (txtTellerCode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Teller Code");
                    btnSearch.Text = "Search";
                    return;
                }
                else
                {
                    OracleResultSet result = new OracleResultSet();
                    result.Query = "select * from tellers where teller = '" + txtTellerCode.Text.Trim() + "'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            txtLName.Text = result.GetString("ln").Trim();
                            txtFName.Text = result.GetString("fn").Trim();
                            txtMi.Text = result.GetString("mi").Trim();
                            bTellerFound = true;
                        }
                        else
                        {
                            MessageBox.Show("Teller not found.");
                            bTellerFound = false;
                            return;
                        }
                    }
                    result.Close();

                    if (bTellerFound)
                    {
                        result.Query = "select * from or_assigned where teller = '" + txtTellerCode.Text.Trim() + "'";
                        if (result.Execute())
                        {
                            if (result.Read())
                            {
                                bTellerWithOR = true;
                            }
                            else
                            {
                                txtFrom.Enabled = true;
                                txtTo.Enabled = true;
                                btnDeclare.Enabled = true;
                                bTellerWithOR = false;
                                // RMC 20141127 QA Tarlac ver (s)
                                if (AppSettingsManager.GetConfigValue("56") == "Y")
                                    txtSeries.Enabled = true;
                                // RMC 20141127 QA Tarlac ver (e)
                            }
                        }
                        result.Close();

                        if (bTellerWithOR)
                        {
                            result.Query = "select * from or_current where teller = '" + txtTellerCode.Text.Trim() + "'";
                            if (result.Execute())
                            {
                                if (result.Read())
                                {
                                    txtFrom.Enabled = false;
                                    txtTo.Enabled = false;
                                    btnDeclare.Enabled = false;
                                    btnReturn.Enabled = true;
                                    txtFrom.Text = result.GetInt("FROM_OR_NO").ToString();
                                    txtTo.Text = result.GetInt("TO_OR_NO").ToString();
                                    txtCurrOr.Text = result.GetInt("CUR_OR_NO").ToString();
                                    // RMC 20141127 QA Tarlac ver (s)
                                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                                        txtSeries.Text = result.GetString("or_series");
                                    // RMC 20141127 QA Tarlac ver (e)
                                }
                            }
                            result.Close();
                        }
                        else
                        {
                            btnDeclare.Enabled = true;
                            btnReturn.Enabled = false;
                            // RMC 20141203 corrected error in online payment (s)
                            txtFrom.Text = string.Empty;
                            txtTo.Text = string.Empty;
                            txtCurrOr.Text = string.Empty;
                            txtSeries.Text = string.Empty;
                            // RMC 20141203 corrected error in online payment (e)
                        }
                    }
                }
            }
            else
            {
                CleanMe();
            }
        }

        private void CleanMe()
        {
            btnSearch.Text = "Search";
            txtTellerCode.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtMi.Text = string.Empty;
            txtFrom.Text = string.Empty;
            txtTo.Text = string.Empty;
            txtCurrOr.Text = string.Empty;
            btnReturn.Enabled = false;
            btnDeclare.Enabled = false;
            txtSeries.Text = string.Empty;  // RMC 20141127 QA Tarlac ver
            txtSeries.Enabled = false;  // RMC 20141203 corrected error in online payment
            UpdateList();
        }
        private void frmReturnDeclareOR_Load(object sender, EventArgs e)
        {
            // RMC 20141127 QA Tarlac ver (s)
            if (AppSettingsManager.GetConfigValue("56") == "Y")
            {
                this.txtSeries.Visible = true;
                this.lblSeries.Visible = true;

                this.txtFrom.Location = new System.Drawing.Point(102, 400);
                this.txtFrom.Size = new System.Drawing.Size(152, 22);
                this.lblFrom.Location = new System.Drawing.Point(166, 425);

                this.txtTo.Location = new System.Drawing.Point(269, 400);
                this.txtTo.Size = new System.Drawing.Size(152, 22);
                this.lblTo.Location = new System.Drawing.Point(333, 425);

                this.txtCurrOr.Location = new System.Drawing.Point(428, 400);
                this.txtCurrOr.Size = new System.Drawing.Size(152, 22);
                this.lblCurrOr.Location = new System.Drawing.Point(473, 425);
            }
            else
            {
                this.txtSeries.Visible = false;
                this.lblSeries.Visible = false;
                this.txtSeries.Text = "";   // RMC 20141203 corrected error in online payment 

                this.txtFrom.Location = new System.Drawing.Point(28, 400);
                this.txtFrom.Size = new System.Drawing.Size(168, 22);
                this.lblFrom.Location = new System.Drawing.Point(97, 425);

                this.txtTo.Location = new System.Drawing.Point(220, 400);
                this.txtTo.Size = new System.Drawing.Size(168, 22);
                this.lblTo.Location = new System.Drawing.Point(294, 425);

                this.txtCurrOr.Location = new System.Drawing.Point(412, 400);
                this.txtCurrOr.Size = new System.Drawing.Size(168, 22);
                this.lblCurrOr.Location = new System.Drawing.Point(466, 425);
            }
            // RMC 20141127 QA Tarlac ver (e)

            UpdateList();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DateTime dtSystemDate;
            string sCurrentDate = string.Empty;
            dtSystemDate = AppSettingsManager.GetSystemDate();
            sCurrentDate = string.Format("{0:MM/dd/yyyy}", dtSystemDate);

            if(MessageBox.Show("Are you sure you want to return these OR range?","Return OR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet result = new OracleResultSet();
                result.Query = "insert into or_assigned_hist select * from or_assigned where teller = '" + txtTellerCode.Text.Trim() + "'";
                if(result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                result.Query = "delete from or_assigned where teller = '" + txtTellerCode.Text.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                result.Query = "delete from or_current where teller = '" + txtTellerCode.Text.Trim() + "'";
                if (result.ExecuteNonQuery() != 0)
                {

                }
                result.Close();

                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")   // RMC 20141203 corrected error in online payment
                {
                    result.Query = "insert into or_returned values (:1, :2, :3, :4, :5, :6)";
                    result.AddParameter(":1", txtTellerCode.Text.Trim());
                    result.AddParameter(":2", txtSeries.Text.Trim());
                    result.AddParameter(":3", txtCurrOr.Text.Trim());
                    result.AddParameter(":4", txtTo.Text.Trim());
                    result.AddParameter(":5", dtSystemDate);
                    result.AddParameter(":6", AppSettingsManager.SystemUser.UserCode);
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }// RMC 20141127 QA Tarlac ver (e)
                else
                {
                    result.Query = "insert into or_returned values (:1, :2, :3, :4, :5)";
                    result.AddParameter(":1", txtTellerCode.Text.Trim());
                    result.AddParameter(":2", txtCurrOr.Text.Trim());
                    result.AddParameter(":3", txtTo.Text.Trim());
                    result.AddParameter(":4", dtSystemDate);
                    result.AddParameter(":5", AppSettingsManager.SystemUser.UserCode);
                    if (result.ExecuteNonQuery() != 0)
                    {

                    }
                    result.Close();
                }
                //CleanMe();
                // RMC 20141203 corrected error in online payment, put rem

                // AUDIT TRAIL
                string sObject = string.Empty;
                // RMC 20141127 QA Tarlac ver (s)
                if (AppSettingsManager.GetConfigValue("56") == "Y")
                    sObject = txtTellerCode.Text + " " + txtSeries.Text + " " + txtFrom.Text + " " + txtTo.Text;
                else// RMC 20141127 QA Tarlac ver (e)
                    sObject = txtTellerCode.Text + " " + txtFrom.Text + " " + txtTo.Text;
                LibraryClass.AuditTrail("CROR", "multiple table", sObject);
                // AUDIT TRAIL

                // RMC 20141203 corrected error in online payment (s)
                CleanMe();
                MessageBox.Show("O.R. range returned","Declare/Return O.R.",MessageBoxButtons.OK,MessageBoxIcon.Information);
                // RMC 20141203 corrected error in online payment (e)

            }
        }

        private void dgvTellerOR_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
//             if(e.RowIndex != -1)
//             {
//                 FillControl(e.RowIndex);
//             }
        }

        private void FillControl(int i)
        {
            FillData(dgvTellerOR.Rows[i].Cells[0].Value.ToString());
            //SearchTeller();
        }

        private void FillData(string strTellerCode)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from tellers where teller = '" + strTellerCode.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    txtTellerCode.Text = strTellerCode.Trim();
                    txtLName.Text = result.GetString("ln").Trim();
                    txtFName.Text = result.GetString("fn").Trim();
                    txtMi.Text = result.GetString("mi").Trim();
                }
            }
            result.Close();

            result.Query = "select * from or_current where teller = '" + strTellerCode.Trim() + "'";
            if(result.Execute())
            {
                if(result.Read())
                {
                    txtFrom.Text = result.GetInt("FROM_OR_NO").ToString();
                    txtTo.Text = result.GetInt("TO_OR_NO").ToString();
                    txtCurrOr.Text = result.GetInt("CUR_OR_NO").ToString();

                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        txtSeries.Text = result.GetString("or_series");
                    // RMC 20141127 QA Tarlac ver (e)

                    // RMC 20141203 corrected error in online payment (s)
                    btnDeclare.Enabled = false;
                    btnReturn.Enabled = true;
                    // RMC 20141203 corrected error in online payment (e)
                }
                else
                {
                    txtFrom.Text = string.Empty;
                    txtTo.Text = string.Empty;
                    txtCurrOr.Text = string.Empty;
                    // RMC 20141127 QA Tarlac ver (s)
                    if (AppSettingsManager.GetConfigValue("56") == "Y")
                        txtSeries.Text = string.Empty;
                    // RMC 20141127 QA Tarlac ver (e)

                    // RMC 20141203 corrected error in online payment (s)
                    btnDeclare.Enabled = true;
                    btnReturn.Enabled = false;
                    // RMC 20141203 corrected error in online payment (e)
                }
            }
            result.Close();
        }

        private void dgvTellerOR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FillData(dgvTellerOR.SelectedRows[0].Cells[0].Value.ToString());

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanMe();
        }

        private void txtFrom_TextChanged(object sender, EventArgs e)
        {

        }

    }
}