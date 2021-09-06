using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.BusinessType;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmAddlBusiness : Form
    {
        private string m_sBIN = "";
        private string m_sTaxYear = "";
        private int m_iRow = -1;
        private bool m_bWithChange = false;
        DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
        DataGridViewComboBoxColumn comboMainBns = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn comboStatus = new DataGridViewComboBoxColumn();

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public bool WithChange
        {
            get { return m_bWithChange; }
            set { m_bWithChange = value; }
        }

        public frmAddlBusiness()
        {
            InitializeComponent();
        }

        private void frmAddlBusiness_Load(object sender, EventArgs e)
        {
            dgvApplyOtherLine.Columns.Clear();

            dgvApplyOtherLine.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvApplyOtherLine.Columns.Add("SC", "Sub-Category");
            dgvApplyOtherLine.Columns.Add("CAP", "Capital");
            dgvApplyOtherLine.Columns.Add("PREVGR", "Prev. Gross");   
            dgvApplyOtherLine.Columns.Add("GR", "Gross");
                        
            LoadComboValues();
            LoadComboCell("", 0);
            //dgvApplyOtherLine.Rows[0].Cells[1] = comboCell;
            
            dgvApplyOtherLine.RowHeadersVisible = false;
            dgvApplyOtherLine.Columns[0].Width = 175;
            dgvApplyOtherLine.Columns[1].Width = 175;
            dgvApplyOtherLine.Columns[2].Width = 80;
            dgvApplyOtherLine.Columns[3].Width = 80;
            dgvApplyOtherLine.Columns[4].Width = 80;
            dgvApplyOtherLine.Columns[5].Width = 60;
            dgvApplyOtherLine.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvApplyOtherLine.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvApplyOtherLine.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvApplyOtherLine.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            LoadOtherLine();
            LoadApplyOtherLine();

            m_bWithChange = false;
        }

        private void LoadComboValues()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sRevYear = AppSettingsManager.GetConfigValue("07");

            comboMainBns.HeaderCell.Value = "Main Businesses";
            dgvApplyOtherLine.Rows.Add();
            dgvApplyOtherLine.Columns.Insert(0, comboMainBns);

            comboMainBns.Items.Clear();

            pSet.Query = string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", sRevYear);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    comboMainBns.Items.AddRange(pSet.GetString("bns_desc"));
                }
            }
            pSet.Close();

            comboStatus.HeaderCell.Value = "Status";
            dgvApplyOtherLine.Columns.Insert(5, comboStatus); 
            comboStatus.Items.Clear();
            comboStatus.Items.AddRange("NEW", "REN");
        }

        private void LoadOtherLine()
        {
            OracleResultSet pRec = new OracleResultSet();
            dgvOtherLine.Columns.Clear();
            dgvOtherLine.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvOtherLine.Columns.Add("MAIN", "Main Businesses");
            dgvOtherLine.Columns.Add("SC", "Sub-Category");
            dgvOtherLine.Columns.Add("CAP", "Capital");
            dgvOtherLine.Columns.Add("PREVGR", "Prev. Gross");   
            dgvOtherLine.Columns.Add("GR", "Gross");
            dgvOtherLine.Columns.Add("STAT", "Status");
            
            dgvOtherLine.RowHeadersVisible = false;
            dgvOtherLine.Columns[0].Width = 175;
            dgvOtherLine.Columns[1].Width = 175;
            dgvOtherLine.Columns[2].Width = 80;
            dgvOtherLine.Columns[3].Width = 80;
            dgvOtherLine.Columns[4].Width = 80;
            dgvOtherLine.Columns[5].Width = 60;
            dgvOtherLine.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherLine.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvOtherLine.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvOtherLine.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            int iRow = 0;
            string sMainBnsCode = "";
            string sSubBnsCode = "";

            pRec.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    dgvOtherLine.Rows.Add("");
                    sMainBnsCode = pRec.GetString("bns_code_main").Substring(0,2);
                    sSubBnsCode = pRec.GetString("bns_code_main");

                    dgvOtherLine[0, iRow].Value = AppSettingsManager.GetBnsDesc(sMainBnsCode);
                    dgvOtherLine[1, iRow].Value = AppSettingsManager.GetBnsDesc(sSubBnsCode);
                    dgvOtherLine[2, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    dgvOtherLine[3, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("prev_gross"));
                    dgvOtherLine[4, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));
                    dgvOtherLine[5, iRow].Value = pRec.GetString("bns_stat");
                    iRow++;
                }
            }
            pRec.Close();
        }

        private void LoadComboCell(string sBnsCode, int iRow)
        {
            OracleResultSet result = new OracleResultSet();
            string sRevYear = AppSettingsManager.GetConfigValue("07");

            comboCell = new DataGridViewComboBoxCell();

            comboCell.Items.Clear();

            result.Query = string.Format("select * from bns_table where trim(bns_code) like '{0}%' and fees_code = 'B'", sBnsCode);
            result.Query += string.Format(" and length(trim(bns_code)) > 2 and rev_year = '{0}' order by bns_code", sRevYear);
            if (result.Execute())
            {
                while (result.Read())
                    comboCell.Items.AddRange(result.GetString("bns_desc"));
            }
            result.Close();

            try
            {
                dgvApplyOtherLine.Rows[iRow].Cells[1] = comboCell;
            }
            catch
            {
            }
        }

        private void dgvApplyOtherLine_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            string sBnsCode = "";

            /*if (e.ColumnIndex == 1)
            {
                if (dgvApplyOtherLine[0, e.RowIndex].Value != null)
                {
                    sBnsCode = dgvApplyOtherLine[0, e.RowIndex].Value.ToString().Trim();

                    pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}' order by bns_code", sBnsCode, AppSettingsManager.GetConfigValue("07"));
                    if (pSet.Execute())
                    {
                        sBnsCode = "";
                        if (pSet.Read())
                        {
                            sBnsCode = pSet.GetString("bns_code");
                        }
                    }
                    pSet.Close();

                    LoadComboCell(sBnsCode, e.RowIndex);
                }
            }*/
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (dgvApplyOtherLine.Rows.Count > 0)
                m_bWithChange = true;
            else
                m_bWithChange = false;


            this.Close();
        }

        private void dgvApplyOtherLine_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            string sBnsCode = "";

            if (e.ColumnIndex == 5)
            {
                if (dgvApplyOtherLine[0, e.RowIndex].Value != null &&
                dgvApplyOtherLine[1, e.RowIndex].Value != null &&
                dgvApplyOtherLine[5, e.RowIndex].Value != null)
                {
                    if(dgvApplyOtherLine.Rows.Count - 1 == e.RowIndex)
                        dgvApplyOtherLine.Rows.Add("");
                }
            }

            if (e.ColumnIndex == 0)
            {
                if (dgvApplyOtherLine[0, e.RowIndex].Value != null)
                {
                    sBnsCode = dgvApplyOtherLine[0, e.RowIndex].Value.ToString().Trim();

                    pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}' order by bns_code", sBnsCode, AppSettingsManager.GetConfigValue("07"));
                    if (pSet.Execute())
                    {
                        sBnsCode = "";
                        if (pSet.Read())
                        {
                            sBnsCode = pSet.GetString("bns_code");
                        }
                    }
                    pSet.Close();

                    LoadComboCell(sBnsCode, e.RowIndex);
                }

            }

            if (e.ColumnIndex == 1)
            {
                if (dgvApplyOtherLine[1, e.RowIndex].Value != null)
                {
                    sBnsCode = AppSettingsManager.GetBnsCodeByDesc(dgvApplyOtherLine[1, e.RowIndex].Value.ToString().Trim());

                    string sTmpBnsCode = "";
                    for (int iRow = 0; iRow < dgvOtherLine.Rows.Count; iRow++)
                    {
                        sTmpBnsCode = AppSettingsManager.GetBnsCodeByDesc(dgvOtherLine[1,iRow].Value.ToString().Trim());

                        if (sBnsCode == sTmpBnsCode)
                        {
                            MessageBox.Show("Business line already exists.","Business Mapping",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dgvApplyOtherLine[1, iRow].Value = "";
                            return;

                        }
                    }

                    for (int iRow = 0; iRow < dgvApplyOtherLine.Rows.Count; iRow++)
                    {
                        if (iRow != e.RowIndex)
                        {
                            sTmpBnsCode = AppSettingsManager.GetBnsCodeByDesc(dgvApplyOtherLine[1, iRow].Value.ToString().Trim());

                            if (sBnsCode == sTmpBnsCode)
                            {
                                MessageBox.Show("Business line already exists.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dgvApplyOtherLine[1, iRow].Value = "";
                                return;
                            }
                        }
                    }
                }
            }

            if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                double dTemp = 0;
                try
                {
                    if (dgvApplyOtherLine[e.ColumnIndex, e.RowIndex].Value != null)
                    {
                        double.TryParse(dgvApplyOtherLine[e.ColumnIndex, e.RowIndex].Value.ToString(), out dTemp);
                        dgvApplyOtherLine[e.ColumnIndex, e.RowIndex].Value = string.Format("{0:#,###.00}", dTemp);
                    }
                }
                catch
                {
                    dgvApplyOtherLine[e.ColumnIndex, e.RowIndex].Value = "0";
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            if (!Validations())
                return;

            if (MessageBox.Show("Save application for other line/s of business?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd.Query = "delete from btm_addl_bns_que ";
                pCmd.Query += " where bin = '" + m_sBIN + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                for (int iRow = 0; iRow < dgvApplyOtherLine.Rows.Count; iRow++)
                {
                    string sMainDesc = "";
                    string sSubDesc = "";
                    string sStatus = "";
                    string sBnsCode = "";
                    string sCapital = "0";
                    string sGross = "0";
                    string sPrevGross = "0";

                    if(dgvApplyOtherLine[0, iRow].Value != null)
                        sMainDesc = dgvApplyOtherLine[0, iRow].Value.ToString().Trim();
                    if(dgvApplyOtherLine[1, iRow].Value != null)
                        sSubDesc = dgvApplyOtherLine[1, iRow].Value.ToString().Trim();
                    if(dgvApplyOtherLine[2, iRow].Value != null)                    
                        sCapital = dgvApplyOtherLine[2, iRow].Value.ToString().Trim();
                    if(dgvApplyOtherLine[3, iRow].Value != null)
                        sPrevGross = dgvApplyOtherLine[3, iRow].Value.ToString().Trim();
                    if(dgvApplyOtherLine[4, iRow].Value != null)
                        sGross = dgvApplyOtherLine[4, iRow].Value.ToString().Trim();
                    if(dgvApplyOtherLine[5, iRow].Value != null)
                        sStatus = dgvApplyOtherLine[5, iRow].Value.ToString().Trim(); ;

                    if (sMainDesc != "" && sSubDesc != "" && sStatus == "")
                    {
                        MessageBox.Show("Indicate business status for " + sMainDesc + ".", "Business Tax Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    /*if (sStatus == "NEW" && sCapital == "")
                    {
                        MessageBox.Show("Indicate capital for " + sMainDesc + ".", "Business Tax Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    if (sStatus == "REN" && (sPrevGross == "" || sGross == ""))
                    {
                        MessageBox.Show("Indicate gross & previous gross for " + sMainDesc + ".", "Business Tax Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }*/ // disable validation

                    if (sMainDesc.Trim() != "" && sSubDesc.Trim() != "" && sStatus.Trim() != "")
                    {
                        sBnsCode = AppSettingsManager.GetBnsCodeByDesc(sSubDesc);

                        if (sBnsCode.Trim() != "")
                        {
                            pCmd.Query = "insert into btm_addl_bns_que values (";
                            pCmd.Query += "'" + m_sBIN + "', ";
                            pCmd.Query += "'" + sBnsCode + "', ";
                            pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sCapital)) + "', ";
                            pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sGross)) + "', ";
                            pCmd.Query += "'" + m_sTaxYear + "', ";
                            pCmd.Query += "'" + sStatus + "','1', ";
                            pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sPrevGross)) + "')";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            pCmd.Query = "delete from btm_transfer_table ";
                            pCmd.Query += " where bin = '" + m_sBIN + "'";
                            pCmd.Query += " and trans_app_code = '" + sBnsCode + "'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                            pCmd.Query = "insert into btm_transfer_table ";
                            pCmd.Query += "values('" + m_sBIN + "','AD','" + sBnsCode + "','','',' ',' ',' ',";
                            pCmd.Query += "' ',' ',' ',' ',' ',' ',' ',";
                            pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }
                        }
                    }

                    m_bWithChange = true;    
                }

                
                MessageBox.Show("Application for other line/s of business saved.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                
                
            }
                
        }

        private void LoadApplyOtherLine()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBnsCode = "";
            string sMainDesc = "";
            string sSubDesc = "";
            int iRow = 0;

            pRec.Query = string.Format("select * from btm_addl_bns_que where bin = '{0}'", m_sBIN);
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sBnsCode = pRec.GetString("bns_code_main");
                    sSubDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sMainDesc = AppSettingsManager.GetBnsDesc(sBnsCode.Substring(0, 2));

                    dgvApplyOtherLine.Rows.Add("");
                    dgvApplyOtherLine[0, iRow].Value = sMainDesc;
                    dgvApplyOtherLine[1, iRow].Value = sSubDesc;
                    dgvApplyOtherLine[2, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("capital"));
                    dgvApplyOtherLine[3, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("prev_gross"));
                    dgvApplyOtherLine[4, iRow].Value = string.Format("{0:#,###.00}", pRec.GetDouble("gross"));
                    dgvApplyOtherLine[5, iRow].Value = pRec.GetString("bns_stat");
                    iRow++;
                }
            }
            pRec.Close();
             
        }

        private bool Validations()
        {
            string sSubDesc = "";
            string sBnsCode = "";
            string sTmpBnsCode = "";
            int iCntData = 0;

            for (int iRow = 0; iRow < dgvApplyOtherLine.Rows.Count; iRow++)
            {
                if (dgvApplyOtherLine[1, iRow].Value != null)
                {
                    sSubDesc = dgvApplyOtherLine[1, iRow].Value.ToString().Trim();
                    sBnsCode = AppSettingsManager.GetBnsCodeByDesc(sSubDesc);

                    for (int iRow1 = 0; iRow1 < dgvApplyOtherLine.Rows.Count; iRow1++)
                    {
                        if (dgvApplyOtherLine[1, iRow1].Value != null)
                        {
                            sSubDesc = dgvApplyOtherLine[1, iRow1].Value.ToString().Trim();
                            sTmpBnsCode = AppSettingsManager.GetBnsCodeByDesc(sSubDesc);

                            if ((sBnsCode == sTmpBnsCode) && (iRow != iRow1))
                            {
                                MessageBox.Show("Duplicate line of business detected.\nPlease check.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return false;
                            }
                        }
                    }

                    iCntData++;
                }
            }

            if (iCntData == 0)
            {
                MessageBox.Show("No data to save", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void dgvApplyOtherLine_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    DataGridView grid = sender as DataGridView;
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("Delete", null, new EventHandler(contextMenuStrip1_Click));
                    Point pt = grid.PointToClient(Control.MousePosition);
                    menu.Show(dgvApplyOtherLine, pt.X, pt.Y);
                    break;
                
                default:
                    break;
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            if (m_iRow < 0)
            {
                MessageBox.Show("Select row you want to delete","Business Mapping",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string sBnsDesc = dgvApplyOtherLine[1, m_iRow].Value.ToString();
            int iTmpRow = m_iRow + 1;

            if (MessageBox.Show("Delete row " + iTmpRow + "?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgvApplyOtherLine.Rows.RemoveAt(m_iRow);
                
            }
            
        }

       
        private void dgvApplyOtherLine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_iRow = e.RowIndex;
        }

        private void btnBnsType_Click(object sender, EventArgs e)
        {
            string sBnsCode = "";
            int intTempRow = 0;
            int intTempCol = 0;
            intTempRow = dgvApplyOtherLine.SelectedCells[0].RowIndex;
            intTempCol = dgvApplyOtherLine.SelectedCells[0].ColumnIndex;

            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(false);
            frmBnsType.ShowDialog();
            
            sBnsCode = frmBnsType.m_strBnsCode;

            dgvApplyOtherLine[0,intTempRow].Value = AppSettingsManager.GetBnsDesc(sBnsCode.Substring(0,2));
            LoadComboCell(sBnsCode.Substring(0, 2), intTempRow);
            dgvApplyOtherLine[1, intTempRow].Value = frmBnsType.m_sBnsDescription;

        }

    }
}