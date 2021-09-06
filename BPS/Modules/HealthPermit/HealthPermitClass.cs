using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;   // RMC 20150104 addl mods

namespace Amellar.Modules.HealthPermit
{
    public class HealthPermitClass
    {
        public OracleResultSet result = new OracleResultSet();
        public OracleResultSet result1 = new OracleResultSet();
        public string m_sTmpBIN = string.Empty;

        public DataTable HealthPermitView(string bin)
        {
            DataTable table = new DataTable();
            // RMC 20141211 QA Health Permit module (s)
            //result.Query = "select distinct tax_year from emp_names where bin = '" + bin + "' order by tax_year desc";
            result.Query = "select distinct tax_year from emp_names where (bin = '" + bin + "' or temp_bin = '" + bin + "') order by tax_year desc";    // RMC 20141228 modified permit printing (lubao)
            string sTaxYear = result.ExecuteScalar();
            // RMC 20141211 QA Health Permit module (e)
            
            //result.Query = "Select * from emp_names where bin ='" + bin + "'";
            result.Query = "Select * from emp_names where (bin ='" + bin + "' or temp_bin = '" + bin + "')";// RMC 20141228 modified permit printing (lubao)
            result.Query += " and tax_year = '" + sTaxYear + "'";   // RMC 20141211 QA Health Permit module
            table.Columns.Add("BIN");
            table.Columns.Add("TAX YEAR");
            table.Columns.Add("Last Name");
            table.Columns.Add("First Name");
            table.Columns.Add("M.I");
            table.Columns.Add("Occupation");
            table.Columns.Add("Age");
            table.Columns.Add("Date of Birth");
            table.Columns.Add("Gender");
            table.Columns.Add("Nationality");
            table.Columns.Add("Place of Work");
            table.Columns.Add("Address");
            table.Columns.Add("TIN");
            //DJAC-20150107 mod permit (
            table.Columns.Add("CTC No."); 
            table.Columns.Add("CTC Issued On"); 
            table.Columns.Add("CTC Issued At"); 
            //DJAC-20150107 mod permit )
            table.Columns.Add("Status");
            table.Columns.Add("RH No");
            table.Columns.Add("Reg. No.");
            // RMC 20141228 modified permit printing (lubao) (s)
            table.Columns.Add("ISSUANCE");
            table.Columns.Add("EXPIRATION");
            table.Columns.Add("XRAY DATE");
            table.Columns.Add("XRAY RESULT");
            table.Columns.Add("HEPA-B DATE");
            table.Columns.Add("HEPA-B RESULT");
            table.Columns.Add("DRUG TEST DATE");
            table.Columns.Add("DRUG TEST RESULT");
            table.Columns.Add("FECALYSIS DATE");
            table.Columns.Add("FECALYSIS RESULT");
            table.Columns.Add("URINALYSIS DATE");
            table.Columns.Add("URINALYSIS RESULT");
            // RMC 20141228 modified permit printing (lubao) (e)

            if (result.Execute())
            {
                while (result.Read())
                {
                    table.Rows.Add(
                        result.GetString("bin"),
                        result.GetString("tax_year"),
                        result.GetString("emp_ln"),
                        result.GetString("emp_fn"),
                        result.GetString("emp_mi"),
                        result.GetString("emp_occupation"),
                        result.GetInt("emp_age"),
                        result.GetString("emp_date_of_birth"),
                        result.GetString("emp_gender"),
                        result.GetString("emp_nationality"),
                        result.GetString("emp_place_of_work"),
                        result.GetString("emp_address"),
                        result.GetString("emp_tin"),
                        //DJAC-20150107 mod permit (
                        result.GetString("emp_ctc_number"), 
                        result.GetString("emp_ctc_issued_on"),
                        result.GetString("emp_ctc_issued_at"), 
                        //DJAC-20150107 mod permit )
                        result.GetString("emp_status"),
                        result.GetString("emp_rh_no"),
                        result.GetString("emp_id"),
                        result.GetString("EMP_ISSUANCE_DATE"),
                        result.GetString("EMP_EXPIRATION_DATE"),
                        result.GetString("EMP_XRAY_DATE"),
                        result.GetString("EMP_XRAY_RESULT"),
                        result.GetString("EMP_HEPAB_DATE"),
                        result.GetString("EMP_HEPAB_RESULT"),
                        result.GetString("EMP_DRUG_TEST_DATE"),
                        result.GetString("EMP_DRUG_TEST_RESULT"),
                        result.GetString("EMP_FECALYSIS_DATE"),
                        result.GetString("EMP_FECALYSIS_RESULT"),
                        result.GetString("EMP_URINALYSIS_DATE"),
                        result.GetString("EMP_URINALYSIS_RESULT"));
                }
            }
            if (!result.Commit())
            {
                result.Rollback();
            }
            return table;
        }

        //public void AddHealthPermit(string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status, string tax_year, string bin, string rh_no, string issuance)
        //public void AddHealthPermit(string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status, string tax_year, string bin, string rh_no, string issuance, string expiration, string xray_date, string xray_res, string hepa_date, string hepa_res, string drug_date, string drug_res, string feca_date, string feca_res, string uri_date, string uri_res, string sBnsName, string sBnsAdd)   // RMC 20141228 modified permit printing (lubao)
        public void AddHealthPermit(string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status, string tax_year, string bin, string rh_no, string issuance, string expiration, string xray_date, string xray_res, string hepa_date, string hepa_res, string drug_date, string drug_res, string feca_date, string feca_res, string uri_date, string uri_res, string sBnsName, string sBnsAdd, string ctcNo, string issuedOn, string issuedAt) //DJAC-20150107 mod permit
        {
            //DJAC-20150107 mod permit (
            string sLN = StringUtilities.HandleApostrophe(last_name);
            string sFn = StringUtilities.HandleApostrophe(first_name);
            string sMI = StringUtilities.HandleApostrophe(middle_initial);
            string sOcc = StringUtilities.HandleApostrophe(occupation);
            string sAge = age;
            string sDOB = date_of_birth;
            string sGen = gender;
            string sNat = StringUtilities.HandleApostrophe(nationality);
            string sPOW = StringUtilities.HandleApostrophe(place_of_work);
            string sAdd = StringUtilities.HandleApostrophe(address);
            string sTin = tin;
            string sStat = status;
            string sTY = tax_year;
            string sBIN = bin;
            string sRHNo = rh_no;
            string sIss = issuance;
            string sCTCNo = ctcNo;
            string sIssOn = issuedOn;
            string sIssAt = issuedAt;
            //DJAC-20150107 mod permit )

            // RMC 20141228 modified permit printing (lubao) (s)
            string sTmpBIN = string.Empty;
            m_sTmpBIN = "";
            if (bin.Trim() == "")
            {
                sTmpBIN = GenTempBin(tax_year);
                m_sTmpBIN = sTmpBIN;
            }
            else
                sTmpBIN = bin;
            // RMC 20141228 modified permit printing (lubao) (e)
                
            result.Query = "Insert into emp_names(emp_ln, emp_fn, emp_mi, emp_occupation, emp_age, emp_date_of_birth, emp_gender, ";
            result.Query += "emp_nationality, emp_place_of_work, emp_address, emp_tin, emp_status, tax_year, bin, emp_rh_no, ";
            result.Query += "emp_id, emp_issuance_date, emp_ctc_number, emp_ctc_issued_on, emp_ctc_issued_at, bns_nm, bns_add, temp_bin, EMP_EXPIRATION_DATE, EMP_XRAY_DATE, EMP_XRAY_RESULT, ";
            result.Query += "EMP_HEPAB_DATE,EMP_HEPAB_RESULT, EMP_DRUG_TEST_DATE,EMP_DRUG_TEST_RESULT,EMP_FECALYSIS_DATE, EMP_FECALYSIS_RESULT,";
            result.Query += "EMP_URINALYSIS_DATE,EMP_URINALYSIS_RESULT) ";
            //result.Query += "values('" + a + "', '" + b + "', '" + c + "', '" + d + "', '" + e + "', '" + f + "', '" + g + "', '" + h + "', ";
            //result.Query += "'" + i + "', '" + j + "', '" + k + "', '" + l + "', '" + m + "', '" + n + "', '" + o + "', concat(concat('" + sTY + "', '-'), ";
            //result.Query += "LPAD(emp_sequence.nextval, 6, '0')), '" + q + "', '" + r + "', '" + s + "', '" + t + "', '" + StringUtilities.HandleApostrophe(sBnsName) + "', '" + StringUtilities.HandleApostrophe(sBnsAdd) + "', '" + sTmpBIN + "', ";
            //DJAC-20150107 mod permit (
            result.Query += "values('" + sLN + "', '" + sFn + "', '" + sMI + "', '" + sOcc + "', '" + sAge + "', '" + sDOB + "', '" + sGen + "', '" + sNat + "', ";
            result.Query += "'" + sPOW + "', '" + sAdd + "', '" + sTin + "', '" + sStat + "', '" + sTY + "', '" + sBIN + "', '" + sRHNo + "', concat(concat('" + sTY + "', '-'), ";
            result.Query += "LPAD(emp_sequence.nextval, 6, '0')), '" + sIss + "', '" + sCTCNo + "', '" + sIssOn + "', '" + sIssAt + "', '" + StringUtilities.HandleApostrophe(sBnsName) + "', '" + StringUtilities.HandleApostrophe(sBnsAdd) + "', '" + sTmpBIN + "', ";
            //DJAC-20150107 mod permit )
            result.Query += "'"+expiration+"','"+xray_date+"','"+StringUtilities.HandleApostrophe(xray_res)+"','"+hepa_date+"','"+StringUtilities.HandleApostrophe(hepa_res)+"','"+drug_date+"','"+StringUtilities.HandleApostrophe(drug_res)+"','"+feca_date+"','"+StringUtilities.HandleApostrophe(feca_res)+"',";
            result.Query += "'"+uri_date+"','"+StringUtilities.HandleApostrophe(uri_res)+"')"; // RMC 20141228 modified permit printing (lubao)

            //result.Query = "Insert into emp_names(emp_ln, emp_fn, emp_mi, emp_occupation, emp_age, emp_date_of_birth, emp_gender, emp_nationality, emp_place_of_work, emp_address, emp_tin, emp_status, tax_year, bin, emp_rh_no, emp_id) values(:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, '" + rh_no + "', concat(concat(:15, '-'), LPAD(emp_sequence.nextval, 6, '0')))";
            //result.AddParameter(":1", StringUtilities.HandleApostrophe(last_name)); // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":2", StringUtilities.HandleApostrophe(first_name));    // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":3", StringUtilities.HandleApostrophe(middle_initial));  // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":4", StringUtilities.HandleApostrophe(occupation));    // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":5", age);
            //result.AddParameter(":6", date_of_birth);
            //result.AddParameter(":7", gender);
            //result.AddParameter(":8", StringUtilities.HandleApostrophe(nationality));   // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":9", StringUtilities.HandleApostrophe(place_of_work)); // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":10", StringUtilities.HandleApostrophe(address));  // RMC 20141211 QA Health Permit module, added handleapostrophe
            //result.AddParameter(":11", tin);
            //result.AddParameter(":12", status);
            //result.AddParameter(":13", tax_year);
            //result.AddParameter(":14", bin);
            //result.AddParameter(":15", tax_year);
                result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();
        }

        //public void UpdateHealthPermit(string id, string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status)
        //public void UpdateHealthPermit(string id, string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status, string rh_no, string issuance, string expiration, string xray_date, string xray_res, string hepa_date, string hepa_res, string drug_date, string drug_res, string feca_date, string feca_res, string uri_date, string uri_res,string sBnsName, string sBnsAdd)  // RMC 20141228 modified permit printing (lubao)
        public void UpdateHealthPermit(string id, string last_name, string first_name, string middle_initial, string occupation, string age, string date_of_birth, string gender, string nationality, string place_of_work, string address, string tin, string status, string rh_no, string issuance, string expiration, string xray_date, string xray_res, string hepa_date, string hepa_res, string drug_date, string drug_res, string feca_date, string feca_res, string uri_date, string uri_res,string sBnsName, string sBnsAdd, string ctcNo, string issuedOn, string issuedAt)  //DJAC-20150107 mod permit
        {
            AddHistory(id);
            // RMC 20141211 QA Health Permit module, added handleapostrophe
            result.Query = "Update emp_names set emp_ln = '" + StringUtilities.HandleApostrophe(last_name) 
                + "', emp_fn = '" + StringUtilities.HandleApostrophe(first_name) + "', emp_mi = '" + StringUtilities.HandleApostrophe(middle_initial) 
                + "', emp_occupation = '" + StringUtilities.HandleApostrophe(occupation) + "', emp_age = '" + age 
                + "', emp_date_of_birth = '" + date_of_birth + "', emp_gender = '" + gender 
                + "', emp_nationality = '" + nationality + "', emp_place_of_work = '" + StringUtilities.HandleApostrophe(place_of_work) 
                + "', emp_address = '" + StringUtilities.HandleApostrophe(address) + "', emp_tin = '" + tin +"', emp_status = '" + status
                + "', bns_nm = '" + StringUtilities.HandleApostrophe(sBnsName) + "', bns_add = '" + StringUtilities.HandleApostrophe(sBnsAdd)   // RMC 20141228 modified permit printing (lubao)
                + "', emp_rh_no = '" + StringUtilities.HandleApostrophe(rh_no) + "', emp_issuance_date = '"+ issuance + "', EMP_EXPIRATION_DATE = '" + expiration
                + "', EMP_XRAY_DATE = '" + xray_date + "', EMP_XRAY_RESULT = '" + StringUtilities.HandleApostrophe(xray_res) + "', EMP_HEPAB_DATE = '" + hepa_date
                + "', EMP_HEPAB_RESULT = '" + StringUtilities.HandleApostrophe(hepa_res) + "', EMP_DRUG_TEST_DATE = '" + drug_date + "', EMP_DRUG_TEST_RESULT = '" + StringUtilities.HandleApostrophe(drug_res)
                + "', EMP_FECALYSIS_DATE = '" + feca_date + "', EMP_FECALYSIS_RESULT = '" + StringUtilities.HandleApostrophe(feca_res)
                + "', EMP_URINALYSIS_DATE = '" + uri_date + "', EMP_URINALYSIS_RESULT = '" + StringUtilities.HandleApostrophe(uri_res)
                + "', emp_ctc_number = '" + ctcNo + "', emp_ctc_issued_on = '" + issuedOn + "', emp_ctc_issued_at = '" + issuedAt //DJAC-20150107 mod permit
                + "' where emp_id = '" + id + "'";
            // RMC 20141211 QA Health Permit module, added handleapostrophe
            result.Transaction = true;
            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();
        }

        public void DeleteHealthPermit(string id)
        {
            AddHistory(id);
            result.Query = "Delete from emp_names where emp_id = '" + id + "'";
            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();
        }

        public string GetAddlCode(string sAddlDesc)
        {
            string sAddlCode = string.Empty;
            result.Query = "select * from addl_info_tbl where addl_desc like  '%"+sAddlDesc+"%'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sAddlCode = result.GetString("addl_code");
                }
            }
            result.Close();

            return sAddlCode;
        }

        public void AddHistory(string id)
        {
            result.Query = "Insert into emp_names_hist (select * from emp_names where emp_id = '" + id + "')";
            result.ExecuteNonQuery();
            if (!result.Commit())
            {
                result.Rollback();
            }
            result.Close();
        }

        //public void UpdateGender(string gender, string bin)
        public void UpdateGender(string gender, string bin, string sTaxYear)    
        {
            // RMC 20141211 QA Health Permit module (s)
            OracleResultSet pCmd = new OracleResultSet();
            string sAddlCode = string.Empty;
            string sGender = string.Empty;
            int iEmpTot = 0;

            for (int i = 1; i <= 2; i++)
            {
                if (i == 1)
                {
                    sGender = "FEMALE";
                }
                else
                {
                    sGender = " MALE";
                }

                sAddlCode = GetAddlCode(sGender);

                int iEmpCount = 0;
                //result.Query = "select count(*) from emp_names where bin = '" + bin + "' and emp_gender = '" + sGender.Trim() + "'";
                result.Query = "select count(*) from emp_names where (bin = '" + bin + "' or temp_bin = '" + bin + "') and emp_gender = '" + sGender.Trim() + "' and tax_year = '" + sTaxYear + "'";  // RMC 20150104 addl mods
                int.TryParse(result.ExecuteScalar(), out iEmpCount);

                iEmpTot += iEmpCount;

                
                result.Query = "select * from addl_info where bin = '" + bin + "' and addl_code = '" + sAddlCode + "'";
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        pCmd.Query = "update addl_info set value = " + iEmpCount + " where bin = '" + bin + "' and addl_code = '" + sAddlCode + "'";
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                    else
                    {
                        pCmd.Query = "insert into addl_info values(:1, :2, :3)";
                        pCmd.AddParameter(":1", bin);
                        pCmd.AddParameter(":2", sAddlCode);
                        pCmd.AddParameter(":3", iEmpCount);
                        if (pCmd.ExecuteNonQuery() == 0)
                        { }
                    }
                }
            }
            // RMC 20150104 addl mods (s)
            // update other_info table for HEALTH FEE
            UpdateOtherInfo(bin, sTaxYear, "HEALTH FEE", iEmpTot);

            // exclude owner in number of employees
            int iOwnCount = 0;
            result.Query = "select count(*) from emp_names where (bin = '" + bin + "' or temp_bin = '" + bin + "') and tax_year = '" + sTaxYear + "' and emp_occupation = 'OWNER'";
            int.TryParse(result.ExecuteScalar(), out iOwnCount);
            iEmpTot = iEmpTot - iOwnCount;

            // update other_info table for NUMBER OF WORKERS
            UpdateOtherInfo(bin, sTaxYear, "NUMBER OF WORKERS", iEmpTot);
            // RMC 20150104 addl mods (e)

            pCmd.Query = "update businesses set num_employees = :1 where bin = '" + bin + "' and tax_year = '" + sTaxYear + "'";
            pCmd.AddParameter(":1", iEmpTot);
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            pCmd.Query = "update business_que set num_employees = :1 where bin = '" + bin + "' and tax_year = '" + sTaxYear + "'";
            pCmd.AddParameter(":1", iEmpTot);
            if (pCmd.ExecuteNonQuery() == 0)
            { }

            // RMC 20141211 QA Health Permit module (e)

            //int s = 0;
            //if (gender == "MALE")
            //{
            //    gender = "001";
            //}
            //else
            //{
            //    gender = "002";
            //}
            //result.Query = "Select * from addl_info where bin = '" + bin + "' and addl_code = '" + gender + "'";
            //if (result.Execute())
            //{
            //    while (result.Read())
            //    {
            //        s = Convert.ToInt32(result.GetString("value"));
            //    }
            //}
            //if (gender == "001")
            //{
            //    s = s + 1;
            //    result1.Query = "Update addl_info set value = '" + s.ToString()
            //    + "' where addl_code = '" + gender + "' and bin = '" + bin + "'";
            //    result1.Transaction = true;
            //    result1.ExecuteNonQuery();
            //    if (!result1.Commit())
            //    {
            //        result1.Rollback();
            //    }
            //    result1.Close();
            //}
            //else
            //{
            //    s = s + 1;
            //    result1.Query = "Update addl_info set value = '" + s.ToString()
            //    + "' where addl_code = '" + gender + "' and bin = '" + bin + "'";
            //    result1.Transaction = true;
            //    result1.ExecuteNonQuery();
            //    if (!result1.Commit())
            //    {
            //        result1.Rollback();
            //    }
            //    result1.Close();
            //}
            //if (!result.Commit())
            //{
            //    result.Rollback();
            //}
            //result.Close();
        }

        private string GenTempBin(string sTaxYear)
        {
            // RMC 20141228 modified permit printing (lubao)
            OracleResultSet pCmd = new OracleResultSet();
            int iSeries = 0;
            string sTmpBIN = string.Empty;

            //pCmd.Query = "select * from emp_names where tax_year = '" + sTaxYear + "' order by temp_bin desc";
            //pCmd.Query = "select * from emp_names where tax_year = '" + sTaxYear + "' and trim(temp_bin) is not null order by temp_bin desc";   // RMC 20150102 mods in permit
            // RMC 20150117 (s)
            pCmd.Query = "select * from emp_names where tax_year = '" + sTaxYear + "' and trim(temp_bin) is not null ";
            pCmd.Query += " and temp_bin like '" + sTaxYear + "%' order by temp_bin desc";
            // RMC 20150117 (e)
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                {
                    sTmpBIN = pCmd.GetString("temp_bin");
                    int.TryParse(StringUtilities.Right(sTmpBIN, 7), out iSeries);
                    iSeries++;
                }
                else
                {
                    iSeries = 1;
                }
            }
            pCmd.Close();

            sTmpBIN = string.Format("{0}-{1:000000#}", sTaxYear, iSeries);
            return sTmpBIN;
        }

        private void UpdateOtherInfo(string sBIN, string sTaxYear, string sTmp, int iEmpCount)
        {
            // RMC 20150104 addl mods
            OracleResultSet pCmd = new OracleResultSet();
            OracleResultSet result = new OracleResultSet();
            string sOtherInfoCode = string.Empty;
            string sDataType = string.Empty;

            string sMainBnsCode = AppSettingsManager.GetBnsCodeMain(sBIN);

            pCmd.Query = "select * from default_code where rev_year = '" + ConfigurationAttributes.RevYear + "'";
            pCmd.Query += " and default_desc like '" + sTmp + "%'";
            if (pCmd.Execute())
            {
                if (pCmd.Read())
                {
                    sOtherInfoCode = pCmd.GetString("default_code");
                    sDataType = pCmd.GetString("data_type");
                }
            }
            pCmd.Close();

            int iCnt = 0;
            result.Query = "select count(*) from other_info where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and default_code = '" + sOtherInfoCode + "'";
            int.TryParse(result.ExecuteScalar(), out iCnt);

            if (sOtherInfoCode != "" && sDataType != "" && iEmpCount != 0)
            {
                if (iCnt > 0)
                {
                    pCmd.Query = "update other_info set data = " + iEmpCount + " where bin = '" + sBIN + "' and tax_year = '" + sTaxYear + "' and default_code = '" + sOtherInfoCode + "'";
                    if (pCmd.ExecuteNonQuery() == 0)
                    { }
                }
                else
                {
                    pCmd.Query = "INSERT INTO other_info VALUES (:1,:2,:3,:4,:5,:6,:7)";
                    pCmd.AddParameter(":1", sBIN);
                    pCmd.AddParameter(":2", sTaxYear);
                    pCmd.AddParameter(":3", sMainBnsCode);
                    pCmd.AddParameter(":4", sOtherInfoCode);
                    pCmd.AddParameter(":5", sDataType);
                    pCmd.AddParameter(":6", iEmpCount);
                    pCmd.AddParameter(":7", ConfigurationAttributes.RevYear);
                    pCmd.ExecuteNonQuery();
                }
            }
        }
    }
}
