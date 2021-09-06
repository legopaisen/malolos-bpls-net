using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.BPLSApp;
using Amellar.Common.SearchBusiness;
using Amellar.Common.SearchOwner;
using Amellar.Common.StringUtilities;
using Amellar.Common.BusinessType;
using Amellar.Common.frmBns_Rec;

namespace Amellar.Modules.BusinessMapping
{
    public partial class frmMapping : Form
    {
        
        
        
        public string m_sBnsStat = "";
        public string m_sPermit = "";
        public string m_sPermitDate = "";
        public string m_sBnsCode = "";
        public string m_sNewBnsCode = "";
        public string m_sLeaseAmt = "0";
        public string m_sX = "";
        public string m_sY = "";
        public string m_sBnsNm = "";
        public string m_sAddNo = "";
        public string m_sStreet = "";
        public string m_sBrgy = "";
        public string m_sZone = "";
        public string m_sDist = "";
        public string m_sMun = "";
        public string m_sProv = "";
        public string m_sBnsLoc = "";
        public string m_sNewLoc = "";
        public string m_sOwnCode = "";
        public string m_sNewOwnCode = "";
        public string m_sGisBrgy = "";
        public string m_sPlace = "";
        public string m_sBussOwnCode = "";
        public string m_sArea = "";
        public string m_sStoreys = "";
        public bool m_bInit = true;
        public bool m_bAddNewRecord = false;
        private string m_strSource = "";
        private Mapping MappingClass = null;

        public string SourceClass
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public frmMapping()
        {
            InitializeComponent();
            bin1.GetLGUCode = ConfigurationAttributes.LGUCode;
            bin1.GetDistCode = ConfigurationAttributes.DistCode;
        }

        private void frmMapping_Load(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();
            try
            {
                pGisRec.CreateNewConnectionGIS();
                
                m_sGisBrgy = "";
                cmbBrgy.Items.Clear();

                string sBrgyCode = "";
                pGisRec.Query = "select distinct substr(pin,8,3) from GIS_BUSINESS_LOCATION order by substr(pin,8,3)";
                if (pGisRec.Execute())
                {
                    cmbBrgy.Items.Add("");
                    while (pGisRec.Read())
                    {
                        sBrgyCode = pGisRec.GetString(0);

                        pRec.Query = string.Format("select * from brgy where brgy_code = '{0}'", sBrgyCode);
                        if (pRec.Execute())
                        {
                            if (pRec.Read())
                            {
                                cmbBrgy.Items.Add(pRec.GetString("brgy_nm"));
                            }
                        }
                        pRec.Close();
                    }
                }
                pGisRec.Close();

                cmbBussBrgy.Items.Clear();
                pRec.Query = "select * from brgy order by brgy_nm";
                if (pRec.Execute())
                {
                    cmbBussBrgy.Items.Add("");

                    while (pRec.Read())
                    {
                        cmbBussBrgy.Items.Add(pRec.GetString("brgy_nm"));
                    }
                }
                pRec.Close();

                cmbBussDist.Items.Clear();
                pRec.Query = "select distinct dist_nm from brgy order by dist_nm";
                if (pRec.Execute())
                {
                    cmbBussDist.Items.Add("");

                    while (pRec.Read())
                    {
                        cmbBussDist.Items.Add(pRec.GetString(0));
                    }
                }
                pRec.Close();

                cmbPlace.Items.Clear();
                cmbPlace.Items.Add("");
                cmbPlace.Items.Add("RENTED");
                cmbPlace.Items.Add("OWNED");

                cmbOrgKind.Items.Clear();
                cmbOrgKind.Items.Add("");
                cmbOrgKind.Items.Add("SINGLE PROPRIETORSHIP");
                cmbOrgKind.Items.Add("CORPORATION");
                cmbOrgKind.Items.Add("PARTNERSHIP");

                txtBussCity.Text = AppSettingsManager.GetConfigValue("02");

                //txtPlate.Focus();
                bin1.txtTaxYear.Focus();

                if (this.SourceClass == "Official")
                    MappingClass = new MappingOfficial(this);
                else
                    MappingClass = new MappingUnofficial(this);

                MappingClass.FormLoad();
            }
            catch {
                // RMC 20140724 corrected error in BUsiness Mapping if no GIS
                MessageBox.Show("No GIS connection detected");
                this.Close();
            }
        }

        private void cmbBrgy_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pRec = new OracleResultSet();
            OracleResultSet pGisRec = new OracleResultSet();
            pGisRec.CreateNewConnectionGIS();

            string sBrgyCode = "";
            m_sX = "";
            m_sY = "";

            sBrgyCode = AppSettingsManager.GetBrgyCode(cmbBrgy.Text.Trim());

            //if (!TaskMan.IsObjectLock(cmbBrgy.Text.Trim(), "BUSINESS MAPPING: " + cmbBrgy.Text.Trim(), "ADD", "ASS"))
            {
                if (TaskMan.IsObjectLock(m_sGisBrgy, "BUSINESS MAPPING: " + m_sGisBrgy, "DELETE", "ASS"))
                { }

                m_sGisBrgy = cmbBrgy.Text.Trim();
                LoadPin(sBrgyCode);
                
                txtLandPIN.Text = "";
                txtBldgName.Text = "";
            }
            /*else
            {
                if (cmbBrgy.Text.Trim() == "")
                {
                    if (TaskMan.IsObjectLock(m_sGisBrgy, "BUSINESS MAPPING: " + m_sGisBrgy, "DELETE", "ASS"))
                    { }

                    m_sGisBrgy = "";
                    LoadPin("");
                    LoadBldgCode("");
                }
                else
                {
                    cmbBrgy.Text = m_sGisBrgy;
                    return;
                }
            }*/
        }

        private void cmbBldgCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleResultSet pGisRec = new OracleResultSet();
            pGisRec.CreateNewConnectionGIS();

            m_sX = "";
            m_sY = "";
            double dx = 0;
            double dy = 0;

            pGisRec.Query = string.Format("select * from GIS_BUSINESS_LOCATION where bldg_code = '{0}' and pin = '{1}'",cmbBldgCode.Text, cmbLandPin.Text);
            if (pGisRec.Execute())
            {
                if (pGisRec.Read())
                {
                    txtLandPIN.Text = pGisRec.GetString("pin");
                    txtBldgName.Text = pGisRec.GetString("bldg_name");
                    m_sX = pGisRec.GetDouble("x").ToString();
                    m_sY = pGisRec.GetDouble("y").ToString();
                    //dx = pGisRec.GetFloat("x");
                    //dy = pGisRec.get
                }
                else
                    txtBldgName.Text = "";
                
            }
            pGisRec.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MappingClass.BinSearch();
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit module without saving?", "Business Mapping", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                OnClose();
            else
                return;
        }

        public void OnClose()
        {
            if (TaskMan.IsObjectLock(bin1.GetBin(), "BUSINESS MAPPING", "DELETE", "ASS"))
            {}
            if (TaskMan.IsObjectLock(cmbBrgy.Text.Trim(), "BUSINESS MAPPING: " + cmbBrgy.Text.Trim(), "DELETE", "ASS"))
            {}
            if (TaskMan.IsObjectLock(m_sGisBrgy, "BUSINESS MAPPING: " + m_sGisBrgy, "DELETE", "ASS"))
            {}
            if (TaskMan.IsObjectLock(txtBIN.Text.Trim(), "BUSINESS MAPPING", "DELETE", "ASS"))
            { }
            
            this.Close();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MappingClass.Save();

        }

        private void btnOwnSearch_Click(object sender, EventArgs e)
        {
            MappingClass.OwnSearch();
        }

        private void btnPlaceOwnSearch_Click(object sender, EventArgs e)
        {
            MappingClass.PlaceOwnSearch();
        }

        private void chkApplyNewBussName_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ApplyNewBussName(sender, e);
        }

        private void chkApplyNewLoc_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ApplyNewLoc(sender, e);
        }

        private void chkApplyNewBussType_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ApplyNewBussType(sender, e);
        }

        private void chkApplyNewOwner_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ApplyNewOwner(sender, e);
            
        }

        private void chkOtherLine_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.OtherLine(sender, e);
        }
   

        private void txtOfficialBussName_Leave(object sender, EventArgs e)
        {
            MappingClass.ValidateBussName();
        }

        

        private void cmbPlace_SelectedValueChanged(object sender, EventArgs e)
        {
            MappingClass.PlaceSelectedValueChanged();
        }

        private void btnBnsType_Click(object sender, EventArgs e)
        {
            frmBusinessType frmBnsType = new frmBusinessType();
            frmBnsType.SetFormStyle(false);
            frmBnsType.ShowDialog();
            txtBnsType.Text = frmBnsType.m_sBnsDescription;
            m_sBnsCode = frmBnsType.m_strBnsCode;
        }

        private void cmbLandPin_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBldgCode(cmbLandPin.Text.Trim());
            
        }

        private void chkChangePermit_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ChangePermit(sender, e);
        }

        private void chkChangeBussPlace_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ChangeBussPlace(sender, e);
        }

        private void txtPermitNo_Leave(object sender, EventArgs e)
        {
            int intPermit = 0;
            int.TryParse(txtPermitNo.Text.Trim(), out intPermit);

            if (intPermit == 0)
            {
                MessageBox.Show("Invalid permit no. format", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtPermitNo.Text = "";
                return;
            }

            txtPermitNo.Text = string.Format("{0:0000#}", intPermit);

            MappingClass.ValidatePermitNo();
            
        }

        private void txtRent_Leave(object sender, EventArgs e)
        {
            double dblRent = 0;

            double.TryParse(txtRent.Text.Trim(), out dblRent);

            txtRent.Text = string.Format("{0:#,###.00}", dblRent);
        }

        private void cmbPlaceOwnDist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPermitYear_Leave(object sender, EventArgs e)
        {
            int intPermit = 0;
            int.TryParse(txtPermitYear.Text.Trim(), out intPermit);

            if (intPermit == 0)
            {
                MessageBox.Show("Invalid permit year format", "Business Mapping", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtPermitYear.Text = "";
                return;
            }

            txtPermitYear.Text = string.Format("{0:000#}", intPermit);

            MappingClass.ValidatePermitYear();
        }

        public void LoadPin(string sBrgyCode)
        {
            OracleResultSet pGisRec = new OracleResultSet();
            pGisRec.CreateNewConnectionGIS();

            cmbLandPin.Items.Clear();

            pGisRec.Query = string.Format("select distinct (pin) from GIS_BUSINESS_LOCATION where substr(pin,8,3) = '{0}' order by pin", sBrgyCode);
            if (pGisRec.Execute())
            {
                while (pGisRec.Read())
                {
                    cmbLandPin.Items.Add(pGisRec.GetString("pin"));
                }
            }
            pGisRec.Close();

        }

        public void LoadBldgCode(string sLandPin)
        {
            OracleResultSet pGisRec = new OracleResultSet();
            pGisRec.CreateNewConnectionGIS();

            cmbBldgCode.Items.Clear();

            pGisRec.Query = string.Format("select * from GIS_BUSINESS_LOCATION where pin = '{0}' order by bldg_code", cmbLandPin.Text.Trim());
            if (pGisRec.Execute())
            {
                while (pGisRec.Read())
                {
                    cmbBldgCode.Items.Add(pGisRec.GetString("bldg_code"));
                }
            }
            pGisRec.Close();

            txtBldgName.Text = "";
        }

        

        
        private void chkChangeArea_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ChangeArea(sender, e);
        }

        private void chkChangeStoreys_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ChangeStoreys(sender, e);
        }

        private void txtArea_Leave(object sender, EventArgs e)
        {
            double dblArea = 0;

            double.TryParse(txtArea.Text.ToString(), out dblArea);
            txtArea.Text = string.Format("{0:#,###.00}", dblArea);
        }

        private void txtStoreys_Leave(object sender, EventArgs e)
        {
            int iStoreys = 0;

            int.TryParse(txtStoreys.Text.ToString(), out iStoreys);
            txtStoreys.Text = string.Format("{0:###}", iStoreys);

            txtOwnLn.Focus();
        }

        private void chkChangeTaxYear_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.ChangePermitYear(sender, e);
        }

        private void chkNotEncoded_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.NotEncoded(sender, e);
        }

        private void chkUndeclared_CheckStateChanged(object sender, EventArgs e)
        {
            MappingClass.Undeclared(sender, e);
        }

        private void btnUntag_Click(object sender, EventArgs e)
        {
            MappingClass.Untag();
        }

        private void cmbOwnBrgy_Leave(object sender, EventArgs e)
        {
            cmbOwnBrgy.Text = cmbOwnBrgy.Text.ToUpper();
        }

        private void cmbPlaceOwnBrgy_Leave(object sender, EventArgs e)
        {
            cmbPlaceOwnBrgy.Text = cmbPlaceOwnBrgy.Text.ToUpper();
        }

        private void txtOwnLn_Leave(object sender, EventArgs e)
        {
            if (txtOwnLn.Text.Trim() != "")
            {
                frmSearchOwner SearchOwner = new frmSearchOwner();
                SearchOwner.m_sPageWatch = "PAGE2"; 
                SearchOwner.OwnLastName = txtOwnLn.Text.Trim();
                SearchOwner.GetValues();    
                if (!SearchOwner.m_bNoRecordFound) 
                    SearchOwner.ShowDialog();

                if (SearchOwner.m_strOwnCode != "")
                {
                    txtOwnCode.Text = SearchOwner.m_strOwnCode;
                    txtOwnLn.Text = SearchOwner.m_sOwnLn;
                    txtOwnFn.Text = SearchOwner.m_sOwnFn;
                    txtOwnMi.Text = SearchOwner.m_sOwnMi;
                    txtOwnAddNo.Text = SearchOwner.m_sOwnAdd;
                    txtOwnStreet.Text = SearchOwner.m_sOwnStreet;
                    cmbOwnBrgy.Text = SearchOwner.m_sOwnBrgy;
                    cmbOwnDist.Text = SearchOwner.m_sOwnDist;
                    txtOwnMun.Text = SearchOwner.m_sOwnMun;
                    txtOwnProv.Text = SearchOwner.m_sOwnProv;
                    txtOwnZip.Text = SearchOwner.m_sOwnZip;

                }
                /*else
                {
                    txtOwnCode.Text = "";
                    txtOwnFn.Text = "";
                    txtOwnMi.Text = "";
                    txtOwnAddNo.Text = "";
                    txtOwnStreet.Text = "";
                    cmbOwnBrgy.Text = "";
                    cmbOwnDist.Text = "";
                    txtOwnMun.Text = "";
                    txtOwnProv.Text = "";
                    txtOwnZip.Text = "";

                }*/
            }
        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void cmbOwnDist_Leave(object sender, EventArgs e)
        {
            txtOwnMun.Focus();
        }

        private void txtOwnZip_Leave(object sender, EventArgs e)
        {
            cmbPlace.Focus();
        }

        
        
        
    }
}