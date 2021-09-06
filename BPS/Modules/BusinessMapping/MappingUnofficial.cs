
// RMC 20120420 corrected validation of permit if undeclared
// RMC 20120419 added tagging of inspector in Business Mapping

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;


namespace Amellar.Modules.BusinessMapping
{
    public class MappingUnofficial:Mapping
    {
        private bool m_bNotEncoded = true;
        private string m_strTempBIN = "";
        private bool m_bUpdateRecord = false;
        private string m_sUpdateBIN = "";

        public MappingUnofficial(frmMapping Form)
            : base(Form)
        {
        }

        public override void FormLoad()
        {
            MappingFrm.bin1.Visible = false;
            MappingFrm.txtBIN.Visible = true;
            //MappingFrm.btnSearch.Visible = false;

            this.EnableControls(1, true);
            this.EnableControls(2, true);
            this.EnableControls(3, true);
            this.EnableControls(4, true);

            MappingFrm.grpEdit.Text = " Tag as: ";
            MappingFrm.chkUndeclared.Visible = true;
            MappingFrm.chkNotEncoded.Visible = true;
            MappingFrm.chkNotEncoded.Checked = true;
            m_bNotEncoded = true;

            MappingFrm.chkApplyNewBussName.Visible = false;
            MappingFrm.chkApplyNewBussType.Visible = false;
            MappingFrm.chkApplyNewLoc.Visible = false;
            MappingFrm.chkApplyNewOwner.Visible = false;
            MappingFrm.chkOtherLine.Visible = false;
            MappingFrm.chkChangePermit.Visible = false;
            MappingFrm.chkChangeBussPlace.Visible = false;
            MappingFrm.chkChangeArea.Visible = false;
            MappingFrm.chkChangeStoreys.Visible = false;
            MappingFrm.chkChangeTaxYear.Visible = false;
            MappingFrm.btnOwnSearch.Enabled = true;
            MappingFrm.cmbPlace.Enabled = true;

            MappingFrm.chkOtherLine.Visible = true;
            MappingFrm.chkOtherLine.Location = new System.Drawing.Point(31, 95);
            MappingFrm.chkOtherLine.Text = "Add Other line/s of Bussines";

            MappingFrm.btnSave.Enabled = true;

            m_strTempBIN = "";

            MappingFrm.m_bInit = false;
            MappingFrm.btnUntag.Enabled = false;
            m_bUpdateRecord = false;
            m_sUpdateBIN = "";

            LoadInspector();
        }

        public override void Save()
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();

            bool bValidateTempBin = true;
            string sBIN = "";

            try
            {

                 pCmd = new OracleResultSet();
                 pCmd.Transaction = true; 

                if (MappingFrm.chkNotEncoded.Checked)
                {
                    sBIN = MappingFrm.txtBIN.Text;

                    if (sBIN.Trim() == "")
                    {
                        MessageBox.Show("BIN for Not Encoded record required", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        pCmd.Rollback();
                        pCmd.Close();
                        return;

                    }
                }
                else
                {
                    if (!m_bUpdateRecord)
                        MappingFrm.txtBIN.Text = "";

                    sBIN = MappingFrm.txtBIN.Text.Trim();

                    if (sBIN == "")
                    {
                        sBIN = GenerateTempBIN();

                        while (bValidateTempBin)
                        {
                            int iCnt = 0;
                            pRec.Query = "select count(*) from btm_temp_businesses where tbin = '" + sBIN + "'";
                            int.TryParse(pRec.ExecuteScalar(), out iCnt);

                            /*
                            pCmd.Query = "update tmp_bin_serial set bin_serial = '" + sBIN.Substring(5, 7) + "'";
                            pCmd.Query += " where bin_year = '" + ConfigurationAttributes.CurrentYear + "'";
                            if (pCmd.ExecuteNonQuery() == 0)
                            { }
                             */

                            result.Query = "update tmp_bin_serial set bin_serial = '" + sBIN.Substring(5, 7) + "'";
                            result.Query += " where bin_year = '" + ConfigurationAttributes.CurrentYear + "'";
                            if (result.ExecuteNonQuery() == 0)
                            { }

                            if (iCnt == 0)
                            {
                                bValidateTempBin = false;
                            }
                            else
                            {
                                sBIN = GenerateTempBIN();
                            }
                        }   
                    }
                }

                if (!Validations(sBIN))
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    return;
                }

                if (MessageBox.Show("Save record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pCmd.Query = "delete from btm_temp_businesses where tbin = '" + sBIN + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "delete from btm_gis_loc where bin = '" + sBIN + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "delete from btm_temp_businesses where tbin = '" + m_sUpdateBIN + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "delete from btm_gis_loc where bin = '" + m_sUpdateBIN + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    string sDate = string.Format("{0:MM/dd/yyyy}", AppSettingsManager.GetCurrentDate());
                    if (m_bNotEncoded)
                        MappingFrm.m_sPermit = MappingFrm.txtPermitYear.Text.Trim() + "-" + MappingFrm.txtPermitNo.Text.Trim();
                    else
                        MappingFrm.m_sPermit = "";

                    double dArea = 0;
                    double.TryParse(MappingFrm.txtArea.Text.ToString(), out dArea);
                    int iStoreys = 0;
                    int.TryParse(MappingFrm.txtStoreys.Text.ToString(), out iStoreys);

                    pCmd.Query = "insert into btm_temp_businesses values (";
                    pCmd.Query += "'" + sBIN + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.ToString().Trim()) + "', ";
                    pCmd.Query += "'" + MappingFrm.m_sBnsStat + "', ";
                    pCmd.Query += "'" + MappingFrm.txtOwnCode.Text.Trim() + "', ";
                    if (m_bNotEncoded)
                        pCmd.Query += "'" + sBIN + "', ";
                    else
                        pCmd.Query += "'', ";
                    pCmd.Query += "'" + MappingFrm.txtPermitYear.Text.Trim() + "', ";
                    pCmd.Query += "'', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussAdd.Text.Trim()) + "', ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtBussStreet.Text.Trim()) + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBussCity.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBussDist.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBussZone.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBussBrgy.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBussProv.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbOrgKind.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtPlaceOwnCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.m_sBnsCode + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbPlace.Text.Trim() + "', ";
                    pCmd.Query += "'0', ";
                    pCmd.Query += "'" + MappingFrm.m_sPermit + "', ";
                    pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'), ";
                    pCmd.Query += "'" + StringUtilities.HandleApostrophe(MappingFrm.txtRemarks.Text.Trim()) + "', ";
                    pCmd.Query += "'" + AppSettingsManager.SystemUser.UserCode + "',";
                    pCmd.Query += "to_date('" + sDate + "', 'MM/dd/yyyy'), ";
                    pCmd.Query += " " + dArea + ", ";
                    pCmd.Query += " " + iStoreys + ", ' ', ";
                    pCmd.Query += "'" + MappingFrm.cmbInspector.Text.Trim() + "')"; // RMC 20120419 added tagging of inspector in Business Mapping
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    pCmd.Query = "insert into btm_gis_loc values(";
                    pCmd.Query += "'" + sBIN + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbBldgCode.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.txtBldgName.Text.Trim() + "', ";
                    pCmd.Query += "'" + MappingFrm.cmbLandPin.Text.Trim() + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    if (MappingFrm.chkOtherLine.Checked == true)
                    {
                        pCmd.Query = "update btm_addl_bns_que set bin = '" + sBIN + "'";
                        pCmd.Query += " where bin = '" + m_strTempBIN + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = "update btm_transfer_table set bin = '" + sBIN + "'";
                        pCmd.Query += " where bin = '" + m_strTempBIN + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}'", m_strTempBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}'", sBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}'", m_sUpdateBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from btm_transfer_table where bin = '{0}'", m_strTempBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from btm_transfer_table where bin = '{0}'", sBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        pCmd.Query = string.Format("delete from btm_transfer_table where bin = '{0}'", m_sUpdateBIN);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }

                        
                    }

                    SaveGISTable();   

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

                    if (AuditTrail.InsertTrail("ABM-U", "btm_temp_businesses", sData) == 0)
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Temporary BIN: " + sBIN + " saved", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MappingFrm.txtBIN.Text = sBIN;

                    if (MessageBox.Show("Add new record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (TaskMan.IsObjectLock(sBIN, "BUSINESS MAPPING", "DELETE", "ASS"))
                        { }

                        ClearControls(1);
                        ClearControls(2);
                        ClearControls(3);
                        MappingFrm.cmbInspector.Text = "";  // RMC 20120419 added tagging of inspector in Business Mapping
                        MappingFrm.txtBIN.Text = "";
                        MappingFrm.cmbPlace.Text = "";
                        MappingFrm.cmbPlace.Enabled = true;
                        MappingFrm.btnOwnSearch.Enabled = true;
                        MappingFrm.chkNotEncoded.Checked = true;
                        m_bNotEncoded = true;
                        MappingFrm.m_bInit = false;
                        MappingFrm.btnSearch.Text = "Search Bin";
                        m_strTempBIN = "";
                        MappingFrm.btnUntag.Enabled = false;

                        this.EnableControls(1, true);
                        this.EnableControls(2, true);
                        this.EnableControls(3, true);
                        this.EnableControls(4, true);
                        m_bUpdateRecord = false;
                        MappingFrm.btnOwnSearch.Text = "Search";
                        MappingFrm.btnPlaceOwnSearch.Text = "Search";
                        m_sUpdateBIN = "";
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

        private string GenerateTempBIN()
        {
            OracleResultSet pRec = new OracleResultSet();
            
            int iBINSerial = 0;
            string sBINSerial = "0";
            string sTmpBIN = "";

            pRec.Query = "select * from tmp_bin_serial where bin_year = '" + ConfigurationAttributes.CurrentYear + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    iBINSerial = pRec.GetInt("BIN_SERIAL") + 1;
                }
                else
                {
                    pCmd.Query = "insert into tmp_bin_serial values (";
                    pCmd.Query += "'" + ConfigurationAttributes.CurrentYear + "','" + sBINSerial + "')";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }

                    iBINSerial += 1;
                }
            }
            pRec.Close();

            sBINSerial = string.Format("{0:000000#}", iBINSerial);

            sTmpBIN = ConfigurationAttributes.CurrentYear + "-" + sBINSerial;
            return sTmpBIN;
        }

        public override void NotEncoded(object sender, EventArgs e)
        {
            if (MappingFrm.chkNotEncoded.CheckState.ToString() == "Checked")
            {
                MappingFrm.chkUndeclared.Checked = false;
                MappingFrm.txtPermitYear.ReadOnly = false;
                MappingFrm.txtPermitNo.ReadOnly = false;
                MappingFrm.dtpPermitDate.Enabled = true;
                MappingFrm.txtBIN.ReadOnly = false;
                m_bNotEncoded = true;

                if (!MappingFrm.m_bInit)
                {
                    if (TaskMan.IsObjectLock(MappingFrm.txtBIN.Text, "BUSINESS MAPPING", "DELETE", "ASS"))
                    { }

                    MappingFrm.txtBIN.Text = "";
                    /*ClearControls(1);
                    ClearControls(2);
                    ClearControls(3);*/
                    EnableControls(1, true);
                    EnableControls(2, true);
                    EnableControls(3, true);
                    MappingFrm.txtBIN.Text = "";
                    MappingFrm.cmbPlace.Text = "";
                    MappingFrm.cmbPlace.Enabled = true;
                    MappingFrm.btnOwnSearch.Enabled = true;
                    //MappingFrm.chkNotEncoded.Checked = true;
                    m_bNotEncoded = true;
                    //MappingFrm.m_bInit = true;
                    MappingFrm.btnSearch.Text = "Search Bin";
                    MappingFrm.chkOtherLine.Checked = false;
                }
                else
                {
                    if (!m_bUpdateRecord)
                        MappingFrm.txtBIN.Text = "";
                }
            }
        }

        public override void Undeclared(object sender, EventArgs e)
        {
            if (MappingFrm.chkUndeclared.CheckState.ToString() == "Checked")
            {
                MappingFrm.chkNotEncoded.Checked = false;
                MappingFrm.txtPermitYear.ReadOnly = true;
                MappingFrm.txtPermitNo.ReadOnly = true;
                MappingFrm.dtpPermitDate.Enabled = false;
                MappingFrm.dtpPermitDate.Value = AppSettingsManager.GetCurrentDate();
                MappingFrm.txtBIN.ReadOnly = false;
                m_bNotEncoded = false;

                if (!MappingFrm.m_bInit)
                {
                    if (TaskMan.IsObjectLock(MappingFrm.txtBIN.Text, "BUSINESS MAPPING", "DELETE", "ASS"))
                    { }

                    MappingFrm.txtBIN.Text = "";
                    /*ClearControls(1);
                    ClearControls(2);
                    ClearControls(3);*/
                    EnableControls(1, true);
                    EnableControls(2, true);
                    EnableControls(3, true);
                    MappingFrm.txtBIN.Text = "";
                    MappingFrm.cmbPlace.Text = "";
                    MappingFrm.cmbPlace.Enabled = true;
                    MappingFrm.btnOwnSearch.Enabled = true;
                    //MappingFrm.chkNotEncoded.Checked = true;
                    m_bNotEncoded = false;
                    //MappingFrm.m_bInit = true;
                    MappingFrm.btnSearch.Text = "Search Bin";
                    MappingFrm.chkOtherLine.Checked = false;
                }
                else
                {
                    if (!m_bUpdateRecord)
                        MappingFrm.txtBIN.Text = "";
                }
            }
        }

        public override void BinSearch()
        {
            OracleResultSet pRec = new OracleResultSet();
            
            if (MappingFrm.btnSearch.Text == "Search Bin")
            {
                if (MappingFrm.txtBIN.Text.Trim() != "")
                {
                    if (!TaskMan.IsObjectLock(MappingFrm.txtBIN.Text.Trim(), "BUSINESS MAPPING", "ADD", "ASS"))
                    {
                        if (ValidateBINIfMatched(MappingFrm.txtBIN.Text.Trim()))
                            LoadValues(MappingFrm.txtBIN.Text.Trim());
                    }
                }
                else
                {
                    pRec.Close();
                    MessageBox.Show("No record found", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ClearControls(1);
                    ClearControls(2);
                    ClearControls(3);
                    this.EnableControls(1, true);
                    this.EnableControls(2, true);
                    this.EnableControls(3, true);
                    this.EnableControls(4, true);
                    m_bUpdateRecord = false;
                    return;
                }

                
            }
            else
            {
                if (TaskMan.IsObjectLock(MappingFrm.txtBIN.Text.Trim(), "BUSINESS MAPPING", "DELETE", "ASS"))
                { }

                ClearControls(1);
                ClearControls(2);
                ClearControls(3);
                ClearOtherControls();   // RMC 20120419 added tagging of inspector in Business Mapping
                this.EnableControls(1, true);
                this.EnableControls(2, true);
                this.EnableControls(3, true);
                this.EnableControls(4, true);
                MappingFrm.txtBIN.Text = "";
                MappingFrm.cmbPlace.Text = "";
                MappingFrm.cmbPlace.Enabled = true;
                MappingFrm.btnOwnSearch.Enabled = true;
                MappingFrm.chkNotEncoded.Checked = true;
                m_bNotEncoded = true;
                MappingFrm.m_bInit = false;
                MappingFrm.btnSearch.Text = "Search Bin";
                MappingFrm.chkOtherLine.Checked = false;
                MappingFrm.btnUntag.Enabled = false;
                m_strTempBIN = "";
                m_bUpdateRecord = false;
                m_sUpdateBIN = "";
            }
            
        }

        private bool Validations(string sBIN)
        {
            OracleResultSet pRec = new OracleResultSet();

            if (MappingFrm.txtBldgName.Text.Trim() == "")
            {
                MessageBox.Show("Building name required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

            if (MappingFrm.cmbPlace.Text.Trim() == "RENTED" && (MappingFrm.txtOwnCode.Text.Trim() == MappingFrm.txtPlaceOwnCode.Text.Trim()))
            {
                MessageBox.Show("Conflict in business place ownership", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

            if (MappingFrm.txtBussAdd.Text.Trim() == "")
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

            if (MappingFrm.cmbPlace.Text.Trim() == "")
            {
                MessageBox.Show("Business place type required.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            pRec.Query = "select * from btm_temp_businesses";
            pRec.Query += string.Format(" where trim(bns_nm) = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()));
            pRec.Query += string.Format(" and bns_house_no = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtBussAdd.Text.Trim()));
            pRec.Query += string.Format(" and bns_street = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtBussStreet.Text.Trim()));
            pRec.Query += string.Format(" and bns_brgy = '{0}'", StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(MappingFrm.cmbBussBrgy.Text.Trim())));
            pRec.Query += string.Format(" and tbin <> '{0}'", sBIN);
            string sTmpBIN = "";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sTmpBIN = pRec.GetString("tbin");

                    if (MessageBox.Show("Business name already added with BIN: "+sTmpBIN+" .\nContinue?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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

            // validate old bin
            //pRec.Query = "select * from ref_no_tbl where old_bin = '" + sBIN + "'";
            pRec.Query = "select * from app_permit_no where app_no = '" + sBIN + "'"; //MCR 20150114
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sTmpBIN = pRec.GetString("bin");

                    MessageBox.Show("Old BIN: " + sBIN + " is an official business.\nPlease check.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    pRec.Close();
                    return false;
                }
                else
                    pRec.Close();
            }

            if (m_bNotEncoded)
            {
                string sDate = string.Format("{0:yyyy}", MappingFrm.dtpPermitDate.Value);

                if (MappingFrm.txtPermitYear.Text.Trim() != sDate)
                {
                    MessageBox.Show("Conflict in permit year and permit date", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (MappingFrm.chkOtherLine.Checked == true)
            {
                int iCount = 0;
                pRec.Query = "select count(*) from btm_addl_bns_que where bin = '" + m_strTempBIN + "'";
                int.TryParse(pRec.ExecuteScalar(), out iCount);

                if (iCount == 0)
                {
                    MessageBox.Show("No other line of business found, please check","Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            if (m_bNotEncoded)  // RMC 20120420 corrected validation of permit if undeclared
            {
                if (!ValidatePermit(sBIN, m_sUpdateBIN))
                    return false;
            }
            return true;
        }

        private void LoadGIS()
        {
            OracleResultSet pSet = new OracleResultSet();
            string sBrgyCode = "";
            string sLandPIN = "";
            string sBldgCode = "";
            string sBldgName = "";

            pSet.Query = "select * from btm_gis_loc where bin = '" + MappingFrm.txtBIN.Text.Trim() + "'";
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    sLandPIN = pSet.GetString("land_pin");
                    sBrgyCode = sLandPIN.Substring(7, 3);
                    sBldgCode = pSet.GetString("bldg_code");
                    sBldgName = pSet.GetString("bldg_name");
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from brgy where brgy_code = '{0}'", sBrgyCode);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    MappingFrm.cmbBrgy.Text = pSet.GetString("brgy_nm").Trim();
                }
            }
            pSet.Close();

            MappingFrm.cmbLandPin.Text = sLandPIN;
            MappingFrm.cmbBldgCode.Text = sBldgCode;
            MappingFrm.txtBldgName.Text = sBldgName;
        }

        public override void OtherLine(object sender, EventArgs e)
        {
            if(m_strTempBIN == "" && MappingFrm.txtBIN.Text.Trim() != "")
                m_strTempBIN = MappingFrm.txtBIN.Text.Trim();

            DateTime dtCurrent = AppSettingsManager.GetCurrentDate();

            //if (MappingFrm.chkOtherLine.CheckState.ToString() == "Checked")
            if (MappingFrm.chkOtherLine.Checked)
            {
                if (MappingFrm.txtOfficialBussName.Text.ToString() == "")
                {
                    MessageBox.Show("Indicate business name first", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MappingFrm.chkOtherLine.Checked = false;
                    return;
                }

                if (m_strTempBIN == "")
                {
                    m_strTempBIN = string.Format("{0:00#}-{1:00#}:{2:00#}:{3:00#}:{4}", dtCurrent.Day.ToString(), dtCurrent.Hour.ToString(), dtCurrent.Minute.ToString(), dtCurrent.Second.ToString(), AppSettingsManager.SystemUser.UserCode);
                }

                if (!MappingFrm.m_bInit)
                {
                    frmAddlBusiness frmAddlBusiness = new frmAddlBusiness();
                    frmAddlBusiness.BIN = m_strTempBIN;
                    frmAddlBusiness.TaxYear = MappingFrm.txtPermitYear.Text.Trim();
                    frmAddlBusiness.ShowDialog();

                    LoadOtherLine();
                }
                
            }
        }

        private void LoadOtherLine()
        {
            OracleResultSet result = new OracleResultSet();
            int iCount = 0;
            string sBIN = "";

            sBIN = MappingFrm.txtBIN.Text.Trim();

            if (sBIN == "")
                sBIN = m_strTempBIN;

            result.Query = "select count(*) from btm_addl_bns_que where bin = '" + sBIN + "'";
            int.TryParse(result.ExecuteScalar(), out iCount);

            if (iCount > 0)
                MappingFrm.chkOtherLine.Checked = true;
            else
                MappingFrm.chkOtherLine.Checked = false;
        }

        public override void LoadValues(string sBIN)
        {
            OracleResultSet pRec = new OracleResultSet();
            string sPermit = "";
            string sOldBin = "";
            DateTime dtpPermit;

            ClearControls(1);
            ClearControls(2);
            ClearControls(3);

            this.EnableControls(1, true);
            this.EnableControls(2, true);
            this.EnableControls(3, true);
            this.EnableControls(4, true);

            pRec.Query = "select * from btm_temp_businesses where tbin = '" + MappingFrm.txtBIN.Text.Trim() + "'";
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    m_sUpdateBIN = MappingFrm.txtBIN.Text.Trim();

                    sPermit = pRec.GetString("permit_no").Trim();
                    dtpPermit = pRec.GetDateTime("permit_dt");
                    if (sPermit != "")
                    {
                        MappingFrm.txtPermitYear.Text = sPermit.Substring(0, 4);
                        MappingFrm.txtPermitNo.Text = sPermit.Substring(5, 5);
                    }
                    MappingFrm.txtOfficialBussName.Text = pRec.GetString("bns_nm");
                    MappingFrm.txtBussAdd.Text = pRec.GetString("bns_house_no");
                    MappingFrm.txtBussStreet.Text = pRec.GetString("bns_street");
                    MappingFrm.cmbBussBrgy.Text = pRec.GetString("bns_brgy");
                    MappingFrm.txtBussZone.Text = pRec.GetString("bns_zone");
                    MappingFrm.cmbBussDist.Text = pRec.GetString("bns_dist");
                    MappingFrm.txtBussCity.Text = pRec.GetString("bns_mun");
                    MappingFrm.txtBussProv.Text = pRec.GetString("bns_prov");
                    MappingFrm.cmbOrgKind.Text = pRec.GetString("orgn_kind");
                    MappingFrm.m_sBnsCode = pRec.GetString("bns_code");
                    MappingFrm.txtBnsType.Text = AppSettingsManager.GetBnsDesc(MappingFrm.m_sBnsCode);
                    MappingFrm.txtArea.Text = string.Format("{0:#,###.00}", pRec.GetDouble("flr_area"));
                    MappingFrm.txtStoreys.Text = string.Format("{0:#,###}", pRec.GetInt("num_storeys"));
                    MappingFrm.m_sOwnCode = pRec.GetString("own_code");
                    MappingFrm.m_sBussOwnCode = pRec.GetString("busn_own");
                    MappingFrm.cmbPlace.Text = pRec.GetString("place_occupancy");
                    MappingFrm.txtRent.Text = string.Format("{0:#,###.00}", pRec.GetDouble("rent_lease_mo"));
                    MappingFrm.txtRemarks.Text = pRec.GetString("remarks").Trim();
                    MappingFrm.cmbInspector.Text = pRec.GetString("inspected_by").Trim();   // RMC 20120419 added tagging of inspector in Business Mapping

                    sOldBin = pRec.GetString("old_bin").Trim();

                    MappingFrm.m_bInit = true;
                    if (sOldBin == "")
                        MappingFrm.chkUndeclared.Checked = true;
                    else
                        MappingFrm.chkNotEncoded.Checked = true;

                    LoadOwner(MappingFrm.m_sOwnCode, true);
                    LoadOwner(MappingFrm.m_sBussOwnCode, false);
                    LoadGIS();
                    LoadOtherLine();

                    MappingFrm.btnSearch.Text = "Clear";
                    MappingFrm.m_bInit = false;

                    MappingFrm.btnUntag.Enabled = true;
                    pRec.Close();
                }
                else
                {
                    pRec.Close();
                    MessageBox.Show("No record found", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    m_bUpdateRecord = false;
                    return;
                }
            }

            
        }

        private void SaveGISTable()
        {
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();

                pGisRec.Query = "update gis_business_location set bldg_name = '" + StringUtilities.HandleApostrophe(MappingFrm.txtBldgName.Text.Trim()) + "'";
                pGisRec.Query += " where pin = '" + MappingFrm.cmbLandPin.Text + "' and bldg_code = '" + MappingFrm.cmbBldgCode.Text + "'";
                if (pGisRec.ExecuteNonQuery() == 0)
                { }
            }
            catch { }
        }

        public override void Untag()
        {
            pCmd = new OracleResultSet();
            pCmd.Transaction = true;

            if (MessageBox.Show("Un-tag record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pCmd.Query = "delete from btm_temp_businesses where tbin = '" + MappingFrm.txtBIN.Text.Trim() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = "delete from btm_gis_loc where bin = '" + MappingFrm.txtBIN.Text.Trim() + "'";
                if (pCmd.ExecuteNonQuery() == 0)
                { }

                pCmd.Query = string.Format("delete from btm_addl_bns_que where bin = '{0}' and tax_year = '{1}'", MappingFrm.txtBIN.Text.Trim(), MappingFrm.txtPermitYear.Text.Trim());
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

                if (AuditTrail.InsertTrail("ABM-U-U", "btm_temp_businesses", MappingFrm.txtBIN.Text.Trim()) == 0)
                {
                    pCmd.Rollback();
                    pCmd.Close();
                    MessageBox.Show(pCmd.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Record un-tagged", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (MessageBox.Show("Add new record?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (TaskMan.IsObjectLock(MappingFrm.txtBIN.Text.Trim(), "BUSINESS MAPPING", "DELETE", "ASS"))
                    { }

                    ClearControls(1);
                    ClearControls(2);
                    ClearControls(3);
                    MappingFrm.txtBIN.Text = "";
                    MappingFrm.cmbPlace.Text = "";
                    MappingFrm.cmbPlace.Enabled = true;
                    MappingFrm.btnOwnSearch.Enabled = true;
                    MappingFrm.chkNotEncoded.Checked = true;
                    m_bNotEncoded = true;
                    MappingFrm.m_bInit = false;
                    MappingFrm.btnSearch.Text = "Search Bin";
                    m_strTempBIN = "";

                }
                else
                    Close();
            }
        }

        public override void PlaceSelectedValueChanged()
        {
            if (MappingFrm.cmbPlace.Text == "RENTED")
            {
                EnableControls(3, true);
            }
            else
            {
                EnableControls(3, false);
                LoadOwner(MappingFrm.txtOwnCode.Text, false);
                //MappingFrm.chkChangeBussPlace.Checked = false;
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
            catch { }

            pRec.Query = "select * from btm_temp_businesses";
            pRec.Query += string.Format(" where tbin = '{0}'", sBIN);
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
                        MappingFrm.txtBIN.Text = "";
                        MappingFrm.cmbPlace.Text = "";
                        MappingFrm.cmbPlace.Enabled = true;
                        MappingFrm.btnOwnSearch.Enabled = true;
                        MappingFrm.chkNotEncoded.Checked = true;
                        m_bNotEncoded = true;
                        MappingFrm.m_bInit = false;
                        MappingFrm.btnSearch.Text = "Search Bin";
                        m_strTempBIN = "";
                        MappingFrm.btnUntag.Enabled = false;

                        this.EnableControls(1, true);
                        this.EnableControls(2, true);
                        this.EnableControls(3, true);
                        this.EnableControls(4, true);
                        m_bUpdateRecord = false;
                        return false;
                    }
                    else
                    {
                        try
                        {
                            pGisRec.Query = "select * from gis_businesses where bin = '" + sBIN + "'";
                            if (pGisRec.Execute())
                            {
                                if (pGisRec.Read())
                                {
                                    MappingFrm.cmbBrgy.Text = pGisRec.GetString("barangay").Trim();

                                    MappingFrm.m_sGisBrgy = MappingFrm.cmbBrgy.Text.Trim();
                                    MappingFrm.LoadPin(AppSettingsManager.GetBrgyCode(MappingFrm.cmbBrgy.Text));
                                    MappingFrm.cmbLandPin.Text = pGisRec.GetString("land_pin").Trim();

                                    MappingFrm.LoadBldgCode(MappingFrm.cmbLandPin.Text);
                                    MappingFrm.cmbBldgCode.Text = pGisRec.GetString("bldgcode");

                                }
                            }
                            pGisRec.Close();
                        }
                        catch { }

                        pRec.Close();

                        MappingFrm.btnUntag.Enabled = true;
                        m_bUpdateRecord = true;

                        return true;
                    }
                }
            }
            pRec.Close();

            return true;
        }

        public override void ValidatePermitNo()
        {
            if (!ValidatePermit(MappingFrm.txtBIN.Text.ToString().Trim(),m_sUpdateBIN))
            {
                MappingFrm.txtPermitNo.Text = "";
                return;
            }
        }

        public override void ValidatePermitYear()
        {
            if (!ValidatePermit(MappingFrm.txtBIN.Text.ToString().Trim(),m_sUpdateBIN))
            {
                MappingFrm.txtPermitYear.Text = "";
                return;
            }
        }

        public override void ValidateBussName()
        {
            OracleResultSet pRec = new OracleResultSet();
            string sBIN = "";

            pRec.Query = "select * from businesses";
            pRec.Query += string.Format(" where bns_nm = '{0}'", StringUtilities.HandleApostrophe(MappingFrm.txtOfficialBussName.Text.Trim()));
            if (pRec.Execute())
            {
                if (pRec.Read())
                {
                    sBIN = pRec.GetString("bin");

                    if (MessageBox.Show("Warning: Double business name with BIN:" + sBIN + ".\nContinue?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        pRec.Close();
                        MappingFrm.txtOfficialBussName.Text = "";
                        return;
                    }

                }

            }
            pRec.Close();
        }
    }
}
