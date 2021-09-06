
// RMC 20120419 added tagging of inspector in Business Mapping
// RMC 20120417 Corrections in validation of Permit for official business tagging in business mapping
// RMC 20120330 Modified validation of Business mapping tagging in GIS table
// RMC 20120207 modifications in business mapping

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;
using Amellar.Common.frmBns_Rec;

namespace Amellar.Modules.BusinessMapping
{
    public class MappingOfficial:Mapping
    {
        private bool m_bUpdateRecord = false;

        public MappingOfficial(frmMapping Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            MappingFrm.bin1.Visible = true;
            MappingFrm.txtBIN.Visible = false;
            MappingFrm.chkUndeclared.Visible = false;
            MappingFrm.chkNotEncoded.Visible = false;
            MappingFrm.btnUntag.Enabled = false;
            m_bUpdateRecord = false;

            LoadInspector();
        }

        public override void BinSearch()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = string.Empty;

            m_bUpdateRecord = false;

            if (MappingFrm.btnSearch.Text == "Clear")
            {
                if (TaskMan.IsObjectLock(MappingFrm.bin1.GetBin(), "BUSINESS MAPPING", "DELETE", "ASS"))
                { }

                MappingFrm.bin1.txtTaxYear.Text = "";
                MappingFrm.bin1.txtBINSeries.Text = "";
                ClearControls(1);
                ClearControls(2);
                ClearControls(3);
                ClearControls(4);
                ClearOtherControls();   // RMC 20120419 added tagging of inspector in Business Mapping
                MappingFrm.btnSearch.Text = "Search Business";
                MappingFrm.m_bInit = true;
                MappingFrm.btnUntag.Enabled = false;
                
            }
            else
            {
                if (MappingFrm.bin1.txtTaxYear.Text == "" || MappingFrm.bin1.txtBINSeries.Text == "")
                {
                    frmSearchBns = new frmSearchBusiness();

                    frmSearchBns.ShowDialog();
                    if (frmSearchBns.sBIN.Length > 1)
                    {
                        MappingFrm.bin1.txtTaxYear.Text = frmSearchBns.sBIN.Substring(7, 4).ToString();
                        MappingFrm.bin1.txtBINSeries.Text = frmSearchBns.sBIN.Substring(12, 7).ToString();

                        if (!TaskMan.IsObjectLock(MappingFrm.bin1.GetBin(), "BUSINESS MAPPING", "ADD", "ASS"))
                        {
                            if (ValidateBINIfMatched(MappingFrm.bin1.GetBin()))
                                LoadValues(MappingFrm.bin1.GetBin());
                        }
                    }
                    
                }
                else
                {

                    if (!TaskMan.IsObjectLock(MappingFrm.bin1.GetBin(), "BUSINESS MAPPING", "ADD", "ASS"))
                    {
                        if (ValidateBINIfMatched(MappingFrm.bin1.GetBin()))
                            LoadValues(MappingFrm.bin1.GetBin());
                    }

                }
            }

        }

        public override void ChangePermit(object sender, EventArgs e)
        {
            if (MappingFrm.chkChangePermit.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtPermitNo.ReadOnly = false;
                MappingFrm.dtpPermitDate.Enabled = true;
                MappingFrm.txtPermitNo.Focus();
                
                LoadAdjustments(MappingFrm.bin1.GetBin(), "CPMT");
            }
            else
            {
                MappingFrm.txtPermitNo.ReadOnly = true;
                MappingFrm.dtpPermitDate.Enabled = false;
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                {
                    MappingFrm.txtPermitNo.Text = MappingFrm.m_sPermit.Substring(5, 5);
                }
            }
        }

        public override void ChangePermitYear(object sender, EventArgs e)
        {
            if (MappingFrm.chkChangeTaxYear.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtPermitYear.ReadOnly = false;
                MappingFrm.dtpPermitDate.Enabled = true;
                MappingFrm.txtPermitYear.Focus();

                LoadAdjustments(MappingFrm.bin1.GetBin(), "CTYR");  //tax year
            }
            else
            {
                MappingFrm.txtPermitYear.ReadOnly = true;
                MappingFrm.dtpPermitDate.Enabled = false;
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                {
                    MappingFrm.txtPermitYear.Text = MappingFrm.m_sPermit.Substring(0, 4);
                }
            }
        }

        public override void LoadValues(string sBIN)
        {
            ClearControls(1);
            ClearControls(2);
            ClearControls(3);
            ClearControls(4);
            ClearOtherControls();
            MappingFrm.m_bAddNewRecord = false;

            OracleResultSet pRec = new OracleResultSet();

            sList.ReturnValueByBin = sBIN;

            if (sList.BPLSAppSettings.Count == 0)
            {
                MessageBox.Show("Record not found", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            for (int i = 0; i < sList.BPLSAppSettings.Count; i++)
            {
                MappingFrm.txtOfficialBussName.Text = StringUtilities.RemoveApostrophe(sList.BPLSAppSettings[i].sBnsNm);
                MappingFrm.m_sBnsNm = MappingFrm.txtOfficialBussName.Text.Trim();
                MappingFrm.txtBussAdd.Text = sList.BPLSAppSettings[i].sBnsHouseNo;
                MappingFrm.m_sAddNo = MappingFrm.txtBussAdd.Text;
                MappingFrm.txtBussStreet.Text = sList.BPLSAppSettings[i].sBnsStreet;
                MappingFrm.m_sStreet = MappingFrm.txtBussStreet.Text.Trim();
                MappingFrm.cmbBussBrgy.Text = sList.BPLSAppSettings[i].sBnsBrgy;
                MappingFrm.m_sBrgy = MappingFrm.cmbBussBrgy.Text.Trim();
                MappingFrm.txtBussZone.Text = sList.BPLSAppSettings[i].sBnsZone;
                MappingFrm.m_sZone = MappingFrm.txtBussZone.Text.Trim();
                MappingFrm.cmbBussDist.Text = sList.BPLSAppSettings[i].sBnsDist;
                MappingFrm.m_sDist = MappingFrm.cmbBussDist.Text.Trim();
                MappingFrm.txtBussCity.Text = sList.BPLSAppSettings[i].sBnsMun;
                MappingFrm.m_sMun = MappingFrm.txtBussCity.Text.Trim();
                MappingFrm.txtBussProv.Text = sList.BPLSAppSettings[i].sBnsProv;
                MappingFrm.m_sProv = MappingFrm.txtBussProv.Text.Trim();
                MappingFrm.m_sBnsCode = sList.BPLSAppSettings[i].sBnsCode;
                MappingFrm.txtBnsType.Text = AppSettingsManager.GetBnsDesc(MappingFrm.m_sBnsCode);
                MappingFrm.cmbOrgKind.Text = sList.BPLSAppSettings[i].sOrgnKind;
                MappingFrm.cmbPlace.Text = sList.BPLSAppSettings[i].sPlaceOccupancy;
                MappingFrm.m_sPlace = MappingFrm.cmbPlace.Text;
                MappingFrm.m_sBnsStat = sList.BPLSAppSettings[i].sBnsStat;
                MappingFrm.m_sPermit = sList.BPLSAppSettings[i].sPermitNo;
                if (MappingFrm.m_sPermit != "") // RMC 20120207 modifications in business mapping
                {
                    MappingFrm.txtPermitYear.Text = MappingFrm.m_sPermit.Substring(0, 4);
                    MappingFrm.txtPermitNo.Text = MappingFrm.m_sPermit.Substring(5, 5);
                }
                MappingFrm.m_sPermitDate = sList.BPLSAppSettings[i].sPermitDate;
                MappingFrm.dtpPermitDate.Value = DateTime.Parse(MappingFrm.m_sPermitDate);
                MappingFrm.m_sLeaseAmt = sList.BPLSAppSettings[i].sRentLeaseMo;
                MappingFrm.txtRent.Text = string.Format("{0:#,###.00}", Convert.ToDouble(sList.BPLSAppSettings[i].sRentLeaseMo));
                MappingFrm.m_sBnsLoc = AppSettingsManager.GetBnsAddress(sBIN);
                MappingFrm.m_sArea = sList.BPLSAppSettings[i].m_sFLR_AREA;
                MappingFrm.m_sStoreys = sList.BPLSAppSettings[i].m_sNUM_STOREYS;
                MappingFrm.txtArea.Text = string.Format("{0:#,###.00}", Convert.ToDouble(MappingFrm.m_sArea));
                MappingFrm.txtStoreys.Text = string.Format("{0:#,###}", Convert.ToDouble(MappingFrm.m_sStoreys));

                MappingFrm.m_sOwnCode = sList.BPLSAppSettings[i].sOwnCode;
                MappingFrm.m_sBussOwnCode = sList.BPLSAppSettings[i].sBussOwn;

                LoadOwner(sList.BPLSAppSettings[i].sOwnCode, true);
                LoadOwner(sList.BPLSAppSettings[i].sBussOwn, false);

                MappingFrm.btnSearch.Text = "Clear";
                MappingFrm.btnSave.Enabled = true;
                EnableControls(4, true);

                // if already matched
                LoadGisValues(MappingFrm.bin1.GetBin());
                LoadAdjustments(MappingFrm.bin1.GetBin(), "");
                LoadAdjustmentsMain(MappingFrm.bin1.GetBin(), true);

                MappingFrm.m_bInit = false;

                m_bUpdateRecord = true;
            }
        }

        private void LoadAdjustments(string sBIN, string sApplCode)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sApplType = "";

            pRec.Query = "select * from btm_update where bin = '" + sBIN + "'";
            if (sApplCode != "")
                pRec.Query += " and appl_type = '" + sApplCode + "'";
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sApplType = pRec.GetString("appl_type");

                    if (sApplType == "CBNS")
                    {
                        MappingFrm.chkApplyNewBussName.Checked = true;
                        MappingFrm.txtOfficialBussName.Text = StringUtilities.RemoveApostrophe(pRec.GetString("new_bns_name"));

                    }

                    if (sApplType == "TLOC")
                    {
                        LoadNewLocation(sBIN);
                    }

                    if (sApplType == "TOWN")
                    {
                        MappingFrm.chkApplyNewOwner.Checked = true;
                        string sOwnCode = pRec.GetString("new_own_code");
                        LoadOwner(sOwnCode, true);

                    }
                    if (sApplType == "CTYP")
                    {
                        MappingFrm.chkApplyNewBussType.Checked = true;
                    }

                    if (sApplType == "ADDL")
                    {
                        MappingFrm.chkOtherLine.Checked = true;
                    }

                    if (sApplType == "CBOW")
                    {
                        MappingFrm.chkChangeBussPlace.Checked = true;
                        string sOwnCode = pRec.GetString("new_own_code");
                        LoadOwner(sOwnCode, false);
                    }

                    if (sApplType == "CPMT")
                    {
                        MappingFrm.chkChangePermit.Checked = true;
                        string sPermit = pRec.GetString("permit_no");
                        MappingFrm.txtPermitNo.Text = sPermit.Substring(5, 5);
                    }

                    if (sApplType == "CTYR")
                    {
                        MappingFrm.chkChangeTaxYear.Checked = true;
                        string sPermit = pRec.GetString("permit_no");
                        MappingFrm.txtPermitYear.Text = sPermit.Substring(0, 4);
                    }

                    if (sApplType == "AREA")
                    {
                        MappingFrm.chkChangeArea.Checked = true;
                        string sArea = string.Format("{0:#,###.00}", Convert.ToDouble(pRec.GetString("new_bns_name")));
                        MappingFrm.txtArea.Text = sArea;
                    }

                    if (sApplType == "CSTR")
                    {
                        MappingFrm.chkChangeStoreys.Checked = true;
                        string sStorey = string.Format("{0:#,###}", Convert.ToInt32(pRec.GetString("new_bns_name")));
                        MappingFrm.txtStoreys.Text = sStorey;
                    }
                }
            }
            pRec.Close();



        }

        private void LoadAdjustmentsMain(string sBIN, bool bOfficialBns)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sPermitNo = "";

            if (bOfficialBns)
                pRec.Query = "select * from btm_businesses where bin = '" + sBIN + "'";
            else
                pRec.Query = "select * from btm_temp_businesses where tbin = '" + sBIN + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    MappingFrm.txtRemarks.Text = pRec.GetString("remarks").Trim();
                    MappingFrm.cmbPlace.Text = pRec.GetString("place_occupancy").Trim();
                    MappingFrm.txtRent.Text = string.Format("{0:#,###.00}", pRec.GetDouble("rent_lease_mo"));
                    sPermitNo = pRec.GetString("permit_no").Trim();
                    MappingFrm.cmbInspector.Text = pRec.GetString("inspected_by").Trim();   // RMC 20120419 added tagging of inspector in Business Mapping

                    if (sPermitNo != "" && sPermitNo != "-") // RMC 20120417 Corrections in validation of Permit for official business tagging in business mapping
                    {
                        MappingFrm.txtPermitYear.Text = sPermitNo.Substring(0, 4);
                        MappingFrm.txtPermitNo.Text = sPermitNo.Substring(5, 5);
                    }

                    MappingFrm.dtpPermitDate.Value = pRec.GetDateTime("permit_dt");
                    MappingFrm.txtArea.Text = string.Format("{0:#,###.00}", pRec.GetDouble("flr_area"));
                    MappingFrm.txtStoreys.Text = string.Format("{0:#,###}", pRec.GetInt("num_storeys"));
                }
            }
            pRec.Close();
        }

        private void LoadNewLocation(string sBIN)
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "select * from btm_transfer_table where bin = '" + sBIN + "' and trans_app_code = 'TL'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MappingFrm.chkApplyNewLoc.Checked = true;

                    MappingFrm.txtBussAdd.Text = pSet.GetString("addr_no");
                    MappingFrm.txtBussStreet.Text = pSet.GetString("addr_street");
                    MappingFrm.cmbBussBrgy.Text = pSet.GetString("addr_brgy");
                    MappingFrm.txtBussZone.Text = pSet.GetString("addr_zone");
                    MappingFrm.cmbBussDist.Text = pSet.GetString("addr_dist");
                    MappingFrm.txtBussCity.Text = pSet.GetString("addr_mun");
                    MappingFrm.txtBussProv.Text = pSet.GetString("addr_prov");

                }
            }
            pSet.Close();
        }

        public override void Save()
        {
            OracleResultSet pRec = new OracleResultSet();

            string sBIN = "";

            try
            {
                if (MappingFrm.bin1.txtTaxYear.Text != "" && MappingFrm.bin1.txtBINSeries.Text != "")
                    sBIN = MappingFrm.bin1.GetBin();

                if (!Validations(sBIN))
                {
                    return;
                }

                if (MessageBox.Show("Save record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pCmd = new OracleResultSet();
                    pCmd.Transaction = true;

                    SaveNewBusinessName();
                    SaveNewLocation();
                    SaveNewBnsType();
                    SaveNewOwner();
                    SaveOtherLine();

                    pCmd.Query = string.Format("delete from btm_businesses where bin = '{0}'", sBIN);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    DateTime dtPermitdate = Convert.ToDateTime(MappingFrm.m_sPermitDate);
                    double dLeaseAmt = 0;
                    Double.TryParse(MappingFrm.txtRent.Text.ToString(), out dLeaseAmt);
                    MappingFrm.m_sLeaseAmt = string.Format("{0:####}", dLeaseAmt);

                    string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                    string sPermit = MappingFrm.txtPermitYear.Text.Trim() + "-" + MappingFrm.txtPermitNo.Text.Trim();
                    string sPermitDate = string.Format("{0:MM/dd/yyyy}", MappingFrm.dtpPermitDate.Value);
                    double dArea = 0;
                    double.TryParse(MappingFrm.txtArea.Text.ToString(), out dArea);
                    int iStoreys = 0;
                    int.TryParse(MappingFrm.txtStoreys.Text.ToString(), out iStoreys);

                    pCmd.Query = "insert into btm_businesses values (";
                    pCmd.Query += "'" + sBIN + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()) + "', ";
                    pCmd.Query += "'" + MappingFrm.m_sBnsStat + "', ";
                    pCmd.Query += "'" + MappingFrm.txtOwnCode.Text.Trim() + "', '',";
                    pCmd.Query += "'" + MappingFrm.txtPermitYear.Text.Trim() + "', '',";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussAdd.Text.Trim()) + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussStreet.Text.Trim()) + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussCity.Text.Trim()) + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBussDist.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBussZone.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBussBrgy.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBussProv.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbOrgKind.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtPlaceOwnCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.m_sBnsCode + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbPlace.Text.Trim() + "', ";
                    pCmd.Query += " " + dLeaseAmt + ", ";
                    pCmd.Query += "'" + sPermit + "', "; ;
                    pCmd.Query += "to_date('" + sPermitDate + "', 'MM/dd/yyyy'), ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtRemarks.Text.Trim()) + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(AppSettingsManager.SystemUser.UserCode) + "', ";
                    pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'), ";
                    pCmd.Query += " " + dArea + ", ";
                    pCmd.Query += " " + iStoreys + ", ";
                    pCmd.Query += "'" + MappingFrm.cmbInspector.Text.Trim() + "')"; // RMC 20120419 added tagging of inspector in Business Mapping
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    SaveAdjustments();

                    SaveGISTable();

                    pCmd.Query = string.Format("delete from btm_gis_loc where bin = '{0}'", sBIN);
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "insert into btm_gis_loc values(";
                    pCmd.Query += "'" + sBIN + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBldgName.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbLandPin.Text.Trim() + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (!pCmd.Commit())
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    pCmd.Close();

                    string sData = "";
                    if (m_bUpdateRecord)
                        sData = "Updated " + sBIN;
                    else
                        sData = "Added " + sBIN;

                    if (AuditTrail.InsertTrail("ABM-M", "btm_businesses", sData) == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Record saved", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Add new record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (TaskMan.IsObjectLock(sBIN, "BUSINESS MAPPING", "DELETE", "ASS"))
                        { }

                        MappingFrm.bin1.txtTaxYear.Text = "";
                        MappingFrm.bin1.txtBINSeries.Text = "";
                        this.ClearControls(1);
                        this.ClearControls(2);
                        this.ClearControls(3);
                        this.ClearControls(4);
                        this.ClearOtherControls();
                        MappingFrm.btnSearch.Text = "Search Business";
                        MappingFrm.btnSave.Enabled = false;
                        MappingFrm.btnUntag.Enabled = false;
                        m_bUpdateRecord = false;
                        MappingFrm.btnOwnSearch.Text = "Search";
                        MappingFrm.btnPlaceOwnSearch.Text = "Search";
                    }
                    else
                        Close();
                }
            }
            catch
            {
                pCmd.Rollback();
                pCmd.Close();
            }
           
        }

        private void SaveGISTable()
        {
            OracleResultSet pGisRec = new OracleResultSet();

            try
            {
                pGisRec.CreateNewConnectionGIS();
                OracleResultSet pRec = new OracleResultSet();

                pGisRec.Query = "delete from gis_businesses where bin = '" + MappingFrm.bin1.GetBin() + "'";
                if (pGisRec.ExecuteNonQuery() == 0)
                { }

                pGisRec.Query = "delete from gis_bin where bin = '" + MappingFrm.bin1.GetBin() + "'";
                if (pGisRec.ExecuteNonQuery() == 0)
                { }

                int iID = 0;

                iID = GetGisID(pGisRec);    // RMC 20120207 modifications in business mapping

                if (iID == 0)   // RMC 20120207 modifications in business mapping
                {
                    pGisRec.Query = "insert into gis_businesses values (";
                    pGisRec.Query += "id_seq.nextval, ";
                    pGisRec.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                    pGisRec.Query += "'', '" + MappingFrm.cmbLandPin.Text.Trim() + "',"; // bldg pin,land pin
                    pGisRec.Query += "'0', '.', '.',";	//legend, code, hotlink
                    pGisRec.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()) + "', ";
                    pGisRec.Query += "'" + MappingFrm.m_sX + "', ";	//longitude
                    pGisRec.Query += "'" + MappingFrm.m_sY + "', ";	//latitude
                    pGisRec.Query += "'', ";
                    pGisRec.Query += "'" + MappingFrm.cmbBrgy.Text.Trim() + "', ";
                    pGisRec.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "')";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }
                }

                /*string sID = "";
                pGisRec.Query = "select * from gis_businesses ";
                pGisRec.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
                if (pGisRec.Execute())
                {
                    if (pGisRec.Read())
                    {
                        sID = string.Format("{0:####}", pGisRec.GetInt("id"));
                    }
                }
                pGisRec.Close();
                */
                // RMC 20120207 modifications in business mapping

                string sBrgyCode = "";

                sBrgyCode = AppSettingsManager.GetBrgyCode(MappingFrm.cmbBrgy.Text.Trim());

                pGisRec.Query = "insert into gis_bin values (";
                pGisRec.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                pGisRec.Query += "'', '" + MappingFrm.cmbLandPin.Text.Trim() + "',"; // bldg pin,land pin
                //pGisRec.Query += "'" + sID + "', ";   // RMC 20120207 modifications in business mapping
                pGisRec.Query += "" + iID + ",";    // RMC 20120207 modifications in business mapping
                pGisRec.Query += "'', ";	// district
                pGisRec.Query += "'" + sBrgyCode + "', '0', '.', ";
                pGisRec.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "', ";
                pGisRec.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()) + "')";
                if (pGisRec.ExecuteNonQuery() == 0)
                { }

                pGisRec.Query = "update gis_business_location set bldg_name = '" + StringUtilities.HandleApostrophe(MappingFrm.txtBldgName.Text.Trim()) + "'";
                pGisRec.Query += " where pin = '" + MappingFrm.cmbLandPin.Text + "' and bldg_code = '" + MappingFrm.cmbBldgCode.Text + "'";
                if (pGisRec.ExecuteNonQuery() == 0)
                { }

                // RMC 20120207 modifications in business mapping (s)
                pGisRec.Query = "select * from gis_business_map where bin = '" + MappingFrm.bin1.GetBin() + "'";
                if (pGisRec.Execute())
                {
                    if (pGisRec.Read())
                    {
                        pGisRec.Close();

                        pGisRec.Query = "update gis_business_map set mapped = '1'";
                        pGisRec.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
                        if (pGisRec.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pGisRec.Close();

                        pGisRec.Query = "insert into gis_business_map values (";
                        pGisRec.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pGisRec.Query += "'" + MappingFrm.cmbBrgy.Text.Trim() + "', '1')";
                        if (pGisRec.ExecuteNonQuery() == 0)
                        { }
                    }
                }
                // RMC 20120207 modifications in business mapping (E)
            }
            catch { }
        }

        private void SaveAdjustments()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sPrevOwnCode = "";
            string sNewOwnCode = "";
            string sAppCode = "";
            string sNewBnsName = "";
            string sApplType = "";
            string sOldBnsCode = "";
            string sNewBnsCode = "";

            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            pCmd.Query = string.Format("delete from btm_update where bin = '{0}'", MappingFrm.bin1.GetBin());
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            pRec.Query = string.Format("select * from btm_transfer_table where bin = '{0}'", MappingFrm.bin1.GetBin());
            if (pRec.Execute())
            {
                while (pRec.Read())
                {
                    sPrevOwnCode = pRec.GetString("prev_own_code");
                    sNewOwnCode = pRec.GetString("new_own_code");
                    sAppCode = pRec.GetString("trans_app_code");

                    if (sAppCode == "TO")
                        sApplType = "TOWN";
                    else if (sAppCode == "TL")
                        sApplType = "TLOC";
                    else if (sAppCode == "CB")
                        sApplType = "CBNS";
                    else if (sAppCode == "CT")
                        sApplType = "CTYP";
                    else if (sAppCode == "AD")
                        sApplType = "ADDL";

                    DateTime dtPermitDate = Convert.ToDateTime(MappingFrm.m_sPermitDate);


                    if (sApplType == "CBNS")
                    {
                        sNewBnsName = pRec.GetString("own_ln");

                        pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,new_bns_name,old_bns_name,bns_user,dt_save) values (";
                        pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pCmd.Query += "'" + sApplType + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                        pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(sNewBnsName) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.m_sBnsNm) + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }

                    if (sApplType == "TLOC")
                    {
                        pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_bns_loc,new_bns_loc,bns_user,dt_save) values (";
                        pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pCmd.Query += "'" + sApplType + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                        pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.m_sBnsLoc) + "', ";
                        pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.m_sNewLoc) + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                    }

                    if (sApplType == "TOWN")
                    {
                        pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_own_code,new_own_code,bns_user,dt_save) values (";
                        pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pCmd.Query += "'" + sApplType + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                        pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                        pCmd.Query += "'" + MappingFrm.m_sOwnCode + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sNewOwnCode + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }

                    if (sApplType == "CTYP")
                    {
                        sOldBnsCode = pRec.GetString("prev_own_code");
                        sNewBnsCode = pRec.GetString("bns_code");

                        pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_bns_code,new_bns_code,bns_user,dt_save) values (";
                        pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pCmd.Query += "'" + sApplType + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                        pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                        pCmd.Query += "'" + sOldBnsCode + "', ";
                        pCmd.Query += "'" + sNewBnsCode + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }

                    if (sApplType == "ADDL")
                    {
                        sNewBnsCode = pRec.GetString("bns_code");

                        pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,new_bns_code,bns_user,dt_save) values (";
                        pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                        pCmd.Query += "'" + sApplType + "', ";
                        pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                        pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                        pCmd.Query += "'" + sNewBnsCode + "', ";
                        pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                        pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }

                }
            }
            pRec.Close();

            if (MappingFrm.chkChangePermit.Checked || MappingFrm.chkChangeTaxYear.Checked)
            {
                string sPermit = MappingFrm.txtPermitYear.Text.Trim() + "-" + MappingFrm.txtPermitNo.Text.Trim();
                string sPermitDate = string.Format("{0:MM/dd/yyyy}", MappingFrm.dtpPermitDate.Value);

                if (MappingFrm.chkChangePermit.Checked)
                    sApplType = "CPMT";
                else if (MappingFrm.chkChangeTaxYear.Checked)
                    sApplType = "CTYR";

                pCmd.Query = "insert into btm_update (bin, appl_type, permit_no, permit_dt, bns_user, dt_save) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                pCmd.Query += "'" + sApplType + "', ";
                pCmd.Query += "'" + sPermit + "', ";
                pCmd.Query += "to_date('" + sPermitDate + "', 'MM/dd/yyyy'), ";
                pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            if (MappingFrm.chkChangeBussPlace.Checked)
            {
                sApplType = "CBOW";

                pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_own_code,new_own_code,bns_user,dt_save) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                pCmd.Query += "'" + sApplType + "', ";
                pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                pCmd.Query += "'" + MappingFrm.m_sBussOwnCode + "', ";
                pCmd.Query += "'" + MappingFrm.txtPlaceOwnCode.Text.Trim() + "', ";
                pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

            }

            if (MappingFrm.chkChangeArea.Checked)
            {
                sApplType = "AREA";

                pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_bns_name,new_bns_name,bns_user,dt_save) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                pCmd.Query += "'" + sApplType + "', ";
                pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                pCmd.Query += "'" + MappingFrm.m_sArea + "', ";
                pCmd.Query += "'" + MappingFrm.txtArea.Text.Trim() + "', ";
                pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            if (MappingFrm.chkChangeStoreys.Checked)
            {
                sApplType = "CSTR";

                pCmd.Query = "insert into btm_update(bin,appl_type,permit_no,permit_dt,old_bns_name,new_bns_name,bns_user,dt_save) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', ";
                pCmd.Query += "'" + sApplType + "', ";
                pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                pCmd.Query += "to_date('" + MappingFrm.m_sPermitDate + "', 'MM/dd/yyyy'), ";
                pCmd.Query += "'" + MappingFrm.m_sStoreys + "', ";
                pCmd.Query += "'" + MappingFrm.txtStoreys.Text.Trim() + "', ";
                pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }
        }

        public override void ChangeBussPlace(object sender, EventArgs e)
        {
            if (MappingFrm.chkChangeBussPlace.CheckState.ToString() == "Checked")
            {
                MappingFrm.cmbPlace.Enabled = true;

                LoadAdjustments(MappingFrm.bin1.GetBin(), "CBOW");
            }
            else
            {
                MappingFrm.cmbPlace.Enabled = false;
                EnableControls(3, false);

                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                {
                    MappingFrm.cmbPlace.Text = MappingFrm.m_sPlace;
                    LoadOwner(MappingFrm.m_sBussOwnCode, false);
                }
            }
        }

        public override void ApplyNewBussName(object sender, EventArgs e)
        {
            if (MappingFrm.chkApplyNewBussName.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtOfficialBussName.ReadOnly = false;
                MappingFrm.txtOfficialBussName.Focus();

                LoadAdjustments(MappingFrm.bin1.GetBin(), "CBNS");
            }
            else
            {
                MappingFrm.txtOfficialBussName.ReadOnly = true;
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                    MappingFrm.txtOfficialBussName.Text = MappingFrm.m_sBnsNm;
            }
        }

        public override void ApplyNewLoc(object sender, EventArgs e)
        {
            if (MappingFrm.chkApplyNewLoc.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtBussAdd.ReadOnly = false;
                MappingFrm.txtBussStreet.ReadOnly = false;
                MappingFrm.cmbBussBrgy.Enabled = true;
                MappingFrm.txtBussZone.ReadOnly = false;
                MappingFrm.cmbBussDist.Enabled = true;
                MappingFrm.txtBussProv.ReadOnly = false;
                MappingFrm.txtBussAdd.Focus();

                LoadAdjustments(MappingFrm.bin1.GetBin(), "TLOC");
            }
            else
            {
                MappingFrm.txtBussAdd.ReadOnly = true;
                MappingFrm.txtBussStreet.ReadOnly = true;
                MappingFrm.cmbBussBrgy.Enabled = false;
                MappingFrm.txtBussZone.ReadOnly = true;
                MappingFrm.cmbBussDist.Enabled = false;
                MappingFrm.txtBussProv.ReadOnly = true;

                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                {
                    MappingFrm.txtBussAdd.Text = MappingFrm.m_sAddNo;
                    MappingFrm.txtBussStreet.Text = MappingFrm.m_sStreet;
                    MappingFrm.cmbBussBrgy.Text = MappingFrm.m_sBrgy;
                    MappingFrm.txtBussZone.Text = MappingFrm.m_sZone;
                    MappingFrm.cmbBussDist.Text = MappingFrm.m_sDist;
                    MappingFrm.txtBussCity.Text = MappingFrm.m_sMun;
                    MappingFrm.txtBussProv.Text = MappingFrm.m_sProv;
                }
            }
        }

        public override void ApplyNewBussType(object sender, EventArgs e)
        {
            if (MappingFrm.chkApplyNewBussType.CheckState.ToString() == "Checked")
            {
                if (!MappingFrm.m_bInit)
                {
                    frmChangeType frmChangeType = new frmChangeType();
                    frmChangeType.BIN = MappingFrm.bin1.GetBin();
                    frmChangeType.TaxYear = MappingFrm.txtPermitYear.Text.Trim();
                    frmChangeType.Status = MappingFrm.m_sBnsStat;
                    frmChangeType.ShowDialog();
                    MappingFrm.m_sNewBnsCode = frmChangeType.NewBnsCode;
                    MappingFrm.txtBnsType.Text = AppSettingsManager.GetBnsDesc(MappingFrm.m_sNewBnsCode);

                    MappingFrm.chkApplyNewBussType.Checked = frmChangeType.WithChange;
                }
            }
            else
            {
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                {
                    if (MessageBox.Show("Delete application in change of business type?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    MappingFrm.txtBnsType.Text = AppSettingsManager.GetBnsDesc(MappingFrm.m_sBnsCode);
                }
            }
        }

        public override void ApplyNewOwner(object sender, EventArgs e)
        {
            if (MappingFrm.chkApplyNewOwner.CheckState.ToString() == "Checked")
            {
                MappingFrm.btnOwnSearch.Enabled = true;
                MappingFrm.txtOwnLn.ReadOnly = false;
                MappingFrm.txtOwnFn.ReadOnly = false;
                MappingFrm.txtOwnMi.ReadOnly = false;
                MappingFrm.txtOwnAddNo.ReadOnly = false;
                MappingFrm.txtOwnStreet.ReadOnly = false;
                MappingFrm.cmbOwnBrgy.Enabled = true;
                MappingFrm.cmbOwnDist.Enabled = true;
                MappingFrm.txtOwnMun.ReadOnly = false;
                MappingFrm.txtOwnProv.ReadOnly = false;
                MappingFrm.txtOwnZip.ReadOnly = false;
                MappingFrm.m_sNewOwnCode = "";
                MappingFrm.txtOwnLn.Focus();

                LoadAdjustments(MappingFrm.bin1.GetBin(), "TOWN");
            }
            else
            {
                MappingFrm.btnOwnSearch.Enabled = false;
                MappingFrm.txtOwnLn.ReadOnly = true;
                MappingFrm.txtOwnFn.ReadOnly = true;
                MappingFrm.txtOwnMi.ReadOnly = true;
                MappingFrm.txtOwnAddNo.ReadOnly = true;
                MappingFrm.txtOwnStreet.ReadOnly = true;
                MappingFrm.cmbOwnBrgy.Enabled = false;
                MappingFrm.cmbOwnDist.Enabled = false;
                MappingFrm.txtOwnMun.ReadOnly = true;
                MappingFrm.txtOwnProv.ReadOnly = true;
                MappingFrm.txtOwnZip.ReadOnly = true;

                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                    LoadOwner(MappingFrm.m_sOwnCode, true);

            }
        }

        private void SaveNewBusinessName()
        {
            pCmd.Query = "delete from btm_transfer_table ";
            pCmd.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
            pCmd.Query += " and trans_app_code = 'CB' ";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            if (MappingFrm.chkApplyNewBussName.Checked == true)
            {
                string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

                pCmd.Query = "insert into btm_transfer_table (bin, trans_app_code, own_ln, app_date) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', 'CB', ";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()) + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            //return true;
        }

        private void SaveNewLocation()
        {
            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            pCmd.Query = "delete from btm_transfer_table ";
            pCmd.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
            pCmd.Query += " and trans_app_code = 'TL' ";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            if (MappingFrm.chkApplyNewLoc.Checked == true)
            {
                string sHouseNo, sStreet, sMun, sBrgy, sZone, sDistrict, sProv, sBnsAddress;
                if (MappingFrm.txtBussAdd.Text.Trim() == "." || MappingFrm.txtBussAdd.Text.Trim() == "")
                    sHouseNo = "";
                else
                    sHouseNo = MappingFrm.txtBussAdd.Text.Trim() + " ";
                if (MappingFrm.txtBussStreet.Text.Trim() == "." || MappingFrm.txtBussStreet.Text.Trim() == "")
                    sStreet = "";
                else
                    sStreet = MappingFrm.txtBussStreet.Text.Trim() + ", ";
                if (MappingFrm.cmbBussBrgy.Text.Trim() == "." || MappingFrm.cmbBussBrgy.Text.Trim() == "")
                    sBrgy = "";
                else
                    sBrgy = MappingFrm.cmbBussBrgy.Text.Trim() + ", ";
                if (MappingFrm.txtBussProv.Text.Trim() == "." || MappingFrm.txtBussProv.Text.Trim() == "")
                    sProv = "";
                else
                    sProv = MappingFrm.txtBussProv.Text.Trim();
                if (MappingFrm.txtBussZone.Text.Trim() == "." || MappingFrm.txtBussZone.Text.Trim() == "ZONE" || MappingFrm.txtBussZone.Text.Trim() == "")
                    sZone = "";
                else
                    sZone = "ZONE " + MappingFrm.txtBussZone.Text.Trim() + " ";
                if (MappingFrm.cmbBussDist.Text.Trim() == "." || MappingFrm.cmbBussDist.Text.Trim() == "")
                    sDistrict = "";
                else
                    sDistrict = MappingFrm.cmbBussDist.Text.Trim() + ", ";
                if (MappingFrm.txtBussCity.Text.Trim() == "." || MappingFrm.txtBussCity.Text.Trim() == "")
                    sMun = "";
                else
                    sMun = MappingFrm.txtBussCity.Text.Trim();

                MappingFrm.m_sNewLoc = sHouseNo + sStreet + sBrgy + sZone + sDistrict + sMun;

                pCmd.Query = "insert into btm_transfer_table (bin, trans_app_code, addr_no, addr_street, addr_brgy, addr_zone, addr_dist, addr_mun, addr_prov, app_date) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', 'TL',";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussAdd.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussStreet.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussBrgy.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtBussZone.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussDist.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtBussCity.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtBussProv.Text.Trim()) + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            //return true;
        }

        private void SaveNewBnsType()
        {
            if (MappingFrm.chkApplyNewBussType.Checked == false)
            {
                pCmd.Query = string.Format("delete from BTM_CHANGE_CLASS_TBL where bin = '{0}'", MappingFrm.bin1.GetBin());
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            //return true;
        }

        private void SaveOtherLine()
        {
            if (MappingFrm.chkOtherLine.Checked == false)
            {
                pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}' and tax_year = '{1}'", MappingFrm.bin1.GetBin(), MappingFrm.txtPermitYear.Text.Trim());
                if (pCmd.ExecuteNonQuery() == 0)
                { }
            }

            //return true;
        }

        private void SaveNewOwner()
        {
            string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());

            pCmd.Query = "delete from btm_transfer_table ";
            pCmd.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
            pCmd.Query += " and trans_app_code = 'TO' ";
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            if (MappingFrm.chkApplyNewOwner.Checked == true)
            {
                pCmd.Query = "insert into btm_transfer_table (bin, trans_app_code,prev_own_code, new_own_code, own_ln, own_fn, own_mi, addr_no,addr_street, addr_brgy, addr_dist, addr_mun, addr_prov, app_date) values (";
                pCmd.Query += "'" + MappingFrm.bin1.GetBin() + "', 'TO', ";
                pCmd.Query += "'" + MappingFrm.m_sOwnCode + "', ";
                pCmd.Query += "'" + MappingFrm.m_sNewOwnCode + "', ";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOwnLn.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtOwnFn.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtOwnMi.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOwnAddNo.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtOwnStreet.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.cmbOwnBrgy.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.cmbOwnDist.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtOwnMun.Text.Trim()) + "', ";
                pCmd.Query += "'" + StringUtilities.SetEmptyToSpace(MappingFrm.txtOwnProv.Text.Trim()) + "', ";
                pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'))";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

            }

            //return true;
        }

        public override void OtherLine(object sender, EventArgs e)
        {
            if (MappingFrm.chkOtherLine.CheckState.ToString() == "Checked")
            {
                if (!MappingFrm.m_bInit)
                {
                    frmAddlBusiness frmAddlBusiness = new frmAddlBusiness();
                    frmAddlBusiness.BIN = MappingFrm.bin1.GetBin();
                    frmAddlBusiness.TaxYear = MappingFrm.txtPermitYear.Text.Trim();
                    frmAddlBusiness.ShowDialog();
                    LoadOtherLine();
                    
                }
            }
        }

        private bool ValidateBINIfMatched(string sBIN)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();
            }
            catch { return false; }

            m_bUpdateRecord = false;

            pRec.Query = "select * from btm_businesses";
            pRec.Query += string.Format(" where bin = '{0}'", sBIN);
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    if (MessageBox.Show("BIN already tagged.\nUpdate record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        pRec.Close();
                        if (TaskMan.IsObjectLock(sBIN, "BUSINESS MAPPING", "DELETE", "ASS"))
                        { }

                        ClearControls(1);
                        ClearControls(2);
                        ClearControls(3);
                        ClearControls(4);
                        
                        return false;
                    }
                    else
                    {
                        //pGisRec.Query = "select * from gis_businesses where bin = '" + sBIN + "'";
                        pGisRec.Query = "select * from gis_bin where bin = '" + sBIN + "'";  // RMC 20120330 Modified validation of Business mapping tagging in GIS table
                        if (pGisRec.Execute())
                        {
                            if (pGisRec.Read())
                            {
                                //MappingFrm.cmbBrgy.Text = pGisRec.GetString("barangay").Trim();
                                MappingFrm.cmbBrgy.Text = AppSettingsManager.GetBrgyName(pGisRec.GetString("brgy").Trim()); // RMC 20120330 Modified validation of Business mapping tagging in GIS table

                                MappingFrm.m_sGisBrgy = MappingFrm.cmbBrgy.Text.Trim();
                                MappingFrm.LoadPin(AppSettingsManager.GetBrgyCode(MappingFrm.cmbBrgy.Text));
                                MappingFrm.cmbLandPin.Text = pGisRec.GetString("land_pin").Trim();

                                MappingFrm.LoadBldgCode(MappingFrm.cmbLandPin.Text);
                                MappingFrm.cmbBldgCode.Text = pGisRec.GetString("bldgcode");

                            }
                        }
                        pGisRec.Close();
                        pRec.Close();
                         
                        

                        MappingFrm.btnUntag.Enabled = true;
                        m_bUpdateRecord = true;
                        return true;
                    }
                }
                // GDE 20121003
                else
                {
                    pRec.Close();
                    pRec.Query = "select * from btm_temp_businesses";
                    pRec.Query += string.Format(" where bin = '{0}'", sBIN);
                    if (pRec.Execute())
                    {
                        if (pRec.Read())
                        {
                            if (MessageBox.Show("BIN already tagged.\nUpdate record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                pRec.Close();
                                if (TaskMan.IsObjectLock(sBIN, "BUSINESS MAPPING", "DELETE", "ASS"))
                                { }

                                ClearControls(1);
                                ClearControls(2);
                                ClearControls(3);
                                ClearControls(4);

                                return false;
                            }
                            else
                            {
                                //pGisRec.Query = "select * from gis_businesses where bin = '" + sBIN + "'";
                                pGisRec.Query = "select * from gis_bin where bin = '" + sBIN + "'";  // RMC 20120330 Modified validation of Business mapping tagging in GIS table
                                if (pGisRec.Execute())
                                {
                                    if (pGisRec.Read())
                                    {
                                        //MappingFrm.cmbBrgy.Text = pGisRec.GetString("barangay").Trim();
                                        MappingFrm.cmbBrgy.Text = AppSettingsManager.GetBrgyName(pGisRec.GetString("brgy").Trim()); // RMC 20120330 Modified validation of Business mapping tagging in GIS table

                                        MappingFrm.m_sGisBrgy = MappingFrm.cmbBrgy.Text.Trim();
                                        MappingFrm.LoadPin(AppSettingsManager.GetBrgyCode(MappingFrm.cmbBrgy.Text));
                                        MappingFrm.cmbLandPin.Text = pGisRec.GetString("land_pin").Trim();

                                        MappingFrm.LoadBldgCode(MappingFrm.cmbLandPin.Text);
                                        MappingFrm.cmbBldgCode.Text = pGisRec.GetString("bldgcode");

                                    }
                                }
                                pGisRec.Close();
                                pRec.Close();



                                MappingFrm.btnUntag.Enabled = true;
                                m_bUpdateRecord = true;
                                return true;
                            }
                        }
                    }
                }
                // GDE 20121003
            }
            pRec.Close();

            return true;
        }

        public override void ValidateBussName()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";

            pRec.Query = "select * from businesses";
            pRec.Query += string.Format(" where bns_nm = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()));
            pRec.Query += string.Format(" and bin <> '{0}'", MappingFrm.bin1.GetBin());
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    MessageBox.Show("Warning: Double business name", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    using (frmDouble_Buss frmDoubleBuss = new frmDouble_Buss())
                    {
                        pRec.Close();

                        frmDoubleBuss.txtBussName.Text = MappingFrm.txtOfficialBussName.Text.Trim();
                        frmDoubleBuss.ShowDialog();
                        MappingFrm.txtPlate.Text = frmDoubleBuss.txtBussPlate.Text.Trim();

                        if (MappingFrm.txtPlate.Text != "")
                        {
                            pRec.Query = string.Format("select * from buss_plate where bns_plate = '{0}'", MappingFrm.txtPlate.Text.Trim());
                            if (pRec.Execute())
                            {
                                if (pRec.Read())
                                {
                                    sBIN = pRec.GetString("bin");

                                    pRec.Close();
                                    if (!TaskMan.IsObjectLock(sBIN, "BUSINESS MAPPING", "ADD", "ASS"))
                                    {
                                        if (ValidateBINIfMatched(sBIN))
                                            LoadValues(sBIN);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (frmDoubleBuss.txtBussAdd.Text.Trim() != "")
                            {
                                MappingFrm.txtBussAdd.Text = frmDoubleBuss.m_sAdd;
                                MappingFrm.txtBussStreet.Text = frmDoubleBuss.m_sStreet;
                                MappingFrm.cmbBussBrgy.Text = frmDoubleBuss.m_sBrgy;
                            }
                        }
                    }
                }

            }
            pRec.Close();
        }

        public override void ChangeArea(object sender, EventArgs e)
        {
            if (MappingFrm.chkChangeArea.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtArea.ReadOnly = false;

                LoadAdjustments(MappingFrm.bin1.GetBin(), "AREA");
            }
            else
            {
                MappingFrm.txtArea.ReadOnly = true;
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                    MappingFrm.txtArea.Text = MappingFrm.m_sArea;
            }
        }

        public override void ChangeStoreys(object sender, EventArgs e)
        {
            if (MappingFrm.chkChangeStoreys.CheckState.ToString() == "Checked")
            {
                MappingFrm.txtStoreys.ReadOnly = false;
                LoadAdjustments(MappingFrm.bin1.GetBin(), "CSTR");
            }
            else
            {
                MappingFrm.txtStoreys.ReadOnly = true;
                if (MappingFrm.bin1.txtTaxYear.Text.Trim() != "" && MappingFrm.bin1.txtBINSeries.Text.Trim() != "")
                    MappingFrm.txtStoreys.Text = MappingFrm.m_sStoreys;
            }
        }

        private bool Validations(string sBIN)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (MappingFrm.txtBldgName.Text.Trim() == "")
            {
                MessageBox.Show("Building name required.","Business Mapping",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtOwnLn.Text.Trim() == "")
            {
                MessageBox.Show("Owner's name required", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            // RMC 20120419 added tagging of inspector in Business Mapping (s)
            if (MappingFrm.cmbInspector.Text.Trim() == "")
            {
                MessageBox.Show("Inspector's name required", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            // RMC 20120419 added tagging of inspector in Business Mapping (e)

            MappingFrm.txtOwnCode.Text = AppSettingsManager.EnlistOwner(MappingFrm.txtOwnLn.Text.Trim(), MappingFrm.txtOwnFn.Text.Trim(), MappingFrm.txtOwnMi.Text.Trim(), MappingFrm.txtOwnAddNo.Text.Trim(), MappingFrm.txtOwnStreet.Text.Trim(), MappingFrm.cmbOwnDist.Text.Trim().ToUpper(), "", MappingFrm.cmbOwnBrgy.Text.Trim().ToUpper(), MappingFrm.txtOwnMun.Text.Trim(), MappingFrm.txtOwnProv.Text.Trim(), MappingFrm.txtOwnZip.Text.Trim());
            MappingFrm.m_sNewOwnCode = MappingFrm.txtOwnCode.Text;

            if (MappingFrm.m_sNewOwnCode == "")
                return false;

            if (MappingFrm.cmbPlace.Text.Trim() == "")
            {
                MessageBox.Show("Business place type required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtPlaceOwnCode.Text.Trim() == "" && MappingFrm.cmbPlace.Text.Trim() == "OWNED")
            {
                MappingFrm.txtPlaceOwnCode.Text = MappingFrm.txtOwnCode.Text.Trim();
                LoadOwner(MappingFrm.txtPlaceOwnCode.Text, false);
            }
            if (MappingFrm.txtPlaceOwnLn.Text.Trim() == "" && MappingFrm.cmbPlace.Text.Trim() == "RENTED")
            {
                MessageBox.Show("Business place owner's name required", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            MappingFrm.txtPlaceOwnCode.Text = AppSettingsManager.EnlistOwner(MappingFrm.txtPlaceOwnLn.Text.Trim(), MappingFrm.txtPlaceOwnFn.Text.Trim(), MappingFrm.txtPlaceOwnMi.Text.Trim(), MappingFrm.txtPlaceOwnAdd.Text.Trim(), MappingFrm.txtPlaceOwnStreet.Text.Trim(), MappingFrm.cmbPlaceOwnDist.Text.Trim().ToUpper(), "", MappingFrm.cmbPlaceOwnBrgy.Text.Trim().ToUpper(), MappingFrm.txtPlaceOwnMun.Text.Trim(), MappingFrm.txtPlaceOwnProv.Text.Trim(), MappingFrm.txtPlaceOwnZip.Text.Trim());


            if (MappingFrm.cmbBrgy.Text.Trim() == "")
            {
                MessageBox.Show("Select GIS mapped barangay first.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if ((MappingFrm.m_sX == "0" || MappingFrm.m_sX == "") && (MappingFrm.m_sY == "0" || MappingFrm.m_sY == ""))
            {
                MessageBox.Show("Location not yet mapped.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.cmbBrgy.Text.Trim() != MappingFrm.cmbBussBrgy.Text.Trim())
            {
                MessageBox.Show("GIS mapped barangay does not match business barangay location.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.cmbBldgCode.Text.Trim() == "")
            {
                MessageBox.Show("Select GIS mapped bldg code first.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtOfficialBussName.Text.Trim() == "")
            {
                MessageBox.Show("Business name required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtBussAdd.Text == "")
            {
                MessageBox.Show("Business address required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtOwnCode.Text.Trim() == "")
            {
                MessageBox.Show("Owner's name required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.txtBnsType.Text.Trim() == "")
            {
                MessageBox.Show("Business type required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (MappingFrm.cmbOrgKind.Text.Trim() == "")
            {
                MessageBox.Show("Organization kind required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            

            pRec.Query = "select * from btm_businesses";
            pRec.Query += string.Format(" where trim(bns_nm) = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()));
            pRec.Query += string.Format(" and bns_house_no = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtBussAdd.Text.Trim()));
            pRec.Query += string.Format(" and bns_street = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtBussStreet.Text.Trim()));
            pRec.Query += string.Format(" and bns_brgy = '{0}'", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussBrgy.Text.Trim())));
            pRec.Query += string.Format(" and bin <> '{0}'", sBIN);
            
            if (pRec.Execute())
            {
                string sTmpBIN = "";
                if (pRec.Read())
                {
                    sTmpBIN = pRec.GetString("bin");

                    if (MessageBox.Show("Business name already added with BIN: " + sTmpBIN + " .\nContinue?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        pRec.Close();
                        return false;
                    }

                    /*MessageBox.Show("Business name already added.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pRec.Close();
                    return false;*/
                }
            }
            pRec.Close();

            if (MappingFrm.chkApplyNewBussName.Checked == true)
            {
                if (StringUtilities.HandleApostrophe(MappingFrm.m_sBnsNm) == StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()))
                {
                    MessageBox.Show("No changes made in Business Name.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtOfficialBussName.Focus();
                    return false;

                }

                if (StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()) == "")
                {
                    MessageBox.Show("Specify new business name.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

            }

            if (MappingFrm.chkApplyNewLoc.Checked == true)
            {
                if (MappingFrm.m_sAddNo == MappingFrm.txtBussAdd.Text.Trim()
                    && MappingFrm.m_sStreet == MappingFrm.txtBussStreet.Text.Trim()
                    && MappingFrm.m_sBrgy == StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussBrgy.Text.Trim())
                    && MappingFrm.m_sDist == StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussDist.Text.Trim())
                    && MappingFrm.m_sZone == MappingFrm.txtBussZone.Text.Trim()
                    && MappingFrm.m_sMun == MappingFrm.txtBussCity.Text.Trim())
                {
                    MessageBox.Show("No changes made in Business Location.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtBussAdd.Focus();
                    return false;
                }

                if (MappingFrm.txtBussAdd.Text.Trim() == "" || MappingFrm.txtBussStreet.Text.Trim() == ""
                    || MappingFrm.cmbBussBrgy.Text.Trim() == "" || MappingFrm.txtBussCity.Text.Trim() == "")
                {
                    MessageBox.Show("Fill-up all necessary fields.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtBussAdd.Focus();
                    return false;
                }
            }

            if (MappingFrm.chkApplyNewOwner.Checked == true)
            {
                if (MappingFrm.m_sOwnCode == MappingFrm.m_sNewOwnCode)
                {
                    MessageBox.Show("No changes made in Owner.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtOwnLn.Focus();
                    return false;
                }

                if (MappingFrm.txtOwnLn.Text.Trim() == "")
                {
                    MessageBox.Show("Owner's last name required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtOwnLn.Focus();
                    return false;
                }
            }

            if (MappingFrm.chkChangePermit.Checked == true)
            {
                string sNewPermit = MappingFrm.txtPermitYear.Text.Trim() + "-" + MappingFrm.txtPermitNo.Text.Trim();
                bool bChanged = false;

                if (MappingFrm.m_sPermit != sNewPermit)
                {
                    bChanged = true;
                }

                if (MappingFrm.dtpPermitDate.Value != DateTime.Parse(MappingFrm.m_sPermitDate))
                {
                    bChanged = true;
                }

                if (!bChanged)
                {
                    MessageBox.Show("No changes made in Permit.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.txtPermitYear.Focus();
                    return false;
                }
            }

            if (MappingFrm.chkChangeBussPlace.Checked)
            {
                if (MappingFrm.m_sBussOwnCode == MappingFrm.txtPlaceOwnCode.Text.Trim())
                {
                    MessageBox.Show("No changes made in Business Place ownership", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (MappingFrm.chkChangeArea.Checked)
            {
                string sOldArea = string.Format("{0:###.00}", Convert.ToDouble(MappingFrm.m_sArea));
                string sNewArea = string.Format("{0:###.00}", Convert.ToDouble(MappingFrm.txtArea.Text.ToString()));

                if (sOldArea == sNewArea)
                {
                    MessageBox.Show("No changes made in Area", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (MappingFrm.chkChangeStoreys.Checked)
            {
                string sOldStoreys = string.Format("{0:###}", MappingFrm.m_sStoreys);
                string sNewStoreys = string.Format("{0:###}", MappingFrm.txtStoreys.Text.ToString());

                if (sOldStoreys == sNewStoreys)
                {
                    MessageBox.Show("No changes made in No. of Storeys", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            string sDate = string.Format("{0:yyyy}", MappingFrm.dtpPermitDate.Value);

            if (MappingFrm.chkChangeTaxYear.Checked == true) // RMC 20120417 Corrections in validation of Permit for official business tagging in business mapping
            {
                if (MappingFrm.txtPermitYear.Text.Trim() != sDate)
                {
                    MessageBox.Show("Conflict in permit year and permit date", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            return true;
        }

        public override void Untag()
        {
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();
            }
            catch { }
            if (MessageBox.Show("Un-tag record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd = new OracleResultSet();
                pCmd.Transaction = true;

                pCmd.Query = "delete from btm_transfer_table ";
                pCmd.Query += " where bin = '" + MappingFrm.bin1.GetBin() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from BTM_CHANGE_CLASS_TBL where bin = '{0}'", MappingFrm.bin1.GetBin());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}' and tax_year = '{1}'", MappingFrm.bin1.GetBin(), MappingFrm.txtPermitYear.Text.Trim());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from btm_businesses where bin = '{0}'", MappingFrm.bin1.GetBin());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from btm_gis_loc where bin = '{0}'", MappingFrm.bin1.GetBin());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from btm_update where bin = '{0}'", MappingFrm.bin1.GetBin());
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                if (!pCmd.Commit())
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    pGisRec.Query = "delete from gis_businesses where bin = '" + MappingFrm.bin1.GetBin() + "'";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }

                    pGisRec.Query = "delete from gis_bin where bin = '" + MappingFrm.bin1.GetBin() + "'";
                    if (pGisRec.ExecuteNonQuery() == 0)
                    { }
                }
                catch { }

                if (AuditTrail.InsertTrail("ABM-M-U", "btm_businesses", MappingFrm.bin1.GetBin()) == 0)
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record un-tagged", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (MessageBox.Show("Add new record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (TaskMan.IsObjectLock(MappingFrm.bin1.GetBin(), "BUSINESS MAPPING", "DELETE", "ASS"))
                    { }

                    MappingFrm.bin1.txtTaxYear.Text = "";
                    MappingFrm.bin1.txtBINSeries.Text = "";
                    this.ClearControls(1);
                    this.ClearControls(2);
                    this.ClearControls(3);
                    this.ClearControls(4);
                    this.ClearOtherControls();
                    MappingFrm.btnSearch.Text = "Search Business";
                    MappingFrm.btnSave.Enabled = false;
                    MappingFrm.btnUntag.Enabled = false;

                }
                else
                    Close();
            }
        }

        public override void PlaceSelectedValueChanged()
        {
            if (MappingFrm.cmbPlace.Text == "RENTED")
            {
                if (MappingFrm.chkChangeBussPlace.Checked == true)
                    EnableControls(3, true);
            }
            else
            {
                EnableControls(3, false);
                LoadOwner(MappingFrm.txtOwnCode.Text, false);
                //MappingFrm.chkChangeBussPlace.Checked = false;
            }
        }

        private void LoadOtherLine()
        {
            OracleResultSet result = new OracleResultSet();
            int iCount = 0;
            string sBIN = "";

            sBIN = MappingFrm.bin1.GetBin();

            
            result.Query = "select count(*) from btm_addl_bns_que where bin = '" + sBIN + "'";
            int.TryParse(result.ExecuteScalar(), out iCount);

            if (iCount > 0)
                MappingFrm.chkOtherLine.Checked = true;
            else
                MappingFrm.chkOtherLine.Checked = false;
        }

        public override void ValidatePermitNo()
        {
            if(!ValidatePermit(MappingFrm.bin1.GetBin(),""))
            {
                MappingFrm.txtPermitNo.Text = "";
                return;
            }
        }

        public override void ValidatePermitYear()
        {
            if (!ValidatePermit(MappingFrm.bin1.GetBin(),""))
            {
                MappingFrm.txtPermitYear.Text = "";
                return;
            }
        }

        private int GetGisID(OracleResultSet pGisRec2)
        {
            // RMC 20120207 modifications in business mapping
            int iID = 0;

            pGisRec2.Query = "select * from gis_businesses where land_pin = '" + MappingFrm.cmbLandPin.Text.Trim() + "'";
            pGisRec2.Query += " and bldgcode = '" + MappingFrm.cmbBldgCode.Text.Trim() + "'";
            if (pGisRec2.Execute())
            {
                if (pGisRec2.Read())
                {
                    iID = pGisRec2.GetInt("id");

                    pGisRec2.Close();

                    pGisRec2.Query = "update gis_businesses set bin = '', legend = '1'";
                    pGisRec2.Query += " where land_pin = '" + MappingFrm.cmbLandPin.Text.Trim() + "'";
                    pGisRec2.Query += " and bldgcode = '" + MappingFrm.cmbBldgCode.Text.Trim() + "'";
                    if (pGisRec2.ExecuteNonQuery() == 0)
                    { }
                }
                else
                    pGisRec2.Close();
            }
            
            return iID;
        }
    }
}
