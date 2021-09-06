
// RMC 20110816


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmUnOfficialList : Form
    {
        private bool m_bContinue = true;
        private UnOfficialList ListClass = null;
        protected string m_sSource = string.Empty;

        public bool Continue
        {
            get { return m_bContinue; }
            set { m_bContinue = value; }
        }

        public string InspectionNumber
        {
            get { return txtInspectionNo.Text; }
            set { txtInspectionNo.Text = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public frmUnOfficialList()
        {
            InitializeComponent();
        }

        private void frmUnOfficialList_Load(object sender, EventArgs e)
        {
            if (this.Source == "BUSINESS MAPPING")
            {
                ListClass = new UnOfficialListBTM(this);
                this.Text = "Unencoded businesses (BUSINESS MAPPING DRIVE)";
            }
            else
            {
                ListClass = new UnOfficialListInspection(this);
                // this.Text = "Unofficial Business (BY INSPECTION/DISCOVERED THRU BUSINESS MAPPING DRIVE)"; // RMC 20150425 disabled viewing of controls for business mapping in business record module, put rem

                // RMC 20150425 disabled viewing of controls for business mapping in business record module (s)
                OracleResultSet pGisRec = new OracleResultSet();
                try
                {
                    pGisRec.CreateNewConnectionGIS();
                    this.Text = "Unofficial Business (BY INSPECTION/DISCOVERED THRU BUSINESS MAPPING DRIVE)";
                }
                catch
                {
                    chkTaxMapped.Visible = false;
                    this.Text = "Unofficial Business (BY INSPECTION)";
                }
                // RMC 20150425 disabled viewing of controls for business mapping in business record module (e)                               
                
            }

            ListClass.FormLoad();
        }

        private void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            ListClass.chkAll_CheckStateChanged(sender, e);
            
        }

        private void chkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            ListClass.chkFilter_CheckStateChanged(sender, e);
            
        }

       
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                txtInspectionNo.Text = dgvList[0, e.RowIndex].Value.ToString();
                txtBnsName.Text = dgvList[1, e.RowIndex].Value.ToString();
                txtBnsAdd.Text = dgvList[2, e.RowIndex].Value.ToString();
                txtOwnName.Text = dgvList[3, e.RowIndex].Value.ToString();
                cmbInspector.Text = dgvList[4, e.RowIndex].Value.ToString();
            }
            catch
            {
                txtInspectionNo.Text = "";
                txtBnsName.Text = "";
                txtBnsAdd.Text = "";
                txtOwnName.Text = "";
                cmbInspector.Text = "";
            }

            if (txtInspectionNo.Text != null)
                btnSearch.Text = "Clear";

        }
           

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ListClass.ClearControls();
            m_bContinue = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_bContinue = true;

            if (txtInspectionNo.Text.ToString().Trim() != "")
            {
                if (MessageBox.Show("Continue adding Business: " + txtBnsName.Text.ToString() + "?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtInspectionNo.Text = "";
                }
            }
            else
            {
                if (MessageBox.Show("Do you want to continue application of new business record without inspection details?", "Business Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            this.Close();    
        }
              
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Text == "Search")
            {
                ListClass.ViewList();
                btnSearch.Text = "Clear";
            }
            else
            {
                ListClass.ClearControls();
                //ListClass.ViewList();
            }
        }

        private void chkInspected_CheckStateChanged(object sender, EventArgs e)
        {
            ListClass.chkInspected_CheckStateChanged(sender, e);
        }

        private void chkTaxMapped_CheckStateChanged(object sender, EventArgs e)
        {
            ListClass.chkTaxMapped_CheckStateChanged(sender, e);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        
    }
}