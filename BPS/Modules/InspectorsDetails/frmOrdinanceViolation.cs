using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmOrdinanceViolation : Form
    {
        private string m_sISNum = string.Empty;
        private string m_sInspectorCode = string.Empty;
		private string m_sDateInspected = string.Empty;
        private string m_sBin = string.Empty;
        private string m_sBussDtl = string.Empty;
        
        //MCR 20191121 (s)
        private bool m_bNigList = false;
        public bool isNigList
        {
            get { return m_bNigList; }
            set { m_bNigList = value; }
        }
        private string m_sDivisionCode = string.Empty;
        public string DivisionCode
        {
            get { return m_sDivisionCode; }
            set { m_sDivisionCode = value; }
        }
        //MCR 20191121 (e)

        public string ISNum
        {
            get { return m_sISNum; }
            set { m_sISNum = value; }
        }

        public string InspectorCode
        {
            get {return m_sInspectorCode;}
            set {m_sInspectorCode = value;}
        }
        
        public string DateInspected
        {
            get {return m_sDateInspected;}
            set {m_sDateInspected = value;}
        }

        public string Bin
        {
            get {return m_sBin;}
            set {m_sBin = value;}
        }

        public string BussDtl
        {
            get { return m_sBussDtl; }
            set { m_sBussDtl = value; }
        }

        public frmOrdinanceViolation()
        {
            InitializeComponent();
        }

        private void frmOrdinanceViolation_Load(object sender, EventArgs e)
        {
            dgvList.Columns.Clear();
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("CODE", "Code");
            dgvList.Columns.Add("DESC", "Description");
            if (m_bNigList == false)
                dgvList.Columns.Add("REF", "Reference");
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 100;
            dgvList.Columns[2].Width = 200;

            if (m_bNigList == false)
                dgvList.Columns[3].Width = 200;

            dgvList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            if (m_bNigList == false)
                dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            UpdateList();

            if (m_bNigList == false)
            {
                if (m_sISNum.Trim() != "")
                    CheckViolationUfc();
                else
                    CheckViolation();
            }
            else
            {
                CheckViolationNig();
            }
        }

        private void UpdateList()
        {
            OracleResultSet pSet = new OracleResultSet();
            int iRow = 0;

            if (m_bNigList == false)
                pSet.Query = "select * from violation_table order by violation_code";
            else
                pSet.Query = "select * from nigvio_tbl where division_code = '" + m_sDivisionCode + "' order by violation_code asc";

            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = false;
                    dgvList[1, iRow].Value = pSet.GetString("violation_code").Trim();
                    dgvList[2, iRow].Value = pSet.GetString("violation_desc").Trim();
                    if (m_bNigList == false)
                        dgvList[3, iRow].Value = pSet.GetString("reference").Trim();
                    iRow++;
                }
            }
            pSet.Close();
        }

        private void CheckViolationUfc()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sViolationCode = string.Empty;

            pSet.Query = string.Format("select * from violation_ufc where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}' order by violation_code", m_sInspectorCode, m_sISNum, m_sDateInspected);
            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    sViolationCode = pSet.GetString("violation_code");

                    for(int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
			        {
                        if (dgvList[1, iRow].Value.ToString().Trim() == sViolationCode)
                        {
                            dgvList[0, iRow].Value = true;
                        }
                    }
				}
			}
			pSet.Close();
		}

        private void CheckViolation()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sViolationCode = string.Empty;

            pSet.Query = string.Format("select * from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}' order by violation_code", m_sInspectorCode, m_sBin, m_sDateInspected);
            if(pSet.Execute())
            {
                while(pSet.Read())
                {
                    sViolationCode = pSet.GetString("violation_code");

                    for(int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
			        {
                        if (dgvList[1, iRow].Value.ToString().Trim() == sViolationCode)
                        {
                            dgvList[0, iRow].Value = true;
                        }
                    }
				}
			}
			pSet.Close();
	    }

        private void CheckViolationNig()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sViolationCode = string.Empty;

            pSet.Query = string.Format("select * from nigvio_list where division_code = '{0}' and bin = '{1}' and date_inspected = '{2}' order by violation_code", m_sDivisionCode, m_sBin, m_sDateInspected);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sViolationCode = pSet.GetString("violation_code");

                    for (int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
                    {
                        if (dgvList[1, iRow].Value.ToString().Trim() == sViolationCode)
                        {
                            dgvList[0, iRow].Value = true;
                        }
                    }
                }
            }
            pSet.Close();
        } //MCR 20191121

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if ((bool)dgvList[0, e.RowIndex].Value)
                {
                    dgvList[0, e.RowIndex].Value = false;
                }
                else
                {
                    dgvList[0, e.RowIndex].Value = true;
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (MessageBox.Show("Save Changes?", "Ordinance Violation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (m_bNigList == false) //MCR 20191121
                {
                    if (m_sISNum.Trim() != "")
                    {
                        // RMC 20171115 Added history of untagged violations (s)
                        pSet.Query = string.Format("insert into violation_ufc_hist select a.*,'{0}' from violation_ufc a where inspector_code = '{1}' and is_number = '{2}'", AppSettingsManager.SystemUser.UserCode, m_sInspectorCode, m_sISNum);
                        if (pSet.ExecuteNonQuery() == 0) { }
                        // RMC 20171115 Added history of untagged violations (e)

                        pSet.Query = string.Format("delete from violation_ufc where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sInspectorCode, m_sISNum, m_sDateInspected);
                        if (pSet.ExecuteNonQuery() == 0) { } //AFM 20201203 MAO-20-14050
                    }
                    else
                    {
                        // RMC 20171115 Added history of untagged violations (s)
                        pSet.Query = string.Format("insert into violations_hist select a.*,'{0}' from violations a where inspector_code = '{1}' and bin = '{2}'", AppSettingsManager.SystemUser.UserCode, m_sInspectorCode, m_sBin);
                        if (pSet.ExecuteNonQuery() == 0) { }
                        // RMC 20171115 Added history of untagged violations (e)

                        pSet.Query = string.Format("delete from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}'", m_sInspectorCode, m_sBin, m_sDateInspected);
                        if (pSet.ExecuteNonQuery() == 0) { } //AFM 20201203 MAO-20-14050

                    }
                }
                else
                {
                    pSet.Query = string.Format("insert into nigvio_list_hist select a.*,'{0}' from nigvio_list a where division_code = '{1}' and bin = '{2}'", AppSettingsManager.SystemUser.UserCode, m_sDivisionCode, m_sBin);
                    if (pSet.ExecuteNonQuery() == 0) { }
                    pSet.Query = string.Format("delete from nigvio_list where division_code = '{0}' and bin = '{1}' and date_inspected = '{2}'", m_sDivisionCode, m_sBin, m_sDateInspected);
                    if (pSet.ExecuteNonQuery() == 0) { } //AFM 20201203 MAO-20-14050
                }

                if (pSet.ExecuteNonQuery() == 0)
                { }

                for (int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
                {
                    if ((bool)dgvList[0, iRow].Value)
                    {
                        if (m_bNigList == false) //MCR 20191121
                        {
                            if (m_sISNum.Trim() != "")
                            {
                                pSet.Query = "insert into violation_ufc values(:1,:2,:3,:4)";
                                pSet.AddParameter(":1", m_sInspectorCode);
                                pSet.AddParameter(":2", m_sISNum);
                                pSet.AddParameter(":3", m_sDateInspected);
                                pSet.AddParameter(":4", dgvList[1, iRow].Value.ToString().Trim());
                            }
                            else
                            {
                                pSet.Query = "insert into violations values(:1,:2,:3,:4)";
                                pSet.AddParameter(":1", m_sInspectorCode);
                                pSet.AddParameter(":2", m_sBin);
                                pSet.AddParameter(":3", m_sDateInspected);
                                pSet.AddParameter(":4", dgvList[1, iRow].Value.ToString().Trim());
                            }
                        }
                        else
                        {
                            pSet.Query = "insert into nigvio_list values(:1,:2,:3,:4,:5)";
                            pSet.AddParameter(":1", m_sDivisionCode);
                            pSet.AddParameter(":2", m_sBin);
                            pSet.AddParameter(":3", m_sDateInspected);
                            pSet.AddParameter(":4", dgvList[1, iRow].Value.ToString().Trim());
                            pSet.AddParameter(":5", m_sInspectorCode);
                        }

                        if (pSet.ExecuteNonQuery() == 0)
                        { }
                    }
                }

                MessageBox.Show("Record saved.", "Ordinance Violation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //AFM 20200401 remove from negative list when all violations removed (s)
            pSet.Query = "delete from nigvio_details a where a.bin = '" + m_sBin + "' and a.division_code = '" + m_sDivisionCode + "' and a.bin not in (select b.bin from nigvio_list b where a.bin = b.bin and a.division_code = b.division_code)";
            if (pSet.ExecuteNonQuery() == 0) { }
            //AFM 20200401 remove from negative list when all violations removed (e)
            pSet.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Close();
        }

    }
}