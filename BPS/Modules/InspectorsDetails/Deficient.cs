
// RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.SearchBusiness;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.BPLSApp;
using Amellar.Common.LogIn;

namespace Amellar.Modules.InspectorsDetails
{
    public class Deficient : InsDetails
    {
        OracleResultSet pSet = new OracleResultSet();
        private string m_sBusinessName = string.Empty;
        private string m_sOwnerName = string.Empty;
        private string m_sSelectedBin = string.Empty;        
        private string m_sSelectedDate = string.Empty;
        private string m_sOption = string.Empty;
        private string m_sSelectedInspector = string.Empty;
        BPLSAppSettingList sList = new BPLSAppSettingList();
                
        DateTime dtDate1;
        DateTime dtDate2;

        public Deficient(frmInspectorDetails Form)
            : base(Form)
        {
        }
        
        public override void FormLoad()
        {
            DetailsFrm.lblIS.Location = new System.Drawing.Point(365, 159);
            DetailsFrm.txtISNo.Location = new System.Drawing.Point(398, 156);
            DetailsFrm.bin1.Visible = true;
            DetailsFrm.txtISNo.ReadOnly = true;
            DetailsFrm.bin1.txtTaxYear.Focus();
            m_sOption = "I";

            m_sSelectedBin = "";
            this.LoadInspector(m_sSelectedBin);
            
        }


        private void OnLoadInspectorDetails(string strInspectorCode, string strBin)
        {
            DetailsFrm.dgvDetails.Columns.Clear();
            DetailsFrm.dgvDetails.Columns.Add("BIN", "BIN");
            DetailsFrm.dgvDetails.Columns.Add("DATE", "Date Inspected");
            DetailsFrm.dgvDetails.RowHeadersVisible = false;
            DetailsFrm.dgvDetails.Columns[0].Width = 160;
            DetailsFrm.dgvDetails.Columns[1].Width = 120;
            DetailsFrm.dgvDetails.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DetailsFrm.dgvDetails.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;

            string sBin = string.Empty;
            string sRemarks = string.Empty;
            string sDate = string.Empty;
            
            if (strBin != "")
            {
                //pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and is_option = '{1}' and bin = '{2}' order by inspector_code, date_inspected", strInspectorCode, m_sOption, strBin);
                pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and is_option = '{1}' and bin = '{2}' and is_settled = 'N' order by inspector_code, date_inspected", strInspectorCode, m_sOption, strBin); // RMC 20110816
            }
            else
            {
                //pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and is_option = '{1}' order by date_inspected,bin", strInspectorCode, m_sOption);
                pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and is_option = '{1}' and is_settled = 'N' order by date_inspected,bin", strInspectorCode, m_sOption); // RMC 20110816
            }
            if(pSet.Execute())
            {
                int iRow = 0;
                m_sSelectedBin = strBin;

                while (pSet.Read())
                {
                    sBin = pSet.GetString("bin").Trim();
                    sDate = pSet.GetString("date_inspected");
                    sRemarks = pSet.GetString("inspector_remarks");

                    if (iRow == 0)
                    {
                        m_sSelectedInspector = strInspectorCode;
                        m_sSelectedBin = sBin;
                        m_sSelectedDate = sDate;
                    }

                    DetailsFrm.dgvDetails.Rows.Add("");
                    DetailsFrm.dgvDetails[0, iRow].Value = sBin;
                    DetailsFrm.dgvDetails[1, iRow].Value = sDate;
                    iRow++;
                    
                }

                if (m_sSelectedBin != "")
                    OnLoadData(m_sSelectedInspector, m_sSelectedBin, m_sSelectedDate);
            }
            pSet.Close();
        }

        private void OnLoadData(string strInspectorCode, string strBin, string sDateInspected)
        {
            this.ClearControls();

            m_sBusinessName = "";
            m_sOwnerName = "";
            DetailsFrm.bin1.txtLGUCode.Text = strBin.Substring(0, 3);   //117-00-2008-0003944
            DetailsFrm.bin1.txtDistCode.Text = strBin.Substring(4, 2);   //117-00-2008-0003944
            DetailsFrm.bin1.txtTaxYear.Text = strBin.Substring(7, 4);    //117-00-2008-0003944
            DetailsFrm.bin1.txtBINSeries.Text = strBin.Substring(12, 7);

            pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}' and is_option = '{3}'", strInspectorCode, strBin,sDateInspected, m_sOption);
            pSet.Query += " and is_settled = 'N'";  // RMC 20110816
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    DetailsFrm.txtRemarks.Text = pSet.GetString("inspector_remarks");
		            DetailsFrm.txtAddlRemarks.Text = pSet.GetString("addition_info");
		            DetailsFrm.txtISNo.Text = pSet.GetString("is_no");
                    DetailsFrm.dtpDateInspected.Value = DateTime.Parse(pSet.GetString("date_inspected"));
		        }
            }
            pSet.Close();

            LoadBusinessInfo(strBin);

	        DetailsFrm.txtBnsName.Text = m_sBusinessName;
	        DetailsFrm.txtOwnName.Text = m_sOwnerName;
	        
            LoadViolation(strInspectorCode, strBin, sDateInspected);

            //DetailsFrm.btnEdit.Enabled = true;
            //DetailsFrm.btnDelete.Enabled = true;

            ManageControls();

            DetailsFrm.btnSearch.Text = "Clear";
        }

        private void LoadBusinessInfo(string strBin)
        {
            string sBnsCode = string.Empty;
            StringBuilder strQuery = new StringBuilder();

            pSet.Query = string.Format("select * from business_que where BIN = '{0}'", strBin);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strQuery.Append(string.Format("select * from business_que where BIN = '{0}'", strBin));
                }
                else
                {
                    pSet.Close();

                    pSet.Query = string.Format("select * from businesses where BIN = '{0}'", strBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            strQuery.Append(string.Format("select * from businesses where BIN = '{0}'", strBin));
                        }
                        else
                        {
                            pSet.Close();

                            pSet.Query = string.Format("select * from buss_hist where BIN = '{0}'", strBin);
                            if (pSet.Execute())
                            {
                                if (pSet.Read())
                                {
                                    strQuery.Append(string.Format("select * from buss_hist where BIN = '{0}'", strBin));
                                }
                            }
                            pSet.Close();
                        }
                    }
                }
            }
                        
            pSet.Query = strQuery.ToString();
            if(strQuery.ToString() == "") //JARS 20170830
            {
                MessageBox.Show("Business already renewed/paid for current tax year.");
                return;
            }
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
                    sBnsCode = pSet.GetString("bns_code").Trim();

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

                    DetailsFrm.txtBnsType.Text = AppSettingsManager.GetBnsDesc(sBnsCode);
                }
            }
            pSet.Close();
        }

        private void LoadViolation(string strInspectorCode, string strBin, string sDateInspected)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sViolation = "";
            string sReference = "";
    
            dtDate2 = DetailsFrm.dtpDateInspected.Value;
            // GDE 20130709 add try and catch
            try
            {
                dtDate1 = DateTime.Parse(m_sSelectedDate);
            }
            catch
            {
                dtDate1 = DateTime.Now;
            }
        
            if(DetailsFrm.btnEdit.Text == "Update" && (dtDate1 != dtDate2))
            {
		        pSet.Query = "update violations set date_inspected = :1 where inspector_code = :2 and bin = :3 and date_inspected = :4";
                pSet.AddParameter(":1", dtDate2);
                pSet.AddParameter(":2", strInspectorCode);
                pSet.AddParameter(":3", strBin);
                pSet.AddParameter(":4", dtDate1);
		        if(pSet.ExecuteNonQuery() == 0)
                {}
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

            pSet.Query = string.Format("select * from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}' order by violation_code", strInspectorCode, strBin, sDateInspected);
		    if(pSet.Execute())
            {
                while(pSet.Read())
                {
		            sCode = pSet.GetString("violation_code").Trim();

			        pRec.Query = string.Format("select * from violation_table where violation_code = '{0}'",sCode);
                    if(pRec.Execute())
			        {
                        if(pRec.Read())
			            {
				            sViolation = pRec.GetString("violation_desc").Trim();
				            sReference = pRec.GetString("reference").Trim();
			            }
                    }
                    pRec.Close();

				    DetailsFrm.dgvViolations.Rows.Add("");
			        DetailsFrm.dgvViolations[0,iRow].Value = sCode;
			        DetailsFrm.dgvViolations[1,iRow].Value = sViolation;
                    DetailsFrm.dgvViolations[2,iRow].Value = sReference;
                    iRow++;
			    }
            }
            pSet.Close();
		}

        public override void CellClick(int iCol, int iRow)
        {
            m_sSelectedBin = "";
            m_sSelectedDate = "";

            try
            {

                m_sSelectedBin = DetailsFrm.dgvDetails[0, iRow].Value.ToString();
                m_sSelectedDate = DetailsFrm.dgvDetails[1, iRow].Value.ToString();
                
                OnLoadData(m_sSelectedInspector, m_sSelectedBin, m_sSelectedDate);
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
                m_sSelectedBin = "";

                OnLoadInspectorDetails(m_sSelectedInspector, m_sSelectedBin);
            }
            catch
            {
                this.ClearControls();
            }
        }

        public override void Search()
        {
            if (DetailsFrm.btnSearch.Text == "Clear")
            {
                this.ClearControls();
                DetailsFrm.btnSearch.Text = "Search";
                DetailsFrm.bin1.txtTaxYear.Focus();

                m_sSelectedBin = "";
                DetailsFrm.dgvViolations.Columns.Clear();
                //LoadInspector(m_sSelectedBin);
            }
            else
            {
                if (DetailsFrm.bin1.txtTaxYear.Text != "" || DetailsFrm.bin1.txtBINSeries.Text != "")
                {
                    m_sSelectedBin = DetailsFrm.bin1.GetBin();

                    LoadBusinessInfo(m_sSelectedBin);
                    LoadInspector(m_sSelectedBin);
                }
                else
                {
                    frmSearchBusiness frmSearchBns = new frmSearchBusiness();

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        DetailsFrm.bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        DetailsFrm.bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        m_sSelectedBin = DetailsFrm.bin1.GetBin();

                        LoadInspector(m_sSelectedBin);

                        
                    }
                    else
                        this.ClearControls();
                }
            }
        }

        public override void Add()
        {
            if (DetailsFrm.btnAdd.Text == "Add")
            {
                frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                //frmInspectorLogIn.ShowDialog(); // GDE disabled this validation as per jester 20120430
                this.ClearControls();
                this.DetailsFrm.bin1.txtTaxYear.Focus();

                if (frmInspectorLogIn.m_sUserCode != "")
                {
                    //m_sSelectedInspector = frmInspectorLogIn.m_sUserCode; // GDE 20120430
                   
                    this.EnableControls(true);
                    DetailsFrm.txtBnsName.ReadOnly = true;
                    DetailsFrm.txtOwnName.ReadOnly = true;

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
                }
            }
            else
            {
                Validate();
            }

        }

        public override void Edit()
        {
            if (DetailsFrm.btnEdit.Text == "Edit")
            {
                frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                frmInspectorLogIn.ShowDialog();

                this.ClearControls();
                this.DetailsFrm.bin1.txtTaxYear.Focus();

                if (frmInspectorLogIn.m_sUserCode != "")
                {
                    m_sSelectedInspector = frmInspectorLogIn.m_sUserCode;

                   //m_sDate1.Format("%02d/%02d/%04d", mc_vdtDateInspected.GetMonth(), mc_vdtDateInspected.GetDay(), mc_vdtDateInspected.GetYear()); // CTS 10122003

                    DetailsFrm.btnEdit.Text = "Update";
                    DetailsFrm.btnClose.Text = "Cancel";

                    this.EnableControls(false);
                    DetailsFrm.bin1.txtTaxYear.ReadOnly = false;
                    DetailsFrm.bin1.txtBINSeries.ReadOnly = false;
                    DetailsFrm.btnViolation.Enabled = false;
                    DetailsFrm.btnAdd.Enabled = false;
                    DetailsFrm.btnDelete.Enabled = true;
                    DetailsFrm.btnPrint.Enabled = false;
                    
                    DetailsFrm.txtRemarks.ReadOnly = false;
                    DetailsFrm.txtAddlRemarks.ReadOnly = false;
                    DetailsFrm.txtRemarks.Focus();
                    
                }
            }
            else
                Validate();
        }

        private void Validate()
        {
            string sBin = string.Empty;
            string sDate = string.Empty;
            bool bSw = false;

            if (m_sOption == "I" && DetailsFrm.txtISNo.Text.ToString().Trim() == "")
	        {
                // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request  (s)
                if (MessageBox.Show("Inspection # required.\nAuto-generate #?", "Unofficial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DetailsFrm.txtISNo.Text = GenIsNumber();
                }
                else
                {   // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request  (e)
                    MessageBox.Show("Enter Inspection Slip Number.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
	        }

            if(m_sSelectedInspector == "")
            {
                MessageBox.Show("Enter Inspection code.","Deficient",MessageBoxButtons.OK,MessageBoxIcon.Stop);
		        return;
            }
		
            if(DetailsFrm.bin1.txtTaxYear.Text != "" && DetailsFrm.bin1.txtBINSeries.Text != "")
	        {
		        sBin = DetailsFrm.bin1.GetBin();

                sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value);
		                		
		        pSet.Query = string.Format("select * from businesses where bin = '{0}'",sBin);
                if(pSet.Execute())
                {
                    if(pSet.Read())
                    {
                        bSw = true;
                    }
                    else
                    {
                        pSet.Close();
		                
                        pSet.Query = string.Format("select * from business_que where bin = '{0}'",sBin);
			            if(pSet.Execute())
                        {
                            if(pSet.Read())
                            {
                                bSw = true;

                            }
                            else
			                {
                                pSet.Close();

				                pSet.Query = string.Format("select * from buss_hist where bin = '{0}'",sBin);
				                if(pSet.Execute())
                                {
                                    if(pSet.Read())
                                    {
                                        bSw = true;
			                        }
			                    }
                                pSet.Close();
                            }
                        }
                    }
                }
				
				if(bSw)
		        {
			        LoadBusinessInfo(sBin);

			        pSet.Query = string.Format("select * from inspector_details where inspector_code = '{0}' and bin = '{1}'", m_sSelectedInspector, sBin);
                    pSet.Query+= string.Format(" and date_inspected = '{0}' and is_option = '{1}'", sDate, m_sOption);
                    pSet.Query += " and is_settled = 'N'";  // RMC 20110816
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            //Edit
                            if (DetailsFrm.btnEdit.Text == "Update")
                            {
                                if (MessageBox.Show("Do you want to save changes?", "Deficient", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    pSet.Close();

                                    pSet.Query = string.Format("delete from inspector_details where inspector_code = '{0}'", m_sSelectedInspector);
                                    pSet.Query += string.Format(" and bin = '{0}' and date_inspected = '{1}' and is_option = '{2}'", m_sSelectedBin, m_sSelectedDate, m_sOption);
                                    pSet.Query += " and is_settled = 'N'";  // RMC 20110816
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }

                                    pSet.Query = "insert into inspector_details values(:1,:2,:3,:4,:5,:6,:7,'N')";
                                    pSet.AddParameter(":1", m_sSelectedInspector);
                                    pSet.AddParameter(":2", sBin);
                                    pSet.AddParameter(":3", sDate);
                                    pSet.AddParameter(":4", DetailsFrm.txtRemarks.Text);
                                    pSet.AddParameter(":5", DetailsFrm.txtAddlRemarks.Text);
                                    pSet.AddParameter(":6", DetailsFrm.txtISNo.Text);
                                    pSet.AddParameter(":7", m_sOption);
                                    if (pSet.ExecuteNonQuery() == 0)
                                    { }

                                    DetailsFrm.btnEdit.Text = "Edit";
                                    DetailsFrm.btnClose.Text = "Close";
                                    this.EnableControls(false);

                                    DetailsFrm.btnPrint.Enabled = true;
                                    DetailsFrm.btnViolation.Enabled = true;
                                    DetailsFrm.btnTag.Enabled = true;
                                    DetailsFrm.btnUntag.Enabled = true;
                                    DetailsFrm.btnNotice.Enabled = true;
                                    DetailsFrm.btnAdd.Enabled = true;
                                    DetailsFrm.btnDelete.Enabled = true;

                                    m_sSelectedBin = "";
                                    this.LoadInspector(m_sSelectedBin);

                                    MessageBox.Show("Record saved.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                    if (AuditTrail.InsertTrail("ABIDD-EDIT", "inspector_details", sBin) == 0)
                                    {
                                        pSet.Rollback();
                                        pSet.Close();
                                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Record already exists.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else
                        {   //Add

                            if (MessageBox.Show("Do you want to save this record?", "Deficient", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                pSet.Close();

                                pSet.Query = "insert into inspector_details values(:1,:2,:3,:4,:5,:6,:7,'N')";
                                pSet.AddParameter(":1", m_sSelectedInspector);
                                pSet.AddParameter(":2", sBin);
                                pSet.AddParameter(":3", sDate);
                                pSet.AddParameter(":4", StringUtilities.HandleApostrophe(DetailsFrm.txtRemarks.Text.ToString().Trim()));
                                pSet.AddParameter(":5", StringUtilities.HandleApostrophe(DetailsFrm.txtAddlRemarks.Text.ToString().Trim()));
                                pSet.AddParameter(":6", StringUtilities.HandleApostrophe(DetailsFrm.txtISNo.Text.ToString().Trim()));
                                pSet.AddParameter(":7", m_sOption);
                                if (pSet.ExecuteNonQuery() == 0)
                                { }

                                DetailsFrm.btnAdd.Text = "Add";
                                DetailsFrm.btnClose.Text = "Close";
                                this.EnableControls(false);

                                DetailsFrm.btnPrint.Enabled = true;
                                DetailsFrm.btnViolation.Enabled = true;
                                DetailsFrm.btnTag.Enabled = true;
                                DetailsFrm.btnUntag.Enabled = true;
                                DetailsFrm.btnNotice.Enabled = true;
                                DetailsFrm.btnEdit.Enabled = true;
                                DetailsFrm.btnDelete.Enabled = true;

                                m_sSelectedBin = "";
                                this.LoadInspector(m_sSelectedBin);

                                MessageBox.Show("Record saved.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                if (AuditTrail.InsertTrail("ABIDD-ADD", "inspector_details", sBin) == 0)
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
        		else
		        {
                    MessageBox.Show("Business record does not exists.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Information);
		        }
	        }
        }

        public override void Delete()
        {
            string sBin = string.Empty;
            string sDate = string.Empty;

            sBin = DetailsFrm.bin1.GetBin();

            frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
            frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
            frmInspectorLogIn.ShowDialog();

            if (frmInspectorLogIn.m_sUserCode != "")
	    	{
		    	sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value);

                if (MessageBox.Show("Are you sure you want to delete this record?", "Deficient", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pSet.Query = string.Format("Select bin from inspector_details where bin in (select bin from official_with_notice) and bin = '{0}'", sBin);
                    if(pSet.Execute())
                    {
                        if(pSet.Read())
                        {
                            MessageBox.Show("Business already have a notice.\nDeleting not allowed.","Deficient",MessageBoxButtons.OK, MessageBoxIcon.Stop);
				            return;
			            }
			            else
			            {
                            pSet.Close();

			                pSet.Query = string.Format("delete from inspector_details where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, sBin, sDate);
                            pSet.Query += " and is_settled = 'N'";  // RMC 20110816
				            if(pSet.ExecuteNonQuery() == 0)
                            {}
			            }
                    }
			
			        pSet.Query = string.Format("delete from violations where inspector_code = '{0}' and bin = '{1}' and date_inspected = '{2}'", m_sSelectedInspector, sBin, sDate);
                    if (pSet.ExecuteNonQuery() == 0)
                    { }

                    m_sSelectedBin = "";
                    this.LoadInspector(m_sSelectedBin);

                    MessageBox.Show("Record has been deleted.", "Deficient", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (AuditTrail.InsertTrail("ABIDD-DEL", "multiple table", sBin) == 0)
                    {
                        pSet.Rollback();
                        pSet.Close();
                        MessageBox.Show(pSet.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        public override void Print()
        {
            using (frmPrintOptions frmPrintOptions = new frmPrintOptions())
            {
                frmPrintOptions.Option = m_sOption;
                frmPrintOptions.ShowDialog();
            }
        }

        public override void Violation()
        {
            using (frmOrdinanceViolation frmViolation = new frmOrdinanceViolation())
            {
                frmViolation.BussDtl = "DEFICIENT";
                frmViolation.InspectorCode = m_sSelectedInspector;
                frmViolation.Bin = m_sSelectedBin;
                frmViolation.DateInspected = m_sSelectedDate;
                frmViolation.ShowDialog();

                LoadViolation(m_sSelectedInspector, m_sSelectedBin, m_sSelectedDate);
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
	        
            
	        pSet.Query = string.Format("select * from official_closure_tagging where bin = '{0}'",m_sSelectedBin);
            if(pSet.Execute())
            {
                if(pSet.Read())
	            {
                    MessageBox.Show("Business already tagged.", "Closure Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);
		            return;
	            }
	            else
	            {
                    pSet.Close();

                    // GDE 20120531 (s){
                    /*
                    frmInspectorLogIn frmInspectorLogIn = new frmInspectorLogIn();
                    frmInspectorLogIn.txtUserCode.Text = m_sSelectedInspector;
                    frmInspectorLogIn.ShowDialog();
                     */
                    frmLogIn fLogIn = new frmLogIn();
                    fLogIn.ShowDialog();
                    // GDE 20120531 (e)}
    
                    //if (frmInspectorLogIn.m_sUserCode != "") // GDE 20120531
                    if (fLogIn.m_sUserCode != "")
                    {
		                //sDate = string.Format("{0:MM/dd/yyyy}", DetailsFrm.dtpDateInspected.Value); // GDE 20120531
                        sDate = string.Format("{0:MM/dd/yyyy}", DateTime.Now);
			            sRemarks = DetailsFrm.txtRemarks.Text.Trim();
                        sAddl = DetailsFrm.txtAddlRemarks.Text.Trim();    
			            sReason = sRemarks + " " + sAddl;

                        if (MessageBox.Show("Continue closure tagging?", "Closure Tagging", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            pSet.Query = "insert into official_closure_tagging values (:1, :2, :3, :4)";
                            pSet.AddParameter(":1", m_sSelectedBin);
                            pSet.AddParameter(":2", DateTime.Parse(sDate));
                            pSet.AddParameter(":3", m_sSelectedInspector);
                            pSet.AddParameter(":4", StringUtilities.HandleApostrophe(sReason));
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sSelectedBin);

                            pSet.Query = "insert into tagged_bns values (:1)";
                            pSet.AddParameter(":1", sOwnCode);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Business successfully tagged.", "Closure Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (AuditTrail.InsertTrail("ABIDD-TAG", "multiple table", m_sSelectedBin) == 0)
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

        public override void Untag()
        {
            OracleResultSet pSet = new OracleResultSet();

            string sDate = string.Empty;
            string sRemarks = string.Empty;
            string sAddl = string.Empty;
            string sReason = string.Empty;
            
            pSet.Query = string.Format("select * from official_closure_tagging where bin = '{0}'", m_sSelectedBin);
            if(pSet.Execute())
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
                        frmLiftingOrder.Bin = m_sSelectedBin;
                        frmLiftingOrder.ShowDialog();

                        if (frmLiftingOrder.Save)
                        {
                            pSet.Query = string.Format("delete from official_closure_tagging where bin = '{0}'", m_sSelectedBin);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            pSet.Query = string.Format("delete from closure_tagging where bin = '{0}'", m_sSelectedBin);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            string sOwnCode = AppSettingsManager.GetBnsOwnCode(m_sSelectedBin);

                            pSet.Query = string.Format("delete from tagged_bns where spcl_num = '{0}'", sOwnCode);
                            if (pSet.ExecuteNonQuery() == 0)
                            { }

                            MessageBox.Show("Business has been successfully un-tagged.", "Closure Un-Tagging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (AuditTrail.InsertTrail("ABIDD-UNTAG", "multiple table", m_sSelectedBin) == 0)
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

        private void ManageControls()
        {
            pSet.Query = string.Format("select * from official_closure_tagging where bin = '{0}'", m_sSelectedBin);
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

        public override void IssueNotice()
        {
            using (frmOfficialTagging frmOfficialTagging = new frmOfficialTagging())
            {
                frmOfficialTagging.Inspector = m_sSelectedInspector;
                frmOfficialTagging.Source = "DEFICIENT";

                if (m_sSelectedBin != "")
                {
                    frmOfficialTagging.Watcher = "ONE";
                    frmOfficialTagging.Bin = m_sSelectedBin;
                    frmBnsInfo fBnsInfo = new frmBnsInfo();
                    fBnsInfo.FillData(m_sSelectedBin);
                    fBnsInfo.ShowDialog();
                    frmOfficialTagging.ShowDialog();
                }
                else
                {
                    frmOfficialTagging.Watcher = "";
                    frmOfficialTagging.ShowDialog();
                }
            }
        }

        private void LoadInspector(string strBin)
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

            if (strBin != "")
            {
                pSet.Query = "select * from inspector where inspector_code in ";
                pSet.Query += string.Format("(select inspector_code from inspector_details where bin = '{0}'", strBin); // RMC 20111007 corrected error in inspection module, deleted ')' 
                pSet.Query += " and is_settled = 'N')";   // RMC 20110816    // RMC 20111007 corrected error in inspection module, added ')' 
            }
            else
            {
                pSet.Query = "select * from inspector order by inspector_code";
            }
            if (pSet.Execute())
            {
                int iRow = 0;
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

            try
            {
                OnLoadInspectorDetails(m_sSelectedInspector, strBin);
            }
            catch
            {
            }

        }

        public override void RefreshList()
        {
            this.FormLoad();
        }

        private string GenIsNumber()
        {
            // RMC 20120316 added auto-generation of Inspection # in Inspector's module, user-request 
            OracleResultSet pRec = new OracleResultSet();
            int iIsNumber = 0;
            string sIsNumber = "";

            pRec.Query = "select * from inspector_details where is_no like 'D%' order by is_no desc";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sIsNumber = pRec.GetString("is_no");
                    sIsNumber = sIsNumber.Substring(2, sIsNumber.Length - 2);
                    iIsNumber = Convert.ToInt32(sIsNumber) + 1;
                }
                else
                    iIsNumber = 1;
            }
            pRec.Close();

            sIsNumber = string.Format("D-{0:###}", iIsNumber);

            return sIsNumber;
        }
    }
}
