using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.ContainerWithShadow;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchUser;
using System.Collections;

namespace Amellar.Common.Reports
{
    public partial class frmEncodersLog : Form
    {
        int m_iReportRadioNumber = 0;

        public frmEncodersLog()
        {
            InitializeComponent();
        }

        private void frmEncodersLog_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
            txtUserCode.Focus();
            rdoList.PerformClick();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpFrom.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpFrom.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime cdtToday = new DateTime();
            cdtToday = AppSettingsManager.GetSystemDate();

            if (dtpTo.Value.Date > cdtToday.Date || dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Invalid date entry, Please check.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpTo.Value = AppSettingsManager.GetSystemDate();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kbtnSearchUser_Click(object sender, EventArgs e)
        {
            frmSearchUser frmSearchUser = new frmSearchUser();
            frmSearchUser.ShowDialog();
            txtUserCode.Text = frmSearchUser.UserCode;
        }

        private void BtnControls(bool blnEnable)
        {
            txtUserCode.Enabled = blnEnable;
            kbtnSearchUser.Enabled = blnEnable;
            dtpFrom.Enabled = blnEnable;
            dtpTo.Enabled = blnEnable;
        }

        private void rdoList_Click(object sender, EventArgs e)
        {
            m_iReportRadioNumber = 0;
            txtUserCode.Text = "ALL";
            BtnControls(true);
        }

        private void rdoSummary_Click(object sender, EventArgs e)
        {
            m_iReportRadioNumber = 1;
            txtUserCode.Text = "ALL";
            BtnControls(false);
            dtpFrom.Enabled = true;
            dtpTo.Enabled = true;
        }

        private void rdoEncodeRec_Click(object sender, EventArgs e)
        {
            m_iReportRadioNumber = 2;
            txtUserCode.Text = "ALL";
            BtnControls(false);
            dtpTo.Enabled = true;
            dtpFrom.Enabled = true;
        }

        private void rdoEncoderRep_Click(object sender, EventArgs e)
        {
            m_iReportRadioNumber = 3;
            txtUserCode.Text = "ALL";
            BtnControls(true);
            txtUserCode.Enabled = false;
            kbtnSearchUser.Enabled = false;
        }

        private void rdoEncoderRec_Click(object sender, EventArgs e)
        {
            m_iReportRadioNumber = 4;
            txtUserCode.Text = "ALL";
            BtnControls(false);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (m_iReportRadioNumber == 0)
                EncodersListing();
            else if (m_iReportRadioNumber == 1)
                EncodersSummary();
            else if (m_iReportRadioNumber == 2)
            {
                // RMC 20150226 added build-up encoder's report (s)
                if (AppSettingsManager.GetSystemType == "A")
                    EncodersEncodedRecordBPS();
                else// RMC 20150226 added build-up encoder's report (e) 
                    EncodersEncodedRecord();
            }
            else if (m_iReportRadioNumber == 3)
                EncoderDeficientBydate();
            else if (m_iReportRadioNumber == 4)
                PrintImageReportMonitoring();
        }

        private void EncodersListing()
        {
            String sQuery, sColHeader, sBContent, sHeader, sUserCode, sLogIn, sLogOut, sOLogin;
            String sLName = "", sFName = "", sMI = "", sTCode = "";
            int iCount = 0;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            string m_sTempUserCode = String.Empty;

            sColHeader = "^1500|^5000|^2000|^2000;CODE|FULL NAME|LOGIN|LOGOUT;";
            sBContent = "<1500|<5000|^2000|^2000;";
            sHeader = "^10500;";
            sHeader += AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^10500;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^10500;Listing of Encoder's Log";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 8; 
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Table = "^10500;Period   from " + dtpFrom.Text + "  to " + dtpTo.Text;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (txtUserCode.Text.Trim() == "ALL")
                m_sTempUserCode = "";
            else
                m_sTempUserCode = txtUserCode.Text.Trim();

            sQuery = "select distinct(to_char(dt_login,'MM/dd/yyyy')) as dt_log from encoders_log where to_date(to_char(dt_login,'MM/dd/yyyy'),'MM/dd/yyyy')>= to_date('" + dtpFrom.Text + "','MM/dd/yyyy') and to_date(to_char(dt_logout,'MM/dd/yyyy'),'MM/dd/yyyy')<=to_date('" + dtpTo.Text + "','MM/dd/yyyy') order by dt_log";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iCount = 0;
                    sUserCode = "";
                    sTCode = "";

                    sOLogin = pSet.GetDateTime("dt_log").ToShortDateString().Trim();
                    axVSPrinter1.FontBold = true;
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Table = "<3000;Date : " + sOLogin;
                    axVSPrinter1.Paragraph = "";

                    sQuery = "select distinct(encoder_code) from encoders_log where encoder_code like '" + m_sTempUserCode + "%' and to_char(dt_login,'MM/dd/yyyy') = to_date('" + sOLogin + "','MM/dd/yyyy')";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            sUserCode = pSet.GetString("encoder_code").Trim();
                            iCount++;

                            pSet2.Query = "select * from sys_users where usr_code = '" + sUserCode + "'";
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sLName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_ln").Trim());
                                    sFName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_fn").Trim());
                                    sMI = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_mi").Trim());
                                }
                            pSet2.Close();

                            pSet2.Query = "select * from encoders_log where encoder_code = '" + sUserCode + "' and to_char(dt_login,'MM/dd/yyyy')= to_date('" + sOLogin + "','MM/dd/yyyy')";
                            if (pSet2.Execute())
                                while (pSet2.Read())
                                {

                                    sLogIn = pSet2.GetDateTime("dt_login").ToShortDateString().Trim();
                                    sLogOut = pSet2.GetDateTime("dt_logout").ToShortDateString().Trim();

                                    if (sTCode == sUserCode)
                                    {
                                        sBContent += "|";
                                        sBContent += "";
                                        sBContent += "";
                                        sBContent += "|";
                                    }
                                    else
                                    {
                                        sTCode = pSet2.GetString("encoder_code").Trim();
                                        sBContent += sUserCode + "|";
                                        sBContent += sLName + ", ";
                                        sBContent += sFName + " ";
                                        sBContent += sMI + "|";
                                    }

                                    sBContent += sLogIn + "|";
                                    sBContent += sLogOut + ";";
                                }

                            pSet2.Close();
                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = sBContent;
                            sBContent = "<1500|<5000|^2000|^2000;";
                        }
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = "<2500;Total Number of Encoder: " + iCount.ToString("#,##0");
                    }
                    pSet1.Close();
                }
            }
            pSet.Close();
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void EncodersSummary()
        {
            String sQuery, sColHeader, sBContent, sHeader, sUserCode, sLogIn = "", sLogOut = "", sOLogIn;
            String sLName = "", sFName = "", sMI = "";
            int iCount = 0;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            sColHeader = "^1500|^5000|^2000|^2000;CODE|FULL NAME|LOGIN|LOGOUT;";
            sBContent = "<1500|<5000|^2000|^2000;";
            sHeader = "^10500;";
            sHeader += AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^10500;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^10500;Summary of Encoder's Log";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Table = "^10500;Period   from " + dtpFrom.Text + "  to " + dtpTo.Text;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            sQuery = "select distinct(to_char(dt_login,'MM/dd/yyyy')) as dt_log from encoders_log where to_date(to_char(dt_login,'MM/dd/yyyy'),'MM/dd/yyyy')>= to_date('" + dtpFrom.Text + "','MM/dd/yyyy') and to_date(to_char(dt_logout,'MM/dd/yyyy'),'MM/dd/yyyy')<=to_date('" + dtpTo.Text + "','MM/dd/yyyy') order by dt_log";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iCount = 0;
                    sOLogIn = pSet.GetDateTime("dt_log").ToShortDateString().Trim();
                    axVSPrinter1.FontBold = true;
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Table = "<3000;Date : " + sOLogIn;
                    axVSPrinter1.Paragraph = "";

                    sQuery = "select distinct(encoder_code) from encoders_log where to_char(dt_login,'MM/dd/yyyy')= to_date('" + sOLogIn + "','MM/dd/yyyy')";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            sUserCode = pSet1.GetString("encoder_code").Trim();
                            iCount++;

                            sQuery = "select dt_login from encoders_log where encoder_code = '" + sUserCode + "' and to_char(dt_login,'MM/dd/yyyy')=to_date('" + sOLogIn + "','MM/dd/yyyy') order by dt_login";
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                    sLogIn = pSet2.GetDateTime("dt_login").ToShortDateString().Trim();
                            pSet2.Close();

                            sQuery = "select dt_logout from encoders_log where encoder_code = '" + sUserCode + "' and to_char(dt_login,'MM/dd/yyyy')=to_date('" + sOLogIn + "','MM/dd/yyyy') order by dt_logout desc";
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                    sLogOut = pSet2.GetDateTime("dt_logout").ToShortDateString().Trim();
                            pSet2.Close();

                            sQuery = string.Format("select * from sys_users where usr_code = '{0}'", sUserCode);
                            pSet2.Query = sQuery;
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sLName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_ln").Trim());
                                    sFName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_fn").Trim());
                                    sMI = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_mi").Trim());
                                }
                            pSet2.Close();

                            sBContent += sUserCode + "|";
                            sBContent += sLName + ", ";
                            sBContent += sFName + " ";
                            sBContent += sMI + "|";
                            sBContent += sLogIn + "|";
                            sBContent += sLogOut + ";";
                        }
                        axVSPrinter1.FontBold = false;
                        axVSPrinter1.Table = sBContent;
                        sBContent = "<1500|<5000|^2000|^2000;";
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = "<2500;Total Number of Encoder: " + iCount.ToString("#,##0");
                    }
                    pSet1.Close();
                }
            }
            else
            {
                MessageBox.Show("No Record(s) found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            pSet.Close();
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void EncodersEncodedRecord()
        {
            String sQuery, sColHeader, sBContent, sHeader, sUserCode, sNumRecord, sOLogin = "";
            String sLName = "", sFName = "", sMI = "";
            int iCount = 0, iNumRecord = 0;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            sColHeader = "^1500|^5000|^4000;Code|Full Name|Number of Encoded Records;";
            sBContent = "<1500|<5000|^4000;";
            sHeader = "^10500;";
            sHeader += AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^10500;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^10500;Encoder's Encoded Payment Records";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Table = "^10500;Period   from " + dtpFrom.Text + "  to " + dtpTo.Text;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (txtUserCode.Text == "ALL")
                txtUserCode.Text = "";

            if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
            {
                pSet.Query = "select distinct(trunc(tdatetime)) as date_save from a_trail where mod_code = 'CSP' and ";
                pSet.Query += "trunc(tdatetime) between to_date('" + dtpFrom.Text + "','MM/dd/yyyy') ";
                pSet.Query += "and to_date('" + dtpTo.Text + "','MM/dd/yyyy') order by tdatetime";
            }
            else
            {
                pSet.Query = "select distinct to_char(date_posted,'MM/dd/yyyy') as date_save from pay_hist ";
                pSet.Query += "where date_posted between to_date('" + dtpFrom.Text + "','MM/dd/yyyy') and to_date('" + dtpTo.Text + "','MM/dd/yyyy') and data_mode = 'POS' order by date_save asc";
            }
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iCount = 0;
                    sUserCode = "";

                    sOLogin = pSet.GetString("date_save");
                    axVSPrinter1.FontBold = true;
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Table = "<3000;Date : " + sOLogin;
                    axVSPrinter1.Paragraph = "";

                    if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
                    {
                        sQuery = "select distinct(usr_code) as user from a_trail where mod_code = 'CSP' and ";
                        sQuery += "usr_code like '" + txtUserCode.Text + "%' and ";
                        sQuery += "trunc(tdatetime) = to_date('" + sOLogin + "','MM/dd/yyyy')";
                    }
                    else // based on records
                        sQuery = "select distinct(bns_user) from pay_hist where bns_user like '" + txtUserCode.Text + "%' and date_posted = to_date('" + sOLogin + "','MM/dd/yyyy')";
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            sUserCode = pSet1.GetString("bns_user").Trim();
                            iCount++;

                            pSet2.Query = "select * from sys_users where usr_code = '" + sUserCode + "'";
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sLName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_ln").Trim());
                                    sFName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_fn").Trim());
                                    sMI = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_mi").Trim());
                                }
                            pSet2.Close();

                            if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
                            {
                                pSet2.Query = "select count(distinct object) as rec_count from a_trail where mod_code = 'CSP' and ";
                                pSet2.Query += "trunc(tdatetime) = to_date('" + sOLogin + "','MM/dd/yyyy') ";
                                pSet2.Query += "and usr_code = '" + sUserCode + "'";
                            }
                            else // based on records
                            pSet2.Query = "select count(*) as rec_count from pay_hist where bns_user = '" + sUserCode + "' and date_posted = to_date('" + sOLogin + "','MM/dd/yyyy')";
                            if (pSet2.Execute())
                            {
                                while (pSet2.Read())
                                {
                                    sNumRecord = pSet2.GetInt("rec_count").ToString().Trim();

                                    sBContent += sUserCode + "|";
                                    sBContent += sLName + ", ";
                                    sBContent += sFName + " ";
                                    sBContent += sMI + "|";
                                    sBContent += sNumRecord + ";";

                                    iNumRecord += Convert.ToInt32(sNumRecord);
                                }
                            }
                            pSet2.Close();

                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = sBContent;
                            sBContent = "<1500|<5000|^4000;";
                        }
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = "<2500;Total Number of Encoder: " + iCount.ToString("#,##0");
                        axVSPrinter1.Table = "<3500;Total Number of Encoded Record: " + iNumRecord.ToString("#,##0");
                    }
                    pSet1.Close();
                }
            }
            else
            {
                MessageBox.Show("No Record(s) found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            pSet.Close();
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void EncoderDeficientBydate()
        {
            try
            {
                String sQuery, sColHeader, sBContent, sHeader, sUserCode, sLogIn, sLogOut, sOLogin;
                String sLName = "", sFName = "", sMI = "", sCnt, sDate, sCntDef = "0";

                ArrayList arr_strEncoder = new ArrayList();
                ArrayList arr_strEncodedCnt = new ArrayList();

                int iCount = 0, iRCount = 0;
                int iDefCount = 0, iRDefCount = 0;
                string m_sTempUserCode = String.Empty;

                OracleResultSet pSet = new OracleResultSet();

                OracleResultSet pSetBlob = new OracleResultSet();
                OracleResultSet pSet1Blob = new OracleResultSet();
                OracleResultSet pSet2Blob = new OracleResultSet();

                pSetBlob.CreateBlobConnection();
                pSet1Blob.CreateBlobConnection();
                pSet2Blob.CreateBlobConnection();

                sColHeader = "^2000|^4000|^2000|^2000;Code|Full Name|No of Records Encoded|No of Deficient Encoded|";
                sBContent = "<2000|<4000|^2000|^2000;";

                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Arial Narrow"; // MCV 20090728 arial
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontSize = 10; // MCV 20090728 10
                axVSPrinter1.Table = "^11000;Republic of the Philippines";
                sHeader = AppSettingsManager.GetConfigValue("09");
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = "^11000;" + sHeader;
                axVSPrinter1.FontBold = false;
                if (AppSettingsManager.GetConfigValue("08").Trim() != String.Empty)
                {
                    sHeader = "Province of ";
                    sHeader += AppSettingsManager.GetConfigValue("08").Trim();
                    axVSPrinter1.Table = "^11000;" + sHeader;
                }

                axVSPrinter1.Paragraph = "";
                axVSPrinter1.Table = "^11000;Encoder's Report By Date";
                // RMC 20150226 added build-up encoder's report (s)
                if(AppSettingsManager.GetSystemType == "A")
                    axVSPrinter1.Table = "^11000;Business Records";
                else
                    axVSPrinter1.Table = "^11000;Payment Records";
                // RMC 20150226 added build-up encoder's report (e)

                axVSPrinter1.FontBold = false;
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Table = "^11000;Period   from " + dtpFrom.Text + "  to " + dtpTo.Text;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = sColHeader;
                axVSPrinter1.FontBold = false;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

                if (txtUserCode.Text.Trim() == "ALL" || txtUserCode.Text.Trim() == "")
                    m_sTempUserCode = "%";
                else
                    m_sTempUserCode = txtUserCode.Text.Trim() + "%";

                /*sQuery = "select distinct to_char(upload_dt, 'MM/dd/yyyy') as encode_dt from docblob_tbl ";
                sQuery += "where upload_dt >= to_date('" + dtpFrom.Text + "', 'MM/dd/yyyy') and ";
                sQuery += "to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') <= to_date('" + dtpTo.Text + "', 'MM/dd/yyyy') ";
                sQuery += "and bin is not null and sys_type = 'C' order by 1 ";*/

                // RMC 20150226 added build-up encoder's report (s)
                sQuery = "select distinct to_char(upload_dt, 'MM/dd/yyyy') as encode_dt from docblob_tbl ";
                sQuery += "where upload_dt >= to_date('" + dtpFrom.Text + "', 'MM/dd/yyyy') and ";
                sQuery += "to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') <= to_date('" + dtpTo.Text + "', 'MM/dd/yyyy') ";
                sQuery += "and bin is not null and sys_type = '" + AppSettingsManager.GetSystemType + "' ";
                sQuery += "UNION ";
                sQuery += "select distinct to_char(upload_dt, 'MM/dd/yyyy') as encode_dt from docblob_twopage ";
                sQuery += "where upload_dt >= to_date('" + dtpFrom.Text + "', 'MM/dd/yyyy') and ";
                sQuery += "to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') <= to_date('" + dtpTo.Text + "', 'MM/dd/yyyy') ";
                sQuery += "and bin is not null and sys_type = '" + AppSettingsManager.GetSystemType + "' ";
                sQuery += "order by 1";
                // RMC 20150226 added build-up encoder's report (e)
                
                 
                //pRec = cLib.QueryBlobResult(sQuery);
                pSetBlob.Query = sQuery;

                if (pSetBlob.Execute())
                    while (pSetBlob.Read())
                    {
                        iRDefCount = 0; // MCV 20090728 added
                        iRCount = 0;

                        sDate = pSetBlob.GetString("encode_dt").Trim();

                        axVSPrinter1.FontBold = true;
                        axVSPrinter1.Table = "<5000;" + sDate;
                        axVSPrinter1.FontBold = false;

                        for (int iCntPos = 1; iCntPos <= 2; iCntPos++)
                        {
                            arr_strEncoder.Clear();
                            arr_strEncodedCnt.Clear();
                            /*sQuery = "select usr, count(*) as cnt from ";
                            sQuery += "(select upload_by as usr, count(distinct bin||doc_code) as cnt from docblob_tbl ";
                            sQuery += "group by upload_by, to_char(upload_dt, 'MM/dd/yyyy'), bin, doc_code, sys_type "; // MCV 20090826 added sys_type
                            sQuery += "having to_char(upload_dt, 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') and bin is not null and sys_type = 'C' ) "; // MCV 20090826 added systype
                            sQuery += "group by usr order by 1 ";*/
                            //pRec = cLib.QueryBlobResult(sQuery);

                            // RMC 20150226 added build-up encoder's report (s)
                            sQuery = "select usr, count(*) as cnt from ";
                            sQuery += "(select upload_by as usr, count(distinct bin||doc_code) as cnt from docblob_tbl ";
                            sQuery += "group by upload_by, to_char(upload_dt, 'MM/dd/yyyy'), bin, doc_code, sys_type "; // MCV 20090826 added sys_type
                            //sQuery += "having to_char(upload_dt, 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";
                            sQuery += "having to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";    // RMC 20161122 corrected error in encoder's report
                            sQuery += "and bin is not null and sys_type = '"+AppSettingsManager.GetSystemType+"' ) ";
                            sQuery += "group by usr ";  // RMC 20161122 corrected error in encoder's report
                            sQuery += "UNION ";
                            sQuery += "select usr, count(*) as cnt from ";
                            sQuery += "(select upload_by as usr, count(distinct bin||doc_code) as cnt from docblob_twopage ";
                            sQuery += "group by upload_by, to_char(upload_dt, 'MM/dd/yyyy'), bin, doc_code, sys_type "; // MCV 20090826 added sys_type
                            //sQuery += "having to_char(upload_dt, 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";
                            sQuery += "having to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";   // RMC 20161122 corrected error in encoder's report
                            sQuery += "and bin is not null and sys_type = '" + AppSettingsManager.GetSystemType + "' ) "; 
                            sQuery += "group by usr order by 1 ";
                            // RMC 20150226 added build-up encoder's report (e)
                            
                            pSet1Blob.Query = sQuery;
                            if (pSet1Blob.Execute())
                                while (pSet1Blob.Read())
                                {
                                    String sTmp = pSet1Blob.GetString("usr").Trim();
                                    sQuery = "select * from sys_users where usr_code = '" + sTmp + "' ";
                                    if (iCntPos == 1)
                                        sQuery += "and (usr_pos = 'DE' or usr_pos like 'DATA ENCODER%') ";
                                    else
                                        sQuery += "and (usr_pos = 'DC' or usr_pos like 'DATA CONTROLLER%') ";
                                    pSet.Query = sQuery;
                                    if (pSet.Execute())
                                        if (pSet.Read())
                                        {
                                            arr_strEncoder.Add(sTmp);
                                            arr_strEncodedCnt.Add(pSet1Blob.GetInt("cnt").ToString());
                                        }
                                    pSet.Close();
                                }
                            pSet1Blob.Close();

                            sQuery = "";
                            for (int i = 0; i < arr_strEncoder.Count; i++)
                            {
                                sQuery += "select '" + arr_strEncoder[i].ToString() + "' as encoder, '" + arr_strEncodedCnt[i].ToString() + "' as cnt from dual ";
                                if (arr_strEncoder.Count - 1 != i)
                                    sQuery += "union all ";
                            }
                            if (arr_strEncoder.Count == 0)
                                break;

                            pSet1Blob.Query = sQuery;
                            if (pSet1Blob.Execute())
                            //if (pSet1Blob.Read()) // RMC 20161130 corrected errors in encoder's report,put rem
                                {
                                    axVSPrinter1.FontBold = true;
                                    axVSPrinter1.FontSize = 10;
                                    switch (iCntPos)
                                    {
                                        case 1:
                                            axVSPrinter1.Table = "<5000;DATA ENCODERS";
                                            break;
                                        case 2:
                                            axVSPrinter1.Table = "<5000;DATA CONTROLLERS";
                                            break;
                                    }
                                    axVSPrinter1.FontBold = false;

                                    while (pSet1Blob.Read())
                                    {
                                        sUserCode = pSet1Blob.GetString("encoder").Trim();

                                        //sCnt = pSet1Blob.GetInt("cnt").ToString().Trim();
                                        sCnt = pSet1Blob.GetString("cnt").Trim();  // RMC 20161122 corrected error in encoder's report
                                        iCount += Convert.ToInt32(sCnt);
                                        iRCount += Convert.ToInt32(sCnt);

                                        pSet.Query = "select * from sys_users where usr_code = '" + sUserCode + "'";
                                        if (pSet.Execute())
                                            if (pSet.Read())
                                            {
                                                sLName = StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString("usr_ln").Trim());
                                                sFName = StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString("usr_fn").Trim());
                                                sMI = StringUtilities.StringUtilities.RemoveApostrophe(pSet.GetString("usr_mi").Trim());
                                            }
                                        pSet.Close();

                                        arr_strEncoder.Clear();
                                        
                                        sQuery = "select bin, doc_code from docblob_tbl ";
                                        sQuery += "group by sys_type, bin, doc_code, upload_by, to_char(upload_dt, 'MM/dd/yyyy') ";
                                        sQuery += "having ";
                                        sQuery += "upload_by = '" + sUserCode + "' ";
                                        //sQuery += "and sys_type = 'C' "; // MCV 20090826 added sys_type
                                        sQuery += "and sys_type = '" + AppSettingsManager.GetSystemType + "' ";// RMC 20150226 added build-up encoder's report 
                                        //sQuery += "and to_char(upload_dt, 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";
                                        sQuery += "and to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";   // RMC 20161122 corrected error in encoder's report
                                        // RMC 20150226 added build-up encoder's report (S)
                                        sQuery += "UNION ";
                                        sQuery += "select bin, doc_code from docblob_tbl ";
                                        sQuery += "group by sys_type, bin, doc_code, upload_by, to_char(upload_dt, 'MM/dd/yyyy') ";
                                        sQuery += "having ";
                                        sQuery += "upload_by = '" + sUserCode + "' ";
                                        sQuery += "and sys_type = '" + AppSettingsManager.GetSystemType + "' ";
                                        //sQuery += "and to_char(upload_dt, 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";
                                        sQuery += "and to_date(to_char(upload_dt, 'MM/dd/yyyy'), 'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy') ";   // RMC 20161122 corrected error in encoder's report
                                        // RMC 20150226 added build-up encoder's report (E)

                                        pSet2Blob.Query = sQuery;
                                        if (pSet2Blob.Execute())
                                            while (pSet2Blob.Read())
                                            {
                                                String sTmp, sTmp1;
                                                sTmp = pSet2Blob.GetString("bin").Trim();
                                                sTmp1 = pSet2Blob.GetString("doc_code").Trim();
                                                arr_strEncoder.Add("'" + sTmp + "' as bin, '" + sTmp1 + "' as doc_code ");
                                            }
                                        pSet2Blob.Close();

                                        sQuery = "";
                                        sQuery = "with xxx as (";
                                        for (int i = 0; i < arr_strEncoder.Count; i++)
                                        {
                                            sQuery += "select " + arr_strEncoder[i].ToString() + " from dual ";
                                            if (arr_strEncoder.Count - 1 != i)
                                                sQuery += "union all ";
                                        }
                                        sQuery += ") ";
                                        sQuery += "select count(distinct (bin||doc_code)) as sCntDef from xxx where bin in (select rec_acct_no from def_records ";
                                        //sQuery += "where to_char(to_date(dt_save, 'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy')) ";
                                        sQuery += "where to_date(to_char(dt_save, 'MM/dd/yyyy'),'MM/dd/yyyy') = to_date('" + sDate + "','MM/dd/yyyy')) ";// RMC 20161122 corrected error in encoder's report

                                        /*pSet2Blob.Query = sQuery;
                                        if (pSet2Blob.Execute())
                                            if (pSet2Blob.Read())
                                            {
                                                sCntDef = pSet2Blob.GetInt("sCntDef").ToString();
                                            }
                                            else
                                                sCntDef = "0";
                                        pSet2Blob.Close();*/
                                        // RMC 20161122 corrected error in encoder's report (s)
                                        pSet.Query = sQuery;
                                        if (pSet.Execute())
                                            if (pSet.Read())
                                            {
                                                sCntDef = pSet.GetInt("sCntDef").ToString();
                                            }
                                            else
                                                sCntDef = "0";
                                        pSet.Close();
                                        // RMC 20161122 corrected error in encoder's report (e)

                                        iDefCount += Convert.ToInt32(sCntDef);
                                        iRDefCount += Convert.ToInt32(sCntDef);

                                        sBContent = "<2000|<4000|^2000|^2000;";
                                        sBContent += sUserCode + "|";
                                        sBContent += sLName + ", ";
                                        sBContent += sFName + " ";
                                        sBContent += sMI + "|";
                                        sBContent += sCnt + "|";
                                        sBContent += sCntDef + ";";

                                        axVSPrinter1.Table = sBContent;
                                    }
                                }
                            pSet1Blob.Close();
                        }

                        if (m_sTempUserCode == "%")
                        {
                            axVSPrinter1.Paragraph = "";
                            axVSPrinter1.FontBold = true;
                            axVSPrinter1.Table = "<6000;Total Number of Records Encoded: " + iRCount.ToString("#,##0");
                            axVSPrinter1.Table = "<6000;Total Number of Deficient Records Encoded: " + iRDefCount.ToString("#,##0");
                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Paragraph = "";
                        }
                    }
                pSetBlob.Close();

                axVSPrinter1.Paragraph = "";
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = "<7000;Overall Total Number of Records Encoded: " + iCount.ToString("#,##0");
                axVSPrinter1.Table = "<7000;Overall Total Number of Deficient Records Encoded: " + iDefCount.ToString("#,##0");
                axVSPrinter1.FontBold = false;

                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            }
            catch { }
        }

        private void PrintImageReportMonitoring()
        {
            try
            {
                String sQuery, sHeader, sBrgyName, sBrgyCode, sAsOf = "";
                String sTmpFormat, sTmpString;
                String sGTotCnt = "0", sGTotCanc = "0", sGTotPend = "0", sGTotRej = "0",
                    sGTotCPR = "0", sGTotPercCPR = "0", sGTotPercFin,
                    sGTotLand = "0", sGTotEncoded = "0", sGTotPercEncoded;
                int iCount = 0;

                OracleResultSet pSet = new OracleResultSet();

                OracleResultSet pSetBlob = new OracleResultSet();
                OracleResultSet pSet1Blob = new OracleResultSet();
                
                pSetBlob.CreateBlobConnection();
                pSet1Blob.CreateBlobConnection();

                pSet.Query = "select 'as of '||to_char(sysdate, 'MM/dd/yyyy') as as_of from dual";
                if (pSet.Execute())
                    if (pSet.Read())
                        sAsOf = pSet.GetString("as_of").ToString();
                pSet.Close();

                axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
                axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
                axVSPrinter1.FontName = "Arial Narrow";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.FontSize = 10;
                axVSPrinter1.Table = "^11000;Republic of the Philippines";
                sHeader = AppSettingsManager.GetConfigValue("09");
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Table = "^11000;" + sHeader;
                axVSPrinter1.FontBold = false;
                if (AppSettingsManager.GetConfigValue("08") != String.Empty)
                {
                    sHeader = "Province of ";
                    sHeader += AppSettingsManager.GetConfigValue("08");
                    axVSPrinter1.Table = "^11000;" + sHeader;
                }
                axVSPrinter1.Paragraph = "";
                //axVSPrinter1.TextColor = (RGB(0,0,255));

                if (AppSettingsManager.GetSystemType == "A")    // RMC 20150226 added build-up encoder's report
                    axVSPrinter1.Table = "^11000;Image Tagging Report Monitoring - Business Records";
                else
                    axVSPrinter1.Table = "^11000;Image Tagging Report Monitoring - Payment Records";    // RMC 20150226 added build-up encoder's report
                //axVSPrinter1.TextColor(RGB(0,0,0));
                axVSPrinter1.FontSize = 8;
                axVSPrinter1.FontName = "Arial";
                axVSPrinter1.Table = "^11000;" + sAsOf;
                axVSPrinter1.Paragraph = "";
                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColumns;
                axVSPrinter1.FontBold = true;
                axVSPrinter1.Paragraph = "";

                sTmpFormat = "~^1900|~^1000|~^1000|~^900|~^1000|~^900|~^900|~^900|~^900|~^1000;"; //~^2500
                sTmpString = "Barangay|Exported|Encoded|%|Cancelled|Pending|Rejected|Total|%|%";
                axVSPrinter1.Table = sTmpFormat + sTmpString;

                sTmpFormat = "~^1900|~^1000|~^1000|~^900|~^1000|~^900|~^900|~^900|~^900|~^1000;";
                sTmpString = "|Records|Records|||||||Finished";
                axVSPrinter1.Table = sTmpFormat + sTmpString;

                axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
                axVSPrinter1.Table = ";;;";

                axVSPrinter1.FontBold = false;

                String sTmpBrgyName, sTotalCnt = "0", sTotLand = "0", sTotBldg, sTotMach, sTotImpr, sTotTree;
                String sTotCanc = "0", sTotPending = "0", sTotRejected = "0", sTotEncoded;
                String sTotCPR = "0", sTotPercCPR = "0", sTotPercMOU, sTotPercFin = "0", sTotPercEncoded = "0";

                //pSet.Query = "select * from brgy order by dist_code, brgy_code order by brgy_code asc";
                pSet.Query = "select * from brgy order by dist_code, brgy_code";    // RMC 20150226 added build-up encoder's report
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        iCount++;

                        sBrgyCode = pSet.GetString("brgy_code").Trim();
                        sBrgyName = pSet.GetString("brgy_nm").Trim();

                        /*if (Convert.ToInt16(AppSettingsManager.GetConfigValue("40")) > 1)
                        {
                            sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added systype
                            sQuery += "from dual ";
                        }
                        else
                        {
                            sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "') as tot_cnt, ";
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and bin is not null) as tot_land, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added systype
                            sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added systype
                            sQuery += "from dual ";
                        }*/
                        //pRec = cLib.QueryBlobResult(sQuery);
                        // RMC 20150226 added build-up encoder's report (s)
                        sQuery = "select (select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "'  and sect_name = '" + sBrgyName + "') as tot_cnt, ";
                        sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "'  and sect_name = '" + sBrgyName + "' and bin is not null) as tot_land, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "'  and sect_name = '" + sBrgyName + "' and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "'  and sect_name = '" + sBrgyName + "' and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added systype
                        sQuery += "from dual ";
                        sQuery += "UNION ";
                        sQuery += "select (select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added systype
                        sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added systype
                        sQuery += "from dual ";
                        // RMC 20150226 added build-up encoder's report (e)

                        pSetBlob.Query = sQuery;
                        if (pSetBlob.Execute())
                            if (pSetBlob.Read())
                            {
                                sTotalCnt = pSetBlob.GetInt("tot_cnt").ToString().Trim();
                                sTotLand = pSetBlob.GetInt("tot_land").ToString().Trim();
                                sTotCanc = pSetBlob.GetInt("tot_cancelled").ToString().Trim();
                                sTotPending = pSetBlob.GetInt("tot_pending").ToString().Trim();
                                sTotRejected = pSetBlob.GetInt("tot_rejected").ToString().Trim();

                                sTotEncoded = sTotLand;

                                try
                                {
                                    sTotPercEncoded = Convert.ToString((Convert.ToInt32(sTotEncoded) / Convert.ToInt32(sTotalCnt)) * 100);
                                }
                                catch { sTotPercEncoded = "0"; }

                                sTotCPR = Convert.ToString(Convert.ToInt32(sTotCanc) + Convert.ToInt32(sTotPending) + Convert.ToInt32(sTotRejected));

                                try
                                {
                                    sTotPercCPR = Convert.ToString((Convert.ToInt32(sTotCPR) / Convert.ToInt32(sTotalCnt)) * 100);
                                }
                                catch { sTotPercCPR = "0"; }

                                try
                                {
                                    sTotPercFin = Convert.ToString(((Convert.ToInt32(sTotCanc) + Convert.ToInt32(sTotRejected) + Convert.ToInt32(sTotEncoded)) / Convert.ToInt32(sTotalCnt)) * 100);	//Note: Perc finished does not include pending, pendings should be clarified and encoded
                                }
                                catch { sTotPercFin = "0"; }

                                // 4 grand totals
                                sGTotCnt = Convert.ToString(Convert.ToInt32(sGTotCnt) + Convert.ToInt32(sTotalCnt));
                                sGTotLand = Convert.ToString(Convert.ToInt32(sGTotLand) + Convert.ToInt32(sTotLand));
                                sGTotEncoded = Convert.ToString(Convert.ToInt32(sGTotEncoded) + Convert.ToInt32(sTotEncoded));
                                sGTotCanc = Convert.ToString(Convert.ToInt32(sGTotCanc) + Convert.ToInt32(sTotCanc));
                                sGTotPend = Convert.ToString(Convert.ToInt32(sGTotPend) + Convert.ToInt32(sTotPending));
                                sGTotRej = Convert.ToString(Convert.ToInt32(sGTotRej) + Convert.ToInt32(sTotRejected));
                                sGTotCPR = Convert.ToString(Convert.ToInt32(sGTotCPR) + Convert.ToInt32(sTotCPR));
                                sGTotPercCPR = Convert.ToString(Convert.ToInt32(sGTotPercCPR) + Convert.ToInt32(sTotPercCPR));
                            }
                        pSetBlob.Close();

                        sTmpFormat = "~<1900|~>1000|~>1000|~>900|~>1000|~>900|~>900|~>900|~>900|~>1000;";
                        sTmpString = sBrgyName + " (" + sBrgyCode + ")|" + Convert.ToDouble(sTotalCnt).ToString("#,##0") + "|" + Convert.ToDouble(sTotLand).ToString("#,##0");
                        sTmpString += "|" + Convert.ToDouble(sTotPercEncoded).ToString("#,##0") + "|" + Convert.ToDouble(sTotCanc).ToString("#,##0") + "|" + Convert.ToDouble(sTotPending).ToString("#,##0") + "|" + Convert.ToDouble(sTotRejected).ToString("#,##0") + "|" + Convert.ToDouble(sTotCPR).ToString("#,##0") + "|" + Convert.ToDouble(sTotPercCPR).ToString("#,##0") + "|" + Convert.ToDouble(sTotPercFin).ToString("#,##0");
                        axVSPrinter1.Table = sTmpFormat + sTmpString;
                    }
                }
                pSet.Close();

                // RMC 20110706 Added in Report Encoders Log scanned records where temp brgy not in brgy table (s)
                int iTmp = 0;
                pSetBlob.Query = "select distinct (sect_name) from docblob_tbl where sys_type = 'C' order by sect_name";
                if (pSetBlob.Execute())
                    while (pSetBlob.Read())
                    {
                        sBrgyName = pSetBlob.GetString(0).Trim();
                        sBrgyCode = "";

                        pSet.Query = "select * from brgy where trim(brgy_nm) = '" + sBrgyName + "'";
                        if (pSet.Execute())
                            if (pSet.Read())
                            {
                                iTmp++;

                                if (iTmp == 1)
                                {
                                    axVSPrinter1.Paragraph = "";
                                    axVSPrinter1.Paragraph = "";
                                    sTmpFormat = "~<5500;";
                                    sTmpString = "Group(s) not in Barangay Table";
                                    axVSPrinter1.Table = sTmpFormat + sTmpString;

                                }

                                if (sBrgyName == String.Empty)
                                {
                                    /*if (Convert.ToInt16(AppSettingsManager.GetConfigValue("40")) > 1)
                                    {
                                        sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                        sQuery += "from dual ";
                                    }
                                    else
                                    {
                                        sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null) as tot_cnt, "; // MCV 20090826 added systype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and trim(sect_name) is null and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                        sQuery += "from dual ";
                                    }*/

                                    // RMC 20150226 added build-up encoder's report (s)
                                    sQuery = "select (select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null) as tot_cnt, "; // MCV 20090826 added systype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                    sQuery += "from dual ";
                                    sQuery += "UNION ";
                                    sQuery += "select (select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and trim(sect_name) is null and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                    sQuery += "from dual ";

                                    // RMC 20150226 added build-up encoder's report (e)

                                    sBrgyName = "UNCATEGORIZED";
                                }
                                else
                                {
                                    /*if (Convert.ToInt16(AppSettingsManager.GetConfigValue("40")) > 1)
                                    {
                                        sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                        sQuery += "from dual ";
                                    }
                                    else
                                    {
                                        sQuery = "select (select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "') as tot_cnt, "; // MCV 20090826 added systype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                        sQuery += "(select count(*) from docblob_tbl where sys_type = 'C' and sect_name = '" + sBrgyName + "' and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                        sQuery += "from dual ";
                                    }*/

                                    // RMC 20150226 added build-up encoder's report (s)
                                    sQuery = "select (select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "') as tot_cnt, "; // MCV 20090826 added systype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_tbl where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                    sQuery += "from dual ";
                                    sQuery += "UNION ";
                                    sQuery += "select (select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1) as tot_cnt, "; // MCV 20090826 added systype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and bin is not null) as tot_land, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and iscancelled = 1) as tot_cancelled, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and ispending = 1) as tot_pending, "; // MCV 20090826 added sytype
                                    sQuery += "(select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "' and sect_name = '" + sBrgyName + "' and mod(id, 2) = 1 and deficient is null and isrejected = 1) as tot_rejected "; // MCV 20090826 added sytype
                                    sQuery += "from dual ";

                                    // RMC 20150226 added build-up encoder's report (e)
                                }

                                pSet1Blob.Query = sQuery;
                                if (pSet1Blob.Execute())
                                    if (pSet1Blob.Read())
                                    {
                                        sTotalCnt = pSet1Blob.GetInt("tot_cnt").ToString().Trim();
                                        sTotLand = pSet1Blob.GetInt("tot_land").ToString().Trim();
                                        sTotCanc = pSet1Blob.GetInt("tot_cancelled").ToString().Trim();
                                        sTotPending = pSet1Blob.GetInt("tot_pending").ToString().Trim();
                                        sTotRejected = pSet1Blob.GetInt("tot_rejected").ToString().Trim();

                                        sTotEncoded = sTotLand;

                                        try
                                        {
                                            sTotPercEncoded = Convert.ToString((Convert.ToInt32(sTotEncoded) / Convert.ToInt32(sTotalCnt)) * 100);
                                        }
                                        catch { sTotPercEncoded = "0"; }

                                        sTotCPR = Convert.ToString(Convert.ToInt32(sTotCanc) + Convert.ToInt32(sTotPending) + Convert.ToInt32(sTotRejected));

                                        try
                                        {
                                            sTotPercCPR = Convert.ToString((Convert.ToInt32(sTotCPR) / Convert.ToInt32(sTotalCnt)) * 100);
                                        }
                                        catch { sTotPercCPR = "0"; }

                                        try
                                        {
                                            sTotPercFin = Convert.ToString(((Convert.ToInt32(sTotCanc) + Convert.ToInt32(sTotRejected) + Convert.ToInt32(sTotEncoded)) / Convert.ToInt32(sTotalCnt)) * 100);	//Note: Perc finished does not include pending, pendings should be clarified and encoded
                                        }
                                        catch { sTotPercFin = "0"; }

                                        // 4 grand totals
                                        sGTotCnt = Convert.ToString(Convert.ToInt32(sGTotCnt) + Convert.ToInt32(sTotalCnt));
                                        sGTotLand = Convert.ToString(Convert.ToInt32(sGTotLand) + Convert.ToInt32(sTotLand));
                                        sGTotEncoded = Convert.ToString(Convert.ToInt32(sGTotEncoded) + Convert.ToInt32(sTotEncoded));
                                        sGTotCanc = Convert.ToString(Convert.ToInt32(sGTotCanc) + Convert.ToInt32(sTotCanc));
                                        sGTotPend = Convert.ToString(Convert.ToInt32(sGTotPend) + Convert.ToInt32(sTotPending));
                                        sGTotRej = Convert.ToString(Convert.ToInt32(sGTotRej) + Convert.ToInt32(sTotRejected));
                                        sGTotCPR = Convert.ToString(Convert.ToInt32(sGTotCPR) + Convert.ToInt32(sTotCPR));
                                        sGTotPercCPR = Convert.ToString(Convert.ToInt32(sGTotPercCPR) + Convert.ToInt32(sTotPercCPR));
                                    }
                                pSet1Blob.Close();

                                sTmpFormat = "~<1900|~>1000|~>1000|~>900|~>1000|~>900|~>900|~>900|~>900|~>1000;";
                                sTmpString = sBrgyName + " (" + sBrgyCode + ")|" + Convert.ToDouble(sTotalCnt).ToString("#,##0") + "|" + Convert.ToDouble(sTotLand).ToString("#,##0");
                                sTmpString += "|" + Convert.ToDouble(sTotPercEncoded).ToString("#,##0") + "|" + Convert.ToDouble(sTotCanc).ToString("#,##0") + "|" + Convert.ToDouble(sTotPending).ToString("#,##0") + "|" + Convert.ToDouble(sTotRejected).ToString("#,##0") + "|" + Convert.ToDouble(sTotCPR).ToString("#,##0") + "|" + Convert.ToDouble(sTotPercCPR).ToString("#,##0") + "|" + Convert.ToDouble(sTotPercFin).ToString("#,##0");
                                axVSPrinter1.Table = sTmpFormat + sTmpString;
                            }
                        pSet.Close();
                    }
                pSetBlob.Close();

                // for validation 
                String sValidTotal = "0";
                //pSetBlob.Query = "select count(*) from docblob_tbl where sys_type = 'C'";
                // RMC 20150226 added build-up encoder's report (s)
                pSetBlob.Query = "select count(*) from docblob_tbl where sys_type = '"+AppSettingsManager.GetSystemType+"'";
                pSetBlob.Query += "UNION ";
                pSetBlob.Query += "select count(*) from docblob_twopage where sys_type = '" + AppSettingsManager.GetSystemType + "'";
                // RMC 20150226 added build-up encoder's report (e)
                if (pSetBlob.Execute())
                    if (pSetBlob.Read())
                    {
                        sValidTotal = pSetBlob.GetInt(0).ToString();
                    }
                pSetBlob.Close();

                axVSPrinter1.Table = ";;";

                try
                {
                    sGTotPercEncoded = Convert.ToString((Convert.ToInt32(sGTotEncoded) / Convert.ToInt32(sGTotCnt)) * 100);
                }
                catch { sGTotPercEncoded = "0"; }

                try
                {
                    sGTotPercCPR = Convert.ToString((Convert.ToInt32(sGTotCPR) / Convert.ToInt32(sGTotCnt)) * 100);
                }
                catch { sGTotPercCPR = "0"; }

                try
                {
                    sGTotPercFin = Convert.ToString(((Convert.ToInt32(sGTotEncoded) + Convert.ToInt32(sGTotCanc) + Convert.ToInt32(sGTotRej)) / Convert.ToInt32(sGTotCnt)) * 100); // MCV 20090826 5 decimal
                }
                catch { sGTotPercFin = "0"; }

                sTmpFormat = "~<1900|~>1000|~>1000|~>900|~>1000|~>900|~>900|~>900|~>900|~>1000;";
                sTmpString = "|" + Convert.ToDouble(sGTotCnt).ToString("#,##0") + "|" + Convert.ToDouble(sGTotLand).ToString("#,##0");
                sTmpString += "|" + Convert.ToDouble(sGTotPercEncoded).ToString("#,##0") + "|" + Convert.ToDouble(sGTotCanc).ToString("#,##0") + "|" + Convert.ToDouble(sGTotPend).ToString("#,##0") + "|" + Convert.ToDouble(sGTotRej).ToString("#,##0");
                sTmpString += "|" + Convert.ToDouble(sGTotCPR).ToString("#,##0") + "|" + Convert.ToDouble(sGTotPercCPR).ToString("#,##0") + "|" + Convert.ToDouble(sGTotPercFin).ToString("#,##0");
                axVSPrinter1.Table = sTmpFormat + sTmpString;

                axVSPrinter1.Paragraph = "";

                sTmpFormat = "~<1900|~>1000|~>1000|~>900|~>1000|~>900|~>900|~>900|~>900|~>1000;";
                sTmpString = "Total Scanned Records:|" + Convert.ToDouble(sValidTotal).ToString("#,##0");
                axVSPrinter1.Table = sTmpFormat + sTmpString;

                axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
            }
            catch { }
        }

        private void ToolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1,1,axVSPrinter1.PageCount);
        }

        private void toolSettingPageSetup_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrintPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }

        private void EncodersEncodedRecordBPS()
        {
            // RMC 20150226 added build-up encoder's report 
            String sQuery, sColHeader, sBContent, sHeader, sUserCode, sNumRecord, sOLogin = "";
            String sLName = "", sFName = "", sMI = "";
            int iCount = 0, iNumRecord = 0;
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            sColHeader = "^1500|^5000|^4000;Code|Full Name|Number of Encoded Records;";
            sBContent = "<1500|<5000|^4000;";
            sHeader = "^10500;";
            sHeader += AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^10500;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^10500;Encoder's Encoded Business Records";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.Table = "^10500;Period   from " + dtpFrom.Text + "  to " + dtpTo.Text;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (txtUserCode.Text == "ALL")
                txtUserCode.Text = "";

            if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
            {
                pSet.Query = "select distinct(trunc(tdatetime)) as date_save from a_trail where mod_code = 'ABA' and ";
                pSet.Query += "trunc(tdatetime) between to_date('"+dtpFrom.Text+"','MM/dd/yyyy') ";
                //pSet.Query += "and to_date('" + dtpTo.Text + "','MM/dd/yyyy') order by tdatetime";
                pSet.Query += "and to_date('" + dtpTo.Text + "','MM/dd/yyyy') order by date_save";  // RMC 20161122 corrected error in encoder's report
            }
            else // based on records
            {
                pSet.Query = "select distinct(trim(save_tm)) as date_save from businesses where ";
                pSet.Query += "TO_DATE(save_tm) between to_date('" + dtpFrom.Text + "','MM/dd/YYYY') ";
                pSet.Query += "and to_date('" + dtpTo.Text + "','MM/dd/YYYY') order by date_save";
            }
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    iCount = 0;
                    sUserCode = "";

                    DateTime dtDate;
                    dtDate = pSet.GetDateTime("date_save");
                    sOLogin = string.Format("{0:MM/dd/yyyy}",dtDate);

                    axVSPrinter1.FontBold = true;
                    axVSPrinter1.Paragraph = "";
                    axVSPrinter1.Table = "<3000;Date : " + sOLogin;
                    axVSPrinter1.Paragraph = "";

                    if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
                    {
                        //pSet1.Query = "select distinct(usr_code) as user from a_trail where mod_code = 'ABA' and ";
                        pSet1.Query = "select distinct(usr_code) as sys_user from a_trail where mod_code = 'ABA' and "; // RMC 20161122 corrected error in encoder's report
                        pSet1.Query += "usr_code like '" + txtUserCode.Text + "%' and ";
                        pSet1.Query += "trunc(tdatetime) = to_date('" + sOLogin + "','MM/dd/yyyy')";
                    }
                    else // based on records
                    {
                        //pSet1.Query = "select distinct(bns_user) as user from businesses where bns_user like '" + txtUserCode.Text + "%' ";
                        pSet1.Query = "select distinct(bns_user) as sys_user from businesses where bns_user like '" + txtUserCode.Text + "%' "; // RMC 20161122 corrected error in encoder's report
                        pSet1.Query += "and to_date(save_tm) = to_date('" + sOLogin + "','MM/dd/yyyy')";
                    }
                    if (pSet1.Execute())
                    {
                        while (pSet1.Read())
                        {
                            //sUserCode = pSet1.GetString("user").Trim();
                            sUserCode = pSet1.GetString("sys_user").Trim(); // RMC 20161122 corrected error in encoder's report
                            iCount++;

                            pSet2.Query = "select * from sys_users where usr_code = '" + sUserCode + "'";
                            if (pSet2.Execute())
                                if (pSet2.Read())
                                {
                                    sLName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_ln").Trim());
                                    sFName = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_fn").Trim());
                                    sMI = StringUtilities.StringUtilities.RemoveApostrophe(pSet2.GetString("usr_mi").Trim());
                                }
                            pSet2.Close();

                            if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
                            {
                                pSet2.Query = "select count(distinct object) as rec_count from a_trail where mod_code = 'ABA' and ";
                                pSet2.Query += "trunc(tdatetime) = to_date('" + sOLogin + "','MM/dd/yyyy') ";
                                pSet2.Query += "and usr_code = '" + sUserCode + "'";
                            }
                            else // based on records
                            {
                                pSet2.Query = "select count(*) as rec_count from businesses where bns_user = '" + sUserCode + "' ";
                                pSet2.Query += "and to_date(save_tm) = to_date('" + sOLogin + "','MM/dd/yyyy')";
                            }
                            if (pSet2.Execute())
                            {
                                while (pSet2.Read())
                                {
                                    sNumRecord = pSet2.GetInt("rec_count").ToString().Trim();

                                    sBContent += sUserCode + "|";
                                    sBContent += sLName + ", ";
                                    sBContent += sFName + " ";
                                    sBContent += sMI + "|";
                                    sBContent += sNumRecord + ";";

                                    iNumRecord += Convert.ToInt32(sNumRecord);
                                }
                            }
                            pSet2.Close();

                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = sBContent;
                            sBContent = "<1500|<5000|^4000;";
                        }
                        axVSPrinter1.Paragraph = "";
                        axVSPrinter1.Table = "<2500;Total Number of Encoder: " + iCount.ToString("#,##0");
                        axVSPrinter1.Table = "<3500;Total Number of Encoded Record: " + iNumRecord.ToString("#,##0");
                    }
                    pSet1.Close();
                }
            }
            else
            {
                MessageBox.Show("No Record(s) found!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            pSet.Close();

            if (iCount > 0)
            {
                if (AppSettingsManager.GetConfigValue("64") == "1") // based on trail
                    axVSPrinter1.Table = "<2500;Note: Based on trail";
                else// based on records
                    axVSPrinter1.Table = "<2500;Note: Based on records";
            }

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

    }
}