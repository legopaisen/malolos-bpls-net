

// RMC 20120419 added tagging of inspector in Business Mapping
// RMC 20120330 Modified validation of Business mapping tagging in GIS table
// RMC 20120328 correction in business mapping loading of GIS Map values



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.BPLSApp;
using Amellar.Common.SearchOwner;
using Amellar.Common.StringUtilities;
using Amellar.Common.SearchBusiness;

namespace Amellar.Modules.BusinessMapping
{
    public class Mapping
    {
        protected frmMapping MappingFrm = null;
        protected OracleResultSet pCmd = new OracleResultSet();
        protected BPLSAppSettingList sList = new BPLSAppSettingList();
        protected frmSearchBusiness frmSearchBns = new frmSearchBusiness();

        public Mapping(frmMapping Form)
        {
            this.MappingFrm = Form;
        }

        public virtual void FormLoad()
        {
            
            
        }

        public virtual void BinSearch()
        {
        }

        public virtual void Save()
        {
        }

        public virtual void Untag()
        {
        }

        public virtual void LoadValues(string sBIN)
        {
        }

        public virtual void ChangePermit(object sender, EventArgs e)
        {
        }

        public virtual void ChangePermitYear(object sender, EventArgs e)
        {
        }

        public virtual void ChangeBussPlace(object sender, EventArgs e)
        {
        }

        public virtual void ApplyNewBussName(object sender, EventArgs e)
        {
        }

        public virtual void ApplyNewLoc(object sender, EventArgs e)
        {
        }

        public virtual void ApplyNewBussType(object sender, EventArgs e)
        {
        }

        public virtual void ApplyNewOwner(object sender, EventArgs e)
        {
        }

        public virtual void OtherLine(object sender, EventArgs e)
        {
        }

        public virtual void ValidateBussName()
        {
        }

        

        public virtual void ChangeArea(object sender, EventArgs e)
        {
        }

        public virtual void ChangeStoreys(object sender, EventArgs e)
        {
        }

        public virtual void NotEncoded(object sender, EventArgs e)
        {
        }

        public virtual void Undeclared(object sender, EventArgs e)
        {
        }

        public virtual void PlaceSelectedValueChanged()
        {
            
        }

        protected void ClearControls(int iGrp)
        {
            if (iGrp == 1)
            {
                MappingFrm.txtOfficialBussName.Text = "";
                MappingFrm.txtBussAdd.Text = "";
                MappingFrm.txtBussStreet.Text = "";
                MappingFrm.cmbBussBrgy.Text = "";
                MappingFrm.txtBussZone.Text = "";
                MappingFrm.cmbBussDist.Text = "";
                //MappingFrm.txtBussCity.Text = "";
                MappingFrm.txtBussProv.Text = "";
                MappingFrm.txtBnsType.Text = "";
                MappingFrm.cmbOrgKind.Text = "";
                MappingFrm.txtPermitYear.Text = "";
                MappingFrm.txtPermitNo.Text = "";
                MappingFrm.dtpPermitDate.Value = AppSettingsManager.GetCurrentDate();
                MappingFrm.txtRemarks.Text = "";
                MappingFrm.txtArea.Text = "";
                MappingFrm.txtStoreys.Text = "";

                EnableControls(iGrp, false);
            }
            if (iGrp == 2)
            {
                MappingFrm.txtOwnCode.Text = "";
                MappingFrm.txtOwnLn.Text = "";
                MappingFrm.txtOwnFn.Text = "";
                MappingFrm.txtOwnMi.Text = "";
                MappingFrm.txtOwnAddNo.Text = "";
                MappingFrm.txtOwnStreet.Text = "";
                MappingFrm.cmbOwnBrgy.Text = "";
                MappingFrm.cmbOwnDist.Text = "";
                MappingFrm.txtOwnMun.Text = "";
                MappingFrm.txtOwnProv.Text = "";
                MappingFrm.txtOwnZip.Text = "";
                MappingFrm.m_sNewOwnCode = "";
                
                EnableControls(iGrp, false);
            }
            if (iGrp == 3)
            {
                MappingFrm.txtPlaceOwnCode.Text = "";
                MappingFrm.txtPlaceOwnLn.Text = "";
                MappingFrm.txtPlaceOwnFn.Text = "";
                MappingFrm.txtPlaceOwnMi.Text = "";
                MappingFrm.txtPlaceOwnAdd.Text = "";
                MappingFrm.txtPlaceOwnStreet.Text = "";
                MappingFrm.cmbPlaceOwnBrgy.Text = "";
                MappingFrm.cmbPlaceOwnDist.Text = "";
                MappingFrm.txtPlaceOwnMun.Text = "";
                MappingFrm.txtPlaceOwnProv.Text = "";
                MappingFrm.txtPlaceOwnZip.Text = "";
                MappingFrm.txtRent.Text = "";

                EnableControls(iGrp, false);
            }
            if (iGrp == 4)
            {
                MappingFrm.chkApplyNewBussName.Checked = false;
                MappingFrm.chkApplyNewBussType.Checked = false;
                MappingFrm.chkApplyNewLoc.Checked = false;
                MappingFrm.chkApplyNewOwner.Checked = false;
                MappingFrm.chkOtherLine.Checked = false;
                MappingFrm.chkChangePermit.Checked = false;
                MappingFrm.chkChangeBussPlace.Checked = false;
                MappingFrm.chkChangeArea.Checked = false;
                MappingFrm.chkChangeStoreys.Checked = false;
                MappingFrm.chkChangeTaxYear.Checked = false;

                EnableControls(iGrp, false);
            }
        }

        protected void EnableControls(int iGrp, bool blnEnable)
        {
            if (iGrp == 1)
            {
                MappingFrm.txtOfficialBussName.ReadOnly = !blnEnable;
                MappingFrm.txtBussAdd.ReadOnly = !blnEnable;
                MappingFrm.txtBussStreet.ReadOnly = !blnEnable;
                MappingFrm.txtBussZone.ReadOnly = !blnEnable;
                MappingFrm.cmbBussBrgy.Enabled = blnEnable;
                MappingFrm.cmbBussDist.Enabled = blnEnable;
                //txtBussCity.ReadOnly = !blnEnable;
                MappingFrm.txtBussProv.ReadOnly = !blnEnable;
                MappingFrm.btnBnsType.Enabled = blnEnable;
                MappingFrm.cmbOrgKind.Enabled = blnEnable;
                MappingFrm.txtArea.ReadOnly = !blnEnable;
                MappingFrm.txtStoreys.ReadOnly = !blnEnable;
            }

            if (iGrp == 2)
            {
                MappingFrm.txtOwnLn.ReadOnly = !blnEnable;
                MappingFrm.txtOwnFn.ReadOnly = !blnEnable;
                MappingFrm.txtOwnMi.ReadOnly = !blnEnable;
                MappingFrm.txtOwnAddNo.ReadOnly = !blnEnable;
                MappingFrm.txtOwnStreet.ReadOnly = !blnEnable;
                MappingFrm.cmbOwnBrgy.Enabled = blnEnable;
                MappingFrm.cmbOwnDist.Enabled = blnEnable;
                MappingFrm.txtOwnMun.ReadOnly = !blnEnable;
                MappingFrm.txtOwnProv.ReadOnly = !blnEnable;
                MappingFrm.txtOwnZip.ReadOnly = !blnEnable;
            }

            if (iGrp == 3)
            {
                MappingFrm.txtPlaceOwnLn.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnFn.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnMi.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnAdd.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnStreet.ReadOnly = !blnEnable;
                MappingFrm.cmbPlaceOwnBrgy.Enabled = blnEnable;
                MappingFrm.cmbPlaceOwnDist.Enabled = blnEnable;
                MappingFrm.txtPlaceOwnMun.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnProv.ReadOnly = !blnEnable;
                MappingFrm.txtPlaceOwnZip.ReadOnly = !blnEnable;
                //cmbPlace.Enabled = blnEnable;
                MappingFrm.txtRent.ReadOnly = !blnEnable;
                MappingFrm.btnPlaceOwnSearch.Enabled = blnEnable;

            }

            if (iGrp == 4)
            {
                MappingFrm.chkApplyNewBussName.Enabled = blnEnable;
                MappingFrm.chkApplyNewBussType.Enabled = blnEnable;
                MappingFrm.chkApplyNewLoc.Enabled = blnEnable;
                MappingFrm.chkApplyNewOwner.Enabled = blnEnable;
                MappingFrm.chkOtherLine.Enabled = blnEnable;
                MappingFrm.chkChangePermit.Enabled = blnEnable;
                MappingFrm.chkChangeBussPlace.Enabled = blnEnable;
                MappingFrm.chkChangeArea.Enabled = blnEnable;
                MappingFrm.chkChangeStoreys.Enabled = blnEnable;
                MappingFrm.chkChangeTaxYear.Enabled = blnEnable;
            }

        }

        protected void LoadGisValues(string sBIN)
        {
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();

                //pGisRec.Query = "select * from gis_businesses where bin = '" + sBIN + "'";
                pGisRec.Query = "select * from gis_bin where bin = '" + sBIN + "'";  // RMC 20120330 Modified validation of Business mapping tagging in GIS table
                if (pGisRec.Execute())
                {
                    if (pGisRec.Read())
                    {
                        //MappingFrm.cmbBrgy.Text = pGisRec.GetString("barangay").Trim();
                        MappingFrm.cmbBrgy.Text = AppSettingsManager.GetBrgyName(pGisRec.GetString("brgy").Trim()); // RMC 20120330 Modified validation of Business mapping tagging in GIS table
                        MappingFrm.cmbBldgCode.Text = pGisRec.GetString("bldgcode").Trim();
                    }
                    else
                    {
                        // RMC 20120328 correction in business mapping loading of GIS Map values (s)
                        MappingFrm.cmbBrgy.Text = "";
                        MappingFrm.cmbBldgCode.Text = "";
                        MappingFrm.cmbLandPin.Text = "";
                        MappingFrm.cmbBldgCode.Text = "";

                        //MessageBox.Show("No record found in GIS Mapping, please check GIS map","Business Mapping",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // RMC 20120328 correction in business mapping loading of GIS Map values (e)
                    }
                }
                pGisRec.Close();
            }
            catch { }
        }

        protected void LoadOwner(string sOwnCode, bool blnBussOwn)
        {
            sList.OwnName = sOwnCode;

            for (int j = 0; j < sList.OwnNamesSetting.Count; j++)
            {
                if (blnBussOwn)
                {
                    MappingFrm.txtOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                    MappingFrm.m_sNewOwnCode = MappingFrm.txtOwnCode.Text.Trim();
                    MappingFrm.txtOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                    MappingFrm.txtOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                    MappingFrm.txtOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                    MappingFrm.txtOwnAddNo.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                    MappingFrm.txtOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                    MappingFrm.cmbOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                    MappingFrm.cmbOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                    MappingFrm.txtOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                    MappingFrm.txtOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                    MappingFrm.txtOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                    MappingFrm.btnOwnSearch.Text = "Clear";
                }
                else
                {
                    MappingFrm.txtPlaceOwnCode.Text = sList.OwnNamesSetting[j].sOwnerCode;
                    MappingFrm.txtPlaceOwnLn.Text = sList.OwnNamesSetting[j].sLn;
                    MappingFrm.txtPlaceOwnFn.Text = sList.OwnNamesSetting[j].sFn;
                    MappingFrm.txtPlaceOwnMi.Text = sList.OwnNamesSetting[j].sMi;
                    MappingFrm.txtPlaceOwnAdd.Text = sList.OwnNamesSetting[j].sOwnHouseNo;
                    MappingFrm.txtPlaceOwnStreet.Text = sList.OwnNamesSetting[j].sOwnStreet;
                    MappingFrm.cmbPlaceOwnBrgy.Text = sList.OwnNamesSetting[j].sOwnBrgy;
                    MappingFrm.cmbPlaceOwnDist.Text = sList.OwnNamesSetting[j].sOwnDist;
                    MappingFrm.txtPlaceOwnMun.Text = sList.OwnNamesSetting[j].sOwnMun;
                    MappingFrm.txtPlaceOwnProv.Text = sList.OwnNamesSetting[j].sOwnProv;
                    MappingFrm.txtPlaceOwnZip.Text = sList.OwnNamesSetting[j].sOwnZip;
                    MappingFrm.btnPlaceOwnSearch.Text = "Clear";
                }
            }
        }

        public void OwnSearch()
        {
            if (MappingFrm.btnOwnSearch.Text.Trim() == "Clear")
            {
                ClearControls(2);
                MappingFrm.btnOwnSearch.Text = "Search";
                //MappingFrm.btnSave.Enabled = false;
            }
            else
            {

                frmSearchOwner SearchOwner = new frmSearchOwner();
                SearchOwner.m_sPageWatch = "PAGE2";
                SearchOwner.ShowDialog();

                MappingFrm.txtOwnCode.Text = SearchOwner.m_strOwnCode;
                MappingFrm.txtOwnLn.Text = SearchOwner.m_sOwnLn;
                MappingFrm.txtOwnFn.Text = SearchOwner.m_sOwnFn;
                MappingFrm.txtOwnMi.Text = SearchOwner.m_sOwnMi;
                MappingFrm.txtOwnAddNo.Text = SearchOwner.m_sOwnAdd;
                MappingFrm.txtOwnStreet.Text = SearchOwner.m_sOwnStreet;
                MappingFrm.cmbOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                MappingFrm.cmbOwnDist.Text = SearchOwner.m_sOwnDist;
                MappingFrm.txtOwnMun.Text = SearchOwner.m_sOwnMun;
                MappingFrm.txtOwnProv.Text = SearchOwner.m_sOwnProv;
                MappingFrm.txtOwnZip.Text = SearchOwner.m_sOwnZip;
                MappingFrm.m_sNewOwnCode = MappingFrm.txtOwnCode.Text;

                if (MappingFrm.cmbPlace.Text.Trim() == "OWNED")
                {
                    MappingFrm.txtPlaceOwnCode.Text = SearchOwner.m_strOwnCode;
                    MappingFrm.txtPlaceOwnLn.Text = SearchOwner.m_sOwnLn;
                    MappingFrm.txtPlaceOwnFn.Text = SearchOwner.m_sOwnFn;
                    MappingFrm.txtPlaceOwnMi.Text = SearchOwner.m_sOwnMi;
                    MappingFrm.txtPlaceOwnAdd.Text = SearchOwner.m_sOwnAdd;
                    MappingFrm.txtPlaceOwnStreet.Text = SearchOwner.m_sOwnStreet;
                    MappingFrm.cmbPlaceOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                    MappingFrm.cmbPlaceOwnDist.Text = SearchOwner.m_sOwnDist;
                    MappingFrm.txtPlaceOwnMun.Text = SearchOwner.m_sOwnMun;
                    MappingFrm.txtPlaceOwnProv.Text = SearchOwner.m_sOwnProv;
                    MappingFrm.txtPlaceOwnZip.Text = SearchOwner.m_sOwnZip;
                }

                if (MappingFrm.txtOwnCode.Text.Trim() != "")
                {
                    MappingFrm.btnOwnSearch.Text = "Clear";
                    EnableControls(2, false);
                }
            }
        }

        public void PlaceOwnSearch()
        {
            frmSearchOwner SearchOwner = new frmSearchOwner();
            SearchOwner.m_sPageWatch = "PAGE2";
            SearchOwner.ShowDialog();

            MappingFrm.txtPlaceOwnCode.Text = SearchOwner.m_strOwnCode;
            MappingFrm.txtPlaceOwnLn.Text = SearchOwner.m_sOwnLn;
            MappingFrm.txtPlaceOwnFn.Text = SearchOwner.m_sOwnFn;
            MappingFrm.txtPlaceOwnMi.Text = SearchOwner.m_sOwnMi;
            MappingFrm.txtPlaceOwnAdd.Text = SearchOwner.m_sOwnAdd;
            MappingFrm.txtPlaceOwnStreet.Text = SearchOwner.m_sOwnStreet;
            MappingFrm.cmbPlaceOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
            MappingFrm.cmbPlaceOwnDist.Text = SearchOwner.m_sOwnDist;
            MappingFrm.txtPlaceOwnMun.Text = SearchOwner.m_sOwnMun;
            MappingFrm.txtPlaceOwnProv.Text = SearchOwner.m_sOwnProv;
            MappingFrm.txtPlaceOwnZip.Text = SearchOwner.m_sOwnZip;
        }

        
        protected void ClearOtherControls()
        {
            MappingFrm.m_sBnsStat = "";
            MappingFrm.m_sPermit = "";
            MappingFrm.m_sPermitDate = "";
            MappingFrm.m_sBnsCode = "";
            MappingFrm.m_sBnsNm = "";
            MappingFrm.m_sAddNo = "";
            MappingFrm.m_sStreet = "";
            MappingFrm.m_sBrgy = "";
            MappingFrm.m_sZone = "";
            MappingFrm.m_sDist = "";
            MappingFrm.m_sMun = "";
            MappingFrm.m_sProv = "";
            MappingFrm.m_sBnsLoc = "";
            MappingFrm.m_sNewLoc = "";
            MappingFrm.m_sOwnCode = "";
            MappingFrm.m_sLeaseAmt = "0";
            MappingFrm.m_sBussOwnCode = "";
            MappingFrm.cmbInspector.Text = "";  // RMC 20120419 added tagging of inspector in Business Mapping
        }

        protected void Close()
        {
            MappingFrm.OnClose();
        }

        public virtual void ValidatePermitNo()
        {
        }

        public virtual void ValidatePermitYear()
        {
        }

        //protected bool ValidatePermit(string sBIN)
        protected bool ValidatePermit(string sBIN, string sUpdateBIN)
        {
            //validate
            string sPermit = MappingFrm.txtPermitYear.Text.Trim() + "-" + MappingFrm.txtPermitNo.Text.Trim();
            string strRecBin = "";

            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = string.Format("select * from businesses where permit_no = '{0}'", sPermit);
            if(sBIN != "")
                pSet.Query += string.Format(" and bin <> '{0}'", sBIN);
            if(sUpdateBIN != "")
                pSet.Query += string.Format(" and bin <> '{0}'", sUpdateBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strRecBin = pSet.GetString("bin");

                    MessageBox.Show("Duplicate Permit Number with BIN: " + strRecBin + ".\nVerify data first.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from btm_businesses where permit_no = '{0}'", sPermit);
            if (sBIN != "")
                pSet.Query += string.Format(" and bin <> '{0}'", sBIN);
            if (sUpdateBIN != "")
                pSet.Query += string.Format(" and bin <> '{0}'", sUpdateBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strRecBin = pSet.GetString("bin");

                    MessageBox.Show("Permit Number already used by mapped BIN: " + strRecBin + ".\nVerify data first.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();

            pSet.Query = string.Format("select * from btm_temp_businesses where permit_no = '{0}'", sPermit);
            if (sBIN != "")
                pSet.Query += string.Format(" and tbin <> '{0}'", sBIN);
            if (sUpdateBIN != "")
                pSet.Query += string.Format(" and tbin <> '{0}'", sUpdateBIN);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    strRecBin = pSet.GetString("tbin");

                    MessageBox.Show("Permit Number already used by mapped BIN: " + strRecBin + ".\nVerify data first.", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            pSet.Close();

            return true;
        }

        protected void LoadInspector()
        {
            // RMC 20120419 added tagging of inspector in Business Mapping

            OracleResultSet pSet = new OracleResultSet();

            MappingFrm.cmbInspector.Items.Clear();

            pSet.Query = "select * from inspector order by inspector_code";
            if (pSet.Execute())
            {
                MappingFrm.cmbInspector.Items.Add("");

                while (pSet.Read())
                {
                    MappingFrm.cmbInspector.Items.Add(pSet.GetString("inspector_code"));
                }
            }
            pSet.Close();
        }

        
    }
}
