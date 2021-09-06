using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.LogIn;
using Amellar.Common.Reports;

namespace TreasurersModule
{
    public partial class frmPenaltyTagging : Form
    {
        public SystemUser m_objSystemUser;
        ArrayList Arrlist_PopulateBin = new ArrayList();
        ArrayList Arrlist_PopulateTaxYear = new ArrayList();

        public frmPenaltyTagging()
        {
            m_objSystemUser = new SystemUser();
            InitializeComponent();
            bin1.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin1.GetDistCode = AppSettingsManager.GetConfigObject("11");
        }

        private void dgvList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgvSender = null;
            if (sender.Equals(dgvList))
            {
                dgvSender = dgvList;
                dgvTagged.ClearSelection();
                btnTagUnTag.Text = "&Tag";
                btnApprove.Enabled = false;
                txtMemoranda.ReadOnly = false;
                chkTagged.Checked = false;
                for (int i = 0; i < dgvTagged.RowCount; i++)
                    dgvTagged.Rows[i].Cells[0].Value = false;
            }
            else
            {
                dgvSender = dgvTagged;
                dgvList.ClearSelection();
                btnTagUnTag.Text = "&Untag";
                btnApprove.Enabled = true;
                txtMemoranda.ReadOnly = true;
                chkTagging.Checked = false;
                for (int i = 0; i < dgvList.RowCount; i++)
                    dgvList.Rows[i].Cells[0].Value = false;
            }
            btnTagUnTag.Enabled = true;

            if (e.RowIndex != -1)
            {
                txtBnsName.Text = AppSettingsManager.GetBnsName(dgvSender.Rows[e.RowIndex].Cells[1].Value.ToString());
                txtBnsAddress.Text = AppSettingsManager.GetBnsAdd(dgvSender.Rows[e.RowIndex].Cells[1].Value.ToString(), "");

                if (sender.Equals(dgvTagged))
                    txtMemoranda.Text = LoadMemoranda(dgvSender.Rows[e.RowIndex].Cells[1].Value.ToString(), dgvSender.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }

        private void frmPenaltyTagging_Load(object sender, EventArgs e)
        {
            FilterControl(false);
            LoadBrgy();
            LoadTaggedRecords();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvTagged.ClearSelection();
            dgvList.Rows.Clear();
            txtBnsAddress.Text = "";
            txtBnsName.Text = "";
            txtMemoranda.Text = "";
            btnTagUnTag.Enabled = false;
            btnApprove.Enabled = false;
            bin1.txtTaxYear.Text = "";
            bin1.txtBINSeries.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sQuery = "";
            bool isChecked = false;
            object oSelectedRB = null;
            foreach (object c in this.Controls)
            {
                if (c is RadioButton)
                    if (((RadioButton)c).Checked)
                    {
                        isChecked = true;
                        oSelectedRB = c;
                        break;
                    }
            }

            if (!isChecked)
            {
                MessageBox.Show("Select Filter by");
                return;
            }

            if (oSelectedRB.Equals(rdoBIN))
                sQuery = "select distinct bin,tax_year From taxdues where bin = '" + bin1.GetBin() + "' order by bin,tax_year";
            else if (oSelectedRB.Equals(rdoBrgy))
            {
                String sBrgy = "";
                if (cmbBrgy.Text == "ALL")
                    sBrgy = "%%";
                else
                    sBrgy = cmbBrgy.Text.Trim() + "%";
                sQuery = "select distinct T.bin,T.tax_year From taxdues T inner join businesses B on B.bin = T.bin where bns_brgy like '" + sBrgy + "' order by T.bin,T.tax_year";
            }
            else
                return;

            OracleResultSet pSet = new OracleResultSet();
            dgvList.Rows.Clear();
            Arrlist_PopulateTaxYear.Clear();
            Arrlist_PopulateBin.Clear();
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                    dgvList.Rows.Add(false, pSet.GetString(0), pSet.GetString(1));
            pSet.Close();

            if (dgvList.RowCount <= 0)
                MessageBox.Show("No record found!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rdoBIN_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(rdoBIN))
            {
                FilterControl(true);
                cmbBrgy.Enabled = false;
                bin1.txtTaxYear.Focus();
            }
            else if (sender.Equals(rdoBrgy))
            {
                FilterControl(false);
                cmbBrgy.Enabled = true;
                cmbBrgy.Focus();
            }
        }

        private void cmbBrgy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void FilterControl(bool blnEnable)
        {
            bin1.txtTaxYear.Enabled = blnEnable;
            bin1.txtBINSeries.Enabled = blnEnable;
            cmbBrgy.Enabled = blnEnable;
        }

        private void LoadBrgy()
        {
            cmbBrgy.Items.Clear();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select * from brgy order by brgy_nm";
            if (result.Execute())
            {
                cmbBrgy.Items.Add("ALL");
                while (result.Read())
                    cmbBrgy.Items.Add(result.GetString("brgy_nm").Trim());
            }
            result.Close();
        }

        private void LoadTaggedRecords()
        {
            String sValue = "";
            dgvTagged.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from waive_penalty order by bin,tax_year";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    sValue = pSet.GetInt(3).ToString();
                    if (sValue == "1")
                        sValue = "Approved";
                    else
                        sValue = "Pending";
                    dgvTagged.Rows.Add(false, pSet.GetString(0), pSet.GetString(1), sValue);
                }
            pSet.Close();
            Arrlist_PopulateTaxYear.Clear();
            Arrlist_PopulateBin.Clear();

            dgvTagged.ClearSelection();
            txtBnsAddress.Text = "";
            txtBnsName.Text = "";
            txtMemoranda.Text = "";
            btnTagUnTag.Enabled = false;
            btnApprove.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string LoadMemoranda(String pBin, String pTaxyear)
        {
            String sValue = "";
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from waive_penalty where bin = '" + pBin + "' and tax_year = '" + pTaxyear + "'";
            if (pSet.Execute())
                if (pSet.Read())
                    sValue = StringUtilities.RemoveApostrophe(pSet.GetString(2).Trim());
            pSet.Close();
            return sValue;
        }

        private bool PopulateChoices(DataGridView pDgvSender)
        {
            Arrlist_PopulateBin.Clear();
            Arrlist_PopulateTaxYear.Clear();

            bool isChecked = false;
            if (pDgvSender.Equals(dgvList))
            {
                for (int i = 0; i < dgvList.RowCount; i++)
                {
                    if ((bool)dgvList.Rows[i].Cells[0].Value)
                    {
                        Arrlist_PopulateBin.Add(dgvList.Rows[i].Cells[1].Value);
                        Arrlist_PopulateTaxYear.Add(dgvList.Rows[i].Cells[2].Value);
                        isChecked = true;
                    }
                }
            }
            else //dgvTag
            {
                for (int i = 0; i < dgvTagged.RowCount; i++)
                {
                    if ((bool)dgvTagged.Rows[i].Cells[0].Value)
                    {
                        Arrlist_PopulateBin.Add(dgvTagged.Rows[i].Cells[1].Value);
                        Arrlist_PopulateTaxYear.Add(dgvTagged.Rows[i].Cells[2].Value);
                        isChecked = true;
                    }
                }
            }

            if (isChecked)
                return true;
            else
            {
                MessageBox.Show("Select Bin(s) first!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        private bool CheckMemoranda()
        {
            if (txtMemoranda.Text.Trim() == "")
            {
                MessageBox.Show("Memoranda is required!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;    
            }

            return true;
        }

        private void btnTagUnTag_Click(object sender, EventArgs e)
        {
            String sValue = "";
                
            if (btnTagUnTag.Text == "&Tag")
            {
                if (!PopulateChoices(dgvList))
                    return;
                
                if (!CheckMemoranda())
                    return;

                for (int i = 0; i < Arrlist_PopulateBin.Count; i++)
                    sValue += Arrlist_PopulateBin[i].ToString() + "(" + Arrlist_PopulateTaxYear[i].ToString() + ") /";
                sValue = sValue.Remove(sValue.Length - 3, 3);

                Saving("Tagging");
                btnTagUnTag.Enabled = false;
                if (AuditTrail.InsertTrail("WSPB", "waive_penalty", "Tagging of " + sValue) == 0)
                    return;
            }
            else //&Untag
            {
                if (!PopulateChoices(dgvTagged))
                    return;

                for (int i = 0; i < Arrlist_PopulateBin.Count; i++)
                    sValue += Arrlist_PopulateBin[i].ToString() + "(" + Arrlist_PopulateTaxYear[i].ToString() + ") / ";
                sValue = sValue.Remove(sValue.Length - 3, 3);

                Saving("UnTagging");
                btnTagUnTag.Enabled = false;

                if (AuditTrail.InsertTrail("WSPB", "waive_penalty", "Untagging of " + sValue) == 0)
                    return;
            }

            LoadTaggedRecords();
        }

        private void Saving(String pAction)
        {
            String sValue = "";
            String sQuery = "";

            OracleResultSet pSet = new OracleResultSet();
            if (pAction == "Tagging")
            {
                for (int i = 0; i < Arrlist_PopulateBin.Count; i++)
                {
                    if (CheckifAlreadyTagged(Arrlist_PopulateBin[i].ToString(), Arrlist_PopulateTaxYear[i].ToString()))
                        continue;

                    sQuery = "insert into waive_penalty values ('" + Arrlist_PopulateBin[i].ToString() + "','" + Arrlist_PopulateTaxYear[i].ToString() + "','" + StringUtilities.RemoveApostrophe(txtMemoranda.Text) + "',0)";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
                sValue = "Tagged.";
            }
            else if (pAction == "UnTagging")
            {
                for (int i = 0; i < Arrlist_PopulateBin.Count; i++)
                {
                    sQuery = "delete from waive_penalty where bin = '" + Arrlist_PopulateBin[i].ToString() + "' and tax_year = '" + Arrlist_PopulateTaxYear[i].ToString() + "'";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
                sValue = "Untagged.";
            }
            else //Approving
            {
                for (int i = 0; i < Arrlist_PopulateBin.Count; i++)
                {
                    if (CheckifAlreadyApproved(Arrlist_PopulateBin[i].ToString(), Arrlist_PopulateTaxYear[i].ToString()))
                        continue;

                    sQuery = "update waive_penalty set status = 1 where bin = '" + Arrlist_PopulateBin[i].ToString() + "' and tax_year = '" + Arrlist_PopulateTaxYear[i].ToString() + "'";
                    pSet.Query = sQuery;
                    pSet.ExecuteNonQuery();
                }
                sValue = "Approved.";
            }

            MessageBox.Show("BIN(s) successfully " + sValue, " ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool CheckifAlreadyTagged(String pBin, String pTaxyear)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "Select * from waive_penalty where bin = '" + pBin + "' and tax_year = '" + pTaxyear + "'";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    //MessageBox.Show("BIN already Tagged!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }

        private bool CheckifAlreadyApproved(String pBin, String pTaxyear)
        {
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "Select * from waive_penalty where bin = '" + pBin + "' and tax_year = '" + pTaxyear + "' and Status = 1";
            if (pSet.Execute())
                if (pSet.Read())
                {
                    //MessageBox.Show("BIN Already Approved!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pSet.Close();
                    return true;
                }
            pSet.Close();
            return false;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (!PopulateChoices(dgvTagged))
                return;

            if (MessageBox.Show("Are you sure you want to approve?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sSystemUserName = "";
                string sSystemUserCode = "";
                sSystemUserName = AppSettingsManager.SystemUser.UserName;
                sSystemUserCode = AppSettingsManager.SystemUser.UserCode;

                // (s) ALJ 20110907 re-assessment validation - Approving Officers
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

                if (m_objSystemUser.Load(sSystemUserCode))
                    AppSettingsManager.g_objSystemUser = m_objSystemUser;

                if (!bAppoved)
                {
                    MessageBox.Show("Approval failed.", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                // (e) ALJ 20110907 re-assessment validation - Approving Officers

                Saving("Approving");
                LoadTaggedRecords();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmCompromiseAgreement dlg = new frmCompromiseAgreement();
            dlg.ReportTitle = "LIST OF BUSINESSES WITH WAIVED SURCHARGE AND PENALTY";
            dlg.ShowDialog();
        }

        private void chkTagging_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(chkTagging))
            {
                if (chkTagging.Checked)
                {
                    for (int i = 0; i < dgvList.RowCount; i++)
                        dgvList.Rows[i].Cells[0].Value = true;
                }
                else
                {
                    for (int i = 0; i < dgvList.RowCount; i++)
                        dgvList.Rows[i].Cells[0].Value = false;
                }
            }
            else
            {
                if (chkTagged.Checked)
                {
                    for (int i = 0; i < dgvTagged.RowCount; i++)
                        dgvTagged.Rows[i].Cells[0].Value = true;
                }
                else
                {
                    for (int i = 0; i < dgvTagged.RowCount; i++)
                        dgvTagged.Rows[i].Cells[0].Value = false;
                }
            }
        }

    }
}