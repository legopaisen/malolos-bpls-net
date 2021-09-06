
// RMC 20120111 corrected deleting line in schedule

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public partial class frmSchedule : Form
    {
        private string m_strSource = string.Empty;
        private Schedule ScheduleClass = null;
        private string m_strGridPrevValue = string.Empty;
        private string m_strGridPrevNewValue = string.Empty; //AFM 20200915
        private string m_strGridPrevValueCode = string.Empty; //AFM 20200915
        
        public string SourceClass
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string RevYear
        {
            get { return txtRevYear.Text; }
            set { txtRevYear.Text = value; }
        }

        public TextBox TextRevYear
        {
            get { return txtRevYear; }
            set { txtRevYear = value; }
        }

        public Label LabelOne
        {
            get { return lblOne; }
            set { lblOne = value; }
        }

        public Label Label12
        {
            get { return label12; }
            set { label12 = value; }
        }

        public DataGridView ListOne
        {
            get { return this.dgvListOne; }
            set { this.dgvListOne = value; }
        }

        public Label LabelTwo
        {
            get { return lbltwo; }
            set { lbltwo = value; }
        }

        public Label LabelNewRate
        {
            get { return lblNewRate; }
            set { lblNewRate = value; }
        }

        public Label LabelNewMinTax
        {
            get { return lblNewMinTax; }
            set { lblNewMinTax = value; }
        }

        public Label LabelNew
        {
            get { return lblNew; }
            set { lblNew = value; }
        }

        public DataGridView ListTwo
        {
            get { return this.dgvListtwo; }
            set { this.dgvListtwo = value; }
        }

        public string BnsCode
        {
            get { return txtBnsCode.Text; }
            set { txtBnsCode.Text = value; }
        }

        public TextBox TextBnsCode
        {
            get { return txtBnsCode; }
            set { txtBnsCode = value; }
        }

        public ComboBox ComboBnsDesc
        {
            get { return this.cmbBnsDesc; }
            set { this.cmbBnsDesc = value; }
        }

        public string FeesCode
        {
            get { return txtFeesCode.Text; }
            set { txtFeesCode.Text = value; }
        }

        public TextBox TextFeesCode
        {
            get { return txtFeesCode; }
            set { txtFeesCode = value; }
        }

        public string MinTax
        {
            get { return txtMinTax.Text; }
            set { txtMinTax.Text = value; }
        }

        public TextBox TextMinTax
        {
            get { return txtMinTax; }
            set { txtMinTax = value; }
        }

        public string MaxTax
        {
            get { return txtMaxTax.Text; }
            set { txtMaxTax.Text = value; }
        }

        public TextBox TextMaxTax
        {
            get { return txtMaxTax; }
            set { txtMaxTax = value; }
        }

        public Label LabelMinTax
        {
            get { return lblMinTax; }
            set { lblMinTax = value; }
        }

        public Label LabelMaxTax
        {
            get { return lblMaxTax; }
            set { lblMaxTax = value; }
        }

        public ComboBox ComboFeesDesc
        {
            get { return this.cmbFeesDesc; }
            set { this.cmbFeesDesc = value; }
        }

        public string FeesDesc
        {
            get { return cmbFeesDesc.Text; }
            set { cmbFeesDesc.Text = value; }
        }

        public CheckBox CheckLicense
        {
            get {return this.chkLicense;}
            set {this.chkLicense = value;}
        }

        public CheckBox CheckFees
        {
            get { return this.chkFees; }
            set { this.chkFees = value; }
        }

        public CheckBox CheckGross
        {
            get { return this.chkLicGross; }
            set { this.chkLicGross = value; }
        }

        public CheckBox CheckFeesQtr
        {
            get { return this.chkFeesQtr; }
            set { this.chkFeesQtr = value; }
        }

        public CheckBox CheckFeesMonth
        {
            get { return this.chkFeesMonth; }
            set { this.chkFeesMonth = value; }
        }

        public CheckBox CheckFeesYear
        {
            get { return this.chkFeesYear; }
            set { this.chkFeesYear = value; }
        }

        public CheckBox CheckFeesInt
        {
            get { return this.chkFeesInt; }
            set { this.chkFeesInt = value; }
        }

        public CheckBox CheckFeesSurch
        {
            get { return this.chkFeesSurch; }
            set { this.chkFeesSurch = value; }
        }

        public TextBox TextConfig
        {
            get { return this.txtMeans; }
            set { this.txtMeans = value; }
        }

        public string Means
        {
            get { return txtMeans.Text; }
            set {txtMeans.Text = value;}
        }

        public KryptonButton ButtonConfig
        {
            get { return this.btnConfig; }
            set { this.btnConfig = value; }
        }

        public TextBox TextLicNewRate
        {
            get { return this.txtLicNewRate; }
            set { this.txtLicNewRate = value; }
        }

        public string LicNewRate
        {
            get { return txtLicNewRate.Text; }
            set { txtLicNewRate.Text = value; }
        }

        public TextBox TextLicMinTax
        {
            get { return this.txtLicMinTax; }
            set { this.txtLicMinTax = value; }
        }

        public string LicMinTax
        {
            get { return txtLicMinTax.Text; }
            set { txtLicMinTax.Text = value; }
        }

        public CheckBox CheckLicQtrDec
        {
            get { return this.chkLicQtrDec; }
            set { this.chkLicQtrDec = value; }
        }

        public TextBox TextSurcharge
        {
            get { return this.txtSurcharge; }
            set { this.txtSurcharge = value; }
        }

        public string Surcharge
        {
            get { return txtSurcharge.Text; }
            set { txtSurcharge.Text = value; }
        }

        public TextBox TextInterest
        {
            get { return this.txtInterest; }
            set { this.txtInterest = value; }
        }

        public string Interest
        {
            get { return txtInterest.Text; }
            set { txtInterest.Text = value; }
        }

        public TextBox TextDiscount
        {
            get { return this.txtDiscRate; }
            set { this.txtDiscRate = value; }
        }

        public string Discount
        {
            get { return txtDiscRate.Text; }
            set { txtDiscRate.Text = value; }
        }

        public KryptonButton ButtonAdd
        {
            get { return this.btnAdd; }
            set { this.btnAdd = value; }
        }

        public KryptonButton ButtonEdit
        {
            get { return this.btnEdit; }
            set { this.btnEdit = value; }
        }

        public KryptonButton ButtonDelete
        {
            get { return this.btnDelete; }
            set { this.btnDelete = value; }
        }

        public KryptonButton ButtonClose
        {
            get { return this.btnClose; }
            set { this.btnClose = value; }
        }

        public string Excess
        {
            get { return txtExcess.Text; }
            set { txtExcess.Text = value; }
        }

        public TextBox TextExcess
        {
            get { return this.txtExcess; }
            set { this.txtExcess = value; }
        }

        public string AddExcess
        {
            get { return txtAddEx.Text; }
            set { txtAddEx.Text = value; }
        }

        public string SelectedBnsSubCode
        {
            get { return txtTmpSubCode.Text; }
            set { txtTmpSubCode.Text = value; }
        }

        public string SelectedMeans
        {
            get { return txtTmpMeans.Text; }
            set { txtTmpMeans.Text = value; }
        }

        public string SelectedBnsSubCat
        {
            get { return txtTmpSubCat.Text; }
            set { txtTmpSubCat.Text = value; }
        }

        public TextBox TextAddExcess
        {
            get { return this.txtAddEx; }
            set { this.txtAddEx = value; }
        }

        public KryptonButton ButtonConfigQtr
        {
            get { return this.btnConfigQtr; }
            set { this.btnConfigQtr = value; }
        }

        public frmSchedule()
        {
            InitializeComponent();
        }
                
        private void frmSchedule_Load(object sender, EventArgs e)
        {
            ScheduleClass = new Schedule(this);
            this.ScheduleClass.InitialFormLoad();
            this.chkLicense.Checked = true;
        }

        private void FormLoad()
        {
            if (this.SourceClass == "License")
                ScheduleClass = new License(this);
            else if (this.SourceClass == "Fees")
                ScheduleClass = new Fees(this);
            else if (this.SourceClass == "Fire")
                ScheduleClass = new Fire(this);

            this.ScheduleClass.FormLoad();
        }

        private void chkLicense_CheckedChanged(object sender, EventArgs e)
        {
            

            
        }

        private void chkFees_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkAddtl_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void cmbBnsDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ScheduleClass.SelChangeBnsDesc();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (TaskMan.IsObjectLock("SCHEDULE", "SCHEDULE", "DELETE", "ASS"))
                this.ScheduleClass.Close();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ScheduleClass.Add();
        }

        private void cmbBnsDesc_Leave(object sender, EventArgs e)
        {
            string strTemp = string.Empty;

            strTemp = cmbBnsDesc.Text;
            cmbBnsDesc.Text = strTemp.ToString().ToUpper();
        }

        private void chkLicGross_CheckedChanged(object sender, EventArgs e)
        {
            this.ScheduleClass.ClearLists();
        } 
        private void dgvListOne_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
            this.ScheduleClass.ClickListOne(e.RowIndex, e.ColumnIndex);
        }

        private void dgvListOne_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.ScheduleClass.ClickListOne(e.RowIndex, e.ColumnIndex);
        }


        private void dgvListOne_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = e.Control as ComboBox;
            if (combo != null)
            {
                // Remove an existing event-handler, if present, to avoid 
                // adding multiple handlers when the editing control is reused.
                combo.SelectedIndexChanged -=
                    new EventHandler(ComboBox_SelectedIndexChanged);

                // Add the event handler. 
                combo.SelectedIndexChanged +=
                    new EventHandler(ComboBox_SelectedIndexChanged);
                
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  this.ScheduleClass.ListOneComboEdit(((ComboBox)sender).SelectedItem.ToString());
            
           
        }

        private void dgvListtwo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.ScheduleClass.ListTwoEndEdit(e.ColumnIndex, e.RowIndex, m_strGridPrevValue);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void dgvListtwo_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_strGridPrevValue = string.Empty;
            if(dgvListtwo[e.ColumnIndex, e.RowIndex].Value != null)
                m_strGridPrevValue = dgvListtwo[e.ColumnIndex, e.RowIndex].Value.ToString();
            
        }
        

        private void dgvListOne_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.ScheduleClass.ClickListOne(e.RowIndex, e.ColumnIndex);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private bool ValidationUsage() //AFM 20200915
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            result.Query = "select * from businesses where bns_code = '" + m_strGridPrevValueCode + "'";
            if(result.Execute())
                if(result.Read())
                {
                    MessageBox.Show("Cannot edit value! Record already has existing transaction. Please contact support.");
                    result.Close();
                    return false;
                }
                else
                {
                    result2.Query = "select * from business_que where bns_code = '" + m_strGridPrevValueCode + "'";
                    if(result.Execute())
                        if(result.Read())
                        {
                            MessageBox.Show("Cannot edit value! Record already has existing transaction. Please contact support.");
                            result2.Close();
                            return false;
                        }
                }
            return true;
        }

        private void dgvListOne_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //AFM 20200915 MAO-20-13622 (s)
            try
            {
                m_strGridPrevNewValue = dgvListOne[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            catch 
            {
                m_strGridPrevNewValue = "";
            }
            if(m_strGridPrevNewValue.Trim() != m_strGridPrevValue.Trim())
            {
                if (!ValidationUsage())
                {
                    dgvListOne[e.ColumnIndex, e.RowIndex].Value = m_strGridPrevValue;
                    return;
                }
                else if(m_strGridPrevNewValue == "")
                    dgvListOne[e.ColumnIndex, e.RowIndex].Value = m_strGridPrevValue;
            }
            //AFM 20200915 MAO-20-13622 (e)
            try
            {
                this.ScheduleClass.ListOneComboEdit(m_strGridPrevValue);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try //GMC 20150826 Empty instanct of an Object
            {
                this.ScheduleClass.ButtonConfig();
            }catch{}
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkFees_CheckStateChanged(object sender, EventArgs e)
        {
            
            if (this.chkFees.CheckState.ToString() == "Checked")
            {
                m_strSource = "Fees";
                //this.chkFees.Checked = true;
                this.chkLicense.Checked = false;
                this.FormLoad();
            }
            
        }

        private void chkLicense_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.chkLicense.CheckState.ToString() == "Checked")
            {
                m_strSource = "License";
                //this.chkLicense.Checked = true;
                this.chkFees.Checked = false;
                this.FormLoad();
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.ScheduleClass.Edit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ScheduleClass.Delete();
        }

        private void txtLicMinTax_TextChanged(object sender, EventArgs e)
        {

        }

        private void containerWithShadow2_Load(object sender, EventArgs e)
        {

        }

        private void cmbFeesDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ScheduleClass.SelChangeFeesDesc();
        }

        private void txtTmpSubCode_TextChanged(object sender, EventArgs e)
        {
            this.ScheduleClass.ChangedTmpSubCode();
        }

        private void dgvListtwo_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnAdd.Text == "&Save" || btnEdit.Text == "&Update")
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (MessageBox.Show("Delete entire row?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dgvListtwo.Rows.RemoveAt(dgvListtwo.SelectedCells[0].RowIndex);
                        ScheduleClass.ListTwoEndEdit(dgvListtwo.SelectedCells[0].ColumnIndex, dgvListtwo.SelectedCells[0].RowIndex, "");
                        
                    }
                }
                
                if (e.KeyCode == Keys.Tab)
                {
                    if (chkLicGross.Checked == true)
                    {
                        int intTempRow = dgvListtwo.SelectedCells[0].RowIndex;
                        int intTempCol = dgvListtwo.SelectedCells[0].ColumnIndex;

                        if (intTempCol == 1)
                        {
                            dgvListtwo.CurrentCell = GetNextCell(dgvListtwo.CurrentCell);
                            e.Handled = true;
                            
                        }
                    }
                }
            }

        }

        public DataGridViewCell GetNextCell(DataGridViewCell currentCell) 
        { 
            int i = 0; 
            DataGridViewCell nextCell = currentCell; 

            do {
                int nextCellIndex = (nextCell.ColumnIndex + 3) % dgvListtwo.ColumnCount;
                int nextRowIndex = nextCellIndex == 0 ? (nextCell.RowIndex) % dgvListtwo.RowCount : nextCell.RowIndex;
                nextCell = dgvListtwo.Rows[nextRowIndex].Cells[nextCellIndex]; 
                i++; 
            }
            while (i < dgvListtwo.RowCount * dgvListtwo.ColumnCount && nextCell.ReadOnly);          
            
            return nextCell; 
        } 

        private void txtTmpMeans_TextChanged(object sender, EventArgs e)
        {
            this.ScheduleClass.ChangedMeans();
        }

        private void dgvListOne_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnAdd.Text == "&Save" || btnEdit.Text == "&Update")
            {
                if (e.KeyCode == Keys.Delete)
                {
                    ScheduleClass.DeleteSubBusiness(dgvListOne.SelectedCells[0].RowIndex);
                }

                /*if (e.KeyCode == Keys.Tab)
                {
                    dgvListtwo.Focus();
                }*/
            }
        }

        private void dgvListOne_MouseDown(object sender, MouseEventArgs e)
        {
            if (btnAdd.Text == "&Save" || btnEdit.Text == "&Update")
            {
                switch (e.Button)
                {
                    /*case MouseButtons.Left:
                        MessageBox.Show(this, "Left Button Click"); 
                        break;*/
                    case MouseButtons.Right:
                        DataGridView grid = sender as DataGridView;
                        ContextMenuStrip menu = new ContextMenuStrip();
                        menu.Items.Add("Delete", null, new EventHandler(contextMenuStrip1_Click));
                        Point pt = grid.PointToClient(Control.MousePosition);
                        menu.Show(dgvListOne, pt.X, pt.Y);
                        break;
                    /*case MouseButtons.Middle:
                        break;*/
                    default:
                        break;
                }
            }
        }

        private void ShowContextMenu()
        {
            
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            int intTempRow = dgvListOne.SelectedCells[0].RowIndex;

            ScheduleClass.DeleteSubBusiness(intTempRow);
            //dgvListOne.Rows.RemoveAt(dgvListOne.SelectedCells[0].RowIndex);
        }

        private void dgvListtwo_MouseDown(object sender, MouseEventArgs e)
        {
            if (btnAdd.Text == "&Save" || btnEdit.Text == "&Update")
            {
                switch (e.Button)
                {
                    /*case MouseButtons.Left:
                        MessageBox.Show(this, "Left Button Click");
                        break;*/
                    case MouseButtons.Right:
                        DataGridView grid = sender as DataGridView;
                        ContextMenuStrip menu = new ContextMenuStrip();
                        menu.Items.Add("Insert Row", null, new EventHandler(contextMenuStrip2_Click));
                        Point pt = grid.PointToClient(Control.MousePosition);
                        menu.Show(dgvListtwo, pt.X, pt.Y);
                        break;
                    /*case MouseButtons.Middle:
                        break;*/
                    default:
                        break;
                }
            }
        }
               

        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            int intTempRow = dgvListtwo.SelectedCells[0].RowIndex;

            dgvListtwo.Rows.Insert(intTempRow, "");
        }

        private void cmbFeesDesc_Leave(object sender, EventArgs e)
        {
            string strTemp = string.Empty;

            strTemp = cmbFeesDesc.Text;
            cmbFeesDesc.Text = strTemp.ToString().ToUpper();

            if (btnAdd.Text == "&Save" || btnEdit.Text == "&Update")
            {
            }
            else
            {
                txtFeesCode.Text = AppSettingsManager.GetFeesCodeByDesc(cmbFeesDesc.Text.Trim(), "FS");
                if (txtFeesCode.Text == "")
                    cmbFeesDesc.Text = "";

                this.ScheduleClass.SelChangeFeesDesc();
            }
        }

        private void txtLicMinTax_Leave(object sender, EventArgs e)
        {
            string strTemp = string.Empty;

            strTemp = txtLicMinTax.Text;

            txtLicMinTax.Text = string.Format("{0:##0.00}", Convert.ToDouble(strTemp));
        }

        private void chkFeesMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFeesMonth.Checked == true)
            {
                chkFeesQtr.Checked = false;
                chkFeesYear.Checked = false;
            }
        }

        private void chkFeesQtr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFeesQtr.Checked == true)
            {
                chkFeesMonth.Checked = false;
                chkFeesYear.Checked = false;
            }
        }

        private void chkFeesYear_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFeesYear.Checked == true)
            {
                chkFeesMonth.Checked = false;
                chkFeesQtr.Checked = false;
            }
        }

        private void btnConfigQtr_Click(object sender, EventArgs e)
        {
            /*if (chkFeesQtr.Checked == true)
            {
                ScheduleClass.ButtonQuarterConfig();
            }*/
            ScheduleClass.ButtonQuarterConfig();// GMC 20150826
        }

        private void txtAddEx_Leave(object sender, EventArgs e)
        {
            // RMC 20130107
            try
            {
                if (dgvListtwo[1, 0].Value.ToString() != "")
                    //ScheduleClass.ListTwoEndEdit(0, 1, "");
                    ScheduleClass.ListTwoEndEdit(0, 0, ""); // RMC 20161213 mods in schedule
            }
            catch { }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // RMC 20150117 
            if (AppSettingsManager.Granted("AUTPS"))  // RMC 20110809
            {
                //if (!TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "ADD", "ASS"))    // RMC 20111005 changed object locking
                {
                    using (frmPrintSchedule frmPrintSchedule = new frmPrintSchedule())
                    {
                        frmPrintSchedule.ShowDialog();

                        //if (TaskMan.IsObjectLock("PRINT SCHEDULE", "SCHEDULE", "DELETE", "ASS"))  // RMC 20111005 
                        {
                        }
                    }
                }

               
            }
        }

        private void txtMinTax_Leave(object sender, EventArgs e)
        {
            // RMC 20150305 corrections in schedules module
            ScheduleClass.EditMinMax();
        }

        private void txtMaxTax_Leave(object sender, EventArgs e)
        {
            // RMC 20150305 corrections in schedules module
            ScheduleClass.EditMinMax();
        }

        private void dgvListOne_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // RMC 20161216 corrected data grid view error in schedule
            m_strGridPrevValue = string.Empty;
            m_strGridPrevValueCode = string.Empty;
            if(dgvListOne[e.ColumnIndex, e.RowIndex].Value != null)
            {
                m_strGridPrevValue = dgvListOne[e.ColumnIndex, e.RowIndex].Value.ToString();
                m_strGridPrevValueCode = dgvListOne[0, e.RowIndex].Value.ToString(); //AFM 20200915
            }
        }

        private void containerWithShadow1_Load(object sender, EventArgs e)
        {

        }

    }
}