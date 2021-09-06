////////////////////////////

// RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner

////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Common.AppSettings
{
    public class SearchAndReplaceClass
    {
        OracleResultSet result = new OracleResultSet();
        OracleResultSet result1 = new OracleResultSet();
        public bool bState = true;
        public string m_sBIN = string.Empty, m_sOwnCode = string.Empty, m_sLN = string.Empty, m_sFN = string.Empty, m_sMI = string.Empty;
        public string m_sAdd = string.Empty, m_sStreet = string.Empty, m_sBrgy = string.Empty, m_sZone = string.Empty;
        public string m_sDist = string.Empty, m_sMun = string.Empty, m_sProv = string.Empty;
        public string m_sZip = string.Empty;    // RMC 20110414

        public void EditOwner(string sBin)
        {
            result.Query = "select * from businesses where bin = :1";
            result.AddParameter(":1", sBin);
            if (result.Execute())
            {
                if (result.Read())
                {
                    m_sOwnCode = result.GetString("own_code");
                    result1.Query = "select * from own_names where own_code = :1";
                    result1.AddParameter(":1", m_sOwnCode.Trim());
                    if (result1.Execute())
                    {
                        if (result1.Read())
                        {
                            m_sAdd = result1.GetString("own_house_no");
                            m_sBrgy = result1.GetString("own_brgy");
                            m_sStreet = result1.GetString("own_street");
                            m_sProv = result1.GetString("own_prov");
                            m_sZone = result1.GetString("own_zone");
                            m_sMun = result1.GetString("own_mun");
                            m_sLN = result1.GetString("own_ln");
                            m_sFN = result1.GetString("own_fn");
                            m_sMI = result1.GetString("own_mi");
                            m_sZip = result1.GetString("own_zip");  // RMC 20110414
                        }
                    }
                    result1.Close();
                }
                else
                {
                    result.Close();
                    result.Query = "select * from business_que where bin = :1";
                    result.AddParameter(":1", sBin);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            m_sOwnCode = result.GetString("own_code");
                            result1.Query = "select * from own_names where own_code = :1";
                            result1.AddParameter(":1", m_sOwnCode.Trim());
                            if (result1.Execute())
                            {
                                if (result1.Read())
                                {
                                    m_sAdd = result1.GetString("own_house_no");
                                    m_sBrgy = result1.GetString("own_brgy");
                                    m_sStreet = result1.GetString("own_street");
                                    m_sProv = result1.GetString("own_prov");
                                    m_sZone = result1.GetString("own_zone");
                                    m_sMun = result1.GetString("own_mun");
                                    m_sLN = result1.GetString("own_ln");
                                    m_sFN = result1.GetString("own_fn");
                                    m_sMI = result1.GetString("own_mi");
                                    m_sZip = result1.GetString("own_zip");  // RMC 20110414

                                }
                                
                            }
                            result1.Close();
                        }
                        else
                        {
                            m_sOwnCode = string.Empty;
                            m_sLN = string.Empty;
                            m_sFN = string.Empty;
                            m_sMI = string.Empty;
                            m_sAdd = string.Empty;
                            m_sStreet = string.Empty;
                            m_sBrgy = string.Empty;
                            m_sZone = string.Empty;
                            m_sDist = string.Empty;
                            m_sMun = string.Empty;
                            m_sProv = string.Empty;
                            m_sZip = string.Empty;  // RMC 20110414
                            bState = false;
                        }
                    }
                    
                }
                
            }
            result.Close();

        }

        public void WindowState(int iState, bool bControlState)
        {
            
        }

        public void UpdateOwner()
        {
            //if (MessageBox.Show("Save edit owner's information of " + m_sBIN + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            if (MessageBox.Show("Save edited owner's information of Own code: " + m_sBIN + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)    // RMC 20110902
            {
                result.Query = "update own_names set own_ln = :1, own_fn = :2, own_mi = :3, own_house_no = :4, own_street = :5, own_dist = :6, own_zone = :7, own_brgy = :8, own_mun = :9, own_prov = :10, own_zip = :11 where own_code = :12";
                result.AddParameter(":1", StringUtilities.StringUtilities.HandleApostrophe(m_sLN)); // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":2", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sFN)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":3", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sMI)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":4", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sAdd)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":5", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sStreet)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":6", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sDist)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":7", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sZone)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":8", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sBrgy)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":9", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sMun)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":10", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sProv)));    // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":11", StringUtilities.StringUtilities.HandleApostrophe(StringUtilities.StringUtilities.SetEmptyToSpace(m_sZip))); // RMC 20110414  // RMC 20110825 added handleapostrophe and setemptytospace in S&R Edit owner
                result.AddParameter(":12", m_sOwnCode);
                if (result.Execute())
                {
                    MessageBox.Show("Owner Updated");
                }
                result.Close();
            }


        }

    }
}
