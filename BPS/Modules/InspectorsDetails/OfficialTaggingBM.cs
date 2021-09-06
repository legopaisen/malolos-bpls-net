
// RMC 20120418 added searching by address in Business mappin printing of notice
// RMC 20120417 Modified Final notice format
// RMC 20120329 Modifications in Notice of violation
// RMC 20120313 changed 'date sent' to 'date received' in notice tagging
// RMC 20120222 added printing of notice for un-official business in business mapping

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Modules.BusinessReports;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.InspectorsDetails
{
    public class OfficialTaggingBM : OfficialTagging
    {
        private string m_sQuery = "";   // RMC 20120329 Modifications in Notice of violation
        private bool m_bInsertNotice = false;
        DateTime dFrom = new DateTime();
        DateTime dTo = new DateTime();
        public OfficialTaggingBM(frmOfficialTagging Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            TaggingFrm.btnSearch.Visible = true;

            // RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table (s)
            OracleResultSet pCmd = new OracleResultSet();

            pCmd.Query = "delete from unoff_with_notice where is_number is null";
            if (pCmd.ExecuteNonQuery() == 0)
            { }
            // RMC 20120418 added auto-correct of null data inserted in unoff_with_notice table (e)

            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            m_sBnsAdd = "";
            m_sOwnName = "";
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            
         //   if (m_sWatcher == "ALL")
            if (m_sWatcher != "")    // RMC 20120417 Modified Final notice format
            {
                ListAll();
                TaggingFrm.chkWONotice.Checked = true;
            }
            else if (m_sWatcher == "")
                TaggingFrm.chkWNotice.Checked = true;

            // RMC 20120417 Modified Final notice format (s)
            if (AppSettingsManager.GetConfigValue("19") == "2")
            {
                TaggingFrm.chkW2ndNotice.Enabled = false;
            }
            else
            {
                TaggingFrm.chkW2ndNotice.Enabled = true;
            }
            // RMC 20120417 Modified Final notice format (e)
        }

        private void ListAll()
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

            // GDE 20120416 added
            if (m_sBnsNm != string.Empty)
            {
                pSet.Query = "select * from btm_temp_businesses where bns_nm like '%" + m_sBnsNm + "%' ";
                pSet.Query += " and trim(bin) is null";   // RMC 20120417 Modified Final notice format
                pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
                pSet.Query += " order by bns_brgy, tbin";
            }
            else
            // GDE 20120416 added
            {
                pSet.Query = "select * from btm_temp_businesses where trim(bin) is null";
                pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
                pSet.Query += " order by bns_brgy, tbin";
            }
            if (pSet.Execute())
            {
                int iCnt = 0;
                while (pSet.Read())
                {
                    iCnt++;
                    sIsNum = pSet.GetString("tbin");
                    sBnsNm = pSet.GetString("Bns_Nm");

                    /*if (iCnt == 1)
                        m_sISNum = sIsNum;
                    */
                    // RMC 20120418 added searching by address in Business mappin printing of notice

                    

                    sOwnCode = pSet.GetString("own_code");
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;  // RMC 20120418 added searching by address in Business mappin printing of notice
                    }

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

            pSet.Query = string.Format("select * from btm_temp_businesses where tbin = '{0}'", m_sISNum);
            if (pSet.Execute())
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


                    // GDE 20120426 edit bns brgy {
                    if(sBnsZone.Trim() == string.Empty)
                        TaggingFrm.txtBnsAdd.Text = sBnsNum + " " + sBnsStreet + ", " + sBnsBrgy + " " + sBnsDist + ", " + sBnsMun;
                    else if (sBnsDist.Trim() == string.Empty)
                        TaggingFrm.txtBnsAdd.Text = sBnsNum + " " + sBnsStreet + ", " + sBnsBrgy + " " + sBnsZone + ", " + sBnsMun;
                    else if (sBnsDist.Trim() == string.Empty && sBnsZone.Trim() == string.Empty)
                        TaggingFrm.txtBnsAdd.Text = sBnsNum + " " + sBnsStreet + ", " + sBnsBrgy + " " + sBnsMun;
                    else
                    // GDE 20120426 edit bns brgy }
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

            //pSet.Query = "select * from btm_temp_businesses where tbin in (select trim(is_number) from unoff_with_notice ) order by tbin";

            // RMC 20120329 Modifications in Notice of violation (s)
            // 1st notice
            pSet.Query = "select * from btm_temp_businesses where tbin in ";
            pSet.Query+= "(select is_number from unofficial_notice_closure having max(notice_number) = 1 group by is_number) "; // GDE 20120507
            //pSet.Query += "(select is_number from unofficial_notice_closure having max(notice_number) = 1 and notice_date between '" + dFrom + "' and '" + dTo + "' group by is_number) ";
            pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            if (m_sBnsNm != "")
                pSet.Query += " and bns_nm like '%" + StringUtilities.HandleApostrophe(m_sBnsNm) + "%'";
            if (m_sOwnName != "")
                pSet.Query += " and own_code in (select own_code from own_names where own_ln || ' ' || own_fn || ' ' || own_mi like '%" + m_sOwnName + "%')";
            if (m_sBnsAdd != "")
            {
                pSet.Query += " and bns_house_no || ' ' || bns_street || ', ' || bns_brgy ";
                pSet.Query += " || ' ' || bns_zone || ', ' || bns_dist || ', ' || bns_mun like '%" + StringUtilities.HandleApostrophe(m_sBnsAdd) + "%'";
            }
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            //pSet.Query += "and trim(bin) is null order by tbin"; // GDE 20120507 remove order by
            pSet.Query += "and trim(bin) is null";
            m_sQuery = pSet.Query.ToString();
           
            // RMC 20120329 Modifications in Notice of violation (e)
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("tbin").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;
                    }

                    iRow++;
                    iCount++;
                }

                if (iRow == 0)
                {
                    //MessageBox.Show("No Unofficial Business with Notice.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20120329 Modifications in Notice of violation
                    m_sISNum = "";
                    //return;
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
                TaggingFrm.chkW2ndNotice.Checked = false;
                TaggingFrm.chkWClosure.Checked = false;

                m_sOwnName = "";
                m_sBnsAdd = "";
                m_sBnsNm = "";
                OnWithNotice();

            }
        }

        public override void CheckWONotice()
        {
            if (TaggingFrm.chkWONotice.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWTag.Checked = false;
                TaggingFrm.chkW2ndNotice.Checked = false;
                TaggingFrm.chkWClosure.Checked = false;

                m_sOwnName = "";
                m_sBnsAdd = "";
                m_sBnsNm = "";
                OnWithoutNotice();
            }
        }

        public override void CheckWTag()
        {
            if (TaggingFrm.chkWTag.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWONotice.Checked = false;
                TaggingFrm.chkW2ndNotice.Checked = false;
                TaggingFrm.chkWClosure.Checked = false;

                m_sOwnName = "";
                m_sBnsAdd = "";
                m_sBnsNm = "";
                OnWithTag();
            }
        }

        public override void CheckWClosureNotice()
        {
            if (TaggingFrm.chkWClosure.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWONotice.Checked = false;
                TaggingFrm.chkW2ndNotice.Checked = false;
                TaggingFrm.chkWTag.Checked = false;

                m_sOwnName = "";
                m_sBnsAdd = "";
                m_sBnsNm = "";
                OnWClosureNotice();
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

            pSet.Query = "select * from btm_temp_businesses where tbin not in (select trim(is_number) from unoff_with_notice) and "; // GDE 20120507 disregard the tag state as per eric // GDE 20120509 
            //pSet.Query = "select * from btm_temp_businesses where "; // GDE 20120509 filter businesses without notice
            //pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
            pSet.Query += "bns_brgy = '" + m_sWatcher + "'";
            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            if (m_sBnsNm != "")
                pSet.Query += " and bns_nm like '%" + StringUtilities.HandleApostrophe(m_sBnsNm) + "%'";
            if (m_sOwnName != "")
                pSet.Query += " and own_code in (select own_code from own_names where own_ln || ' ' || own_fn || ' ' || own_mi like '%" + m_sOwnName + "%')";
            if (m_sBnsAdd != "")
            {
                pSet.Query += " and bns_house_no || ' ' || bns_street || ', ' || bns_brgy ";
                pSet.Query += " || ' ' || bns_zone || ', ' || bns_dist || ', ' || bns_mun like '%" + StringUtilities.HandleApostrophe(m_sBnsAdd) + "%'";
            }
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            pSet.Query += " and trim(bin) is null ";
            m_sQuery = pSet.Query.ToString();   // RMC 20120329 Modifications in Notice of violation
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("tbin").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;
                    }


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

            m_sISNum = "";
            pSet.Query = "select * from btm_temp_businesses where tbin in (select trim(is_number) ";
            pSet.Query += " from norec_closure_tagging) ";
            pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            if (m_sBnsNm != "")
                pSet.Query += " and bns_nm like '%" + StringUtilities.HandleApostrophe(m_sBnsNm) + "%'";
            if (m_sOwnName != "")
                pSet.Query += " and own_code in (select own_code from own_names where own_ln || ' ' || own_fn || ' ' || own_mi like '%" + m_sOwnName + "%')";
            if (m_sBnsAdd != "")
            {
                pSet.Query += " and bns_house_no || ' ' || bns_street || ', ' || bns_brgy ";
                pSet.Query += " || ' ' || bns_zone || ', ' || bns_dist || ', ' || bns_mun like '%" + StringUtilities.HandleApostrophe(m_sBnsAdd) + "%'";
            }
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            //pSet.Query += " order by tbin"; // GDE 20120508
            m_sQuery = pSet.Query.ToString();   // RMC 20120329 Modifications in Notice of violation
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("tbin").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);


                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;
                    }

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

                    if (iNoticeNum == 0)
                    {
                        MessageBox.Show("You cannot issue notice to this business because this business has been\nalready tagged for closure", "Issue Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                        m_bInsertNotice = true;

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

            if (!frmSendNotice.TransactionOk)
            {
                m_bInsertNotice = false;
                return;
            }

            if (m_bInsertNotice)
            {
                pSet.Query = "insert into unoff_with_notice values (:1)";
                pSet.AddParameter(":1", m_sISNum);
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }


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

                        if (AuditTrail.InsertTrail("ABM-U-NI", "unofficial_notice_closure", m_sISNum) == 0)
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

                    if (AuditTrail.InsertTrail("ABM-U-NI", "unofficial_notice_closure", m_sISNum) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            pSet.Close();

           // LoadNotice(m_sISNum);

            // RMC 20120418 added searching by address in Business mappin printing of notice (S)
            if (TaggingFrm.chkWONotice.Checked)
                OnWithoutNotice();
            if (TaggingFrm.chkWNotice.Checked)
                OnWithNotice();
            if (TaggingFrm.chkW2ndNotice.Checked)
                OnWith2ndNotice();
            if (TaggingFrm.chkWClosure.Checked)
                OnWClosureNotice();
            if (TaggingFrm.chkWTag.Checked)
                OnWithTag();
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)

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

                                if (AuditTrail.InsertTrail("ABM-U-NS", "unofficial_notice_closure", m_sISNum) == 0)
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

            //OnWithNotice();
            // RMC 20120418 added searching by address in Business mappin printing of notice (S)
            if (TaggingFrm.chkWONotice.Checked)
                OnWithoutNotice();
            if (TaggingFrm.chkWNotice.Checked)
                OnWithNotice();
            if (TaggingFrm.chkW2ndNotice.Checked)
                OnWith2ndNotice();
            if (TaggingFrm.chkWClosure.Checked)
                OnWClosureNotice();
            if (TaggingFrm.chkWTag.Checked)
                OnWithTag();
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)

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

                if (AuditTrail.InsertTrail("ABM-U-ND", "multiple table", m_sISNum) == 0)
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

                if (AuditTrail.InsertTrail("ABM-U-ND", "unofficial_notice_closure", m_sBin) == 0)
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
            string sOwnCode = "";
            string sBnsNm = "";
            bool bWatch = false;

            frmBussReport PrintNotice = new frmBussReport();

            pSet.Query = string.Format("select * from unofficial_notice_closure where is_number = '{0}' order by notice_number desc", m_sISNum);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    PrintNotice.NoticeDate = pSet.GetDateTime("notice_date");
                    bWatch = true;
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from btm_temp_businesses where tbin = '{0}'", m_sISNum);
            if (pSet.Execute())
            {

                if (pSet.Read())
                {
                    sOwnCode = pSet.GetString("own_code");
                    sBnsNm = pSet.GetString("bns_nm");
                    
                    PrintNotice.OwnCode = sOwnCode;
                    PrintNotice.UnofficialBnsNm = sBnsNm;
                    PrintNotice.BIN = pSet.GetString("tbin");
                    if (!bWatch)// GDE 20120424
                        PrintNotice.NoticeDate = pSet.GetDateTime("save_tm"); 
                    //PrintNotice.ReportSwitch = "Violation";
                    PrintNotice.ReportSwitch = GetNoticeToPrint();  // RMC 20120329 Modifications in Notice of violation

                    // RMC 20120417 Modified Final notice format (s)
                    if (PrintNotice.ReportSwitch == "")
                    {
                        MessageBox.Show("No notice to print.\nTag inspection notice first", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (PrintNotice.ReportSwitch == " ")
                    {
                        return;
                    }
                    // RMC 20120417 Modified Final notice format (e)

                    PrintNotice.ShowDialog();
                    
                    if (AuditTrail.InsertTrail("ABM-U-NG", "multiple table", m_sISNum) == 0)
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
            try
            {
                m_sISNum = "";
                TaggingFrm.txtBnsName.Text = TaggingFrm.dgvBnsInfo[1, iRow].Value.ToString().Trim();
                m_sISNum = TaggingFrm.dgvBnsInfo[0, iRow].Value.ToString().Trim();
                TaggingFrm.txtOwnName.Text = TaggingFrm.dgvBnsInfo[2, iRow].Value.ToString().Trim();    // RMC 20120417 Modified Final notice format
                
            }
            catch { }

            LoadNotice(m_sISNum);
        }

        public override void CheckW2ndNotice()
        {
            if (TaggingFrm.chkW2ndNotice.CheckState.ToString() == "Checked")
            {
                TaggingFrm.chkWONotice.Checked = false;
                TaggingFrm.chkWTag.Checked = false;
                TaggingFrm.chkWNotice.Checked = false;
                TaggingFrm.chkWClosure.Checked = false;

                m_sOwnName = "";
                m_sBnsAdd = "";
                m_sBnsNm = "";
                OnWith2ndNotice();

            }
        }

        public override void OnWith2ndNotice()
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

            //pSet.Query = "select * from btm_temp_businesses where tbin in (select trim(is_number) from unoff_with_notice ) order by tbin";

            // RMC 20120329 Modifications in Notice of violation (s)
            // 2nd notice
            pSet.Query = "select * from btm_temp_businesses where tbin in ";
            pSet.Query += "(select is_number from unofficial_notice_closure having max(notice_number) = 2 group by is_number) ";
            pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            if (m_sBnsNm != "")
                pSet.Query += " and bns_nm like '%" + StringUtilities.HandleApostrophe(m_sBnsNm) + "%'";
            if (m_sOwnName != "")
                pSet.Query += " and own_code in (select own_code from own_names where own_ln || ' ' || own_fn || ' ' || own_mi like '%" + m_sOwnName + "%')";
            if (m_sBnsAdd != "")
            {
                pSet.Query += " and bns_house_no || ' ' || bns_street || ', ' || bns_brgy ";
                pSet.Query += " || ' ' || bns_zone || ', ' || bns_dist || ', ' || bns_mun like '%" + StringUtilities.HandleApostrophe(m_sBnsAdd) + "%'";
            }
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            pSet.Query += "and trim(bin) is null order by tbin";
            m_sQuery = pSet.Query.ToString();
            // RMC 20120329 Modifications in Notice of violation (e)
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("tbin").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    
                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;
                    }


                    iRow++;
                    iCount++;
                }

                if (iRow == 0)
                {
                    //MessageBox.Show("No Unofficial Business with Notice.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20120329 Modifications in Notice of violation
                    m_sISNum = "";
                    //return;
                }
            }
            pSet.Close();

            TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            LoadNotice(m_sISNum);

        }

        public override void OnWClosureNotice()
        {
            // RMC 20120329 Modifications in Notice of violation
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

            
            // closure notice
            pSet.Query = "select * from btm_temp_businesses where tbin in ";
            // RMC 20120417 Modified Final notice format (s)
            if (AppSettingsManager.GetConfigValue("19") == "2")  
                pSet.Query += "(select is_number from unofficial_notice_closure having max(notice_number) = 2 group by is_number) ";
            else  // RMC 20120417 Modified Final notice format (e)
                pSet.Query += "(select is_number from unofficial_notice_closure having max(notice_number) = 3 group by is_number) ";
            pSet.Query += " and bns_brgy = '" + m_sWatcher + "'";   // RMC 20120417 Modified Final notice format
            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            if (m_sBnsNm != "")
                pSet.Query += " and bns_nm like '%" + StringUtilities.HandleApostrophe(m_sBnsNm) + "%'";
            if (m_sOwnName != "")
                pSet.Query += " and own_code in (select own_code from own_names where own_ln || ' ' || own_fn || ' ' || own_mi like '%" + m_sOwnName + "%')";
            if (m_sBnsAdd != "")
            {
                pSet.Query += " and bns_house_no || ' ' || bns_street || ', ' || bns_brgy ";
                pSet.Query += " || ' ' || bns_zone || ', ' || bns_dist || ', ' || bns_mun like '%" + StringUtilities.HandleApostrophe(m_sBnsAdd) + "%'";
            }
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            pSet.Query += "and trim(bin) is null "; // GDE 20120430 temp
            //pSet.Query += "and trim(bin) is null and trim(tbin) not in (select trim(is_number) from norec_closure_tagging) order by tbin"; 
            
            m_sQuery = pSet.Query.ToString();
            
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sIsNum = pSet.GetString("tbin").Trim();
                    sBnsNm = pSet.GetString("Bns_Nm").Trim();

                    sOwnCode = pSet.GetString("own_code").Trim();
                    sOwner = AppSettingsManager.GetBnsOwner(sOwnCode);

                    

                    TaggingFrm.dgvBnsInfo.Rows.Add("");
                    TaggingFrm.dgvBnsInfo[0, iRow].Value = sIsNum;
                    TaggingFrm.dgvBnsInfo[1, iRow].Value = sBnsNm;
                    TaggingFrm.dgvBnsInfo[2, iRow].Value = sOwner;

                    if (iRow == 0)
                    {
                        m_sISNum = sIsNum;
                        TaggingFrm.txtBnsName.Text = sBnsNm;
                        TaggingFrm.txtOwnName.Text = sOwner;
                    }

                    iRow++;
                    iCount++;
                }

                if (iRow == 0)
                {
                    //MessageBox.Show("No Unofficial Business with Notice.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20120329 Modifications in Notice of violation
                    m_sISNum = "";
                    //return;
                }
            }
            pSet.Close();

            TaggingFrm.lblCount.Text = string.Format("{0:###}", iCount);

            LoadNotice(m_sISNum);
        }

        private string GetNoticeToPrint()
        {
            // RMC 20120329 Modifications in Notice of violation

            OracleResultSet pSet = new OracleResultSet();
            string sMaxNoticeNum = "0";
            string sNoticeToPrint = "";
            string sCurrentDate = "";
            string sExpDate = "";
            DateTime odtNoticeRcvd = AppSettingsManager.GetCurrentDate();
            DateTime odtExpDate = AppSettingsManager.GetCurrentDate();

            pSet.Query = "select * from unofficial_notice_closure where is_number = '" + m_sISNum + "' order by notice_number desc ";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sMaxNoticeNum = pSet.GetString("notice_number");
                    odtNoticeRcvd = pSet.GetDateTime("notice_sent");
                }
            }
            pSet.Close();

            if (sMaxNoticeNum == "1")
            {
                // RMC 20120417 Modified Final notice format (s)
                if (AppSettingsManager.GetConfigValue("19") == "3")
                    sNoticeToPrint = "Violation";
                else if (AppSettingsManager.GetConfigValue("19") == "2")
                    sNoticeToPrint = "Closure";

                odtExpDate = odtNoticeRcvd.AddDays(5);

                sExpDate = string.Format("{0:MM/dd/yyyy}", odtExpDate);
                sCurrentDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                if (Convert.ToDateTime(sExpDate) > Convert.ToDateTime(sCurrentDate))
                {
                    if (MessageBox.Show("Inspection notice not yet expired.\nContinue printing Final Notice?", "Sending of Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        sNoticeToPrint = " ";
                    }
                }
                // RMC 20120417 Modified Final notice format (e)
            }
            else if (sMaxNoticeNum == "2")
            {
                sNoticeToPrint = "Closure";
            }
            else
                sNoticeToPrint = "";

            return sNoticeToPrint;

        }

        public override void ButtonPrintList()
        {
            // RMC 20120329 Modifications in Notice of violation
            

            if (m_sQuery == "")
            {
                MessageBox.Show("Select list to print","Notice",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;

            }

            frmPrintOptions fOptions = new frmPrintOptions();
            fOptions.rdoInspector.Enabled = false;
            fOptions.cmbInspector.Visible = false;
            fOptions.rdoBin.Enabled = false;
            fOptions.cmbInspector.Enabled = false;
            fOptions.txtIS.Enabled = false;
            fOptions.bin1.Enabled = false;
            fOptions.btnGenerate.Visible = false;
            fOptions.btnClose.Text = "Ok";
            fOptions.label1.Text = "Notice Date:";
            fOptions.ShowDialog();
            dFrom = fOptions.dtpDateFrom.Value;
            dTo = fOptions.dtpDateTo.Value;


            frmBussReport PrintNotice = new frmBussReport();
            PrintNotice.ReportSwitch = "Notice List";

            
            // RMC 20120417 Modified Final notice format (s)
            if (TaggingFrm.chkWONotice.Checked)
            {
                PrintNotice.OwnCode = "1";
                m_sQuery = m_sQuery + " and save_tm between '" + string.Format("{0:dd-MMM-yy}", dFrom) + "' and '" + string.Format("{0:dd-MMM-yy}", dTo) + "'  order by tbin";
                
            }
            else if (TaggingFrm.chkWNotice.Checked)
            {
                PrintNotice.OwnCode = "2";
                m_sQuery = m_sQuery + " and tbin in (select is_number from unofficial_notice_closure where notice_date between '" + string.Format("{0:dd-MMM-yy}", dFrom) + "' and '" + string.Format("{0:dd-MMM-yy}", dTo) + "')"; // gde 20120508 ADDED
            }
            else if (TaggingFrm.chkW2ndNotice.Checked)
                PrintNotice.OwnCode = "3";
            else if (TaggingFrm.chkWClosure.Checked)
                PrintNotice.OwnCode = "4";
            else
                PrintNotice.OwnCode = "5";
            // RMC 20120417 Modified Final notice format (e)
            PrintNotice.m_dFrom = dFrom; // GDE 20120507
            PrintNotice.m_dTo = dTo; // GDE 20120507
            PrintNotice.Query = m_sQuery;
            PrintNotice.ShowDialog();
        }

        public override void ButtonSearch()
        {
            // RMC 20120417 Modified Final notice format

            if (TaggingFrm.txtBnsName.Text.Trim() != "" || TaggingFrm.txtBnsAdd.Text.Trim() != ""
                || TaggingFrm.txtOwnName.Text.Trim() != "")
            {
                
             /*   int iIndex = -1;
                string sBIN = "";

                for (int iRow = 0; iRow < TaggingFrm.dgvBnsInfo.Rows.Count; iRow++)
                {
                    string sBnsName = "";
                    string sOwnName = "";
                    sBnsName = TaggingFrm.dgvBnsInfo[1, iRow].Value.ToString().Trim();
                    sOwnName = TaggingFrm.dgvBnsInfo[2, iRow].Value.ToString().Trim();

                    if((sBnsName.Contains(TaggingFrm.txtBnsName.Text.Trim()) && TaggingFrm.txtBnsName.Text.Trim() != "")
                        || (sOwnName.Contains(TaggingFrm.txtOwnName.Text.Trim()) && TaggingFrm.txtOwnName.Text.Trim() != ""))
                    {
                        iIndex = iRow;
                        sBIN = TaggingFrm.dgvBnsInfo[0, iRow].Value.ToString().Trim();
                        break;
                    }
                }

                if (iIndex != -1)
                {
                    LoadNotice(sBIN);
                    TaggingFrm.dgvBnsInfo.CurrentCell = TaggingFrm.dgvBnsInfo[0, iIndex];
                }

                if (sBIN == "")
                {
                    MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }*/

                // RMC 20120418 added searching by address in Business mappin printing of notice (s)
                m_sBnsNm = TaggingFrm.txtBnsName.Text.Trim();
                m_sBnsAdd = TaggingFrm.txtBnsAdd.Text.Trim();
                m_sOwnName = TaggingFrm.txtOwnName.Text.Trim();

                if (TaggingFrm.chkWONotice.Checked)
                    OnWithoutNotice();
                if (TaggingFrm.chkWNotice.Checked)
                    OnWithNotice();
                if (TaggingFrm.chkW2ndNotice.Checked)
                    OnWith2ndNotice();
                if (TaggingFrm.chkWClosure.Checked)
                    OnWClosureNotice();
                if (TaggingFrm.chkWTag.Checked)
                    OnWithTag();
                // RMC 20120418 added searching by address in Business mappin printing of notice (e)
            }
            else
            {
                MessageBox.Show("No criteria to search.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public override void ButtonClear()
        {
            // RMC 20120417 Modified Final notice format
            

            // RMC 20120418 added searching by address in Business mappin printing of notice (s)
            m_sBnsAdd = "";
            m_sOwnName = "";
            m_sBnsNm = "";

            if (TaggingFrm.chkWONotice.Checked)
                OnWithoutNotice();
            if (TaggingFrm.chkWNotice.Checked)
                OnWithNotice();
            if (TaggingFrm.chkW2ndNotice.Checked)
                OnWith2ndNotice();
            if (TaggingFrm.chkWClosure.Checked)
                OnWClosureNotice();
            if (TaggingFrm.chkWTag.Checked)
                OnWithTag();

            TaggingFrm.txtBnsName.Text = "";
            TaggingFrm.txtBnsAdd.Text = "";
            TaggingFrm.txtOwnName.Text = "";
            
            // RMC 20120418 added searching by address in Business mappin printing of notice (e)
        }

        public override void CellRowEnter(int iRow)
        {
            // RMC 20120418 added searching by address in Business mappin printing of notice
            try
            {
                if (iRow != -1)
                {
                    m_sISNum = "";
                    TaggingFrm.txtBnsName.Text = TaggingFrm.dgvBnsInfo[1, iRow].Value.ToString().Trim();
                    m_sISNum = TaggingFrm.dgvBnsInfo[0, iRow].Value.ToString().Trim();
                    TaggingFrm.txtOwnName.Text = TaggingFrm.dgvBnsInfo[2, iRow].Value.ToString().Trim();
                }

            }
            catch { }

            LoadNotice(m_sISNum);
        }
    }
}
