using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.ApplicationRequirements
{
    public partial class frmAppBnsNature : Form
    {
        private string m_sCode = string.Empty;

        public string ReqCode
        {
            get { return m_sCode; }
            set { m_sCode = value; }
        }

        public frmAppBnsNature()
        {
            InitializeComponent();
        }

        private void frmAppBnsNature_Load(object sender, EventArgs e)
        {
            txtCode.Text = m_sCode;
            chkViewAll.Checked = false;
            chkSelected.Checked = true;

            PopulateList("Selected");
        }

        private void UpdateList(string sBnsCode, int iRow)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from requirements_chklist_tmp where req_code = '{0}' and bns_code = '{1}'", m_sCode, sBnsCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dgvList[0, iRow].Value = true;
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from requirements_chklist where req_code = '{0}' and bns_code = '{1}'", m_sCode, sBnsCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    dgvList[0, iRow].Value = true;
                }
            }
            pSet.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /*OracleResultSet pCmd = new OracleResultSet();
            string sBnsCode = "";

            pCmd.Query = string.Format("delete from requirements_chklist_tmp where req_code = '{0}'", m_sCode);
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            int iRow = 0;
            for (iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                if ((bool)dgvList[0, iRow].Value)
                {
                    sBnsCode = dgvList[2, iRow].Value.ToString().Trim();

                    pCmd.Query = "insert into requirements_chklist_tmp values (:1,:2)";
                    pCmd.AddParameter(":1", m_sCode);
                    pCmd.AddParameter(":2", sBnsCode);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
            }

            if (iRow > 0)
            {
                MessageBox.Show("Record saved", "Specific Nature of Business", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "" && btnSearch.Text == "Search")
            {
                PopulateList(txtSearch.Text.Trim());
                btnSearch.Text = "Clear";
            }
            else
            {
                txtSearch.Text = "";
                btnSearch.Text = "Search";

                if (chkViewAll.Checked == true)
                    PopulateList("All");
                else
                    PopulateList("Selected");
            }

        }

        private void PopulateList(string sFilter)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBnsCode = "";
            string sBnsDesc = "";
            int iRow = 0;

            dgvList.Columns.Clear();
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("NATURE", "Nature of Business");
            dgvList.Columns.Add("CODE", "Code");
            dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 300;
            dgvList.Columns[2].Visible = false;

            if (sFilter == "Selected")
            {
                pRec.Query = string.Format("select * from REQUIREMENTS_CHKLIST_tmp where req_code = '{0}' order by bns_code ", m_sCode);
                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        dgvList.Rows.Add("");
                        sBnsCode = pRec.GetString("bns_code");
                        sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);

                        dgvList[0, iRow].Value = true;
                        dgvList[1, iRow].Value = sBnsDesc;
                        dgvList[2, iRow].Value = sBnsCode;

                        iRow++;
                    }
                }
                pRec.Close();
            }
            else
            {
                pRec.Query = "select * from bns_table where fees_code = 'B' and length(bns_code) > 2 ";
                if (sFilter != "All")
                    pRec.Query += string.Format(" and bns_desc like '{0}'", StringUtilities.HandleApostrophe(txtSearch.Text.Trim()));
                pRec.Query += " order by bns_desc";

                if (pRec.Execute())
                {
                    while (pRec.Read())
                    {
                        sBnsCode = pRec.GetString("bns_code");
                        sBnsDesc = pRec.GetString("bns_desc");

                        dgvList.Rows.Add("");
                        dgvList[0, iRow].Value = false;
                        dgvList[1, iRow].Value = sBnsDesc;
                        dgvList[2, iRow].Value = sBnsCode;

                        UpdateList(sBnsCode, iRow);
                        iRow++;
                    }
                }
                pRec.Close();
            }
        }

        private void chkViewAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkViewAll.CheckState.ToString() == "Checked")
            {
                chkSelected.Checked = false;
                PopulateList("All");
            }
        }

        private void chkSelected_CheckStateChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (chkSelected.CheckState.ToString() == "Checked")
            {
                chkViewAll.Checked = false;

                PopulateList("Selected");
                
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            string sBnsCode = "";

            if (e.ColumnIndex == 0)
            {
                sBnsCode = dgvList[2, e.RowIndex].Value.ToString().Trim();

                pCmd.Query = string.Format("delete from requirements_chklist_tmp where req_code = '{0}' and bns_code = '{1}'", m_sCode, sBnsCode);
                if (pCmd.ExecuteNonQuery() == 0)
                { }
                 
                //dgvList.EndEdit();

                if ((bool)dgvList[0, e.RowIndex].Value == true)
                    dgvList[0, e.RowIndex].Value = false;
                else
                    dgvList[0, e.RowIndex].Value = true;

                if ((bool)dgvList[0, e.RowIndex].Value == true)
                {
                    pCmd.Query = "insert into requirements_chklist_tmp values (:1,:2)";
                    pCmd.AddParameter(":1", m_sCode);
                    pCmd.AddParameter(":2", sBnsCode);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
            }
        }
        
    }
}