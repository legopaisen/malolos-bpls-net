using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Common.AddlInfo
{
    public partial class frmAddlInfo : Form
    {
        AddlInfo AddlInfo = new AddlInfo();
        private string m_strSource = string.Empty;
        private string m_strBnsCode = string.Empty;
        private string m_strRevYear = string.Empty;
        private string m_strBIN = string.Empty;
        private string m_strTaxYear = string.Empty;
        private string m_strTempBIN = string.Empty;
        private string m_strPrevValue = string.Empty;
        private string m_sNumberOfEmployee = string.Empty;  // RMC 20110810
        private string m_sBusinessArea = string.Empty;  // RMC 20110810
        private string m_sVehicle = string.Empty;   // RMC 20110810

        public string SourceClass
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public string BnsCode
        {
            get { return m_strBnsCode; }
            set { m_strBnsCode = value; }
        }

        public string RevYear
        {
            get { return m_strRevYear; }
            set { m_strRevYear = value; }
        }

        public string BIN
        {
            get { return m_strBIN; }
            set { m_strBIN = value; }
        }

        public string TaxYear
        {
            get { return m_strTaxYear; }
            set { m_strTaxYear = value; }
        }

        public string TempBIN
        {
            get { return m_strTempBIN; }
            set { m_strTempBIN = value; }
        }

        public string BusinessArea  // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sBusinessArea; }
            set { m_sBusinessArea = value; }

        }

        public string EmployeeNo    // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sNumberOfEmployee; }
            set { m_sNumberOfEmployee = value; }
        }

        public string VehicleNo    // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sVehicle; }
            set { m_sVehicle = value; }
        }
                
        public frmAddlInfo()
        {
            InitializeComponent();
        }

        private void frmGrid_Load(object sender, EventArgs e)
        {
            AddlInfo.BIN = m_strBIN;
            AddlInfo.TaxYear = m_strTaxYear;
            AddlInfo.BusinessCode = m_strBnsCode;
            dgvGrid.DataSource = AddlInfo.GetAddlInfo();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // RMC 20110810 (s)
            m_sBusinessArea = AddlInfo.BusinessArea;
            m_sNumberOfEmployee = AddlInfo.EmployeeNo;
            m_sVehicle = AddlInfo.VehicleNo;
            // RMC 20110810 (e)
            this.Close();   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes?", "Additional Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Code", typeof(String));
                dataTable.Columns.Add("Description", typeof(String));
                dataTable.Columns.Add("Type", typeof(String));
                dataTable.Columns.Add("Unit", typeof(String));

                string sDefaultCode = string.Empty;
                string sDefaultDesc = string.Empty;
                string sDataType = string.Empty;
                string sUnit = string.Empty;

                for (int i = 0; i < dgvGrid.Rows.Count-1; i++)
                {
                    sDefaultCode = dgvGrid[0, i].Value.ToString();
                    sDefaultDesc = dgvGrid[1, i].Value.ToString();
                    sDataType = dgvGrid[2, i].Value.ToString();
                    sUnit = dgvGrid[3, i].Value.ToString();

                    dataTable.Rows.Add(sDefaultCode, sDefaultDesc, sDataType, sUnit);
                }

                if (m_strBIN == "")
                {
                    DateTime dtCurrent = AppSettingsManager.GetSystemDate();
                    dtCurrent.Year.ToString();

                    m_strTempBIN = string.Format("{0:0000#}-{1:00#}-{2:00#}-{3:00#}:{4:00#}:{5:00#}", dtCurrent.Year.ToString(), dtCurrent.Month.ToString(), dtCurrent.Day.ToString(), dtCurrent.Hour.ToString(), dtCurrent.Minute.ToString(), dtCurrent.Second.ToString());
                    AddlInfo.BIN = m_strTempBIN;
                }
                else
                    AddlInfo.BIN = m_strBIN;

                AddlInfo.TaxYear = m_strTaxYear;
                AddlInfo.BusinessCode = m_strBnsCode;
                AddlInfo.UpdateAddlInfo(dataTable);

                // RMC 20110810 (s)
                m_sBusinessArea = AddlInfo.BusinessArea;
                m_sNumberOfEmployee = AddlInfo.EmployeeNo;
                m_sVehicle = AddlInfo.VehicleNo;
                this.Close();
                // RMC 20110810 (e)
            }
        }

        private void dgvGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strValue = string.Empty;
            string strType = string.Empty;

            
            if (dgvGrid[e.ColumnIndex, e.RowIndex].Value != null)
            {
                try
                {
                    strValue = dgvGrid[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                }
                catch
                {
                    MessageBox.Show("Error in Field\nShould be numeric", "Additional Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dgvGrid[e.ColumnIndex, e.RowIndex].Value = "0.00";
                    return;
                }

                double d;
                d = Convert.ToDouble(strValue);

                strType = dgvGrid[2, e.RowIndex].Value.ToString().Trim();

                if (strType == "A" || strType == "AR" || strType == "RR")
                    m_strPrevValue = string.Format("{0:##.00}", d);
                else
                    m_strPrevValue = string.Format("{0:0}", d);

                dgvGrid[e.ColumnIndex, e.RowIndex].Value = m_strPrevValue;
            }
        }

        private void dgvGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_strPrevValue = dgvGrid[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();
        }

        private void dgvGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // RMC 20110801 (s)
            if (e.ColumnIndex == 3)
                dgvGrid.ReadOnly = false;
            else
                dgvGrid.ReadOnly = true;
            // RMC 20110801 (e)
        }
    }
}