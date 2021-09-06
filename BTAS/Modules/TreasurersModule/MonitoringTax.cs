
// RMC 20120119 corrected approval memoranda in treasurers module
// RMC 20120117 Corrections in approving treasurers module when memoranda was cancelled
// RMC 20120113 Capture memoranda first before approval in BTax Monitoring
// RMC 20120109 added validation of access rights in Treasurer's module Return transaction
// RMC 20120109 added refresh button in Btax monitoring
// RMC 20120109 corrected field type in display of Btax monitoring
// RMC 20120105 validate if already approved at bptfo 
// RMC 20120103 modified approval of BTAX monitoring
// RMC 20120103 added capturing of memoranda in approving btax
// ALJ 20111227 save to for SOA Printing
// RMC 20111227 added Gross monitoring module for gross >= 200000

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Amellar.Common.AppSettings;
using Amellar.Common.LogIn;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;

namespace Amellar.BPLS.TreasurersModule
{
    public class MonitoringTax : Monitoring
    {
        private int m_iCnt = 0; // RMC 20150811 modified approval in treasurer's module

        public MonitoringTax(frmBTaxMonitoring Form)
            : base(Form)
        { }


        public override void UpdateList()
        {
            OracleResultSet pRec = new OracleResultSet();
            int iRow = 0;

            MonitoringFrm.dgvList.Columns.Clear();
            MonitoringFrm.dgvList.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            MonitoringFrm.dgvList.Columns.Add("BIN", "BIN");
            MonitoringFrm.dgvList.Columns.Add("PARAMETER", "PARAMETER");
            MonitoringFrm.dgvList.Columns.Add("PREVTAX", "PREV. TAX PAID");
            MonitoringFrm.dgvList.Columns.Add("CURRDUE", "CURR. TAX DUE");
            MonitoringFrm.dgvList.Columns.Add("UCODE", "USER CODE");
            MonitoringFrm.dgvList.Columns.Add("STATUS", "STATUS");
            MonitoringFrm.dgvList.Columns.Add("PRINTED", "PRINTED");
            MonitoringFrm.dgvList.Columns.Add("DATE", "DATE/TIME");
            MonitoringFrm.dgvList.Columns.Add("TAXYEAR", "TAX YEAR");

            MonitoringFrm.dgvList.RowHeadersVisible = false;
            MonitoringFrm.dgvList.Columns[0].Width = 150;
            MonitoringFrm.dgvList.Columns[1].Width = 100;
            MonitoringFrm.dgvList.Columns[2].Width = 120;
            MonitoringFrm.dgvList.Columns[3].Width = 120;
            MonitoringFrm.dgvList.Columns[4].Width = 100;
            MonitoringFrm.dgvList.Columns[5].Width = 50;
            MonitoringFrm.dgvList.Columns[6].Width = 50;
            MonitoringFrm.dgvList.Columns[7].Width = 100;
            MonitoringFrm.dgvList.Columns[8].Width = 100;
            MonitoringFrm.dgvList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MonitoringFrm.dgvList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            MonitoringFrm.dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            MonitoringFrm.dgvList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            MonitoringFrm.dgvList.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            string sAction = "";
            string sPrinted = "";
            int iPrinted = 0;

            pRec.Query = "select * from treasurers_module where (action = '0' or action = '1') order by dt_save desc";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    MonitoringFrm.dgvList.Rows.Add("");
                    MonitoringFrm.dgvList[0, iRow].Value = pRec.GetString("bin");
                    MonitoringFrm.dgvList[1, iRow].Value = pRec.GetString("parameter");
                    //MonitoringFrm.dgvList[2, iRow].Value = string.Format("{0:#,###.00}", pRec.GetFloat("prev_tax"));
                    //MonitoringFrm.dgvList[3, iRow].Value = string.Format("{0:#,###.00}", pRec.GetFloat("curr_tax"));
                    MonitoringFrm.dgvList[2, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("prev_tax"));    // RMC 20120109 corrected field type in display of Btax monitoring
                    MonitoringFrm.dgvList[3, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("curr_tax"));  // RMC 20120109 corrected field type in display of Btax monitoring
                    MonitoringFrm.dgvList[4, iRow].Value = pRec.GetString("user_code");
                    sAction = pRec.GetString("action");

                    if (sAction == "0")
                        sAction = "HOLD";
                    else if (sAction == "1")
                        sAction = "RETURN";

                    MonitoringFrm.dgvList[5, iRow].Value = sAction;

                    iPrinted = pRec.GetInt("is_printed");

                    if (iPrinted == 1)
                        sPrinted = "YES";
                    else
                        sPrinted = "NO";

                    MonitoringFrm.dgvList[6, iRow].Value = sPrinted;
                    MonitoringFrm.dgvList[7, iRow].Value = pRec.GetDateTime("dt_save");
                    MonitoringFrm.dgvList[8, iRow].Value = pRec.GetString("tax_year");

                    if (iRow == 0)
                    {
                        MonitoringFrm.LoadValues(iRow);
                    }

                    iRow++;
                }

            }
            pRec.Close();

        }

        public override void Approve()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sUserCode = "";
            string sAction = "";
            string sTaxYear = "";
            int iRow = 0;

            if (MonitoringFrm.txtBnsName.Text.Trim() != "")
            {
                iRow = MonitoringFrm.dgvList.SelectedCells[0].RowIndex;
                if (MonitoringFrm.dgvList[4, iRow].Value != null)
                    sUserCode = MonitoringFrm.dgvList[4, iRow].Value.ToString().Trim();
                if (MonitoringFrm.dgvList[5, iRow].Value != null)
                    sAction = MonitoringFrm.dgvList[5, iRow].Value.ToString().Trim();
                if (MonitoringFrm.dgvList[8, iRow] != null)
                    sTaxYear = MonitoringFrm.dgvList[8, iRow].Value.ToString().Trim();

                // RMC 20120113 Capture memoranda first before approval in BTax Monitoring (s)
                frmMemoranda MemoFrm = new frmMemoranda();
                string sMemoranda = "";

                if (ValidateMemoranda(sTaxYear))
                {
                    if (sAction == "HOLD")
                        sAction = "0";
                    else if (sAction == "RETURN")
                        sAction = "1";

                    // RMC 20120119 corrected approval memoranda in treasurers module (s)
                    MemoFrm.m_sQuery = "select memoranda from treasurers_module where bin = '" + MonitoringFrm.bin1.GetBin() + "'";
                    MemoFrm.m_sQuery += " and action = '" + sAction + "' and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                    MemoFrm.ShowDialog();
                    sMemoranda = MemoFrm.m_sMemo;
                    // RMC 20120119 corrected approval memoranda in treasurers module (e)

                    //while (sMemoranda.Trim() == "")
                    if (sMemoranda.Trim() != "")    // RMC 20120117 Corrections in approving treasurers module when memoranda was cancelled
                    {
                        /*MemoFrm.m_sQuery = "select memoranda from treasurers_module where bin = '" + MonitoringFrm.bin1.GetBin() + "'";
                        MemoFrm.m_sQuery += " and action = '" + sAction + "' and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                        MemoFrm.ShowDialog();
                        sMemoranda = MemoFrm.m_sMemo;*/
                        // RMC 20120119 corrected approval memoranda in treasurers module

                        pSet.Query = "update treasurers_module set memoranda = '" + sMemoranda + "' ";
                        pSet.Query += " where bin = '" + MonitoringFrm.bin1.GetBin() + "' and action = '" + sAction + "' ";
                        pSet.Query += " and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }
                }
                // RMC 20120113 Capture memoranda first before approval in BTax Monitoring (e)

                if (MessageBox.Show("Are you sure you want to approve " + MonitoringFrm.bin1.GetBin() + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sSystemUserName = "";
                    string sSystemUserCode = "";
                    sSystemUserName = AppSettingsManager.SystemUser.UserName;
                    sSystemUserCode = AppSettingsManager.SystemUser.UserCode;

                    // (s) ALJ 20110907 re-assessment validation - Approving Officers
                    string sApprover = "";
                    string sAppGroup = "";
                    string sUser = "";
                    bool bAppoved = true;

                    /*// RMC 20150811 modified approval in treasurer's module (s)
                    // determine if 2 office will approve
                    pSet.Query = "select count(distinct usr_group) from approver_tbl";
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            m_iCnt = pSet.GetInt(0);
                        }
                    }
                    pSet.Close();                     
                    // RMC 20150811 modified approval in treasurer's module (e)*/ //REM MCR 20170605

                    //for (int i = 1; i <= 2; i++)
                    //for (int i = 1; i <= 1; i++)    // RMC 20120103 modified approval of BTAX monitoring
                    //for (int i = 1; i <= m_iCnt; i++)   // RMC 20150811 modified approval in treasurer's module 
                    //{
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
                            //pSet.Query += " and usr_group <> 'TREASURER'";    // RMC 20150811 modified approval in treasurer's module
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    sApprover = pSet.GetString("usr_code");
                                    sAppGroup = pSet.GetString("usr_group");

                                    pSet.Close();

                                    // RMC 20120103 modified approval of BTAX monitoring (s)
                                    if (!ValidateIfApproved(sAppGroup, sApprover, MonitoringFrm.bin1.GetBin(), sTaxYear))
                                    {
                                        // RMC 20120112 (S)
                                        //load back system user code
                                        if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                                        {
                                            AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                                        }
                                        // RMC 20120112 (E)


                                        return;
                                    }
                                    // RMC 20120103 modified approval of BTAX monitoring (e)

                                    if (AuditTrail.InsertTrail("AUTM-A", "treasurers_module", MonitoringFrm.bin1.GetBin()) == 0)
                                    {
                                        pSet.Rollback();
                                        pSet.Close();
                                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                        // RMC 20120112 (S)
                                        //load back system user code
                                        if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                                        {
                                            AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                                        }
                                        // RMC 20120112 (E)

                                        return;
                                    }

                                    /*if (i == 1) // RMC 20150811 modified approval in treasurer's module, enabled
                                    //if (sAppGroup == "LICENSE")  // RMC 20120103 modified approval of BTAX monitoring      
                                        MessageBox.Show("First approval. OK!", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    else
                                        MessageBox.Show("Second approval. OK!", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);*/ //REM MCR 20170605
                                }
                                else
                                    bAppoved = false;
                            }
                        }
                        else
                            bAppoved = false;   // RMC 20120109
                    //}

                    if (!bAppoved)
                    {
                        MessageBox.Show("Approval failed.", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        // RMC 20120109 (S)
                        //load back system user code
                        if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                        {
                            AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                        }
                        // RMC 20120109 (E)

                        return;
                    }
                    // (e) ALJ 20110907 re-assessment validation - Approving Officers


                    if (sAction == "HOLD")
                        sAction = "0";
                    else if (sAction == "RETURN")
                        sAction = "1";

                    if (ValidateMemoranda(sTaxYear))    // RMC 20120113 Capture memoranda first before approval in BTax Monitoring
                    {
                        // RMC 20120103 added capturing of memoranda in approving btax (s)
                        //frmMemoranda MemoFrm = new frmMemoranda();
                        //string sMemoranda = "";
                        MemoFrm = new frmMemoranda();
                        sMemoranda = "";


                        while (sMemoranda.Trim() == "")
                        {
                            MemoFrm.m_sQuery = "select memoranda from treasurers_module where bin = '" + MonitoringFrm.bin1.GetBin() + "'";
                            MemoFrm.m_sQuery += " and action = '" + sAction + "' and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                            MemoFrm.ShowDialog();
                            sMemoranda = MemoFrm.m_sMemo;

                            pSet.Query = "update treasurers_module set memoranda = '" + sMemoranda + "' ";
                            pSet.Query += " where bin = '" + MonitoringFrm.bin1.GetBin() + "' and action = '" + sAction + "' ";
                            pSet.Query += " and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                            if (pSet.ExecuteNonQuery() == 0)
                            { }
                        }
                        // RMC 20120103 added capturing of memoranda in approving btax (e)
                    }

                    if (ValidateIfApproved(MonitoringFrm.bin1.GetBin(), sTaxYear))  // RMC 20120103 added capturing of memoranda in approving btax
                    {
                        pSet.Query = "update treasurers_module set action = '2'";
                        pSet.Query += " where bin = '" + MonitoringFrm.bin1.GetBin() + "' and action = '" + sAction + "' ";
                        pSet.Query += " and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        // RMC 20170117 correction in Treasurer's module approval (s)
                        pSet.Query = "delete from TREASURERS_MODULE_TMP where ";
                        pSet.Query += " bin = '" + MonitoringFrm.bin1.GetBin() + "' and";
                        pSet.Query += " tax_year = '" + sTaxYear + "'";
                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                        // RMC 20170117 correction in Treasurer's module approval (e)

                        // (s) ALJ 20111227 save to for SOA Printing
                        //     upon approve
                        if (!AppSettingsManager.IsForSoaPrint(MonitoringFrm.bin1.GetBin(), sTaxYear)) // not yet ready for SOA Printing
                        {
                            string sBaseTaxYear;
                            // get the base due year
                            pSet.Query = "select * from taxdues where bin = '" + MonitoringFrm.bin1.GetBin() + "' and (due_state = 'N' or due_state = 'Q')";
                            if (pSet.Execute())
                            {
                                if (pSet.Read()) // NEW business (with qtr dec consideration)
                                {
                                    pSet.Query = "SELECT MIN(tax_year) AS min_year FROM taxdues WHERE bin = '" + MonitoringFrm.bin1.GetBin() + "'";
                                }
                                else // RENEWAL/RET
                                {
                                    pSet.Query = "SELECT MIN(tax_year) AS min_year FROM taxdues WHERE bin = '" + MonitoringFrm.bin1.GetBin() + "' and qtr_to_pay = '1'";
                                }
                                if (pSet.Execute())
                                {
                                    if (pSet.Read())
                                    {
                                        sBaseTaxYear = pSet.GetString("min_year");
                                        if (AppSettingsManager.SaveForSoa(MonitoringFrm.bin1.GetBin(), sBaseTaxYear, sUserCode)) { };
                                    }
                                }
                            }
                            pSet.Close();
                        }
                        // (e) ALJ 20111227 save to for SOA Printing

                        UpdateList();

                        //load back system user code
                        if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                        {
                            AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                        }
                    }
                }
            }
        }

        public override void Return()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sUserCode = "";
            string sAction = "";
            string sTaxYear = "";
            int iRow = 0;

            if (MonitoringFrm.txtBnsName.Text.Trim() != "")
            {
                iRow = MonitoringFrm.dgvList.SelectedCells[0].RowIndex;
                if (MonitoringFrm.dgvList[4, iRow].Value != null)
                    sUserCode = MonitoringFrm.dgvList[4, iRow].Value.ToString().Trim();
                if (MonitoringFrm.dgvList[5, iRow].Value != null)
                    sAction = MonitoringFrm.dgvList[5, iRow].Value.ToString().Trim();
                if (MonitoringFrm.dgvList[8, iRow] != null)
                    sTaxYear = MonitoringFrm.dgvList[8, iRow].Value.ToString().Trim();

                if (MessageBox.Show("Are you sure you want to return " + MonitoringFrm.bin1.GetBin() + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // RMC 20120109 added validation of access rights in Treasurer's module Return transaction (s)
                    string sSystemUserName = "";
                    string sSystemUserCode = "";
                    sSystemUserName = AppSettingsManager.SystemUser.UserName;
                    sSystemUserCode = AppSettingsManager.SystemUser.UserCode;

                    string sApprover = "";
                    string sAppGroup = "";

                    frmLogIn fLog = new frmLogIn();
                    fLog.sFormState = "LOGIN";
                    fLog.Text = "Approving Officer";
                    fLog.ShowDialog();

                    if (fLog.m_objSystemUser.UserCode != string.Empty)
                    {
                        pSet.Query = "select * from approver_tbl where usr_code = '" + fLog.m_objSystemUser.UserCode + "'";
                        pSet.Query += " and usr_group <> 'TREASURER'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sApprover = pSet.GetString("usr_code");
                                sAppGroup = pSet.GetString("usr_group");
                            }
                            else
                            {
                                //load back system user code
                                if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                                {
                                    AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                                }

                                MessageBox.Show("You have no rights in this module", "Treasurer's Module", MessageBoxButtons.OK, MessageBoxIcon.Stop); // RMC 20150528 enabled treas module
                                return;
                            }
                        }
                        pSet.Close();
                    }
                    else
                    {
                        //load back system user code
                        if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                        {
                            AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                        }

                        
                        return;
                    }

                    // RMC 20120109 added validation of access rights in Treasurer's module Return transaction (e)

                    if (sAction == "HOLD")
                        sAction = "0";
                    else if (sAction == "RETURN")
                        sAction = "1";

                    frmMemoranda MemoFrm = new frmMemoranda();
                    string sMemoranda = "";

                    while (sMemoranda.Trim() == "")
                    {
                        MemoFrm.m_sQuery = "select memoranda from treasurers_module where bin = '" + MonitoringFrm.bin1.GetBin() + "'";
                        MemoFrm.m_sQuery += " and action = '" + sAction + "' and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                        MemoFrm.ShowDialog();
                        sMemoranda = MemoFrm.m_sMemo;
                    }

                    pSet.Query = "update treasurers_module set action = '1', memoranda = '" + sMemoranda + "' ";
                    pSet.Query += " where bin = '" + MonitoringFrm.bin1.GetBin() + "' and action = '" + sAction + "' ";
                    pSet.Query += " and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    if (AuditTrail.InsertTrail("AUTM-R", "treasurers_module", MonitoringFrm.bin1.GetBin()) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    UpdateList();

                    // RMC 20120109 (S)
                    if (MonitoringFrm.m_objSystemUser.Load(sSystemUserCode))
                    {
                        AppSettingsManager.g_objSystemUser = MonitoringFrm.m_objSystemUser;
                    }
                    // RMC 20120109 (E)
                }
            }
        }

        private bool ValidateIfApproved(string sAppGroup, string sApprover, string sBIN, string sTaxYear)
        {
            // RMC 20120103 modified approval of BTAX monitoring
            OracleResultSet pCheck = new OracleResultSet();

            // RMC 20120105 validate if already approved at bptfo (s)
            if (sAppGroup == "PERMIT")
            {
                pCheck.Query = "select * from TREASURERS_MODULE_TMP where bin = '" + sBIN + "'";
                pCheck.Query += " and tax_year = '" + sTaxYear + "' and app_group = 'LICENSE'";
                if (pCheck.Execute())
                {
                    if (!pCheck.Read())
                    {
                        pCheck.Close();
                        MessageBox.Show("This record should be appoved first at BPTFO", "Treasurer's Module", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                }
            }
            // RMC 20120105 validate if already approved at bptfo (e)

            pCheck.Query = "select * from TREASURERS_MODULE_TMP where bin = '" + sBIN + "'";
            pCheck.Query += " and tax_year = '" + sTaxYear + "' and app_group = '" + sAppGroup + "'";
            if (pCheck.Execute())
            {
                if (pCheck.Read())
                {
                    MessageBox.Show("Record already been approved in this office");
                    pCheck.Close();
                    return false;
                }
                else
                {
                    pCheck.Close();

                    string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                    pCheck.Query = "delete from TREASURERS_MODULE_TMP where ";
                    pCheck.Query += " bin = '" + sBIN + "' and";
                    pCheck.Query += " tax_year = '" + sTaxYear + "' and ";
                    pCheck.Query += " app_group = '" + sAppGroup + "'";
                    if (pCheck.ExecuteNonQuery() == 0)
                    { }

                    pCheck.Query = "insert into TREASURERS_MODULE_TMP values (";
                    pCheck.Query += "'" + sBIN + "',";
                    pCheck.Query += "'" + sTaxYear + "', ";
                    pCheck.Query += "'" + sApprover + "', ";
                    pCheck.Query += "'" + sAppGroup + "', ";
                    pCheck.Query += " to_date('" + sDate + "','MM/dd/yyyy'))";
                    if (pCheck.ExecuteNonQuery() == 0)
                    { }


                }
            }
            pCheck.Close();

            return true;

        }

        private bool ValidateIfApproved(string sBIN, string sTaxYear)
        {
            // RMC 20120103 modified approval of BTAX monitoring
            OracleResultSet pCheck = new OracleResultSet();
            int iCnt = 0;

            pCheck.Query = "select count(*) from TREASURERS_MODULE_TMP where bin = '" + sBIN + "'";
            pCheck.Query += " and tax_year = '" + sTaxYear + "'";
            if (pCheck.Execute())
            {
                if (pCheck.Read())
                    iCnt = pCheck.GetInt(0);
            }
            pCheck.Close();

            //if (iCnt >= 2)
            //if (iCnt >= 1)  // RMC 20131227 FOR MATI 1 APPROVAL ONLY (IN CTO)
            if (iCnt >= m_iCnt)  // RMC 20150811 modified approval in treasurer's module 
                return true;
            else
                return false;
        }

        public override void Refresh()
        {
            // RMC 20120109 added refresh button in Btax monitoring
            UpdateList();
        }

        private bool ValidateMemoranda(string sTaxYear)
        {
            // RMC 20120113 Capture memoranda first before approval in BTax Monitoring
            OracleResultSet pMemo = new OracleResultSet();
            string sMemo = "";

            pMemo.Query = "select * from treasurers_module where bin = '" + MonitoringFrm.bin1.GetBin() + "'";
            pMemo.Query += " and tax_year = '" + sTaxYear + "'";
            if (pMemo.Execute())
            {
                if (pMemo.Read())
                {
                    sMemo = pMemo.GetString("memoranda").Trim();

                    pMemo.Close();

                    if (sMemo == "")
                        return true;
                    else
                        return false;
                }
            }

            return true;
        }
    }
}