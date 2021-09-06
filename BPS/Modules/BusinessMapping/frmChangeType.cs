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
    public partial class frmChangeType : Form
    {
        private string m_sBIN = string.Empty;
        private string m_sTaxYear = string.Empty;
        private string m_sNewBnsCode = string.Empty;
        private bool m_bWithChange = false;
        private string m_sStat = string.Empty;

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

        public string Status
        {
            get { return m_sStat; }
            set { m_sStat = value; }
        }

        public string NewBnsCode
        {
            get { return m_sNewBnsCode; }
            set { m_sNewBnsCode = value; }
        }

        public bool WithChange
        {
            get { return m_bWithChange; }
            set { m_bWithChange = value; }
        }

        public frmChangeType()
        {
            InitializeComponent();
        }

        private void frmChangeType_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            OracleResultSet pRec = new OracleResultSet();
            dgvList.Columns.Clear();

            dgvList.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            dgvList.Columns.Add("OLD_CODE", "Old Code");
            dgvList.Columns.Add("OLD_TYPE", "Current Business Type/s");
            dgvList.Columns.Add("NEW_TYPE", "Apply new Business Type");
            dgvList.Columns.Add("NEW_CODE", "New Code");
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("BNS_STAT", "Status");
            
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Visible = false;
            dgvList.Columns[1].Width = 230;
            dgvList.Columns[2].Width = 230;
            dgvList.Columns[3].Visible = false;
            dgvList.Columns[4].Width = 30;
            dgvList.Columns[5].Visible = false;

            dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            string sBnsCode = "";
            int iRow = 0;

            dgvList.Rows.Add("");
            sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
            dgvList[0, iRow].Value = sBnsCode;
            dgvList[1, iRow].Value = "(MAIN) " + AppSettingsManager.GetBnsDesc(sBnsCode);
            dgvList[4, iRow].Value = false;
            GetChangedType(sBnsCode, iRow);
            dgvList[5, iRow].Value = m_sStat;

            pRec.Query = "select * from addl_bns where bin = '" + m_sBIN + "' and tax_year = '" + m_sTaxYear + "' order by bns_code_main";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    iRow++;
                    sBnsCode = pRec.GetString("bns_code_main");
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = sBnsCode;
                    dgvList[1, iRow].Value = AppSettingsManager.GetBnsDesc(sBnsCode);
                    dgvList[4, iRow].Value = false;
                    dgvList[5, iRow].Value = pRec.GetString("bns_stat");

                    GetChangedType(sBnsCode, iRow);
                }
            }
            pRec.Close();
        }

        private void GetChangedType(string sOldBnsCode, int iRow)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sNewBnsCode = "";

            pSet.Query = "select * from btm_change_class_tbl where bin = '" + m_sBIN + "' and old_bns_code = '" + sOldBnsCode + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sNewBnsCode = pSet.GetString("NEW_BNS_CODE");
                    dgvList[2, iRow].Value = AppSettingsManager.GetBnsDesc(sNewBnsCode);
                    dgvList[3, iRow].Value = sNewBnsCode;
                    dgvList[4, iRow].Value = true;
                    
                }
            }
            pSet.Close();
                        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel new business type/s?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_bWithChange = false;
                this.Close();
            }
            else
                return;
        }

        private void dgvList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
                dgvList.ReadOnly = true;
            else
                dgvList.ReadOnly = false;

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();

            string sNewBnsCode = "";
            string sOldBnsCode = "";
            string sMainBuss = "";
            string sType = "";
            string sCapital = "";
            string sGross = "";
            string sPrevGross = "";
            string sStatus = "";

            m_bWithChange = false;

            if (MessageBox.Show("Save new business type/s?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd.Query = "delete from btm_change_class_tbl where bin = '"+m_sBIN+"'";
                if(pCmd.ExecuteNonQuery() == 0)
                {}

                int iRow;
                for (iRow = 0; iRow < dgvList.Rows.Count; iRow++)
                {
                    try
                    {
                        sOldBnsCode = dgvList[0, iRow].Value.ToString();
                        sNewBnsCode = dgvList[3, iRow].Value.ToString();
                        sStatus = dgvList[5, iRow].Value.ToString();

                        sCapital = "0";
                        sGross = "0";
                        sPrevGross = "0";

                        if (iRow == 0)
                        {
                            sMainBuss = "Y";
                            sType = "CT";
                            m_sNewBnsCode = sNewBnsCode;
                        }
                        else
                        {
                            sMainBuss = "N";
                            sType = "AD";
                        }
                        string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                        DateTime dDate = Convert.ToDateTime(sDate);
                        if (sNewBnsCode != "")
                        {
                            pCmd.Query = "insert into btm_change_class_tbl values (:1, :2, :3, '0', :4, :5, :6)";
                            pCmd.AddParameter(":1", m_sBIN);
                            pCmd.AddParameter(":2", sOldBnsCode);
                            pCmd.AddParameter(":3", sNewBnsCode);
                            pCmd.AddParameter(":4", sStatus);
                            pCmd.AddParameter(":5", sMainBuss);
                            pCmd.AddParameter(":6", dDate);
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            if (iRow > 0)
                            {
                                pCmd.Query = "delete from btm_addl_bns_que ";
                                pCmd.Query += " where bin = '" + m_sBIN + "' ";
                                pCmd.Query += " and bns_code_main = '" + sNewBnsCode + "'";
                                if (pCmd.ExecuteNonQuery() == 0)
                                { }

                                pCmd.Query = "delete from btm_addl_bns_que ";
                                pCmd.Query += " where bin = '" + m_sBIN + "' ";
                                pCmd.Query += " and bns_code_main = '" + sOldBnsCode + "'";
                                if (pCmd.ExecuteNonQuery() == 0)
                                { }

                                pCmd.Query = "insert into btm_addl_bns_que values (";
                                pCmd.Query += "'" + m_sBIN + "', ";
                                pCmd.Query += "'" + sNewBnsCode + "', ";
                                pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sCapital)) + "', ";
                                pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sGross)) + "', ";
                                pCmd.Query += "'" + m_sTaxYear + "', ";
                                pCmd.Query += "'" + sStatus + "','1', ";
                                pCmd.Query += "'" + string.Format("{0:##.00}", Convert.ToDouble(sPrevGross)) + "')";
                                if (pCmd.ExecuteNonQuery() == 0)
                                { }

                            }

                            pCmd.Query = "delete from btm_transfer_table ";
                            pCmd.Query += " where bin = '" + m_sBIN + "'";
                            pCmd.Query += " and trans_app_code = '" + sType + "'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            pCmd.Query = "insert into btm_transfer_table ";
                            pCmd.Query += "values('" + m_sBIN + "','" + sType + "','" + sNewBnsCode + "','" + sOldBnsCode + "','',' ',' ',' ',";
                            pCmd.Query += "' ',' ',' ',' ',' ',' ',' ',";
                            pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }

                            m_bWithChange = true;
                        }
                    }
                    catch { }
                }

                if (iRow > 0)
                {
                    MessageBox.Show("New business type/s saved.", "Bussines Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No new business type/s to save", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                dgvList.ReadOnly = true;
            else if (e.ColumnIndex == 4)
            {
                //dgvList.BeginEdit(true);

                if (dgvList[4, e.RowIndex].Value != null)
                {
                    if ((bool)dgvList[4, e.RowIndex].Value == true)
                    {
                        dgvList[4, e.RowIndex].Value = false;
                        dgvList[2, e.RowIndex].Value = "";
                        dgvList[3, e.RowIndex].Value = "";
                    }
                    else
                    {
                        frmBusinessType frmBnsType = new frmBusinessType();
                        frmBnsType.SetFormStyle(false);
                        frmBnsType.ShowDialog();

                        if (Validations(frmBnsType.m_strBnsCode, e.RowIndex))
                        {
                            dgvList[2, e.RowIndex].Value = frmBnsType.m_sBnsDescription;
                            dgvList[3, e.RowIndex].Value = frmBnsType.m_strBnsCode;

                            if (dgvList[3, e.RowIndex].Value != null)
                                dgvList[4, e.RowIndex].Value = true;
                            else
                                dgvList[4, e.RowIndex].Value = false;
                        }
                        else
                        {
                            dgvList[2, e.RowIndex].Value = "";
                            dgvList[3, e.RowIndex].Value = "";
                            dgvList[4, e.RowIndex].Value = false;
                        }
                    }
                    dgvList.EndEdit();
                }
            }
            
        }

        private void dgvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
                dgvList.ReadOnly = true;
            else
                dgvList.ReadOnly = false;
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
                dgvList.ReadOnly = true;
            else
                dgvList.ReadOnly = false;
        }

        private bool Validations(string sNewBnsCode, int iRow)
        {
            string sTmpNewBnsCode = "";
            string sTmpOldBnsCode = "";

            for (int iTmpRow = 0; iTmpRow < dgvList.Rows.Count; iTmpRow++)
            {
                sTmpOldBnsCode = dgvList[0, iTmpRow].Value.ToString();

                if (dgvList[3, iTmpRow].Value != null)
                    sTmpNewBnsCode = dgvList[3, iTmpRow].Value.ToString();
                else
                    sTmpNewBnsCode = "";

                if (sTmpNewBnsCode == sNewBnsCode || sTmpOldBnsCode == sNewBnsCode)
                {
                    MessageBox.Show("Business type "+ AppSettingsManager.GetBnsDesc(sNewBnsCode) + " already exists.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            return true;
        }
    }
}