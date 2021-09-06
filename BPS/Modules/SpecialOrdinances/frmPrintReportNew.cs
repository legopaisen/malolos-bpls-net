using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.SpecialOrdinances
{
    public partial class frmPrintReportNew : Form
    {
        public string m_strSource = string.Empty;
        public string m_sUser = string.Empty;
        public string m_sFrom = string.Empty;
        public string m_sTo = string.Empty;
        public string m_sTag = string.Empty;
        public string m_sBnsType = string.Empty;

        public frmPrintReportNew()
        {
            InitializeComponent();
        }

        private void PrintForm()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            string sBoiBin = "";
			string sBoiFrom = "";
            string sBoiTo = "";
			string sBoiTag = "";
			string sBoiUser = "";
			string sBnsType = "";
            string sBoiBnsNm = "";
		    string sBoiOwner = "";
		    string sBoiStat = "";
		    string sContent = "";
            string sCnt = "";
            int iCnt = 0;

            if (m_sUser == "%" || m_sUser == "")
                sContent = "User: ALL";
            else
                sContent = "User: " + m_sUser;

            result.Query = string.Format("select * from boi_table where user_code like '{0}%'", m_sUser);
            if (m_sFrom != "" && m_sTo != "" && m_sFrom != "%" && m_sTo != "%")
            {
                result.Query += string.Format(" and datefrom >= to_date('{0}','MM/dd/yyyy') and dateto <= to_date('{1}','MM/dd/yyyy') ", m_sFrom, m_sTo);

                sContent += "  Date From:" + m_sFrom + " to " + m_sTo;
            }
            else
                sContent += "  Date Effectivity: ALL";

            if (m_sBnsType == "%" || m_sBnsType == "")
                sContent += "  Business Type: ALL";
            else
                sContent += "  Business Type: " + m_sBnsType;

        	result.Query+= string.Format(" and bns_type like '{0}%'", m_sBnsType);
            if (m_sTag != "" && m_sTag != "%")
            {
                result.Query += string.Format(" and date_save like ('{0}%')", m_sTag);

                sContent += "  Tagged Date: " + m_sTag;
            }
            else
                sContent += "  Tagged Date: ALL";

	        result.Query+= string.Format(" and exempt_type = '{0}'", m_strSource);
            result.Query += " order by bin";
            if(result.Execute())
            {
                this.axVSPrinter1.FontBold = true;
                this.axVSPrinter1.Table = string.Format("<10000;{0}", sContent);
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.Paragraph = "";
                this.axVSPrinter1.FontBold = false;

                while (result.Read())
                {
                    iCnt++;
                    sBoiBin = result.GetString("bin");
                    sBoiFrom = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("datefrom"));
                    sBoiTo = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("dateto"));
                    sBoiTag = string.Format("{0:MM/dd/yyyy}", DateTime.Parse(result.GetString("date_save")));
                    sBoiUser = result.GetString("user_code");
                    sBoiUser = AppSettingsManager.GetUserName(sBoiUser);
                    sBnsType = result.GetString("bns_type");

                    pSet.Query = string.Format("select bns_nm, own_code, bns_stat from businesses where bin = '{0}'", sBoiBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            sBoiBnsNm = pSet.GetString("bns_nm");
                            sBoiOwner = pSet.GetString("own_code");
                            sBoiStat = pSet.GetString("bns_stat");
                            sBoiOwner = AppSettingsManager.GetBnsOwner(sBoiOwner);
                        }
                    }
                    pSet.Close();

                    sCnt = string.Format("{0:##}",iCnt);

                    sContent = sCnt + ".) " + sBoiBin + "|";
                    sContent += sBoiBnsNm + "|";
                    sContent += sBoiOwner + "|";
                    sContent += sBoiStat + "|";
                    sContent += sBoiFrom + "|";
                    sContent += sBoiTo;
                    
                    this.axVSPrinter1.Table = string.Format("<2500|<4000|<4000|^800|^1100|^1100;{0}", sContent);
                    
                }
            }
            result.Close();
		
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.Paragraph = "";
            
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Printed by:|{0}", AppSettingsManager.SystemUser.UserCode);
            this.axVSPrinter1.Table = string.Format("<2000|<5000;Date printed:|{0}", AppSettingsManager.GetCurrentDate());
        }

        

        private void frmPrintReportNew_Load(object sender, EventArgs e)
        {
            this.axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprA4;
            this.axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape;

            this.axVSPrinter1.MarginLeft = 1000;
            this.axVSPrinter1.MarginTop = 700;
            this.axVSPrinter1.MarginBottom = 500;

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;

            //this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            this.PrintForm();

            this.axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void axVSPrinter1_NewPageEvent(object sender, EventArgs e)
        {
            string strProvinceName = string.Empty;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taCenterMiddle;
            this.axVSPrinter1.FontBold = true;
            this.axVSPrinter1.FontSize = (float)10.0;
            this.axVSPrinter1.Paragraph = "Republic of the Philippines";

            strProvinceName = AppSettingsManager.GetConfigValue("08");
            if (strProvinceName != string.Empty)
                this.axVSPrinter1.Paragraph = strProvinceName;
            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("09");

            this.axVSPrinter1.Paragraph = AppSettingsManager.GetConfigValue("41");
            this.axVSPrinter1.Paragraph = "";

            if (m_strSource == "BOI")
                this.axVSPrinter1.Paragraph = "LIST OF BUSINESSES UNDER B.O.I.";
            else
                this.axVSPrinter1.Paragraph = "LIST OF PEZA MEMBERS";
            this.axVSPrinter1.Paragraph = "";

            this.axVSPrinter1.FontBold = false;
            this.axVSPrinter1.TextAlign = VSPrinter7Lib.TextAlignSettings.taLeftTop;
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontSize = (float)8.0;

            string strData = "";
            this.axVSPrinter1.FontUnderline = true;
            this.axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            strData = "||||TAX HOLIDAY";
            this.axVSPrinter1.Table = string.Format("^2500|^4000|^4000|^800|^2200;{0}", strData);
            strData = "B I N|BUSINESS NAME|OWNER|STATUS|FROM|TO";
            this.axVSPrinter1.Table = string.Format("^2500|^4000|^4000|^800|^1100|^1100;{0}", strData);
            this.axVSPrinter1.Paragraph = "";
            this.axVSPrinter1.FontUnderline = false;
        }
    }
}