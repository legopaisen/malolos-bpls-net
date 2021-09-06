
// RMC 20120109 added refresh button in gross monitoring
// RMC 20120109 corrected field type in display of Gross monitoring
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
using System.Data;


namespace Amellar.BPLS.TreasurersModule
{
    public class MonitoringGross:Monitoring
    {
        DataTable dataTable = new DataTable();
        
        public MonitoringGross(frmBTaxMonitoring Form)
            : base(Form)
        { }

        public override void FormLoad()
        {
            
            
        }

        public override void UpdateList()
        {
            MonitoringFrm.btnReturn.Visible = false;

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            OracleResultSet pRec = new OracleResultSet();

            dataTable = new DataTable();
            
            dataTable.Columns.Add("BIN", typeof(String));
            dataTable.Columns.Add("PARAMETER", typeof(String));
            dataTable.Columns.Add("PREV. GROSS", typeof(String));
            dataTable.Columns.Add("CURR. GROSS", typeof(String));
            dataTable.Columns.Add("USER CODE", typeof(String));
            dataTable.Columns.Add("STATUS", typeof(String));
            dataTable.Columns.Add("PRINTED", typeof(String));
            dataTable.Columns.Add("DATE/TIME", typeof(String));
            dataTable.Columns.Add("TAX YEAR", typeof(String));

            int iRow = 0;
            string sBIN = "";
            string sParameter = "";
            string sPrevGross = "";
            string sCurrGross = "";
            string sUserCode = "";
            string sAction = "";
            string sDate = "";
            string sTaxYear = "";
            string sPrinted = "";

            pRec.Query = "select * from GROSS_MONITORING where action = '0' order by BIN";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");
                    sParameter = pRec.GetString("parameter");
                    //sPrevGross = string.Format("{0:#,###.00}", pRec.GetFloat("prev_gross"));
                    //sCurrGross = string.Format("{0:#,###.00}", pRec.GetFloat("curr_gross"));
                    sPrevGross = string.Format("{0:#,###.00}", pRec.GetDouble("prev_gross"));   // RMC 20120109 corrected field type in display of Gross monitoring
                    sCurrGross = string.Format("{0:#,###.00}", pRec.GetDouble("curr_gross"));   // RMC 20120109 corrected field type in display of Gross monitoring
                    sUserCode = pRec.GetString("user_code");
                    sAction = pRec.GetString("action");
                    if (sAction == "0")
                        sAction = "HOLD";
                    sDate = string.Format("{0:MM/dd/yyyy}", pRec.GetDateTime("dt_save"));
                    sTaxYear = pRec.GetString("tax_year");
                    sPrinted = pRec.GetString("is_printed");

                    dataTable.Rows.Add(sBIN, sParameter, sPrevGross, sCurrGross, sUserCode, sAction, sPrinted, sDate, sTaxYear);
                    
                }
            }
            pRec.Close();

            MonitoringFrm.dgvList.DataSource = dataTable;
            MonitoringFrm.dgvList.Columns[0].Width = 150;
            MonitoringFrm.dgvList.Columns[1].Width = 100;
            MonitoringFrm.dgvList.Columns[2].Width = 120;
            MonitoringFrm.dgvList.Columns[3].Width = 120;
            MonitoringFrm.dgvList.Columns[4].Width = 100;
            MonitoringFrm.dgvList.Columns[5].Width = 50;
            MonitoringFrm.dgvList.Columns[6].Width = 50;
            MonitoringFrm.dgvList.Columns[7].Width = 100;
            MonitoringFrm.dgvList.Columns[8].Width = 100;
            MonitoringFrm.dgvList.Refresh();

            if (MonitoringFrm.dgvList.Rows.Count > 0)
                MonitoringFrm.LoadValues(0);
            
            
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

                if (MessageBox.Show("Are you sure you want to approve " + MonitoringFrm.bin1.GetBin() + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                    pSet.Query = "update gross_monitoring set action = '1', dt_approved = to_date('"+sDate+"','MM/dd/yyyy') ";
                    pSet.Query += " where bin = '" + MonitoringFrm.bin1.GetBin() + "' and action = '0' ";
                    pSet.Query += " and user_code = '" + sUserCode + "' and tax_year = '" + sTaxYear + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("BIN approved.\n You may print SOA.", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (AuditTrail.InsertTrail("AUTM-AG", "treasurers_module", MonitoringFrm.bin1.GetBin()) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    UpdateList();
                }
            }
        }
        public override void Refresh()
        {
            // RMC 20120109 added refresh button in gross monitoring
            UpdateList();
        }
        
    }
}
