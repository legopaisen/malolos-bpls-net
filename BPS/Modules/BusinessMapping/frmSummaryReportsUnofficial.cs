


// RMC 20120417 Modified Final notice format
// GDE 20120416 test
// RMC 20120222 added printing of notice for un-official business in business mapping

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Modules.InspectorsDetails;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmSummaryReportsUnofficial : Form
    {
        private string m_sApplType = string.Empty;
        private string m_sQuery = string.Empty;
        private string m_strSource = string.Empty;
        private string m_sBnsNm = string.Empty; // GDE 20120416 test

        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public frmSummaryReportsUnofficial()
        {
            InitializeComponent();
        }

        private void frmSummaryReport_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (m_strSource == "Unofficial")
            {
                this.Text = "Business Mapping Un-Official Business Report";
                
            }
            else
            {
                this.Text = "Businesses not Mapped";
                
            }

            cmbBrgy.Items.Clear();
            pRec.Query = "select * from brgy order by brgy_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                }
            }
            pRec.Close();

            cmbBrgy.SelectedIndex = 0;
            UpdateList();
        }

        private void cmbBrgy_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            UpdateList();
        }


        private void UpdateList()
        {
            OracleResultSet pRec = new OracleResultSet();

            int iRow = 0;
            dgvList.Columns.Clear();
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("BIN", "BIN");
            dgvList.Columns.Add("BNS_NAME", "Business Name");
            dgvList.Columns[0].Width = 20;
            dgvList.Columns[1].Width = 200;
            dgvList.Columns[2].Width = 300;
            
            if (m_strSource == "Unofficial")
            {
                dgvList.Columns[0].Visible = false; // enable this when notice is ready
                pRec.Query = "select tbin, bns_nm from btm_temp_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";
                pRec.Query += " and trim(bin) is null";   // RMC 20120417 Modified Final notice format
                pRec.Query += " order by tbin";
            }
            else
            {
                dgvList.Columns[0].Visible = false;
                pRec.Query = "select bin, bns_nm from businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "' and bin not in";
                //pRec.Query += " (select bin from btm_businesses) "; // GDE 20120716 as per jester
                pRec.Query += " (select bin from btm_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "' union select bin from btm_temp_businesses where bns_brgy = '" + cmbBrgy.Text.Trim() + "' and bin <> ' ')";
                pRec.Query += " order by bin";
            }
            
            m_sQuery = pRec.Query.ToString();
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = false;
                    dgvList[1, iRow].Value = pRec.GetString(0);
                    dgvList[2, iRow].Value = pRec.GetString(1);
                    iRow++;
                }
            }
            pRec.Close();

            lblCnt.Text = string.Format("{0:###}", iRow);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintList_Click(object sender, EventArgs e)
        {
            if (m_strSource == "Unofficial")
                MessageBox.Show("This will print all unofficial businesses located at " + cmbBrgy.Text.ToString() + "", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("This will print all businesses not mapped located at " + cmbBrgy.Text.ToString() + "", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (dgvList.Rows.Count > 0)
            {
                frmReportTest PrintClass = new frmReportTest();
                PrintClass.ReportName = m_strSource;
                PrintClass.Barangay = cmbBrgy.Text.ToString();
                PrintClass.Query = m_sQuery;
                if (m_strSource == "Unofficial")
                    PrintClass.ReportSwitch = "Unencoded Businesses (with Permit)";
                else
                    PrintClass.ReportSwitch = "";
                PrintClass.ShowDialog();
            }
            else
            {
                MessageBox.Show("No record to print", "Report", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            // RMC 20120222 added printing of notice for un-official business in business mapping

            if (MessageBox.Show("Print notice for Barangay: "+ cmbBrgy.Text + "", "Sending of Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmOfficialTagging TaggingFrm = new frmOfficialTagging();
                TaggingFrm.Source = "Business Mapping";
                //TaggingFrm.Watcher = "ALL";
                TaggingFrm.Watcher = cmbBrgy.Text;  // RMC 20120417 Modified Final notice format
                TaggingFrm.BnsNm = m_sBnsNm;
                TaggingFrm.ShowDialog();
            }
        }

        private void dgvList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // GDE 20120416 test
            if (e.RowIndex != -1)
            {
                try
                {
                    m_sBnsNm = dgvList.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
                catch
                {
                }


            }
        }

    }
}