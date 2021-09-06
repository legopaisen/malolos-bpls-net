
// RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.BusinessType;
using Amellar.Common.BPLSApp;
using Amellar.Common.AddlInfo;
using Amellar.BPLS.Billing;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.AdditionalBusiness
{
    public partial class frmAdditionalBusiness : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        private bool m_bNewAddlFlag = false;
        private bool m_SwLoad = false;
        private bool m_bIsApplSave = false;
        private string m_strBIN = string.Empty;
        private string m_strStatus = string.Empty;
        private string m_strMainBnsDesc = string.Empty;
        private string m_strBnsCode = string.Empty;
        private string m_strBnsDesc = string.Empty;
        private string m_strCapital = string.Empty;
        private string m_strGross = string.Empty;
	    private string m_strQtr = string.Empty;
        private string m_strRevYear = string.Empty;
        private string m_strPrevValue = string.Empty;
        private string[] m_sArrayBnsCode = new string[] { "" };
        private DateTime m_odtOperationStart;
        DataGridViewComboBoxColumn comboMainBns = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn comboSub = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxColumn comboStatus = new DataGridViewComboBoxColumn();
        DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
        private int m_iActiveCellCol = 0;   // RMC 20110824
        private int m_iActiveCellRow = 0;   // RMC 20110824
        Billing BillingClass = null;
        private string m_strTransCode = string.Empty;   // RMC 20110809
        private DateTime m_dLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private bool hasQue = false; //AFM 20200123

        public string BIN
        {
            get { return m_strBIN; }
            set { m_strBIN = value; }
        }

        public string Status
        {
            get { return m_strStatus; }
            set { m_strStatus = value; }
        }

        public string Qtr
        {
            get { return m_strQtr; }
            set { m_strQtr = value; }
        }

        public string MainBnsDesc
        {
            get { return m_strMainBnsDesc; }
            set { m_strMainBnsDesc = value; }
        }

        public bool AddlFlag
        {
            get { return m_bNewAddlFlag; }
            set { m_bNewAddlFlag = value; }
        }

        public DateTime OperationStart
        {
            get { return m_odtOperationStart; }
            set { m_odtOperationStart = value; }
        }

        public bool ApplSave
        {
            get { return m_bIsApplSave; }
            set { m_bIsApplSave = value; }
        }

        public string TransCode // RMC 20110809
        {
            get { return m_strTransCode; }
            set { m_strTransCode = value; }
        }

        public frmAdditionalBusiness()
        {
            InitializeComponent();

            
        }

        private void frmAdditionalBusiness_Load(object sender, EventArgs e)
        {
            try
            {
                m_strRevYear = AppSettingsManager.GetConfigValue("07");
                txtBIN.Text = m_strBIN;

                dgvList.Columns.Clear();

                dgvList.Columns.Add("SC", "Sub-Category");
                dgvList.Columns.Add("CAP", "Capital");
                dgvList.Columns.Add("PREVGR", "Prev. Gross");   // RMC 20110809
                dgvList.Columns.Add("GR", "Gross");
                
                LoadComboValues();

                dgvList.Rows[0].Cells[1] = comboCell;

                dgvList.RowHeadersVisible = false;
                dgvList.Columns[0].Width = 175;
                dgvList.Columns[1].Width = 175;
                /*dgvList.Columns[2].Width = 100;
                dgvList.Columns[3].Width = 100;
                dgvList.Columns[4].Width = 70;*/
                dgvList.Columns[2].Width = 90;  // RMC 20110809
                dgvList.Columns[3].Width = 90;  // RMC 20110809
                dgvList.Columns[4].Width = 90;  // RMC 20110809
                dgvList.Columns[5].Width = 70;  // RMC 20110809
                dgvList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvList.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvList.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvList.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;  // RMC 20110809
                dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvList.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvList.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;  // RMC 20110809

                if (m_strBIN.Length == 19)
                {
                    txtBIN.Text = m_strBIN;

                    LoadValues();
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteAddlQue() //AFM 20200123 MAO-20-11967
        {
            OracleResultSet result = new OracleResultSet();
            int iCnt = 0;
            result.Query = @"select count(*) from addl_bns_que where bin = '" + m_strBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);
            if (iCnt > 0)
            {
                result.Query = @"delete from addl_bns_que where bin = '" + m_strBIN + "' and tax_year = '" + txtTaxYear.Text.Trim() + "'";
                result.ExecuteNonQuery();
            }
            result.Close();
        }

        private void LoadValues()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pRec2 = new OracleResultSet();

            string bnsCapital = string.Empty;
            string bnsGross = string.Empty;
            string bnsPrevGross = string.Empty; // RMC 20110809
            string bnsStatus = string.Empty;
            string bnsCode = string.Empty;
            string bnsCodeMain = string.Empty;
            string bnsDescMain = string.Empty;
            string bnsDesc = string.Empty;
            string strTaxYear = string.Empty;
            string strQtr = string.Empty;
            string strBnsCodeOld = string.Empty;
            double m_dCapital = 0;
			double m_dGross = 0;
			int iRow = 0;      

            if (m_bNewAddlFlag)
	        {
				InitNewAddl();

                // RMC 20170109 enable adding of new buss in renewal application (s)
                if (m_strTransCode == "REN-APP-EDIT" || m_strTransCode == "REN-APP" || m_strTransCode == "NEW-APP" || m_strTransCode == "NEW-APP-EDIT")
                { }// RMC 20170109 enable adding of new buss in renewal application (e)
                else
                return;
		    }
	
            if(StringUtilities.Left(m_strStatus,4) == "BUSS" && !Granted.Grant("ABECP"))
                btnCompute.Visible = false;
		        	
	        if (txtBnsStat.Text.Trim() == "NEW") 
		        m_strQtr = AppSettingsManager.GetQtr(m_odtOperationStart.ToString());
            else 
		        m_strQtr = "1";


            // RMC 20110824 (s)
            pSet.Query = string.Format("select bns_code from business_que where bin = '{0}'", m_strBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    strBnsCodeOld = pSet.GetString("bns_code");
                else
                {
                    pSet.Close();
                    // RMC 20110824 (e)
                    pSet.Query = string.Format("select bns_code from businesses where bin = '{0}'", m_strBIN);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                            strBnsCodeOld = pSet.GetString("bns_code");
                    }
                    pSet.Close();
                }
            }

            if (m_bNewAddlFlag) //AFM 2000123 MAO-20-11967 load addl bns que when it has data
            {
                if(hasQue == true)
                {
                    if (MessageBox.Show("Detected new changes lately on additional business. Discard changes?", "Additional Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        DeleteAddlQue(); //AFM 20200123 MAO-20-11967
                        dgvList.Rows.Clear();
                    }

                }
            }

            // RMC 20110824 (s)
            pSet.Query = string.Format("select count(*) from addl_bns where bin = '{0}' and tax_year = '{1}'", m_strBIN,txtTaxYear.Text.Trim());
            pSet.Query += string.Format(" and bns_code_main not in (select bns_code_main from retired_bns where bin = '{0}' and tax_year = '{1}')", m_strBIN, txtTaxYear.Text.Trim());
	        pSet.Query += string.Format(" and bns_code_main <> '{0}'", strBnsCodeOld);
            int iRecCnt = 0;
            int.TryParse(pSet.ExecuteScalar(), out iRecCnt);

            if (iRecCnt == 0)
            {
                pSet.Query = string.Format("select count(*) from addl_bns where bin = '{0}' order by tax_year desc", m_strBIN);
                int.TryParse(pSet.ExecuteScalar(), out iRecCnt);
            }
            m_sArrayBnsCode = new string[iRecCnt];
            // RMC 20110824 (e)
            
            pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", m_strBIN,txtTaxYear.Text.Trim());
            pSet.Query += string.Format(" and bns_code_main not in (select bns_code_main from retired_bns where bin = '{0}' and tax_year = '{1}')", m_strBIN, txtTaxYear.Text.Trim());
	        pSet.Query += string.Format(" and bns_code_main <> '{0}'", strBnsCodeOld);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    m_SwLoad = true;
                    btnAdd.Enabled = true;
                    //btnSave.Enabled = true;

                    pSet.Close();
                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            m_dCapital = 0;
                            m_dGross = 0;
                            m_dCapital = pSet.GetDouble("capital");
                            m_dGross = pSet.GetDouble("gross");

                            bnsCapital = string.Format("{0:#,##.00}", m_dCapital);
                            bnsGross = string.Format("{0:#,##.00}", m_dGross);
                            bnsStatus = pSet.GetString("bns_stat");

                            /*if (txtBnsStat.Text.Trim() == "REN" && bnsStatus == "NEW")
                            {
                                bnsStatus = txtBnsStat.Text.Trim();
                                bnsCapital = "0.00";
                            }*/

                            // RMC 20110809 (s)
                            bnsPrevGross = string.Format("{0:#,##.00}", pSet.GetDouble("prev_gross"));  
                            if (m_strTransCode == "REN-APP")
                            {
                                bnsStatus = "REN";
                                bnsCapital = "0.00";
                            
                            }
                            // RMC 20110809 (e)

                            bnsCode = pSet.GetString("bns_code_main").Trim();
                            m_sArrayBnsCode[iRow] = bnsCode;
                            bnsCodeMain = StringUtilities.Left(bnsCode, 2);

                            pRec.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCodeMain, m_strRevYear);
                            bnsDescMain = "";
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                    bnsDescMain = pRec.GetString("bns_desc");
                            }
                            pRec.Close();

                            
                            pRec.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCode, m_strRevYear);
                            bnsDesc = "";
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                    bnsDesc = pRec.GetString("bns_desc");
                            }
                            pRec.Close();

                            dgvList.Rows.Add("");

                            LoadComboCell(bnsCode); // RMC 20110824

                            dgvList[0, iRow].Value = bnsDescMain;
                            dgvList[1, iRow].Value = bnsDesc;
                            dgvList[2, iRow].Value = bnsCapital;
                            //dgvList[3, iRow].Value = bnsGross;
                            //dgvList[4, iRow].Value = bnsStatus;
                            dgvList[3, iRow].Value = bnsPrevGross;  // RMC 20110809
                            dgvList[4, iRow].Value = bnsGross;  // RMC 20110809
                            dgvList[5, iRow].Value = bnsStatus; // RMC 20110809

                            iRow++;
                            m_iActiveCellRow++; // RMC 20110824
                        }
                    }
                }
                else
                {
                    pSet.Close();


                    // pSet.Query = string.Format("select * from addl_bns where bin = '{0}' order by tax_year desc", m_strBIN);


                    pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and bns_stat <> 'RET' order by tax_year desc", m_strBIN);  // RMC 20160113 corrections in updating bns_stat in Retirement transaction
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strTaxYear = pSet.GetString("tax_year");
                            strQtr = pSet.GetString("qtr");

                            pRec.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", m_strBIN, strTaxYear);
                            pRec.Query += string.Format(" and bns_code_main not in (select bns_code_main from retired_bns where bin = '{0}' and tax_year = '{1}')", m_strBIN, txtTaxYear.Text.Trim());
                            pRec.Query += string.Format(" and bns_code_main <> '{0}'", StringUtilities.SetEmptyToSpace(strBnsCodeOld));
                            iRow = 0;
                            if (pRec.Execute())
                            {
                                m_SwLoad = true;
                                btnAdd.Enabled = true;
                                //btnSave.Enabled = true;

                                while (pRec.Read())
                                {

                                    m_dCapital = 0;
                                    m_dGross = 0;
                                    m_dCapital = pRec.GetDouble("capital");
                                    m_dGross = pRec.GetDouble("gross");

                                    bnsCapital = string.Format("{0:#,##.00}", m_dCapital);
                                    bnsGross = string.Format("{0:#,##.00}", m_dGross);
                                    
                                    bnsStatus = pRec.GetString("bns_stat");

                                    /*if (txtBnsStat.Text.Trim() == "REN" && bnsStatus == "NEW")
                                    {
                                        bnsStatus = txtBnsStat.Text.Trim();
                                        bnsCapital = "0.00";
                                    }*/

                                    // RMC 20110809 (s)
                                    bnsPrevGross = string.Format("{0:#,##.00}", pSet.GetDouble("prev_gross"));
                                    if (m_strTransCode == "REN-APP" || m_strTransCode == "REN-APP-EDIT")
                                    {
                                        bnsStatus = "REN";
                                        bnsCapital = "0.00";

                                        if (bnsStatus == "NEW")
                                            bnsPrevGross = bnsCapital;
                                        else
                                            bnsPrevGross = bnsGross;
                                    }
                                    // RMC 20110809 (e)


                                    bnsCode = pRec.GetString("bns_code_main");
                                    m_sArrayBnsCode[iRow] = bnsCode;
                                    bnsCodeMain = StringUtilities.Left(bnsCode, 2);

                                    pRec2.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCodeMain, m_strRevYear);
                                    bnsDescMain = "";
                                    if (pRec2.Execute())
                                    {
                                        if (pRec2.Read())
                                            bnsDescMain = pRec2.GetString("bns_desc");
                                    }
                                    pRec2.Close();

                                    pRec2.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCode, m_strRevYear);
                                    bnsDesc = "";
                                    if (pRec2.Execute())
                                    {
                                        if (pRec2.Read())
                                            bnsDesc = pRec2.GetString("bns_desc");
                                    }
                                    pRec2.Close();

                                    dgvList.Rows.Add("");

                                    LoadComboCell(bnsCode); // RMC 20110824

                                    dgvList[0, iRow].Value = bnsDescMain;
                                    dgvList[1, iRow].Value = bnsDesc;
                                    dgvList[2, iRow].Value = bnsCapital;
                                    //dgvList[3, iRow].Value = bnsGross;
                                    //dgvList[4, iRow].Value = bnsStatus;
                                    dgvList[3, iRow].Value = bnsPrevGross;  // RMC 20110809
                                    dgvList[4, iRow].Value = bnsGross;  // RMC 20110809
                                    dgvList[5, iRow].Value = bnsStatus; // RMC 20110809

                                    iRow++;
                                    m_iActiveCellRow++; // RMC 20110824

                                }
                            }
                            pRec.Close();
                        }
                        else
                        {
                            btnAdd.Enabled = false;
                            m_SwLoad = false;
                        }
                    }
                    pSet.Close();
                }
            }

            //btnSave.Enabled = false;
            
		
        }

        private void InitNewAddl()
        {
            OracleResultSet pRec = new OracleResultSet();
            int iRow = 0;

            string bnsCapital = string.Empty;
            string bnsGross = string.Empty;
            string bnsPrevGross = string.Empty; // RMC 20110809
            string bnsStatus = string.Empty;
            string bnsCode = string.Empty;
            string bnsCodeMain = string.Empty;
            string bnsDescMain = string.Empty;
            string bnsDesc = string.Empty;
            string strTaxYear = string.Empty;
            string strQtr = string.Empty;
            string strBnsCodeOld = string.Empty;
            double m_dCapital = 0;
            double m_dGross = 0;

            // RMC 20110824 (s)
            pSet.Query = string.Format("select count(*) from addl_bns_que where bin = '{0}'  and tax_year = '{1}' and bns_stat <> 'RET'", m_strBIN, txtTaxYear.Text.ToString());
            int iRecCnt = 0;
            int.TryParse(pSet.ExecuteScalar(), out iRecCnt);
            m_sArrayBnsCode = new string[iRecCnt];
            // RMC 20110824 (e)

            pSet.Query = string.Format("select * from addl_bns_que where bin = '{0}'  and tax_year = '{1}' and bns_stat <> 'RET'",m_strBIN, txtTaxYear.Text.ToString());
	        if(pSet.Execute())
	        {
                if (pSet.Read())
                {
                    iRow = 0;
                    m_strQtr = pSet.GetString("qtr");
                    btnAdd.Enabled = true;
                    //btnSave.Enabled = true;

                    pSet.Close();

                    if (pSet.Execute())
                    {
                        while (pSet.Read())
                        {
                            m_dCapital = 0;
                            m_dGross = 0;
                            m_dCapital = pSet.GetDouble("capital");
                            m_dGross = pSet.GetDouble("gross");
                            bnsPrevGross = string.Format("{0:#,##.00}", pSet.GetDouble("prev_gross"));  // RMC 20110809

                            bnsCapital = string.Format("{0:#,##.00}", m_dCapital);
                            bnsGross = string.Format("{0:#,##.00}", m_dGross);

                            bnsStatus = pSet.GetString("bns_stat");
                            bnsCode = pSet.GetString("bns_code_main");
                            m_sArrayBnsCode[iRow] = bnsCode;

                            bnsCodeMain = StringUtilities.Left(bnsCode, 2);

                            pRec.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCodeMain, m_strRevYear);
                            if (pRec.Execute())
                            {
                                bnsDescMain = "";
                                if (pRec.Read())
                                    bnsDescMain = pRec.GetString("bns_desc");
                            }
                            pRec.Close();

                            pRec.Query = string.Format("select * from bns_table where bns_code = '{0}' and fees_code = 'B' and rev_year = '{1}'", bnsCode, m_strRevYear);
                            if (pRec.Execute())
                            {
                                bnsDesc = "";
                                if (pRec.Read())
                                    bnsDesc = pRec.GetString("bns_desc");
                            }
                            pRec.Close();

                            dgvList.Rows.Add("");

                            LoadComboCell(bnsCode); // RMC 20110824

                            dgvList[0, iRow].Value = bnsDescMain;
                            dgvList[1, iRow].Value = bnsDesc;
                            dgvList[2, iRow].Value = bnsCapital;
                            //dgvList[3, iRow].Value = bnsGross;
                            //dgvList[4, iRow].Value = bnsStatus;
                            dgvList[3, iRow].Value = bnsPrevGross;  // RMC 20110809
                            dgvList[4, iRow].Value = bnsGross;  // RMC 20110809
                            dgvList[5, iRow].Value = bnsStatus; // RMC 20110809
                            
                            iRow++;
                            m_iActiveCellRow++; // RMC 20110824

                        }
                        hasQue = true; //AFM 20200123
                    }
                    pSet.Close();
                }
                else
                    btnAdd.Enabled = false;
            }
	    }
                
        private void dgvList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            double dblValue = 0;
            string strMeans = string.Empty;
            string bnsCode = string.Empty;

            m_strPrevValue = "0";

            try
            {
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4)  // RMC 20110809
                {
                    if (dgvList[e.ColumnIndex, e.RowIndex].Value != null)
                    {
                        try
                        {
                            dblValue = Convert.ToDouble(dgvList[e.ColumnIndex, e.RowIndex].Value);
                        }
                        catch
                        {
                            dblValue = 0;
                        }

                        m_strPrevValue = string.Format("{0:#,##.00}", dblValue);
                    }
                }

                m_iActiveCellCol = e.ColumnIndex;
                m_iActiveCellRow = e.RowIndex;

                if (e.ColumnIndex == 1)
                {
                    string strDesc = string.Empty;
                    if (dgvList[0, e.RowIndex].Value != null)
                    {
                        strDesc = dgvList[0, e.RowIndex].Value.ToString();

                        if (strDesc == "")
                            return;

                        pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}' order by bns_code", strDesc, m_strRevYear);
                        if (pSet.Execute())
                        {
                            bnsCode = "";
                            if (pSet.Read())
                            {
                                bnsCode = pSet.GetString("bns_code");
                            }
                        }
                        pSet.Close();

                        LoadComboCell(bnsCode); // RMC 20110824
                        /*comboCell = new DataGridViewComboBoxCell();
                        dgvList.Rows[e.RowIndex].Cells[1] = comboCell;

                        string strRowValue = string.Empty;
                        pSet.Query = string.Format("select * from bns_table where trim(bns_code) like '{0}%' and fees_code = 'B'", bnsCode);
                        pSet.Query += string.Format(" and length(trim(bns_code)) > 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
                        if (pSet.Execute())
                        {
                            comboCell.Items.Clear();

                            while (pSet.Read())
                            {
                                comboCell.Items.AddRange(pSet.GetString("bns_desc"));


                            }
                        }
                        pSet.Close();*/





                    }
                }
            }
            catch
            {
            }
        }

        private void LoadComboValues()
        {
            comboMainBns.HeaderCell.Value = "Main Businesses";
            dgvList.Columns.Insert(0, comboMainBns);

            comboMainBns.Items.Clear();

            //dgvList.Rows.Add("");

            pSet.Query = string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    comboMainBns.Items.AddRange(pSet.GetString("bns_desc"));
                }
            }
            pSet.Close();

            /*comboSub.HeaderCell.Value = "Sub-Category";
            dgvList.Columns.Insert(1, comboSub);
            comboSub.Items.Clear();

            pSet.Query = string.Format("select * from bns_table where fees_code = 'B' and length(trim(bns_code)) > 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    comboSub.Items.AddRange(pSet.GetString("bns_desc"));
                    
                }
            }
            pSet.Close();*/

            comboStatus.HeaderCell.Value = "Status";
            //dgvList.Columns.Insert(4, comboStatus);
            dgvList.Columns.Insert(5, comboStatus); // RMC 20110809 
            comboStatus.Items.Clear();

            // RMC 20110809 (s) Note: NEW other line in renewal should be applied on Permit-update
            if (m_strTransCode == "REN-APP-EDIT" || m_strTransCode == "REN-APP")
                //comboStatus.Items.AddRange("REN");  // RMC 20110809 (e)
                comboStatus.Items.AddRange("NEW", "REN");   // RMC 20170109 enable adding of new buss in renewal application
            else if (m_strTransCode == "NEW-APP" || m_strTransCode == "NEW-APP-EDIT")
                comboStatus.Items.AddRange("NEW");
            else
                comboStatus.Items.AddRange("NEW", "REN");
            
        }

        
        private void dgvList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string sTemp = "";
                
                if (e.ColumnIndex == 0)
                {
                    if (dgvList[0, e.RowIndex].Value != null)
                    {
                        comboCell = new DataGridViewComboBoxCell();
                        dgvList.Rows[e.RowIndex].Cells[1] = comboCell;

                        // RMC 20110824
                        sTemp = dgvList[0, e.RowIndex].Value.ToString().ToUpper();
                        dgvList[0, e.RowIndex].Value = sTemp;
                        // RMC 20110824
                    }
                }

                if (e.ColumnIndex == 1)
                {
                    if (dgvList[1, e.RowIndex].Value != null)
                    {
                        // RMC 20110824
                        sTemp = dgvList[1, e.RowIndex].Value.ToString().ToUpper();
                        dgvList[1, e.RowIndex].Value = sTemp;
                        // RMC 20110824

                        if (m_strMainBnsDesc == dgvList[1, e.RowIndex].Value.ToString().Trim())
                        {
                            MessageBox.Show("Cannot add additional business which is your main business.", "Additional Business", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dgvList[1, e.RowIndex].Value = "";
                        }
                        else
                        {
                            for (int i = 0; i < dgvList.Rows.Count; i++)
                            {
                                if (i != e.RowIndex)
                                {
                                    if (dgvList[1, i].Value != null)
                                    {
                                        if (dgvList[1, e.RowIndex].Value.ToString().Trim() == dgvList[1, i].Value.ToString().Trim())
                                        {
                                            MessageBox.Show("Business type already exist.", "Additional Business", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                            dgvList[1, e.RowIndex].Value = "";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4) // RMC 20110809
                {
                    string strValue = string.Empty;

                    //strValue = dgvList[e.ColumnIndex, e.RowIndex].Value.ToString();

                    try
                    {
                        strValue = dgvList[e.ColumnIndex, e.RowIndex].Value.ToString(); // RMC 20110809
                        strValue = string.Format("{0:#,###.##}", Convert.ToDouble(strValue));
                        if (strValue == "") // RMC 20110824
                            strValue = "0"; // RMC 20110824
                        dgvList[e.ColumnIndex, e.RowIndex].Value = strValue;    // RMC 20110809
                    }
                    catch
                    {
                        MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dgvList[e.ColumnIndex, e.RowIndex].Value = string.Format("{0:#,###.##}", Convert.ToDouble(m_strPrevValue)); ;
                        return;
                    }
                }

                if (m_bNewAddlFlag)
                {
                    string bnsDesc = string.Empty;
                    string bnsCode = string.Empty;

                    if (dgvList[1, e.RowIndex].Value != null)
                        bnsDesc = dgvList[1, e.RowIndex].Value.ToString();

                    pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'", bnsDesc, m_strRevYear);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            bnsCode = pSet.GetString("bns_code");
                        }
                    }
                    pSet.Close();

                    pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and bns_code_main = '{1}' and tax_year = '{2}'", m_strBIN, bnsCode, txtTaxYear.Text);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            MessageBox.Show("Cannot add additional business already exist.", "Additional Business", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dgvList[1, e.RowIndex].Value = "";
                        }
                    }
                    pSet.Close();

                    //dgvList[4,e.RowIndex].Value = "NEW";
                    //dgvList[5, e.RowIndex].Value = "NEW";   // RMC 20110809   // RMC 20170109 enable adding of new buss in renewal application, put rem
                }

                if (StringUtilities.Left(txtBnsStat.Text.Trim(), 3) == "NEW")
                    //dgvList[4,e.RowIndex].Value = "NEW";
                    dgvList[5, e.RowIndex].Value = "NEW";   // RMC 20110809

                /*if (dgvList[4, e.RowIndex].Value != null)
                {
                    if (dgvList[4, e.RowIndex].Value.ToString() == "REN")
                        dgvList[2, e.RowIndex].Value = "";
                    else if (dgvList[4, e.RowIndex].Value.ToString() == "NEW")
                        dgvList[3, e.RowIndex].Value = "";
                }*/

                // RMC 20110809 (s)
                if (dgvList[5, e.RowIndex].Value != null)   // RMC 20110809
                {
                    if (dgvList[5, e.RowIndex].Value.ToString() == "REN")
                        dgvList[2, e.RowIndex].Value = "";
                    else if (dgvList[5, e.RowIndex].Value.ToString() == "NEW")
                        dgvList[4, e.RowIndex].Value = "";
                }
                // RMC 20110809 (e)

                /* m_strStatus = dgvList[4,e.RowIndex].Value.ToString();

                 if((dgvList[4,e.RowIndex].Value.ToString() != StringUtilities.StringUtilities.Left(m_strStatus,3) && 
                     StringUtilities.StringUtilities.Left(m_strStatus,3) == "NEW") && (dgvList[4,e.RowIndex].Value.ToString() != "")) 
                 {
                     MessageBox("Cannot apply business with different business status","Additional Business",16);
                     m_fArray.SetTextMatrix(Row,2,"");
                     m_fArray.SetTextMatrix(Row,3,"");
                     m_fArray.SetTextMatrix(Row,4,"");
                 }????? */

                if (e.ColumnIndex == 0)
                {
                    if (dgvList[0, e.RowIndex].Value != null)
                    {
                        if (dgvList[0, e.RowIndex].Value.ToString() != "PEDDLERS")
                            dgvList[1, e.RowIndex].Value = "";
                        else
                        {
                            dgvList[1, e.RowIndex].Value = "PEDDLERS";
                        }
                    }
                }

                try
                {
                    /*if (dgvList[0, e.RowIndex].Value.ToString() != "" && dgvList[1, e.RowIndex].Value.ToString() != ""
                        && (dgvList[2, e.RowIndex].Value.ToString() != "" || dgvList[3, e.RowIndex].Value.ToString() != "")
                        && dgvList[4, e.RowIndex].Value.ToString() != "")*/

                    if (dgvList[0, e.RowIndex].Value.ToString() != "" && dgvList[1, e.RowIndex].Value.ToString() != ""
                        && (dgvList[2, e.RowIndex].Value.ToString() != "" || dgvList[4, e.RowIndex].Value.ToString() != "")
                        && dgvList[5, e.RowIndex].Value.ToString() != "")   // RMC 20110809
                    {
                        //btnSave.Enabled = true;
                        btnAdd.Enabled = true;
                    }
                    else
                        btnAdd.Enabled = false;

                }
                catch
                {
                    btnAdd.Enabled = false;
                }

            }
            catch 
            { }
        }

        private void btnAddlBnsInfo_Click(object sender, EventArgs e)
        {
            if (m_strBnsCode == "")
            {
                MessageBox.Show("Select business code.");
                return;
            }

            using (frmAddlInfo frmGrid = new frmAddlInfo())
            {
                frmGrid.BnsCode = m_strBnsCode;
                frmGrid.TaxYear = txtTaxYear.Text.ToString().Trim();
                frmGrid.RevYear = AppSettingsManager.GetConfigValue("07");
                frmGrid.BIN = m_strBIN;
                frmGrid.ShowDialog();
            }
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string strBnsDesc = string.Empty;
            m_strBnsCode = "";

            try
            {
                if (dgvList[1, e.RowIndex].Value != null)
                    strBnsDesc = dgvList[1, e.RowIndex].Value.ToString();
            }
            catch
            {
                strBnsDesc = "";
            }

            pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'",StringUtilities.HandleApostrophe(strBnsDesc),m_strRevYear);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    m_strBnsCode = pSet.GetString("bns_code");
            }
            pSet.Close();

            try
            {
                m_strCapital = dgvList[2, e.RowIndex].Value.ToString();
            }
            catch
            {
                m_strCapital = "";
            }

            try
            {
                //m_strGross = dgvList[3, e.RowIndex].Value.ToString();
                m_strGross = dgvList[4, e.RowIndex].Value.ToString();
            }
            catch
            {
                m_strGross = "";
            }

            try
            {
                //m_strStatus = dgvList[4, e.RowIndex].Value.ToString();
                m_strStatus = dgvList[5, e.RowIndex].Value.ToString();  // RMC 20110809
            }
            catch
            {
                m_strStatus = "";
            }

            try
            {
                // RMC 20110809 (s)
                if (e.ColumnIndex == 3 && (m_strTransCode == "REN-APP" || m_strTransCode == "REN-APP-EDIT"))
                    dgvList.ReadOnly = true;
                else
                    dgvList.ReadOnly = false;
                // RMC 20110809 (e)

                m_iActiveCellCol = e.ColumnIndex;
                m_iActiveCellRow = e.RowIndex;
            }
            catch
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();

            // RMC 20110824 (s)
            int iArrayRowCnt = 0;
            string sTmpBnsCode = "";
            foreach (string s in m_sArrayBnsCode)
            {
                iArrayRowCnt++;
            }
            // RMC 20110824 (e)

            if (m_bNewAddlFlag)
		    {
                pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}'", m_strBIN);
                pSet.Query += string.Format(" and bns_code in (select bns_code_main from addl_bns_que where bin = '{0}' and tax_year = '{1}')", m_strBIN, txtTaxYear.Text.Trim());
                pSet.Query += string.Format(" and tax_year = '{0}'", txtTaxYear.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

			    pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}' and tax_year = '{1}'",m_strBIN,txtTaxYear.Text.Trim());
                pSet.Query+= string.Format(" and bin in (select bin from permit_update_appl where bin = '{0}'", m_strBIN);
			    pSet.Query+= string.Format(" and tax_year = '{0}' and appl_type = 'ADDL' and data_mode = 'QUE')", txtTaxYear.Text.Trim());
                if(pSet.ExecuteNonQuery() == 0)
                {
                }

                if (m_strTransCode == "REN-APP-EDIT" || m_strTransCode == "REN-APP" || m_strTransCode == "NEW-APP" || m_strTransCode == "NEW-APP-EDIT") //AFM 20200123
                {
                    pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}' and tax_year = '{1}'", m_strBIN, txtTaxYear.Text.Trim());
                    if (pSet.ExecuteNonQuery() == 0)
                    {
                    }
                }
			}
		    else
		    {
                pSet.Query = string.Format("delete from bill_gross_info where bin = '{0}'", m_strBIN);
                pSet.Query += string.Format(" and bns_code in (select bns_code_main from addl_bns_que where bin = '{0}' and tax_year = '{1}')", m_strBIN, txtTaxYear.Text.Trim());
                pSet.Query += string.Format(" and tax_year = '{0}'", txtTaxYear.Text.Trim());
                if (pSet.ExecuteNonQuery() == 0)
                {
                }

		        pSet.Query = string.Format("delete from addl_bns where bin = '{0}' and tax_year = '{1}' and bns_stat <> 'RET'",m_strBIN,txtTaxYear.Text.Trim());
                if(pSet.ExecuteNonQuery() == 0)
                {
                }
		    }

            int iRow = 0;   // RMC 20110824
            for (iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                try
                {
                    if (dgvList[0, iRow].Value == null || dgvList[0, iRow].Value == "") // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross 
                        break;

                    // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross (s)
                    if ((dgvList[1, iRow].Value != null && dgvList[5, iRow].Value == null))
                    {
                        MessageBox.Show("Please specify business status", "Business Records", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross (e)

                    /*if(txtBnsStat.Text.Trim() == "NEW")
                        dgvList[3,iRow].Value = "0.00";
				
                    if(dgvList[4,iRow].Value.ToString() == "NEW")
                        dgvList[3,iRow].Value = "0.00";

                    if(dgvList[4,iRow].Value.ToString() == "REN")
                        dgvList[2,iRow].Value = "0.00";*/

                    // RMC 20110809 (s)
                    if (txtBnsStat.Text.Trim() == "NEW")
                        dgvList[4, iRow].Value = "0.00";

                    if (dgvList[5, iRow].Value.ToString() == "NEW")
                        dgvList[4, iRow].Value = "0.00";

                    if (dgvList[5, iRow].Value.ToString() == "REN")
                        dgvList[2, iRow].Value = "0.00";

                    if (dgvList[3, iRow].Value == null)
                        dgvList[3, iRow].Value = "0.00";
                    // RMC 20110809 (e)
                }
                catch { }

                string s1 = string.Empty;
                string s2 = string.Empty;
                string s3 = string.Empty;
                string bnsCode = string.Empty;
                string strQtr = string.Empty;
                string strDateOperated = string.Format("{0:MM/dd/yyyy}", m_odtOperationStart);
                string strCapital = string.Empty;
                string strGross = string.Empty;
                string strPrevGross = string.Empty; // RMC 20110809
                //string strBnsStat = dgvList[4,iRow].Value.ToString().Trim();
                string strBnsStat = dgvList[5, iRow].Value.ToString().Trim();   // RMC 20110809
                string xBin = string.Empty;
                string strOrNumber = string.Empty;
                double dCapital = 0;
                double dGross = 0;
                double dPrevGross = 0;  // RMC 20110809

                try
                {
                    s1 = dgvList[0, iRow].Value.ToString().Trim();
                    s2 = dgvList[1, iRow].Value.ToString().Trim();
                    //s3 = dgvList[4,iRow].Value.ToString().Trim();
                    s3 = dgvList[5, iRow].Value.ToString().Trim();  // RMC 20110809
                }
                catch
                {
                }

                if (s1 != "" && s2 != "" && s3 != "")
                {
                    try
                    {
                        bnsCode = "";

                        pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'", s2, m_strRevYear);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                                bnsCode = pSet.GetString("bns_code");
                        }
                        pSet.Close();

                        if (dgvList[2, iRow].Value == null) // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross
                            dCapital = 0;
                        else
                            dCapital = Convert.ToDouble(dgvList[2, iRow].Value.ToString());
                        //dGross = Convert.ToDouble(dgvList[3,iRow].Value.ToString());
                        if (dgvList[4, iRow].Value == null) // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross
                            dGross = 0;
                        else
                            dGross = Convert.ToDouble(dgvList[4, iRow].Value.ToString());   // RMC 20110809
                        if (dgvList[3, iRow].Value == null) // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross
                            dPrevGross = 0;
                        else
                            dPrevGross = Convert.ToDouble(dgvList[3, iRow].Value.ToString());   // RMC 20110809

                        strCapital = string.Format("{0:##.00}", dCapital);
                        strGross = string.Format("{0:##.00}", dGross);
                        strPrevGross = string.Format("{0:##.00}", dPrevGross); // RMC 20110809

                        if (m_bNewAddlFlag)
                        {
                            strQtr = AppSettingsManager.GetQtr(strDateOperated);
                            //pSet.Query = "insert into addl_bns_que (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr) values (:1,:2,:3,:4,:5,:6,:7)";
                            pSet.Query = "insert into addl_bns_que (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr, prev_gross) values (:1,:2,:3,:4,:5,:6,:7,:8)"; // RMC 20110809
                        }
                        else
                        {
                            if (strBnsStat == "NEW")
                                strQtr = AppSettingsManager.GetQtr(strDateOperated);
                            else
                                strQtr = "1";

                            //pSet.Query = "insert into addl_bns (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr) values (:1,:2,:3,:4,:5,:6,:7)";
                            pSet.Query = "insert into addl_bns (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr, prev_gross) values (:1,:2,:3,:4,:5,:6,:7,:8)"; // RMC 20110809
                        }


                        pSet.AddParameter(":1", m_strBIN);
                        pSet.AddParameter(":2", bnsCode);
                        pSet.AddParameter(":3", strCapital);
                        pSet.AddParameter(":4", strGross);
                        pSet.AddParameter(":5", txtTaxYear.Text.Trim());
                        pSet.AddParameter(":6", strBnsStat);
                        pSet.AddParameter(":7", strQtr);
                        pSet.AddParameter(":8", strPrevGross);  // RMC 20110809
                        if (pSet.ExecuteNonQuery() == 0)
                        {
                        }

                        // RMC 20150205 discovery delinquent mods (s)
                        int intCurrentYear = 0;
                        int.TryParse(ConfigurationAttributes.CurrentYear, out intCurrentYear);
                        int iTaxYear = 0;
                        int.TryParse(txtTaxYear.Text.Trim(), out iTaxYear);
                        iTaxYear = iTaxYear + 1;
                        string sTaxYear = string.Empty;

                        while (iTaxYear <= intCurrentYear)
                        {
                            sTaxYear = string.Format("{0:####}", iTaxYear);

                            if (m_bNewAddlFlag)
                            {
                                strQtr = AppSettingsManager.GetQtr(strDateOperated);
                                //pSet.Query = "insert into addl_bns_que (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr) values (:1,:2,:3,:4,:5,:6,:7)";
                                pSet.Query = "insert into addl_bns_que (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr, prev_gross) values (:1,:2,:3,:4,:5,:6,:7,:8)"; // RMC 20110809
                            }
                            else
                            {
                                if (strBnsStat == "NEW")
                                    strQtr = AppSettingsManager.GetQtr(strDateOperated);
                                else
                                    strQtr = "1";

                                //pSet.Query = "insert into addl_bns (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr) values (:1,:2,:3,:4,:5,:6,:7)";
                                pSet.Query = "insert into addl_bns (bin, bns_code_main, capital, gross, tax_year, bns_stat, qtr, prev_gross) values (:1,:2,:3,:4,:5,:6,:7,:8)"; // RMC 20110809
                            }


                            pSet.AddParameter(":1", m_strBIN);
                            pSet.AddParameter(":2", bnsCode);
                            pSet.AddParameter(":3", strCapital);
                            pSet.AddParameter(":4", strGross);
                            pSet.AddParameter(":5", sTaxYear);
                            pSet.AddParameter(":6", "REN");
                            pSet.AddParameter(":7", strQtr);
                            pSet.AddParameter(":8", strPrevGross);  // RMC 20110809
                            if (pSet.ExecuteNonQuery() == 0)
                            {
                            }
                            iTaxYear = iTaxYear + 1;

                        }

                        UpdateBillGrossInfo(m_strBIN, bnsCode, txtTaxYear.Text.Trim(), strBnsStat, dCapital, dGross);
                        // RMC 20150205 discovery delinquent mods (e)

                        m_bIsApplSave = true; //permit update transaction


                        //MessageBox.Show("Additional business saved.", "Additional Business", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20110824 Transferred

                        string strObj = m_strBIN.Trim() + "/" + txtTaxYear.Text.Trim() + "/" + bnsCode;
                        //JARS 20180925 (S)
                        if(strBnsStat == "NEW")
                        {
                            strObj = strObj + "/CAPITAL:" + dCapital.ToString("#,##0.00");
                        }
                        else if(strBnsStat == "REN")
                        {
                            strObj = strObj + "/GROSS:" + dGross.ToString("#,##0.00");
                        }
                        //JARS 20180925 (E)
                        TransLog.UpdateLog(m_strBIN, txtBnsStat.Text, txtTaxYear.Text, "AAB", m_dLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per binTransLog.UpdateLog(TransferAppFrm.BIN, TransferAppFrm.BnsStat, TransferAppFrm.TaxYear, "AAAPUT", TransferAppFrm.LogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                        if (AuditTrail.InsertTrail("AAB", "addl_bns", StringUtilities.HandleApostrophe(strObj)) == 0)
                        {
                            pSet.Rollback();
                            pSet.Close();
                            MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch { }

                    if (m_SwLoad)  // Having Additional Business(es) loaded to flex array
                    {
                        xBin = m_strBIN;

                        // RMC 20110824 (s)
                        try
                        {
                            if (iRow < iArrayRowCnt)
                                sTmpBnsCode = m_sArrayBnsCode[iRow].ToString();
                            else
                                sTmpBnsCode = "";
                        }
                        catch
                        {
                            sTmpBnsCode = "";
                        }
                        // RMC 20110824 (e)

                        pSet.Query = string.Format("select or_no from pay_hist where bin = '{0}'", xBin);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                strOrNumber = pSet.GetString("or_no");

                                pRec.Query = string.Format("update or_table set bns_code_main = '{0}' where or_no = '{1}'", bnsCode, strOrNumber);
                                pRec.Query += string.Format(" and bns_code_main = '{0}'", sTmpBnsCode);    // RMC 20110824 changed m_sArrayBnsCode[iRow].ToString() to sTmpBnsCode
                                if (pRec.ExecuteNonQuery() == 0)
                                {
                                }
                            }
                        }
                        pSet.Close();

                        pRec.Query = string.Format("update taxdues set bns_code_main = '{0}' where bin = '{1}' ", bnsCode, xBin);
                        pRec.Query += string.Format("and bns_code_main = '{0}'", sTmpBnsCode); // RMC 20110824 changed m_sArrayBnsCode[iRow].ToString() to sTmpBnsCode
                        if (pRec.ExecuteNonQuery() == 0)
                        {
                        }

                        pRec.Query = string.Format("update taxdues_hist set bns_code_main = '{0}' where bin = '{1}' ", bnsCode, xBin);
                        pRec.Query += string.Format("and bns_code_main = '{0}' and tax_year = '{1}'", sTmpBnsCode, txtTaxYear.Text.Trim()); // RMC 20110824 changed m_sArrayBnsCode[iRow].ToString() to sTmpBnsCode
                        if (pRec.ExecuteNonQuery() == 0)
                        {
                        }

                        pRec.Query = string.Format("update bill_no set bns_code_main = '{0}' where bin = '{1}' ", bnsCode, xBin);
                        pRec.Query += string.Format("and bns_code_main = '{0}'", sTmpBnsCode);  // RMC 20110824 changed m_sArrayBnsCode[iRow].ToString() to sTmpBnsCode
                        if (pRec.ExecuteNonQuery() == 0)
                        {
                        }

                        pRec.Query = string.Format("update bill_hist set bns_code_main = '{0}' where bin = '{1}' ", bnsCode, xBin);
                        pRec.Query += string.Format("and bns_code_main = '{0}' and tax_year = '{1}'", sTmpBnsCode, txtTaxYear.Text.Trim()); // RMC 20110824 changed m_sArrayBnsCode[iRow].ToString() to sTmpBnsCode
                        if (pRec.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
            }

            if (iRow >= 0)
            {
                MessageBox.Show("Additional business saved.", "Additional Business", MessageBoxButtons.OK, MessageBoxIcon.Information);   // RMC 20110824 Transferred
            }

            this.Close();
        }

        private void btnSearchBnsType_Click(object sender, EventArgs e)
        {
            frmBusinessType frmBnsType = new frmBusinessType();
            //frmBnsType.SetFormStyle(true);
            frmBnsType.SetFormStyle(false); // RMC 20110819 change search engine format called
            frmBnsType.ShowDialog();

            try
            {
                m_strBnsCode = StringUtilities.Left(frmBnsType.m_strBnsCode, 2);
                m_strBnsDesc = frmBnsType.m_sBnsDescription;
            }
            catch
            {
                m_strBnsCode = "";  // RMC 20150205 corrections in additional business module
                m_strBnsDesc = "";  // RMC 20150205 corrections in additional business module
            }

            pSet.Query = string.Format("select bns_desc from bns_table where bns_code = '{0}' and fees_code = 'B'", m_strBnsCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                    m_strMainBnsDesc = pSet.GetString("bns_desc");
            }
            pSet.Close();

            //LoadComboCell(m_strBnsCode);    // RMC 20110824
            //dgvList[1, m_iActiveCellRow].Value = m_strBnsDesc;

            if(m_strMainBnsDesc != "" && m_strBnsDesc !=  "") //JARS 20170822 so that the error messages will not pop-up
            {
                dgvList[0, m_iActiveCellRow].Value = m_strMainBnsDesc;
                LoadComboCell(m_strBnsCode);
                dgvList[1, m_iActiveCellRow].Value = m_strBnsDesc;
            }

            #region comments
            /*comboCell = new DataGridViewComboBoxCell();
            
            comboCell.Items.Clear();

            pSet.Query = string.Format("select * from bns_table where trim(bns_code) like '{0}%' and fees_code = 'B'", m_strBnsCode);
            pSet.Query += string.Format(" and length(trim(bns_code)) > 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (pSet.Execute())
            {
                while (pSet.Read())
                    comboCell.Items.AddRange(pSet.GetString("bns_desc"));
            }
            pSet.Close();*/

           /* int iTmpRow = 0;
            for(int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
            {
                if (dgvList[0, iRow].Value == null && dgvList[1, iRow].Value == null)
                {
                    dgvList[0, iRow].Value = m_strMainBnsDesc;
                    //dgvList[1, iRow].Value = m_strBnsDesc;
                    iTmpRow = iRow;
                }
                else if(dgvList[0,iRow].Value.ToString() == "" && dgvList[1,iRow].Value.ToString() == "")
                {
                    dgvList[0, iRow].Value = m_strMainBnsDesc;
                    //dgvList[1, iRow].Value = m_strBnsDesc;
                    iTmpRow = iRow;
                }
            }*/

            /*if (iTmpRow != 0)
             {
                 dgvList.Rows[iTmpRow].Cells[1] = comboCell;
                 dgvList[1, iTmpRow].Value = m_strBnsDesc;
             }*/
            #endregion
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvList.Rows.Add();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete this record?", "Additional Business", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    //dgvList.Rows.RemoveAt(dgvList.SelectedCells[0].RowIndex);
                    dgvList.Rows.Add("");
                    dgvList.Rows.RemoveAt(m_iActiveCellRow);

                    UpdateOtherInfo(m_strBnsCode);  // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross
                    
                }
                catch
                {
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();
            string sFeesCode = string.Empty;
            string sFeesDesc = string.Empty;

            sFeesCode = "MAYOR'S PERMIT";

            pSet.Query = string.Format("select * from tax_and_fees_table where (fees_desc = '{0}'", StringUtilities.HandleApostrophe(sFeesCode));
            pSet.Query += " or fees_desc like '%MAYOR%')";
            pSet.Query += " and rev_year = '" + AppSettingsManager.GetConfigValue("07") + "'"; //MCR 20141118

            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sFeesCode = pSet.GetString("fees_code");
                    sFeesDesc = pSet.GetString("fees_desc");
                }
                else
                {
                    sFeesCode = "";
                    sFeesDesc = "";
                }
            }
            pSet.Close();

            double dCapital = 0;
            double dGross = 0;
            string sRowStat = string.Empty;
            string sRowCapital = string.Empty;
            string sRowGross = string.Empty;
            string sRowBnsCode = string.Empty;

            int intRow = 0;
            
            try
            {
                intRow = this.dgvList.SelectedCells[0].RowIndex;
            }
            catch
            {
                intRow = 0;
            }

            try
            {
                //sRowStat = dgvList[4, intRow].Value.ToString();
                sRowStat = dgvList[5, intRow].Value.ToString(); // RMC 20110809
                sRowCapital = dgvList[2, intRow].Value.ToString();
                //sRowGross = dgvList[3, intRow].Value.ToString();
                sRowGross = dgvList[4, intRow].Value.ToString();    // RMC 20110809
                sRowBnsCode = dgvList[1, intRow].Value.ToString();

                pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(sRowBnsCode), m_strRevYear);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                        sRowBnsCode = pSet.GetString("bns_code");
                }
                pSet.Close();

                double.TryParse(sRowCapital, out dCapital);
                double.TryParse(sRowGross, out dGross);

                frmBillFee BillFeeForm = new frmBillFee();
                BillFeeForm.SourceClass = new BillFee(BillFeeForm);
                BillFeeForm.BIN = m_strBIN;
                BillFeeForm.Status = sRowStat;
                BillFeeForm.Capital = dCapital;
                BillFeeForm.Gross = dGross;
                BillFeeForm.BusinessCode = sRowBnsCode;
                BillFeeForm.FeesCode = sFeesCode;
                BillFeeForm.Text = sFeesDesc; // set header
                BillFeeForm.TaxYear = txtTaxYear.Text.Trim();
                BillFeeForm.Quarter = AppSettingsManager.GetQtr(m_odtOperationStart.ToString());
                BillFeeForm.RevisionYear = m_strRevYear;
                BillFeeForm.ShowDialog();
            }
            catch
            {
                sRowStat = "";
                sRowCapital = "";
                sRowGross = "";
                sRowBnsCode = "";
                MessageBox.Show("Select additional business first.","Compute Permit",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }

            
        }

        private void LoadComboCell(string sBnsCode)
        {
            // RMC 20110824
            OracleResultSet result = new OracleResultSet();

            comboCell = new DataGridViewComboBoxCell();

            comboCell.Items.Clear();

            result.Query = string.Format("select * from bns_table where trim(bns_code) like '{0}%' and fees_code = 'B'", sBnsCode);
            result.Query += string.Format(" and length(trim(bns_code)) > 2 and rev_year = '{0}' order by bns_code", m_strRevYear);
            if (result.Execute())
            {
                while (result.Read())
                    comboCell.Items.AddRange(result.GetString("bns_desc"));
            }
            result.Close();

            dgvList.Rows[m_iActiveCellRow].Cells[1] = comboCell;
        }

        private void UpdateOtherInfo(string sBnsCode)
        {
            // RMC 20120104 corrected error in addl bns with no entry of gross/cap/pre gross
            OracleResultSet pOtherinfo = new OracleResultSet();

            pOtherinfo.Query = "delete from other_info where bin = '" + txtBIN.Text + "'";
            pOtherinfo.Query += " and tax_year = '" + txtTaxYear.Text + "'";
            pOtherinfo.Query += " and bns_code = '" + sBnsCode + "'";
            if (pOtherinfo.ExecuteNonQuery() == 0)
            { }

        }

        private void UpdateBillGrossInfo(string sBIN, string sBnsCode, string sTaxYear, string sBnsStat, double dCapital, double dGross)
        {
            // RMC 20150205
            int intCurrentYear = 0;
            int.TryParse(ConfigurationAttributes.CurrentYear, out intCurrentYear);
            int iTaxYear = 0;
            int.TryParse(sTaxYear, out iTaxYear);

            while (iTaxYear <= intCurrentYear)
            {
                sTaxYear = string.Format("{0:####}", iTaxYear);

                OraclePLSQL plsqlCmd = new OraclePLSQL();
                double dGrCap;
                int iIsReset = 0;
                plsqlCmd.Transaction = true;
                plsqlCmd.ProcedureName = "update_billgross_info";
                plsqlCmd.ParamValue = sBIN;
                plsqlCmd.AddParameter("p_sBIN", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
                plsqlCmd.ParamValue = sBnsCode;
                plsqlCmd.AddParameter("p_sBnsCode", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
                plsqlCmd.ParamValue = sTaxYear;
                plsqlCmd.AddParameter("p_sTaxYear", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
                plsqlCmd.ParamValue = "1";
                plsqlCmd.AddParameter("p_sQtr", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
                plsqlCmd.ParamValue = sBnsStat;
                plsqlCmd.AddParameter("p_sStatus", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);
                plsqlCmd.ParamValue = sBnsStat.Substring(0, 1);
                plsqlCmd.AddParameter("p_sDueState", OraclePLSQL.OracleDbTypes.Varchar2, ParameterDirection.Input);

                plsqlCmd.ParamValue = dCapital;
                plsqlCmd.AddParameter("p_fCapital", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

                plsqlCmd.ParamValue = dGross;
                plsqlCmd.AddParameter("p_fGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

                plsqlCmd.ParamValue = 0;
                plsqlCmd.AddParameter("p_fPreGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
                plsqlCmd.ParamValue = 0;
                plsqlCmd.AddParameter("p_fAdjGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);

                plsqlCmd.ParamValue = 0;
                plsqlCmd.AddParameter("p_fVatGross", OraclePLSQL.OracleDbTypes.Double, ParameterDirection.Input);
                plsqlCmd.ParamValue = iIsReset;
                plsqlCmd.AddParameter("o_iIsReset", OraclePLSQL.OracleDbTypes.Int32, ParameterDirection.Output);

                plsqlCmd.ExecuteNonQuery();
                if (!plsqlCmd.Commit())
                    plsqlCmd.Rollback();
                else
                    int.TryParse(plsqlCmd.ReturnValue("o_iIsReset").ToString(), out iIsReset);
                plsqlCmd.Close();

                iTaxYear = iTaxYear + 1;
                sBnsStat = "REN";
            }

        }
		
    }
}