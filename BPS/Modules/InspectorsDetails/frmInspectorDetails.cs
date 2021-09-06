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

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmInspectorDetails : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_strSource = string.Empty;
        private InsDetails DetailsClass = null;
                
        public string Source
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public frmInspectorDetails()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

       
        public void ClearControls()
        {
            this.bin1.GetBINSeries = "";
            this.bin1.GetTaxYear = "";
            this.txtISNo.Text = "";
            this.txtBnsName.Text = "";
            this.txtBnsAdd.Text = "";
            this.cmbBnsBrgy.Text = "";
            this.cmbBnsDist.Text = "";
            this.cmbBnsOrgKind.Text = "";
            this.txtBnsStreet.Text = "";
            this.txtBnsZone.Text = "";
            this.txtBnsCity.Text = "";
            this.txtBnsProv.Text = "";
            this.txtBnsType.Text = "";
            this.txtOwnName.Text = "";
            this.txtOwnAdd.Text = "";
            this.cmbOwnBrgy.Text = "";
            this.cmbOwnDist.Text = "";
            this.txtOwnStreet.Text = "";
            this.txtOwnCity.Text = "";
            this.txtOwnProv.Text = "";
            this.txtOwnZone.Text = "";
            this.txtRemarks.Text = "";
            this.txtAddlRemarks.Text = "";
            this.btnSearch.Text = "Search";
            this.dgvViolations.Columns.Clear(); // RMC 20171116 added violation report
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DetailsClass.Search();
        }

        private void LoadCombo()
        {
            pSet.Query = "SELECT DISTINCT(TRIM(BRGY_NM)) FROM BRGY  ORDER BY TRIM(BRGY_NM) ASC";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbBnsBrgy.Items.Add(pSet.GetString(0).Trim());
                    cmbOwnBrgy.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();

            pSet.Query = "select distinct(dist_nm) from brgy order by dist_nm asc";
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    cmbBnsDist.Items.Add(pSet.GetString(0).Trim());
                    cmbOwnDist.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();

            cmbBnsOrgKind.Items.Add("SINGLE PROPRIETORSHIP");
            cmbBnsOrgKind.Items.Add("PARTNERSHIP");
            cmbBnsOrgKind.Items.Add("CORPORATION");

        }

        private void frmInspectorDetails_Load(object sender, EventArgs e)
        {
            if (this.Source == "Deficient")
            {
                DetailsClass = new Deficient(this);
                this.Text = "Deficient Business";
            }
            else
            {
                DetailsClass = new Unofficial(this);
                this.Text = "Unofficial Business";
            }

            this.DetailsClass.FormLoad();

            LoadCombo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                this.Close();
            else
            {
                this.EnableControls(false);
                this.ClearControls();
                this.btnAdd.Text = "Add";
                this.btnEdit.Text = "Edit";
                this.btnDelete.Text = "Delete";
                this.btnClose.Text = "Close";
                this.btnSearch.Text = "Search";
                

                this.btnPrint.Enabled = true;
                this.btnViolation.Enabled = true;
                this.btnTag.Enabled = true;
                this.btnUntag.Enabled = true;
                this.btnNotice.Enabled = true;
                this.btnAdd.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                
            }
        }

        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DetailsClass.CellClick(e.ColumnIndex, e.RowIndex);
            this.btnUntag.Enabled = true;   //JARS09272015
        }

        private void dgvInspectors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DetailsClass.InspectorsCellClick(e.ColumnIndex, e.RowIndex);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DetailsClass.Add();
        }

        public void EnableControls(bool blnEnable)
        {
            this.bin1.txtTaxYear.ReadOnly = !blnEnable;
            this.bin1.txtBINSeries.ReadOnly = !blnEnable;
            
            this.txtBnsName.ReadOnly = !blnEnable;
            this.txtBnsAdd.ReadOnly = !blnEnable;
            this.cmbBnsBrgy.Enabled = blnEnable;
            this.cmbBnsDist.Enabled = blnEnable;
            this.cmbBnsOrgKind.Enabled = blnEnable;
            this.txtBnsStreet.ReadOnly = !blnEnable;
            this.txtBnsZone.ReadOnly = !blnEnable;
            this.txtBnsCity.ReadOnly = !blnEnable;
            this.txtBnsProv.ReadOnly = !blnEnable;
            this.txtBnsType.ReadOnly = !blnEnable;
            this.txtOwnName.ReadOnly = !blnEnable;
            this.txtOwnAdd.ReadOnly = !blnEnable;
            this.cmbOwnBrgy.Enabled = blnEnable;
            this.cmbOwnDist.Enabled = blnEnable;
            this.txtOwnStreet.ReadOnly = !blnEnable;
            this.txtOwnCity.ReadOnly = !blnEnable;
            this.txtOwnProv.ReadOnly = !blnEnable;
            this.txtOwnZone.ReadOnly = !blnEnable;
            this.txtRemarks.ReadOnly = !blnEnable;
            this.txtAddlRemarks.ReadOnly = !blnEnable;
            this.btnBnsTypes.Enabled = false;

            
            this.dgvDetails.Enabled = !blnEnable;
            this.dgvInspectors.Enabled = !blnEnable;

            this.txtOwnName.Visible = true;
            this.txtFirstName.Visible = false;
            this.txtLastName.Visible = false;
            this.txtMI.Visible = false;
            this.lblFN.Visible = false;
            this.lblMI.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DetailsClass.Edit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DetailsClass.Delete();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DetailsClass.Print();
        }

        private void btnViolation_Click(object sender, EventArgs e)
        {
            DetailsClass.Violation();
        }

        private void btnTag_Click(object sender, EventArgs e)
        {
            DetailsClass.Tag();
        }

        private void btnUntag_Click(object sender, EventArgs e)
        {
            DetailsClass.Untag();
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            DetailsClass.IssueNotice();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DetailsClass.RefreshList();
        }

        private void txtISNo_Leave(object sender, EventArgs e)
        {
            DetailsClass.ValidateISNo();
        }

        private void btnBnsTypes_Click(object sender, EventArgs e)
        {
            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(true);
            frmBnsType.ShowDialog();
            txtBnsType.Text = frmBnsType.m_sBnsDescription;
            
        }

        private void dgvInspectors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}