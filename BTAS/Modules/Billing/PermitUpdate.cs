using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Windows.Forms;
using System.Data;

namespace Amellar.BPLS.Billing
{
    class PermitUpdate:Billing
    {
        public PermitUpdate(frmBilling Form) : base(Form)
        { }

        public override void ReturnValues()
        {
            m_bPermitUpdateFlag = false;    // RMC 20140725 Added Permit update billing
            string strApplType = string.Empty;
            m_blnNewAddl = false;
            m_blnTransLoc = false;
            m_blnTransOwn = false;
            m_blnChClass = false;
            m_blnChBnsName = false;
            m_blnChOrgKind = false;
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select distinct appl_type, dt_operated from permit_update_appl where bin = '{0}' and data_mode = 'QUE'", BillingForm.BIN);
            if (result.Execute())
            {
                while (result.Read())
                {
                    m_bPermitUpdateFlag = true; // RMC 20140725 Added Permit update billing
                    strApplType = result.GetString("appl_type").Trim();
                    m_sDtOperated = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("dt_operated"));
                    BillingForm.Quarter = AppSettingsManager.GetQtr(m_sDtOperated);
                    if (strApplType == "ADDL")
                        m_blnNewAddl = true;
                    if (strApplType == "TLOC")
                        m_blnTransLoc = true;
                    if (strApplType == "TOWN")
                        m_blnTransOwn = true;
                    if (strApplType == "CTYP")
                        m_blnChClass = true;                    
                    if (strApplType == "CBNS")
                        m_blnChBnsName = true;
                    // RMC 20140725 Added Permit update billing (s)
                    if (strApplType == "CORG")    
                        m_blnChOrgKind = true;
                    // RMC 20140725 Added Permit update billing (e)

                    m_sPUTApplType = strApplType;   // RMC 20150108
                }
            }
            result.Close();

            if (m_blnNewAddl == true || m_blnTransLoc == true || m_blnTransOwn == true || m_blnChClass == true || m_blnChBnsName == true
                || m_blnChOrgKind == true)  // RMC 20140725 Added Permit update billing
            {
                result = new OracleResultSet();
                int iCnt = 0;

                result.Query = string.Format("select count(*) from business_que where bin = '{0}'", BillingForm.BIN);
                int.TryParse(result.ExecuteScalar(), out iCnt);

                if (iCnt > 0)
                {
                    result.Query = string.Format("select * from business_que where bin = '{0}'", BillingForm.BIN);
                    m_bIsRenPUT = true;
                }
                else
                {
                    result.Query = string.Format("select * from businesses where bin = '{0}'", BillingForm.BIN);
                    m_bIsRenPUT = false;
                }
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        // RMC 20150108 (s)
                        m_sMainBnsStat = result.GetString("bns_stat");
                        // RMC 20150108 (e)

                        if(!m_bIsRenPUT)
                            m_sDueState = "P";
                        else
                        {// RMC 20150108 (s)
                            if (m_sMainBnsStat == "NEW")
                            {
                                m_sDueState = "N";
                                m_sDtOperated = string.Format("{0:MM/dd/yyyy}", result.GetDateTime("dt_operated"));
                                BillingForm.Quarter = AppSettingsManager.GetQtr(m_sDtOperated); // RMC 20160517 correction in permit-update billing with renewal application
                            }
                            else
                            {
                                if (m_sMainBnsStat == "REN")
                                    m_sDueState = "R";
                                else
                                    m_sDueState = "X";

                                BillingForm.Quarter = "1";  // RMC 20160517 correction in permit-update billing with renewal application
                            }
                        }// RMC 20150108 (e)

                        BillingForm.BillFlag = false;
                        //BillingForm.Quarter = "1";
                        BillingForm.TaxYear = result.GetString("tax_year");
                        BillingForm.BusinessName = result.GetString("bns_nm").Trim();
                        m_sOwnCode = result.GetString("own_code").Trim();
                        BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);
                        m_sMainBnsCode = result.GetString("bns_code").Trim();
                        m_fMainCapital = result.GetDouble("capital");
                        m_fMainGross = result.GetDouble("gr_1");
                        BillingForm.Status = m_sMainBnsStat;
                        BillingForm.BusinessCode = m_sMainBnsCode;
                        BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
                        BillingForm.Gross = string.Format("{0:#,##0.00}", m_fMainGross);
                        GetMinMaxDueYear(BillingForm.BIN, out m_sBaseDueYear, out m_sMaxDueYear);
                        m_sCurrentYear = m_sMaxDueYear;
                        PopulateBnsType();
                        PopulateGrossInfo(BillingForm.Status, m_sDueState, BillingForm.TaxYear, BillingForm.Quarter, BillingForm.TaxYear, BillingForm.Quarter, "REG");
                        LoadAddlInfo();
                        LoadGrossCapital();
                        AddlInfoReadOnly(true);
                        m_bIsInitLoad = false;
                        BillingForm.EnabledControls(true);
                        BillingForm.TaxYearTextBox.ReadOnly = false;
                    }
                }
                result.Close();


                if (m_blnChClass)
                {
                    result = new OracleResultSet();
                    result.Query = "select * from change_class_tbl";
                    result.Query += " where bin = '" + BillingForm.BIN + "'";
                    result.Query += " and is_main = 'Y'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sMainBnsCode = result.GetString("new_bns_code");
                            BillingForm.BusinessCode = m_sMainBnsCode;
                            PopulateBnsType();
                            m_fMainCapital = result.GetDouble("capital");
                            BillingForm.Capital = string.Format("{0:#,##0.00}", m_fMainCapital);
                            if (!m_bIsRenPUT)   // RMC 20160517 correction in permit-update billing with renewal application
                                BillingForm.Quarter = AppSettingsManager.GetQtr(m_sDtOperated);
                        }
                    }
                    result.Close();
                }

                if (m_blnTransOwn)
                {
                    result = new OracleResultSet();
                    result.Query = "select new_own_code,old_own_code from permit_update_appl";
                    result.Query += " where bin = '" + BillingForm.BIN + "' and data_mode = 'QUE'";
                    result.Query += " and appl_type = 'TOWN'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sOwnCode = result.GetString("new_own_code");
                            BillingForm.OwnersName = LibraryClass.GetOwnerName(2, m_sOwnCode);
                            m_sPrevOwnCode = result.GetString("old_own_code");
                        }
                    }
                    result.Close();
                }

                if (m_blnTransLoc)
                {
                    result = new OracleResultSet();
                    result.Query = "select new_bns_loc,old_bns_loc from permit_update_appl";
                    result.Query += " where bin = '" + BillingForm.BIN + "' and data_mode = 'QUE'";
                    result.Query += " and appl_type = 'TLOC'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sNewBnsLoc = result.GetString("new_bns_loc");
                            m_sPrevBnsLoc = result.GetString("old_bns_loc");
                        }
                    }
                    result.Close();
                }

                if (m_blnChBnsName)
                {
                    result = new OracleResultSet();
                    result.Query = "select new_bns_name,old_bns_name from permit_update_appl";
                    result.Query += " where bin = '" + BillingForm.BIN + "' and data_mode = 'QUE'";
                    result.Query += " and appl_type = 'CBNS'";
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            BillingForm.BusinessName = result.GetString("new_bns_name");
                            m_sOldBnsName = result.GetString("old_bns_name");
                        }
                    }
                    result.Close();
                }

                BillingForm.btnRetrieveBilling.Enabled = true;
                /*BillingForm.btnEditAddlInfo.Enabled = false;
                if (BillingForm.Status == "NEW")
                    BillingForm.btnEditAddlInfo.Enabled = false;
                else
                    BillingForm.btnEditAddlInfo.Enabled = true;*/
                // RMC 20150325 modified billing for permit update application, put rem
            }
            else
            {
                MessageBox.Show("No permit update application found.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result.Close();
                return;
            }
        }
    }
}
