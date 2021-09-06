using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Modules.LiquidationReports;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmNigReport : Form
    {
        public frmNigReport()
        {
            InitializeComponent();
        }

        private void frmNigReport_Load(object sender, EventArgs e)
        {
            LoadDivision();
        }

        private void LoadDivision()
        {
            ArrayList arrList = new ArrayList();
            arrList.Add("");
            arrList.Add("ENGINEERING");
            arrList.Add("BPLO");
            arrList.Add("ZONING");
            arrList.Add("SANITARY");
            arrList.Add("BFP");
            arrList.Add("BENRO");
            arrList.Add("CENRO");
            arrList.Add("CHO");
            arrList.Add("PESO"); //AFM 20191212 MAO-19-11583
            arrList.Add("MAPUMA"); //AFM 20191212 MAO-19-11716

            String sValue = AppSettingsManager.GetUserDiv(AppSettingsManager.SystemUser.UserCode);

            if (sValue == "BPLO" || AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
            {
                for (int i = 0; i < arrList.Count; i++)
                    cmbDivision.Items.Add(arrList[i].ToString());
            }
            else
            {
                if (arrList.Contains(sValue)) //to prevent other office with access rights but not related to this module
                    cmbDivision.Items.Add(sValue);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            String sQuery = @"select NL.BIN,NL.DIVISION_CODE,NT.VIOLATION_DESC,NL.DATE_INSPECTED,NL.USR_CODE from nigvio_list NL 
inner join nigvio_tbl NT on NT.DIVISION_CODE = NL.DIVISION_CODE and NT.VIOLATION_CODE = NL.VIOLATION_CODE where ";

            if (cmbDivision.Text != "")
            {
                sQuery += "NL.division_code = '" + cmbDivision.Text.Trim() + "'";
                sQuery += "and";
            }

            //sQuery += " NL.DATE_INSPECTED between '" + dtpFrom.Value.ToString("MM/dd/yyyy") + "' and '" + dtpTo.Value.ToString("MM/dd/yyyy") + "'";
            sQuery += " TO_DATE(NL.DATE_INSPECTED, 'MM/dd/yyyy') between '" + dtpFrom.Value.ToString("dd-MMM-yyyy") + "' and '" + dtpTo.Value.ToString("dd-MMM-yyyy") + "'"; //AFM 20200407 applied correct date format

            if (rdoBin.Checked)
                sQuery += " order by NL.BIN asc";
            else
                sQuery += " order by NL.DIVISION_CODE asc";

            frmLiqReports frmLiqReports = new frmLiqReports();
            frmLiqReports.Query = sQuery;
            frmLiqReports.DateFrom = dtpFrom.Value;
            frmLiqReports.DateTo = dtpTo.Value;
            frmLiqReports.ReportTitle = "Negative List";
            frmLiqReports.ReportSwitch = "Negative List";
            frmLiqReports.ShowDialog();
        }

        private void cmbDivision_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}