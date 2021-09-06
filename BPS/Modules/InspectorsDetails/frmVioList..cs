using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmVioList : Form
    {
        public frmVioList()
        {
            InitializeComponent();
        }

        String sDesc = "";

        private void frmVioList_Load(object sender, EventArgs e)
        {
            LoadDivision();
            LoadData();
        }

        private void LoadDivision()
        {
            ArrayList arrList = new ArrayList();
            arrList.Add("ENGINEERING");
            arrList.Add("BPLO");
            arrList.Add("ZONING");
            arrList.Add("SANITARY");
            arrList.Add("BFP");
            arrList.Add("BENRO");
            arrList.Add("CENRO");
            arrList.Add("CHO");
            arrList.Add("PESO"); //AFM 20191212 MAO-19-11583
            arrList.Add("MAPUMA");//AFM 20191226 MAO-19-11716

            String sValue = AppSettingsManager.GetUserDiv(AppSettingsManager.SystemUser.UserCode);

            if (sValue == "BPLO" || AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
            {
                for (int i = 0; i < arrList.Count; i++)
                    cmbDivision.Items.Add(arrList[i].ToString());
            }
            else
            {
                if (arrList.Contains(sValue)) //to prevent other office with access rights but not related to this module
                    cmbDivision.Items.Add(sValue);
            }
        }

        private void LoadData()
        {
            String sValue = AppSettingsManager.GetUserDiv(AppSettingsManager.SystemUser.UserCode);
            dgViolist.Rows.Clear();
            OracleResultSet pSet = new OracleResultSet();

            
            if (sValue == "BPLO" || AppSettingsManager.SystemUser.UserCode == "SYS_PROG")
                pSet.Query = "select * from nigvio_tbl order by division_code asc, violation_code asc";
            else
                pSet.Query = "select * from nigvio_tbl where division_code = '" + sValue + "' order by division_code asc, violation_code asc";
            if (pSet.Execute())
                while (pSet.Read())
                {
                    dgViolist.Rows.Add(pSet.GetString(0), pSet.GetString(1), StringUtilities.RemoveApostrophe(pSet.GetString(2)));
                }
            pSet.Close();

            dgViolist.ClearSelection();
            ClearText();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void EnableControl(bool blnEnabled)
        {
            btnAdd.Enabled = blnEnabled;
            btnEdit.Enabled = blnEnabled;
            btnDelete.Enabled = blnEnabled;
            btnCancel.Enabled = blnEnabled;
            dgViolist.Enabled = blnEnabled;
            cmbDivision.Enabled = blnEnabled;
            txtViolation.Enabled = blnEnabled;
        }

        private void ClearText()
        {
            txtVioCode.Text = "";
            txtViolation.Text = "";
            cmbDivision.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add")
            {
                btnAdd.Text = "Save";
                btnCancel.Text = "Cancel";
                EnableControl(true);
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                dgViolist.Enabled = false;
                ClearText();
            }
            else
            {
                if (cmbDivision.Text == "" || txtVioCode.Text.Trim() == ""
                    || txtViolation.Text.Trim() == "")
                {
                    MessageBox.Show("Fill-up empty field(s)", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to save?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = "insert into nigvio_tbl values (:1,:2,:3)";
                    pSet.AddParameter(":1", cmbDivision.Text.Trim());
                    pSet.AddParameter(":2", txtVioCode.Text.Trim());
                    pSet.AddParameter(":3", StringUtilities.HandleApostrophe(txtViolation.Text.Trim()));
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("New record has been saved.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnCancel.PerformClick();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (btnEdit.Text == "Edit")
            {
                btnEdit.Text = "Update";
                btnCancel.Text = "Cancel";
                sDesc = txtViolation.Text.Trim();
                EnableControl(true);
                cmbDivision.Enabled = false;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                dgViolist.Enabled = false;
            }
            else
            {
                if (cmbDivision.Text.Trim() == "")
                    return;

                if (sDesc == txtViolation.Text.Trim())
                    return;// due to no changes

                if (MessageBox.Show("Are you sure you want to update?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OracleResultSet pSet = new OracleResultSet();
                    pSet.Query = string.Format("update nigvio_tbl set violation_desc = '{0}' where violation_code = '{1}' and division_code = '{2}'", StringUtilities.HandleApostrophe(txtViolation.Text.Trim()), txtVioCode.Text.Trim(), cmbDivision.Text.Trim());
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    MessageBox.Show("Record updated.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCancel.PerformClick();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbDivision.Text.Trim() == "")
                return;

            if (MessageBox.Show("Do you want to delete " + txtVioCode.Text.Trim() + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OracleResultSet pSet = new OracleResultSet();

                pSet.Query = string.Format("delete from nigvio_tbl where violation_code = '{0}' and division_code = '{1}'", txtVioCode.Text.Trim(), cmbDivision.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                { }

                MessageBox.Show("Record deleted.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnCancel.Text = "Cancel";
                btnCancel.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Cancel")
            {
                btnAdd.Text = "Add";
                cmbDivision.Text = "";
                btnEdit.Text = "Edit";
                btnCancel.Text = "Close";
                EnableControl(false);
                dgViolist.Enabled = true;
                btnAdd.Enabled = true;
                btnCancel.Enabled = true;
                LoadData();
                sDesc = "";
                txtVioCode.Text = "";
            }
            else
            {
                this.Close();
            }
        }

        private void dgViolist_SelectionChanged(object sender, EventArgs e)
        {
            if (dgViolist.RowCount >= 1)
            {
                cmbDivision.Text = dgViolist.CurrentRow.Cells[0].Value.ToString();
                txtVioCode.Text = dgViolist.CurrentRow.Cells[1].Value.ToString();
                txtViolation.Text = dgViolist.CurrentRow.Cells[2].Value.ToString();
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                ClearText();
            }
        }

        private void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDivision.Text.Trim() == "")
                return;

            GenerateCodeSeries();
        }

        private void GenerateCodeSeries()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iCount = 0;

            pSet.Query = "select nvl(max(violation_code),0) from nigvio_tbl where division_code = '" + cmbDivision.Text.Trim() + "'";
            int.TryParse(pSet.ExecuteScalar().ToString(), out iCount);

            iCount++;

            txtVioCode.Text = string.Format("{0:0000000#}", iCount);
        }

        private void cmbDivision_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dgViolist_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}