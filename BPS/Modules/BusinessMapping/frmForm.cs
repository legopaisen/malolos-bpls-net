using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmForm : Form
    {
        private bool m_bCheckAll = false;

        public frmForm()
        {
            InitializeComponent();
        }

        private void frmForm_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            chkData.Checked = true;

            cmbBrgy.Items.Clear();
            pRec.Query = "select * from brgy order by brgy_nm";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                }
            }
            pRec.Close();

            cmbBrgy.SelectedIndex = 0;

            chkBussName.Checked = true;

            m_bCheckAll = false;

            
        }

        private void cmbBrgy_SelectedValueChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pSet = new OracleResultSet();

            int iRow = 0;
            dgvList.Columns.Clear();
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("BNSSTREET", "Street");
            dgvList.Columns.Add("BNSCOUNT", "Business Count");
            dgvList.Columns.Add("STATUS", "Printed");
            dgvList.Columns[0].Width = 40;
            dgvList.Columns[1].Width = 150;
            dgvList.Columns[2].Width = 120;
            dgvList.Columns[3].Width = 50;
            dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;

            pRec.Query = "select distinct bns_street, count(*) from businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "' ";
            pRec.Query += " group by bns_street order by bns_street";
            if (pRec.Execute())
            {
                dgvList.Rows.Add("");
                dgvList[0, iRow].Value = false;
                dgvList[1, iRow].Value = "ALL";
                dgvList[2, iRow].Value = "";
                iRow++;

                while (pRec.Read())
                {
                    dgvList.Rows.Add("");    
                    dgvList[0, iRow].Value = false;
                    dgvList[1, iRow].Value = pRec.GetString(0);
                    dgvList[2, iRow].Value = string.Format("{0:###}", pRec.GetInt(1));

                    int iCnt = 0;
                    pSet.Query = "select count(*) from btm_print_status where bns_brgy = '" + cmbBrgy.Text.ToString() + "' ";
                    pSet.Query += " and bns_street = '" + StringUtilities.HandleApostrophe(pRec.GetString(0)) + "'";
                    pSet.Query += " and print_status = 'Y'";
                    int.TryParse(pSet.ExecuteScalar(), out iCnt);

                    if (iCnt > 0)
                        dgvList[3, iRow].Value = "Y";
                    else
                        dgvList[3, iRow].Value = "";
                    iRow++;
                }
            }
            pRec.Close();


            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int iStCnt = 0;

            
            if (!chkBlank.Checked)
            {
                if (lblCount.Text == "0" || lblCount.Text.Trim() == "")
                {
                    MessageBox.Show("Select street to print", "Accomplishment Form", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            string sArea = "";
            string sBnsStreet = "(";
            int iCnt = 0;

            for (int i = 0; i < dgvList.Rows.Count; i++)
            {
                if ((bool)dgvList[0, i].Value == true)
                {
                    if (iCnt > 0)
                    {
                        sBnsStreet += " or ";
                        sArea += ", ";
                    }

                    iCnt++;
                    sBnsStreet += "bns_street = '" + StringUtilities.HandleApostrophe(dgvList[1, i].Value.ToString().Trim()) + "'";
                    sArea += dgvList[1, i].Value.ToString().Trim();
                }
            }
            sBnsStreet += ")";

            
            frmReportTest PrintClass = new frmReportTest();

            if (chkBlank.Checked)
            {
                PrintClass.ReportName = "Accomplishment";
                PrintClass.Barangay = "";
                PrintClass.Street = "";
                PrintClass.Area = "";
                PrintClass.BlankForm = true;
            }
            else
            {
                PrintClass.ReportName = "Accomplishment";
                PrintClass.Barangay = cmbBrgy.Text.ToString().Trim();
                PrintClass.Street = sBnsStreet;
                if (m_bCheckAll)
                    PrintClass.Area = "ALL";
                else
                    PrintClass.Area = sArea;
                if (chkBussName.Checked)
                    PrintClass.OrderBy = "bns_nm";
                else if (chkPermitNo.Checked)
                    PrintClass.OrderBy = "permit_no";
                PrintClass.BlankForm = false;
            }
            PrintClass.ShowDialog();

            cmbBrgy_SelectedValueChanged(sender, e);
        }

        private void chkBussName_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBussName.CheckState.ToString() == "Checked")
            {
                chkPermitNo.Checked = false;
            }

        }

        private void chkPermitNo_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkPermitNo.CheckState.ToString() == "Checked")
            {
                chkBussName.Checked = false;
            }

        }

        private void chkData_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkData.CheckState.ToString() == "Checked")
            {
                chkBlank.Checked = false;
                cmbBrgy.Enabled = true;
                chkPermitNo.Enabled = true;
                chkBussName.Enabled = true;
            }
        }

        private void chkBlank_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBlank.CheckState.ToString() == "Checked")
            {
                chkData.Checked = false;
                cmbBrgy.Enabled = false;
                chkPermitNo.Enabled = false;
                chkBussName.Enabled = false;
                dgvList.Columns.Clear();
                lblCount.Text = "";
            }
        }

        private void SetCheckAll(bool bCheck)
        {
            for (int i = 0; i < dgvList.Rows.Count; i++)
            {
                dgvList[0, i].Value = bCheck;
            }

            m_bCheckAll = bCheck;
        }

        private void ViewCount()
        {
            int iBussCnt = 0;
            int iTotCnt = 0;

            for (int i = 0; i < dgvList.Rows.Count; i++)
            {
                if ((bool)dgvList[0, i].Value)
                {
                    if (dgvList[2, i].Value != null)
                    {
                        int.TryParse(dgvList[2, i].Value.ToString(), out iBussCnt);
                        iTotCnt += iBussCnt;
                    }
                }
                
            }
            

            lblCount.Text = string.Format("{0:#,###}", iTotCnt);
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool bTmp = false;

            if ((bool)dgvList[e.ColumnIndex, e.RowIndex].Value)
                dgvList[e.ColumnIndex, e.RowIndex].Value = false;
            else
                dgvList[e.ColumnIndex, e.RowIndex].Value = true;
            
            bTmp = (bool)dgvList[e.ColumnIndex, e.RowIndex].Value;

            if (e.RowIndex == 0)
            {
                if ((bool)dgvList[0, 0].Value)
                {
                    SetCheckAll(true);
                }
                else
                    SetCheckAll(false);

            }
            else
                dgvList[e.ColumnIndex, 0].Value = false;

            ViewCount();

        }

        
    }
}