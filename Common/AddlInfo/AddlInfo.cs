

// RMC 20110810 added capturing of addl info value to business record

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using System.Windows.Forms; // RMC 20110809

namespace Amellar.Common.AddlInfo
{
    /// <summary>
    /// ALJ 20090710
    /// Additional Information Class
    /// </summary>
    public class AddlInfo
    {
        private string m_sBnsCode, m_sBIN, m_sTaxYear;
        private string m_sNumberOfEmployee, m_sBusinessArea, m_sVehicle;    // RMC 20110810 added capturing of addl info value to business record

        public AddlInfo()
        {
        }

        public string BusinessCode
        {
            set { m_sBnsCode = value; }
        }

        public string BIN
        {
            set { m_sBIN = value; }
        }

        public string TaxYear
        {
            set { m_sTaxYear = value; }
        }

        public string BusinessArea  // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sBusinessArea; }
            set { m_sBusinessArea = value; }
            
        }

        public string EmployeeNo    // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sNumberOfEmployee; }
            set { m_sNumberOfEmployee = value; }
        }

        public string VehicleNo    // RMC 20110810 added capturing of addl info value to business record
        {
            get { return m_sVehicle; }
            set { m_sVehicle = value; }
        }
        /// <summary>
        /// ALJ 20090710
        /// populate the addl info grid using DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetAddlInfo()
        {
            
                OracleResultSet pSet = new OracleResultSet();
                DataTable dataTable = new DataTable();
                string sDefaultCode, sDefaultDesc, sDataType, sUnit;
                sUnit = string.Empty;

                // RMC 20110810 added capturing of addl info value to business record (s)
                m_sNumberOfEmployee = "";
                m_sBusinessArea = "";
                m_sVehicle = "";
                // RMC 20110810 added capturing of addl info value to business record (e)

                dataTable.Columns.Add("Code", typeof(String));
                dataTable.Columns.Add("Description", typeof(String));
                dataTable.Columns.Add("Type", typeof(String));
                dataTable.Columns.Add("Unit", typeof(String));
                try
                {
                //pSet.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where a.default_code = b.default_code and a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2 order by a.default_code";    // RMC 20111124 added ordering in addl_info // GDE 20130621
                pSet.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2 order by a.default_code";
                //pSet.Query = "select distinct (a.default_code), a.default_desc, a.data_type  from default_code a, default_others b where a.rev_year = b.rev_year and b.default_fee like :1 and a.rev_year = :2 and a.default_code = b.default_code order by a.default_code";    // RMC 20140107 adjusted viewing of default fees in business record addl info
                pSet.AddParameter(":1", m_sBnsCode.Substring(0, 2) + "%");
                //pSet.AddParameter(":1", m_sBnsCode);
                pSet.AddParameter(":2", ConfigurationAttributes.RevYear);
                if (pSet.Execute())
                {
                    OracleResultSet pSetOtherInfo = new OracleResultSet();
                    while (pSet.Read())
                    {
                        sDefaultCode = pSet.GetString("default_code").Trim();
                        sDefaultDesc = pSet.GetString("default_desc").Trim();
                        sDataType = pSet.GetString("data_type").Trim();
                        pSetOtherInfo.Query = "select data from other_info where bin = :1 and tax_year = :2 and bns_code = :3 and default_code = :4 and rev_year = :5";
                        pSetOtherInfo.AddParameter(":1", m_sBIN);
                        pSetOtherInfo.AddParameter(":2", m_sTaxYear);
                        pSetOtherInfo.AddParameter(":3", m_sBnsCode);
                        pSetOtherInfo.AddParameter(":4", sDefaultCode);
                        pSetOtherInfo.AddParameter(":5", ConfigurationAttributes.RevYear);
                        if (pSetOtherInfo.Execute())
                        {
                            if (pSetOtherInfo.Read())
                            {
                                if (sDataType == "A" || sDataType == "AR" || sDataType == "RR")
                                    sUnit = string.Format("{0:#,##0.00}", pSetOtherInfo.GetDouble("data"));
                                else
                                    sUnit = string.Format("{0:#,###}", pSetOtherInfo.GetDouble("data"));
                            }
                            else
                                sUnit = string.Empty;
                        }

                        dataTable.Rows.Add(sDefaultCode, sDefaultDesc, sDataType, sUnit);

                        // RMC 20110810 added capturing of addl info value to business record (s)
                        if (sDefaultDesc.Contains("NUMBER OF EMPLYOYEE") || sDefaultDesc.Contains("EMPLOYEE") || sDefaultDesc.Contains("NUMBER OF WORKERS") || sDefaultDesc.Contains("WORKERS"))  // RMC 20140106 corrected capturing of number of workers in permit
                            m_sNumberOfEmployee = sUnit;
                        if (sDefaultDesc.Contains("AREA"))
                            m_sBusinessArea = sUnit;
                        if (sDefaultDesc.Contains("DELIVERY"))
                            m_sVehicle = sUnit;
                        // RMC 20110810 added capturing of addl info value to business record (e)

                    }
                    pSetOtherInfo.Close();
                }
                pSet.Close();

               
            }
            catch (Exception a)
            { MessageBox.Show(a.ToString(), " ", MessageBoxButtons.OK, MessageBoxIcon.Stop); }
            return dataTable;
            
        }

        public void UpdateAddlInfo(DataTable p_dataTable)
        {
            OracleResultSet pCmd = new OracleResultSet();
            string sDefaultCode, sDefaultDesc, sDataType, sUnit;

            // RMC 20110810 added capturing of addl info value to business record (s)
            m_sNumberOfEmployee = "";
            m_sBusinessArea = "";
            m_sVehicle = "";
            // RMC 20110810 added capturing of addl info value to business record (e)

            double dData;
            int i, iRows;
            iRows = p_dataTable.Rows.Count;
            pCmd.Query = "DELETE FROM other_info WHERE bin = :1 AND tax_year = :2 AND bns_code = :3 AND rev_year = :4";
            pCmd.AddParameter(":1", m_sBIN);
            pCmd.AddParameter(":2", m_sTaxYear);
            pCmd.AddParameter(":3", m_sBnsCode);
            pCmd.AddParameter(":4", ConfigurationAttributes.RevYear);
            pCmd.ExecuteNonQuery();
            for (i = 0; i < iRows; ++i)
            {
                sDefaultCode = p_dataTable.Rows[i][0].ToString();
                sDefaultDesc = p_dataTable.Rows[i][1].ToString();
                sDataType    = p_dataTable.Rows[i][2].ToString();
                sUnit        = p_dataTable.Rows[i][3].ToString();
                double.TryParse(sUnit, out dData);
                if (dData >= 0)
                {
                    pCmd.Query = "INSERT INTO other_info VALUES (:1,:2,:3,:4,:5,:6,:7)";
                    pCmd.AddParameter(":1", m_sBIN);
                    pCmd.AddParameter(":2", m_sTaxYear);
                    pCmd.AddParameter(":3", m_sBnsCode);
                    pCmd.AddParameter(":4", sDefaultCode);
                    pCmd.AddParameter(":5", sDataType);
                    pCmd.AddParameter(":6", dData);
                    pCmd.AddParameter(":7", ConfigurationAttributes.RevYear);
                    pCmd.ExecuteNonQuery();
                }

                // RMC 20110810 added capturing of addl info value to business record (s)
                //if (sDefaultDesc.Contains("NUMBER OF EMPLYOYEE") || sDefaultDesc.Contains("EMPLOYEE") || sDefaultDesc.Contains("NUMBER OF WORKERS") || sDefaultDesc.Contains("WORKERS"))  // RMC 20140106 corrected capturing of number of workers in permit
                if ((sDefaultDesc.Contains("NUMBER OF EMPLYOYEE") || sDefaultDesc.Contains("EMPLOYEE") || sDefaultDesc.Contains("NUMBER OF WORKERS") || sDefaultDesc.Contains("WORKERS")) && !sDefaultDesc.Contains("FEE"))  // MCR 20141119 corrected capturing of number of workers in permit
                    m_sNumberOfEmployee = sUnit;
                if (sDefaultDesc.Contains("AREA"))
                    m_sBusinessArea = sUnit;
                if (sDefaultDesc.Contains("DELIVERY"))
                    m_sVehicle = sUnit;
                // RMC 20110810 added capturing of addl info value to business record (e)
                

            }
            pCmd.Close();

        }

    }

}
