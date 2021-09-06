

// RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table
// RMC 20120313 changed 'date sent' to 'date received' in notice tagging



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Modules.BusinessReports;

namespace Amellar.Modules.InspectorsDetails
{
    public class OfficialTaggingUnofficial:OfficialTagging
    {
        public OfficialTaggingUnofficial(frmOfficialTagging Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            TaggingFrm.btnSearch.Visible = false;
            TaggingFrm.groupBox1.Enabled = false; // GDE 20120502
            // RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table (s)
            OracleResultSet pCmd = new OracleResultSet();

            pCmd.Query = "delete from unoff_with_notice where is_number is null";
            if (pCmd.ExecuteNonQuery() == 0)
            { }
            // RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table (e)

            if (m_sWatcher == "ONE")
                OneByOne();
            else if (m_sWatcher == "")
                TaggingFrm.chkWNotice.Checked = true;
        }

        public override void OneByOne()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;
            int iCount = 0;
            string sIsNum = string.Empty;
            string sBnsNm = string.Empty;
            string sOwnCode = string.Empty;
            string sOwner = string.Empty;

            TaggingFrm.dgvBnsInfo.Columns.Clear();
            TaggingFrm.dgvBnsInfo.Columns.Add("ISNUM", "IS NUMBER");
            TaggingFrm.dgvBnsInfo.Columns.Add("BNSNAME", "BUSINESS NAME");
            TaggingFrm.dgvBnsInfo.Columns.Add("OWNNAME", "OWNER'S NAME");
            TaggingFrm.dgvBnsInfo.RowHeadersVisible = false;
            TaggingFrm.dgvBnsInfo.Columns[0].Width = 150;
            TaggingFrm.dgvBnsInfo.Columns[1].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[2].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            
            pSet.Query = string.Format("select * from unofficial_info_tbl where is_number in (select trim(is_number) from unofficial_dtls) and is_number = '{0}'", m_sISNum);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("is_number");
                    sBnsNm = pSet.GetString("Bns_Nm");

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                    }

                    sOwnCode = pSet.GetString("own_code");
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;
                    TaggingFrm.m_sBnsType = pSet.GetString("bns_type");
                    iCount++;
                    iRow++;
                }
                TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            }
            pSet.Close();

            LoadNotice(m_sISNum);


        }

        private void LoadNotice(string sIsNum)
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;
            string sNoticeDate = string.Empty;
            string sNoticeSent = string.Empty;
            string sNoticeNum = string.Empty;

            GetBnsAddress();

            TaggingFrm.dgvNotice.Columns.Clear();
            TaggingFrm.dgvNotice.Columns.Add("NO", "NOTICE NO.");
            TaggingFrm.dgvNotice.Columns.Add("ISSUED", "DATE ISSUED");
            //TaggingFrm.dgvNotice.Columns.Add("SENT", "DATE SENT");
            TaggingFrm.dgvNotice.Columns.Add("SENT", "DATE RECEIVED");  // RMC 20120313 changed 'date sent' to 'date received' in notice tagging
            TaggingFrm.dgvNotice.RowHeadersVisible = false;
            TaggingFrm.dgvNotice.Columns[0].Width = 100;
            TaggingFrm.dgvNotice.Columns[1].Width = 120;
            TaggingFrm.dgvNotice.Columns[2].Width = 120;
            TaggingFrm.dgvNotice.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            TaggingFrm.dgvNotice.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvNotice.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pSet.Query = string.Format("select * from unofficial_notice_closure where is_number = '{0}' order by Notice_Number", sIsNum);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sNoticeNum = pSet.GetString("Notice_number").Trim();
                    sNoticeDate = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("Notice_Date"));

                    if (pSet.GetDateTime("Notice_sent") == DateTime.Now)
                        sNoticeSent = "";
                    else
                        sNoticeSent = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("Notice_sent"));

                    TaggingFrm.dgvNotice.Rows.Add("");
                    TaggingFrm.dgvNotice[0, iRow].Value = sNoticeNum;
                    TaggingFrm.dgvNotice[1, iRow].Value = sNoticeDate;
                    TaggingFrm.dgvNotice[2, iRow].Value = sNoticeSent;

                    iRow++;
                }
            }
            pSet.Close();
        }

        private void GetBnsAddress()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBnsNum = string.Empty;
            string sBnsStreet = string.Empty;
            string sBnsBrgy = string.Empty;
            string sBnsZone = string.Empty;
            string sBnsDist = string.Empty;
            string sBnsMun = string.Empty;
            string sBnsProv = string.Empty;

            pSet.Query = string.Format("select * from unofficial_info_tbl where trim(is_number) = '{0}'", m_sISNum);
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsNum = pSet.GetString("Bns_house_no").Trim();
                    sBnsStreet = pSet.GetString("Bns_street").Trim();
                    sBnsBrgy = pSet.GetString("Bns_brgy").Trim();
                    sBnsZone = pSet.GetString("Bns_zone").Trim();
                    sBnsDist = pSet.GetString("Bns_dist").Trim();
                    sBnsMun = pSet.GetString("Bns_mun").Trim();
                    sBnsProv = pSet.GetString("Bns_prov").Trim();

                    TaggingFrm.txtBnsAdd.Text = sBnsNum + " " + sBnsStreet + ", " + sBnsBrgy + " " + sBnsZone + ", " + sBnsDist + ", " + sBnsMun;
                }

            }
            pSet.Close();
        }

        public override void OnWithNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;
            int iCount = 0;
            string sIsNum = string.Empty;
            string sBnsNm = string.Empty;
            string sOwnCode = string.Empty;
            string sOwner = string.Empty;

            TaggingFrm.dgvBnsInfo.Columns.Clear();
            TaggingFrm.dgvBnsInfo.Columns.Add("ISNUM", "IS NUMBER");
            TaggingFrm.dgvBnsInfo.Columns.Add("BNSNAME", "BUSINESS NAME");
            TaggingFrm.dgvBnsInfo.Columns.Add("OWNNAME", "OWNER'S NAME");
            TaggingFrm.dgvBnsInfo.RowHeadersVisible = false;
            TaggingFrm.dgvBnsInfo.Columns[0].Width = 150;
            TaggingFrm.dgvBnsInfo.Columns[1].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[2].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pSet.Query = string.Format("select * from unofficial_info_tbl where is_number in (select trim(is_number) from unoff_with_notice where is_number in (select trim(is_number) from unofficial_dtls where inspector_code = '{0}')) order by IS_NUMBER", m_sIns);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("is_number").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                    }

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    iRow++;
                    iCount++;
                }

                if (iRow == 0)
                {
                    MessageBox.Show("No Unofficial Business with Notice.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            pSet.Close();

            TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            LoadNotice(m_sISNum);

        }

        public override void CheckWNotice()
        {
            if (TaggingFrm.chkWNotice.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWONotice.Checked = false;
                TaggingFrm.chkWTag.Checked = false;

                OnWithNotice();

            }
        }

        public override void CheckWONotice()
        {
            if (TaggingFrm.chkWONotice.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWTag.Checked = false;

                OnWithoutNotice();
            }
        }

        public override void CheckWTag()
        {
            if (TaggingFrm.chkWTag.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWONotice.Checked = false;

                OnWithTag();
            }
        }

        public override void OnWithoutNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;
            int iCount = 0;
            string sIsNum = string.Empty;
            string sBnsNm = string.Empty;
            string sOwnCode = string.Empty;
            string sOwner = string.Empty;

            TaggingFrm.dgvBnsInfo.Columns.Clear();
            TaggingFrm.dgvBnsInfo.Columns.Add("ISNUM", "IS NUMBER");
            TaggingFrm.dgvBnsInfo.Columns.Add("BNSNAME", "BUSINESS NAME");
            TaggingFrm.dgvBnsInfo.Columns.Add("OWNNAME", "OWNER'S NAME");
            TaggingFrm.dgvBnsInfo.RowHeadersVisible = false;
            TaggingFrm.dgvBnsInfo.Columns[0].Width = 150;
            TaggingFrm.dgvBnsInfo.Columns[1].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[2].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pSet.Query = string.Format("select * from unofficial_info_tbl where is_number not in (select trim(is_number) from unoff_with_notice) and is_number in (select trim(is_number) from unofficial_dtls where inspector_code = '{0}') order by is_number", m_sIns);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("is_number").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                    }

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    iRow++;
                    iCount++;
                }
            }
            pSet.Close();

            TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            LoadNotice(m_sISNum);


        }

        public override void OnWithTag()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;
            int iCount = 0;
            string sIsNum = string.Empty;
            string sBnsNm = string.Empty;
            string sOwnCode = string.Empty;
            string sOwner = string.Empty;

            TaggingFrm.dgvBnsInfo.Columns.Clear();
            TaggingFrm.dgvBnsInfo.Columns.Add("ISNUM", "IS NUMBER");
            TaggingFrm.dgvBnsInfo.Columns.Add("BNSNAME", "BUSINESS NAME");
            TaggingFrm.dgvBnsInfo.Columns.Add("OWNNAME", "OWNER'S NAME");
            TaggingFrm.dgvBnsInfo.RowHeadersVisible = false;
            TaggingFrm.dgvBnsInfo.Columns[0].Width = 150;
            TaggingFrm.dgvBnsInfo.Columns[1].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[2].Width = 200;
            TaggingFrm.dgvBnsInfo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            TaggingFrm.dgvBnsInfo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pSet.Query = "select * from unofficial_info_tbl where is_number in (select trim(is_number) from norec_closure_tagging) order by is_number";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("is_number").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                    }

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    iRow++;
                    iCount++;
                }

            }
            pSet.Close();

            TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            LoadNotice(m_sISNum);
        }

        public override void ButtonIssueNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iNoticeNum = 0;

            pSet.Query = string.Format("select trim(is_number) from unoff_with_notice where is_number = '{0}'", m_sISNum);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    string sConfig = "";

                    sConfig = AppSettingsManager.GetConfigValue("19");

                    pSet.Query = string.Format("select count(*) from unofficial_notice_closure where is_number = '{0}'", m_sISNum);
                    int.TryParse(pSet.ExecuteScalar().ToString(), out iNoticeNum);

                    if(iNoticeNum == 0)
                    {
                        MessageBox.Show("You cannot issue notice to this business because this business has been\nalready tagged for closure","Issue Notice",MessageBoxButtons.OK, MessageBoxIcon.Stop);
    				    return;
                    }

                    if (iNoticeNum < Convert.ToInt32(sConfig))
                    {
                        IssueNotice();
                    }
                    else
                    {
                        MessageBox.Show("You cannot issue another notice because you reach \nthe maximum number of " + sConfig + ".");
                        return;
                    }
                }
                else
                {
                    pSet.Close();

                    if (m_sISNum != "") // RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table
                    {
                        pSet.Query = "insert into unoff_with_notice values (:1)";
                        pSet.AddParameter(":1", m_sISNum);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        IssueNotice();
                    }
                }
            }
        }

        private void IssueNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sNoticeSent = string.Empty;
            string sNoticeNum = string.Empty;
            string sRealNum = string.Empty;
            string sCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
            int iNoticeNum = 0;

            DateTime dtNoticeDate = AppSettingsManager.GetCurrentDate();
            frmSendNotice frmSendNotice = new frmSendNotice();
            frmSendNotice.NoticeSwitch = "ISSUE";
            frmSendNotice.ShowDialog();
            dtNoticeDate = frmSendNotice.NoticeDate;
            sCurrentDate = string.Format("{0:MM/dd/yyyy}", dtNoticeDate);

            pSet.Query = string.Format("select * from unofficial_notice_closure where is_number = '{0}' order by notice_number", m_sISNum);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from unofficial_notice_closure where is_number = '{0}' order by notice_number", m_sISNum);
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            sNoticeSent = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("Notice_Sent"));

                            if (pSet.GetDateTime("Notice_Sent") == DateTime.Now)
                                sNoticeSent = "";

                            iNoticeNum++;
                        }
                        sNoticeNum = string.Format("{0:###}", iNoticeNum + 1);
                        sRealNum = string.Format("{0:###}", iNoticeNum);

                    }
                    pSet.Close();

                    if (sNoticeSent != "")
                    {
                        pSet.Query = "insert into unofficial_notice_closure (is_number, notice_date, Notice_Number) values (:1, to_date(:2,'MM/dd/yyyy'),:3)";
                        pSet.AddParameter(":1", m_sISNum);
                        pSet.AddParameter(":2", sCurrentDate);
                        pSet.AddParameter(":3", sNoticeNum);
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        if (AuditTrail.InsertTrail("ABIDUI-ISS", "unofficial_notice_closure", m_sISNum) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("You cannot Issue another notice because Notice " + sRealNum + " has not been sent.", "Issue Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }


                }
                else
                {
                    pSet.Close();

                    pSet.Query = "insert into unofficial_notice_closure (IS_number, notice_date, Notice_Number) values (:1, to_date(:2,'MM/dd/yyyy'),'1')";
                    pSet.AddParameter(":1", m_sISNum);
                    pSet.AddParameter(":2", sCurrentDate);
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    if (AuditTrail.InsertTrail("ABIDUI-ISS", "unofficial_notice_closure", m_sISNum) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            pSet.Close();

            LoadNotice(m_sISNum);
    
        }

        public override void ButtonSendNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sNoticeSent = string.Empty;
            string sNoticeNum = string.Empty;
            try
            {
                sNoticeNum = TaggingFrm.dgvNotice[0, TaggingFrm.dgvNotice.SelectedCells[0].RowIndex].Value.ToString().Trim();
            }
            catch
            {
                sNoticeNum = "";
            }

            pSet.Query = string.Format("select * from unofficial_notice_closure where is_number = '{0}' and notice_number = '{1}'", m_sISNum, sNoticeNum);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    if (pSet.GetDateTime("Notice_Sent") == DateTime.Now)
                        sNoticeSent = "";
                    else
                        sNoticeSent = string.Format("{0:MM/dd/yyyy}", pSet.GetDateTime("Notice_Sent"));

                    using (frmSendNotice frmSendNotice = new frmSendNotice())
                    {
                        frmSendNotice.Bin = m_sISNum;
                        frmSendNotice.NoticeNum = sNoticeNum;
                        frmSendNotice.NoticeDate = pSet.GetDateTime("notice_date");

                        if (sNoticeSent == "")
                        {
                            frmSendNotice.Source = "UnOfficial";
                            frmSendNotice.ShowDialog();
                            LoadNotice(m_sISNum);
                        }
                        else
                        {
                            if (MessageBox.Show("Notice Number " + sNoticeNum + " already been sent.\nDo you want to change the Notice Sent date?", "Sending of Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                frmSendNotice.Source = "UnOfficial";
                                frmSendNotice.ShowDialog();
                                LoadNotice(m_sISNum);
                                MessageBox.Show("You have successfully changed the notice date sent.", "Official Business Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (AuditTrail.InsertTrail("ABIDUI-SEN", "unofficial_notice_closure", m_sISNum) == 0)
                                {
                                    pSet.Rollback();
                                    pSet.Close();
                                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                                MessageBox.Show("No changes has been made.", "Sending Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You cannot send a notice now, try to issue a notice first.", "Sending Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            pSet.Close();
        }

        public override void ButtonDeleteNotice()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sNoticeNum = string.Empty;
            int iNoticeNum = 0;

            pSet.Query = string.Format("select count(*) from unofficial_notice_closure where is_number = '{0}' ", m_sISNum);
            int.TryParse(pSet.ExecuteScalar().ToString(), out iNoticeNum);

            sNoticeNum = TaggingFrm.dgvNotice[0, TaggingFrm.dgvNotice.SelectedCells[0].RowIndex].Value.ToString();

            if (Convert.ToInt32(sNoticeNum) < iNoticeNum)
            {
                MessageBox.Show("You cannot delete Notice Number " + sNoticeNum + ".", "Deleting Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                if (sNoticeNum != "")
                    DeleteNotice(sNoticeNum);
            }

            OnWithNotice();
    
        }

        private void DeleteNotice(string sNoticeNum)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (MessageBox.Show("Are you sure you want to delete Notice Number " + sNoticeNum + "?", "Delete Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (sNoticeNum == "1")
            {
                pSet.Query = string.Format("delete from unofficial_notice_closure where is_number = '{0}' and notice_number = '{1}'", m_sISNum, sNoticeNum);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from unoff_with_notice where is_number = '{0}'", m_sISNum);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                if (AuditTrail.InsertTrail("ABIDUI-DEL", "multiple table", m_sISNum) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                pSet.Query = string.Format("delete from unofficial_notice_closure where is_number = '{0}' and notice_number = '{1}'", m_sISNum, sNoticeNum);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                if (AuditTrail.InsertTrail("ABIDUI-DEL", "unofficial_notice_closure", m_sBin) == 0)
                {
                    pSet.Rollback();
                    pSet.Close();
                    MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            LoadNotice(m_sBin);
    
        }

        public override void ButtonGenerate()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            string sRemarks = string.Empty;
            string sAddl = string.Empty;

            frmBussReport PrintNotice = new frmBussReport();

            pSet.Query = string.Format("select * from unofficial_dtls where is_number = '{0}'", m_sISNum);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sRemarks = pSet.GetString("Inspector_remarks").Trim();
                    sAddl = pSet.GetString("Addition_info").Trim();

                    //PrintClass = new PrintInspection();
                    frmPrintInspection PrintClass = new frmPrintInspection();   // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter
                    PrintClass.Source = "6";
                    PrintClass.ReportName = "INSPECTOR DETAILS";
                    PrintClass.Remarks = sRemarks;
                    PrintClass.AddlRemarks = sAddl;
                    PrintClass.BnsAdd = TaggingFrm.txtBnsAdd.Text.Trim();
                    PrintClass.BnsName = TaggingFrm.txtBnsName.Text.Trim();
                    PrintClass.BnsOwner = TaggingFrm.dgvBnsInfo.CurrentRow.Cells[2].Value.ToString();
                    //MCR ADD 20140515
                    PrintClass.BnsType = TaggingFrm.m_sBnsType;
                    
                    PrintClass.DateCover = pSet.GetString("date_inspected");

                    string sOwnCode = "";
                    pRec.Query = string.Format("select * from unofficial_info_tbl where is_number = '{0}'", m_sISNum);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sOwnCode = pRec.GetString("own_code");
                            /*PrintClass.Owner = AppSettingsManager.GetBnsOwner(sOwnCode);*/
                        }
                    }
                    pRec.Close();

                    // use calamba format
                    //PrintNotice.OwnCode = sOwnCode;
                    //PrintNotice.OwnCode = "";   // RMC 20120222 added printing of notice for un-official business in business mapping
                    //PrintNotice.NoticeDate = Convert.ToDateTime(pSet.GetString("date_inspected"));
                    //PrintNotice.ReportSwitch = "Violation";
                    //PrintNotice.ShowDialog();
                    int i = 0;
                    string sWatch = string.Empty;

                    pRec.Query = string.Format("Select count(*) from unofficial_notice_closure where is_number = '{0}'", m_sISNum);
                    int.TryParse(pRec.ExecuteScalar().ToString(), out i);

                    sWatch = string.Format("{0:##}", i);

                    //if (sWatch == "1")
                    if (sWatch == "1" || sWatch == "0" || sWatch == "") // RMC 20151005 corrections in printing Notice of Unofficial business
                    {
                        PrintClass.NoticeNum = "First";
                        PrintClass.ReportName = " N O T I C E";
                    }
                    if (sWatch == "2")
                    {
                        PrintClass.NoticeNum = "Second";
                        PrintClass.ReportName = "F I N A L   N O T I C E";
                    }
                    if (sWatch == "3")
                    {
                        PrintClass.NoticeNum = "Third";
                        PrintClass.ReportName = "C E A S E   A N D   D E S I S T   O R D E R";
                    }

                    PrintClass.m_sISNum = m_sISNum;
                    PrintClass.m_sBIN = m_sBin;
                    //PrintClass.FormLoad();
                    PrintClass.ShowDialog();    // RMC 20171201 modified printing of Inspection report and Notices, changed to vsprinter

                    if (AuditTrail.InsertTrail("ABIDUI-GEN", "multiple table", m_sISNum) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("No Notice yet.", "Generating Report", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            pSet.Close();

        }

        public override void CellClickBnsInfo(int iRow)
        {
            TaggingFrm.txtBnsName.Text = TaggingFrm.dgvBnsInfo[1, iRow].Value.ToString().Trim();
            m_sISNum = TaggingFrm.dgvBnsInfo[0, iRow].Value.ToString().Trim();
            LoadNotice(m_sISNum);
        }

    }
}
