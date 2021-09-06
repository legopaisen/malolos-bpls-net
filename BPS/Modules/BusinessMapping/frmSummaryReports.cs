using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmSummaryReport : Form
    {
        private string m_sApplType = string.Empty;
        private string m_sQuery = string.Empty;
        
        
        public frmSummaryReport()
        {
            InitializeComponent();
        }

        private void frmSummaryReport_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            //chkAll.Checked = true;

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
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[2].Width = 400;
            dgvList.Columns[0].Visible = false;

            pRec.Query = "select * from btm_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";

            if (chkAll.Checked == true || m_sApplType == "")
            {
                pRec.Query += " and bin in ";
                pRec.Query += " (select bin from btm_update)";
                
            }
            else
            {
                if (m_sApplType != "")
                {
                    pRec.Query += " and bin in ";
                    pRec.Query += " (select bin from btm_update where (" + m_sApplType + "))";
                }
                
            }
            
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = false;
                    dgvList[1, iRow].Value = pRec.GetString("bin");
                    //dgvList[2, iRow].Value = pRec.GetString("bns_nm");
                    dgvList[2, iRow].Value = AppSettingsManager.GetBnsName(pRec.GetString("bin"));
                    iRow++;
                }
            }
            pRec.Close();

            lblCnt.Text = string.Format("{0:###}", iRow);
        }

        private void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkAll.CheckState.ToString() == "Checked")
            {
                chkTaxYear.Checked = true;
                chkPermit.Checked = true;
                chkBussName.Checked = true;
                chkLoc.Checked = true;
                chkBussPlace.Checked = true;
                chkBussType.Checked = true;
                chkOtherLine.Checked = true;
                chkOwner.Checked = true;
                chkArea.Checked = true;
                chkStoreys.Checked = true;

                UpdateList();
            }
        }

        private void ValidateCheckAll()
        {
            bool bCheckAll = true;
            int iCnt = 0;

            m_sApplType = "";

            if (chkTaxYear.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CTYR'";
                iCnt++;
            }
            if (chkPermit.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CPMT'";
                iCnt++;
            }
            if (chkBussName.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CBNS'";
                iCnt++;
            }
            if (chkLoc.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'TLOC'";
                iCnt++;
            }
            if (chkBussPlace.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CBOW'";
                iCnt++;
            }
            if (chkBussType.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CTYP'";
                iCnt++;
            }
            if (chkOtherLine.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'ADDL'";
                iCnt++;
            }
            if (chkOwner.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'TOWN'";
                iCnt++;
            }
            if (chkArea.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'AREA'";
                iCnt++;
            }
            if (chkStoreys.Checked == false)
                bCheckAll = false;
            else
            {
                if (iCnt > 0)
                    m_sApplType += " or ";
                m_sApplType += "appl_type = 'CSTR'";
                iCnt++;
            }

            if (bCheckAll)
                chkAll.Checked = true;
            else
                chkAll.Checked = false;

            UpdateList();
        }

        private void chkTaxYear_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkTaxYear.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkPermit_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkPermit.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkBussName_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkBussName.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkLoc_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkLoc.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkBussPlace_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkBussName.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkBussType_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkBussType.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkOtherLine_CheckStateChanged(object sender, EventArgs e)
        {
            //if(chkOtherLine.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/

        }

        private void chkOwner_CheckStateChanged(object sender, EventArgs e)
        {
            //if(chkOwner.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkArea_CheckStateChanged(object sender, EventArgs e)
        {
            //if(chkArea.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/
        }

        private void chkStoreys_CheckStateChanged(object sender, EventArgs e)
        {
            //if(chkStoreys.CheckState.ToString() == "Checked")
            {
                ValidateCheckAll();
            }
            /*else
                chkAll.Checked = false;*/

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintList_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("This will print all businesses with changes located at "+cmbBrgy.Text.ToString()+"","Report",MessageBoxButtons.OK, MessageBoxIcon.Information);

            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = "select count(*) from btm_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";
            pRec.Query += " and bin in ";
            pRec.Query += " (select bin from btm_update)";
            int.TryParse(pRec.ExecuteScalar(), out iCnt);
            
            if(iCnt > 0)
            {
                pRec.Query = "select * from btm_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";
                pRec.Query += " and bin in ";
                pRec.Query += " (select bin from btm_update)";
                pRec.Query += " order by bns_nm";

                m_sQuery = pRec.Query.ToString();

                frmReportTest PrintClass = new frmReportTest();
                PrintClass.ReportName = "Summary Official";
                PrintClass.Barangay = cmbBrgy.Text.ToString();
                PrintClass.Query = m_sQuery;
                PrintClass.ShowDialog();
            }
            else
            {
                MessageBox.Show("No record to print", "Report", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnPrintDetails_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            int iCnt = 0;

            pRec.Query = "select count(*) from btm_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";
            pRec.Query += " and bin in ";
            pRec.Query += " (select bin from btm_update)";
            int.TryParse(pRec.ExecuteScalar(), out iCnt);

            if (iCnt > 0)
            {
                pRec.Query = "select * from btm_businesses where bns_brgy = '" + cmbBrgy.Text.ToString() + "'";
                pRec.Query += " and bin in ";
                pRec.Query += " (select bin from btm_update)";
                pRec.Query += " order by bns_nm";

                m_sQuery = pRec.Query.ToString();

                frmReportTest PrintClass = new frmReportTest();
                PrintClass.ReportName = "Summary Official Details";
                PrintClass.Barangay = cmbBrgy.Text.ToString();
                PrintClass.Query = m_sQuery;
                PrintClass.ShowDialog();
            }
            else
            {
                MessageBox.Show("No record to print", "Report", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        

        
        
    }
}