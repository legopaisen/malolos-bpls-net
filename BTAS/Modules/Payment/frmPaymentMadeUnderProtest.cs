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

namespace Amellar.Modules.Payment
{
    public partial class frmPaymentMadeUnderProtest : Form
    {
        public frmPaymentMadeUnderProtest()
        {
            InitializeComponent();
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

        private void frmPaymentMadeUnderProtest_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = AppSettingsManager.GetSystemDate();
            dtpTo.Value = AppSettingsManager.GetSystemDate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String sQuery, sCurrentUser, sFromDate, sToDate;
            String sBin, sTaxYear, sOrNo, sBlank, sColumnHeader, sColumnContent, sBFeesAmount, sFeesAmount, sSurchPenAmount, sTotalAmount;

            sCurrentUser = AppSettingsManager.GetUserName(AppSettingsManager.SystemUser.UserCode);

            sFromDate = string.Format("{1}/{2}/{0}", dtpFrom.Value.Year, dtpFrom.Value.Month, dtpFrom.Value.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);
            sToDate = string.Format("{1}/{2}/{0}", dtpTo.Value.Year, dtpTo.Value.Month, dtpTo.Value.Day);//  {3}:{4}:{5}, m_dtSaveTime.Hour, m_dtSaveTime.Minute, m_dtSaveTime.Second);

            sColumnHeader = "^2800|^1000|^1400|^1400|^1400|^1400|^1500;B I N|Tax Year|O.R.|Tax|Fees|Surch & Int|Total";

            axVSPrinter1.PaperSize = VSPrinter7Lib.PaperSizeSettings.pprLetter;
            axVSPrinter1.Orientation = VSPrinter7Lib.OrientationSettings.orPortrait;
            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paStartDoc;
            axVSPrinter1.FontName = "Arial";
            axVSPrinter1.MarginTop = 770;
            axVSPrinter1.MarginLeft = 770;
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Table = "^10600;Republic of the Philippines";
            axVSPrinter1.Table = "^10600;" + AppSettingsManager.GetConfigObject("09").Trim() + "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontSize = 12;
            axVSPrinter1.Table = "^10600;PAYMENTS UNDER PROTEST";
            axVSPrinter1.FontSize = 10;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbTopBottom;
            axVSPrinter1.Table = sColumnHeader;
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.TableBorder = VSPrinter7Lib.TableBorderSettings.tbNone;

            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pSet1 = new OracleResultSet();
            sQuery = string.Format("select * from protested_payment where trunc(dt_save) >= to_date('{0}','MM/dd/yyyy') and trunc(dt_save) <= to_date('{1}','MM/dd/yyyy') order by bin, tax_year, or_no", sFromDate, sToDate);
            pSet.Query = sQuery;
            if (pSet.Execute())
                while (pSet.Read())
                {
                    sColumnContent = "^2800|^1000|^1400|>1400|>1400|>1400|>1500;";

                    sBin = "";
                    sTaxYear = "";
                    sOrNo = "";
                    sBFeesAmount = "";
                    sFeesAmount = "";
                    sSurchPenAmount = "";
                    sTotalAmount = "";

                    sBin = pSet.GetString("bin").Trim();
                    sTaxYear = pSet.GetString("tax_year").Trim();
                    sOrNo = pSet.GetString("or_no").Trim();

                    sQuery = string.Format("select sum(fees_due) as sTmpAmount from or_table where or_no = '{0}' and tax_year = '{1}' and fees_code = 'B'", sOrNo, sTaxYear);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sBFeesAmount = pSet1.GetDouble("sTmpAmount").ToString("#,##0.00");
                    pSet1.Close();

                    sQuery = string.Format("select sum(fees_due) as sTmpAmount from or_table where or_no = '{0}' and tax_year = '{1}' and fees_code <> 'B'", sOrNo, sTaxYear);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sFeesAmount = pSet1.GetDouble("sTmpAmount").ToString("#,##0.00");
                    pSet1.Close();

                    sQuery = string.Format("select sum(fees_pen+fees_surch) as sTmpAmount from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, sTaxYear);
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sSurchPenAmount = pSet1.GetDouble("sTmpAmount").ToString("#,##0.00");
                    pSet1.Close();

                    sQuery = string.Format("select sum(fees_amtdue) as sTmpAmount from or_table where or_no = '{0}' and tax_year = '{1}'", sOrNo, sTaxYear);	// JGR 09192005 Oracle Adjustment
                    pSet1.Query = sQuery;
                    if (pSet1.Execute())
                        if (pSet1.Read())
                            sTotalAmount = pSet1.GetDouble("sTmpAmount").ToString("#,##0.00");
                    pSet1.Close();

                    sColumnContent += sBin + "|" + sTaxYear + "|" + sOrNo + "|" + sBFeesAmount + "|" + sFeesAmount + "|" + sSurchPenAmount + "|" + sTotalAmount + "";
                    axVSPrinter1.Table = sColumnContent;
                }

            pSet.Close();
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.Paragraph = "";
            axVSPrinter1.FontSize = 8;
            axVSPrinter1.Table = "<1200|<5000;Printed by :| " + sCurrentUser;
            axVSPrinter1.Table = "<1200|<5000;Date printed :| " + AppSettingsManager.GetSystemDate();

            axVSPrinter1.Action = VSPrinter7Lib.ActionSettings.paEndDoc;
        }
    }
}