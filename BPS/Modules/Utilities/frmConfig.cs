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
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public partial class frmConfig : Form
    {
        private PrintOtherReports PrintClass = null;
        private string m_sQuery = string.Empty;
        private string m_sMode = string.Empty;
        private int intCurrentCell = 0;
        private bool m_bDblClk = false;

        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            this.LoadConfig();

            dgvList.ReadOnly = true;
            m_bDblClk = true;

        }

        private void LoadConfig()
        {
            OracleResultSet result = new OracleResultSet();

            dgvList.Columns.Clear();
            dgvList.Columns.Add("CODE", "Code");
            dgvList.Columns.Add("HEADER", "Header");
            dgvList.Columns.Add("DESC", "Description");
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 60;
            dgvList.Columns[1].Width = 200;
            dgvList.Columns[2].Width = 215;
            dgvList.Refresh();

            result.Query = "select code, remarks, object from config order by code";
            if (result.Execute())
            {
                while (result.Read())
                {
                    dgvList.Rows.Add(result.GetString(0), result.GetString(1), result.GetString(2));
                }
            }
            result.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Cancel")
            {
                this.btnNew.Text = "New";
                this.btnEdit.Text = "Edit";
                this.btnClose.Text = "Close";
                this.btnNew.Enabled = true;
                this.btnEdit.Enabled = true;
                this.btnPrint.Enabled = true;
                dgvList.EditMode = DataGridViewEditMode.EditProgrammatically;
                if (m_sMode == "New")
                    dgvList.Rows.RemoveAt(intCurrentCell);
                dgvList.ReadOnly = true;
                m_sMode = "";
                m_bDblClk = true;
            }
            else
                this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            int intCode = 0;
            string strCode = string.Empty;

            if(btnNew.Text == "New")
	        {
                this.btnNew.Text = "Save";
                this.btnClose.Text = "Cancel";
		        this.btnEdit.Enabled = false;
                this.btnPrint.Enabled = false;
		        
                intCode = dgvList.Rows.Count;
                //strCode = Convert.ToString(intCode);
                strCode = GetConfigCode();  // RMC 20171110 modified generation of config code

                dgvList.Rows.Add(strCode, "", "");
                this.dgvList.CurrentCell = this.dgvList[1, intCode-1];
                intCurrentCell = intCode - 1;
                m_sMode = "New";
                m_bDblClk = false;
		    }
	        else
	        {
                if (dgvList[1, intCurrentCell].Value.ToString() != "")
                {
                    if (MessageBox.Show("Save changes?", "Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pSet.Query = "insert into config (code, object, remarks, security) values (:1, :2, :3, '0')";
                        pSet.AddParameter(":1", dgvList[0, intCurrentCell].Value.ToString().Trim());
                        pSet.AddParameter(":2", dgvList[2, intCurrentCell].Value.ToString().Trim());    // RMC 20111206 change row to 2
                        pSet.AddParameter(":3", dgvList[1, intCurrentCell].Value.ToString().Trim());    // RMC 20111206 change row to 1
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        string strObj = dgvList[0, intCurrentCell].Value.ToString().Trim();

                        if (AuditTrail.InsertTrail("AUSA", "config", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Record saved.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        m_sMode = "";
                        this.btnNew.Text = "New";
                        this.btnClose.Text = "Close";
                        this.btnEdit.Enabled = true;
                        this.btnPrint.Enabled = true;
                        this.dgvList.ReadOnly = true;
                        m_bDblClk = true;
                    }
                }
                else
                {
                    MessageBox.Show("Record requires header.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
              
	        }
        }

        private void dgvList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvList.BeginEdit(true);

            if (m_sMode == "New")
            {
                if(e.RowIndex != intCurrentCell)
                    dgvList.ReadOnly = true;
                else
                    dgvList.ReadOnly = false;
            }
            else if(m_sMode == "Edit")
            {
                if(e.ColumnIndex != 2)
                    dgvList.ReadOnly = true;
                else
                {
                    if (dgvList[0, e.RowIndex].Value != null)
                    {
                        if (!this.Secured(dgvList[0, e.RowIndex].Value.ToString().Trim()))
                            dgvList.ReadOnly = false;
                        else
                            dgvList.ReadOnly = true;
                    }
                }
            }
            
            dgvList.RefreshEdit();
        }

        private void dgvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string strTemp = string.Empty;

            try
            {
                strTemp = dgvList[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
                dgvList[e.ColumnIndex, e.RowIndex].Value = strTemp;
            }
            catch
            {
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (this.btnEdit.Text == "Edit")
            {
                this.btnEdit.Text = "Update";
                this.btnClose.Text = "Cancel";
                this.btnNew.Enabled = false;
                this.btnPrint.Enabled = false;
                this.dgvList.ReadOnly = false; //JARS 20171106
                m_sMode = "Edit";
                m_bDblClk = false;
            }
            else
            {
                if (MessageBox.Show("Update changes?", "Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    int intCtr = 0;
                    for (int intRow = 0; intRow < dgvList.Rows.Count - 1; intRow++)
                    {
                        bool blnSave = false;
                        string strHeader = string.Empty;
                        string strDesc = string.Empty;
                        string strFlexHeader = string.Empty;
                        string strFlexDesc = string.Empty;
                        string strObject = string.Empty;
                        string strCode = string.Empty;

                        try
                        {
                            strCode = dgvList[0, intRow].Value.ToString().Trim();
                        }
                        catch
                        {
                            strCode = "";
                        }

                        pSet.Query = string.Format("select * from config where code = '{0}'", strCode);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                strHeader = pSet.GetString("remarks").Trim();
                                strDesc = pSet.GetString("object").Trim();

                                try
                                {
                                    strFlexHeader = dgvList[1, intRow].Value.ToString().Trim();
                                }
                                catch
                                {
                                    strFlexHeader = "";
                                }

                                try
                                {
                                    strFlexDesc = dgvList[2, intRow].Value.ToString().Trim();
                                }
                                catch 
                                { 
                                    strFlexDesc = "";
                                }

                                strObject = "Config Code: " + strCode + "/ "; // RMC 20141105 added config code edited in trail of system configuration
                                if (strHeader != strFlexHeader && strDesc != strFlexDesc)
                                {
                                    blnSave = true;
                                    intCtr++;
                                    //strObject = strHeader + "->" + strFlexHeader + "/" + strDesc + "->" + strFlexDesc;
                                    strObject += "Description: " + strHeader + "->" + strFlexHeader + "/Object: " + strDesc + "->" + strFlexDesc;  // RMC 20141105 added config code edited in trail of system configuration
                                }

                                if (strHeader != strFlexHeader && strDesc == strFlexDesc)
                                {
                                    blnSave = true;
                                    intCtr++;
                                    //strObject = strHeader + "->" + strFlexHeader;
                                    strObject += "Description: " + strHeader + "->" + strFlexHeader;  // RMC 20141105 added config code edited in trail of system configuration
                                }

                                if (strHeader == strFlexHeader && strDesc != strFlexDesc)
                                {
                                    blnSave = true;
                                    intCtr++;
                                    //strObject = strDesc + "->" + strFlexDesc;
                                    strObject += "Object: " + strDesc + "->" + strFlexDesc;  // RMC 20141105 added config code edited in trail of system configuration
                                }
                            }
                        }
                        pSet.Close();

                        if (blnSave)
                        {
                            pSet.Query = string.Format("update config set object = '{0}', remarks = '{1}' where code = '{2}'", StringUtilities.HandleApostrophe(strFlexDesc), StringUtilities.HandleApostrophe(strFlexHeader), strCode);    // RMC 20150227 corrected error in updating config when data has apostrophe
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }

                            if (AuditTrail.InsertTrail("AUSE", "config", StringUtilities.HandleApostrophe(strObject)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                    }

                    if (intCtr > 0)
                    {
                        MessageBox.Show("Record saved.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.btnNew.Text = "New";
                    this.btnEdit.Text = "Edit";
                    this.btnClose.Text = "Close";
                    this.btnPrint.Enabled = true;
                    this.btnNew.Enabled = true;
                    this.dgvList.ReadOnly = true;
                    m_sMode = "";
                    m_bDblClk = true;
                }

                this.LoadConfig();
            }
        }

        private bool Secured(string strCode)
        {
            OracleResultSet pSet = new OracleResultSet();
            int intSecurity = 1;

            pSet.Query = string.Format("select * from config where code = '{0}'",strCode);
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    intSecurity = pSet.GetInt("security");
                }
            }
            pSet.Close();
	
	        if(intSecurity == 0)
                return false;
            else
                return true;
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string strCode = string.Empty;
            string strObject = string.Empty;
            string sSw = string.Empty;
            int intSecurity = 0;

            try
            {
                strCode = dgvList[0, e.RowIndex].Value.ToString();
            }
            catch { }

            pSet.Query = string.Format("select * from config where code = '{0}'", strCode);

            if (m_bDblClk && Granted.Grant("AUSF"))
            {
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sSw = "OFF";
                        intSecurity = pSet.GetInt("security");
                        if (intSecurity == 1)
                            sSw = "ON";

                        if (MessageBox.Show("Security level is currently " + sSw + ".\nWould you like to change it?", "Config", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (intSecurity == 1)
                            {
                                result.Query = string.Format("update config set security = {0} where code = '{1}'", 0, strCode);
                                strObject = string.Format("ON -> OFF / {0}", strCode);
                            }
                            else
                            {
                                result.Query = string.Format("update config set security = {0} where code = '{1}'", 1, strCode);
                                strObject = string.Format("OFF -> ON / {0}", strCode);
                            }
                            if (result.ExecuteNonQuery() == 0)
                            {
                            }

                            if (AuditTrail.InsertTrail("AUSSL", "config", StringUtilities.HandleApostrophe(strObject)) == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No record found.", "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }

                }
                pSet.Close();
            }
            else
            {
                if(pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        sSw = "OFF";
                        intSecurity = pSet.GetInt("security");

                        if (intSecurity == 1)
                            sSw = "ON";

                        MessageBox.Show("Security level is currently " + sSw, "Config", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet result = new OracleResultSet();

            PrintClass = new PrintOtherReports();
            PrintClass.ReportName = "CONFIGURATION";
            PrintClass.Source = "2";
            PrintClass.FormLoad();
        }

        private string GetConfigCode()
        {
            // RMC 20171110 modified generation of config code
            OracleResultSet pSet = new OracleResultSet();
            string sCode = string.Empty;
            int iCode = 0;

            pSet.Query = "select * from config order by code desc";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    int.TryParse(pSet.GetString("code"), out iCode);
                    iCode++;
                    sCode = string.Format("{0:####}", iCode);
                }
            }
            pSet.Close();

            return sCode;
        }
    }
}