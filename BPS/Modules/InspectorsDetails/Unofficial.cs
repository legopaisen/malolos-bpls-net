
// RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request 

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using Amellar.Common.AppSettings;
using Amellar.Common.BPLSApp;

namespace Amellar.Modules.InspectorsDetails
{
    public class Unofficial : InsDetails
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sOption = string.Empty;
        private string m_sSelectedIS = string.Empty;
        private string m_sSelectedDate = string.Empty;
        private string m_sBusinessName = string.Empty;
        private string m_sOwnerName = string.Empty;
        private string m_sSelectedInspector = string.Empty;
        BPLSAppSettingList sList = new BPLSAppSettingList();

        DateTime dtDate1;
        DateTime dtDate2;

        public Unofficial(frmInspectorDetails Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            DetailsFrm.lblIS.Location = new System.Drawing.Point(26, 159);
            DetailsFrm.txtISNo.Location = new System.Drawing.Point(78, 156);
            DetailsFrm.bin1.Visible = false;
            DetailsFrm.txtISNo.ReadOnly = false;
            DetailsFrm.txtISNo.Focus();
            m_sOption = "I";

            m_sSelectedIS = "";
            this.LoadInspector(m_sSelectedIS);
        }

        private void OnLoadInspectorDetails(string strInspectorCode, string strIS)
        {
            DetailsFrm.dgvDetails.Columns.Clear();
            DetailsFrm.dgvDetails.Columns.Add("SNO", "Inspection Slip #");
            DetailsFrm.dgvDetails.Columns.Add("DATE", "Date Inspected");
            DetailsFrm.dgvDetails.RowHeadersVisible = false;
            DetailsFrm.dgvDetails.Columns[0].Width = 160;
            DetailsFrm.dgvDetails.Columns[1].Width = 120;
            DetailsFrm.dgvDetails.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DetailsFrm.dgvDetails.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            string sISNum = string.Empty;
            string sRemarks = string.Empty;
            string sDate = string.Empty;
            
            if (strIS != "")
            {
                pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' and is_option = '{1}' and is_number = '{2}' order by inspector_code, date_inspected", strInspectorCode, m_sOption, strIS);
            }
            else
            {
                pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' and is_option = '{1}' order by date_inspected,is_number", strInspectorCode, m_sOption);
            }
            if(pSet.Execute())
            {
                int iRow = 0;
                m_sSelectedIS = strIS;

                while (pSet.Read())
                {
                    sISNum = pSet.GetString("is_number").Trim();
                    sDate = pSet.GetString("date_inspected").Trim();
                    sRemarks = pSet.GetString("inspector_remarks").Trim();

                    if (iRow == 0)
                    {
                        m_sSelectedInspector = strInspectorCode;
                        m_sSelectedIS = sISNum;
                        m_sSelectedDate = sDate;
                    }

                    DetailsFrm.dgvDetails.Rows.Add("");
                    DetailsFrm.dgvDetails[0, iRow].Value = sISNum;
                    DetailsFrm.dgvDetails[1, iRow].Value = sDate;
                    iRow++;
                    
                }

                if(m_sSelectedIS != "")
                    OnLoadData(m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
            }
        }

        private void OnLoadData(string strInspectorCode, string strIS, string sDateInspected)
        {
            this.ClearControls();

            m_sBusinessName = "";
            m_sOwnerName = "";
            

            pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", strInspectorCode, strIS, sDateInspected);
            pSet.Query += " and is_number in (select is_number from unofficial_info_tbl where trim(bin_settled) is null)";  // RMC 20110816
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    DetailsFrm.txtRemarks.Text = pSet.GetString("inspector_remarks");
                    DetailsFrm.txtAddlRemarks.Text = pSet.GetString("addition_info");
                    DetailsFrm.dtpDateInspected.Value = DateTime.Parse(pSet.GetString("date_inspected"));
                }
            }
            pSet.Close();

            LoadBusinessInfo(strIS);

            DetailsFrm.txtBnsName.Text = m_sBusinessName;
            DetailsFrm.txtOwnName.Text = m_sOwnerName;
            
            LoadViolation(strInspectorCode, strIS, sDateInspected);

            ManageControls();

            DetailsFrm.btnSearch.Text = "Clear";
        }

        private void LoadInspector(string strIS)
        {
            DetailsFrm.dgvInspectors.Columns.Clear();
            DetailsFrm.dgvInspectors.Columns.Add("CODE", "Code");
            DetailsFrm.dgvInspectors.Columns.Add("NAME", "Inspector Name");
            DetailsFrm.dgvInspectors.RowHeadersVisible = false;
            DetailsFrm.dgvInspectors.Columns[0].Width = 70;
            DetailsFrm.dgvInspectors.Columns[1].Width = 200;
            DetailsFrm.dgvInspectors.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DetailsFrm.dgvInspectors.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            string sCode = "";
            string sLN = "";
            string sFN = "";
            string sMI = "";
            string sName = "";

            if (strIS != "")
            {
                pSet.Query = "select * from inspector where inspector_code in ";
                pSet.Query += string.Format("(select inspector_code from unofficial_dtls where is_number = '{0}')", strIS);
            }
            else
            {
                pSet.Query = "select * from inspector order by inspector_code";
            }
            int iRow = 0;   // RMC 20110902
            if (pSet.Execute())
            {
                // int iRow = 0; // RMC 20110902 put rem
                while (pSet.Read())
                {
                    sCode = pSet.GetString("inspector_code").Trim();
                    if (iRow == 0)
                        m_sSelectedInspector = sCode;

                    sLN = pSet.GetString("inspector_ln").Trim();
                    sFN = pSet.GetString("inspector_fn").Trim();
                    sMI = pSet.GetString("inspector_mi").Trim();
                    sName = sLN + ", " + sFN + " " + sMI + ".";
                    DetailsFrm.dgvInspectors.Rows.Add("");
                    DetailsFrm.dgvInspectors[0, iRow].Value = sCode;
                    DetailsFrm.dgvInspectors[1, iRow].Value = sName;
                    iRow++;

                }

                pSet.Close();
            }

            // RMC 20110901 Added auto-tagging if Lessor is not a registered business (s)
            OracleResultSet pRec = new OracleResultSet();

            if (strIS == "")
            {
                pSet.Query = "select distinct inspector_code from unofficial_dtls where inspector_code not in ";
                pSet.Query += " (select inspector_code from inspector) order by inspector_code";
                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sCode = pSet.GetString("inspector_code").Trim();

                        pRec.Query = "select * from sys_users where usr_code = '" + sCode + "'";
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                                sLN = pRec.GetString("usr_ln").Trim();
                                sFN = pRec.GetString("usr_fn").Trim();
                                sMI = pRec.GetString("usr_mi").Trim();
                                sName = sLN + ", " + sFN + " " + sMI + ".";
                                DetailsFrm.dgvInspectors.Rows.Add("");
                                DetailsFrm.dgvInspectors[0, iRow].Value = sCode;
                                DetailsFrm.dgvInspectors[1, iRow].Value = sName;
                                iRow++;

                            }
                        }
                        pRec.Close();
                    }
                }
                pSet.Close();

            }
            // RMC 20110901 Added auto-tagging if Lessor is not a registered business (e)

            try
            {
                OnLoadInspectorDetails(m_sSelectedInspector, strIS);
            }
            catch
            {
            }

        }

        private void LoadBusinessInfo(string strIS)
        {
            string sBnsCode = string.Empty;

            DetailsFrm.txtISNo.Text = strIS;

            pSet.Query = string.Format("select * from unofficial_info_tbl where IS_NUMBER = '{0}'", strIS);
            if(pSet.Execute())
            {
                if(pSet.Read())
                {
                    m_sOwnerName = AppSettingsManager.GetBnsOwner(pSet.GetString("own_code"));
                    m_sBusinessName = pSet.GetString("bns_nm");
                    
                    DetailsFrm.txtBnsName.Text = m_sBusinessName;
                    DetailsFrm.txtBnsAdd.Text = pSet.GetString("bns_house_no").Trim();
                    DetailsFrm.txtBnsCity.Text = pSet.GetString("bns_mun").Trim();
                    DetailsFrm.txtBnsStreet.Text = pSet.GetString("bns_street").Trim();
                    DetailsFrm.cmbBnsDist.Text = pSet.GetString("bns_dist").Trim();
                    DetailsFrm.txtBnsZone.Text = pSet.GetString("bns_zone").Trim();
                    DetailsFrm.cmbBnsBrgy.Text = pSet.GetString("bns_brgy").Trim();
                    DetailsFrm.txtBnsProv.Text = pSet.GetString("bns_prov").Trim();
                    DetailsFrm.cmbBnsOrgKind.Text = pSet.GetString("orgn_kind").Trim();
                    DetailsFrm.txtBnsType.Text = pSet.GetString("bns_type").Trim();
                    
                    DetailsFrm.txtOwnName.Text = m_sOwnerName;

                    sList.OwnName = pSet.GetString("own_code");
                    for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
                    {
                        DetailsFrm.txtOwnAdd.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                        DetailsFrm.txtOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                        DetailsFrm.cmbOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                        DetailsFrm.cmbOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                        DetailsFrm.txtOwnZone.Text = sList.OwnNamesSetting[j].sOwnZone;
                        DetailsFrm.txtOwnCity.Text = sList.OwnNamesSetting[j].sOwnMun;
                        DetailsFrm.txtOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                        DetailsFrm.txtZip.Text = sList.OwnNamesSetting[j].sOwnZip;  // RMC 20110819
                    }

                    
                }
            }
            pSet.Close();
        }

        private void LoadViolation(string strInspectorCode, string strIS, string sDateInspected)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sViolation = "";
            string sReference = "";

            dtDate2 = DetailsFrm.dtpDateInspected.Value;
            dtDate1 = DateTime.Parse(m_sSelectedDate);

            if (DetailsFrm.btnEdit.Text == "Update" && (dtDate1 != dtDate2))
            {
                pSet.Query = "update violation_ufc set date_inspected = :1 where inspector_code = :2 and is_number = :3 and date_inspected = :4";
                pSet.AddParameter(":1", dtDate2);
                pSet.AddParameter(":2", strInspectorCode);
                pSet.AddParameter(":3", strIS);
                pSet.AddParameter(":4", dtDate1);
                if (pSet.ExecuteNonQuery() == 0)
                { }
            }

            int iRow = 0;
            string sCode = string.Empty;

            DetailsFrm.dgvViolations.Columns.Clear();
            DetailsFrm.dgvViolations.Columns.Add("CODE", "Violation Code");
            DetailsFrm.dgvViolations.Columns.Add("DESC", "Ordinance Violation");
            DetailsFrm.dgvViolations.Columns.Add("REF", "Reference");
            DetailsFrm.dgvViolations.RowHeadersVisible = false;
            DetailsFrm.dgvViolations.Columns[0].Width = 100;
            DetailsFrm.dgvViolations.Columns[1].Width = 200;
            DetailsFrm.dgvViolations.Columns[2].Width = 200;
            DetailsFrm.dgvViolations.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DetailsFrm.dgvViolations.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DetailsFrm.dgvViolations.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            pSet.Query = string.Format("select * from violation_ufc where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}' order by violation_code", strInspectorCode, strIS, sDateInspected);
            if (pSet.Execute())
            {
                while (pSet.Read())
                {
                    sCode = pSet.GetString("violation_code").Trim();

                    pRec.Query = string.Format("select * from violation_table where violation_code = '{0}'", sCode);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            sViolation = pRec.GetString("violation_desc").Trim();
                            sReference = pRec.GetString("reference").Trim();
                        }
                    }
                    pRec.Close();

                    DetailsFrm.dgvViolations.Rows.Add("");
                    DetailsFrm.dgvViolations[0, iRow].Value = sCode;
                    DetailsFrm.dgvViolations[1, iRow].Value = sViolation;
                    DetailsFrm.dgvViolations[2, iRow].Value = sReference;
                    iRow++;
                }
            }
            pSet.Close();
        }

        public override void Search()
        {
            if (DetailsFrm.btnSearch.Text == "Clear")
            {
                this.ClearControls();
                DetailsFrm.btnSearch.Text = "Search";
                DetailsFrm.txtISNo.Focus();

                m_sSelectedIS = "";
                DetailsFrm.dgvViolations.Columns.Clear();
                
            }
            else
            {
                if(DetailsFrm.txtISNo.Text != "")
                {
                    m_sSelectedIS = DetailsFrm.txtISNo.Text.Trim();

                    LoadBusinessInfo(m_sSelectedIS);
                    LoadInspector(m_sSelectedIS);
                }
                else
                {
                    frmSearchIS frmSearchBns = new frmSearchIS();
                    
                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.ISNum != "")
                    {
                        DetailsFrm.txtISNo.Text = frmSearchBns.ISNum;

                        m_sSelectedIS = DetailsFrm.txtISNo.Text;

                        LoadBusinessInfo(m_sSelectedIS);
                        LoadInspector(m_sSelectedIS);
                    }
                    else
                        this.ClearControls();
                }
            }
        }

        public override void Tag()
        {
            OracleResultSet pSet = new OracleResultSet();

            string sDate = string.Empty;
            string sRemarks = string.Empty;
            string sAddl = string.Empty;
            string sReason = string.Empty;
            string sOwnCode = string.Empty;

            int iNoticeNum = 0, iConfig = 0;

            pSet.Query = string.Format("select * from norec_closure_tagging where is_number = '{0}'", m_sSelectedIS);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MessageBox.Show("Business already tagged.", "Closure Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    pSet.Close();

                    frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                    frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                    frmInspectorLogIn.ShowDialog();

                    if (frmInspectorLogIn.m_sUserCode != "")
                    {
                        sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value);
                        sRemarks = DetailsFrm.txtRemarks.Text.Trim();
                        sAddl = DetailsFrm.txtAddlRemarks.Text.Trim();
                        sReason = sRemarks + " " + sAddl;

                        if (MessageBox.Show("Continue closure tagging?", "Closure Tagging", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pSet.Query = "insert into norec_closure_tagging values (:1, :2, :3, :4)";
                            pSet.AddParameter(":1", m_sSelectedIS);
                            pSet.AddParameter(":2", DateTime.Parse(sDate));
                            pSet.AddParameter(":3", m_sSelectedInspector);
                            pSet.AddParameter(":4", StringUtilities.HandleApostrophe(sReason));
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = string.Format("select * from unofficial_info_tbl where is_number = '{0}'", m_sSelectedIS);
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                    sOwnCode = pSet.GetString("own_code");
                            }
                            pSet.Close();

                            pSet.Query = "insert into tagged_bns values (:1)";
                            pSet.AddParameter(":1", sOwnCode);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Business successfully tagged.", "Closure Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (AuditTrail.InsertTrail("ABIDU-TAG", "multiple table", m_sSelectedIS) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }

            ManageControls();

        }

        private void ManageControls()
        {
            pSet.Query = string.Format("select * from norec_closure_tagging where trim(is_number) = '{0}'", m_sSelectedIS);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    DetailsFrm.btnTag.Enabled = false;
                    DetailsFrm.btnUntag.Enabled = true;
                }
                else
                {
                    DetailsFrm.btnTag.Enabled = true;
                    DetailsFrm.btnUntag.Enabled = false;
                }
            }
        }

        public override void CellClick(int iCol, int iRow)
        {
            m_sSelectedIS = "";
            m_sSelectedDate = "";

            try
            {

                m_sSelectedIS = DetailsFrm.dgvDetails[0, iRow].Value.ToString();
                m_sSelectedDate = DetailsFrm.dgvDetails[1, iRow].Value.ToString();

                OnLoadData(m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
            }
            catch
            {
                this.ClearControls();
            }
        }

        public override void InspectorsCellClick(int iCol, int iRow)
        {
            m_sSelectedInspector = "";

            try
            {
                m_sSelectedInspector = DetailsFrm.dgvInspectors[0, iRow].Value.ToString();
                m_sSelectedIS = "";

                OnLoadInspectorDetails(m_sSelectedInspector, m_sSelectedIS);
            }
            catch
            {
                this.ClearControls();
            }
        }

        public override void RefreshList()
        {
            this.FormLoad();
        }

        public override void Untag()
        {
            OracleResultSet pSet = new OracleResultSet();
            OracleResultSet pRec = new OracleResultSet();

            string sDate = string.Empty;
            string sRemarks = string.Empty;
            string sAddl = string.Empty;
            string sReason = string.Empty;

            pSet.Query = string.Format("select * from norec_closure_tagging where trim(is_number) = '{0}'", m_sSelectedIS);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    if (MessageBox.Show("Are you sure you want to untag this record?", "Closure Un-Tagging", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    pSet.Close();

                    using (frmLiftingOrder frmLiftingOrder = new frmLiftingOrder())
                    {
                        frmLiftingOrder.ISNum = m_sSelectedIS;
                        frmLiftingOrder.BnsType = "UNOFFICIAL";
                        frmLiftingOrder.ShowDialog();

                        if (frmLiftingOrder.Save)
                        {
                            pSet.Query = string.Format("delete from norec_closure_tagging where trim(is_number) = '{0}'", m_sSelectedIS);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            string sOwnCode = string.Empty;

                            pRec.Query = string.Format("select * from unofficial_info_tbl where is_number = '{0}'", m_sSelectedIS);
                            if(pRec.Execute())
                            {
                                if(pRec.Read())
                                {
                                    sOwnCode = pRec.GetString("own_code");
                                }
                            }
                            pRec.Close();

                            pSet.Query = string.Format("delete from tagged_bns where spcl_num = '{0}'", sOwnCode);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Business has been successfully un-tagged.", "Closure Un-Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (AuditTrail.InsertTrail("ABIDU-UNTAG", "multiple table", m_sSelectedIS) == 0)
                            {
                                pSet.Rollback();
                                pSet.Close();
                                MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This business has not been tagged for closure.", "Closure un-tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            ManageControls();
        }

        public override void Print()
        {
            using (frmPrintOptions frmPrintOptions = new frmPrintOptions())
            {
                frmPrintOptions.Option = m_sOption;
                frmPrintOptions.Source = "UNOFFICIAL";
                frmPrintOptions.ShowDialog();
            }
        }

        public override void Violation()
        {
            using (frmOrdinanceViolation frmViolation = new frmOrdinanceViolation())
            {
                //if (frmViolation.ISNum == "") //JARS 20170929
                if(m_sSelectedIS == "") // RMC 20171117 correction in validation of IS Num in UnOfficial Business tagging
                {
                    MessageBox.Show("Please Select I.S. Number with corresponding Inspection Date", "Unofficial", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    frmViolation.BussDtl = "";
                    frmViolation.InspectorCode = m_sSelectedInspector;
                    frmViolation.ISNum = m_sSelectedIS;
                    frmViolation.DateInspected = m_sSelectedDate;
                    frmViolation.ShowDialog();

                    LoadViolation(m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
                }
            }
        }

        public override void IssueNotice()
        {
            using (frmOfficialTagging frmOfficialTagging = new frmOfficialTagging())
            {
                frmOfficialTagging.Inspector = m_sSelectedInspector;
                frmOfficialTagging.Source = "UNOFFICIAL";

                if (m_sSelectedIS != "")
                {
                    frmOfficialTagging.Watcher = "ONE";
                    frmOfficialTagging.ISNum = m_sSelectedIS;
                    frmOfficialTagging.m_sBnsType = DetailsFrm.txtBnsType.Text.Trim();
                    frmOfficialTagging.ShowDialog();
                }
                else
                {
                    frmOfficialTagging.Watcher = "";
                    frmOfficialTagging.ShowDialog();
                }
            }
        }

        public override void Add()
        {
            if (DetailsFrm.btnAdd.Text == "Add")
            {
                frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                frmInspectorLogIn.ShowDialog();

                this.ClearControls();
                this.DetailsFrm.txtISNo.Focus();

                if (frmInspectorLogIn.m_sUserCode != "")
                {
                    m_sSelectedInspector = frmInspectorLogIn.m_sUserCode;
                    this.EnableControls(true);
                    
                    DetailsFrm.txtOwnName.ReadOnly = true;
                    DetailsFrm.dgvViolations.Columns.Clear();

                    DetailsFrm.txtISNo.ReadOnly = false;
                    DetailsFrm.btnPrint.Enabled = false;
                    DetailsFrm.btnViolation.Enabled = false;
                    DetailsFrm.btnTag.Enabled = false;
                    DetailsFrm.btnUntag.Enabled = false;
                    DetailsFrm.btnNotice.Enabled = false;

                    DetailsFrm.btnAdd.Text = "Save";
                    DetailsFrm.btnClose.Text = "Cancel";
                    DetailsFrm.btnEdit.Enabled = false;
                    DetailsFrm.btnDelete.Enabled = false;

                    DetailsFrm.dtpDateInspected.Value = AppSettingsManager.GetCurrentDate();

                    DetailsFrm.txtOwnName.Visible = false;
                    DetailsFrm.txtFirstName.Visible = true;
                    DetailsFrm.txtLastName.Visible = true;
                    DetailsFrm.txtMI.Visible = true;
                    DetailsFrm.lblFN.Visible = true;
                    DetailsFrm.lblMI.Visible = true;

                    DetailsFrm.btnBnsTypes.Enabled = true;
                }
                else
                {
                    this.EnableControls(false);

                    DetailsFrm.btnPrint.Enabled = true;
                    DetailsFrm.btnViolation.Enabled = true;
                    DetailsFrm.btnTag.Enabled = true;
                    DetailsFrm.btnUntag.Enabled = true;
                    DetailsFrm.btnNotice.Enabled = true;

                    DetailsFrm.btnAdd.Text = "Add";
                    DetailsFrm.btnClose.Text = "Close";
                    DetailsFrm.btnEdit.Enabled = true;
                    DetailsFrm.btnDelete.Enabled = true;

                    DetailsFrm.txtOwnName.Visible = true;
                    DetailsFrm.txtFirstName.Visible = false;
                    DetailsFrm.txtLastName.Visible = false;
                    DetailsFrm.txtMI.Visible = false;
                    DetailsFrm.lblFN.Visible = false;
                    DetailsFrm.lblMI.Visible = false;
                }
            }
            else
            {
                SaveRec("Add");

                DetailsFrm.txtOwnName.Visible = true;
                DetailsFrm.txtFirstName.Visible = false;
                DetailsFrm.txtLastName.Visible = false;
                DetailsFrm.txtMI.Visible = false;
                DetailsFrm.lblFN.Visible = false;
                DetailsFrm.lblMI.Visible = false;


            }
        }

        private void SaveRec(string sButtonText)
        {
            OracleResultSet pRec = new OracleResultSet();
            string strOwnCode = string.Empty;
            string sOwnCode = string.Empty;
            string sSysUser = string.Empty;
	        DateTime dtSaveTm;

            if(sButtonText != "Update")
            {
                strOwnCode = AppSettingsManager.EnlistOwner(DetailsFrm.txtLastName.Text.Trim(), DetailsFrm.txtFirstName.Text.Trim(), DetailsFrm.txtMI.Text.Trim(), DetailsFrm.txtOwnAdd.Text.Trim(), DetailsFrm.txtOwnStreet.Text.Trim(), DetailsFrm.cmbOwnDist.Text.Trim().ToUpper(), DetailsFrm.txtOwnZone.Text.Trim(), DetailsFrm.cmbOwnBrgy.Text.Trim().ToUpper(), DetailsFrm.txtOwnCity.Text.Trim(), DetailsFrm.txtOwnProv.Text.Trim(), DetailsFrm.txtZip.Text.Trim());    // RMC 20110819 added toupper, zip field

                if(strOwnCode.Trim() == "")
                    return;

            }

            if (DetailsFrm.txtISNo.Text.Trim() == "")
            {
                // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request  (s)
                if (MessageBox.Show("Inspection # required.\nAuto-generate #?", "Unofficial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DetailsFrm.txtISNo.Text = GenIsNumber();
                }
                else
                {   // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request  (e)
                    MessageBox.Show("Inspection # required.", "Unofficial", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            string sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value);
		
            if (MessageBox.Show("Do you want to save changes?", "Unofficial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (sButtonText == "Update")
                {
                    pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            pSet.Close();

                            pSet.Query = string.Format("select * from unofficial_info_tbl where is_number = '{0}'", m_sSelectedIS);
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    sOwnCode = pSet.GetString("Own_Code").Trim();
                                    sSysUser = pSet.GetString("Sys_User").Trim();
                                    dtSaveTm = pSet.GetDateTime("Save_tm");

                                    pRec.Query = string.Format("update unofficial_info_tbl set bns_nm = '{0}', ", DetailsFrm.txtBnsName.Text.Trim());
                                    pRec.Query += string.Format(" bns_house_no = '{0}', ", DetailsFrm.txtBnsAdd.Text.Trim());
                                    pRec.Query += string.Format(" bns_street = '{0}', ", DetailsFrm.txtBnsStreet.Text.Trim());
                                    pRec.Query += string.Format(" bns_brgy = '{0}', ", DetailsFrm.cmbBnsBrgy.Text.Trim());
                                    pRec.Query += string.Format(" bns_zone = '{0}', ", DetailsFrm.txtBnsZone.Text.Trim());
                                    pRec.Query += string.Format(" bns_dist = '{0}', ", DetailsFrm.cmbBnsDist.Text.Trim());
                                    pRec.Query += string.Format(" bns_mun = '{0}', ", DetailsFrm.txtBnsCity.Text.Trim());
                                    pRec.Query += string.Format(" bns_prov = '{0}', ", DetailsFrm.txtBnsProv.Text.Trim());
                                    pRec.Query += string.Format(" orgn_kind = '{0}', ", DetailsFrm.cmbBnsOrgKind.Text.Trim());
                                    pRec.Query += string.Format(" bns_type = '{0}', ", DetailsFrm.txtBnsType.Text.Trim());
                                    pRec.Query += string.Format(" is_number = '{0}' ", DetailsFrm.txtISNo.Text.Trim());
                                    pRec.Query += string.Format(" where is_number = '{0}'", m_sSelectedIS);
                                    if (pRec.ExecuteNonQuery() == 0)
                                    { }

                                    // RMC 20110819 added own zip, handleapostrophe & setemptytospace (s)
                                    pRec.Query = string.Format("update own_names set own_ln = '{0}', ", StringUtilities.HandleApostrophe(DetailsFrm.txtLastName.Text.Trim()));
                                    pRec.Query += string.Format(" own_fn = '{0}', own_mi = '{1}', ", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtFirstName.Text.Trim())), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtMI.Text.Trim())));
                                    pRec.Query += string.Format(" own_house_no = '{0}', own_street = '{1}', ", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtOwnAdd.Text.Trim())), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtOwnStreet.Text.Trim())));
                                    pRec.Query += string.Format(" own_dist = '{0}', own_zone = '{1}', ", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.cmbOwnDist.Text.Trim())), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtOwnZone.Text.Trim())));
                                    pRec.Query += string.Format(" own_brgy = '{0}', own_mun = '{1}', ", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.cmbOwnBrgy.Text.Trim())), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtOwnCity.Text.Trim())));
                                    pRec.Query += string.Format(" own_prov = '{0}', own_zip = '{1}' where own_code = '{2}'", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtOwnProv.Text.Trim())), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(DetailsFrm.txtZip.Text.Trim())), sOwnCode);   
                                    if (pRec.ExecuteNonQuery() == 0)
                                    { }
                                    // RMC 20110819 added own zip, handleapostrophe & setemptytospace (e)

                                    EnableControls(false);

                                }
                            }
                            pSet.Close();

                            pRec.Query = string.Format("delete from unofficial_dtls where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            string sObject = string.Empty;

                            if (m_sSelectedIS != DetailsFrm.txtISNo.Text.Trim())
                                sObject = "Old IS: " + m_sSelectedIS + ". New IS: " + DetailsFrm.txtISNo.Text.Trim();
                            else
                                sObject = m_sSelectedIS;

                            if (AuditTrail.InsertTrail("ABIDU-EDIT", "multiple table", sObject) == 0)
                            {
                                pRec.Rollback();
                                pRec.Close();
                                MessageBox.Show(pRec.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    m_sSelectedIS = DetailsFrm.txtISNo.Text.Trim();

                    pSet.Query = string.Format("select * from unofficial_dtls where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, m_sSelectedIS, sDate);
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            MessageBox.Show("Record already exists.", "Unofficial", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            pRec.Query = "insert into unofficial_info_tbl values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,'')";  // RMC 20110816 added null for bin_settled field
                            pRec.AddParameter(":1", m_sSelectedIS);
                            pRec.AddParameter(":2", strOwnCode);
                            pRec.AddParameter(":3", StringUtilities.HandleApostrophe(DetailsFrm.txtBnsName.Text.Trim()));
                            pRec.AddParameter(":4", DetailsFrm.txtBnsAdd.Text.Trim());
                            pRec.AddParameter(":5", DetailsFrm.txtBnsStreet.Text.Trim());
                            pRec.AddParameter(":6", DetailsFrm.cmbBnsBrgy.Text.Trim());
                            pRec.AddParameter(":7", DetailsFrm.txtBnsZone.Text.Trim());
                            pRec.AddParameter(":8", DetailsFrm.cmbBnsDist.Text.Trim());
                            pRec.AddParameter(":9", DetailsFrm.txtBnsCity.Text.Trim());
                            pRec.AddParameter(":10", DetailsFrm.txtBnsProv.Text.Trim());
                            pRec.AddParameter(":11", DetailsFrm.cmbBnsOrgKind.Text.Trim());
                            pRec.AddParameter(":12", DetailsFrm.txtBnsType.Text.Trim());
                            pRec.AddParameter(":13", AppSettingsManager.SystemUser.UserCode);
                            pRec.AddParameter(":14", DateTime.Parse(sDate));
                            if (pRec.ExecuteNonQuery() == 0)
                            { }

                            if (AuditTrail.InsertTrail("ABIDU-ADD", "multiple table", DetailsFrm.txtISNo.Text.Trim()) == 0)
                            {
                                pRec.Rollback();
                                pRec.Close();
                                MessageBox.Show(pRec.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                pRec.Query = "insert into unofficial_dtls values(:1,:2,:3,:4,:5,:6)";
                pRec.AddParameter(":1", m_sSelectedInspector);
                pRec.AddParameter(":2", m_sSelectedIS);
                pRec.AddParameter(":3", sDate);
                pRec.AddParameter(":4", StringUtilities.HandleApostrophe(DetailsFrm.txtRemarks.Text.Trim()));
                pRec.AddParameter(":5", StringUtilities.HandleApostrophe(DetailsFrm.txtAddlRemarks.Text.Trim()));
                pRec.AddParameter(":6", m_sOption);
                if (pRec.ExecuteNonQuery() == 0)
                { }

                this.EnableControls(false);

                DetailsFrm.btnPrint.Enabled = true;
                DetailsFrm.btnViolation.Enabled = true;
                DetailsFrm.btnTag.Enabled = true;
                DetailsFrm.btnUntag.Enabled = true;
                DetailsFrm.btnNotice.Enabled = true;

                DetailsFrm.btnAdd.Text = "Add";
                DetailsFrm.btnClose.Text = "Close";
                DetailsFrm.btnEdit.Text = "Edit";
                DetailsFrm.btnClose.Text = "Close";
                DetailsFrm.btnAdd.Enabled = true;
                DetailsFrm.btnEdit.Enabled = true;
                DetailsFrm.btnDelete.Enabled = true;

                DetailsFrm.txtOwnName.Visible = true;
                DetailsFrm.txtFirstName.Visible = false;
                DetailsFrm.txtLastName.Visible = false;
                DetailsFrm.txtMI.Visible = false;
                DetailsFrm.lblFN.Visible = false;
                DetailsFrm.lblMI.Visible = false;

                this.LoadInspector("");

                MessageBox.Show("Record saved.", "Unofficial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No changes has been made", "UnOfficial", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.EnableControls(false);

                DetailsFrm.btnPrint.Enabled = true;
                DetailsFrm.btnViolation.Enabled = true;
                DetailsFrm.btnTag.Enabled = true;
                DetailsFrm.btnUntag.Enabled = true;
                DetailsFrm.btnNotice.Enabled = true;

                DetailsFrm.btnAdd.Text = "Add";
                DetailsFrm.btnClose.Text = "Close";
                DetailsFrm.btnEdit.Text = "Edit";
                DetailsFrm.btnClose.Text = "Close";
                DetailsFrm.btnAdd.Enabled = true;
                DetailsFrm.btnEdit.Enabled = true;
                DetailsFrm.btnDelete.Enabled = true;

                DetailsFrm.txtOwnName.Visible = true;
                DetailsFrm.txtFirstName.Visible = false;
                DetailsFrm.txtLastName.Visible = false;
                DetailsFrm.txtMI.Visible = false;
                DetailsFrm.lblFN.Visible = false;
                DetailsFrm.lblMI.Visible = false;

                this.LoadInspector("");
            }
            
        }

        public override void Edit()
        {
            if (DetailsFrm.btnEdit.Text == "Edit")
            {
                frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                frmInspectorLogIn.ShowDialog();

                //this.ClearControls();
                this.DetailsFrm.bin1.txtTaxYear.Focus();

                if (frmInspectorLogIn.m_sUserCode != "")
                {
                    m_sSelectedInspector = frmInspectorLogIn.m_sUserCode;

                    DetailsFrm.btnEdit.Text = "Update";
                    DetailsFrm.btnClose.Text = "Cancel";

                    this.EnableControls(true);
                    DetailsFrm.bin1.txtTaxYear.ReadOnly = false;
                    DetailsFrm.bin1.txtBINSeries.ReadOnly = false;
                    DetailsFrm.btnViolation.Enabled = false;
                    DetailsFrm.btnAdd.Enabled = false;
                    DetailsFrm.btnDelete.Enabled = true;
                    DetailsFrm.btnPrint.Enabled = false;

                    DetailsFrm.txtRemarks.ReadOnly = false;
                    DetailsFrm.txtAddlRemarks.ReadOnly = false;

                    DetailsFrm.txtOwnName.Visible = false;
                    DetailsFrm.txtFirstName.Visible = true;
                    DetailsFrm.txtLastName.Visible = true;
                    DetailsFrm.txtMI.Visible = true;
                    DetailsFrm.lblFN.Visible = true;
                    DetailsFrm.lblMI.Visible = true;

                    DetailsFrm.btnBnsTypes.Enabled = true;

                    DetailsFrm.txtRemarks.Focus();

                }
                else
                {
                    this.EnableControls(false);

                    DetailsFrm.btnPrint.Enabled = true;
                    DetailsFrm.btnViolation.Enabled = true;
                    DetailsFrm.btnTag.Enabled = true;
                    DetailsFrm.btnUntag.Enabled = true;
                    DetailsFrm.btnNotice.Enabled = true;

                    DetailsFrm.btnEdit.Text = "Edit";
                    DetailsFrm.btnClose.Text = "Close";
                    DetailsFrm.btnAdd.Enabled = true;
                    DetailsFrm.btnClose.Enabled = true;
                    DetailsFrm.btnEdit.Enabled = true;
                    DetailsFrm.btnDelete.Enabled = true;

                    DetailsFrm.txtOwnName.Visible = true;
                    DetailsFrm.txtFirstName.Visible = false;
                    DetailsFrm.txtLastName.Visible = false;
                    DetailsFrm.txtMI.Visible = false;
                    DetailsFrm.lblFN.Visible = false;
                    DetailsFrm.lblMI.Visible = false;
                }
            }
            else
                SaveRec("Update");
        }

        public override void Delete()
        {
            string sDate = string.Empty;

            frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
            frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
            frmInspectorLogIn.ShowDialog();

            if (frmInspectorLogIn.m_sUserCode != "")
            {
                sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value);

                if (MessageBox.Show("Are you sure you want to delete this record?", "Unofficial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = string.Format("select trim(is_number) from unofficial_info_tbl where is_number in ( select trim(is_number) from unoff_with_notice) and is_number = '{0}'", m_sSelectedIS);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            MessageBox.Show("Business already have a notice.\nDeleting not allowed.", "Unofficial", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {
                            pSet.Close();

                            pSet.Query = string.Format("Select * from norec_closure_tagging where is_number = '{0}'", m_sSelectedIS);
                            if(pSet.Execute())
                            {
                                if(pSet.Read())
                                {
                                    MessageBox.Show("Business already tagged for closure.\nDeleting not allowed.", "Un-Official", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                                else
                                {
                                    pSet.Close();
                                    
                                    pSet.Query = string.Format("delete from unofficial_dtls where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }

                                    pSet.Query = string.Format("delete from unofficial_info_tbl where is_number = '{0}'", m_sSelectedIS);
                                    if(pSet.ExecuteNonQuery() == 0)
                                    {}

                                    pSet.Query = string.Format("delete from violation_ufc where inspector_code = '{0}' and is_number = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, m_sSelectedIS, m_sSelectedDate);
                                    if(pSet.ExecuteNonQuery() == 0)
                                    {}
            
                                    this.LoadInspector("");

                                    MessageBox.Show("Record has been deleted.", "Un-Official", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    if (AuditTrail.InsertTrail("ABIDU-DELETE", "multiple table", m_sSelectedIS) == 0)
                                    {
                                        pSet.Rollback();
                                        pSet.Close();
                                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void ValidateISNo()
        {
            string sButtonTextAdd = DetailsFrm.btnAdd.Text;
            string sButtonTextEdit = DetailsFrm.btnEdit.Text;

            if (sButtonTextAdd == "Save" || sButtonTextEdit == "Update")
            {
                pSet.Query = string.Format("select * from unofficial_dtls where is_number = '{0}'", DetailsFrm.txtISNo.Text.Trim());
                if (sButtonTextEdit == "Update")
                    pSet.Query += string.Format(" and '{0}' <> '{1}'", DetailsFrm.txtISNo.Text.Trim(), m_sSelectedIS);
                if(pSet.Execute())
                {
                    if(pSet.Read())
                    {
                        MessageBox.Show("Inspection Slip Number already used.", "Un-Official",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        if (sButtonTextAdd == "Save")
                            DetailsFrm.txtISNo.Text = "";
                        else
                            DetailsFrm.txtISNo.Text = m_sSelectedIS;
                        DetailsFrm.txtISNo.Focus();
                        return;
                    }
                    else
                    {
                        pSet.Close();

                        pSet.Query = string.Format("select * from inspector_details where is_no = '{0}'", DetailsFrm.txtISNo.Text.Trim());
                        if (sButtonTextEdit == "Update")
                            pSet.Query += string.Format(" and '{0}' <> '{1}'", DetailsFrm.txtISNo.Text.Trim(), m_sSelectedIS);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                MessageBox.Show("Inspector Slip Number already used.", "Un-Official", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                if (sButtonTextAdd == "Save")
                                    DetailsFrm.txtISNo.Text = "";
                                else
                                    DetailsFrm.txtISNo.Text = m_sSelectedIS;
                                return;
                            }
                        }
                    }
                }
                
            }
        }


        private string GenIsNumber()
        {
            // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request 
            OracleResultSet pRec = new OracleResultSet();
            int iIsNumber = 0;
            string sIsNumber = "";

            pRec.Query = "select * from unofficial_dtls where is_number like 'U%' order by is_number desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sIsNumber = pRec.GetString("is_number");
                    sIsNumber = sIsNumber.Substring(2, sIsNumber.Length-2);
                    iIsNumber = Convert.ToInt32(sIsNumber) + 1;
                }
                else
                    iIsNumber = 1;
            }
            pRec.Close();

            sIsNumber = string.Format("U-{0:###}", iIsNumber);

            return sIsNumber;
        }
                
        
    }
}