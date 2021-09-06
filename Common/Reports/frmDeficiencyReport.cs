using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.ContainerWithShadow;

namespace Amellar.Common.Reports
{
    public partial class frmDeficiencyReport : Form
    {
        public frmDeficiencyReport()
        {
            InitializeComponent();
        }
        private int m_iRadioSum = 0;

        private void frmDeficiencyReport_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
            PrepareCategory();
            PrepareType();
            cmbStatus.SelectedIndex = 0;
            rdoSummary.PerformClick();
        }

        private void PrepareCategory()
        {
            cmbCategory.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from def_cat_table order by cat_name";
            if (pSet.Execute())
            {
                cmbCategory.Items.Add("ALL");
                while (pSet.Read())
                {
                    cmbCategory.Items.Add(StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("cat_name")).Trim());
                }
            }
            pSet.Close();

            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void PrepareType()
        {
            cmbType.Items.Clear();
            OracleResultSet pSet = new OracleResultSet();
            pSet.Query = "select * from def_rec_table order by def_name";
            if (pSet.Execute())
            {
                cmbType.Items.Add("ALL");
                while (pSet.Read())
                {
                    cmbType.Items.Add(StringUtilities.StringUtilities.HandleApostrophe(pSet.GetString("def_name")).Trim());
                }
            }
            pSet.Close();

            if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;
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

        private void rdoSummary_Click(object sender, EventArgs e)
        {
            m_iRadioSum = 1;
        }

        private void rdoList_Click(object sender, EventArgs e)
        {
            m_iRadioSum = 2;
        }

        private void OnSumDeficient()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            String sQuery, sColHeader, sContent, sFrom, sTo, sHeader, sDefCode;
            String sStat1 = "", sDefName, sCatName, sTemp = "";
            String sCategory, sType;
            int iCount = 0, iDCount = 0;

            sFrom = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            sTo = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);
            sColHeader = "^1000|^5000|^1500;Code|Deficient Name|Total Records";

            sHeader = "^7500;";
            sHeader += AppSettingsManager.GetConfigValue("09");
            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait; // 0 portrait 1 landscape
            axVSPrinter1.MarginLeft = 2500;
            axVSPrinter1.MarginTop = 700;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^7500;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "^7500;Summary of Deficient Records";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.Table = "^7500;Period   from " + sFrom + "  to " + sTo;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            if (cmbStatus.Text == "CORRECTED")
                sStat1 = "Y";
            else if (cmbStatus.Text == "UNCORRECTED")
                sStat1 = "N";

            if (cmbCategory.Text == "ALL")
                sCategory = "";
            else
                sCategory = cmbCategory.Text;

            if (cmbType.Text == "ALL")
                sType = "";
            else
                sType = cmbType.Text;

            sQuery = "select distinct cat_name from def_rec_table,def_cat_table where def_rec_table.cat_code = def_cat_table.cat_code and rtrim(cat_name) like '" + sCategory + "%' and rtrim(def_name) like '" + sType + "%'";
            pSet.Query = sQuery;
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sCatName = pSet.GetString("cat_name").Trim();
                    sQuery = "select def_code,def_name from def_rec_table,def_cat_table where def_rec_table.cat_code = def_cat_table.cat_code and cat_name = '" + sCatName + "' and def_name like '" + sType + "%'";
                    if (pSet1.Execute())
                        if (pSet1.Read())
                        {
                            sContent = "<1000|<5000|^1100;";
                            while (pSet1.Read())
                            {
                                iDCount = 0;
                                sDefCode = pSet1.GetString("def_code").Trim();
                                sDefName = pSet1.GetString("def_name").Trim();

                                if (cmbStatus.Text == "ALL")
                                {
                                    sQuery = string.Format("select rec_acct_no,dt_save,def_status from def_records where def_code='{0}' and to_date(dt_save) between to_date('{1}','MM/dd/yyyy') and to_date('{2}','MM/dd/yyyy')", sDefCode, sFrom, sTo);
                                }
                                else
                                {
                                    sQuery = string.Format("select rec_acct_no,dt_save,def_status from def_records where def_code='{0}' and to_date(dt_save,'MM/dd/yyyy') > (to_date('{1}','MM/DD/YYYY') - 1) and to_date(dt_save,'MM/dd/yyyy') < (to_date('{2}','MM/DD/YYYY') + 1) and def_status='{3}'", sDefCode, sFrom, sTo, sStat1);
                                }

                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                    {
                                        if (sTemp != sCatName)
                                        {
                                            axVSPrinter1.FontBold = true;
                                            axVSPrinter1.Table = "<20000;" + sCatName;
                                            sTemp = sCatName;
                                        }

                                        while (pSet2.Read())
                                        {
                                            iDCount++;
                                            iCount++;
                                        }

                                        sContent += sDefCode + "|";
                                        sContent += sDefName + "|";
                                        sContent += iDCount.ToString() + ";";
                                    }
                                pSet2.Close();
                            }
                            axVSPrinter1.FontBold = false;
                            axVSPrinter1.Table = sContent;
                            axVSPrinter1.Paragraph = "";
                        }
                    pSet1.Close();
                }
            }
            pSet.Close();
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = " <3000;Total Number of Deficient Record(s) :   " + iCount.ToString();
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;

        }

        private void OnListDeficient()
        {
            String sQuery, sRefNo, sDefCode, sDtSave, sDefName, sCatName, sBName = "", sBrgy, sAdInfo, sAdInfo1 = "", sAdInfoCode, sAdInfoVal = "", sAdInfoVal1 = "", sStatus;
            String sColHeader, sContent, sHeader, sStat, sFrom, sTo, sStat1 = "";
            int iProgressTotal = 0;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            OracleResultSet pSet2 = new OracleResultSet();

            sFrom = string.Format("{0}/{1}/{2}", dtpFrom.Value.Month, dtpFrom.Value.Day, dtpFrom.Value.Year);
            sTo = string.Format("{0}/{1}/{2}", dtpTo.Value.Month, dtpTo.Value.Day, dtpTo.Value.Year);

            sColHeader = "^1500|^2000|^1700|^600|^2200|^2200|^1800|^1100|^1600;Ref No.|Business Name|Category|Code|Deficient Name|Additional Info|Value|Status|Transaction Date";
            sContent = "<1500|<2000|<1700|^600|<2200|<2200|<1800|^1100|^1600;";

            sHeader = "^14400;" + AppSettingsManager.GetConfigValue("09");

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprFanfoldUS;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orLandscape; // 0 portrait 1 landscape
            axVSPrinter1.MarginLeft = 700;
            axVSPrinter1.MarginTop = 700;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial Narrow";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^14400;Republic of the Phlippines";
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.Table = "^14400;List of Deficient Records";
            axVSPrinter1.FontBold = false;
            axVSPrinter1.Table = "^14400;Period   from " + sFrom + "  to " + sTo;

            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbColTopBottom;
            axVSPrinter1.FontBold = true;
            axVSPrinter1.Table = sColHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontBold = false;

            if (cmbStatus.Text == "CORRECTED")
                sStat1 = "Y";
            else if (cmbStatus.Text == "UNCORRECTED")
                sStat1 = "N";
            else if (cmbStatus.Text == "ALL")
                sStat1 = "%%";

            String sTmpCatName, sTmpDefName;

            if (cmbCategory.Text == "ALL")
                sTmpCatName = "%%";
            else
                sTmpCatName = cmbCategory.Text;

            if (cmbType.Text == "ALL")
                sTmpDefName = "%%";
            else
                sTmpDefName = cmbType.Text;

            sQuery = string.Format(@"select a.def_code,def_name,cat_name,c.rec_acct_no,c.dt_save,c.def_status
		             from def_rec_table a,def_cat_table b,def_records c
					   where a.cat_code = b.cat_code and trim(def_status) like '{0}'
					   and trim(a.def_name) like '{1}' and a.def_code = c.def_code
					   and a.cat_code = b.cat_code and trim(cat_name) like '{2}' 
					   and to_date(dt_save) between to_date('{3}','MM/dd/yyyy') and to_date('{4}','MM/dd/yyyy')  order by cat_name,a.def_code", sStat1, sTmpDefName, sTmpCatName, sFrom, sTo);

            pSet.Query = sQuery;
            if (pSet.Execute())
                if (pSet.Read())
                {
                    while (pSet.Read())
                    {
                        iProgressTotal += 1;
                        sCatName = pSet.GetString("cat_name").Trim();
                        sDefCode = pSet.GetString("def_code").Trim();
                        sDefName = pSet.GetString("def_name").Trim();

                        sRefNo = pSet.GetString("rec_acct_no").Trim();
                        sDtSave = pSet.GetDateTime("dt_save").ToShortDateString().Trim();
                        sStatus = pSet.GetString("def_status").Trim();

                        sQuery = string.Format("select bns_nm,bns_brgy from businesses where bin='{0}'", sRefNo);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            if (pSet1.Read())
                                sBName = pSet.GetString("bns_nm").Trim();
                        pSet1.Close();

                        sQuery = string.Format("select adinfo_name,adinfo_code from def_adinfo_table where def_code='{0}'", sDefCode);
                        pSet1.Query = sQuery;
                        if (pSet1.Execute())
                            while (pSet1.Read())
                            {
                                sAdInfo = pSet1.GetString("adinfo_name").Trim();
                                sAdInfoCode = pSet1.GetString("adinfo_code").Trim();
                                if (sAdInfo1 == String.Empty)
                                    sAdInfo1 = sAdInfo;
                                else
                                    sAdInfo1 += " \n" + sAdInfo;

                                sQuery = string.Format("select adinfo_value from def_rec_adinfo where def_code='{0}' and adinfo_code = '{1}' and rec_acct_no = '{2}'", sDefCode, sAdInfoCode, sRefNo);
                                pSet2.Query = sQuery;
                                if (pSet2.Execute())
                                    if (pSet2.Read())
                                        sAdInfoVal = pSet.GetString("adinfo_value").Trim();
                                    else
                                        sAdInfoVal = "";
                                pSet2.Close();

                                if (sAdInfoVal1 == String.Empty)
                                    sAdInfoVal1 = sAdInfoVal;
                                else
                                    sAdInfoVal1 += " \n" + sAdInfoVal;
                            }
                        pSet1.Close();


                        if (sStatus == "Y")
                            sStat = "CORRECTED";
                        else
                            sStat = "UNCORRECTED";

                        sContent += sRefNo + "|";
                        sContent += sBName + "|";
                        sContent += sCatName + "|";
                        sContent += sDefCode + "|";
                        sContent += sDefName + "|";
                        sContent += sAdInfo1 + "|";
                        sContent += sAdInfoVal1 + "|";
                        sContent += sStat + "|";
                        sContent += sDtSave + ";";

                        axVSPrinter1.Table = sContent;
                        sContent = "<1500|<2000|<1700|^600|<2200|<2200|<1800|^1100|^1600;";

                        sCatName = "";
                        sBName = "";
                        sDefCode = "";
                        sDefName = "";
                        sRefNo = "";
                        sDtSave = "";
                        sStatus = "";
                        sAdInfo1 = "";
                        sAdInfo = "";
                        sAdInfoVal = "";
                        sAdInfoVal1 = "";
                    }
                }
            pSet.Close();

            //axVSPrinter1.SetTable(sContent);
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Table = "<3000;Total Number of Deficient Record(s) :   " + iProgressTotal.ToString();
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (m_iRadioSum == 1)
                OnSumDeficient();
            else if (m_iRadioSum == 2)
                OnListDeficient();
        }

        private void toolPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintQuality = VSPrinter7Lib.PrintQualitySettings.pqHigh;

            if (MessageBox.Show("Are you sure you want to print?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            axVSPrinter1.PrintDoc(1,1,axVSPrinter1.PageCount);
        }

        private void toolSettingPage_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPageSetup);
        }

        private void toolSettingPrint_Click(object sender, EventArgs e)
        {
            axVSPrinter1.PrintDialog(VSPrinter7Lib.PrintDialogSettings.pdPrinterSetup);
        }
    }
}