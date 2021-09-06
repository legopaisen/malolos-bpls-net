using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using System.Collections;

namespace Amellar.Modules.ApplicationRequirements
{
    public partial class frmAppCheckList : Form
    {
        private string m_sBIN = string.Empty;
        private string m_sTaxYear = string.Empty;
        private string m_sSource = string.Empty;
        private string m_sWatTable = string.Empty;
        private bool m_bTempPermit; // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        private ArrayList alCode;    // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        private ArrayList alDesc;    // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        private string m_sTempList;     // RMC 20171110 added configurable option to print temporary permit - requested by Malolos

               public string BIN
        {
            get { return m_sBIN; }
            set { m_sBIN = value; }
        }

        public string TaxYear
        {
            get { return m_sTaxYear; }
            set { m_sTaxYear = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }


        public bool TempPermit  // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        {
            get { return m_bTempPermit; }
            set { m_bTempPermit = value; }
        }

        public string TempList  // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
        {
            get { return m_sTempList; }
            set { m_sTempList = value; }
        }

        public string WatTable
        {
            get { return m_sWatTable; }
            set { m_sWatTable = value; }
        }

        
        public frmAppCheckList()
        {
            InitializeComponent();
        }

        private void frmAppCheckList_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            int iRow = 0;
            DateTime dtYr = AppSettingsManager.GetSystemDate();
            string sYr = dtYr.Year.ToString();

            alCode = new ArrayList();
            alDesc = new ArrayList();

            dgvList.Columns.Clear();
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            if(m_bTempPermit)
                dgvList.Columns.Add("DESC", "Documents For Compliance");
            else
                dgvList.Columns.Add("DESC", "Documents Submitted");
            dgvList.Columns.Add("CODE", "Code");
            dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvList.Columns[0].Width = 30;
            dgvList.Columns[1].Width = 250;
            dgvList.Columns[2].Visible = false;

            string sStatus = "";
            string sBnsCode = "";
            string sOrgKind = "";
            string sBnsCodeAddl = ""; //JHB 20180319 for code 08 addl_bns for printing cert.code 06
           
            double dGross = 0;

            string sBnsStreet = "";     // RMC 20171124 Added checklist requirement specific to Malolos

            if (m_sSource == "PERMIT")
                if (m_sWatTable == "QUE")
                    pRec.Query = string.Format("select * from business_que where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear); //AFM 20200106 requested by malolos. allow viewing of unpaid bin
                else
                    pRec.Query = string.Format("select * from businesses where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
                                 
            else
            {
                int iCnt = 0;
                pRec.Query = string.Format("select count(*) from business_que where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
                int.TryParse(pRec.ExecuteScalar(), out iCnt);

                if (iCnt == 0)
                    pRec.Query = string.Format("select * from businesses where bin = '{0}'", m_sBIN);
                else
                    pRec.Query = string.Format("select * from business_que where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            }
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sStatus = pRec.GetString("bns_stat"); 
                    sBnsCode = pRec.GetString("bns_code" );
                    sOrgKind = pRec.GetString("orgn_kind");
                    dGross = pRec.GetDouble("gr_1");
                    sBnsStreet = pRec.GetString("bns_street");     // RMC 20171124 Added checklist requirement specific to Malolos

                    if (sOrgKind  == "SINGLE PROPRIETORSHIP")
                        sOrgKind = "SINGLE";
                }
            }
            pRec.Close();

            pRec.Query = string.Format("select sum(gross) from addl_bns where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    dGross += pRec.GetDouble(0);
                }
            }
            pRec.Close();

            bool bPeza = false;
            bool bBoi = false;

            int iRecCount = 0;
            pRec.Query = string.Format("select count(*) from boi_table where bin = '{0}' and exempt_type = 'PEZA'", m_sBIN);
            int.TryParse(pRec.ExecuteScalar(), out iRecCount);
            if (iRecCount > 0)
                bPeza = true;

            iRecCount = 0;
            pRec.Query = string.Format("select count(*) from boi_table where bin = '{0}' and exempt_type = 'BOI'", m_sBIN);
            int.TryParse(pRec.ExecuteScalar(), out iRecCount);
            if (iRecCount > 0)
                bBoi = true;
            
            string sOtherDesc = "";
            string sOtherVar = "";

            pRec.Query = "select * from REQUIREMENTS_CHKLIST ";
            if (sStatus == "NEW")
            {
                pRec.Query += " where bns_stat = 'NEW'";
            }
            else
            {
                pRec.Query += " where bns_stat = 'RENEWAL'";
            }
            //pRec.Query += string.Format(" and (bns_code = 'ALL' or bns_code = '{0}'or bns_code = '" +sBnsCodeAddl + "') ", sBnsCode,sBnsCodeAddl); 
            pRec.Query += string.Format(" and (bns_code = 'ALL' or bns_code = '{0}'or bns_code in (SELECT bns_code_main FROM addl_bns WHERE bin = '" + m_sBIN + "')) ", sBnsCode, sBnsCodeAddl); //JHB 20180319 for code 08 addl_bns for printing cert.code 06
            pRec.Query += string.Format(" and bns_org = '{0}'", sOrgKind);
            pRec.Query += " order by req_code";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    
                    sOtherDesc = pRec.GetString("other_desc");
                    sOtherVar = string.Format("{0:###.00}", pRec.GetDouble("other_var"));

                    if (sOtherVar == "")
                        sOtherVar = "0";

                    if (sOtherDesc == "")
                    {
                        dgvList.Rows.Add("");
                        dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                        dgvList[2, iRow].Value = pRec.GetString("req_code").Trim();

                        LoadChecklist(pRec.GetString("req_code"), iRow);
                        iRow++;
                    }
                    else
                    {
                        if (sOtherDesc == "GROSS")
                        {
                            if (dGross >= Convert.ToDouble(sOtherVar))
                            {
                                dgvList.Rows.Add("");
                                dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                                dgvList[2, iRow].Value = pRec.GetString("req_code").Trim();

                                LoadChecklist(pRec.GetString("req_code"), iRow);
                                iRow++;
                            }
                        }

                        if ((sOtherDesc == "PEZA" && bPeza) || (sOtherDesc == "BOI" && bBoi))
                        {
                            dgvList.Rows.Add("");
                            dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                            dgvList[2, iRow].Value = pRec.GetString("req_code").Trim();

                            LoadChecklist(pRec.GetString("req_code"), iRow);
                            iRow++;
                        }

                        // RMC 20171124 Added checklist requirement specific to Malolos (s)
                        if (sOtherDesc == "MAPUMA" && (sBnsStreet.Contains("MALOLOS PUBLIC MARKET") || sBnsStreet.Contains("MAPUMA")))
                        {
                            dgvList.Rows.Add("");
                            dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                            dgvList[2, iRow].Value = pRec.GetString("req_code").Trim();

                            LoadChecklist(pRec.GetString("req_code"), iRow);
                            iRow++;
                        }
                        // RMC 20171124 Added checklist requirement specific to Malolos (e)

                        // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (S)
                        if ((sOtherDesc == "SUBD" || sOtherDesc == "SUBDIVISION") && (sBnsStreet.Contains("SUBD") || sBnsStreet.Contains("SUBDIVISION")))
                        {
                            dgvList.Rows.Add("");
                            dgvList[1, iRow].Value = StringUtilities.RemoveApostrophe(pRec.GetString("req_desc").Trim());
                            dgvList[2, iRow].Value = pRec.GetString("req_code").Trim();

                            LoadChecklist(pRec.GetString("req_code"), iRow);
                            iRow++;
                        } 
                        // RMC 20180105 added entry of SUBDIVISION requirement, same procedure as in MAPUMA (E)

                    }
                }
            }
            pRec.Close();

            // RMC 20171110 added configurable option to print temporary permit - requested by Malolos (s)
            if (m_bTempPermit)
            {
                this.Text = "Select Compliance list";
                btnSave.Visible = false;
                btnSelect.Visible = true;
                btnCancel.Visible = false;

                dgvList.Rows.Clear();
                int intCount = alCode.Count;
                iRow = 0;
                for (int i = 0; i < intCount; i++)
                {
                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = false;
                    dgvList[2, iRow].Value = alCode[i].ToString();
                    dgvList[1, iRow].Value = alDesc[i].ToString();
                    iRow++;
                }               
            }
            else
            {
                btnSave.Visible = true;
                btnSelect.Visible = false;
            }
            // RMC 20171110 added configurable option to print temporary permit - requested by Malolos (e)
        }

        private void LoadChecklist(string sReqCode, int iRow)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();
            DateTime dtYear = AppSettingsManager.GetSystemDate();
            string sYear = dtYear.Year.ToString();
            string sYear2 = string.Empty;

            pRec2.Query = "select max(to_number(tax_year)) from requirements_tbl where bin = '" + m_sBIN + "'"; //AFM 20200106 get year of previous existing checklist
            if(pRec2.Execute())
                if (pRec2.Read())
                {
                    sYear2 = pRec2.GetInt(0).ToString();
                }

            if (m_sTaxYear != sYear2)//AFM 20200106 added condition to determine year
                m_sTaxYear = sYear;

            pRec.Query = string.Format("select * from requirements_tbl where bin = '{0}' and req_code = '{1}' and tax_year = '{2}'", m_sBIN, sReqCode, m_sTaxYear);
            if (pRec.Execute())
            {
                if (pRec.Read())
                    dgvList[0, iRow].Value = true;
                else
                {
                    dgvList[0, iRow].Value = false;
                    alDesc.Add(dgvList[1, iRow].Value.ToString());    // RMC 20171110 added configurable option to print temporary permit - requested by Malolos
                    alCode.Add(dgvList[2, iRow].Value.ToString());    // RMC 20171110 added configurable option to print temporary permit - requested by Malolos

                }
            }
            pRec.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pCmd = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();
            string sCode = "";
            DateTime dtYr = AppSettingsManager.GetSystemDate();
            string sYr = dtYr.Year.ToString(); //AFM 20200106 get current year
            string sYear2 = string.Empty;

            pRec.Query = "select distinct(tax_year) from requirements_tbl where bin = '" + m_sBIN + "'"; //AFM 20200106 check if has existing checklist
            if (pRec.Execute())
                if (pRec.Read())
                {
                    sYear2 = pRec.GetString("tax_year");
                }

            if (m_sTaxYear != sYear2)//AFM 20200106 added condition to determine year
                m_sTaxYear = sYr;

            pCmd.Query = string.Format("delete from requirements_tbl where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear); //m_sTaxYear //AFM 20200106
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            int iRow = 0;
            int iCntRecord = 0;
            for (iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                if ((bool)dgvList[0, iRow].Value)
                {
                    iCntRecord++;
                    sCode = dgvList[2,iRow].Value.ToString().Trim();

                    pCmd.Query = "insert into requirements_tbl values (:1,:2,'Y',:3, :4)";
                    pCmd.AddParameter(":1", m_sBIN);
                    pCmd.AddParameter(":2", sCode);
                    //pCmd.AddParameter(":3", m_sTaxYear);
                    pCmd.AddParameter(":3", sYr); //AFM 20200106 saves current year malolos ver.
                    pCmd.AddParameter(":4", AppSettingsManager.SystemUser.UserCode);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
            }

            if (iCntRecord > 0)
            {
                MessageBox.Show("Checklist saved", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string sModuleCode = "";
                string strObj = "Added document submitted for BIN: " + m_sBIN + ", tax year: " + m_sTaxYear;

                if (m_sSource == "PERMIT")
                    sModuleCode = "APAC";
                else
                    sModuleCode = "AAAC";

                if (AuditTrail.InsertTrail(sModuleCode, "requirements_tbl", strObj) == 0)
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("No record to save", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if ((bool)dgvList[0, e.RowIndex].Value == true)
                    dgvList[0, e.RowIndex].Value = false;
                else
                    dgvList[0, e.RowIndex].Value = true;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string sTmpList = "";
            m_sTempList = "";

            for (int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                if ((bool)dgvList[0, iRow].Value)
                {
                    sTmpList = dgvList[1, iRow].Value.ToString().Trim();
                    if (m_sTempList != "")
                        //m_sTempList += "\n";
                        m_sTempList += " ";    // RMC 20171206 adjust printing of for compliance in temp permit
                    m_sTempList += "*" + sTmpList;
                }
            }

            if (m_sTempList == "")
            {
                MessageBox.Show("Select list to be complied for temporary permit printing","Temporary Permit",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            this.Close();
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                for (int i = 0; i < dgvList.RowCount; i++)
                    dgvList.Rows[i].Cells[0].Value = true;
            }
            else
            {
                for (int i = 0; i < dgvList.RowCount; i++)
                    dgvList.Rows[i].Cells[0].Value = false;
            }
        } //MCR 20191126

    }
}