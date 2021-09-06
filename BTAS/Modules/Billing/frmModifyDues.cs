

// RMC 20150117


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.BPLS.Billing
{
    public partial class frmModifyDues : Form
    {
        private string m_sBIN = string.Empty;
        private string m_sBnsNm = string.Empty;
        private string m_sOwnNm = string.Empty;
        private string m_sTaxYear = string.Empty;
        private bool m_bSaved = false;

        private string m_sType = string.Empty; //MCR 20150120 viewing
        public string Type //MCR 20150120 viewing
        {
            set { m_sType = value; }
        }

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

        public bool DataSaved
        {
            get { return m_bSaved; }
            set { m_bSaved = value; }
        }

        private string m_sBnsStat = String.Empty;
        public string BnsStat
        {
            set { m_sBnsStat = value; }
        }

        public frmModifyDues()
        {
            InitializeComponent();
            bin.GetLGUCode = AppSettingsManager.GetConfigObject(ConfigurationAttributes.LGUCode);
            bin.GetDistCode = AppSettingsManager.GetConfigObject("11");
        }

        private void frmModifyDues_Load(object sender, EventArgs e)
        {
            //m_sBIN = "019-08-2014-0000448";
            //m_sTaxYear = "2012";

            int iTmp = 0;
            int.TryParse(ConfigurationAttributes.RevYear, out iTmp);
            iTmp = iTmp - 1;

            this.Text = "Previous Revenue Year Dues: " + m_sTaxYear + "-" + iTmp.ToString();

            bin.txtTaxYear.Text = m_sBIN.Substring(7, 4);
            bin.txtBINSeries.Text = m_sBIN.Substring(12, 7);
            txtBusinessName.Text = AppSettingsManager.GetBnsName(m_sBIN);
            txtOwnersName.Text = AppSettingsManager.GetBnsOwner(m_sBIN);
            UpdateList();

            if (m_sType != "") //MCR 20150115
                btnSave.Visible = false;
        }

        private void UpdateList()
        {
            dgvTaxFees.Columns.Clear();

            dgvTaxFees.Columns.Add("1","Tax Year");
            dgvTaxFees.Columns.Add("2","Qtr");
            dgvTaxFees.Columns.Add("3","");
            dgvTaxFees.Columns.Add("4","Code");
            dgvTaxFees.Columns.Add("5","Bns Code");
            dgvTaxFees.Columns.Add("6","Description");
            dgvTaxFees.Columns.Add("7","Gross");
            dgvTaxFees.Columns.Add("8", "Amount");
            dgvTaxFees.Columns.Add("9", "Fees Type");
            dgvTaxFees.Columns[0].Width = 50;
            dgvTaxFees.Columns[1].Width = 50;
            dgvTaxFees.Columns[2].Width = 50;
            dgvTaxFees.Columns[3].Width = 50;
            dgvTaxFees.Columns[4].Width = 50;
            dgvTaxFees.Columns[5].Width = 200;
            dgvTaxFees.Columns[6].Width = 75;
            dgvTaxFees.Columns[7].Width = 75;
            dgvTaxFees.Columns[2].Visible = false;
            dgvTaxFees.Columns[3].Visible = false;
            dgvTaxFees.Columns[4].Visible = false;
            dgvTaxFees.Columns[8].Visible = false;
            dgvTaxFees.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvTaxFees.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            //MCR 20150127 (s)
            for (int i = 0; i < dgvTaxFees.ColumnCount; i++)
                dgvTaxFees.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //MCR 20150127 (e)

            int iTaxYear = 0;
            int iCurRevYear = 0;

            int.TryParse(m_sTaxYear, out iTaxYear);
            int.TryParse(ConfigurationAttributes.RevYear, out iCurRevYear);

            if (m_sType == "")
            {
                for (int i = iTaxYear; i < iCurRevYear; i++)
                {
                    DisplayBnsAndMP(i.ToString());

                    //regulatory fees
                    OracleResultSet pSet = new OracleResultSet();
                    string sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
                    string sFeeCode = string.Empty;
                    string sFeesType = string.Empty;
                    string sAmount = string.Empty;

                    pSet.Query = "select fees_code, fees_type from tax_and_fees_table where fees_code <> '" + GetMPCode() + "' order by fees_code";
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            sFeeCode = pSet.GetString(0);
                            if (pSet.GetString(1) == "FS")
                                sFeesType = "TF";
                            else
                                sFeesType = "AF";


                            dgvTaxFees.Rows.Add(i.ToString(), "1", "R", sFeeCode, sBnsCode, AppSettingsManager.GetFeesDesc(sFeeCode), "0", "0", sFeesType);
                        }
                    }
                    pSet.Close();
                    dgvTaxFees.Rows.Add("");    // add space per year
                }
            }
            else
                LoadDues();
        }

        private void LoadDues()
        {
            OracleResultSet pSet = new OracleResultSet();
           // int iCnt = 0;
            dgvTaxFees.ReadOnly = true;
            dgvTaxFees.Columns[6].Visible = false;
            dgvTaxFees.EditMode = DataGridViewEditMode.EditProgrammatically;
            string sFeesType = string.Empty;
            pSet.Query = "select * from taxdues where bin = '" + m_sBIN + "' and tax_year >= '" + m_sTaxYear + "' ";
            pSet.Query += "and tax_year < '" + ConfigurationAttributes.RevYear + "' order by tax_year";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgvTaxFees.Rows.Add(pSet.GetString("tax_year"), pSet.GetString("qtr_to_pay"),
                        pSet.GetString("due_state"), pSet.GetString("tax_code"),
                        pSet.GetString("bns_code_main"), AppSettingsManager.GetFeesDesc(pSet.GetString("tax_code")),
                        "0", pSet.GetDouble("amount"), sFeesType);
                }
            }
            pSet.Close();

            //if (iCnt == 0)
            //    return false;
            //else
            //    return true; 
        }

        private string GetMPCode()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sMPCode = string.Empty;

            //pSet.Query = "select distinct fees_code from bns_table where bns_desc like 'MAYOR%'";
            pSet.Query = "select * from tax_and_fees_table where fees_desc like 'MAYOR%' and rev_year = '" + AppSettingsManager.GetConfigObject("07") + "'"; //MCR 20170519
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sMPCode = pSet.GetString(0);
                }
            }
            pSet.Close();

            return sMPCode;
        }

        private void DisplayBnsAndMP(string sYear)
        {
            //business tax & mayors permit
            string sMPCode = GetMPCode();

            string sBnsCode = AppSettingsManager.GetBnsCodeMain(m_sBIN);
            string sGross = "0";
            
            dgvTaxFees.Rows.Add(sYear, "1","R","B",sBnsCode,"TAX ON " +AppSettingsManager.GetBnsDesc(sBnsCode),"0","0","TF");
            dgvTaxFees.Rows.Add(sYear, "1", "R", sMPCode, sBnsCode, AppSettingsManager.GetFeesDesc(sMPCode), "0", "0","TF");

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select distinct bns_code_main from addl_bns where bin = '" + m_sBIN + "'";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString(0);

                    dgvTaxFees.Rows.Add(sYear, "1", "R", "B", sBnsCode, "TAX ON " + AppSettingsManager.GetBnsDesc(sBnsCode), "0", "0", "TF");
                    dgvTaxFees.Rows.Add(sYear, "1", "R", sMPCode, sBnsCode, AppSettingsManager.GetFeesDesc(sMPCode), "0", "0", "TF");
                }
            }
            pSet.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_sType == "")
            {
                OracleResultSet pSet = new OracleResultSet();
                int iCnt = 0;

                pSet.Query = "select count(*) from taxdues where bin = '" + m_sBIN + "' and tax_year >= '" + m_sTaxYear + "' ";
                pSet.Query += " and tax_year < '" + ConfigurationAttributes.RevYear + "'";
                int.TryParse(pSet.ExecuteScalar(), out iCnt);

                if (iCnt == 0)
                {
                    if (MessageBox.Show("Tax dues not yet saved. Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                    else
                        m_bSaved = false;
                }
                else
                    m_bSaved = true;

                this.Close();
            }
            else
                this.Close();
        }

        private void dgvTaxFees_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {

                //validate float
                try
                {
                    double dTmp = 0;
                    double.TryParse(dgvTaxFees[e.ColumnIndex, e.RowIndex].Value.ToString(), out dTmp);
                    dgvTaxFees[e.ColumnIndex, e.RowIndex].Value = string.Format("{0:#,###.00}", dTmp);
                }
                catch { }
            }

            if (e.ColumnIndex == 7)
            {
                UpdateTotal();
            }
        }

        private void dgvTaxFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                try
                {
                    //validate float
                    double dTmp = 0;
                    double.TryParse(dgvTaxFees[e.ColumnIndex, e.RowIndex].Value.ToString(), out dTmp);
                    dgvTaxFees[e.ColumnIndex, e.RowIndex].Value = string.Format("{0:#,###.00}", dTmp);
                }
                catch { }
            }

            if (e.ColumnIndex == 7)
            {
                UpdateTotal();
            }
        }

        private void dgvTaxFees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                try
                {
                    if (dgvTaxFees[5, e.RowIndex].Value.ToString().Trim() != "")
                        dgvTaxFees.ReadOnly = false;
                    else
                        dgvTaxFees.ReadOnly = true;
                }
                catch { dgvTaxFees.ReadOnly = true; }
            }
            else
                dgvTaxFees.ReadOnly = true;
        }

        private void UpdateTotal()
        {
            double dTotal = 0;
            double dTmp = 0;

            for (int i = 0; i < dgvTaxFees.Rows.Count; i++)
            {
                try
                {
                    double.TryParse(dgvTaxFees[7, i].Value.ToString(), out dTmp);
                }
                catch
                {
                    dTmp = 0;
                }

                dTotal += dTmp;
            }

            txtTotal.Text = string.Format("{0:#,###.00}", dTotal);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            string sFeesCode = string.Empty;
            string sFeesType = string.Empty;
            string sTaxYear = string.Empty;
            string sBnsCode = string.Empty;
            double fAmount = 0;
            double fGross = 0;

            if (MessageBox.Show("Save tax dues?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!ValidateGross())
                    return;

                pCmd.Query = "delete from taxdues where bin = '" + m_sBIN + "' and tax_year >= '" + m_sTaxYear + "' ";
                pCmd.Query += "and tax_year < '" + ConfigurationAttributes.RevYear + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                int iCntAddlCharges = 0;    // RMC 20150120
                string sTmpTaxYear = ""; // RMC 20150120

                for (int i = 0; i < dgvTaxFees.Rows.Count; i++)
                {
                    
                    try
                    {
                        sTaxYear = dgvTaxFees.Rows[i].Cells[0].Value.ToString();

                        // RMC 20150120
                        if (sTaxYear != sTmpTaxYear)
                        {
                            if (sTmpTaxYear != "" && iCntAddlCharges > 0)
                            {
                                InsertDues(sFeesType, "01", fAmount, sTmpTaxYear, sBnsCode);
                            }

                            sTmpTaxYear = sTaxYear;
                            iCntAddlCharges = 0;
                        }
                        // RMC 20150120

                        sFeesCode = dgvTaxFees.Rows[i].Cells[3].Value.ToString();
                        sBnsCode = dgvTaxFees.Rows[i].Cells[4].Value.ToString();
                        sFeesType = dgvTaxFees.Rows[i].Cells[8].Value.ToString();
                        double.TryParse(dgvTaxFees.Rows[i].Cells[7].Value.ToString(), out fAmount);
                        double.TryParse(dgvTaxFees.Rows[i].Cells[6].Value.ToString(), out fGross);
                        if (fAmount > 0)
                        {
                            if (sFeesCode == "B")
                            {
                                InsertDues(sFeesType, "00", fAmount, sTaxYear, sBnsCode);

                                UpdateBillGrossInfo(sBnsCode, sTaxYear, fGross);
                            }
                            else
                            {
                                if (sFeesType == "AF")
                                {
                                    UpdateAddCharges(fAmount, sBnsCode, sFeesCode);
                                    //InsertDues(sFeesType, "01", fAmount, sTaxYear, sBnsCode); // RMC 20150120
                                    iCntAddlCharges++;  // RMC 20150120
                                }
                                else
                                    InsertDues(sFeesType, sFeesCode, fAmount, sTaxYear, sBnsCode);
                            }
                        }

                        
                    }
                    catch { }

                }

                // RMC 20150120 (s) for last taxyear
                if(iCntAddlCharges > 0)
                    InsertDues(sFeesType, "01", fAmount, sTmpTaxYear, sBnsCode);
                // RMC 20150120 (e)

                if (!ValidateSkippedYear())
                {
                    MessageBox.Show("Skipped year was detected. Please verify",this.Text,MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return;
                }

                m_bSaved = true;
                this.Close();
            }
        }

        private bool ValidateSkippedYear()
        {
            string sTaxYear = string.Empty;
            string sFeesCode = string.Empty;

            for (int i = 0; i < dgvTaxFees.Rows.Count; i++)
            {
                try
                {
                    sTaxYear = dgvTaxFees.Rows[i].Cells[0].Value.ToString();
                    sFeesCode = dgvTaxFees.Rows[i].Cells[3].Value.ToString();

                    if (sFeesCode == "B")
                    {
                        OracleResultSet pSet = new OracleResultSet();
                        int iCnt = 0;

                        pSet.Query = "select count(*) from taxdues where bin = '" + m_sBIN + "' and tax_year = '" + sTaxYear + "'";
                        int.TryParse(pSet.ExecuteScalar(), out iCnt);

                        if (iCnt == 0)
                        {
                            return false;
                        }
                    }
                }
                catch{}
            }

            return true;
        }

        private void InsertDues(string p_sFeesType, string p_sFeesCode, double p_fAmount, string p_sTaxYear, string p_sBnsCode)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "insert_dues";
            plsqlCmd.ParamValue = m_sBIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBnsCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sTaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            //plsqlCmd.ParamValue = "R";  //ren
            plsqlCmd.ParamValue = m_sBnsStat;  //ren
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "1";
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = ConfigurationAttributes.RevYear;
            plsqlCmd.AddParameter("p_sRevYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_iSwRE", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sFeesType;
            plsqlCmd.AddParameter("p_sFeesType", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sFeesCode;
            plsqlCmd.AddParameter("p_sFeesCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_fAmount;
            plsqlCmd.AddParameter("p_fAmount", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
            {
                plsqlCmd.Rollback();

            }
            plsqlCmd.Close();
        }

        private void UpdateBillGrossInfo(string p_sBnsCode, string p_sTaxYear, double p_dGross)
        {
            OraclePLSQL plsqlCmd = new OraclePLSQL();
            double dGrCap;
            int iIsReset = 0;
            plsqlCmd.Transaction = true;
            plsqlCmd.ProcedureName = "update_billgross_info";
            plsqlCmd.ParamValue = m_sBIN;
            plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sBnsCode;
            plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = p_sTaxYear;
            plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "1";
            plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "REN";
            plsqlCmd.AddParameter("p_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            plsqlCmd.ParamValue = "R";
            plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
            
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            
            plsqlCmd.ParamValue = p_dGross;
            plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fPreGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fAdjGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
           
            plsqlCmd.ParamValue = 0;
            plsqlCmd.AddParameter("p_fVatGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
            plsqlCmd.ParamValue = iIsReset;
            plsqlCmd.AddParameter("o_iIsReset", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);

            plsqlCmd.ExecuteNonQuery();
            if (!plsqlCmd.Commit())
                plsqlCmd.Rollback();
            else
                int.TryParse(plsqlCmd.ReturnValue("o_iIsReset").ToString(), out iIsReset);
            plsqlCmd.Close();
            
        }

        private bool ValidateGross()
        {
            string sTaxYear = ""; string sFeesCode = ""; string sBnsCode = "";
            double fGross = 0;

            for (int i = 0; i < dgvTaxFees.Rows.Count; i++)
            {
                try
                {
                    sTaxYear = dgvTaxFees.Rows[i].Cells[0].Value.ToString();
                    sFeesCode = dgvTaxFees.Rows[i].Cells[3].Value.ToString();
                    sBnsCode = dgvTaxFees.Rows[i].Cells[4].Value.ToString();
                    double.TryParse(dgvTaxFees.Rows[i].Cells[6].Value.ToString(), out fGross);

                    if (sFeesCode == "B")
                    {
                        if (fGross == 0)
                        {
                            if (MessageBox.Show("No gross entered for " + AppSettingsManager.GetBnsDesc(sBnsCode) + " tax year " + sTaxYear +". Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                return false;
                        }
                    }
                }
                catch { }
            }

            return true;
        }

        private void UpdateAddCharges(double p_fAmount, string p_sBnsCode, string p_sFeesCode)
        {
            OracleResultSet pCmd = new OracleResultSet();

            pCmd.Query = "UPDATE addl_table SET amount = :1, qty = :2 WHERE bin = :3 AND bns_code_main = :4 AND fees_code = :5 AND data_type = 'AD'";
            pCmd.AddParameter(":1", p_fAmount);
            pCmd.AddParameter(":2", 1);
            pCmd.AddParameter(":3", m_sBIN);
            pCmd.AddParameter(":4", p_sBnsCode);
            pCmd.AddParameter(":5", p_sFeesCode);
            pCmd.ExecuteNonQuery();

        }
    }
}