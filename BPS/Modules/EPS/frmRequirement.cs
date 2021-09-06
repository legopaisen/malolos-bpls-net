using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using Amellar.Common.Reports;

namespace Amellar.Modules.EPS
{
    public partial class frmRequirement : Form
    {
        public frmRequirement()
        {
            InitializeComponent();

        }
        private string m_sModuleCode = string.Empty;

        OracleResultSet result = new OracleResultSet();
        OracleResultSet result1 = new OracleResultSet();

        private void frmRequirement_Load(object sender, EventArgs e)
        {
            EnableControls(false);
            getValue();
        }

        private void EnableControls(bool blnEnable)
        {
            txtReq_Description.Enabled = blnEnable;
            txtReq_Id.Enabled = blnEnable;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "&Add")
            {
                EnableControls(true);
                dgvRequirements.ClearSelection();
                ClearFields();
                btnSave.Text = "&Save";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Text = "&Cancel";
                GenerateCode();
                txtReq_Description.Focus();
            }
            else
            {
                if (MessageBox.Show("Save?", " Check List", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (txtReq_Description.Text == "")
                        MessageBox.Show("Description is empty", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        SaveReq("Add");
                        MessageBox.Show("Record saved", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EnableControls(false);
                        btnSave.Text = "&Add";
                        btnEdit.Enabled = true;
                        btnDelete.Enabled = true;
                        btnClose.Text = "&Close";
                        getValue();
                    }
                }

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "&Cancel")
            {
                dgvRequirements.ClearSelection();
                btnClose.Text = "&Close";
                btnSave.Text = "&Add";
                btnEdit.Text = "&Edit";
                EnableControls(false);
                ClearFields();
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
                this.Close();
        }

        private void GenerateCode()
        {
            string sCode = "";
            result.Query = "SELECT * FROM EPS_TBL order by req_code desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sCode = result.GetString("req_code");
                    sCode = string.Format("{0:00#}", Convert.ToInt32(sCode) + 1);
                }
                else
                {
                    sCode = "001";
                }
            }
            result.Close();

            txtReq_Id.Text = sCode;
        }

        private void SaveReq(string Transaction)
        {
            OracleResultSet result = new OracleResultSet();
            string ReqCode = "";
            string ReqDesc = "";
            List<string> m_lstBnsStat = new List<string>();
            string BnStat = "";

            ReqCode = txtReq_Id.Text.Trim();
            ReqDesc = txtReq_Description.Text.Trim();

            string sQuery = string.Format("Insert Into EPS_TBL values('{0}','{1}')", ReqCode, StringUtilities.HandleApostrophe(ReqDesc));
            result.Query = sQuery;
            result.Execute();

            string strObj = "";
            if (Transaction == "Add")
            {
                strObj = "Added Rqmt. ChkList: " + txtReq_Id.Text.Trim() + " / Desc: " + txtReq_Description.Text.Trim();
                m_sModuleCode = "AUEC-A";

            }
            else
            {
                strObj = "Edited Rqmt. ChkList: " + txtReq_Id.Text.Trim() + " / Desc: " + txtReq_Description.Text.Trim();
                m_sModuleCode = "AUEC-E";
            }

            if (AuditTrail.InsertTrail(m_sModuleCode, "eps_tbl", StringUtilities.HandleApostrophe(strObj)) == 0)
            {
                result.Rollback();
                result.Close();
                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            result.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnEdit.Text == "&Edit")
            {
                if (txtReq_Description.Text.Trim() == "")
                {
                    MessageBox.Show("Select requirement description to edit", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                EnableControls(true);
                btnEdit.Text = "&Update";
                btnClose.Text = "&Cancel";
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                txtReq_Description.Focus();
            }
            else
            {
                if (MessageBox.Show("Update?", " Check List", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    result.Query = string.Format("delete from eps_tbl where req_code = '{0}'", txtReq_Id.Text.Trim());
                    if (result.ExecuteNonQuery() == 0)
                    { }
                    if (txtReq_Description.Text == "")
                        MessageBox.Show("Description should have a value", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        SaveReq("Edit");

                        MessageBox.Show("Record updated", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EnableControls(false);
                        btnEdit.Text = "&Edit";
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                        btnClose.Text = "&Close";
                        getValue();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            m_sModuleCode = "AUEC-D";
            string ReqCode = txtReq_Id.Text;
            int iRow = 0;

            string sQuery = string.Format("select * from eps_assessment where req_code = '{0}'", ReqCode);
            result.Query = sQuery;

            if (result.Execute())
            {
                if (result.Read())
                {
                    //MessageBox.Show("Sorry the current record has existing data in Assessment", "Check List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("Requirement has existing data in Assessment, deletion not allowed", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Error); // RMC 20170331 modified message prompt
                    return;
                }
                else
                {
                    if (ReqCode == txtReq_Id.Text)
                    {
                        if (MessageBox.Show("Are you sure to Delete the selected row", " Check List", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            MessageBox.Show("not deleted");
                        }
                        else
                        {
                            string sQuery1 = string.Format("DELETE FROM EPS_TBL WHERE REQ_CODE = {0}", ReqCode);
                            result.Query = sQuery1;
                            result.Execute();
                            MessageBox.Show("Data Successfully Deleted", " Check List", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            result1.Close();

                            string strObj = "";
                            strObj = "Deleted Rqmt. ChkList: " + txtReq_Id.Text.Trim() + " | Desc: " + txtReq_Description.Text.Trim();

                            if (AuditTrail.InsertTrail(m_sModuleCode, "eps_tbl", StringUtilities.HandleApostrophe(strObj)) == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            ClearFields();
                            getValue();
                            result.Close();
                        }
                    }
                }
            }
        }

        public void getValue()
        {
            OracleResultSet pRec = new OracleResultSet();
            int iRow = 0;

            dgvRequirements.Columns.Clear();
            dgvRequirements.Columns.Add("req_code", "CODE");
            dgvRequirements.Columns.Add("req_desc", "DESCRIPTION");

            dgvRequirements.Columns[0].Width = 40; //0
            dgvRequirements.Columns[1].Width = 300; //332

            dgvRequirements.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRequirements.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvRequirements.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvRequirements.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pRec.Query = "select distinct req_code, req_desc from eps_tbl order by req_code"; // JAV 20170330 change bns_stat to req_bns_stat // REM MCR 20170627 req_bns_stat
            if (pRec.Execute())
            {
                while (pRec.Read())
                    dgvRequirements.Rows.Add(pRec.GetString("req_code").Trim(), StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim()));
            }
            pRec.Close();
        }

        private void ClearFields()
        {
            txtReq_Id.Text = "";
            txtReq_Description.Text = "";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ReportClass rClass = new ReportClass();
            //rClass.sOrNo = txtORNo.Text;
            //rClass.sCeasedDate = dtpCeasedDate.Value.ToShortDateString();
            //rClass.sIssuedDate = dtpIssuedDate.Value.ToShortDateString();
            //rClass.sFPTaxYear = m_sTaxYear; // RMC 20151005 mods in retirement module
            //rClass.m_bFullRetirement = bFull;
            //rClass.RetCertLUBAO(sBin);
            //string sBin = "696969";
            rClass.Jameson();
            rClass.PreviewDocu();
        }

        private void dgvRequirements_SelectionChanged(object sender, EventArgs e)
        {
            txtReq_Id.Text = dgvRequirements.CurrentRow.Cells[0].Value.ToString();
            txtReq_Description.Text = dgvRequirements.CurrentRow.Cells[1].Value.ToString();
        }
    }
}