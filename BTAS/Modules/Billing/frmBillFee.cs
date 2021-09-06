using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.TextValidation;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.BPLS.Billing
{
    public partial class frmBillFee : Form
    {
        private string m_sBIN, m_sBnsCode, m_sFeesCode, m_sTaxYear, m_sQtr, m_sStatus, m_sRevYear, _moduleswitch;//peb 20191218
        private double m_fCapital, m_fGross, m_dTotalDue;
        private Fee FeeClass = null;
        private SimpleTextValidator m_objNumberValidator;
        private MoneyTextValidator m_objMoneyValidator;
        private int m_iRowIndex;
        private string _brgy;
        public string moduleswitch//peb 20191218
        {
            get { return _moduleswitch; }
            set { _moduleswitch = value; }
        }
        public string brgy//peb 20191218
        {
            get { return _brgy; }
            set { _brgy = value; }
        }

        public frmBillFee()
        {
            InitializeComponent();
            m_objNumberValidator = new SimpleTextValidator();
            m_objMoneyValidator = new MoneyTextValidator();
            m_objNumberValidator.SetUnsignedIntCharacterSet();
        }

        public Fee SourceClass
        {
            set { FeeClass = value; }

        }

        public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string BusinessCode
        {
            get { return m_sBnsCode; }
            set { m_sBnsCode = value; }
        }

        public string Status
        {
            get { return m_sStatus; }
            set { m_sStatus = value; }
        }

        public string FeesCode
        {
            get { return m_sFeesCode; }
            set { m_sFeesCode = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }
        public string Quarter
        {
            get { return m_sQtr; }
            set { m_sQtr = value; }
        }


        public string RevisionYear
        {
            get { return m_sRevYear; }
            set { m_sRevYear = value; }
        }

        public double Capital
        {
            get { return m_fCapital; }
            set { m_fCapital = value; }
        }

        public double Gross
        {
            get { return m_fGross; }
            set { m_fGross = value; }
        }

        public double TotalDue
        {
            get { return m_dTotalDue; }
            set { m_dTotalDue = value; }
        }

        public DataGridView Fees
        {
            get { return dgvBillFee; }
            set { dgvBillFee = value; }
        }

        public int RowIndex
        {
            get { return m_iRowIndex; }
            set { m_iRowIndex = value; }
        }


        public Label LabelInput
        {
            get { return labelInput; }
            set { labelInput = value; }
        }

        public TextBox Quantity
        {
            get { return txtEnterQty; }
            set { txtEnterQty = value; }
        }

        public TextBox Amount
        {
            get { return txtEnterAmt; }
            set { txtEnterAmt = value; }
        }

        public Button ButtonCompute
        {
            get { return btnCompute; }
            set { btnCompute = value; }
        }

        private void frmBillFee_Load(object sender, EventArgs e)
        {
            FeeClass.UpdateList();
            this.txtTotalDue.Text = string.Format("{0:#,##0.00}", FeeClass.Total());
            double.TryParse(txtTotalDue.Text, out m_dTotalDue);

            //if (moduleswitch == "BARANGAY CLEARANCE FEE")//peb20191218 adding value in datagrid
            //{
            //    dgvBillFee.Rows.Clear();
            //    OracleResultSet result = new OracleResultSet();
            //    result.Query = "select distinct barangay_taxandfeestable.fees_desc,barangay_addl_sched.fees_code ,barangay_addl_sched.amount FROM barangay_taxandfeestable inner join barangay_addl_sched ON  barangay_addl_sched.fees_code=barangay_taxandfeestable.fees_code and (barangay_addl_sched.brgy_nm='" + brgy + "'and barangay_taxandfeestable.barangay ='" + brgy + "' )";
            //    if (result.Execute())
            //    {
            //        if (result.Read())
            //        {
            //            dgvBillFee.Rows.Add(false, result.GetString("fees_desc"), string.Format("{0:#,##0.00}", result.GetInt("amount")), 0);
            //        }
            //    }
            //    result.Close();

            //    OracleResultSet pSet = new OracleResultSet();
            //    OracleResultSet pSet2 = new OracleResultSet();
            //    string sFeesDesc, sFeesCode;
            //    double fAmount;
            //    int iQty;
            //    dgvBillFee.Columns.Clear();
            //    dgvBillFee.Columns.Add(new DataGridViewCheckBoxColumn());
            //    dgvBillFee.Columns.Add("FeesDesc", "Additional Charges");
            //    dgvBillFee.Columns.Add("FeesCode", "Fees Code");
            //    dgvBillFee.Columns.Add("Amount", "Amount");
            //    dgvBillFee.Columns.Add("Qty", "Qty");

            //    dgvBillFee.Columns[0].Width = 20;
            //    dgvBillFee.Columns[0].ReadOnly = false;
            //    dgvBillFee.Columns[1].Width = 350;
            //    dgvBillFee.Columns[1].ReadOnly = true;
            //    dgvBillFee.Columns[2].Width = 80;
            //    dgvBillFee.Columns[2].ReadOnly = true;
            //    dgvBillFee.Columns[2].Visible = false; // hide in ADDL Charges
            //    dgvBillFee.Columns[3].Width = 100;
            //    dgvBillFee.Columns[3].ReadOnly = true;
            //    dgvBillFee.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    dgvBillFee.Columns[4].Width = 80; // Addl Charges
            //    dgvBillFee.Columns[4].ReadOnly = true; // Addl Charges
            //    dgvBillFee.Columns[4].Visible = false;  // Addl Charges          
            //    dgvBillFee.Rows.Clear();
            //    if (moduleswitch == "BARANGAY CLEARANCE FEE")// peb 20191219
            //    {
            //        pSet.Query = "select distinct barangay_taxandfeestable.fees_desc,barangay_addl_sched.fees_code ,barangay_addl_sched.amount FROM barangay_taxandfeestable inner join barangay_addl_sched ON  barangay_addl_sched.fees_code=barangay_taxandfeestable.fees_code and (barangay_addl_sched.brgy_nm='" + brgy + "'  and barangay_taxandfeestable.barangay ='" + brgy + "' )";
            //        if (pSet.Execute())
            //        {
            //            while (pSet.Read())
            //            {
            //                fAmount = 0;
            //                iQty = 0;
            //                sFeesDesc = pSet.GetString("fees_desc").Trim();
            //                sFeesCode = pSet.GetString("fees_code").Trim();
            //                //iQty = pSet.GetInt("qty");
            //                fAmount = pSet.GetDouble("amount");
            //                dgvBillFee.Rows.Add(false, sFeesDesc, sFeesCode, string.Format("{0:#,##0.00}", fAmount), string.Format("{0:#,###}", iQty));
            //            }
            //        }
            //        double totaldue = 0;
            //    }
            //}
        }

        public string fees_code;

        private void dgvBillFee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            if (e.ColumnIndex == 0)
            {
                bool blnIsChecked = false;
                string sCode;
                double fAmount;
                // get the current checkbox status
                blnIsChecked = (bool)dgvBillFee.Rows[e.RowIndex].Cells[0].Value;
                // get the current row index
                m_iRowIndex = e.RowIndex;
                sCode = dgvBillFee.Rows[e.RowIndex].Cells[2].Value.ToString();
                //if (moduleswitch == "BARANGAY CLEARANCE FEE")
                //{
                //    dgvBillFee.Rows[e.RowIndex].Cells[0].Value = true;
                //    fees_code = dgvBillFee.Rows[e.RowIndex].Cells[2].Value.ToString();
                //    pSet.Query = "select fees_term ,fees_type from barangay_taxandfeestable where fees_code='" + fees_code + "' and barangay ='" + brgy + "'";
                //    if (pSet.Execute())
                //    {
                //        if (pSet.Read())
                //        {
                //            //if (pSet.GetString("fees_term") == "Q")
                //            //{
                //            //    btnCompute.Visible = true;
                //            //    labelInput.Visible = true;
                //            //    txtEnterAmt.Visible = true;
                //            //}
                //            if (pSet.GetString("fees_type") == "Q")
                //            {
                //                btnCompute.Visible = true;
                //                labelInput.Visible = true;
                //                txtEnterAmt.Visible = true;
                //            }
                //            //if (fAmount == 0)
                //            //{
                //            //    dgvBillFee.CancelEdit();
                //            //    dgvBillFee.Rows[e.RowIndex].Cells[0].Value = false;
                //            //}
                //        }
                //    }
                //}

                if (AppSettingsManager.GetConfigValue("10") == "216") //FOR MALOLOS ONLY
                {
                    if (moduleswitch != "BARANGAY CLEARANCE FEE") //AFM 20200110 condition to disregard barangay clearance in validation
                    {
                        if (sCode == "17" || sCode == "18") //JARS 20171003 validation for checking of CNC/ECC payments
                        {
                            pSet.Query = "select * from pay_hist where bin = '" + m_sBIN + "' and or_no in (select or_no from or_table where fees_code = '" + sCode + "' and tax_year = '" + m_sTaxYear + "')";
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    MessageBox.Show(dgvBillFee.Rows[e.RowIndex].Cells[1].Value.ToString() + " already paid for in this Tax Year.\nOR Number: " + pSet.GetString("or_no"));
                                    dgvBillFee.CancelEdit();
                                    dgvBillFee.Rows[e.RowIndex].Cells[0].Value = false;
                                    dgvBillFee.Rows[e.RowIndex].Cells[3].Value = "0.00";
                                    return;
                                }
                            }
                            pSet.Close();
                        }
                    }
                }

                if (!blnIsChecked) // compute and checked
                {
                    dgvBillFee.Rows[e.RowIndex].Cells[0].Value = true;
                    if (moduleswitch == "BARANGAY CLEARANCE FEE")
                        fAmount = FeeClass.Compute(sCode, brgy);
                    else
                        fAmount = FeeClass.Compute(sCode);
                    dgvBillFee.Rows[e.RowIndex].Cells[3].Value = string.Format("{0:#,##0.00}", fAmount);
                    if (fAmount == 0)
                    {
                        dgvBillFee.CancelEdit();
                        dgvBillFee.Rows[e.RowIndex].Cells[0].Value = false;
                    }
                }
                else // un-compute and unchecked
                {
                    dgvBillFee.Rows[e.RowIndex].Cells[0].Value = false;
                    dgvBillFee.Rows[e.RowIndex].Cells[3].Value = "0.00";
                }
                this.txtTotalDue.Text = string.Format("{0:#,##0.00}", FeeClass.Total());
            }
        }

        private void dgvBillFee_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dgvBillFee_CellContentClick(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FeeClass.UpdateRows();
            double.TryParse(txtTotalDue.Text, out m_dTotalDue);
            this.Close();
        }

        private void frmBillFee_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txtEnterQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objNumberValidator.HandleKeyPressEvent(sender, e, 10);
        }

        private void txtEnterAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            m_objMoneyValidator.HandleKeyPressEvent(sender, e);
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            //peb 20191227 (s)
            //if (moduleswitch == "BARANGAY CLEARANCE FEE")
            //{
            //    double total;
            //    OracleResultSet result = new OracleResultSet();
            //    result.Query = "select amount from barangay_addl_sched where brgy_nm='" + brgy + "' and fees_code='" + fees_code + "'";
            //    if (result.Execute())
            //    {
            //        if (result.Read())
            //        {
            //            total = (int.Parse(txtEnterAmt.Text) * result.GetDouble("amount"));
            //            dgvBillFee.Rows[m_iRowIndex].Cells[3].Value = string.Format("{0:#,##0.00}", total);
            //            dgvBillFee.Rows[m_iRowIndex].Cells[0].Value = true;
            //            double sum = 0;
            //            for (int i = 0; i < dgvBillFee.Rows.Count; ++i)
            //            {
            //                bool isSelected = Convert.ToBoolean(dgvBillFee.Rows[i].Cells[0].Value);
            //                if (isSelected)
            //                {
            //                    sum += Convert.ToDouble(dgvBillFee.Rows[i].Cells[3].Value);
            //                }
            //            }

            //            txtTotalDue.Text = sum.ToString();
            //        }
            //    }
            //    result.Close();
            //}
            ////peb 20191227 (e)
            //else
            {
                double dAmount;
                dAmount = FeeClass.OnButtonCompute();
                dgvBillFee.Rows[m_iRowIndex].Cells[4].Value = string.Format("{0:0000}", txtEnterQty.Text);
                dgvBillFee.Rows[m_iRowIndex].Cells[3].Value = string.Format("{0:#,##0.00}", dAmount);
                //if (dAmount > 0)
                //    dgvBillFee.Rows[m_iRowIndex].Cells[0].Value = true;  
                //else
                //    dgvBillFee.Rows[m_iRowIndex].Cells[0].Value = false;
                this.txtTotalDue.Text = string.Format("{0:#,##0.00}", FeeClass.Total());
            }
        }

        private void txtTotalDue_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAmount_Click(object sender, EventArgs e)
        {

        }

    }
}