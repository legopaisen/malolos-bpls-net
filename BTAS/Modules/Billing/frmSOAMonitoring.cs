using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SOA;
using Amellar.Common.LogIn;
using System.IO;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.Billing
{
    //AFM 20210624 MODULE REQUESTED BY MALOLOS LGU INTEGRATED WITH BPLS ONLINE
    public partial class frmSOAMonitoring : Form
    {
        public frmSOAMonitoring()
        {
            InitializeComponent();
        }

        public SystemUser m_objSystemUser;
        private string m_sBin = string.Empty;

        private void frmSOAMonitoring_Load(object sender, EventArgs e)
        {
            bin1.txtLGUCode.Text = AppSettingsManager.GetConfigObject("10");
            bin1.txtDistCode.Text = AppSettingsManager.GetConfigObject("11");
            bin1.txtTaxYear.Focus();
            btnViewSOA.Enabled = false;
            PopulateGrid("");
        }
        private void PopulateGrid(string sBinVal)
        {
            dgvList.Rows.Clear();
            string sBIN = string.Empty;
            string sTaxYear = string.Empty;
            string sOwnCode = string.Empty;
            string sBnsUser = string.Empty;
            string sTransaction = string.Empty; //MCR20210927
            double dAmt = 0;
            DateTime dtBill = AppSettingsManager.GetSystemDate();
            OracleResultSet res = new OracleResultSet();
            OracleResultSet res2 = new OracleResultSet();
            OracleResultSet res3 = new OracleResultSet();
            res.Query = @"select distinct SA.bin, sum(SB.total) as amount_due, max(BN.bill_date) as bill_date, BN.bill_user from soa_monitoring SA, soa_tbl SB, bill_no BN where 
                            SA.bin = SB.bin 
                            and SA.bin = BN.bin
                            and SA.bin in (select bin from soa_monitoring where soa_monitoring.bin = SA.bin and status = 'PENDING')
                            and SA.bin like '%" + sBinVal + "%' and tax_year = '" + ConfigurationAttributes.CurrentYear + "'  group by SA.BIN, BN.bill_user"; //MCR 20210708 added taxyear temporary please change this into max taxyear of record
            if(res.Execute())
                while (res.Read())
                {
                    sBIN = res.GetString("bin");
                    dtBill = res.GetDateTime("bill_date");
                    dAmt = res.GetDouble("amount_due");
                    sBnsUser = res.GetString("bill_user");

                    res2.Query = "select bin, tax_year, bns_user, own_code from business_que where bin = '" + sBIN + "'";
                    if(res2.Execute())
                        if (res2.Read())
                        {
                            sTaxYear = res2.GetString("tax_year");
                            sOwnCode = res2.GetString("own_code");
                            sTransaction = res2.GetString("bns_user");
                        }
                        else
                        {
                            res3.Query = "select bin, tax_year, bns_user, own_code from businesses where bin = '" + sBIN + "'";
                            if(res3.Execute())
                                if(res3.Read())
                                {
                                    sTaxYear = res3.GetString("tax_year");
                                    sOwnCode = res3.GetString("own_code");
                                    sTransaction = res2.GetString("bns_user");
                                }

                        }
                    res2.Close();
                    res3.Close();

                    if (sTransaction == "SYS_PROG")
                        sTransaction = "WEB";
                    else
                        sTransaction = "LOCAL";

                    dgvList.Rows.Add(sBIN, dAmt.ToString("#,##0.00"), sBnsUser, dtBill.ToShortDateString(), sTaxYear, sOwnCode, sTransaction);
                }
            res.Close();
            dgvList.ClearSelection();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string sBin = dgvList.SelectedCells[0].Value.ToString();
            string sOwnerCode = dgvList.SelectedCells[5].Value.ToString();

            txtBnsName.Text = AppSettingsManager.GetBnsName(sBin);
            txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(sBin);
            txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnerCode);
            txtOwnAddress.Text = AppSettingsManager.GetBnsOwnAdd(sOwnerCode);
            txtTaxYear.Text = dgvList.SelectedCells[4].Value.ToString();
            m_sBin = sBin;

            btnViewSOA.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtBnsName.Text = string.Empty;
            txtBnsAddress.Text = string.Empty;
            txtOwnName.Text = string.Empty;
            txtOwnAddress.Text = string.Empty;
            txtTaxYear.Text = string.Empty;
            bin1.GetTaxYear = string.Empty;
            bin1.GetBINSeries = string.Empty;
            m_sBin = string.Empty;
            btnViewSOA.Enabled = false;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (m_sBin == "")
                return;

            OracleResultSet pSet = new OracleResultSet();
            if (MessageBox.Show("Approve SOA?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // MCR 20210701 (s)
                string sSystemUserName = "";
                string sSystemUserCode = "";
                sSystemUserName = AppSettingsManager.SystemUser.UserName;
                sSystemUserCode = AppSettingsManager.SystemUser.UserCode;

                string sApprover = "";
                string sAppGroup = "";
                bool bAppoved = true;

                frmLogIn fLog = new frmLogIn();
                fLog.sFormState = "LOGIN";
                fLog.Text = "Approving Officer";
                fLog.ShowDialog();

                if (fLog.m_objSystemUser.UserCode != string.Empty)
                {
                    pSet.Query = "select * from approver_tbl where usr_code = '" + fLog.m_objSystemUser.UserCode + "'";
                    if (sApprover.Trim() != "" && sAppGroup.Trim() != "")
                    {
                        pSet.Query += " and usr_code <> '" + sApprover + "'";
                        pSet.Query += " and usr_group <> '" + sAppGroup + "'";
                    }

                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sApprover = pSet.GetString("usr_code");
                            sAppGroup = pSet.GetString("usr_group");
                            pSet.Close();
                        }
                        else
                            bAppoved = false;
                    }
                }
                else
                    bAppoved = false;

                //if (m_objSystemUser.Load(sSystemUserCode))
                //    AppSettingsManager.g_objSystemUser = m_objSystemUser;

                if (!bAppoved)
                {
                    MessageBox.Show("Approval failed.", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (AppSettingsManager.GetConfigValue("75") == "Y")
                {
                    String sValue = "";
                    //OracleResultSet pSet = new OracleResultSet();
                    //pSet.Query = "select BO_SEND_APPROVED_NOTIF('" + m_sBin + "','" + Convert.ToDouble(dgvList.CurrentRow.Cells[1].Value.ToString()) + "','" + sApprover + "') from dual";
                    pSet.Query = "select BO_SEND_APPROVED_NOTIF('" + m_sBin + "','" + sApprover + "') from dual";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            sValue = pSet.GetString(0);
                    }
                    pSet.Close();
                }
                //MCR 20210701 (e)

                OracleResultSet res = new OracleResultSet();
                res.Query = "DELETE FROM SOA_MONITORING WHERE BIN = '" + m_sBin + "'";
                if (res.ExecuteNonQuery() == 0)
                { }

                res.Query = "INSERT INTO SOA_MONITORING VALUES('" + m_sBin + "', 'APPROVED', '', '')";
                if (res.ExecuteNonQuery() == 0)
                { }

                CreateQRCODE(); //MCR 20210701
                //String sDir = Directory.GetCurrentDirectory() + "\\QR\\QRCODE.exe";
                String sDir = Directory.GetCurrentDirectory() + "\\QRCODE.exe";
                Process.Start(sDir);
            }
            else
                return;


            ClearFields();
            PopulateGrid("");
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (m_sBin == "")
                return;

            if (MessageBox.Show("Return SOA?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                frmApproveReturnMemo form = new frmApproveReturnMemo();
                form.ShowDialog();

                // MCR 20210701 (s)
                if (AppSettingsManager.GetConfigValue("75") == "Y")
                {
                    String sValue = "";
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "select BO_SEND_ON_HOLD_NOTIF('" + m_sBin + "','" + AppSettingsManager.SystemUser.UserCode + "', '" + form.Memo.Trim() + "') from dual";
                    if (pSet.Execute())
                        if (pSet.Read())
                            sValue = pSet.GetString(0);
                    pSet.Close();
                }
                //MCR 20210701 (e)

                if (form.isOK == true)
                {
                    OracleResultSet res = new OracleResultSet();
                    //res.Query = "DELETE FROM SOA_MONITORING WHERE BIN = '" + m_sBin + "'";
                    //if (res.ExecuteNonQuery() == 0)
                    //{ }

                    res.Query = "UPDATE SOA_MONITORING SET STATUS = 'RETURNED', MEMO = '" + StringUtilities.HandleApostrophe(form.Memo.Trim()) + "' where bin = '" + m_sBin + "'";
                    if (res.ExecuteNonQuery() == 0)
                    { }

                    //MCR 20210708 (s)
                    String sRefNo = "";
                    sRefNo = GetSOARefNo(m_sBin);

                    if (AppSettingsManager.GetConfigValue("75") == "Y") //MCR 20210708
                    {
                        String sValue = "";
                        OracleResultSet pSet = new OracleResultSet();
                        pSet.Query = "select BO_VOID_REF_NO('" + sRefNo + "') from dual";
                        if (pSet.Execute())
                            if (pSet.Read())
                                sValue = pSet.GetString(0);
                        pSet.Close();
                    }
                    //MCR 20210708 (e)
                }
            }
            else
                return;

            ClearFields();
            PopulateGrid("");
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearFields();
            PopulateGrid("");
        }

        private void btnViewSOA_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_sBin))
            {
                frmSOA fSOA = new frmSOA();
                fSOA.iFormState = 0;
                fSOA.sBIN = m_sBin;
                fSOA.txtTaxYear.Text = this.txtTaxYear.Text;

                OracleResultSet pTmpValid = new OracleResultSet();
                bool bRen = false;
                pTmpValid.Query = "select * from taxdues where bin = '" + m_sBin + "' and (due_state = 'R' or due_state = 'P')";
                if (pTmpValid.Execute())
                {
                    if (pTmpValid.Read())
                    {
                        bRen = true;
                    }
                    else
                    {
                        bRen = false;
                    }
                }
                pTmpValid.Close();

                if (bRen)
                {
                    fSOA.txtStat.Text = "REN";
                }
                else
                {
                    fSOA.txtStat.Text = "NEW";
                }
                fSOA.GetData(m_sBin);
                fSOA.ShowDialog();
            }
        }

        private void CreateQRCODE()
        {
            //string fileName = Directory.GetCurrentDirectory() + "\\QR\\QRCODE.txt";
            string fileName = Directory.GetCurrentDirectory() + "\\QRCODE.txt";
            try
            {
                String sBillNo = "", sValidity = "";
                double dTotal = 0;
                DateTime dtQtrValid;
                OracleResultSet pSet = new OracleResultSet();
                pSet.Query = "select sum(total),bill_no,soa_validity from soa_tbl where bin = '" + m_sBin + "' group by bill_no, SOA_VALIDITY";
                if (pSet.Execute())
                    if (pSet.Read())
                    {
                        dTotal = pSet.GetDouble(0);
                        sBillNo = pSet.GetString(1);
                        sValidity = pSet.GetDateTime(2).ToString("MM/dd/yyyy");
                    }
                pSet.Close();


                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                    File.Delete(fileName);

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine("BIN: " + m_sBin);
                    sw.WriteLine("TAX YEAR: " + txtTaxYear.Text);
                    sw.WriteLine("BILL NO: " + sBillNo);
                    sw.WriteLine("AMOUNT: " + dTotal.ToString("#,##0.00"));
                    sw.WriteLine("DATE BILLED: " + dgvList.CurrentRow.Cells[3].Value.ToString());
                    sw.WriteLine("PREPARED BY: " + dgvList.CurrentRow.Cells[2].Value.ToString());
                    sw.WriteLine("VALIDITY DATE: " + sValidity);
                    sw.WriteLine("APPROVED BY: " + AppSettingsManager.SystemUser.UserCode);
                }

                //MCR 20210708 (s)
                if (AppSettingsManager.GetConfigValue("75") == "Y")
                {
                    String sValue = "";
                    dtQtrValid = Convert.ToDateTime(sValidity);

                    pSet = new OracleResultSet();
                    pSet.Query = "select BO_GET_REF_NO('" + m_sBin + "','" + dgvList.CurrentRow.Cells[4].Value.ToString() + "','" + sBillNo + "'," + dTotal + ", to_date('" + dtQtrValid.ToString("MM/dd/yyyy") + "','MM/dd/yyyy')) from dual";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            sValue = pSet.GetString(0);

                        OracleResultSet res = new OracleResultSet();
                        res.Query = "UPDATE SOA_MONITORING SET refno = '" + sValue + "' where bin = '" + m_sBin + "'";
                        if (res.ExecuteNonQuery() == 0)
                        { }
                    }
                    pSet.Close();
                }
                //MCR 20210708 (e)
            }
            catch
            {
            }
        }

        private string GetSOARefNo(string sBin)
        {
            String sValue = "";

            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select refno from soa_monitoring where bin = '" + sBin + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sValue = pSet.GetString(0);
            pSet.Close();

            return sValue;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sBIN = bin1.GetBin();
            if (bin1.txtTaxYear.Text.Trim() != string.Empty && bin1.txtBINSeries.Text.Trim() != string.Empty)
            {
                PopulateGrid(sBIN);
                if (dgvList.Rows.Count == 0)
                {
                    MessageBox.Show("No record found!");
                    return;
                }
                ClearFields();

                string sOwnerCode = dgvList[5,0].Value.ToString();

                txtBnsName.Text = AppSettingsManager.GetBnsName(sBIN);
                txtBnsAddress.Text = AppSettingsManager.GetBnsAddress(sBIN);
                txtOwnName.Text = AppSettingsManager.GetBnsOwner(sOwnerCode);
                txtOwnAddress.Text = AppSettingsManager.GetBnsOwnAdd(sOwnerCode);
                txtTaxYear.Text = dgvList[4, 0].Value.ToString();
                m_sBin = sBIN;

                btnViewSOA.Enabled = true;
            }
        }
    }
}