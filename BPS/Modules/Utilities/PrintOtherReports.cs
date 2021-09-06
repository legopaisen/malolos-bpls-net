// RMC 20110908 corrected printing of date & time in audit trail

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.PrintUtilities;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;

namespace Amellar.Modules.Utilities
{
    public class PrintOtherReports
    {
        private VSPrinterEmuModel model;
        private VSPrinterEmuModel pageHeaderModel;
        private string m_strSource = string.Empty;
        private string m_strReportName = string.Empty;
        private string m_strQuery = string.Empty;
        private string m_strBIN = string.Empty;
                                
        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string ReportName
        {
            get { return m_strReportName; }
            set { m_strReportName = value; }
        }

        public string BIN
        {
            get { return m_strBIN; }
            set { m_strBIN = value; }
        }

        public string ReportQuery
        {
            get { return m_strQuery; }
            set { m_strQuery = value; }
        }
        public PrintOtherReports()
        {
            model = new VSPrinterEmuModel();
            model.LeftMargin = 50;
            model.TopMargin = 50;
            model.MaxY = 1100 - 200;

            pageHeaderModel = new VSPrinterEmuModel();
            pageHeaderModel.LeftMargin = 50;
            pageHeaderModel.TopMargin = 50;
            pageHeaderModel.MaxY = 1100 - 200;

        }

        public void FormLoad()
        {
            CreateHeader();
            CreatePageHeader();

            if (m_strSource == "1")
            {
                PrintBrgyList();
            }
            else if (m_strSource == "2")
            {
                PrintConfiguration();
            }
            else if (m_strSource == "3")
            {
                PrintAuditTrailByBin();
            }
            else if (m_strSource == "4")
            {
                PrintAuditTrailByUser();
            }
            else if(m_strSource == "5")
            {
                PrintAuditTrailByTrans();
            }
            else if (m_strSource == "6")
            {
                PrintAuditTrailByUserTrans();
            }

            pageHeaderModel.PageCount = model.PageCount;
            PrintReportPreview();
        }

        private void CreateHeader()
        {
            string strProvinceName = string.Empty;
            model.SetTextAlign(1);
            model.SetFontBold(1);
            model.SetFontSize(10);
            model.SetParagraph("Republic of the Philippines");
            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                model.SetParagraph(strProvinceName);

            model.SetParagraph(AppSettingsManager.GetConfigValue("09"));

            //model.SetParagraph(ConfigurationAttributes.OfficeName);   // RMC 20110809 deleted
            model.SetParagraph(string.Empty);

            model.SetParagraph(m_strReportName);
            model.SetParagraph(string.Empty);

            model.SetFontBold(0);
            model.SetTextAlign(0);
            model.SetTable(string.Empty);
            model.SetFontSize(8);

            string strData = string.Empty;
            if (m_strSource == "1")
            {
                model.SetFontUnderline(1);
                strData = "BARANGAY CODE|BARANGAY NAME|ZONE";
                model.SetTable(string.Format("^2000|^5000|^3000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontUnderline(0);
            }
            else if (m_strSource == "2")
            {
                model.SetFontUnderline(1);
                
                //MCR 20141015 (s)
                //strData = "CODE|HEADER NAME|VALUES|SECURITY";
                //model.SetTable(string.Format("^500|^3000|^3000|^1000;{0}", strData));
                strData = "CODE|HEADER NAME|VALUES";
                model.SetTable(string.Format("^500|^4500|^3000;{0}", strData));
                //MCR 20141015 (e)

                model.SetTable(string.Empty);
                model.SetFontUnderline(0);
            }
            else if (m_strSource == "6") //AFM 20190808 audit trail new report - malolos ver.
            {
                model.SetFontUnderline(1);
                strData = "SYSTEM USER|BUSINESS RECORD APPLICATION BILLING|BUSINESS PERMIT|DATE TIME";
                model.SetTable(string.Format("^2000|^3000|^3000|^2000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontUnderline(0);
            }
            else
            {
                model.SetFontUnderline(1);
                strData = "DATE TIME|OBJECT|TRANSACTION|SYSTEM USER|STATION";
                model.SetTable(string.Format("^1500|^3000|^3000|^2000|^1000;{0}", strData));
                model.SetTable(string.Empty);
                model.SetFontUnderline(0);
            }

        }

        private void CreatePageHeader()
        {
            pageHeaderModel.Clear();
            string strProvinceName = string.Empty;
            pageHeaderModel.SetTextAlign(1);
            pageHeaderModel.SetFontBold(1);
            pageHeaderModel.SetFontSize(10);
            pageHeaderModel.SetParagraph("Republic of the Philippines");

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                pageHeaderModel.SetParagraph(strProvinceName);
            pageHeaderModel.SetParagraph(AppSettingsManager.GetConfigValue("09"));

            //pageHeaderModel.SetParagraph(ConfigurationAttributes.OfficeName); // RMC 20110809 deleted
            pageHeaderModel.SetParagraph(string.Empty);

            pageHeaderModel.SetFontBold(0);
            pageHeaderModel.SetTextAlign(0);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetTable(string.Empty);
            pageHeaderModel.SetFontSize(8);


            string strData;
            if (m_strSource == "1")
            {
                pageHeaderModel.SetFontUnderline(1);
                strData = "BARANGAY CODE|BARANGAY NAME|ZONE";
                pageHeaderModel.SetTable(string.Format("^2000|^5000|^3000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontUnderline(0);
            }
            else if (m_strSource == "2")
            {
                pageHeaderModel.SetFontUnderline(1);
                strData = "CODE|HEADER NAME|VALUES|SECURITY";
                pageHeaderModel.SetTable(string.Format("^500|^3000|^3000|^1000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontUnderline(0);
            }
            else if (m_strSource == "6")
            {
                pageHeaderModel.SetFontUnderline(1);
                strData = "SYSTEM USER|BUSINESS RECORD APPLICATION BILLING|BUSINESS PERMIT|DATE TIME";
                pageHeaderModel.SetTable(string.Format("^2000|^3000|^3000|^2000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontUnderline(0);
            }
            else
            {
                pageHeaderModel.SetFontUnderline(1);
                strData = "DATE TIME|OBJECT|TRANSACTION|SYSTEM USER|STATION";
                pageHeaderModel.SetTable(string.Format("^1500|^3000|^3000|^2000|^1000;{0}", strData));
                pageHeaderModel.SetTable(string.Empty);
                pageHeaderModel.SetFontUnderline(0);
            }

        }

        private void PrintBrgyList()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet rBrgy = new OracleResultSet();

            string strDistCode = string.Empty;
            string strDistName = string.Empty;
            string strData = string.Empty;
            string strBrgyCode = string.Empty;
			string strBrgyName = string.Empty;
            string strZone = string.Empty;
            int iTotalRecCount = 0;
            
            result.Query = "select distinct dist_code,dist_nm from brgy order by dist_nm";
	        if(result.Execute())
            {
                while (result.Read())
		        {
                    try
                    {
			            strDistCode = result.GetString("dist_code");
			            strDistName = result.GetString("dist_nm");
                    }
                    catch
                    {
                        strDistCode = "";
                        strDistName = "";
                    }

                    model.SetFontBold(1);
                    model.SetTable(string.Format("<1100;{0}", strDistName));
			        model.SetFontBold(0);

			        rBrgy.Query = "select distinct brgy_code,brgy_nm,zone from brgy ";
                    if(strDistCode != "")
                        rBrgy.Query += string.Format("where dist_code = '{0}' ", strDistCode);
                    rBrgy.Query += "order by brgy_nm";
                    if(rBrgy.Execute())
                    {
                        while(rBrgy.Read())
			            {
					        iTotalRecCount++;
					        
					        strBrgyCode = rBrgy.GetString("brgy_code");
					        strBrgyName = rBrgy.GetString("brgy_nm");
					        strZone	= rBrgy.GetString("zone");

					        strData = strBrgyCode + "|" + strBrgyName + "|" + strZone; 
					        model.SetTable(string.Format("^2000|<5000|<3000;{0}", strData));
                        }
                    }
                    rBrgy.Close();
                }
            }
            result.Close();
		
	        strData = string.Format("{0:#,##}", iTotalRecCount);
	        model.SetParagraph("");
            model.SetParagraph("");
            model.SetFontBold(1);
            model.SetTable(string.Format("<2500|<1000;Total No. of Barangay :|{0}", strData));
            model.SetFontBold(0);

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

           
        }

        private void PrintReportPreview()
        {
            model.MaxY = 1100;
            VSPrinterEmuDocument doc = new VSPrinterEmuDocument();
            doc.Model = model;

            doc.PageHeaderModel = pageHeaderModel;
            doc.Model.Reset();

            doc.DefaultPageSettings.Margins.Top = 50;
            doc.DefaultPageSettings.Margins.Left = 100;
            doc.DefaultPageSettings.Margins.Bottom = 50;
            doc.DefaultPageSettings.Margins.Right = 100;


            doc.DefaultPageSettings.PaperSize = new PaperSize("", 827, 1169);

            doc.PrintPage += new PrintPageEventHandler(_Doc_PrintPage);

            frmMyPrintPreviewDialog dlgPreview = new frmMyPrintPreviewDialog();
            dlgPreview.Document = doc;
            dlgPreview.ClientSize = new System.Drawing.Size(640, 480);
            dlgPreview.ShowIcon = true;
            //dlgPreview.IsAllowExport = true;
            dlgPreview.WindowState = FormWindowState.Maximized;
            dlgPreview.ShowDialog();
            model.Dispose();
        }

        private void _Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //bool blnVisibleState = this.Visible;
            VSPrinterEmuDocument doc = (VSPrinterEmuDocument)sender;
            doc.Render(e);
        }

        private void PrintConfiguration()
        {
            OracleResultSet result = new OracleResultSet();
            string strCode = string.Empty;
            string strRemarks = string.Empty;
            string strObject = string.Empty;
            string strSecurity = string.Empty;
            string strData = string.Empty;

            result.Query = string.Format("delete from gen_info where report_name = '{0}' and system = 'ASS'", m_strReportName);
            if (result.ExecuteNonQuery() == 0)
            {
            }

            result.Query = "insert into gen_info values (:1, :2, :3, :4, :5)";
            result.AddParameter(":1", m_strReportName);
            result.AddParameter(":2", AppSettingsManager.GetCurrentDate());
            result.AddParameter(":3", AppSettingsManager.SystemUser.UserCode);
            result.AddParameter(":4", "1");
            result.AddParameter(":5", "ASS");
            if (result.ExecuteNonQuery() == 0)
            {
            }

            result.Query = "select * from config order by code";
            if(result.Execute())
            {
                while (result.Read())
                {
                    strCode = result.GetString("code");
                    strRemarks = result.GetString("remarks");
                    //strObject = StringUtilities.HandleApostrophe(result.GetString("object")); //REM MCR 20141015
                    strObject = StringUtilities.HandleApostrophe(result.GetString("object")).Replace(';',' ').Replace('|',' ');
                    //MOD MCR 20141015 (s)
                    //strSecurity = result.GetString("security");

                    //strData = strCode + "|" + strRemarks + "|" + strObject + "|" + strSecurity;
                    strData = strCode + "|" + strRemarks + "|" + strObject;
                    //model.SetTable(string.Format("<500|<3000|<3000|<1000;{0}", strData));
                    model.SetTable(string.Format("<500|<4500|<3000;{0}", strData));
                    //MOD MCR 20141015 (e)
                }
            }
            result.Close();

            result.Query = string.Format("update gen_info set switch = 0 where report_name = '{0}' and system = 'ASS'", m_strReportName);
            if (result.ExecuteNonQuery() == 0)
            {
            }

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

            if (AuditTrail.InsertTrail("AUSP", "config", "Configuration report") == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void PrintAuditTrailByBin()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            string sRetrieveUser = string.Empty;
	        string sRetrieveDateTime = string.Empty;
	        string sRetrieveModeCode = string.Empty;
            string sRetrieveTable = string.Empty;
            string sRetrieveWorkstation = string.Empty;
	        string sRetrieveObject = string.Empty;
	        string sRetrieveUserName = string.Empty;
		    string sRetrieveTransaction = string.Empty;
            string strData = string.Empty;

            string strRep = "USERS LOG BY BUSINESS RECORDS";

            pSet.Query = string.Format("delete from rep_a_trail where user_code = '{0}' and report_name = '{1}'", AppSettingsManager.SystemUser.UserCode, strRep);
            if(pSet.ExecuteNonQuery() == 0)
            {
            }

            result.Query = m_strQuery;
            if (result.Execute())
            {
                strData = "BIN: " + m_strBIN;
                model.SetTable(string.Format("<2500;{0}", strData));
                model.SetTable("");

                while (result.Read())
                {
                    sRetrieveUser = result.GetString("usr_code");
			        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy mm:hh:ss}", result.GetDateTime("tdatetime"));
                    //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", result.GetDateTime("tdatetime"));  // RMC 20110908 corrected printing of date & time in audit trail
                    sRetrieveDateTime = Convert.ToString(result.GetDateTime("tdatetime"));  // RMC 20111122 corrected date format in printing trail
                    sRetrieveModeCode = result.GetString("mod_code");
                    sRetrieveTable = result.GetString("aff_table");
                    sRetrieveWorkstation = result.GetString("work_station");
			        sRetrieveObject = result.GetString("object");
                    sRetrieveObject = ConcatString(sRetrieveObject,38);
                    
			        sRetrieveUserName = AppSettingsManager.GetUserName(sRetrieveUser);
                    sRetrieveUserName = ConcatString(sRetrieveUserName, 20);
			        			
			        sRetrieveTransaction = "";
			
                    pSet.Query = "select right_desc from trail_table where usr_rights = '"+sRetrieveModeCode+"'";
			        if(pSet.Execute())
                    {
                        if(pSet.Read())
			            {
                            sRetrieveTransaction = pSet.GetString("right_desc");
                        }
                    }
                    pSet.Close();
			
                    sRetrieveTransaction = '('+sRetrieveModeCode+')' +' '+ sRetrieveTransaction;

                    sRetrieveTransaction = ConcatString(sRetrieveTransaction,35);

                    strData = sRetrieveDateTime+ "|" +sRetrieveObject+ "|" +sRetrieveTransaction + "|" +sRetrieveUserName+ "|" +sRetrieveWorkstation; 
                    model.SetTable(string.Format("<1500|<3000|<3000|<2000|<1000;{0}", strData));

                    
                    pSet.Query = "insert into rep_a_trail values(:1,:2,' ',:3,:4,:5,:6,:7,:8,:9)";
                    pSet.AddParameter(":1", AppSettingsManager.GetConfigValue("09"));
                    pSet.AddParameter(":2", sRetrieveUserName);
                    pSet.AddParameter(":3", sRetrieveDateTime);
                    pSet.AddParameter(":4", sRetrieveTransaction);
                    pSet.AddParameter(":5", sRetrieveTable);
                    pSet.AddParameter(":6", sRetrieveWorkstation);
                    pSet.AddParameter(":7", sRetrieveObject);
                    pSet.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                    pSet.AddParameter(":9", strRep);
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }

                }
            }
            result.Close();

            model.SetTable(string.Empty);
            model.SetTable(string.Empty);

            model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
            model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));

        }

        

        private string ConcatString(string strValue,int intTmpLength)
        {
            string sTmp = string.Empty;

            sTmp = strValue;
            strValue = StringUtilities.Left(sTmp, intTmpLength);

            int intLength = 0;
            intLength = sTmp.Length;
            
            if (intLength > intTmpLength)
            {
                while (intLength > intTmpLength)
                {
                    intLength -= intTmpLength;
                    sTmp = StringUtilities.Right(sTmp, intLength);

                    if (intLength >= intTmpLength)
                        strValue += "\n" + StringUtilities.Left(sTmp, intTmpLength);

                }

                if (sTmp.Length < intTmpLength)
                    strValue += "\n" + sTmp;
            }
                        
            return strValue;
        }

        private void PrintAuditTrailByUser()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            string sRetrieveUser = string.Empty;
            string sRetrieveDateTime = string.Empty;
            string sRetrieveModeCode = string.Empty;
            string sRetrieveTable = string.Empty;
            string sRetrieveWorkstation = string.Empty;
            string sRetrieveObject = string.Empty;
            string sRetrieveUserName = string.Empty;
            string sRetrieveTransaction = string.Empty;
            string strData = string.Empty;

            string strRep = "USERS LOG BY USER";

            pSet.Query = string.Format("delete from rep_a_trail where user_code = '{0}' and report_name = '{1}'", AppSettingsManager.SystemUser.UserCode, strRep);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            try
            {
                result.Query = m_strQuery;
                if (result.Execute())
                {
                    strData = "SYSTEM USER: " + m_strBIN;
                    model.SetTable(string.Format("<4000;{0}", strData));
                    model.SetTable("");

                    while (result.Read())
                    {
                        sRetrieveUser = result.GetString("usr_code");
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy mm:hh:ss}", result.GetDateTime("tdatetime"));
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", result.GetDateTime("tdatetime"));  // RMC 20110908 corrected printing of date & time in audit trail
                        sRetrieveDateTime = Convert.ToString(result.GetDateTime("tdatetime"));  // RMC 20111122 corrected date format in printing trail
                        sRetrieveModeCode = result.GetString("mod_code");
                        sRetrieveTable = result.GetString("aff_table");
                        sRetrieveWorkstation = result.GetString("work_station");
                        sRetrieveObject = StringUtilities.RemoveOtherCharacters(result.GetString("object"));
                        sRetrieveObject = ConcatString(sRetrieveObject, 32);

                        sRetrieveUserName = AppSettingsManager.GetUserName(sRetrieveUser);
                        sRetrieveUserName = ConcatString(sRetrieveUserName, 20);

                        sRetrieveTransaction = "";

                        pSet.Query = "select right_desc from trail_table where usr_rights = '" + sRetrieveModeCode + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sRetrieveTransaction = pSet.GetString("right_desc");
                            }
                        }
                        pSet.Close();

                        sRetrieveTransaction = '(' + sRetrieveModeCode + ')' + ' ' + sRetrieveTransaction;

                        sRetrieveTransaction = ConcatString(sRetrieveTransaction, 35);

                        strData = sRetrieveDateTime + "|" + sRetrieveObject + "|" + sRetrieveTransaction + "|" + sRetrieveUserName + "|" + sRetrieveWorkstation;
                        model.SetTable(string.Format("<1500|<3000|<3000|<2000|<1000;{0}", strData));


                        pSet.Query = "insert into rep_a_trail values(:1,:2,' ',:3,:4,:5,:6,:7,:8,:9)";
                        pSet.AddParameter(":1", AppSettingsManager.GetConfigValue("09"));
                        pSet.AddParameter(":2", sRetrieveUserName);
                        pSet.AddParameter(":3", sRetrieveDateTime);
                        pSet.AddParameter(":4", sRetrieveTransaction);
                        pSet.AddParameter(":5", sRetrieveTable);
                        pSet.AddParameter(":6", sRetrieveWorkstation);
                        pSet.AddParameter(":7", sRetrieveObject);
                        pSet.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                        pSet.AddParameter(":9", strRep);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                    }
                }
                result.Close();

                model.SetTable(string.Empty);
                model.SetTable(string.Empty);

                model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //AFM20190807 user transactions audit trail (s)
        private void PrintAuditTrailByUserTrans()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            string sRetrieveUser = string.Empty;
            string sRetrieveDateTime = string.Empty;
            string sRetrieveModeCode = string.Empty;
            string sRetrieveTable = string.Empty;
            string sRetrieveWorkstation = string.Empty;
            string sRetrieveObject = string.Empty;
            string sRetrieveUserName = string.Empty;
            string sRetrieveTransaction = string.Empty;
            string sRetrieveBnsPermit = string.Empty; //AFM 20190808
            string sTransactionAlias = string.Empty; //AFM 20190809
            string strData = string.Empty;

            string strRep = "USERS LOG BY USER";

            pSet.Query = string.Format("delete from rep_a_trail where user_code = '{0}' and report_name = '{1}'", AppSettingsManager.SystemUser.UserCode, strRep);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            try
            {
                result.Query = m_strQuery;
                if (result.Execute())
                {
                    //strData = "SYSTEM USER: " + m_strBIN;
                    //model.SetTable(string.Format("<4000;{0}", strData));
                    //model.SetTable("");

                    while (result.Read())
                    {
                        sRetrieveUser = result.GetString("usr_code");
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy mm:hh:ss}", result.GetDateTime("tdatetime"));
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", result.GetDateTime("tdatetime"));  // RMC 20110908 corrected printing of date & time in audit trail
                        sRetrieveDateTime = Convert.ToString(result.GetDateTime("tdatetime"));  // RMC 20111122 corrected date format in printing trail
                        sRetrieveModeCode = result.GetString("mod_code");
                        sRetrieveTable = result.GetString("aff_table");
                        sRetrieveWorkstation = result.GetString("work_station");
                        sRetrieveObject = StringUtilities.RemoveOtherCharacters(result.GetString("object"));
                        sRetrieveObject = ConcatString(sRetrieveObject, 32);
                        sRetrieveBnsPermit = result.GetString("permit_no"); //AFM20190808

                        sRetrieveUserName = AppSettingsManager.GetUserName(sRetrieveUser);
                        sRetrieveUserName = ConcatString(sRetrieveUserName, 20);

                        sRetrieveTransaction = "";

                        //AFM 20190809 modified display of transactions - malolos ver. (s)
                        if (sRetrieveModeCode == "AACA")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - CANCEL APPLICATION";
                        }
                        if (sRetrieveModeCode == "AAE")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - RETIREMENT";
                        }
                        if (sRetrieveModeCode == "AANABA")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - NEW(ADD)";
                        }
                        if (sRetrieveModeCode == "AANABE")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - NEW(EDIT)";
                        }
                        if (sRetrieveModeCode == "AARABA")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - RENEWAL(ADD)";
                        }
                        if (sRetrieveModeCode == "AARABE")
                        {
                            sRetrieveTransaction = "BUSINESS APPLICATION - RENEWAL(EDIT)";
                        }
                        //AFM 20190809 modified display of transactions - malolos ver. (e)

                        //pSet.Query = "select right_desc from trail_table where usr_rights = '" + sRetrieveModeCode + "'";
                        //if (pSet.Execute())
                        //{
                        //    if (pSet.Read())
                        //    {
                        //        sRetrieveTransaction = pSet.GetString("right_desc");
                        //    }
                        //}
                        pSet.Close();

                        //sRetrieveTransaction = '(' + sRetrieveModeCode + ')' + ' ' + sRetrieveTransaction;

                        sRetrieveTransaction = ConcatString(sRetrieveTransaction, 35);

                        strData = sRetrieveUserName + "|" + sRetrieveTransaction + "|" + sRetrieveBnsPermit + "|" + sRetrieveDateTime;
                        model.SetTable(string.Format("^2000|^3000|^3000|^2000;{0}", strData));


                        pSet.Query = "insert into rep_a_trail values(:1,:2,' ',:3,:4,:5,:6,:7,:8,:9)";
                        pSet.AddParameter(":1", AppSettingsManager.GetConfigValue("09"));
                        pSet.AddParameter(":2", sRetrieveUserName);
                        pSet.AddParameter(":3", sRetrieveDateTime);
                        pSet.AddParameter(":4", sRetrieveTransaction);
                        pSet.AddParameter(":5", sRetrieveTable);
                        pSet.AddParameter(":6", sRetrieveWorkstation);
                        pSet.AddParameter(":7", sRetrieveObject);
                        pSet.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                        pSet.AddParameter(":9", strRep);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                    }
                }
                result.Close();

                model.SetTable(string.Empty);
                model.SetTable(string.Empty);

                model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        //AFM20190807 user transactions audit trail (e)

        private void PrintAuditTrailByTrans()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            int iCtr = 0;
            string sRetrieveUser = string.Empty;
            string sRetrieveDateTime = string.Empty;
            string sRetrieveModeCode = string.Empty;
            string sRetrieveTable = string.Empty;
            string sRetrieveWorkstation = string.Empty;
            string sRetrieveObject = string.Empty;
            string sRetrieveUserName = string.Empty;
            string sRetrieveTransaction = string.Empty;
            string strData = string.Empty;

            string strRep = "USERS LOG BY TRANSACTION";

            pSet.Query = string.Format("delete from rep_a_trail where user_code = '{0}' and report_name = '{1}'", AppSettingsManager.SystemUser.UserCode, strRep);
            if (pSet.ExecuteNonQuery() == 0)
            {
            }

            try
            {
                result.Query = m_strQuery;
                if (result.Execute())
                {
                    strData = "MODULE: " + m_strBIN;
                    model.SetTable(string.Format("<4000;{0}", strData));
                    model.SetTable("");

                    while (result.Read())
                    {
                        iCtr += 1;
                        sRetrieveUser = result.GetString("usr_code");
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy mm:hh:ss}", result.GetDateTime("tdatetime"));
                        //sRetrieveDateTime = string.Format("{0:MM/dd/yyyy hh:mm:ss}", result.GetDateTime("tdatetime"));  // RMC 20110908 corrected printing of date & time in audit trail
                        sRetrieveDateTime = Convert.ToString(result.GetDateTime("tdatetime"));  // RMC 20111122 corrected date format in printing trail
                        sRetrieveModeCode = result.GetString("mod_code");
                        sRetrieveTable = result.GetString("aff_table");
                        sRetrieveWorkstation = result.GetString("work_station");
                        sRetrieveObject = StringUtilities.RemoveOtherCharacters(result.GetString("object"));
                        sRetrieveObject = ConcatString(sRetrieveObject, 32);

                        sRetrieveUserName = AppSettingsManager.GetUserName(sRetrieveUser);
                        sRetrieveUserName = ConcatString(sRetrieveUserName, 20);

                        sRetrieveTransaction = "";

                        pSet.Query = "select right_desc from trail_table where usr_rights = '" + sRetrieveModeCode + "'";
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                sRetrieveTransaction = pSet.GetString("right_desc");
                            }
                        }
                        pSet.Close();

                        sRetrieveTransaction = '(' + sRetrieveModeCode + ')' + ' ' + sRetrieveTransaction;

                        sRetrieveTransaction = ConcatString(sRetrieveTransaction, 35);

                        strData = sRetrieveDateTime + "|" + sRetrieveObject + "|" + sRetrieveTransaction + "|" + sRetrieveUserName + "|" + sRetrieveWorkstation;
                        model.SetTable(string.Format("<1500|<3000|<3000|<2000|<1000;{0}", strData));


                        pSet.Query = "insert into rep_a_trail values(:1,:2,' ',:3,:4,:5,:6,:7,:8,:9)";
                        pSet.AddParameter(":1", AppSettingsManager.GetConfigValue("09"));
                        pSet.AddParameter(":2", sRetrieveUserName);
                        pSet.AddParameter(":3", sRetrieveDateTime);
                        pSet.AddParameter(":4", sRetrieveTransaction);
                        pSet.AddParameter(":5", sRetrieveTable);
                        pSet.AddParameter(":6", sRetrieveWorkstation);
                        pSet.AddParameter(":7", sRetrieveObject);
                        pSet.AddParameter(":8", AppSettingsManager.SystemUser.UserCode);
                        pSet.AddParameter(":9", strRep);
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                    }
                }
                result.Close();

                model.SetTable(string.Empty);
                model.SetTable(string.Empty);
                model.SetTable(string.Format("<2000|<5000;Total No of Records:|{0}", string.Format("{0:#,###}", iCtr)));
                model.SetTable(string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode));
                model.SetTable(string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate()));
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
            
    }
}
