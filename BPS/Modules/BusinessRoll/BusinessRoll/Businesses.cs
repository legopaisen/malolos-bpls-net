using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.AppSettings;
using Amellar.Common.AuditTrail;
using Amellar.Common.DataConnector;
using Amellar.Common.EncryptUtilities;
using Amellar.Common.StringUtilities;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BusinessRoll
{
    class Businesses
    {

        OracleResultSet _result = new OracleResultSet();
        OracleResultSet _rs = new OracleResultSet();

        List<String> _strBin = new List<String>();
        public List<String> BIN
        {
            get { return _strBin; }
        }

        List<String> _strBnsName = new List<String>();
        public List<String> BusinessName
        {
            get { return _strBnsName; }
        }

        List<String> _strBnsAddr = new List<String>();
        public List<String> BusinessAddress
        {
            get { return _strBnsAddr; }
        }

        List<String> _strOwnerName = new List<String>();
        public List<String> OwnerName
        {
            get { return _strOwnerName; }
        }

        List<String> _strOwnerAddr = new List<String>();
        public List<String> OwnerAddress
        {
            get { return _strOwnerAddr; }
        }

        List<String> _strBusinessDesc = new List<String>();
        public List<String> BusinessDesc
        {
            get { return _strBusinessDesc; }
        }

        List<String> _strBnsStatus = new List<String>();
        public List<String> Status
        {
            get { return _strBnsStatus; }
        }

        List<Double> _dCapital = new List<Double>();
        public List<Double> Capital
        {
            get { return _dCapital; }
        }

        List<Double> _dGross = new List<Double>();
        public List<Double> Gross
        {
            get { return _dGross; }
        }

        List<DateTime> _dtDateOperated = new List<DateTime>();
        public List<DateTime> DateOperated
        {
            get { return _dtDateOperated; }
        }

        List<String> _strEmpContactNo = new List<String>();
        public List<String> EmpContactNo
        {
            get { return _strEmpContactNo; }
        }

        List<String> _strPermitNo = new List<String>();
        public List<String> PermitNo
        {
            get { return _strPermitNo; }
        }

        List<DateTime> _dtPermitDate = new List<DateTime>();
        public List<DateTime> PermitDate
        {
            get { return _dtPermitDate; }
        }

        List<String> _strBrgy = new List<String>();
        public List<String> Barangay
        {
            get { return _strBrgy; }
        }

        Boolean _HasRecord = false;
        public Boolean HasRecord
        {
            get { return _HasRecord; }
        }

        List<double> _dTaxPaid = new List<double>();
        public List<double> TaxPaid
        {
            get { return _dTaxPaid; }
        }

        List<String> _strQtrPaid = new List<string>();
        public List<String> QuarterPaid
        {
            get { return _strQtrPaid; }
        }

        int _iTopGrossCount = 0;
        public int TopGrossCount
        {
            get { return _iTopGrossCount; }
        }

        public static int BusinessCount(String Brgy, String Status, String TaxYear)
        {
            int iBnsCount = 0;
            OracleResultSet result = new OracleResultSet();
            result.Query = "select count(distinct bin) as iCount from businesses, own_names where businesses.own_code = own_names.own_code ";

            if (Brgy != "ALL")
                result.Query += "and rtrim(bns_brgy) like '" + Brgy + "' ";
            else
                result.Query += "and rtrim(bns_brgy) like '%%' ";

            if (Status != "ALL")
                result.Query += "and rtrim(bns_stat) like '" + Status + "' ";
            else
                result.Query += "and rtrim(bns_stat) like '%%' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    iBnsCount = result.GetInt("iCount");
                }
            }
            result.Close();


            result.Query = "select count(distinct bin) as iCount from buss_hist, own_names where buss_hist.own_code = own_names.own_code ";

            if (Brgy != "ALL")
                result.Query += "and rtrim(bns_brgy) like '" + Brgy + "' ";
            else
                result.Query += "and rtrim(bns_brgy) like '%%' ";

            if (Status != "ALL")
                result.Query += "and rtrim(bns_stat) like '" + Status + "' ";
            else
                result.Query += "and rtrim(bns_stat) like '%%' ";

            if (TaxYear != "")
                result.Query += "and tax_year = '" + TaxYear + "' ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    iBnsCount += result.GetInt("iCount");
                }
            }
            result.Close();

            return iBnsCount;
        }

        public static List<String> BusinessBrgy(String Brgy)
        {
            List<String> strBrgy = new List<String>();
            OracleResultSet result = new OracleResultSet();
            result.Query = "select distinct bns_brgy from businesses where ";

            if (Brgy != "" && Brgy != "ALL")
            {
                result.Query += "rtrim(bns_brgy) like '" + Brgy + "' ";
                result.Query += "union select distinct bns_brgy from buss_hist where rtrim(bns_brgy) like '" + Brgy + "' order by bns_brgy ";
            }
            else if (Brgy == "ALL")
            {
                result.Query += "rtrim(bns_brgy) like '%%' ";
                result.Query += "union select distinct bns_brgy from buss_hist where rtrim(bns_brgy) like '%%' order by bns_brgy ";
            }

            if (result.Execute())
            {
                while (result.Read())
                {
                    strBrgy.Add(result.GetString("bns_brgy"));
                }
            }
            result.Close();

            return strBrgy;
        }

        public static List<String> BnsCodeList(String BnsCode)
        {
            List<String> strBnsCode = new List<String>();
            OracleResultSet result = new OracleResultSet();

            result.Query = "select distinct bns_code from businesses where 1=1 ";

            if (BnsCode != "")
                result.Query += "and rtrim(bns_code) like '" + BnsCode + "' ";
            else
                result.Query += "and rtrim(bns_code) like '%%' ";

            result.Query += "order by bns_code ";

            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsCode.Add(result.GetString("bns_code"));
                }
            }
            result.Close();

            return strBnsCode;

        }       

        public void LoadBusinesses(String Brgy, String Status, String TaxYear, String BnsCode, String Street)
        {
            Double dGrossTmp = 0.00;
            Double dCapitalTmp = 0.00;
            _HasRecord = false;

            _result.Query = "select businesses.*, own_names.* from businesses, own_names where businesses.own_code = own_names.own_code ";

            if (BnsCode != "")
                _result.Query += "and rtrim(bns_code) like '" + BnsCode + "' ";
            else
                _result.Query += "and rtrim(bns_code) like '%%' ";

            if (Brgy != "ALL")
                _result.Query += "and businesses.bns_brgy like '" + StringUtilities.HandleApostrophe(Brgy) + "' ";
            else if (Brgy == "ALL")
                _result.Query += "and businesses.bns_brgy like '%%' ";

            if (Status != "ALL")
                _result.Query += "and businesses.bns_stat like '" + Status + "' ";
            else if (Status == "ALL")
                _result.Query += "and businesses.bns_stat like '%%' ";

            if (TaxYear != "")
                _result.Query += "and businesses.tax_year = '" + TaxYear + "' ";

            if (Street != "ALL")
                _result.Query += "and bns_street like '%%" + StringUtilities.HandleApostrophe(Street) + "%%' ";
            else
                _result.Query += "and bns_street like '%%' ";

            if (Street == "")
            {
                _result.Query += "union all select buss_hist.*, own_names.* from buss_hist,own_names where buss_hist.own_code = own_names.own_code ";

                if (BnsCode != "")
                    _result.Query += "and rtrim(bns_code) like '" + BnsCode + "' ";
                else
                    _result.Query += "and rtrim(bns_code) like '%%' ";

                if (Brgy != "ALL")
                    _result.Query += "and buss_hist.bns_brgy like '" + StringUtilities.HandleApostrophe(Brgy) + "' ";
                else if (Brgy == "ALL")
                    _result.Query += "and buss_hist.bns_brgy like '%%' ";

                if (Status != "ALL")
                    _result.Query += "and buss_hist.bns_stat like '" + Status + "' ";
                else if (Status == "ALL")
                    _result.Query += "and buss_hist.bns_stat like '%%' ";

                if (TaxYear != "")
                    _result.Query += "and buss_hist.tax_year = '" + TaxYear + "' ";
            }


            if (_result.Execute())
            {
                while (_result.Read())
                {
                    //_strBrgy.Add(result.GetString("bns_brgy"));
                    _HasRecord = true;
                    _strBin.Add(_result.GetString("bin"));
                    _strBnsName.Add(_result.GetString("bns_nm"));
                    _strBnsStatus.Add(_result.GetString("bns_stat"));
                    _strPermitNo.Add(_result.GetString("permit_no"));
                    _dtPermitDate.Add(_result.GetDateTime("permit_dt"));
                    _dtDateOperated.Add(_result.GetDateTime("dt_operated"));
                    _strEmpContactNo.Add(_result.GetString("bns_telno"));

                    dGrossTmp = _result.GetDouble("gr_1") + _result.GetDouble("gr_2");
                    dCapitalTmp = _result.GetDouble("capital");

                    String strOwnerCode = _result.GetString("own_code");
                    //_strOwnerName.Add(AppSettingsManager.GetBnsOwnAdd(strOwnerCode));
                    _strOwnerName.Add("BnsOwnAddTmp");

                    String strBnsCode = _result.GetString("bns_code");
                    //_strBusinessDesc.Add(AppSettingsManager.GetBnsDesc(strBnsCode));
                    _strBusinessDesc.Add("BnsDescTmp");

                    //Additional                    
                    String strTaxYear = _result.GetString("tax_year");
                    _rs.Query = "select gross, capital from addl_bns where bin = '" + _strBin + "' and tax_year = '" + strTaxYear + "' ";
                    if (_rs.Execute())
                    {
                        while (_rs.Read())
                        {
                            dGrossTmp += _rs.GetDouble("gross");
                            dCapitalTmp += _rs.GetDouble("capital");
                        }
                    }
                    _rs.Close();

                    _dGross.Add(dGrossTmp);
                    _dCapital.Add(dCapitalTmp);

                }
            }
            _result.Close();
            
        }       

        public void LoadTopGrosses(String Brgy, String Status, String TaxYear, String BnsCode)
        {
            _result.Query = "select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no from businesses a, addl_bns b where 1=1 ";
            if (BnsCode != "")
                _result.Query += "and rtrim(bns_code) like '" + BnsCode + "'";
            else
                _result.Query += "and rtrim(bns_code) like '%%' ";

            if (Brgy != "ALL")
                _result.Query += "and bns_brgy like '" + Brgy + "' ";
            else
                _result.Query += "and bns_brgy like '%%' ";

            if (TaxYear != "")
                _result.Query += "and a.tax_year = '" + TaxYear + "' ";

            if (Status != "ALL")
                _result.Query += "and a.bns_stat = '" + Status + "' ";
            else
                _result.Query += "and a.bns_stat = '%%' ";

            _result.Query += "and a.bin = b.bin and a.tax_year = b.tax_year ";
            _result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
            _result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no ";

            _result.Query += "union select a.bin,bns_nm,bns_code,own_code,sum(distinct gr_1) + sum(gross),gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no from buss_hist a ,addl_bns b where 1=1 ";
            if (BnsCode != "")
                _result.Query += "and rtrim(bns_code) like '" + BnsCode + "'";
            else
                _result.Query += "and rtrim(bns_code) like '%%' ";

            if (TaxYear != "")
                _result.Query += "and a.tax_year = '" + TaxYear + "' ";

            if (Status != "ALL")
                _result.Query += "and a.bns_stat = '" + Status + "' ";
            else
                _result.Query += "and a.bns_stat = '%%' ";

            _result.Query += "and a.bin = b.bin and a.tax_year = b.tax_year ";
            _result.Query += "and a.bin in (select distinct bin from pay_hist where data_mode <> 'UNP') ";
            _result.Query += "group by a.bin,bns_nm,bns_code,own_code,gr_1,gr_2,dt_operated,permit_no,permit_dt,or_no order by 5 desc ";
            
            if (_result.Execute())
            {
                while (_result.Read())
                {
                    _iTopGrossCount++;
                    string strBnsDesc = "";
                    double dGr1 = _result.GetDouble("gr_1");
                    double dGr2 = _result.GetDouble("gr_2");
                    double dGrAddnl = 0;
                    double dTaxPaid = 0;
                    string strORNo = "";
                    string strQtrPaid = "";
                    string strBnsAddr = "";
                    string strOwnName = "";
                    string strOwnAddr = "";

                    strBnsAddr = AppSettingsManager.GetBnsAdd(_result.GetString("bin"), "");
                    strOwnName = AppSettingsManager.GetBnsOwner(_result.GetString("own_code"));
                    strOwnAddr = AppSettingsManager.GetBnsOwnAdd(_result.GetString("own_code"));
                    strBnsDesc = AppSettingsManager.GetBnsDesc(_result.GetString("bns_code"));

                    _rs.Query = "select sum(gross) as gr_addl from addl_bns where bin = '" + _result.GetString("bin") + "' and tax_year = '" + TaxYear + "'";
                    if (_rs.Execute())
                    {
                        while (_rs.Read())
                        {
                            dGrAddnl = _rs.GetDouble("gr_addl");
                        }
                    }
                    _rs.Close();

                    //_rs.Query = "select bns_desc from bns_table where fees_code = 'B' and bns_code = '" + _result.GetString("bns_code") + "' and rev_year = '1993'";
                    //if (_rs.Execute())
                    //{
                    //    while (_rs.Read())
                    //    {
                    //        strBnsDesc = _rs.GetString("bns_desc");
                    //    }
                    //}
                    //_rs.Close();

                    _rs.Query = "select distinct or_no, qtr_paid from pay_hist where bin = '" + _result.GetString("bin") + "' and tax_year = '" + TaxYear + "' order by qtr_paid";
                    if (_rs.Execute())
                    {
                        while (_rs.Read())
                        {
                            strORNo = _rs.GetString("or_no");
                            strQtrPaid = _rs.GetString("qtr_paid");
                        }
                    }
                    _rs.Close();

                    _rs.Query = "select sum(fees_amtdue) as BtaxAmtDue from or_table where or_no = '" + strORNo + "'";
                    if (_rs.Execute())
                    {
                        while (_rs.Read())
                        {
                            dTaxPaid = _rs.GetDouble("BtaxAmtDue");
                        }
                    }
                    _rs.Close();

                    _strBin.Add(_result.GetString("bin"));
                    _strBnsName.Add(_result.GetString("bns_nm"));
                    _strBnsAddr.Add(strBnsAddr);
                    _strOwnerName.Add(strOwnName);
                    _strOwnerAddr.Add(strOwnAddr);
                    _strBusinessDesc.Add(strBnsDesc);
                    _dGross.Add((dGr1 + dGr2) + dGrAddnl);
                    _dtDateOperated.Add(_result.GetDateTime("dt_operated"));
                    _strPermitNo.Add(_result.GetString("permit_no"));
                    _dtPermitDate.Add(_result.GetDateTime("permit_dt"));
                    _dTaxPaid.Add(dTaxPaid);
                    _strQtrPaid.Add(strQtrPaid);
                }
            }
            _result.Close();
        }

        public void LoadTopPayers(String Brgy, String Status, String TaxYear, String BnsCode)
        {
            
        }
    }
}
