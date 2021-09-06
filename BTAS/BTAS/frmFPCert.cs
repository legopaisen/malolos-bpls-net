using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.Reports;

namespace BTAS
{
    public partial class frmFPCert : Form
    {
        public frmFPCert()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadData(string sBin, string sStartYear)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result1 = new OracleResultSet();

            result.Query = "delete from pay_hist_temp where bin = '" + sBin.Trim() + "'";
            if (result.ExecuteNonQuery() != 0)
            {

            }
            result.Close();
            string sFeesCode = string.Empty;
            string sDue = string.Empty;
            string sSurch = string.Empty;
            string sPen = string.Empty;
            string sTotal = string.Empty;
            double dTotalTax = 0;
            double dTotalFees = 0;
            double dTotalSurchPen = 0;
            double dTotalTotal = 0;
            double dDue = 0;
            double dSurch = 0;
            double dPen = 0;
            double dTotal = 0;
            result.Query = "select distinct(or_no), tax_year, or_date, payment_term, qtr_paid, bill_no from pay_hist where bin = '" + sBin.Trim() + "' and tax_year >= '" + sStartYear.Trim() + "' and data_mode <> 'UNP' order by tax_year asc, or_no asc, or_date asc";
            if (result.Execute())
            {
                while (result.Read())
                {
                    result1.Query = "select * from or_table where or_no = '" + result.GetString("or_no").Trim() + "' and qtr_paid = '" + result.GetString("qtr_paid").Trim() + "' and tax_year = '" + result.GetString("tax_year").Trim() + "'";
                    if (result1.Execute())
                    {
                        while (result1.Read())
                        {
                            sFeesCode = result1.GetString("fees_code").Trim();
                            /*
                            sDue = result1.GetString("fees_due").Trim();
                            sSurch = result1.GetString("fees_surch").Trim();
                            sPen = result1.GetString("fees_pen").Trim();
                            sTotal = result1.GetString("fees_amtdue").Trim();
                            double.TryParse(sDue, out dDue);
                            double.TryParse(sSurch, out dSurch);
                            double.TryParse(sPen, out dPen);
                            double.TryParse(sTotal, out dTotal);
                             */
                            dDue = result1.GetDouble("fees_due");
                            try
                            {
                                dSurch = result1.GetDouble("fees_surch");
                            }
                            catch (System.Exception ex)
                            {
                                dSurch = 0;
                            }

                            try
                            {
                                dPen = result1.GetDouble("fees_pen");
                            }
                            catch (System.Exception ex)
                            {
                                dPen = 0;
                            }

                            try
                            {
                                dTotal = result1.GetDouble("fees_amtdue");
                            }
                            catch (System.Exception ex)
                            {
                                dTotal = 0;
                            }


                            if (sFeesCode.Substring(0, 1) == "B")
                                dTotalTax = dTotalTax + dDue;
                            else
                                dTotalFees = dTotalFees + dDue;

                            dTotalSurchPen = dTotalSurchPen + dSurch + dPen;
                            dTotalTotal = dTotalTotal + dTotal;
                        }
                    }
                    result1.Close();


                    //sTax = string.Format("{0:#,##0}", dTotalTax);
                    //sFees = string.Format("{0:#,##0}", dTotalFees);
                    //sSurchPen = string.Format("{0:#,##0}", dTotalSurchPen);
                    //sTotal = string.Format("{0:#,##0}", dTotalTotal);

                    // insert into pay_temp_hist
                    try
                    {
                        result1.Query = "insert into pay_hist_temp values(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)";
                        result1.AddParameter(":1", sBin.Trim());
                        result1.AddParameter(":2", result.GetString("tax_year").Trim());
                        result1.AddParameter(":3", result.GetString("or_no").Trim());
                        result1.AddParameter(":4", result.GetDateTime("or_date"));
                        result1.AddParameter(":5", result.GetString("payment_term").Trim());
                        result1.AddParameter(":6", result.GetString("qtr_paid").Trim());
                        result1.AddParameter(":7", dTotalTax);
                        result1.AddParameter(":8", dTotalFees);
                        result1.AddParameter(":9", dTotalSurchPen);
                        result1.AddParameter(":10", dTotalTotal);
                        result1.AddParameter(":11", result.GetString("bill_no").Trim()); //MCR 20210610
                        if (result1.ExecuteNonQuery() != 0)
                        {

                        }
                        result1.Close();
                    }
                    catch (Exception e)
                    {

                    }
                    dTotalTax = 0;
                    dTotalFees = 0;
                    dTotalSurchPen = 0;
                    dTotalTotal = 0;

                }

            }
            result.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                btnSearch.Text = "Clear";
                string sBin = string.Empty;
                if (txtBINYr.Text.Trim() == string.Empty && txtBINSerial.Text.Trim() == string.Empty)
                {
                    frmSearchBusiness fSearch = new frmSearchBusiness();
                    fSearch.ShowDialog();
                    sBin = fSearch.sBIN;
                }
                else
                    sBin = txtLGUCode.Text.Trim() + "-" + txtDISTCode.Text.Trim() + "-" + txtBINYr.Text.Trim() + "-" + txtBINSerial.Text.Trim();

                LoadInfo(sBin.Trim());
                LoadTaxYear(sBin.Trim());

                if (txtBNSName.Text.Trim() != "")
                {
                    Addlpnl.Visible = true;

                    foreach (object a in this.Addlpnl.Controls)
                        if (a is TextBox)
                            ((TextBox)a).Text = "";
                }
            }
            else
            {
                btnSearch.Text = "Search";
                Addlpnl.Visible = false;
                txtBINYr.Text = string.Empty;
                txtBINSerial.Text = string.Empty;
                txtBNSAdd.Text = string.Empty;
                txtBNSName.Text = string.Empty;
                txtBNSOwner.Text = string.Empty;
                txtBINYr.Focus();
            }
        }

        private void LoadTaxYear(string sBin)
        {
            cboTaxYear.Items.Clear();
            cboTaxYear.Text = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct tax_year from pay_hist where bin = '" + sBin + "'";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    cboTaxYear.Items.Add(pSet.GetString("tax_year"));
                }
            pSet.Close();

            if (cboTaxYear.Items.Count == 1)
                cboTaxYear.SelectedIndex = 0;
        }

        private void LoadInfo(string sBin)
        {
            txtBNSName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBNSAdd.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtBNSOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetBnsOwnCode(sBin));
        }

        private void frmRetCert_Load(object sender, EventArgs e)
        {
            txtBINYr.Focus();
            txtLGUCode.Text = ConfigurationAttributes.LGUCode;
            txtDISTCode.Text = ConfigurationAttributes.DistCode;
            dtpORDate.Value = AppSettingsManager.GetSystemDate();
            dtpIssued.Value = AppSettingsManager.GetSystemDate();
        }

        private void txtBINYr_Leave(object sender, EventArgs e)
        {
            txtBINSerial.Focus();
        }

        private void txtBINSerial_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtBINSerial.Text.Trim() == "")
                    return;

                txtBINSerial.Text = string.Format("{0:0000000}", int.Parse(txtBINSerial.Text.Trim()));
            }
            catch
            {
                MessageBox.Show("Invalid BIN.");
                return;
            }
        }

        private void txtBINYr_TextChanged(object sender, EventArgs e)
        {
            if (txtBINYr.Text.Length == 4)
                txtBINSerial.Focus();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (rdoQtr.Checked == false && rdoYear.Checked == false)
            {
                MessageBox.Show("Select a type of report"); return;
            }

            if (txtORNo.Text.Trim() == "")
            {
                MessageBox.Show("O.R. No. is required"); return;
            }

            if (txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("Amount is required."); return;
            }

            string sBin = string.Empty;
            sBin = txtLGUCode.Text.Trim() + "-" + txtDISTCode.Text.Trim() + "-" + txtBINYr.Text.Trim() + "-" + txtBINSerial.Text.Trim();

            if (rdoYear.Checked == true)
            {
                if (cboTaxYear.Text.Trim() == "")
                {
                    MessageBox.Show("TaxYear is required."); return;
                }

                if (IsFullPaidBns(sBin))
                {
                    LoadData(sBin.Trim(), txtBINYr.Text.Trim());

                    ReportClass rClass = new ReportClass();
                    rClass.sOrNo = txtORNo.Text;
                    if (chkIssued.Checked == true)
                        rClass.sAcceptedDate = dtpIssued.Value.ToShortDateString();
                    rClass.sOrDate = dtpORDate.Value.ToShortDateString();
                    rClass.sOrAmt = txtAmount.Text.Trim();
                    rClass.sFPTaxYear = cboTaxYear.Text.Trim();
                    rClass.FPCert(sBin, 0);
                    rClass.PreviewDocu();
                }
                else
                {
                    MessageBox.Show("Selected business is not yet fully paid.");
                    return;
                }
            }
            else if (rdoQtr.Checked == true)
            {
                if (isPaidBns(sBin))
                {
                    LoadData(sBin.Trim(), txtBINYr.Text.Trim());

                    ReportClass rClass = new ReportClass();
                    rClass.sOrNo = txtORNo.Text;
                    if (chkIssued.Checked == true)
                        rClass.sAcceptedDate = dtpIssued.Value.ToShortDateString();
                    rClass.sOrDate = dtpORDate.Value.ToShortDateString();
                    rClass.sOrAmt = txtAmount.Text.Trim();
                    rClass.sFPTaxYear = AppSettingsManager.GetSystemDate().Year.ToString();
                    rClass.FPCert(sBin, 1);
                    rClass.PreviewDocu();
                }
                else
                {
                    MessageBox.Show("No records found.");
                    return;
                }
            }
        }

        private bool isPaidBns(string sBin)
        {
            bool blnResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct * from pay_hist where bin = :1 and tax_year = :2 and data_mode <> 'UNP' and qtr_paid in ('4','3','2','1') order by tax_year desc, or_date desc, qtr_paid desc";
            result.AddParameter(":1", sBin.Trim());
            result.AddParameter(":2", AppSettingsManager.GetSystemDate().Year);
            if (result.Execute())
            {
                if (result.Read())
                    blnResult = true;
            }
            result.Close();
            return blnResult;
        }

        private bool IsFullPaidBns(string sBin)
        {
            bool blnResult = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = "Select bin,qtr_paid,tax_year,or_no From Pay_Hist Where bin = :1 and tax_year = :2 and Qtr_Paid In ('F','4','3','2') order by qtr_paid desc";
            result.AddParameter(":1", sBin.Trim());
            result.AddParameter(":2", cboTaxYear.Text.Trim());
            if (result.Execute())
            {
                while (result.Read())
                {
                    if (result.GetString("qtr_paid") == "4" || result.GetString("qtr_paid") == "F")
                    {
                        blnResult = true;
                        break;
                    }
                    else if (result.GetString("qtr_paid") == "3")
                    {
                        if (Convert.ToInt32(GetNoOfQtr(result.GetString("or_no"))) >= 2)
                        {
                            blnResult = true;
                            break;
                        }
                    }
                    else if (result.GetString("qtr_paid") == "2")
                    {
                        if (Convert.ToInt32(GetNoOfQtr(result.GetString("or_no"))) >= 3)
                        {
                            blnResult = true;
                            break;
                        }
                    }
                }
            }
            result.Close();
            return blnResult;
        }

        private string GetNoOfQtr(string p_sOrNo)
        {
            // RMC 20130109 display no. of qtr in payment hist
            OracleResultSet pSet = new OracleResultSet();
            string sNoOfQtr = string.Empty;
            string sQtr = string.Empty;


            pSet.Query = "select distinct * from pay_hist where or_no = '" + p_sOrNo + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sNoOfQtr = pSet.GetString("no_of_qtr").Trim();
                    sQtr = pSet.GetString("qtr_paid").Trim();

                    if (sQtr == "F")
                        sNoOfQtr = "4";
                    else
                    {
                        if (sNoOfQtr == "")
                            sNoOfQtr = "1";
                    }
                }
            }
            pSet.Close();

            return sNoOfQtr;
        }

        private void dtpORDate_Leave(object sender, EventArgs e)
        {
            dtpChecker(dtpORDate);
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        private void txtORNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar)
            //   && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
        }

        private void cboTaxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void chkIssued_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIssued.Checked == true)
                dtpIssued.Enabled = true;
            else
                dtpIssued.Enabled = false;
        }

        private void dtpIssued_Leave(object sender, EventArgs e)
        {
            dtpChecker(dtpIssued);
        }

        private void dtpChecker(DateTimePicker dtp)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtp.Value.Date > cdtToday.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtp.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void rdoYear_CheckedChanged(object sender, EventArgs e)
        {
            cboTaxYear.Enabled = true;
        }

        private void rdoQtr_CheckedChanged(object sender, EventArgs e)
        {
            cboTaxYear.Enabled = false;
            cboTaxYear.Text = "";
        }
    }
}