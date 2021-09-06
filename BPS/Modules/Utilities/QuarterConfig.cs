// RMC 20111001 corrected log-out in scheds


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public partial class frmQuarterConfig : Form
    {
        public bool blnWInitialData;

        public string RevYear
        {
            get { return txtRevYear.Text; }
            set { txtRevYear.Text = value; }
        }

        public string FeesCode
        {
            get { return txtFeesCode.Text; }
            set { txtFeesCode.Text = value; }
        }

        public string BnsCode
        {
            get { return txtBnsCode.Text; }
            set { txtBnsCode.Text = value; }
        }

        public string DataType
        {
            get { return txtDataType.Text; }
            set { txtDataType.Text = value; }
        }

        public frmQuarterConfig()
        {
            InitializeComponent();
        }

        private void frmQuarterConfig_Load(object sender, EventArgs e)
        {
            dgvFees.Columns.Clear();
            dgvFees.Columns.Add("GR1", "Value 1");
            dgvFees.Columns.Add("GR2", "Value 2");
            dgvFees.Columns.Add("AMT1", "Q1 Amount");
            dgvFees.Columns.Add("AMT2", "Q2 Amount");
            dgvFees.Columns.Add("AMT3", "Q3 Amount");
            dgvFees.Columns.Add("AMT4", "Q4 Amount");
            dgvFees.Columns[0].Width = 70;
            dgvFees.Columns[1].Width = 70;
            dgvFees.Columns[2].Width = 70;
            dgvFees.Columns[3].Width = 70;
            dgvFees.Columns[4].Width = 70;
            dgvFees.Columns[5].Width = 70;
            dgvFees.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            dgvFees.Columns[0].ReadOnly = true;
            dgvFees.Columns[1].ReadOnly = true;
            dgvFees.Columns[2].ReadOnly = true;
            
            if(txtDataType.Text.ToString() == "F")
            {
                dgvFees.Columns[0].Visible = false;
                dgvFees.Columns[1].Visible = false;
            }

            if (blnWInitialData)
                this.LoadSavedValue();
            else
                this.LoadInitialValues();
            dgvFees.Focus();

            OracleResultSet result = new OracleResultSet();

            cmbCompare.Items.Clear();
            result.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%%' and bns_code <> '{2}'", txtFeesCode.Text.ToString().Trim(), StringUtilities.Left(txtBnsCode.Text.ToString().Trim(), 2), txtBnsCode.Text.ToString().Trim());
            if (result.Execute())
            {
                if (result.Read())
                {
                    result.Close();
                    result.Query = string.Format("select distinct(bns_code) from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%%' and bns_code <> '{2}'", txtFeesCode.Text.ToString().Trim(), StringUtilities.Left(txtBnsCode.Text.ToString().Trim(), 2), txtBnsCode.Text.ToString().Trim());
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            cmbCompare.Items.Add(result.GetString("bns_code"));
                        }
                    }
                }
                else
                {
                    result.Close();
                    result.Query = string.Format("select distinct(bns_code) from fees_sched where fees_code = '{0}' and bns_code like '{1}%%' and bns_code <> '{2}'", txtFeesCode.Text.ToString().Trim(), StringUtilities.Left(txtBnsCode.Text.ToString().Trim(), 2), txtBnsCode.Text.ToString().Trim());
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            cmbCompare.Items.Add(result.GetString("bns_code"));
                        }
                    }
                    result.Close();
                }
            }
        }

        private void txtBnsCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strRowValue = string.Empty;
            double dblRowValue = 0;

            //strRowValue = dgvFees[e.ColumnIndex, e.RowIndex].Value.ToString();
            try
            {
                strRowValue = dgvFees[e.ColumnIndex, e.RowIndex].Value.ToString();  // RMC 20111001 corrected log-out in scheds
                dblRowValue = Convert.ToDouble(strRowValue);
                
            }
            catch
            {
                MessageBox.Show("Invalid entry", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                dgvFees[e.ColumnIndex, e.RowIndex].Value = "";
                return;
            }

            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                if (txtDataType.Text.ToString() == "Q" || txtDataType.Text.ToString() == "QR")
                    strRowValue = string.Format("{0:##}", dblRowValue);
                else
                    strRowValue = string.Format("{0:#,##.00}", dblRowValue);

            }
            else
            {
                strRowValue = string.Format("{0:#,##.00}", dblRowValue);
            }

            dgvFees[e.ColumnIndex, e.RowIndex].Value = strRowValue;
        }

        private void LoadInitialValues()
        {
            OracleResultSet result = new OracleResultSet();


            if (txtDataType.Text.ToString() == "QR")
            {
                result.Query = "select qty1, qty2, amount from tmp_fees_sched where fees_code = :1 and bns_code = :2 and rev_year = :3 and data_type = :4 ";
                result.Query += " order by qty1";
                
            }
            else if (txtDataType.Text.ToString() == "AR")
            {
                result.Query = "select area1, area2, amount from tmp_fees_sched where fees_code = :1 and bns_code = :2 and rev_year = :3 and data_type = :4 ";
                result.Query += " order by area1";
            }
            else if (txtDataType.Text.ToString() == "RR")
            {
                result.Query = "select gr_1, gr_2, amount from tmp_fees_sched where fees_code = :1 and bns_code = :2 and rev_year = :3 and data_type = :4 ";
                result.Query += " order by gr_1";
            }
            else
            {
                result.Query = "select '1', '1', amount from tmp_fees_sched where fees_code = :1 and bns_code = :2 and rev_year = :3 and data_type = :4 ";
                result.Query += " order by amount";
            }
            result.AddParameter(":1", txtFeesCode.Text.ToString());
            result.AddParameter(":2", txtBnsCode.Text.ToString());
            result.AddParameter(":3", txtRevYear.Text.ToString());
            result.AddParameter(":4", txtDataType.Text.ToString());
            
            if (result.Execute())
            {
                int intRow = 0;
                while (result.Read())
                {
                    dgvFees.Rows.Add("");

                    if (txtDataType.Text.ToString() == "AR" || txtDataType.Text.ToString() == "RR")
                    {
                        dgvFees[0, intRow].Value = string.Format("{0:#,##.00}", result.GetDouble(0));
                        dgvFees[1, intRow].Value = string.Format("{0:#,##.00}", result.GetDouble(1));
                    }
                    else if (txtDataType.Text.ToString() == "QR")
                    {
                        dgvFees[0, intRow].Value = string.Format("{0:#,##}", result.GetInt(0));
                        dgvFees[1, intRow].Value = string.Format("{0:#,##}", result.GetInt(1));

                        
                    }
                    else
                    {
                        dgvFees[0, intRow].Value = string.Format("{0:#,##}", 1);
                        dgvFees[1, intRow].Value = string.Format("{0:#,##}", 1);
                    }

                    if (dgvFees[0, intRow].Value.ToString() == "")
                        dgvFees[0, intRow].Value = "0";

                    if (dgvFees[1, intRow].Value.ToString() == "")
                        dgvFees[1, intRow].Value = "0";

                    dgvFees[2, intRow].Value = string.Format("{0:#,##.00}", result.GetDouble(2));

                    intRow++;
                }
            }
            result.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("delete from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}'", txtFeesCode.Text.ToString(), txtBnsCode.Text.ToString());
            if(result.ExecuteNonQuery() == 0)
            {
            }

            /*create table qtr_fee_config_tmp
            (fees_code varchar2(3), 
             * det_buss_code varchar2(4), 
             * rev_year varchar2(4),
             * data_type varchar2(2), 
             * gr_1 float, gr_2 float, amount float,
             * amount2 float, amount3 float, amount4 float);*/

            string strValue1 = string.Empty;
            string strValue2 = string.Empty;
            string strValue3 = string.Empty;
            string strValue4 = string.Empty;
            string strValue5 = string.Empty;
            string strValue6 = string.Empty;
            int intValueCtr = 0;
            string strCompCode = string.Empty;

            strCompCode = cmbCompare.Text.ToString();

            for(int intRow = 0; intRow < dgvFees.Rows.Count; intRow++)
            {
                //if(dgvFees[0, intRow].Value != null)
                if (dgvFees[0, intRow].Value != null && dgvFees[1, intRow].Value != null
                    && dgvFees[2, intRow].Value != null && dgvFees[3, intRow].Value != null
                    && dgvFees[4, intRow].Value != null && dgvFees[5, intRow].Value != null)    // RMC 20111001 corrected log-out in scheds
                {
                    // RMC 20111001 corrected log-out in scheds (s)
                    if (dgvFees[0, intRow].Value.ToString().Trim() == "")
                        strValue1 = "0.00";
                    else
                        strValue1 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[0,intRow].Value.ToString()));

                    if(dgvFees[1,intRow].Value.ToString().Trim() == "")
                        strValue2 = "0.00";
                    else
                        strValue2 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[1,intRow].Value.ToString()));

                    if(dgvFees[2,intRow].Value.ToString().Trim() == "")
                        strValue3 = "0.00";
                    else
                        strValue3 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[2,intRow].Value.ToString()));

                    if(dgvFees[3,intRow].Value.ToString().Trim() == "")
                        strValue4 = "0.00";
                    else
                        strValue4 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[3,intRow].Value.ToString()));

                    if(dgvFees[4,intRow].Value.ToString().Trim() == "")
                        strValue5 = "0.00";
                    else
                        strValue5 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[4,intRow].Value.ToString()));

                    if(dgvFees[5,intRow].Value.ToString().Trim() == "")
                        strValue6 = "0.00";
                    else
                        strValue6 = string.Format("{0:##.00}", Convert.ToDouble(dgvFees[5,intRow].Value.ToString()));
                    // RMC 20111001 corrected log-out in scheds (e)

                    if (strValue4 != "0.00" && strValue5 != "0.00" && strValue6 != "0.00")
                    {
                        result.Query = "insert into qtr_fee_config_tmp values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11)";
                        result.AddParameter(":1", txtFeesCode.Text.ToString());
                        result.AddParameter(":2", txtBnsCode.Text.ToString());
                        result.AddParameter(":3", txtRevYear.Text.ToString());
                        result.AddParameter(":4", txtDataType.Text.ToString());
                        result.AddParameter(":5", strValue1);
                        result.AddParameter(":6", strValue2);
                        result.AddParameter(":7", strValue3);
                        result.AddParameter(":8", strValue4);
                        result.AddParameter(":9", strValue5);
                        result.AddParameter(":10", strValue6);
                        result.AddParameter(":11", cmbCompare.Text.ToString());
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        intValueCtr++;
                    }
                }
            }

            if (intValueCtr > 0)
            {
                MessageBox.Show("Configuration saved.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnClose.Text = "Close";
                this.btnAdd.Enabled = false;
            }
            else
            {
                //MessageBox.Show("No values to save.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // RMC 20110824 (s)
                if (MessageBox.Show("No values to save. Continue?", "Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)  
                {
                    this.btnClose.Text = "Close";
                    this.btnAdd.Enabled = false;
                }   // RMC 20110824 (e)
                else
                    return;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //if(this.btnClose.Text == "Close")
            this.Close();
        }

        private void LoadSavedValue()
        {
            OracleResultSet result = new OracleResultSet();
            string strValue1 = string.Empty;
            string strValue2 = string.Empty;
            string strValue3 = string.Empty;
            string strValue4 = string.Empty;
            string strValue5 = string.Empty;
            string strValue6 = string.Empty;

            result.Query = "select * from qtr_fee_config_tmp where fees_code = :1 and det_buss_code = :2 and rev_year = :3 and data_type = :4 order by gr_1, gr_2";
            result.AddParameter(":1", txtFeesCode.Text.ToString());
            result.AddParameter(":2", txtBnsCode.Text.ToString());
            result.AddParameter(":3", txtRevYear.Text.ToString());
            result.AddParameter(":4", txtDataType.Text.ToString());
            if (result.Execute())
            {
                int intRow = 0;
                double dblValue1 = 0;
                double dblValue2 = 0;
                double dblAmount1 = 0;
                double dblAmount2 = 0;
                double dblAmount3 = 0;
                double dblAmount4 = 0;

                while (result.Read())
                {
                    dblValue1 = result.GetDouble("gr_1");
                    dblValue2 = result.GetDouble("gr_2");
                    dblAmount1 = result.GetDouble("amount");
                    dblAmount2 = result.GetDouble("amount2");
                    dblAmount3 = result.GetDouble("amount3");
                    dblAmount4 = result.GetDouble("amount4");

                    if (txtDataType.Text.ToString() == "QR")
                    {
                        strValue1 = string.Format("{0:#,##}", dblValue1);
                        strValue2 = string.Format("{0:#,##}", dblValue2);
                        if (strValue1 == "")
                            strValue1 = "0";
                        if (strValue2 == "")
                            strValue2 = "0";
                    }
                    else
                    {
                        strValue1 = string.Format("{0:#,##.00}", dblValue1);
                        strValue2 = string.Format("{0:#,##.00}", dblValue2);

                        if (strValue1 == "")
                            strValue1 = "0.00";
                        if (strValue2 == "")
                            strValue2 = "0.00";
                    }
                    strValue3 = string.Format("{0:#,##.00}", dblAmount1);

                    strValue4 = string.Format("{0:#,##.00}", dblAmount2);
                    strValue5 = string.Format("{0:#,##.00}", dblAmount3);
                    strValue6 = string.Format("{0:#,##.00}", dblAmount4);

                    cmbCompare.Text = result.GetString("com_buss_code");

                    dgvFees.Rows.Add("");
                    dgvFees[0, intRow].Value = strValue1;
                    dgvFees[1, intRow].Value = strValue2;
                    dgvFees[2, intRow].Value = strValue3;
                    dgvFees[3, intRow].Value = strValue4;
                    dgvFees[4, intRow].Value = strValue5;
                    dgvFees[5, intRow].Value = strValue6;

                    intRow++;
                }
            }
            result.Close();
        }

        private void dgvFees_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Delete entire row?", "Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgvFees.Rows.RemoveAt(dgvFees.SelectedCells[0].RowIndex);
                }
            }
        }
    }
}