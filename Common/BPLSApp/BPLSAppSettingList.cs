using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.Message_Box;
using Amellar.Common.StringUtilities;

namespace Amellar.Common.BPLSApp
{
    public class BPLSAppSettingList
    {
        // RMC 20110311 with modifications

        public string m_sBin, sOwnName, sOwnLn, sOwnFn, sOwnMi, m_sOwnCode, m_sBnsCode, m_sMainBns;
        private string m_strModule = string.Empty;

        //public string m_sBin, m_sBnsNm, m_sBnsStreet, m_sTaxYear;
        //public string m_sBnsAddNo, m_sBnsBrgy, m_sBnsMun, m_sBnsProv, m_sDate;
        //public string m_sPermitNo;
        //public DateTime dtMPDate;
        public List<BPLSAppSetting> m_listBPLSApp;
        public List<BPLSAppSetting> m_ListOwnNames;
        OracleResultSet rQry = new OracleResultSet();
        public BPLSAppSettingList()
        {
            m_sBin = string.Empty;
            m_listBPLSApp = new List<BPLSAppSetting>();
            m_ListOwnNames = new List<BPLSAppSetting>();
            this.Reset();
        }

        public string ModuleCode
        {
            get { return m_strModule; }
            set { m_strModule = value; }
        }

        public void Reset()
        {
            m_listBPLSApp.Clear();
        }
        public string MainBns
        {
            get { return m_sBnsCode; }
            set
            {
                m_sBnsCode = value;
                rQry.Query = "SELECT * FROM BNS_TABLE WHERE FEES_CODE = 'B' AND BNS_CODE = '" + m_sBnsCode + "'";
                if(rQry.Execute())
                {
                    if(rQry.Read())
                    {
                        m_sMainBns = string.Empty;
                        m_sMainBns = rQry.GetString("bns_desc").Trim();
                    }
                }
                rQry.Close();
            }
        }
        public string OwnName
        {
            get { return m_sOwnCode; }
            set
            {
                m_sOwnCode = value;
                m_ListOwnNames.Clear();


                rQry.Query = "SELECT * FROM OWN_NAMES WHERE trim(OWN_CODE) = :1";   // RMC 20110414 added trim
                rQry.AddParameter(":1", m_sOwnCode.Trim()); // RMC 20110414 added trim
                if (rQry.Execute())
                {
                    if (rQry.Read())
                    {
                        /*m_ListOwnNames.Add(new BPLSAppSetting(rQry.GetString("own_ln").Trim(),
                            rQry.GetString("own_fn").Trim(), rQry.GetString("own_mi").Trim(),
                            rQry.GetString("own_house_no").Trim(), rQry.GetString("own_street").Trim(),
                            rQry.GetString("own_dist").Trim(), rQry.GetString("own_zone").Trim(),
                            rQry.GetString("own_brgy").Trim(), rQry.GetString("own_mun").Trim(),
                            rQry.GetString("own_prov").Trim(), rQry.GetString("own_zip").Trim(),
                            rQry.GetString("own_code").Trim()));    // RMC 20110414 added owner code*/

                        m_ListOwnNames.Add(new BPLSAppSetting(StringUtilities.StringUtilities.RemoveApostrophe(rQry.GetString("own_ln").Trim()),
                            StringUtilities.StringUtilities.RemoveApostrophe(rQry.GetString("own_fn")), rQry.GetString("own_mi"),
                            rQry.GetString("own_house_no"), rQry.GetString("own_street"),
                            rQry.GetString("own_dist"), rQry.GetString("own_zone"),
                            rQry.GetString("own_brgy"), rQry.GetString("own_mun"),
                            rQry.GetString("own_prov"), rQry.GetString("own_zip"),
                            rQry.GetString("own_code")));    // RMC 20110801 Deleted Trim()
                    }
                }
                rQry.Close();

            }
        }

        public string ReturnValueByBin
        {
            get { return m_sBin; }
            set
            {
                m_sBin = value;
                m_listBPLSApp.Clear();

                OracleResultSet pSet = new OracleResultSet();

                if (m_strModule == "NEW-APP-EDIT" || m_strModule == "NEW-APP-VIEW" || m_strModule == "REN-APP-EDIT"
                    || m_strModule == "REN-APP-VIEW"  || m_strModule == "CANCEL-APP")
                    pSet.Query = "SELECT * FROM BUSINESS_QUE WHERE BIN = :1";
                else if (m_strModule == "SPL-APP-EDIT" || m_strModule == "SPL-APP-VIEW")
                    pSet.Query = "SELECT * FROM spl_business_que WHERE BIN = :1";
                else
                    pSet.Query = "SELECT * FROM BUSINESSES WHERE BIN = :1";
                    pSet.AddParameter(":1", m_sBin);
                    if (pSet.Execute())
                    {
                        if (pSet.Read())
                        {
                            m_listBPLSApp.Add(new BPLSAppSetting(
                            pSet.GetString("BNS_NM").Trim(),
                            pSet.GetString("BNS_STAT").Trim(),
                            pSet.GetString("OWN_CODE").Trim(),
                            pSet.GetString("BNS_TELNO").Trim(),
                            pSet.GetString("BNS_HOUSE_NO").Trim(),
                            pSet.GetString("BNS_STREET").Trim(),
                            pSet.GetString("BNS_MUN").Trim(),
                            pSet.GetString("BNS_DIST").Trim(),
                            pSet.GetString("BNS_ZONE").Trim(),
                            pSet.GetString("BNS_BRGY").Trim(),
                            pSet.GetString("BNS_PROV").Trim(),
                            pSet.GetString("BNS_ZIP").Trim(),
                            pSet.GetString("LAND_PIN").Trim(),
                            pSet.GetString("BLDG_PIN").Trim(),
                            pSet.GetString("MACH_PIN").Trim(),
                            pSet.GetString("POFF_HOUSE_NO").Trim(),
                            pSet.GetString("POFF_STREET").Trim(),
                            pSet.GetString("POFF_MUN").Trim(),
                            pSet.GetString("POFF_DIST").Trim(),
                            pSet.GetString("POFF_ZONE").Trim(),
                            pSet.GetString("POFF_BRGY").Trim(),
                            pSet.GetString("POFF_PROV").Trim(),
                            pSet.GetString("POFF_ZIP").Trim(),
                            pSet.GetString("ORGN_KIND").Trim(),
                            pSet.GetString("BUSN_OWN").Trim(),
                            pSet.GetString("CTC_NO").Trim(),
                            pSet.GetDateTime("CTC_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                            pSet.GetString("CTC_ISSUED_AT").Trim(),
                            pSet.GetString("BNS_CODE").Trim(),
                            pSet.GetString("SSS_NO").Trim(),
                            pSet.GetDateTime("SSS_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                            pSet.GetString("DTI_REG_NO").Trim(),
                            pSet.GetDateTime("DTI_REG_DT").ToShortDateString(), // GDE 20110201 
                            pSet.GetDouble("BLDG_VAL"),  // RMC 20110311 from string to double
                            pSet.GetString("PLACE_OCCUPANCY").Trim(),
                            pSet.GetDateTime("RENT_LEASE_SINCE").ToShortDateString(), // GDE 20110201 
                            pSet.GetDouble("RENT_LEASE_MO"), // RMC 20110311 from string to double
                            pSet.GetDouble("FLR_AREA"),  // RMC 20110311 from string to double
                            pSet.GetInt("NUM_STOREYS").ToString(), // RMC 20110801 from string to int
                            pSet.GetDouble("TOT_FLR_AREA"),  // RMC 20110311 from string to double
                            pSet.GetString("PREV_BNS_OWN").Trim(),
                            pSet.GetInt("NUM_EMPLOYEES").ToString(),    // RMC 20110801 from string to int
                            pSet.GetInt("NUM_PROFESSIONAL").ToString(),  // RMC 20110801 from string to int
                            pSet.GetDouble("ANNUAL_WAGES"),  // RMC 20110311 from string to double
                            pSet.GetDouble("AVE_ELECTRIC_BILL"), // RMC 20110311 from string to double
                            pSet.GetDouble("AVE_WATER_BILL"),    // RMC 20110311 from string to double
                            pSet.GetDouble("AVE_PHONE_BILL"),    // RMC 20110311 from string to double
                            pSet.GetDouble("OTHER_UTIL"),    // RMC 20110311 from string to double
                            pSet.GetInt("NUM_DELIV_VEHICLE").ToString(),    // RMC 20110801 from string to int
                            pSet.GetInt("NUM_MACHINERIES").ToString(),   // RMC 20110801 from string to int
                            pSet.GetDateTime("DT_OPERATED").ToShortDateString(), // GDE 20110201
                            pSet.GetString("PERMIT_NO").Trim(),
                            pSet.GetDateTime("PERMIT_DT").ToShortDateString(), // GDE 20110201
                            pSet.GetDouble("GR_1"),  // RMC 20110311 from string to double
                            pSet.GetDouble("GR_2"),  // RMC 20110311 from string to double
                            pSet.GetDouble("CAPITAL"),   // RMC 20110311 from string to double
                            pSet.GetString("OR_NO").Trim(),
                            pSet.GetString("TAX_YEAR").Trim(),
                            pSet.GetString("CANC_REASON").Trim(),
                            pSet.GetDateTime("CANC_DATE").ToShortDateString(), // GDE 20110201
                            pSet.GetString("CANC_BY").Trim(),
                            pSet.GetString("BNS_USER").Trim(),
                            pSet.GetDateTime("SAVE_TM").ToShortDateString(), // GDE 20110201
                            pSet.GetString("MEMORANDA").Trim(),
                            pSet.GetString("bns_email").Trim(), // RMC 20110803
                            pSet.GetString("tin_no").Trim(),    // RMC 20110803
                            pSet.GetDateTime("tin_issued_on").ToShortDateString(),  // RMC 20110803
                            pSet.GetDateTime("dt_applied").ToShortDateString()));    // RMC 20110803

                            //m_listBPLSApp.Add(new BPLSAppSetting(pSet.GetString("bns_nm").Trim(), pSet.GetString("bns_street").Trim()));
                        }
                    }
                    pSet.Close();
                
                

            }
        }
        
        public List<BPLSAppSetting> BPLSAppSettings
        {
            get { return m_listBPLSApp; }
            set { m_listBPLSApp = value; }
        }
        public List<BPLSAppSetting> OwnNamesSetting
        {
            get { return m_ListOwnNames; }
            set { m_ListOwnNames = value; }
        }
        public BPLSAppSetting GetInfoByBIN(string sBin)
        {
            int iCount = m_listBPLSApp.Count;
            for (int i = 0; i <= iCount; i++)
            {
                if (m_listBPLSApp[i].sBnsNm == sBin)
                    return m_listBPLSApp[i];
            }
            return new BPLSAppSetting();
        }

    
        public bool StringCheck(string sString)
        {
                   
            if(sString != string.Empty)
            {
                return true;
            }
            else
            {
               return false;
            }
        }
        public void PageOneCheck()
        {
            /*
            bool bAns = false;
            
            if ((bAns = sList.StringCheck(txtTaxYear.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtTaxYear.Focus();
                MessageBox.Show("Tax Year Required!");
                return;
            }
            if ((bAns = sList.StringCheck(txtMPNo.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtMPNo.Focus();
                MessageBox.Show("Mayor's Permit Required!");
                return;
            }
            if ((bAns = sList.StringCheck(txtBnsName.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtBnsName.Focus();
                MessageBox.Show("Business Name Required!");
                return;
            }
            if ((bAns = sList.StringCheck(txtBnsStreet.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtBnsStreet.Focus();
                MessageBox.Show("Business Street Required!");
                return;
            }
            if ((bAns = sList.StringCheck(cmbBnsBrgy.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                cmbBnsBrgy.Focus();
                MessageBox.Show("Business Barangay Required!");
                return;
            }
            if ((bAns = sList.StringCheck(cmbBnsOrgnKind.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                cmbBnsOrgnKind.Focus();
                MessageBox.Show("Business Organization Required!");
                return;
            }
            if ((bAns = sList.StringCheck(txtBnsType.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtBnsType.Focus();
                MessageBox.Show("Business Type Required!");
                return;
            }
            if ((bAns = sList.StringCheck(txtOwnLn.Text)) == false)
            {
                tabBnsRec.SelectTab(tabPage1);
                txtOwnLn.Focus();
                MessageBox.Show("Owner's Last Name Required!");
                return;
            }
             */



        }
        public string ReturnValuesByBinQue
        {
            get { return m_sBin; }
            set
            {
                m_sBin = value;
                m_listBPLSApp.Clear();

                OracleResultSet pSet = new OracleResultSet();

                bool blnIsSplBns = false;
                pSet.Query = "select * from spl_business_que where bin  = :1";
                pSet.AddParameter(":1", m_sBin);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        blnIsSplBns = true;
                    }
                    else
                    {
                        pSet.Close();
                        pSet.Query = "select * from spl_businesses where bin  = :1";
                        pSet.AddParameter(":1", m_sBin);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            { blnIsSplBns = true; }
                        }
                    }
                }
                pSet.Close();

                if(blnIsSplBns)
                    pSet.Query = "SELECT * FROM SPL_BUSINESS_QUE WHERE BIN = :1";
                else
                    pSet.Query = "SELECT * FROM BUSINESS_QUE WHERE BIN = :1";
                pSet.AddParameter(":1", m_sBin);
                if (pSet.Execute())
                {
                    if (pSet.Read())
                    {
                        m_listBPLSApp.Add(new BPLSAppSetting(
                                pSet.GetString("BNS_NM").Trim(),
                                pSet.GetString("BNS_STAT").Trim(),
                                pSet.GetString("OWN_CODE").Trim(),
                                pSet.GetString("BNS_TELNO").Trim(),
                                pSet.GetString("BNS_HOUSE_NO").Trim(),
                                pSet.GetString("BNS_STREET").Trim(),
                                pSet.GetString("BNS_MUN").Trim(),
                                pSet.GetString("BNS_DIST").Trim(),
                                pSet.GetString("BNS_ZONE").Trim(),
                                pSet.GetString("BNS_BRGY").Trim(),
                                pSet.GetString("BNS_PROV").Trim(),
                                pSet.GetString("BNS_ZIP").Trim(),
                                pSet.GetString("LAND_PIN").Trim(),
                                pSet.GetString("BLDG_PIN").Trim(),
                                pSet.GetString("MACH_PIN").Trim(),
                                pSet.GetString("POFF_HOUSE_NO").Trim(),
                                pSet.GetString("POFF_STREET").Trim(),
                                pSet.GetString("POFF_MUN").Trim(),
                                pSet.GetString("POFF_DIST").Trim(),
                                pSet.GetString("POFF_ZONE").Trim(),
                                pSet.GetString("POFF_BRGY").Trim(),
                                pSet.GetString("POFF_PROV").Trim(),
                                pSet.GetString("POFF_ZIP").Trim(),
                                pSet.GetString("ORGN_KIND").Trim(),
                                pSet.GetString("BUSN_OWN").Trim(),
                                pSet.GetString("CTC_NO").Trim(),
                                pSet.GetDateTime("CTC_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                                pSet.GetString("CTC_ISSUED_AT").Trim(),
                                pSet.GetString("BNS_CODE").Trim(),
                                pSet.GetString("SSS_NO").Trim(),
                                pSet.GetDateTime("SSS_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                                pSet.GetString("DTI_REG_NO").Trim(),
                                pSet.GetDateTime("DTI_REG_DT").ToShortDateString(), // GDE 20110201 
                                pSet.GetDouble("BLDG_VAL"),  // RMC 20110311 from string to double
                                pSet.GetString("PLACE_OCCUPANCY").Trim(),
                                pSet.GetDateTime("RENT_LEASE_SINCE").ToShortDateString(), // GDE 20110201 
                                pSet.GetDouble("RENT_LEASE_MO"), // RMC 20110311 from string to double
                                pSet.GetDouble("FLR_AREA"),  // RMC 20110311 from string to double
                                pSet.GetString("NUM_STOREYS").Trim(),
                                pSet.GetDouble("TOT_FLR_AREA"),  // RMC 20110311 from string to double
                                pSet.GetString("PREV_BNS_OWN").Trim(),
                                pSet.GetString("NUM_EMPLOYEES").Trim(),
                                pSet.GetString("NUM_PROFESSIONAL").Trim(),
                                pSet.GetDouble("ANNUAL_WAGES"),  // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_ELECTRIC_BILL"), // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_WATER_BILL"),    // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_PHONE_BILL"),    // RMC 20110311 from string to double
                                pSet.GetDouble("OTHER_UTIL"),    // RMC 20110311 from string to double
                                pSet.GetString("NUM_DELIV_VEHICLE").Trim(),
                                pSet.GetString("NUM_MACHINERIES").Trim(),
                                pSet.GetDateTime("DT_OPERATED").ToShortDateString(), // GDE 20110201
                                pSet.GetString("PERMIT_NO").Trim(),
                                pSet.GetDateTime("PERMIT_DT").ToShortDateString(), // GDE 20110201
                                pSet.GetDouble("GR_1"),  // RMC 20110311 from string to double
                                pSet.GetDouble("GR_2"),  // RMC 20110311 from string to double
                                pSet.GetDouble("CAPITAL"),   // RMC 20110311 from string to double
                                pSet.GetString("OR_NO").Trim(),
                                pSet.GetString("TAX_YEAR").Trim(),
                                pSet.GetString("CANC_REASON").Trim(),
                                pSet.GetDateTime("CANC_DATE").ToShortDateString(), // GDE 20110201
                                pSet.GetString("CANC_BY").Trim(),
                                pSet.GetString("BNS_USER").Trim(),
                                pSet.GetDateTime("SAVE_TM").ToShortDateString(), // GDE 20110201
                                pSet.GetString("MEMORANDA").Trim(),
                                pSet.GetString("bns_email").Trim(), // RMC 20110803
                                pSet.GetString("tin_no").Trim(),    // RMC 20110803
                                pSet.GetDateTime("tin_issued_on").ToShortDateString(),  // RMC 20110803
                                pSet.GetDateTime("dt_applied").ToShortDateString()));    // RMC 20110803

                        //m_listBPLSApp.Add(new BPLSAppSetting(pSet.GetString("bns_nm").Trim(), pSet.GetString("bns_street").Trim()));
                    }
                    else
                    {
                        pSet.Close();
                        pSet.Query = "SELECT * FROM BUSINESSES WHERE BIN = :1";
                        pSet.AddParameter(":1", m_sBin);
                        if (pSet.Execute())
                        {
                            if (pSet.Read())
                            {
                                m_listBPLSApp.Add(new BPLSAppSetting(
                                pSet.GetString("BNS_NM").Trim(),
                                pSet.GetString("BNS_STAT").Trim(),
                                pSet.GetString("OWN_CODE").Trim(),
                                pSet.GetString("BNS_TELNO").Trim(),
                                pSet.GetString("BNS_HOUSE_NO").Trim(),
                                pSet.GetString("BNS_STREET").Trim(),
                                pSet.GetString("BNS_MUN").Trim(),
                                pSet.GetString("BNS_DIST").Trim(),
                                pSet.GetString("BNS_ZONE").Trim(),
                                pSet.GetString("BNS_BRGY").Trim(),
                                pSet.GetString("BNS_PROV").Trim(),
                                pSet.GetString("BNS_ZIP").Trim(),
                                pSet.GetString("LAND_PIN").Trim(),
                                pSet.GetString("BLDG_PIN").Trim(),
                                pSet.GetString("MACH_PIN").Trim(),
                                pSet.GetString("POFF_HOUSE_NO").Trim(),
                                pSet.GetString("POFF_STREET").Trim(),
                                pSet.GetString("POFF_MUN").Trim(),
                                pSet.GetString("POFF_DIST").Trim(),
                                pSet.GetString("POFF_ZONE").Trim(),
                                pSet.GetString("POFF_BRGY").Trim(),
                                pSet.GetString("POFF_PROV").Trim(),
                                pSet.GetString("POFF_ZIP").Trim(),
                                pSet.GetString("ORGN_KIND").Trim(),
                                pSet.GetString("BUSN_OWN").Trim(),
                                pSet.GetString("CTC_NO").Trim(),
                                pSet.GetDateTime("CTC_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                                pSet.GetString("CTC_ISSUED_AT").Trim(),
                                pSet.GetString("BNS_CODE").Trim(),
                                pSet.GetString("SSS_NO").Trim(),
                                pSet.GetDateTime("SSS_ISSUED_ON").ToShortDateString(), // GDE 20110201 
                                pSet.GetString("DTI_REG_NO").Trim(),
                                pSet.GetDateTime("DTI_REG_DT").ToShortDateString(), // GDE 20110201 
                                pSet.GetDouble("BLDG_VAL"),  // RMC 20110311 from string to double
                                pSet.GetString("PLACE_OCCUPANCY").Trim(),
                                pSet.GetDateTime("RENT_LEASE_SINCE").ToShortDateString(), // GDE 20110201 
                                pSet.GetDouble("RENT_LEASE_MO"), // RMC 20110311 from string to double
                                pSet.GetDouble("FLR_AREA"),  // RMC 20110311 from string to double
                                pSet.GetString("NUM_STOREYS").Trim(),
                                pSet.GetDouble("TOT_FLR_AREA"),  // RMC 20110311 from string to double
                                pSet.GetString("PREV_BNS_OWN").Trim(),
                                pSet.GetString("NUM_EMPLOYEES").Trim(),
                                pSet.GetString("NUM_PROFESSIONAL").Trim(),
                                pSet.GetDouble("ANNUAL_WAGES"),  // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_ELECTRIC_BILL"), // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_WATER_BILL"),    // RMC 20110311 from string to double
                                pSet.GetDouble("AVE_PHONE_BILL"),    // RMC 20110311 from string to double
                                pSet.GetDouble("OTHER_UTIL"),    // RMC 20110311 from string to double
                                pSet.GetString("NUM_DELIV_VEHICLE").Trim(),
                                pSet.GetString("NUM_MACHINERIES").Trim(),
                                pSet.GetDateTime("DT_OPERATED").ToShortDateString(), // GDE 20110201
                                pSet.GetString("PERMIT_NO").Trim(),
                                pSet.GetDateTime("PERMIT_DT").ToShortDateString(), // GDE 20110201
                                pSet.GetDouble("GR_1"),  // RMC 20110311 from string to double
                                pSet.GetDouble("GR_2"),  // RMC 20110311 from string to double
                                pSet.GetDouble("CAPITAL"),   // RMC 20110311 from string to double
                                pSet.GetString("OR_NO").Trim(),
                                pSet.GetString("TAX_YEAR").Trim(),
                                pSet.GetString("CANC_REASON").Trim(),
                                pSet.GetDateTime("CANC_DATE").ToShortDateString(), // GDE 20110201
                                pSet.GetString("CANC_BY").Trim(),
                                pSet.GetString("BNS_USER").Trim(),
                                pSet.GetDateTime("SAVE_TM").ToShortDateString(), // GDE 20110201
                                pSet.GetString("MEMORANDA").Trim(),
                                pSet.GetString("bns_email").Trim(), // RMC 20110803
                                pSet.GetString("tin_no").Trim(),    // RMC 20110803
                                pSet.GetDateTime("tin_issued_on").ToShortDateString(),  // RMC 20110803
                                pSet.GetDateTime("dt_applied").ToShortDateString()));    // RMC 20110803));

                                //m_listBPLSApp.Add(new BPLSAppSetting(pSet.GetString("bns_nm").Trim(), pSet.GetString("bns_street").Trim()));
                            }
                        }
                    }
                }
                pSet.Close();



            }
        }
    }
}
        