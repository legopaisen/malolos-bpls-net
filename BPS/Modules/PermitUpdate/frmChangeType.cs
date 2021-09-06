
// RMC 20120316 Corrected saving in change_class_tbl

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
using Amellar.Common.AddlInfo;
using Amellar.BPLS.Billing;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.TransactionLog;

namespace Amellar.Modules.PermitUpdate
{
    public partial class frmChangeType : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sBIN = string.Empty;
        private string m_sTaxYear = string.Empty;
        private string m_sModule = string.Empty;
        private string m_sDateOperated = string.Empty;
        private string m_sBnsStat = string.Empty;
        private string m_sBnsCode = string.Empty;
        private string m_sCapital = string.Empty;
        private string m_sNewMainBnsDesc = string.Empty;
        private bool m_bIsApplSave = false;
        private bool m_bApplyBilling = true;   // RMC 20141125 mods in trailing permit-update transaction
        private DateTime m_dLogIn = AppSettingsManager.GetSystemDate();  // RMC 20170822 added transaction log feature for tracking of transactions per bin
        private string m_sBussStat = string.Empty;   // RMC 20170822 added transaction log feature for tracking of transactions per bin

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

        public string Module
        {
            get { return m_sModule; }
            set { m_sModule = value; }
        }

        public string NewBnsDesc
        {
            get { return m_sNewMainBnsDesc; }
            set { m_sNewMainBnsDesc = value; }
        }

        public string DateOperated
        {
            get { return m_sDateOperated; }
            set { m_sDateOperated = value; }
        }

        public bool ApplSave
        {
            get { return m_bIsApplSave; }
            set { m_bIsApplSave = value; }
        }

        public bool ApplyBilling    // RMC 20141125 mods in trailing permit-update transaction
        {
            get { return m_bApplyBilling; }
            set { m_bApplyBilling = value; }
        }

        public DateTime LogIn   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        {
            get { return m_dLogIn; }
            set { m_dLogIn = value; }
        }

        public string BnsStat   // RMC 20170822 added transaction log feature for tracking of transactions per bin
        {
            get { return m_sBussStat; }
            set { m_sBussStat = value; }
        }

        public frmChangeType()
        {
            InitializeComponent();
        }

        private void frmChangeType_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            OracleResultSet pRec = new OracleResultSet();

            string sBnsCode, sBnsDesc, sBnsStat, sNewBnsCode, sCapital;
            int iRow = 0;

            dgvList.Columns.Clear();
            dgvList.Columns.Add("OLDBTYPE", "Old Business Type");
            dgvList.Columns.Add("NEWBTYPE", "New Business Type");
            dgvList.Columns.Add("CAPITAL", "Capital");
            dgvList.Columns.Add(new DataGridViewCheckBoxColumn());
            dgvList.Columns.Add("OLDCODE", "Old Code");
            dgvList.Columns.Add("NEWCODE", "New Code");
            dgvList.Columns.Add("STATUS", "Status");
            dgvList.RowHeadersVisible = false;
            dgvList.Columns[0].Width = 180;
            dgvList.Columns[1].Width = 180;
            dgvList.Columns[2].Width = 90;
            dgvList.Columns[3].Width = 30;
            dgvList.Columns[4].Visible = false;
            dgvList.Columns[5].Visible = false;
            dgvList.Columns[6].Visible = false;
            dgvList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            pSet.Query = string.Format("select * from business_que where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Change of Classification is not allowed.\n Record should undergo partial/full retirement.", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close();
                    return;
                }
                else
                {
                    pSet.Close();
                    pSet.Query = string.Format("select * from businesses where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
                }
            }
            
            if(pSet.Execute())
            {
                if (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code");
                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sBnsStat = pSet.GetString("bns_stat");

                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = sBnsDesc;
                    dgvList[2, iRow].Value = "0.00";
                    dgvList[3, iRow].Value = false;
                    dgvList[4, iRow].Value = sBnsCode;
                    dgvList[6, iRow].Value = sBnsStat;

                    pRec.Query = string.Format("select * from change_class_tbl where bin = '{0}' and old_bns_code = '{1}'", m_sBIN, sBnsCode);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sNewBnsCode = pRec.GetString("new_bns_code");
                            sBnsDesc = AppSettingsManager.GetBnsDesc(sNewBnsCode);
                            sCapital = string.Format("{0:##.00}", pRec.GetDouble("capital"));

                            dgvList[1, iRow].Value = sBnsDesc;
                            dgvList[2, iRow].Value = sCapital;
                            dgvList[3, iRow].Value = true;
                            dgvList[5, iRow].Value = sNewBnsCode;
                        }
                    }
                    pRec.Close();
                }
            }
            pSet.Close();

            iRow++;

            pSet.Query = string.Format("select * from addl_bns where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
            if(pSet.Execute())
            {
                while (pSet.Read())
                {
                    sBnsCode = pSet.GetString("bns_code_main");
                    sBnsDesc = AppSettingsManager.GetBnsDesc(sBnsCode);
                    sBnsStat = pSet.GetString("bns_stat");

                    dgvList.Rows.Add("");
                    dgvList[0, iRow].Value = sBnsDesc;
                    dgvList[3, iRow].Value = false;
                    dgvList[4, iRow].Value = sBnsCode;
                    dgvList[6, iRow].Value = sBnsStat;

                    pRec.Query = string.Format("select * from change_class_tbl where bin = '{0}' and old_bns_code = '{1}'", m_sBIN, sBnsCode);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sNewBnsCode = pRec.GetString("new_bns_code");
                            sBnsDesc = AppSettingsManager.GetBnsDesc(sNewBnsCode);
                            sCapital = string.Format("{0:##.00}", pRec.GetDouble("capital"));

                            dgvList[1, iRow].Value = sBnsDesc;
                            dgvList[2, iRow].Value = sCapital;
                            dgvList[3, iRow].Value = true;
                            dgvList[5, iRow].Value = sNewBnsCode;

                        }
                    }
                    pRec.Close();

                    iRow++;
                }
                
            }
            pSet.Close();

            dgvList.EndEdit();
        }

        private void dgvList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                    dgvList.ReadOnly = false;
                else if (e.ColumnIndex == 2 && (bool)dgvList[3, e.RowIndex].Value)
                    dgvList.ReadOnly = false;
                else
                    dgvList.ReadOnly = true;
            }
            catch
            {
            }
        }

        private void dgvList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string sValue = string.Empty;

            if (e.ColumnIndex == 2)
            {
                try
                {
                    sValue = dgvList[2, e.RowIndex].Value.ToString();
                    sValue = string.Format("{0:##.00}", Convert.ToDouble(sValue));
                    dgvList[2, e.RowIndex].Value = sValue;
                }
                catch
                {
                    dgvList[2, e.RowIndex].Value = "0.00";
                }
                
            }


        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string sBnsCode, sBnsDesc, sCapital;

            dgvList.EndEdit();

            if (e.ColumnIndex == 3)
            {
                if ((bool)dgvList[3, e.RowIndex].Value)
                {
                    frmBusinessType frmBnsType = new frmBusinessType();
                    frmBnsType.SetFormStyle(true);
                    frmBnsType.ShowDialog();
                    sBnsDesc = frmBnsType.m_sBnsDescription;
                    sBnsCode = frmBnsType.m_strBnsCode;
                    sCapital = "0.00";

                    if (sBnsCode == "" || sBnsCode == null)
                    {
                        dgvList[3, e.RowIndex].Value = false;
                        sBnsCode = "";
                        sCapital = "";
                        sBnsDesc = "";
                        
                    }
                }
                else
                {
                    sBnsCode = "";
                    sCapital = "";
                    sBnsDesc = "";
                }

                dgvList[1, e.RowIndex].Value = sBnsDesc;
                dgvList[2, e.RowIndex].Value = sCapital;
                dgvList[5, e.RowIndex].Value = sBnsCode;
            }

            if (e.ColumnIndex == 2)
            {
                sCapital = dgvList[2, e.RowIndex].Value.ToString();

                try
                {
                    sCapital = string.Format("{0:##0.00}", Convert.ToDouble(sCapital));
                }
                catch
                {
                    MessageBox.Show("Error in Field", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dgvList[2, e.RowIndex].Value = "0.00";
                    return;
                }
            }

        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvList[6, e.RowIndex].Value != null)
                m_sBnsStat = dgvList[6, e.RowIndex].Value.ToString();

            if (dgvList[5, e.RowIndex].Value != null)
                m_sBnsCode = dgvList[5, e.RowIndex].Value.ToString();

            if (dgvList[2, e.RowIndex].Value != null)
                m_sCapital = dgvList[2, e.RowIndex].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sCapital = string.Empty;
            string sOldBnsCode = string.Empty;
            string sNewBnsCode = string.Empty;
            string sBnsStat = string.Empty;

            if (MessageBox.Show("Save application for Change of Classification?", m_sModule, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pSet.Query = string.Format("delete from addl_bns_que where bin = '{0}' and tax_year = '{1}'", m_sBIN, m_sTaxYear);
                pSet.Query += " and bin in (select bin from permit_update_appl";
                pSet.Query += string.Format(" where bin = '{0}' and tax_year = '{0}'", m_sBIN, m_sTaxYear);
                pSet.Query += " and appl_type = 'CTYP' and data_mode = 'QUE')";
                if (pSet.ExecuteNonQuery() == 0)
                { }

                pSet.Query = string.Format("delete from change_class_tbl where bin = '{0}'", m_sBIN);
                if (pSet.ExecuteNonQuery() == 0)
                { }

                // RMC 20141125 mods in trailing permit-update transaction (s)
                string sObj = string.Empty;
                //TransferAppFrm.BIN + "/PUT: Buss Name to " + TransferAppFrm.txtNewLastName.Text.ToString().Trim();
                if (MessageBox.Show("Apply billing?", "Permit Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    m_bApplyBilling = false;
                else
                    m_bApplyBilling = true;
                // RMC 20141125 mods in trailing permit-update transaction (e)

                for (int iRow = 0; iRow < dgvList.Rows.Count; iRow++)
                {
                    if ((bool) dgvList[3,iRow].Value)
                    {
                        sCapital = string.Format("{0:##.00}", Convert.ToDouble(dgvList[2, iRow].Value.ToString()));
                        sOldBnsCode = dgvList[4, iRow].Value.ToString();
                        sNewBnsCode = dgvList[5, iRow].Value.ToString();
                        sBnsStat = dgvList[6, iRow].Value.ToString();

                        // RMC 20120316 Corrected saving in change_class_tbl (s)
                        DateTime dtSaveDate = AppSettingsManager.GetCurrentDate();
                        string sSaveDate = string.Format("{0:MM/dd/yyyy}", dtSaveDate);
                        // RMC 20120316 Corrected saving in change_class_tbl (e)

                        if (iRow == 0)
                        {
                            m_sNewMainBnsDesc = dgvList[1, iRow].Value.ToString();
                            pSet.Query = "insert into change_class_tbl (BIN, OLD_BNS_CODE, NEW_BNS_CODE, CAPITAL, STATUS, IS_MAIN, APP_DATE) values (:1,:2,:3,:4,:5,'Y',:6)";
                        }
                        else
                        {
                            pSet.Query = "insert into change_class_tbl (BIN, OLD_BNS_CODE, NEW_BNS_CODE, CAPITAL, STATUS, IS_MAIN, APP_DATE) values (:1,:2,:3,:4,:5,'N',:6)";
                        }
                        pSet.AddParameter(":1", m_sBIN);
                        pSet.AddParameter(":2", sOldBnsCode);
                        pSet.AddParameter(":3", sNewBnsCode);
                        pSet.AddParameter(":4", sCapital);
                        pSet.AddParameter(":5", sBnsStat);
                        //pSet.AddParameter(":6", AppSettingsManager.GetCurrentDate());
                        pSet.AddParameter(":6", Convert.ToDateTime(sSaveDate)); // RMC 20120316 Corrected saving in change_class_tbl
                        if (pSet.ExecuteNonQuery() == 0)
                        { }

                        // RMC 20141125 mods in trailing permit-update transaction (s)
                        sObj = m_sBIN + "/PUT: Buss. Code: " + sOldBnsCode + " to " + sNewBnsCode;
                        if (!m_bApplyBilling)
                        {
                            TransLog.UpdateLog(m_sBIN, m_sBussStat, m_sTaxYear, "AAAPUT-NB", m_dLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                            if (AuditTrail.InsertTrail("AAAPUT-NB", "change_class_tbl", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            TransLog.UpdateLog(m_sBIN, m_sBussStat, m_sTaxYear, "AAAPUT", m_dLogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per binTransLog.UpdateLog(TransferAppFrm.BIN, TransferAppFrm.BnsStat, TransferAppFrm.TaxYear, "AAAPUT", TransferAppFrm.LogIn, AppSettingsManager.GetSystemDate());  // RMC 20170822 added transaction log feature for tracking of transactions per bin

                            if (AuditTrail.InsertTrail("AAAPUT", "change_class_tbl", StringUtilities.HandleApostrophe(sObj)) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        // RMC 20141125 mods in trailing permit-update transaction (e)

                        if (iRow != 0)
                        {
                            pSet.Query = "insert into addl_bns_que (BIN, BNS_CODE_MAIN, CAPITAL, GROSS, TAX_YEAR, BNS_STAT, QTR) values (:1,:2,:3,:4,:5,:6,:7)";
                            pSet.AddParameter(":1", m_sBIN);
                            pSet.AddParameter(":2", sNewBnsCode);
                            pSet.AddParameter(":3", sCapital);
                            pSet.AddParameter(":4", "0.00");
                            pSet.AddParameter(":5", m_sTaxYear);
                            pSet.AddParameter(":6", sBnsStat);
                            pSet.AddParameter(":7", AppSettingsManager.GetQtr(m_sDateOperated));
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                        }
                    }
                }

                MessageBox.Show("Application for Change of Classification saved successfully.", m_sModule, MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_bIsApplSave = true;
                this.Close();
            }	
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddlInfo_Click(object sender, EventArgs e)
        {
            if (m_sBnsCode.Trim() != "")
            {
                using (frmAddlInfo frmGrid = new frmAddlInfo())
                {
                    frmGrid.BnsCode = m_sBnsCode;
                    frmGrid.BIN = m_sBIN;
                    frmGrid.TaxYear = m_sTaxYear;
                    frmGrid.ShowDialog();
                }    
            }
            else
            {
                MessageBox.Show("No record found.", m_sModule,MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnPermit_Click(object sender, EventArgs e)
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
                sRowStat = dgvList[4, intRow].Value.ToString();
                sRowCapital = dgvList[2, intRow].Value.ToString();
                sRowGross = dgvList[3, intRow].Value.ToString();
                sRowBnsCode = dgvList[1, intRow].Value.ToString();

                pSet.Query = string.Format("select bns_code from bns_table where fees_code = 'B' and bns_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(sRowBnsCode), AppSettingsManager.GetConfigValue("07"));
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
                BillFeeForm.BIN = m_sBIN;
                BillFeeForm.Status = sRowStat;
                BillFeeForm.Capital = dCapital;
                BillFeeForm.Gross = dGross;
                BillFeeForm.BusinessCode = sRowBnsCode;
                BillFeeForm.FeesCode = sFeesCode;
                BillFeeForm.Text = sFeesDesc; // set header
                BillFeeForm.TaxYear = m_sTaxYear;
                BillFeeForm.Quarter = AppSettingsManager.GetQtr(m_sDateOperated);
                BillFeeForm.RevisionYear = AppSettingsManager.GetConfigValue("07");
                BillFeeForm.ShowDialog();
            }
            catch
            {
                sRowStat = "";
                sRowCapital = "";
                sRowGross = "";
                sRowBnsCode = "";
                MessageBox.Show("Select additional business first.", "Compute Permit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
    }
}