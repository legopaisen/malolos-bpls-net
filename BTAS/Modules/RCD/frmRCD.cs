//Getting of Denominations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.RCD
{
    public partial class frmRCD : Form
    {
        public frmRCD()
        {
            InitializeComponent();
        }

        OracleResultSet m_Set = new OracleResultSet();
        string m_sUser = "";
        List<string> m_sFormType = new List<string>();
        List<string> m_sORFr = new List<string>();
        List<string> m_sORTo = new List<string>();
        List<int> m_iORNo = new List<int>();
        List<double> m_dTotalCollection = new List<double>();

        private void Trail(OracleResultSet RSet, string sUser, string sModCode, string sTable, string sObject)
        {
            if (AuditTrail.InsertTrail(sModCode, sTable, sObject) == 0)
            {
                m_Set.Rollback();
                m_Set.Close();
                MessageBox.Show("Failed to insert audit trail.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void frmRCD_Load(object sender, EventArgs e)
        {
            //this is for testing use only
            dgViewCoin.Enabled = true;
            dgViewPaper.Enabled = true;
            btnGenerate.Enabled = true;
            //this is for testing use only

            try { m_sUser = AppSettingsManager.SystemUser.UserCode; }
            catch { m_sUser = "SYS_PROG"; }

            dgViewPaper.Rows.Add("Quantity", "Amount (Paper)");
            dgViewCoin.Rows.Add("Quantity", "Amount (Coin)");

            for (int i = 0; i != 6; i++)
                dgViewPaper.Rows.Add("", "");

            for (int i = 0; i != 7; i++)
                dgViewCoin.Rows.Add("", "");

            dgViewPaper.Rows[0].HeaderCell.Value = "Denominations";
            dgViewPaper.Rows[1].HeaderCell.Value = "1,000.00";
            dgViewPaper.Rows[2].HeaderCell.Value = "500.00";
            dgViewPaper.Rows[3].HeaderCell.Value = "200.00";
            dgViewPaper.Rows[4].HeaderCell.Value = "100.00";
            dgViewPaper.Rows[5].HeaderCell.Value = "50.00";
            dgViewPaper.Rows[6].HeaderCell.Value = "20.00";

            dgViewCoin.Rows[0].HeaderCell.Value = "Denominations";
            dgViewCoin.Rows[1].HeaderCell.Value = "10.00";
            dgViewCoin.Rows[2].HeaderCell.Value = "5.00";
            dgViewCoin.Rows[3].HeaderCell.Value = "1.00";
            dgViewCoin.Rows[4].HeaderCell.Value = ".50";
            dgViewCoin.Rows[5].HeaderCell.Value = ".10";
            dgViewCoin.Rows[6].HeaderCell.Value = ".05";
            dgViewCoin.Rows[7].HeaderCell.Value = ".01";
            LoadTeller();

        }

        private void LoadTeller()
        {
            //OracleResultSet result = new OracleResultSet();
            //result.Query = "SELECT DISTINCT TELLER FROM PAYMENT WHERE TRN_TYPE <> 'POS'"; // AST 20140122 Added TRN_TYPE <> 'POS'
            //if (result.Execute())
            //{
            //    cboTeller.Items.Clear();
            //    while (result.Read())
            //    {
            //        cboTeller.Items.Add(result.GetString(0).Trim());
            //    }
            //}
            //result.Close();

            //// For testing AST 20140122
            //result.Query = "select distinct collector from dct_used where collector not in (SELECT DISTINCT TELLER FROM PAYMENT where trn_type <> 'POS')";
            //if (result.Execute())
            //{
            //    while (result.Read())
            //    {
            //        cboTeller.Items.Add(result.GetString(0).Trim());
            //    }
            //}
            //result.Close();
            //// For testing

            OracleResultSet result = new OracleResultSet();
            result.Query = "select teller from tellers order by teller";
            if (result.Execute())
            {
                cboTeller.Items.Clear();
                while (result.Read())
                {
                    cboTeller.Items.Add(result.GetString("teller"));
                }
            }
            result.Close();
        }

        private void dgViewPaper_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
                dgViewPaper.Rows[0].Cells[0].Value = "Quantity";
            double dValue = 0;
            for (int i = 1; i != 7; i++)
            {
                try
                {
                    dValue += Convert.ToDouble(dgViewPaper.Rows[i].Cells[1].Value.ToString());
                }
                catch { }
            }
            txtTotalPaper.Text = string.Format("{0: #,###.00}", dValue);
            double dGrandTotal = 0;
            try
            {
                dGrandTotal = Convert.ToDouble(txtTotalCoin.Text) + Convert.ToDouble(txtTotalPaper.Text);
            }
            catch { }
            txtGrandTotal.Text = string.Format("{0: #,###.00}", dGrandTotal);
        }

        private void dgViewPaper_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgViewPaper.CurrentRow.Cells[1].Value = string.Format("{0: #,###.00}", Convert.ToInt32(dgViewPaper.CurrentRow.Cells[0].Value.ToString()) * Convert.ToDouble(dgViewPaper.Rows[e.RowIndex].HeaderCell.Value));
            }
            catch
            {
                if (dgViewPaper.CurrentRow.Index != 0)
                    dgViewPaper.CurrentRow.Cells[1].Value = null;
            }
        }

        private void dgViewCoin_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
                dgViewCoin.Rows[0].Cells[0].Value = "Quantity";
            double dValue = 0;
            for (int i = 1; i != 8; i++)
            {
                try
                {
                    dValue += Convert.ToDouble(dgViewCoin.Rows[i].Cells[1].Value.ToString());
                }
                catch { }
            }
            txtTotalCoin.Text = string.Format("{0: #,###.00}", dValue);
            double dGrandTotal = 0;
            try
            {
                double dCoin = Convert.ToDouble(txtTotalCoin.Text);
                double dPaper = Convert.ToDouble(txtTotalPaper.Text);
                dGrandTotal = dCoin + dPaper;
            }
            catch { }
            txtGrandTotal.Text = string.Format("{0: #,###.00}", dGrandTotal);
        }

        private void dgViewCoin_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgViewCoin.CurrentRow.Cells[1].Value = string.Format("{0: #,###.00}", Convert.ToInt32(dgViewCoin.CurrentRow.Cells[0].Value.ToString()) * Convert.ToDouble(dgViewCoin.Rows[e.RowIndex].HeaderCell.Value));
            }
            catch
            {
                if (dgViewCoin.CurrentRow.Index != 0)
                    dgViewCoin.CurrentRow.Cells[1].Value = null;
            }
        }

        private void dgViewPaper_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 || e.RowIndex == 0 && e.ColumnIndex == 1 || e.ColumnIndex == 1)
                dgViewPaper.ReadOnly = true;
            else
                dgViewPaper.ReadOnly = false;
        }

        private void dgViewCoin_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 || e.RowIndex == 0 && e.ColumnIndex == 1 || e.ColumnIndex == 1)
                dgViewCoin.ReadOnly = true;
            else
                dgViewCoin.ReadOnly = false;
        }

        private void dgViewPaper_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgViewPaper.CurrentCell.ColumnIndex == 0) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void dgViewCoin_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dgViewCoin.CurrentCell.ColumnIndex == 0) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cboTeller_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTeller.SelectedIndex < 0)
                return;

            string strTeller = cboTeller.Text.Trim();
            double dDebit = 0;
            double dCredit = 0;
            double dTotalAmount = 0;
            string strORFr = "";
            string strORTo = "";
            string strCheckNo = "";
            string strCheckNoTmp = "";
            string strBank = "";
            double dCheckAmount = 0;
            double dAmount = 0;
            int iMinOR = 0;
            int iMaxOR = 0;
            string strMinOR = "";
            string strMaxOR = "";
            int iORQty = 0;
            string strTmpORTO = "";
            string sORSeries = "";
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();

            EmptyField();

            /*dCredit = frmReport.GetCredit(strTeller);
            dDebit = frmReport.GetDebit(strTeller);
            dTotalAmount = frmReport.GetTotalCashAmt(strTeller);

            //if (dCredit > 0)
            //   dDebit = dTotalAmount - dCredit;

            txtCredit.Text = string.Format("{0:#,##0.00}", dCredit);
            txtDebit.Text = string.Format("{0:#,##0.00}", dDebit);*/
            // RMC 20140909 Migration QA, put rem

            try
            {
                //MCR 20140820
                /*result.Query = "select or_assigned_hist.* from or_assigned_hist,pay_hist where pay_hist.or_no between or_assigned_hist.or_series||or_assigned_hist.from_or_no and or_assigned_hist.or_series||or_assigned_hist.to_or_no and or_assigned_hist.teller ";
                result.Query += string.Format("not in (select teller from rcd_remit where teller = '{0}' and or_no between or_assigned_hist.from_or_no and or_assigned_hist.to_or_no)", strTeller);
                result.Query += string.Format("and or_assigned_hist.teller = '{0}' order by or_assigned_hist.from_or_no asc", strTeller);*/

               /* result.Query = "select distinct * from or_assigned_hist where teller not in ";
                result.Query += string.Format("(select teller from rcd_remit where teller = '{0}' and or_no between or_assigned_hist.or_series||or_assigned_hist.from_or_no ", strTeller);
                result.Query += string.Format("and or_assigned_hist.or_series||or_assigned_hist.to_or_no ) and teller = '{0}' order by from_or_no asc ", strTeller);
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strORFr = result.GetInt("from_or_no").ToString();
                        strORTo = result.GetInt("to_or_no").ToString();
                        sORSeries = result.GetString("or_series").Trim(); //MCR 20140826

                        rs.Query = "select to_char(max(or_no)) as maxor, to_char(min(or_no)) as minor from or_used ";
                        rs.Query += string.Format("where or_no between '{0}' and '{1}' and teller = '{2}' and teller not in ", sORSeries + strORFr, sORSeries + strORTo, strTeller);
                        rs.Query += "(select teller from partial_remit where or_used.or_no between partial_remit.or_from and partial_remit.or_to and teller = '" + strTeller + "') ";
                        rs.Query += "having to_char(Max(or_no)) is not null"; //ADD MCR 20140626
                       
                        if (rs.Execute())
                            while (rs.Read())
                            {
                                if (strTmpORTO != strORTo)
                                {
                                    strMinOR = rs.GetString("minor").Trim();
                                    strMaxOR = rs.GetString("maxor").Trim();

                                    //MCR 20140826
                                    int.TryParse(strMinOR, out iMinOR);
                                    int.TryParse(strMaxOR, out iMaxOR);

                                    if (iMinOR == 0)
                                        iMinOR = Convert.ToInt32(strMinOR.Substring(2));
                                    if (iMaxOR == 0)
                                        iMaxOR = Convert.ToInt32(strMaxOR.Substring(2));

                                    iORQty = (iMaxOR - iMinOR) + 1;

                                    //dAmount = frmReport.GetAmount(strMinOR, strMaxOR); 
                                    dAmount = frmReport.GetCashAmount(strMinOR, strMaxOR, strTeller); //MCR 20140902

                                    frmReport.GetCheckAmount(strMinOR, strMaxOR, out strCheckNo, out strBank);

                                    if (strCheckNo != strCheckNoTmp && strCheckNo != "")
                                        dCheckAmount += frmReport.GetCheckAmount(strMinOR, strMaxOR, out strCheckNo, out strBank);
                                    strCheckNoTmp = strCheckNo;

                                }
                                strTmpORTO = strORTo;
                            }
                        rs.Close();
                    }
                }
                result.Close();
                */  // RMC 20140909 Migration QA, put rem

                dgViewCoin.Enabled = true;
                dgViewPaper.Enabled = true;
                btnGenerate.Enabled = false; //JARS 20171106

                // RMC 20140909 Migration QA (s)
                txtDebit.Text = string.Format("{0:#,###.00}", frmReport.GetDebit(strTeller));
                txtCredit.Text = string.Format("{0:#,###.00}", frmReport.GetCredit(strTeller));
                /*dAmount = frmReport.GetCashAmount(strTeller);
                dCheckAmount = frmReport.GetCheckAmount(strTeller,"New");*/
                dAmount = frmReport.GetCashAmount(strTeller, dtpDate.Text.ToString());  // RMC 20150106
                dCheckAmount = frmReport.GetCheckAmount(strTeller, "New", dtpDate.Text.ToString()); // RMC 20150106
                // RMC 20140909 Migration QA (e)

                txtCash.Text = string.Format("{0:#,###.00}", dAmount);
                txtCheck.Text = string.Format("{0:#,###.00}", dCheckAmount);
                txtTotalCollected.Text = string.Format("{0:#,###.00}", dAmount + dCheckAmount);
            }
            catch (Exception ex)
            {
                dgViewCoin.Enabled = false;
                dgViewPaper.Enabled = false;
                btnGenerate.Enabled = false;
                MessageBox.Show(ex.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void EmptyField()
        {
            foreach (Control ctrl in kryptonGroup1.Panel.Controls)
                if (ctrl is KryptonTextBox)
                    ctrl.Text = null;

            foreach (Control ctrl in kryptonGroup2.Panel.Controls)
                if (ctrl is KryptonTextBox)
                    ctrl.Text = null;

            dgViewPaper.Enabled = false;
            dgViewCoin.Enabled = false;
            txtGrandTotal.Text = "0";
            txtTotalCoin.Text = "0";
            txtTotalPaper.Text = "0";

            for (int i = 1; i != dgViewPaper.Rows.Count; i++)
            {
                dgViewPaper.Rows[i].Cells[0].Value = null;
                dgViewPaper.Rows[i].Cells[1].Value = null;
            }
            for (int i = 1; i != dgViewCoin.Rows.Count; i++)
            {
                dgViewCoin.Rows[i].Cells[0].Value = null;
                dgViewCoin.Rows[i].Cells[1].Value = null;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //Getting of Denominations(s)
            double dTotalCollected = 0;
            double dGrandTotal = 0;
            double dTotalCash = 0;
            double dTotalCheck = 0; //JARS 20170825
            double dTotalDebit = 0;
            double.TryParse(txtTotalCollected.Text, out dTotalCollected);
            double.TryParse(txtGrandTotal.Text, out dGrandTotal);
            double.TryParse(txtCash.Text, out dTotalCash); //MCR 20140714
            double.TryParse(txtCheck.Text, out dTotalCheck); //JARS 20170825
            double.TryParse(txtDebit.Text, out dTotalDebit);

            //if (dTotalCollected == 0)
            if (dTotalCollected == 0 && dTotalDebit == 0) //AFM 20210623
            {
                MessageBox.Show("No collection to be remitted.", "No Collection", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if ((dGrandTotal != dTotalCash) && (dTotalCheck != dGrandTotal)) //JARS 20170825 IMPROVED CONDITION
            {
                MessageBox.Show("Total Denomination must tally with total Cash/Check Collection", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from or_assigned where teller = '" + cboTeller.Text.Trim() + "'";
            if (result.Execute())
            {
                while (result.Read())
                {
                    MessageBox.Show("You have to return your O.R. before you can proceed.", "Return O.R.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            result.Close();
            //JARS 20171108 (S) PREVENT REMITTANCE IF RCD CODE IS NOT ENTERED IN TELLERS MODULE
            result.Query = "select * from tellers where teller = '"+ cboTeller.Text.Trim() +"'";    // RMC 20171110 changed teller_code to teller
            if(result.Execute())
            {
                if(result.Read())
                {
                    if(result.GetString("rcd_code") == "")
                    {
                        MessageBox.Show("Please enter RCD CODE for this teller before you can proceed.", "Tellers Module", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
            result.Close();
            //JARS 20171108 (E)

            List<string> lstDenomination = new List<string>();
            List<string> lstDenominationQty = new List<string>();
            List<string> lstDenominationAmt = new List<string>();

            //for (int i = 1; i != dgViewPaper.RowCount; i++)
            for (int i = 1; i != dgViewPaper.RowCount; i++) 
            {
                try
                {
                    if (dgViewPaper.Rows[i].Cells[0].Value != null) // JAA 20190307
                    {
                        if (dgViewPaper.Rows[i].Cells[0].Value.ToString() != "")
                            if (int.Parse(dgViewPaper.Rows[i].Cells[0].Value.ToString()) > 0)
                            {
                                lstDenomination.Add(dgViewPaper.Rows[i].HeaderCell.Value.ToString());
                                lstDenominationQty.Add(dgViewPaper.Rows[i].Cells[0].Value.ToString());
                                lstDenominationAmt.Add(dgViewPaper.Rows[i].Cells[1].Value.ToString());
                            }
                    }
                }
                catch { }
            }
            for (int i = 1; i != dgViewCoin.RowCount; i++)
            {
                try
                {
                    if (dgViewCoin.Rows[i].Cells[0].Value != null) // JAA 20190415
                    {
                        if (dgViewCoin.Rows[i].Cells[0].Value.ToString() != "") // JAA 20190415
                        {
                            if (int.Parse(dgViewCoin.Rows[i].Cells[0].Value.ToString()) > 0)
                            {
                                lstDenomination.Add(dgViewCoin.Rows[i].HeaderCell.Value.ToString());
                                lstDenominationQty.Add(dgViewCoin.Rows[i].Cells[0].Value.ToString());
                                lstDenominationAmt.Add(dgViewCoin.Rows[i].Cells[1].Value.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //Getting of Denominations(e)

            frmReport form = new frmReport();
            form.Switch = "New";
            form.TellerCode = cboTeller.Text;
            form.DateTrans = dtpDate.Value;
            //form.FormType = m_sFormType;
            //form.ORFrom = m_sORFr;
            //form.ORTo = m_sORTo;
            form.Denominations = lstDenomination; //Getting of Denominations
            form.DenominationsQty = lstDenominationQty; //Getting of Denominations
            form.DenominationsAmt = lstDenominationAmt;
            form.m_sOrDate = string.Format("{0:MM/dd/yyyy}", dtpDate.Text); // RMC 20150106
            form.ShowDialog();

            //try// remove try catch if this project already attach in main BPLS project
            //{
                AuditTrail.InsertTrail("RCD-G", "or_assigned_hist/or_used", "Generate Report");
                
                // RMC 20140909 Migration QA (s)
                string sRCDNo = form.RCDNo;

                //printing with generated RCD no
                if (sRCDNo.Trim() != "")
                {
                    form = new frmReport();
                    form.Switch = "Print-New";
                    form.RCDNo = sRCDNo;
                    form.TellerCode = cboTeller.Text;
                    form.ShowDialog();
                    EmptyField();
                }
                // RMC 20140909 Migration QA (e)
            //}
            //catch { }
        }

        private void lblGrandTotal_TextChanged(object sender, EventArgs e)
        {
            if (txtGrandTotal.Text != "0")
            {
                try
                {
                    if ((Convert.ToDouble(txtCash.Text) == Convert.ToDouble(txtGrandTotal.Text)) || (Convert.ToDouble(txtCheck.Text) == Convert.ToDouble(txtGrandTotal.Text)))
                        btnGenerate.Enabled = true;
                    else
                        btnGenerate.Enabled = false;
                }
                catch { }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboButtonNavigator_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(btnPrev))
                    cboTeller.SelectedIndex = cboTeller.SelectedIndex - 1;
                else if (sender.Equals(btnNext))
                    cboTeller.SelectedIndex = cboTeller.SelectedIndex + 1;
            }
            catch { }
        }

        private void dtpDate_Leave(object sender, EventArgs e)
        {
            // RMC 20150106
            
            string strTeller = cboTeller.Text.Trim();
            double dDebit = 0;
            double dCredit = 0;
            double dTotalAmount = 0;
            string strORFr = "";
            string strORTo = "";
            string strCheckNo = "";
            string strCheckNoTmp = "";
            string strBank = "";
            double dCheckAmount = 0;
            double dAmount = 0;
            int iMinOR = 0;
            int iMaxOR = 0;
            string strMinOR = "";
            string strMaxOR = "";
            int iORQty = 0;
            string strTmpORTO = "";
            string sORSeries = "";
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rs = new OracleResultSet();
            string sOrDate = string.Empty;
            sOrDate = string.Format("{0:MM/dd/yyyy}", dtpDate.Text);

            EmptyField();
            try
            {
            dgViewCoin.Enabled = true;
            dgViewPaper.Enabled = true;

            // RMC 20140909 Migration QA (s)
            txtDebit.Text = string.Format("{0:#,###.00}", frmReport.GetDebit(strTeller));
            txtCredit.Text = string.Format("{0:#,###.00}", frmReport.GetCredit(strTeller));
            /*dAmount = frmReport.GetCashAmount(strTeller);
            dCheckAmount = frmReport.GetCheckAmount(strTeller,"New");*/
            dAmount = frmReport.GetCashAmount(strTeller, sOrDate);  // RMC 20150106
            dCheckAmount = frmReport.GetCheckAmount(strTeller, "New", sOrDate); // RMC 20150106
            // RMC 20140909 Migration QA (e)

            txtCash.Text = string.Format("{0:#,###.00}", dAmount);
            txtCheck.Text = string.Format("{0:#,###.00}", dCheckAmount);
            txtTotalCollected.Text = string.Format("{0:#,###.00}", dAmount + dCheckAmount);
        }
        catch (Exception ex)
        {
            dgViewCoin.Enabled = false;
            dgViewPaper.Enabled = false;
            btnGenerate.Enabled = false;
            MessageBox.Show(ex.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            // RMC 20150106
            dtpDate_Leave(sender, e);
        }

        private void txtTotalCollected_TextChanged(object sender, EventArgs e) //AFM 20200507 for remittance with check transactions only
        {
            if (txtTotalCollected.Text != "0")
            {
                try
                {
                    if ((Convert.ToDouble(txtCheck.Text) == Convert.ToDouble(txtTotalCollected.Text)) && Convert.ToDouble(txtCash.Text) == 0)
                        btnGenerate.Enabled = true;
                }
                catch { }
            }
        }
    }
}
