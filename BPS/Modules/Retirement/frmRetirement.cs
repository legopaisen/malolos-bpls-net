using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.BPLS.Billing;
using Amellar.Common.frmBns_Rec;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.Retirement
{
    public partial class frmRetirement : Form
    {
        frmSearchBusiness frmSearchBns = new frmSearchBusiness();
        private string m_sTaxYear = string.Empty;
        private bool m_boWaive = false;
        frmBilling BillingForm = new frmBilling();

        bool bToView = false; //View RetiredBns
        public DateTime m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin

        public frmRetirement()
        {
            InitializeComponent();
            bin2.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin2.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmRetirement_Load(object sender, EventArgs e)
        {
            m_boWaive = false;
            ClearControls();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {   
            if (btnSearch.Text == "Search")
            {
                bToView = false;
                if (bin2.txtTaxYear.Text != "" || bin2.txtBINSeries.Text != "")
                {
                    if (!TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "ADD", "ASS"))
                    {
                        LoadValues(bin2.GetBin());
                        m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                    }
                    else
                    {
                        bin2.txtTaxYear.Text = "";
                        bin2.txtBINSeries.Text = "";

                    }
                }
                else
                {
                    frmSearchBns = new frmSearchBusiness();
                    //frmSearchBns.ModuleCode = m_sFormStatus;

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        bin2.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        bin2.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();
                        if (!TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "ADD", "ASS"))
                        {
                            LoadValues(bin2.GetBin());
                            m_dTransLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                        }
                        else
                        {
                            bin2.txtTaxYear.Text = "";
                            bin2.txtBINSeries.Text = "";
                        }
                    }
                }
            }
            else
            {
                if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS")) // RMC 20131229
                {
                }

                ClearControls();
                btnSearch.Text = "Search";
            }
        }

        private void LoadValues(string strBin)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            string sBnsTaxYear = "";
            string sBnsStat = "";
            string sTaxYear = "";
            string sBnsCode = "";
            string sBnsDesc = "";
            string sRETOwnCode = "";

            chkFull.Checked = true;

            pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsTaxYear = pSet.GetString("tax_year").Trim();

                    if (sBnsTaxYear == "")
                    {
                        MessageBox.Show("Data error, no business tax year.", "Retirement",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Record not found.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    btnSearch.Text = "Clear";   // RMC 20110801
                    return;
                }
            }
            pSet.Close();



            pSet.Query = string.Format("select * from business_que where bin = '{0}' and bns_stat <> 'RET'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsStat = pSet.GetString("bns_stat").Trim();
                    sTaxYear = pSet.GetString("tax_year").Trim();

                    if (sBnsTaxYear == "")
                        sBnsTaxYear = sTaxYear;

                    if (MessageBox.Show("Has Pending '" + sBnsStat + "' Application? \nDo you want to delete the pending application then proceed?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show("Proceed to your pending application.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        // RMC 20150105 added retirement no billing rollback module (s)
                        if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS"))
                        {
                        }
                        ClearControls();
                        // RMC 20150105 added retirement no billing rollback module (e)

                        return;
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("delete from business_que where bin = '{0}'", strBin);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year > '{1}' and bns_stat = '{2}'", strBin, sBnsTaxYear, sBnsStat);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from btax where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from bill_no where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from addl where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from fire_tax where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        if (sTaxYear != "")
                        {
                            pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }

                            pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }
                        }

                        pSet.Query = string.Format("delete from other_info where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from other_info_dec where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = string.Format("delete from billing_tagging where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = string.Format("delete from reg_table where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}' and tax_year > '{1}'", strBin, sBnsTaxYear);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObject = strBin + "/" + sTaxYear;
                        if (AuditTrail.InsertTrail("AACA", "business_que", StringUtilities.HandleApostrophe(strObject)) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            int iAns = 0;
            pSet.Query = string.Format("select max(tax_year) as tax_year, max(qtr_paid) as qtr_paid from pay_hist where bin = '{0}'", strBin);  // RMC 20131229 ADDED max(qtr_paid)
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_sTaxYear = pSet.GetString("tax_year").Trim();

                    //MCR 20150127 (s)
                    if (m_sTaxYear.Trim() == "")
                    {
                        MessageBox.Show("No Payment History", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //MCR 20150127 (e)

                    if (Convert.ToInt32(m_sTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                    {
                        string sCurrDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        if ((pSet.GetString("qtr_paid") == "F" || pSet.GetString("qtr_paid") == "4") && AppSettingsManager.GetQtr(sCurrDate) == "1")  // RMC 20131229 adjustments
                        { }
                        else
                        {
                            //if (MessageBox.Show("Warning: Previous year delinquent exists, Waive delinquencies?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            if (MessageBox.Show("Waive current tax dues?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20131229 adjustments
                            {
                                iAns = 1;
                            }
                        }
                    }
                    else
                        m_sTaxYear = "";
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from business_que where bin = '{0}'", strBin);
            if (pSet.ExecuteNonQuery() == 0)
            {
                if (pSet.Read())
                {
                    m_sTaxYear = pSet.GetString("tax_year").Trim();
                    if (Convert.ToInt32(m_sTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                    {
                        //if (MessageBox.Show("Warning: Previous year delinquent exists, Waive delinquencies?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        if (MessageBox.Show("Waive current tax dues?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // RMC 20131229 adjustments
                        {
                            iAns = 1;
                        }
                    }
                    else
                        m_sTaxYear = "";
                }
            }
            pSet.Close();

            if (m_sTaxYear != "")
            {
                if (iAns == 0)
                {
                    chkWaiveTaxAndFees.Checked = false;
                }
                else
                {
                    chkWaiveTaxAndFees.Checked = true;

                    // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (s)
                    pSet.Query = string.Format("insert into taxdues_hist select bin, tax_year, qtr_to_pay, bns_code_main, tax_code, amount, 'X' from taxdues where bin = '{0}' and tax_year = '{1}'", strBin, m_sTaxYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                    // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (e)

                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and tax_year = '{1}'", strBin, m_sTaxYear);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                }
            }

            int iRow = 0;
            pSet.Query = string.Format("select * from businesses where bin  = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsStat = pSet.GetString("bns_stat").Trim();
                    if (sBnsStat == "RET")
                    {
                        //MessageBox.Show("Business already retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // RMC 20150105 added retirement no billing rollback module (s)
                        //if (MessageBox.Show("Business already retired.\nDo you wish to reverse retirement?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //{
                        //     btnRollBack.Visible = true;
                        //}// RMC 20150105 added retirement no billing rollback module (e)
                        //else

                        // RMC 20170321 added validation if with queued retirement application (s)
                        OracleResultSet pCheck = new OracleResultSet();
                        pCheck.Query = "select * from retired_bns_temp where bin = '" + strBin + "'";
                        if (pCheck.Execute())
                        {
                            if (pCheck.Read())
                            {
                            }
                            else
                            {
                        // RMC 20170321 added validation if with queued retirement application (e)

                                // MCR 20150107 (s)
                                if (MessageBox.Show("Business already retired.\nDo you wish to view the values?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    bToView = true;
                                }
                                else
                                {
                                    if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS"))
                                    {
                                    }
                                    // MCR 20150107 (e)
                                    pSet.Close();
                                    bin2.txtTaxYear.Text = "";
                                    bin2.txtBINSeries.Text = "";
                                    RetirementType(true, false);
                                    btnRollBack.Visible = false;
                                    return;
                                }
                            }
                        }
                        pCheck.Close(); // RMC 20170321 added validation if with queued retirement application
                    }

                    txtBnsName.Text = pSet.GetString("bns_nm").Trim();
                    sTaxYear = pSet.GetString("tax_year").Trim();

                    sRETOwnCode = pSet.GetString("own_code");
                    txtName.Text = AppSettingsManager.GetBnsOwner(sRETOwnCode);

                    sBnsCode = pSet.GetString("bns_code");
                    m_sTaxYear = sTaxYear;

                    txtBnsAdd.Text = AppSettingsManager.GetBnsAddress(strBin);
                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);

                    if (chkFull.CheckState.ToString() == "Checked")
                    {
                        dgvList.Rows.Add("");
                        dgvList[0, iRow].Value = true;
                        dgvList[1, iRow].Value = sBnsCode;
                        dgvList[2, iRow].Value = sBnsDesc;
                        dgvList[3, iRow].Value = "0.00";
                    }
                    else
                    {
                        dgvList.Rows.Add("");
                        dgvList[0, iRow].Value = false;
                        dgvList[1, iRow].Value = sBnsCode;
                        dgvList[2, iRow].Value = sBnsDesc;
                        dgvList[3, iRow].Value = "0.00";
                    }

                    iRow++;

                    // RMC 20170120 display retired addl bns in retirement module if bin is already retired (s)
                    if (bToView)
                    {
                        pRec.Query = "select * from retired_bns where bin = '" + strBin + "' and tax_year = '" + sTaxYear + "' and main = 'N'";
                    }// RMC 20170120 display retired addl bns in retirement module if bin is already retired (e)
                    else
                    {
                        pRec.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", strBin, sTaxYear);
                    }
                    if (pRec.Execute())
                    {
                        while (pRec.Read())
                        {
                            sBnsCode = pRec.GetString("bns_code_main");
                            sBnsStat = pRec.GetString("bns_stat");
                            sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);

                            if (chkFull.CheckState.ToString() == "Checked")
                            {
                                dgvList.Rows.Add("");
                                dgvList[0, iRow].Value = true;
                                dgvList[1, iRow].Value = sBnsCode;
                                dgvList[2, iRow].Value = sBnsDesc;
                                dgvList[3, iRow].Value = "0.00";
                            }
                            else
                            {
                                dgvList.Rows.Add("");
                                dgvList[0, iRow].Value = false;
                                dgvList[1, iRow].Value = sBnsCode;
                                dgvList[2, iRow].Value = sBnsDesc;
                                dgvList[3, iRow].Value = "0.00";
                            }
                            iRow++;

                        }
                    }
                    pRec.Close();

                }
            }
            pSet.Close();

            btnSearch.Text = "Clear";
            txtAppNo.ReadOnly = false;
            //MCR 20140107 (s)
            if (bToView)
            {
                pSet.Query = string.Format("select * from retired_bns where bin = '{0}'", strBin);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtAppNo.Text = pSet.GetString("app_no").Trim();
                        txtReason.Text = pSet.GetString("memoranda").Trim();
                        btnCancelApp.Enabled = false;
	                    dtpDate.Value = pSet.GetDateTime("apprvd_date");    // RMC 20150112 mods in retirement billing
                        txtAppNo.ReadOnly = true;
                        txtReason.ReadOnly = true;
                        btnSave.Enabled = false;
                        pSet.Close();
                    }
                }
            }
            //MCR 20140107 (e)
            else
            {
                pSet.Query = string.Format("select * from retired_bns_temp where bin = '{0}'", strBin);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        txtAppNo.Text = pSet.GetString("app_no").Trim();
                        txtReason.Text = pSet.GetString("memoranda").Trim();
                        btnCancelApp.Enabled = true;

                        // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (s)
                        pSet.Close();

                        pSet.Query = "select * from waive_tbl where bin = '" + strBin + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                if (pSet.GetString("tax_fees") == "Y")
                                    chkWaiveTaxAndFees.Checked = true;
                            }
                        }
                        // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (e)
                    }
                }
            }
            pSet.Close();

            LoadViolation();    // RMC 20171115 Added capturing of violation as reason of retirement in retirement module

            dgvList.EndEdit();
        }

        private void RetirementType(bool bFull, bool bPartial)
        {
            if(bFull)
            {
                chkFull.Checked = true;
                chkPartial.Checked = false;
            }
            else
            {
                chkFull.Checked = false;
                chkPartial.Checked = true;
            }
        }

        private void chkFull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFull.CheckState.ToString() == "Checked")
            {
                chkPartial.Checked = false;
                dgvList.ReadOnly = true;

                for (int i = 0; i <= dgvList.Rows.Count - 1; i++)
                {
                    dgvList[0, i].Value = true;
                }
            }
        }

        private void chkPartial_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPartial.CheckState.ToString() == "Checked")
            {
                chkFull.Checked = false;
                dgvList.ReadOnly = false;

                for (int i = 0; i <= dgvList.Rows.Count - 1; i++)
                {
                    dgvList[0, i].Value = false;
                }
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (chkFull.CheckState.ToString() == "Checked")
                    dgvList.ReadOnly = true;
                else
                    dgvList.ReadOnly = false;
            }
            else if (e.ColumnIndex == 3)
            {
                dgvList.ReadOnly = false;
            }
            else
                dgvList.ReadOnly = true;
        }

        private void ClearControls()
        {
            bin2.txtTaxYear.Text = "";
            bin2.txtBINSeries.Text = "";
            txtBnsName.Text = "";
            txtBnsAdd.Text = "";
            txtName.Text = "";
            txtReason.Text = "";
            txtAppNo.Text = "";
            dgvList.Columns.Clear();
            chkFull.Checked = true;
            btnSave.Enabled = true;
            chkBilling.Checked = false;
            chkWaiveTaxAndFees.Checked = false;
            m_boWaive = false;
            

            DataGridViewCheckBoxColumn column1 = new DataGridViewCheckBoxColumn();
            {
                column1.HeaderText = "xxx";
            }

            dgvList.Columns.Insert(0, column1);
            dgvList.Columns.Add("BNSCODE", "CODE");
            dgvList.Columns.Add("BNS", "BUSINESSES");
            dgvList.Columns.Add("GROSS", "GROSS");
            dgvList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[0].Width = 50;
            dgvList.Columns[1].Width = 70;
            dgvList.Columns[2].Width = 200;
            dgvList.Columns[3].Width = 100;
            dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            bin2.txtTaxYear.Focus();
            this.ActiveControl = bin2.txtTaxYear;

            dgvList.ReadOnly = false;

            // RMC 20150105 added retirement no billing rollback module (s)
            btnSearch.Text = "Search";  
            btnRollBack.Visible = false;
            // RMC 20150105 added retirement no billing rollback module (e)

            chkBilling.Checked = true;  // RMC 20150112 mods in retirement billing, to prevent retiring without billing
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            string strBin = string.Empty;
            string strBnsCode = string.Empty;
            string strGross = string.Empty;
            string strRetNo = string.Empty;
            string strQtr = string.Empty;
            string strMain = string.Empty;
            strBin = bin2.GetBin();

            // RMC 20150112 mods in retirement billing (s)
            string sDateClosed = string.Format("{0:MM/dd/yyyy}", dtpDate.Value);
            string sCurrDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            if (bin2.txtTaxYear.Text == "" && bin2.txtBINSeries.Text == "")
            {
                MessageBox.Show("Enter BIN of the business to be retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (txtReason.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Reason for retirement is required.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (sDateClosed == sCurrDate)
            {
                if (MessageBox.Show("Date closed is set to current date.\nContinue?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            // RMC 20150112 mods in retirement billing (e)

            txtAppNo.Text = GetAppNoifExist(strBin); //MCR 20150213 Check APPNO if already Exist
            if (txtAppNo.Text == String.Empty)
                txtAppNo.Text = string.Format("{0}-{1:0000#}", DateTime.Now.Year, int.Parse(AppSettingsManager.GetretAppNo()));
            else if (txtAppNo.Text.Length < 10)
                txtAppNo.Text = string.Format("{0}-{1:0000#}", DateTime.Now.Year, int.Parse(AppSettingsManager.GetretAppNo()));


            if (txtAppNo.Text.ToString().Trim() == "" || txtAppNo.Text.ToString().Trim() == ".")
            {
                MessageBox.Show("Retirement Application Number is required.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20150112 mods in retirement billing (s)
            string sAsk = string.Empty;

            if (chkBilling.Checked == true)
                sAsk = "Do you really want to retire this business WITH BILLING?";
            else
                sAsk = "Do you really want to retire this business WITHOUT BILLING?";

            if (MessageBox.Show("" + sAsk + "", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            // RMC 20150112 mods in retirement billing (e)

            //if (MessageBox.Show("Do you really want to retire this business?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!Validates())
                    return;

                for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                {
                    strBnsCode = dgvList[1, iRow].Value.ToString().Trim();

                    pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}' and bns_code_main = '{1}' and bns_stat = 'RET'", strBin, strBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from retired_bns_temp where bin = '{0}' and bns_code_main = '{1}'", strBin, strBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from business_que where bin = '{0}' and bns_stat = 'RET'", strBin);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                    pSet.Query = string.Format("delete from taxdues where bin = '{0}' and due_state = 'X' and bns_code_main = '{1}'", strBin, strBnsCode);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }


                    /*pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and due_state = 'X' and bns_code_main = '{1}'",strBin,strBnsCode);
                    if(pSet.ExecuteNonQuery() == 0)
                    {
                    }*/
                    // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application
                }

                if (chkFull.Checked == true)	// Full Retirement
                {
                    if (chkBilling.Checked == false)
                    {
                        strRetNo = GenerateRetNo();

                        for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                        {
                            bool blnRetire = (bool)dgvList[0, iRow].Value;

                            if (blnRetire)
                            {
                                strBnsCode = dgvList[1, iRow].Value.ToString();
                                strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, iRow].Value.ToString()));

                                pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", strBin, strBnsCode);
                                if (pSet.ExecuteNonQuery() == 0)
                                {
                                }

                                if (iRow == 0)
                                    pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET','F',:5,'Y',:6,:7,' ',:8,'')";
                                    //pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET','F',:5,'Y',:6,:7,' ',:8)";
                                else
                                    pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET','F',:5,'N',:6,:7,' ',:8,'')";
                                    //pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET','F',:5,'N',:6,:7,' ',:8)";
                                pSet.AddParameter(":1", strBin);
                                pSet.AddParameter(":2", strBnsCode);
                                pSet.AddParameter(":3", strGross);
                                pSet.AddParameter(":4", ConfigurationAttributes.CurrentYear);
                                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(txtReason.Text.ToString().Trim()));
                                pSet.AddParameter(":6", dtpDate.Value);
                                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim()));
                                pSet.AddParameter(":8", strRetNo);
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                if (Convert.ToInt32(m_sTaxYear) == Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                                {
                                    /*pSet.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr)";
                                    pSet.Query += string.Format("select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                    */
                                    // RMC 20151005 mods in retirement module (s)
                                    pSet.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS)";
                                    pSet.Query += string.Format("select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                    // RMC 20151005 mods in retirement module (e)
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }

                                    pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }
                                }
                            }
                        }

                        strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, 0].Value.ToString()));

                        pSet.Query = string.Format("insert into buss_hist select * from businesses where bin ='{0}'", strBin);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = string.Format("update businesses set bns_stat = 'RET',tax_year = '{0}',gr_1 = '{1}' where bin ='{2}'", ConfigurationAttributes.CurrentYear, strGross, strBin);
                         if (pSet.ExecuteNonQuery() == 0)
                        { }
                         TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "AAE-NB", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                        if (AuditTrail.InsertTrail("AAE-NB", "business_que, businesses", StringUtilities.HandleApostrophe(strBin)) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Business has been retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string sDel = "", sWaiveTF = "";

                        if (chkWaiveTaxAndFees.Checked)
                            sWaiveTF = "Y";
                        else
                            sWaiveTF = "N";

                        if (m_boWaive)
                            sDel = "Y";
                        else
                            sDel = "N";

                        pSet.Query = string.Format("delete from waive_tbl where bin = '{0}'", strBin);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = "insert into waive_tbl values(:1,:2,:3,'F','F')";
                        pSet.AddParameter(":1", strBin.Trim());
                        pSet.AddParameter(":2", sWaiveTF.Trim());
                        pSet.AddParameter(":3", sDel.Trim());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        //if (Convert.ToInt32(ConfigurationAttributes.CurrentYear) == Convert.ToInt32(m_sTaxYear)) 
                        {
                            pSet.Query = string.Format("insert into business_que select * from businesses where bin = '{0}'", strBin);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            //pSet.Query = string.Format("update business_que set bns_stat = 'RET' where bin ='{0}'", strBin);
                            pSet.Query = string.Format("update business_que set bns_stat = 'RET',gr_1 = '" + dgvList[3, 0].Value + "' where bin ='{0}'", strBin); //AFM 20200312 get applied gross of main businesses
                            if (pSet.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20150108 (s)
                        if (Convert.ToInt32(m_sTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                        {
                            //pSet.Query = string.Format("update business_que set tax_year = tax_year + 1 where bin ='{0}'", strBin);   // RMC 20170301 corrections in retirement application
                            //pSet.Query = string.Format("update business_que set tax_year ='" + ConfigurationAttributes.CurrentYear + "' where bin ='{0}'", strBin);  	// RMC 20150112 mods in retirement billing
                            string sYear = string.Empty;
                            sYear = string.Format("{0:yyyy}", dtpDate.Value);
                            pSet.Query = "update business_que set tax_year = '" + sYear + "' where bin ='" + strBin + "' ";   // RMC 20170301 corrections in retirement application
                            if (pSet.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20150108 (e)

                        //m_sTaxYear = string.Format("%d",atoi(m_sTaxYear)+1);	//RTL 10262005 edit retirement

                        for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                        {
                            bool blnRetire = (bool)dgvList[0, iRow].Value;

                            if (blnRetire)
                            {
                                strBnsCode = dgvList[1, iRow].Value.ToString();
                                strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, iRow].Value.ToString()));

                                if (iRow == 0)
                                    pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET','F',:5,'Y',:6,:7,' ','')";
                                    //pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET','F',:5,'Y',:6,:7,' ')";
                                else
                                    pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET','F',:5,'N',:6,:7,' ','')";
                                    //pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET','F',:5,'N',:6,:7,' ')";
                                pSet.AddParameter(":1", strBin);
                                pSet.AddParameter(":2", strBnsCode);
                                pSet.AddParameter(":3", strGross);
                                pSet.AddParameter(":4", ConfigurationAttributes.CurrentYear);
                                pSet.AddParameter(":5", StringUtilities.SetEmptyToSpace(txtReason.Text.ToString().Trim()));
                                pSet.AddParameter(":6", dtpDate.Value);
                                pSet.AddParameter(":7", StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim()));
                                if (pSet.ExecuteNonQuery() == 0)
                                { }
                            }
                        }

                        strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, 0].Value.ToString()));

                        pSet.Query = "insert into addl_bns_que (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr)";
                        pSet.Query += " select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr ";
                        pSet.Query += string.Format(" from retired_bns_temp where bin ='{0}' and qtr = 'F' and main = 'N'", strBin);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        /*if (m_sTaxYear != "")
                        {
                            BillingForm.SourceClass = "Billing";
                            BillingForm.BIN = bin2.GetBin();    // RMC 20110801
                            BillingForm.Text = "Billing";
                            BillingForm.ShowDialog();
                            BillingForm.Dispose();

                        }*/
                        // RMC 20110801 temp disabled auto-billing for retirement

                        AppSettingsManager.InsertApplication(strBin, AppSettingsManager.GetSystemDate().Year.ToString()); //OTHER OFFICES

                        TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "AAE-WB", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                        if (AuditTrail.InsertTrail("AAE-WB", "business_que, businesses", StringUtilities.HandleApostrophe(strBin)) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Application for Full Retirement has been saved.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else	// Partial Retirement
                {
                    pSet.Query = string.Format("select qtr from retired_bns where bin = '{0}' order by qtr desc", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strQtr = pSet.GetString("qtr");
                            strQtr = string.Format("{0:##}", Convert.ToInt32(strQtr) + 1);
                        }
                        else
                            strQtr = "1";
                    }
                    pSet.Close();

                    pSet.Query = string.Format("select bns_code from businesses where bin = '{0}'", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            strMain = pSet.GetString("bns_code");
                    }
                    pSet.Close();

                    if (chkBilling.Checked == false)
                    {
                        strRetNo = GenerateRetNo();

                        for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                        {
                            bool blnRetire = (bool)dgvList[0, iRow].Value;

                            if (blnRetire)
                            {
                                strBnsCode = dgvList[1, iRow].Value.ToString();
                                strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, iRow].Value.ToString()));

                                if (strBnsCode == strMain)
                                {
                                    pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}' order by gross desc", strBin, m_sTaxYear);
                                    if (pSet.Execute())
                                    {
                                        if (pSet.Read())
                                        {
                                            string sAddlTY;
                                            strBnsCode = pSet.GetString("bns_code_main");
                                            sAddlTY = pSet.GetString("tax_year");

                                            pRec.Query = string.Format("update businesses set bns_code = '{0}' where bin ='{1}'", strBnsCode, strBin);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }

                                            /*pRec.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr)";
                                            pRec.Query += string.Format(" select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                             */
                                            // RMC 20151005 mods in retirement module (s)
                                            pRec.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS)";
                                            pRec.Query += string.Format(" select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                            // RMC 20151005 mods in retirement module (e)
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }

                                            pRec.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }

                                            pRec.Query = string.Format("update taxdues set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}'", strBnsCode, strBin, m_sTaxYear);
                                            pRec.Query += string.Format(" and bns_code_main = '{0}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strMain);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }

                                            pRec.Query = string.Format("update taxdues_hist set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}'", strBnsCode, strBin, m_sTaxYear);
                                            pRec.Query += string.Format(" and bns_code_main = '{0}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strMain);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }
                                        }
                                    }
                                    pSet.Close();

                                    //pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ',:9)";
                                    pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ',:9,'')";
                                    pSet.AddParameter(":1", strBin);
                                    pSet.AddParameter(":2", strBnsCode);
                                    pSet.AddParameter(":3", strGross);
                                    pSet.AddParameter(":4", ConfigurationAttributes.CurrentYear);
                                    pSet.AddParameter(":5", strQtr);
                                    pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(txtReason.Text.ToString().Trim()));
                                    pSet.AddParameter(":7", dtpDate.Value);
                                    pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim()));
                                    pSet.AddParameter(":9", strRetNo);
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }
                                    TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "AAE-NBP", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                                    // RMC 20141127 mods in trailing partial ret (s)
                                    string sObj = "BIN: " + strBin + "/Buss Code: " + strBnsCode;
                                    if (AuditTrail.InsertTrail("AAE-NBP", "business_que, businesses", StringUtilities.HandleApostrophe(sObj)) == 0)
                                    {
                                        pSet.Rollback();
                                        pSet.Close();
                                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    // RMC 20141127 mods in trailing partial ret (e)
                                }
                                else
                                {
                                   // pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ',:9)";
                                    pSet.Query = "insert into retired_bns values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ',:9,'')";
                                    pSet.AddParameter(":1", strBin);
                                    pSet.AddParameter(":2", strBnsCode);
                                    pSet.AddParameter(":3", strGross);
                                    pSet.AddParameter(":4", ConfigurationAttributes.CurrentYear);
                                    pSet.AddParameter(":5", strQtr);
                                    pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(txtReason.Text.ToString().Trim()));
                                    pSet.AddParameter(":7", dtpDate.Value);
                                    pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim()));
                                    pSet.AddParameter(":9", strRetNo);
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }
                                    TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "AAE-NBP", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin
                                    // RMC 20141127 mods in trailing partial ret (s)
                                    string sObj = "BIN: " + strBin + "/Buss Code: " + strBnsCode;
                                    if (AuditTrail.InsertTrail("AAE-NBP", "business_que, businesses", StringUtilities.HandleApostrophe(sObj)) == 0)
                                    {
                                        pSet.Rollback();
                                        pSet.Close();
                                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    // RMC 20141127 mods in trailing partial ret (e)
                                }

                                // tax code 13???
                                pSet.Query = string.Format("delete from taxdues where bin = '{0}' and bns_code_main = '{1}' and (tax_code = 'B' or tax_code = '13')", strBin, strBnsCode);
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                pSet.Query = string.Format("delete from tax_and_fees where bin = '{0}' and bns_code_main = '{1}'", strBin, strBnsCode);
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                /*pSet.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr)";
                                pSet.Query += string.Format(" select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                 */
                                // RMC 20151005 mods in retirement module (s)
                                pSet.Query = "insert into addl_bns_hist (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS)";
                                pSet.Query += string.Format(" select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr,PREV_GROSS from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                // RMC 20151005 mods in retirement module (e)
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_code_main = '{2}'", strBin, m_sTaxYear, strBnsCode);
                                if (pSet.ExecuteNonQuery() == 0)
                                { }
                            }
                        }


                        MessageBox.Show("All checked businesses have been retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                        {
                            bool blnRetire = (bool)dgvList[0, iRow].Value;

                            if (blnRetire)
                            {
                                strBnsCode = dgvList[1, iRow].Value.ToString();
                                strGross = string.Format("{0:##.00}", Convert.ToDouble(dgvList[3, iRow].Value.ToString()));

                                if (strBnsCode == strMain)
                                {
                                    pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}' order by gross desc", strBin, m_sTaxYear);
                                    if (pSet.Execute())
                                    {
                                        if (pSet.Read())
                                        {
                                            string sAddlBnsCode;
                                            sAddlBnsCode = pSet.GetString("bns_code_main");

                                            pRec.Query = string.Format("update taxdues set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}' ", sAddlBnsCode, strBin, m_sTaxYear);
                                            pRec.Query += string.Format(" and bns_code_main = '{0}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strMain);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }

                                            pRec.Query = string.Format("update taxdues_hist set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}'", sAddlBnsCode, strBin, m_sTaxYear);
                                            pRec.Query += string.Format(" and bns_code_main = '{0}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strMain);
                                            if (pRec.ExecuteNonQuery() == 0)
                                            { }
                                        }
                                    }
                                    pSet.Close();
                                }

                                if (iRow == 0)
                                    pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET',:5,:6,'Y',:7,:8,' ','')";
                                    //pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET',:5,:6,'Y',:7,:8,' ')";
                                else
                                    pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ','')";
                                    //pSet.Query = "insert into retired_bns_temp values(:1,:2,0.00,:3,:4,'RET',:5,:6,'N',:7,:8,' ')";
                                pSet.AddParameter(":1", strBin);
                                pSet.AddParameter(":2", strBnsCode);
                                pSet.AddParameter(":3", strGross);
                                pSet.AddParameter(":4", ConfigurationAttributes.CurrentYear);
                                pSet.AddParameter(":5", strQtr);
                                pSet.AddParameter(":6", StringUtilities.SetEmptyToSpace(txtReason.Text.ToString().Trim()));
                                pSet.AddParameter(":7", dtpDate.Value);
                                pSet.AddParameter(":8", StringUtilities.SetEmptyToSpace(txtAppNo.Text.ToString().Trim()));
                                if (pSet.ExecuteNonQuery() == 0)
                                { }
                                TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "AAE-WBP", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                                // RMC 20141127 mods in trailing partial ret (s)
                                string sObj = "BIN: " + strBin + "/Buss Code: " + strBnsCode;
                                if (AuditTrail.InsertTrail("AAE-WBP", "business_que, businesses", StringUtilities.HandleApostrophe(sObj)) == 0)
                                {
                                    pSet.Rollback();
                                    pSet.Close();
                                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                // RMC 20141127 mods in trailing partial ret (e)
                            }
                        }

                        strGross = string.Format("{0:##.00}", dgvList[3, 0].Value.ToString());

                        string sDel = "", sWaiveTF = "";
                        if (chkWaiveTaxAndFees.Checked)
                            sWaiveTF = "Y";
                        else
                            sWaiveTF = "N";

                        if (m_boWaive)
                            sDel = "Y";
                        else
                            sDel = "N";

                        pSet.Query = string.Format("delete from waive_tbl where bin = '{0}'", strBin);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        //pSet.Query = "insert into waive_tbl values('%s','%s','%s','P','%s')";
                        pSet.Query = "insert into waive_tbl values(:1,:2,:3,'P',:4)";   // RMC 20151005 mods in retirement module
                        pSet.AddParameter(":1", strBin.Trim());
                        pSet.AddParameter(":2", sWaiveTF.Trim());
                        pSet.AddParameter(":3", sDel.Trim());
                        pSet.AddParameter(":4", strQtr.Trim());
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        pSet.Query = "insert into addl_bns_que (bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr)";
                        pSet.Query += " select bin,bns_code_main,capital,gross,tax_year,bns_stat,qtr ";
                        pSet.Query += string.Format(" from retired_bns_temp where bin ='{0}' and qtr = '{1}' and main = 'N'", strBin, strQtr);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        //if(Convert.ToInt32(ConfigurationAttributes.CurrentYear) == Convert.ToInt32(m_sTaxYear))// RMC 20150108 put rem
                        {
                            pSet.Query = string.Format("insert into business_que select * from businesses where bin ='{0}'", strBin);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = string.Format("update business_que set bns_stat = 'RET' where bin ='{0}'", strBin);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }
                        }

                        // RMC 20150108 (s)
                        if (Convert.ToInt32(m_sTaxYear) < Convert.ToInt32(ConfigurationAttributes.CurrentYear))
                        {
                            //pSet.Query = string.Format("update business_que set tax_year = tax_year + 1 where bin ='{0}'", strBin);   // RMC 20170301 corrections in retirement application
                            //pSet.Query = string.Format("update business_que set tax_year ='" + ConfigurationAttributes.CurrentYear + "' where bin ='{0}'", strBin);  	// RMC 20150112 mods in retirement billing
                            string sYear = string.Empty;
                            sYear = string.Format("{0:yyyy}", dtpDate.Value);
                            pSet.Query = "update business_que set tax_year = '" + sYear + "' where bin ='" + strBin + "' ";   // RMC 20170301 corrections in retirement application
                            if (pSet.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20150108 (e)

                        /*if (m_sTaxYear != "")
                        {
                            BillingForm.SourceClass = "Billing";
                            BillingForm.BIN = bin2.GetBin();    // RMC 20110801
                            BillingForm.Text = "Billing";
                            BillingForm.ShowDialog();
                            BillingForm.Dispose();

                        }*/
                        // RMC 20110801 temp disabled auto-billing for retirement


                        MessageBox.Show("Application for Partial Retirement has been saved.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS"))
                {
                }

                ClearControls();
            }
		
		    
        }

        private bool Validate()
        {
            bool boFull, boEmpty;
            boFull = false;
            boEmpty = false;

            if (chkPartial.Checked)
            {
                for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                {
                    bool blnRetire = (bool)dgvList[0, iRow].Value;

                    if (!blnRetire)
                        boFull = true;
                    else
                        boEmpty = true;
                }

                if (!boFull)
                {
                    MessageBox.Show("Unable to apply Full Retirement in Partial Retirement.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (!boEmpty)
                {
                    MessageBox.Show("Please select the business to be retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }
            
        private string GenerateRetNo()
        {
            OracleResultSet pSet = new OracleResultSet();
            string strRetNo = string.Empty;

            int iRetNo = 0;

	        pSet.Query = string.Format("select * from retired_bns where retirement_no is not null order by retirement_no desc");
	        if(pSet.Execute())
            {
                if (pSet.Read())
                    iRetNo = pSet.GetInt("retirement_no");
                else
                    iRetNo = 0; // RMC 20110810
            }
            pSet.Close();

            strRetNo = string.Format("{0:000000000#}",iRetNo + 1);	

            return strRetNo;
        }

        private void dgvList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strValue = string.Empty;

            if (e.ColumnIndex == 3)
            {
                try
                {
                    strValue = dgvList[3, e.RowIndex].Value.ToString();
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                }
                catch
                {
                    MessageBox.Show("Error in Field", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dgvList[3, e.RowIndex].Value = "0.00";
                    return;
                }

            }
        }

        private void btnCancelApp_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            string strBin = bin2.GetBin();

            if (MessageBox.Show("Do you really want to cancel this application?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("select * from retired_bns_temp where bin = '{0}'", strBin);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    { }
                    else
                    {
                        MessageBox.Show("No retirement application to be cancelled.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                pSet.Close();

              
                       
                pSet.Query = string.Format("delete from business_que where bin = '{0}' and bns_stat = 'RET'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}'  and bns_stat = 'RET'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from retired_bns_temp where bin = '{0}'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (s) 
                //pSet.Query = string.Format("insert into taxdues select * from taxdues_hist where bin = '{0}' and due_state = 'X'", strBin);
                pSet.Query = string.Format("insert into taxdues_hist select * from taxdues where bin = '{0}' and due_state = 'X'", strBin);  //JHB 20181122 revise saving of cancelled ret dues
                if (pSet.ExecuteNonQuery() == 0)
                { }

                string sDueState = string.Empty;
                pSet.Query = "select bns_stat from businesses where bin = '" + strBin + "'";
                if(pSet.Execute())
                {
                    if(pSet.Read())
                    {
                        sDueState = pSet.GetString(0);
                        if(sDueState == "NEW")
                            sDueState = "N";
                        else if(sDueState == "REN")
                            sDueState = "R";
                        else
                            sDueState = "X";
                    }
                }

                //pSet.Query = "update taxdues set due_state = '" + sDueState + "' where bin = '" +strBin + "'";  //JHB 20181122 hide having conflict with soa display
               // if (pSet.ExecuteNonQuery() == 0)
                //{ }
                // RMC 20131229 added saving to taxdues_hist when delinq was waived in retirement application (e)

               // pSet.Query = string.Format("delete from taxdues_hist where bin = '{0}' and due_state = 'X'", strBin); //JHB 20181122 update deletion of taxdues
                pSet.Query = string.Format("delete from taxdues where bin = '{0}' and due_state = 'X'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from bill_no where bin = '{0}' and due_state = 'X'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from bill_hist where bin = '{0}' and due_state = 'X'", strBin);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("select * from businesses where bin = '{0}'", strBin);
                if(pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        string strBnsCode;
                        strBnsCode = pSet.GetString("bns_code");

                        pRec.Query = string.Format("update taxdues set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strBnsCode, strBin, m_sTaxYear);
                        if (pRec.ExecuteNonQuery() == 0)
                        { }

                        pRec.Query = string.Format("update taxdues_hist set bns_code_main = '{0}' where bin = '{1}' and tax_year = '{2}' and tax_code <> 'B' and tax_code <> '01' and tax_code <> '13'", strBnsCode, strBin, m_sTaxYear);
                        if (pRec.ExecuteNonQuery() == 0)
                        { }
                    }
                }
                pSet.Close();

                TransLog.UpdateLog(strBin, "RET", ConfigurationAttributes.CurrentYear, "DELETE", m_dTransLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                MessageBox.Show("Application for retirement has been cancelled.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // RMC 20150112 mods in retirement billing (s)
                if (TaskMan.IsObjectLock(strBin, "RETIREMENT", "DELETE", "ASS")) 
                {
                }
                // RMC 20150112 mods in retirement billing (e)

                ClearControls();
                btnSearch.Text = "Search";
                txtAppNo.ReadOnly = true;
                btnCancelApp.Enabled = false;

                /*if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS")) // RMC 20150108
                {
                }*/
                // RMC 20150112 mods in retirement billing, put
            }
        }

        private void txtAppNo_Leave(object sender, EventArgs e)
        {
            if (!bToView)
            {
                OracleResultSet pSet = new OracleResultSet();
                string strBin = bin2.GetBin();

                pSet.Query = string.Format("select * from retired_bns_temp where bin = '{0}' and app_no = '{1}'", strBin, txtAppNo.Text.ToString().Trim());
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        if (MessageBox.Show("Do you want to edit the application?", "Retirement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            txtAppNo.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("select * from retired_bns_temp where app_no = '{0}'", txtAppNo.Text.ToString().Trim());
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                MessageBox.Show("Duplicate Application Number.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                txtAppNo.Text = "";
                                return;
                            }
                        }
                        pSet.Close();

                        pSet.Query = string.Format("select * from retired_bns where app_no = '{0}'", txtAppNo.Text.ToString().Trim());
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                MessageBox.Show("Duplicate Application Number.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                txtAppNo.Text = "";
                                return;
                            }
                        }
                        pSet.Close();
                    }
                }
                pSet.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock(bin2.GetBin(), "RETIREMENT", "DELETE", "ASS"))
            {
            }

            this.Close();
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (bin2.txtTaxYear.Text == "" && bin2.txtBINSeries.Text == "") //JHMN 20170120 validation to prevent BIN being empty
            {
                MessageBox.Show("BIN textbox should not be empty.", "Print Retirement", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (!ValidateRetiredApp())
                return;

            if (dgvList.Rows.Count > 0) // JHMN 20170116 added for retirement application
            {
                frmAppForm PrintAppForm = new frmAppForm();
                PrintAppForm.BIN = bin2.GetBin();
                PrintAppForm.AppNo = txtAppNo.Text.Trim();
                PrintAppForm.ApplType = "Retirement";
                PrintAppForm.BnsCode = dgvList.CurrentRow.Cells[1].Value.ToString().Trim();
                PrintAppForm.Memo = txtReason.Text.ToString();
                PrintAppForm.ShowDialog();
            }
        }

        private bool ValidateRetiredApp()
        {
            // JHMN 20170118 mods in reverse retirement module (s)
            if (AppSettingsManager.GetConfigValue("10") == "243")
                return true;
            else// JHMN 20170118 mods in reverse retirement module (e)
            {
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "select * from retired_bns where bin = '" + bin2.GetBin() + "' and app_no = '" + txtAppNo.Text.Trim() + "' and bns_code_main = '" + dgvList.CurrentRow.Cells[1].Value.ToString().Trim() + "'";
                if (pSet.Execute())
                    if (!pSet.Read())
                    {
                        MessageBox.Show("Data Error, business must be retired", " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                pSet.Close();
                return true;
            }
        }

        private void btnRollBack_Click(object sender, EventArgs e)
        {

        }

        private string GetAppNoifExist(string sBIN)
        {
            string sAppNo = String.Empty;
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from retired_bns where bin = '" + sBIN + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sAppNo = pSet.GetString("app_no");
                else
                {
                    pSet.Close();
                    pSet.Query = "select * from retired_bns_temp where bin = '" + sBIN + "'";
                    if (pSet.Execute())
                        if (pSet.Read())
                            sAppNo = pSet.GetString("app_no");
                }
            pSet.Close();

            if (sAppNo.Length <= 10)
            {
                pSet.Query = "select * from retired_bns_temp where bin = '" + sBIN + "'";
                if (pSet.Execute())
                    if (pSet.Read())
                        sAppNo = pSet.GetString("app_no");
                pSet.Close();
            }
            return sAppNo;
        }

        private bool Validates()
        {
            bool boFull, boEmpty;
            boFull = false;
            boEmpty = false;

            if (chkPartial.Checked)
            {
                for (int iRow = 0; iRow <= dgvList.Rows.Count - 1; iRow++)
                {
                    bool blnRetire = (bool)dgvList[0, iRow].Value;

                    if (!blnRetire)
                        boFull = true;
                    else
                        boEmpty = true;
                }

                if (!boFull)
                {
                    MessageBox.Show("Unable to apply Full Retirement in Partial Retirement.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (!boEmpty)
                {
                    MessageBox.Show("Please select the business to be retired.", "Retirement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }

        private void LoadViolation()
        {
            // RMC 20171115 Added capturing of violation as reason of retirement in retirement module

            String sValueViolation = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select distinct VT.VIOLATION_DESC From VIOLATION_TABLE VT  left join violations VS on VS.VIOLATION_CODE = VT.VIOLATION_CODE where VS.Bin = '" + bin2.GetBin().Trim() + "'";
            if (pSet.Execute())
                while (pSet.Read())
                    sValueViolation += pSet.GetString(0) + "\n";
            pSet.Close();

            if (sValueViolation.Length > 0)
                txtReason.Text = sValueViolation.Remove(sValueViolation.ToString().Length - 1, 1).Trim();

            
        }
        
    }
}