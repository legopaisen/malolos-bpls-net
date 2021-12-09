using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using System.IO;

namespace Amellar.Modules.Utilities
{
    public partial class frmBarangay : Form
    {
        private PrintOtherReports PrintClass = null;
        string m_strBrgyCode = string.Empty;
        string m_strBrgy = string.Empty;
        string m_strZone = string.Empty;
        string m_strDistCode = string.Empty;
        string m_strCapt = string.Empty; //JHB 20191012 add for brgy clearance
        //JHB 20200102(s)
        string m_strTreas = string.Empty;
        string m_strSec = string.Empty;
        string m_strKwd1 = string.Empty;
        string m_strKwd2 = string.Empty;
        string m_strKwd3 = string.Empty;
        string m_strKwd4 = string.Empty;
        string m_strKwd5 = string.Empty;
        string m_strKwd6 = string.Empty;
        string m_strKwd7 = string.Empty;
        string m_strSk = string.Empty;
        //JHB 20200102(e)
        string m_strDist = string.Empty;

        public frmBarangay()
        {
            InitializeComponent();
        }

        private void frmBarangay_Load(object sender, EventArgs e)
        {
            this.UpdateList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                this.btnNew.Text = "Add";
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
                this.EnableControls(false);
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.dgvList.Enabled = true;
                this.ClearControls();
            }
            else
                this.Close();
        }

        private void UpdateList()
        {
            string strQuery = string.Empty;
                        
           // strQuery = "select brgy_code \" Brgy Code\", brgy_nm \" Brgy Name\", zone \" Zone\", dist_code \" Dist Code\", dist_nm \"Dist Name\" from brgy order by brgy_code asc";
            strQuery = "select brgy_code \" Brgy Code\", brgy_nm \" Brgy Name\", zone \" Zone\", dist_code \" Dist Code\", dist_nm \"Dist Name\" ";
            strQuery += ",brgy_capt \" Brgy Capt.\",brgy_treas \" Brgy Treasurer.\",brgy_sec \" Brgy Secretary\" ,sk_chair \" SK Chairman\" ";
            strQuery += ",brgy_kagawad1 \" Kagawad01\",brgy_kagawad2 \" Kagawad02\",brgy_kagawad3 \" Kagawad03\",brgy_kagawad4 \" Kagawad04\" "; //JHB 20191012 add for brgy clearance
            strQuery += ",brgy_kagawad5 \" Kagawad05\",brgy_kagawad6 \" Kagawad06\",brgy_kagawad7 \" Kagawad07\" ";
            strQuery += "from brgy order by brgy_code asc"; //JHB 20191012 add for brgy clearance
           

            DataGridViewOracleResultSet dsUser = new DataGridViewOracleResultSet(dgvList, strQuery, 0, 0);
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 50;
            dgvList.Columns[1].Width = 150;
            dgvList.Columns[2].Width = 100;
            dgvList.Columns[3].Width = 50;
            dgvList.Columns[4].Width = 150;
            dgvList.Columns[5].Width = 150; //JHB 20191012 add for brgy clearance
            //JHB 20200103 (s)
            dgvList.Columns[6].Width = 150;
            dgvList.Columns[7].Width = 150;
            dgvList.Columns[8].Width = 150;
            dgvList.Columns[9].Width = 150;
            dgvList.Columns[10].Width = 150;
            dgvList.Columns[11].Width = 150;
            dgvList.Columns[12].Width = 150;
            dgvList.Columns[13].Width = 150;
            dgvList.Columns[14].Width = 150;
            dgvList.Columns[15].Width = 150; 
            //JHB 20200103 (e)
            dgvList.Refresh();
        }

        private void SelectList(int intRow)
        {
            if (intRow >= 0)
            {
                txtBrgyCode.Text = dgvList[0, intRow].Value.ToString().Trim();
                txtBrgy.Text = dgvList[1, intRow].Value.ToString().Trim();
                txtZone.Text = dgvList[2, intRow].Value.ToString().Trim();
                txtDistCode.Text = dgvList[3, intRow].Value.ToString().Trim();
                txtDist.Text = dgvList[4, intRow].Value.ToString().Trim();
                txtCapt.Text = dgvList[5, intRow].Value.ToString().Trim();  //JHB 20191012 add for brgy clearance
                //JHB 20200103(s)
                txtBrgyTreas.Text     = dgvList[6, intRow].Value.ToString().Trim();
                txtBrgySec.Text       = dgvList[7, intRow].Value.ToString().Trim();
                txtSKChairman.Text    = dgvList[8, intRow].Value.ToString().Trim(); 
                txtKagawad1.Text      = dgvList[9, intRow].Value.ToString().Trim();
                txtKagawad2.Text      = dgvList[10, intRow].Value.ToString().Trim();
                txtKagawad3.Text      = dgvList[11, intRow].Value.ToString().Trim();
                txtKagawad4.Text      = dgvList[12, intRow].Value.ToString().Trim();
                txtKagawad5.Text      = dgvList[13, intRow].Value.ToString().Trim();
                txtKagawad6.Text      = dgvList[14, intRow].Value.ToString().Trim();
                txtKagawad7.Text      = dgvList[15, intRow].Value.ToString().Trim();
                //JHB 20200103(e)

                m_strBrgyCode = txtBrgyCode.Text;
                m_strBrgy = txtBrgy.Text;
                m_strZone = txtZone.Text;
                m_strDistCode = txtDistCode.Text;
                m_strDist = txtDist.Text;
                m_strCapt = txtCapt.Text; //JHB 20191012 add for brgy clearance

                //JHB 20200103(s)
                m_strTreas     = txtBrgyTreas.Text.Trim();
                m_strSec       = txtBrgySec.Text.Trim();
                m_strSk        = txtSKChairman.Text.Trim();
                m_strKwd1      = txtKagawad1.Text.Trim();
                m_strKwd2      = txtKagawad2.Text.Trim();
                m_strKwd3      = txtKagawad3.Text.Trim();
                m_strKwd4      = txtKagawad4.Text.Trim();
                m_strKwd5      = txtKagawad5.Text.Trim();
                m_strKwd6      = txtKagawad6.Text.Trim();
                m_strKwd7      = txtKagawad7.Text.Trim();
                //JHB 20200103(e)

            }
        }

        private void dgvList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectList(e.RowIndex);
        }

        private void dgvList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.SelectList(e.RowIndex);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnNew.Text == "Add")
            {
                this.ClearControls();
                this.EnableControls(true);
                this.btnNew.Text = "Save";
                this.btnEdit.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnClose.Text = "Cancel";
                this.dgvList.Enabled = false;
                //this.txtBrgyCode.Focus();
                this.GenerateBrgyCode();
                this.txtBrgy.Focus();
            }
            else
            {
                if(txtBrgy.Text.Trim() == "" || txtBrgyCode.Text.Trim() == "")
		        {	
                    MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
		        }
		
                if ((txtDist.Text.Trim() == "" || txtDistCode.Text.Trim() == "") && AppSettingsManager.GetConfigObject("23") == "Y")
		        {
			        MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			        return;
		        }

                if(txtZone.Text.Trim() == "" && AppSettingsManager.GetConfigObject("24") == "Y")
				{
			        MessageBox.Show("Please Supply Sufficient Data","Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			        return;
		        }
		
                result.Query = string.Format("select * from brgy where brgy_code ='{0}'",txtBrgyCode.Text.Trim());
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        string strTempBrgyCode = result.GetString("brgy_code");
                        string strTempBrgy = result.GetString("brgy_nm");
                        string strTempDistCode = result.GetString("dist_code");

                        MessageBox.Show("Brgy Code: " + strTempBrgyCode + " and Brgy Name: " + strTempBrgy + "and District Code: " + strTempDistCode + " already exists.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        result.Close();

                        result.Query = "insert into brgy (brgy_code, brgy_nm, zone, dist_code, dist_nm) values (:1,:2,:3,:4,:5)";
                        result.AddParameter(":1", txtBrgyCode.Text.Trim());
                        result.AddParameter(":2", txtBrgy.Text.Trim());
                        result.AddParameter(":3", txtZone.Text.Trim());
                        result.AddParameter(":4", txtDistCode.Text.Trim());
                        result.AddParameter(":5", StringUtilities.SetEmptyToSpace(txtDist.Text.Trim()));
                        result.AddParameter(":6", txtCapt.Text.Trim()); //JHB 20191012 add for brgy clearance
                        //JHB 20200103 (s)
                        result.AddParameter(":7", txtBrgyTreas.Text.Trim());
                        result.AddParameter(":8", txtBrgySec.Text.Trim());
                        result.AddParameter(":9", txtSKChairman.Text.Trim());
                        result.AddParameter(":10", txtKagawad1.Text.Trim());
                        result.AddParameter(":11", txtKagawad2.Text.Trim());
                        result.AddParameter(":12", txtKagawad3.Text.Trim());
                        result.AddParameter(":13", txtKagawad4.Text.Trim());
                        result.AddParameter(":14", txtKagawad5.Text.Trim());
                        result.AddParameter(":15", txtKagawad6.Text.Trim());
                        result.AddParameter(":16", txtKagawad7.Text.Trim());
                        //JHB 20200103 (e)
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();
                        
                        if (AuditTrail.InsertTrail("AUTB-A", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            result.Rollback();
                            result.Close();
                            MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                result.Close();

                this.UpdateList();
	
                this.btnNew.Text = "Add";
                this.btnEdit.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnClose.Text = "Close";
                this.dgvList.Enabled = true;
                this.EnableControls(false);
            }
        }

        private void ClearControls()
        {
            txtBrgyCode.Text = "";
            txtBrgy.Text = "";
            txtZone.Text = "";
            txtDistCode.Text = "";
            txtDist.Text = "";
            txtCapt.Text = ""; //JHB 20191012 add for brgy clearance
            txtBrgyTreas.Text = "";
            txtBrgySec.Text = "";
            txtSKChairman.Text = "";
            txtKagawad1.Text = "";
            txtKagawad2.Text = "";
            txtKagawad3.Text = "";
            txtKagawad4.Text = "";
            txtKagawad5.Text = "";
            txtKagawad6.Text = "";
            txtKagawad7.Text = "";
        }

        private void EnableControls(bool blnEnable)
        {
            //txtBrgyCode.ReadOnly = !blnEnable;
            txtBrgy.ReadOnly = !blnEnable;
            txtCapt.ReadOnly = !blnEnable; //JHB 20191012 add for brgy clearance
            //JHB 20200103 (s)
            txtBrgyTreas.ReadOnly = !blnEnable;
            txtBrgySec.ReadOnly = !blnEnable;
            txtSKChairman.ReadOnly = !blnEnable;
            txtKagawad1.ReadOnly = !blnEnable;
            txtKagawad2.ReadOnly = !blnEnable;
            txtKagawad3.ReadOnly = !blnEnable;
            txtKagawad4.ReadOnly = !blnEnable;
            txtKagawad5.ReadOnly = !blnEnable;
            txtKagawad6.ReadOnly = !blnEnable;
            txtKagawad7.ReadOnly = !blnEnable; 
            //JHB 20200103 (e)

            if (AppSettingsManager.GetConfigObject("23") == "N")
            {
                //txtDistCode.ReadOnly = true;
                txtDist.ReadOnly = true;
            }
            else
            {
                //txtDistCode.ReadOnly = !blnEnable;
                txtDist.ReadOnly = !blnEnable;
            }

            if (AppSettingsManager.GetConfigObject("24") == "N")
                txtZone.ReadOnly = true;
            else
                txtZone.ReadOnly = !blnEnable;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (btnEdit.Text == "Edit")
            {
                if (txtBrgyCode.Text.Trim() == "" || txtBrgy.Text.Trim() == "")
                {
                    MessageBox.Show("No record selected. Please select record to edit.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                this.EnableControls(true);
                this.dgvList.Enabled = false;
                this.btnNew.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnPrint.Enabled = false;
                this.btnClose.Text = "Cancel";
                this.btnEdit.Text = "Update";
            }
            else
            {
                result.Query = string.Format("select * from brgy where brgy_code = '{0}' and brgy_code <> '{1}'", txtBrgyCode.Text.Trim(), m_strBrgyCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Duplicate Barangay Code.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();

                if (txtBrgy.Text.Trim() == "" || txtBrgyCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if ((txtDist.Text.Trim() == "" || txtDistCode.Text.Trim() == "") && AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (txtZone.Text.Trim() == "" && AppSettingsManager.GetConfigObject("24") == "Y")
                {
                    MessageBox.Show("Please Supply Sufficient Data", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                result.Query = string.Format("select * from brgy where brgy_nm = '{0}' and brgy_nm <> '{1}'", txtBrgy.Text.Trim(), m_strBrgy);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        MessageBox.Show("Duplicate Barangay Name.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                result.Close();

                StringBuilder sQuery = new StringBuilder();

                sQuery.Append(string.Format("update brgy set brgy_code = '{0}',brgy_capt = '{1}', ", txtBrgyCode.Text.Trim(), txtCapt.Text.Trim()));
                
                //JHB 20200106 (s)
                sQuery.Append(string.Format("brgy_treas     = '{0}', ", txtBrgyTreas.Text.Trim()));
                sQuery.Append(string.Format("brgy_sec       = '{0}', ", txtBrgySec.Text.Trim()));
                sQuery.Append(string.Format("sk_chair       = '{0}', ", txtSKChairman.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad1  = '{0}', ", txtKagawad1.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad2  = '{0}', ", txtKagawad2.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad3  = '{0}', ", txtKagawad3.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad4  = '{0}', ", txtKagawad4.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad5  = '{0}', ", txtKagawad5.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad6  = '{0}', ", txtKagawad6.Text.Trim()));
                sQuery.Append(string.Format("brgy_kagawad7  = '{0}', ", txtKagawad7.Text.Trim())); 
                //JHB 20200106 (e)
              
                if (AppSettingsManager.GetConfigObject("24") == "Y")
                    sQuery.Append(string.Format("zone = '{0}', ", txtZone.Text.Trim()));
                if (AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    sQuery.Append(string.Format("dist_code = '{0}', ", txtDistCode.Text.Trim()));
                    sQuery.Append(string.Format("dist_nm = '{0}', ", StringUtilities.SetEmptyToSpace(txtDist.Text.Trim())));
                }
                sQuery.Append(string.Format("brgy_nm = '{0}' ", txtBrgy.Text.Trim()));
                sQuery.Append(string.Format("where brgy_code = '{0}' ", m_strBrgyCode));
                sQuery.Append(string.Format("and brgy_nm = '{0}' ", m_strBrgy));
               // sQuery.Append(string.Format("and brgy_capt = '{0}' ", txtCapt.Text.Trim()));//JHB 20191012 add for brgy clearance
                if (AppSettingsManager.GetConfigObject("24") == "Y")
                    sQuery.Append(string.Format("and zone = '{0}' ", m_strZone));
                if (AppSettingsManager.GetConfigObject("23") == "Y")
                {
                    sQuery.Append(string.Format("and dist_code = '{0}'", m_strDistCode));
                    sQuery.Append(string.Format("and dist_nm = '{0}'", m_strDist));
                }

                result.Query = sQuery.ToString();
                if (result.ExecuteNonQuery() == 0)
                {
                }

                string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();

                if (AuditTrail.InsertTrail("AUTB-U", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.UpdateList();
                this.EnableControls(false);
                this.dgvList.Enabled = true;
                this.btnNew.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnPrint.Enabled = true;
                this.btnClose.Text = "Close";
                this.btnEdit.Text = "Edit";
            }        
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            if (txtBrgyCode.Text.Trim() == "" || txtBrgy.Text.Trim() == "")
            {
                MessageBox.Show("No record selected. Please select record to delete.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
	    	else 
            {
                if (MessageBox.Show("The Barangay "+txtBrgy.Text+" will be permanently DELETED.\nPlease confirm.", "Barangay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
                    StringBuilder sQuery = new StringBuilder();

                    sQuery.Append(string.Format("delete from brgy where brgy_code = '{0}' and brgy_nm = '{1}' ", txtBrgyCode.Text.Trim(), txtBrgy.Text.Trim()));
                    if(AppSettingsManager.GetConfigObject("23") == "Y")
                        sQuery.Append(string.Format("and dist_code = '{0}'", txtDistCode.Text.Trim()));
                    result.Query = sQuery.ToString();
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    string strObj = txtBrgyCode.Text.Trim() + "-" + txtBrgy.Text.Trim() + "=" + txtDistCode.Text.Trim();

                    if (AuditTrail.InsertTrail("AUTB-D", "brgy", StringUtilities.HandleApostrophe(strObj)) == 0)
                    {
                        result.Rollback();
                        result.Close();
                        MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.UpdateList();
                    this.ClearControls();
			    }
	        }
        }

        private void GenerateBrgyCode()
        {
            OracleResultSet result = new OracleResultSet();
            string strCode = string.Empty;
            int intCode = 0;

            result.Query = "select * from brgy order by brgy_code desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    strCode = result.GetString("brgy_code");
                }
                else
                    strCode = "0";
            }
            result.Close();

            try
            {
                intCode = Convert.ToInt32(strCode) + 1;
                txtBrgyCode.Text = string.Format("{0:00#}", intCode);
            }
            catch
            {
                MessageBox.Show("Invalid barangay code.", "Barangay", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtBrgyCode.Text = "";
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            /*PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "List of Barangay";
            PrintClass.Source = "1";
            PrintClass.FormLoad();*/

            // RMC 20150520 corrections in reports (s)
            frmReportSched form = new frmReportSched();
            form.Source = "4";
            form.ReportName = "List of Barangay";
            form.ShowDialog();
            // RMC 20150520 corrections in reports (e)
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlg.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string picPath = dlg.FileName.ToString();
                    picBoxLogo.ImageLocation = picPath;
                }
            }
        }

        private void picBoxLogo_Click(object sender, EventArgs e)
        {

        }

    }
}