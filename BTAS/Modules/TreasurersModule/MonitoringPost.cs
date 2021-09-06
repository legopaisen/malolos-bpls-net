using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.AppSettings;
using Amellar.Common.LogIn;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using System.Data;
using System.Windows.Forms;

namespace Amellar.BPLS.TreasurersModule
{
    class MonitoringPost:Monitoring //JARS 20170922
    {
        DataTable dataTable = new DataTable();
        string m_strMemo = string.Empty;

        public MonitoringPost(frmBTaxMonitoring Form)
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

            //dataTable.Columns.Add("OR NUMBER", typeof(String));
            //dataTable.Columns.Add("BIN", typeof(String));
            dataTable.Columns.Add("BIN", typeof(String));
            dataTable.Columns.Add("OR NUMBER", typeof(String));
            dataTable.Columns.Add("BUSINESS STATUS", typeof(String));
            dataTable.Columns.Add("TAX YEAR", typeof(String));
            dataTable.Columns.Add("QUARTER PAID", typeof(String));
            dataTable.Columns.Add("OR DATE", typeof(String));
            dataTable.Columns.Add("TELLER", typeof(String));
            dataTable.Columns.Add("USER", typeof(String));
            dataTable.Columns.Add("MEMO", typeof(String));
            //dataTable.Columns.Add("DATE/TIME", typeof(String));
            //dataTable.Columns.Add("TAX YEAR", typeof(String));

            int iRow = 0;
            string sBin = "";
            string sOrNo = "";
            string sBnsStat = "";
            string sTaxYear = "";
            string sQtrPaid = "";
            string sORDate = "";
            string sTeller = "";
            string sBnsUser = "";
            string sMemo = "";

            pRec.Query = "select * from POS_TEMP order by or_date, or_no asc";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBin = pRec.GetString("bin");
                    sOrNo = pRec.GetString("or_no");
                    sBnsStat = pRec.GetString("bns_stat");
                    sTaxYear = pRec.GetString("tax_year");
                    sQtrPaid = pRec.GetString("qtr_paid");
                    sBnsUser = pRec.GetString("bns_user");
                    sTeller = pRec.GetString("teller");
                    sORDate = pRec.GetDateTime("or_date").ToString("MM/dd/yyyy");
                    sMemo = pRec.GetString("memo");

                    dataTable.Rows.Add(sBin,sOrNo,sBnsStat,sTaxYear,sQtrPaid,sORDate,sTeller,sBnsUser, sMemo);
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
            string sMemo = "";
            int iRow = 0;
            DateTime dtDate = AppSettingsManager.GetCurrentDate();
            string sDateToday = "";

            if (MonitoringFrm.txtBnsName.Text.Trim() != "")
            {
                #region comments
                //iRow = MonitoringFrm.dgvList.SelectedCells[0].RowIndex;
                //if (MonitoringFrm.dgvList[4, iRow].Value != null)
                //    sUserCode = MonitoringFrm.dgvList[4, iRow].Value.ToString().Trim();
                //if (MonitoringFrm.dgvList[5, iRow].Value != null)
                //    sAction = MonitoringFrm.dgvList[5, iRow].Value.ToString().Trim();
                //if (MonitoringFrm.dgvList[8, iRow] != null)
                //    sTaxYear = MonitoringFrm.dgvList[8, iRow].Value.ToString().Trim();

                //if (MessageBox.Show("Are you sure you want to approve " + MonitoringFrm.bin1.GetBin() + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)\
                #endregion
                if (MessageBox.Show("Are you sure you want to approve " + MonitoringFrm.txtOrNo.Text + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                    sMemo = MonitoringFrm.txtMemo.Text + " Approved Date: " + sDate;
                    MonitoringFrm.txtMemo.Text = sMemo;


                    pSet.Query = "update pos_temp set memo = '" + sMemo + "' where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "insert into pay_hist select * from pos_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "insert into or_table select * from pos_or_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "delete from pos_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "delete from pos_or_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }


                    MessageBox.Show("Post Payment approved.", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (AuditTrail.InsertTrail("AUPA", "treasurers_module", MonitoringFrm.bin1.GetBin()) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    UpdateList();
                }
            }
            else
            {
                MessageBox.Show("Select OR to be Approved.", "Post Payment Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        public override void Refresh()
        {
            // RMC 20120109 added refresh button in gross monitoring
            UpdateList();
        }

        public override void Return()
        {
            OracleResultSet pSet = new OracleResultSet();

            if (MonitoringFrm.txtBnsName.Text.Trim() != "")
            {
                if (MessageBox.Show("Are you sure you want to reject OR Number: " + MonitoringFrm.txtOrNo.Text + "?", "BPLS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = "delete from pos_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    pSet.Query = "delete from pos_or_temp where or_no = '" + MonitoringFrm.txtOrNo.Text + "'";
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Post Payment Rejected.", "Post Payment Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateList();
                }
            }
            else
            {
                MessageBox.Show("Select OR to be Rejected.", "Post Payment Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
