using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using GemBox.Spreadsheet;
using Amellar.Common.ExcelReport;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.DILGReport
{
    public class DILGReportTemplate
    {
        private ExcelWorksheet m_objTemplate;
        private ExcelWorksheet m_objWorksheet;
        private int m_intRowIndex = 0;

        public DILGReportTemplate(ExcelWorksheet template, ExcelWorksheet worksheet, DateTime dtFrom, DateTime dtTo, string strYear, bool blnIncludeSurch, bool blnIncludeSplBns) //MCR 20150331 added blnIncludeSplBns
        {
            m_objTemplate = template;
            m_objWorksheet = worksheet;

            m_intRowIndex = 0;
            Hashtable hshReferences = new Hashtable();
            Hashtable hshProperties = new Hashtable();

            ExcelTemplate.CopyColumnStyle(m_objTemplate, m_objWorksheet);

            string strBnsStat = string.Empty, strFeesCode = string.Empty, strColNm = string.Empty;
            int intCountNew = 0, intCountNewWSP = 0, intCountNewCum = 0, intCountNewW = 0;
            int intCountRen = 0, intCountRenWSP = 0, intCountRenCum = 0, intCountRenW = 0;
            int intCountRet = 0, intCountRetWSP = 0, intCountRetCum = 0, intCountRetW = 0;
            double dblTotColMayor = 0.0, dblTotColMayorCum = 0.0;
            double dblTotColReg = 0.0, dblTotColRegCum = 0.0;
            double dblTotColBTax = 0.0, dblTotColBTaxCum = 0.0;

            OracleResultSet result = new OracleResultSet();
            // RMC 20151214 correction in DILG report (s)
            // double entries cause of compromise agreement
            try
            {
                result.Query = "create table pay_hist_tmp (bin varchar2(19), or_date_fr date)";
                if (result.ExecuteNonQuery() == 0)
                { }
            }
            catch { }

            result.Query = "delete from pay_hist_tmp ";
            if (result.ExecuteNonQuery() == 0)
            { }

            result.Query = "insert into pay_hist_tmp ";
            result.Query += string.Format("select distinct bin, min(or_date) from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between  '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}' ", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()));
            result.Query += "and (payment_term = 'F' or qtr_paid = '1' or qtr_paid = '2' or qtr_paid = '3' or qtr_paid = '4') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' ";
            result.Query += string.Format("and tax_year = '{0}' and bin not in ", strYear);
            result.Query += "(select bin from businesses where bns_stat = 'RET') ";
            result.Query += "group by bin";
            if (result.ExecuteNonQuery() == 0)
            { }
            // RMC 20151214 correction in DILG report (e)

            
            /*result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                                         and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear); 
            */
            // RMC 20151214 correction in DILG report, put rem

            // RMC 20151214 correction in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                                         and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151214 correction in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNew = result.GetInt(1);
                    else
                        intCountRen = result.GetInt(1);
                }
            }

            /*result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear);
             */
            // RMC 20151214 correction in DILG report, put rem

            // RMC 20151214 correction in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151214 correction in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNew = intCountNew + result.GetInt(1);
                    else
                        intCountRen = intCountRen + result.GetInt(1);
                }
            }

            if (blnIncludeSplBns) //MCR 20150331
            {
                /*result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                                        and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear);
                 */
                // RMC 20151214 correction in DILG report, put rem

                // RMC 20151214 correction in DILG report (s)
                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                                        and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", dtFrom, dtTo, strYear);
                // RMC 20151214 correction in DILG report (e)

                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strBnsStat = result.GetString(0).Trim();
                        if (strBnsStat == "NEW")
                            intCountNew = intCountNew + result.GetInt(1);
                        else
                            intCountRen = intCountRen + result.GetInt(1);
                    }
                }
            }

            // CUMULATIVE
            /*result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                       and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
             */
            /*
            // RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                       and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
            */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
//            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
//                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
//                                                         and (payment_term = 'F' or qtr_paid = '1' or qtr_paid = '2' or qtr_paid = '3' or qtr_paid = '4') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
//                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)

            //AFM 20210512 MAO-21-15111	- query from binan //JARS 20190801 CORRECTIONS FOR CUMULATIVE YEAR
            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1' or qtr_paid = '2' or qtr_paid = '3' or qtr_paid = '4') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()), strYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewCum = result.GetInt(1);
                    else
                        intCountRenCum = result.GetInt(1);
                }
            }

            /*result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
             */
            /*// RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                           and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
             */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
//            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
//                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
//                                           and bin not in (select bin from businesses where tax_year = '{2}')
//                                           and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)

            //AFM 20210512 MAO-21-15111	- query from binan //JARS 20190801 CORRECTIONS FOR CUMULATIVE YEAR
            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()), strYear);
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewCum = intCountNewCum + result.GetInt(1);
                    else
                        intCountRenCum = intCountRenCum + result.GetInt(1);
                }
            }
            
            if (blnIncludeSplBns) //MCR 20150331
            {
                /*result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
                 */
                /*
                // RMC 20151109 corrections in DILG report (s)
                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                           and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
                // RMC 20151109 corrections in DILG report (e)
                */
                // RMC 20151109 corrections in DILG report, put rem

                // RMC 20151109 corrections in DILG report (s)
//                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
//                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
//                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo, strYear);
                // RMC 20151109 corrections in DILG report (e)

                //AFM 20210512 MAO-21-15111	- query from binan //JARS 20190801 CORRECTIONS FOR CUMULATIVE YEAR
                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' group by bns_stat", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()), strYear);
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strBnsStat = result.GetString(0).Trim();
                        if (strBnsStat == "NEW")
                            intCountNewCum = intCountNewCum + result.GetInt(1);
                        else
                            intCountRenCum = intCountRenCum + result.GetInt(1);
                    }
                }
            }

            // WOMEN SP
            /*result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
            */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewWSP = result.GetInt(1);
                    else
                        intCountRenWSP = result.GetInt(1);
                }
            }

            /*result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
             */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewWSP = intCountNewWSP + result.GetInt(1);
                    else
                        intCountRenWSP = intCountRenWSP + result.GetInt(1);
                }
            }

            if (blnIncludeSplBns) //MCR 20150331
            {
                /*result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
                 */
                // RMC 20151109 corrections in DILG report, put rem

                // RMC 20151109 corrections in DILG report (s)
                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'
                                           group by bns_stat", dtFrom, dtTo, strYear);
                // RMC 20151109 corrections in DILG report (e)
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strBnsStat = result.GetString(0).Trim();
                        if (strBnsStat == "NEW")
                            intCountNewWSP = intCountNewWSP + result.GetInt(1);
                        else
                            intCountRenWSP = intCountRenWSP + result.GetInt(1);
                    }
                }
            }

            // WOMEN
            /*result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
             */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewW = result.GetInt(1);
                    else
                        intCountRenW = result.GetInt(1);
                }
            }

            /*result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
            */
            // RMC 20151109 corrections in DILG report, put rem

            // RMC 20151109 corrections in DILG report (s)
            result.Query = string.Format(@"select bns_stat, count(bin) from buss_hist 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and bin not in (select bin from businesses where tax_year = '{2}')
                                           and tax_year = '{2}' and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
            // RMC 20151109 corrections in DILG report (e)
            if (result.Execute())
            {
                while (result.Read())
                {
                    strBnsStat = result.GetString(0).Trim();
                    if (strBnsStat == "NEW")
                        intCountNewW = intCountNewW + result.GetInt(1);
                    else
                        intCountRenW = intCountRenW + result.GetInt(1);
                }
            }

            if (blnIncludeSplBns) //MCR 20150331
            {
                /*result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}'
                                                         and (payment_term = 'F' or qtr_paid = '1') and qtr_paid <> 'A' and qtr_paid <> 'P' and qtr_paid <> 'X' and tax_year = '{2}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
                 */
                // RMC 20151109 corrections in DILG report, put rem

                // RMC 20151109 corrections in DILG report (s)
                result.Query = string.Format(@"select bns_stat, count(bin) from spl_businesses 
                                           where bin in (select bin from pay_hist_tmp where TO_CHAR(or_date_fr, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}') 
                                           and bns_stat <> 'RET' and tax_year = '{2}' 
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           group by bns_stat", dtFrom, dtTo, strYear);
                // RMC 20151109 corrections in DILG report (e)
                if (result.Execute())
                {
                    while (result.Read())
                    {
                        strBnsStat = result.GetString(0).Trim();
                        if (strBnsStat == "NEW")
                            intCountNewW = intCountNewW + result.GetInt(1);
                        else
                            intCountRenW = intCountRenW + result.GetInt(1);
                    }
                }
            }

//            result.Query = string.Format(@"select count(bin) from businesses 
//                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
//                                           ", dtFrom, dtTo);//REM MCR 20150331 and bns_stat = 'RET'
            result.Query = string.Format(@"select count(bin) from businesses 
                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           ", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo);//REM MCR 20150331 and bns_stat = 'RET' //AFM 20210512 MAO-21-15111	- query from binan
            result.Query += " and bns_stat = 'RET'";    // RMC 20151109 corrections in DILG report
            int.TryParse(result.ExecuteScalar(), out intCountRet);

//            result.Query = string.Format(@"select count(bin) from businesses 
//                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
//                                           ", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), dtTo);//REM MCR 20150331 and bns_stat = 'RET'
            result.Query = string.Format(@"select count(bin) from businesses 
                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           ", DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()));//REM MCR 20150331 and bns_stat = 'RET' //AFM 20210512 MAO-21-15111	- query from binan
            result.Query += " and bns_stat = 'RET'";    // RMC 20151109 corrections in DILG report
            int.TryParse(result.ExecuteScalar(), out intCountRetCum);

            result.Query = string.Format(@"select count(bin) from businesses 
                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')
                                           and orgn_kind = 'SINGLE PROPRIETORSHIP'", dtFrom, dtTo);//REM MCR 20150331 and bns_stat = 'RET'
            result.Query += " and bns_stat = 'RET'";    // RMC 20151109 corrections in DILG report
            int.TryParse(result.ExecuteScalar(), out intCountRetWSP);

            result.Query = string.Format(@"select count(bin) from businesses 
                                           where bin in (select bin from retired_bns where TO_CHAR(apprvd_date, 'YYYY/MM/DD') between '{0:yyyy/MM/dd}' and '{1:yyyy/MM/dd}')
                                           and own_code in (select own_code from own_profile where gender = 'FEMALE')", dtFrom, dtTo);//REM MCR 20150331 and bns_stat = 'RET'
            result.Query += " and bns_stat = 'RET'";    // RMC 20151109 corrections in DILG report
            int.TryParse(result.ExecuteScalar(), out intCountRetW);

            //COLL
            if (blnIncludeSurch)
                strColNm = "fees_amtdue";
            else
                strColNm = "fees_due";

            String sSplBnsTable = "";
            if (blnIncludeSplBns)
                sSplBnsTable = " or bin in (select bin from spl_businesses)";

            result.Query = string.Format(@"select fees_code, sum({0}) from or_table 
                                           where or_no in (select or_no from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{1:yyyy/MM/dd}' and '{2:yyyy/MM/dd}' 
                                           and (bin in (select bin from businesses)
                                           {3})) group by fees_code", strColNm, dtFrom, dtTo, sSplBnsTable);
            if (result.Execute())
            {
                while (result.Read())
                {
                    strFeesCode = result.GetString(0).Trim();
                    if (strFeesCode == "B")
                        dblTotColBTax = result.GetDouble(1);
                    else if (strFeesCode == "01")
                        dblTotColMayor = result.GetDouble(1);
                    else
                        dblTotColReg = dblTotColReg + result.GetDouble(1);
                }
            }
            //COLL CUM
            result.Query = string.Format(@"select fees_code, sum({0}) from or_table 
                                           where or_no in (select or_no from pay_hist where TO_CHAR(or_date, 'YYYY/MM/DD') between '{1:yyyy/MM/dd}' and '{2:yyyy/MM/dd}'  
                                           and (bin in (select bin from businesses)
                                           {3}))group by fees_code", strColNm, DateTime.Parse("01/01/" + dtFrom.Year.ToString()), DateTime.Parse("12/31/" + dtFrom.Year.ToString()), sSplBnsTable); //AFM 20210512 MAO-21-15111 adjusted date for cumulative
            if (result.Execute())
            {
                while (result.Read())
                {
                    strFeesCode = result.GetString(0).Trim();
                    if (strFeesCode == "B")
                        dblTotColBTaxCum = result.GetDouble(1);
                    else if (strFeesCode == "01")
                        dblTotColMayorCum = result.GetDouble(1);
                    else
                        dblTotColRegCum = dblTotColRegCum + result.GetDouble(1);
                }
            }

            result.Close();

            //MCR 20150213 (s) percentage
            //intCountNewWSP = Math.Round(((double)intCountNewWSP / (double)intCountNewCum) * 100);
            //intCountRenWSP = Math.Round(((double)intCountRenWSP / (double)intCountRenCum) * 100);
            //intCountRetWSP = Math.Round(((double)intCountRetWSP / (double)intCountRetCum) * 100);
            //MCR 20150213 (e) percentage

            bool isLUBAO = false;
            if (AppSettingsManager.GetConfigValue("10") == "019")
                isLUBAO = true;

            //NEW
            hshProperties.Add("$COUNTNEW$", intCountNew);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTNEWWSP$", intCountNewWSP);
            else
                hshProperties.Add("$COUNTNEWWSP$", Math.Round(((double)intCountNewWSP / (double)intCountNewCum) * 100) + "%"); //MCR 20150213
            hshProperties.Add("$COUNTNEWCUM$", intCountNewCum);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTNEWW$", intCountNewW);
            else
                hshProperties.Add("$COUNTNEWW$", Math.Round(((double)intCountNewW / (double)intCountNewCum) * 100) + "%"); //MCR 20150213

            //REN
            hshProperties.Add("$COUNTREN$", intCountRen);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTRENWSP$", intCountRenWSP);
            else
                hshProperties.Add("$COUNTRENWSP$", Math.Round(((double)intCountRenWSP / (double)intCountRenCum) * 100) + "%"); //MCR 20150213
            hshProperties.Add("$COUNTRENCUM$", intCountRenCum);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTRENW$", intCountRenW);
            else
                hshProperties.Add("$COUNTRENW$", Math.Round(((double)intCountRenW / (double)intCountRenCum) * 100) + "%"); //MCR 20150213

            //RET
            hshProperties.Add("$COUNTRET$", intCountRet);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTRETWSP$", intCountRetWSP);
            else
                hshProperties.Add("$COUNTRETWSP$", Math.Round(((double)intCountRetWSP / (double)intCountRetCum) * 100) + "%"); //MCR 20150213
            hshProperties.Add("$COUNTRETCUM$", intCountRetCum);
            if (isLUBAO == false)
                hshProperties.Add("$COUNTRETW$", intCountRetW);
            else
                hshProperties.Add("$COUNTRETW$", Math.Round(((double)intCountRetW / (double)intCountRetCum) * 100) + "%"); //MCR 20150213
            //MP
            hshProperties.Add("$TOTCOLMAYOR$", dblTotColMayor);
            hshProperties.Add("$TOTCOLMAYORCUM$", dblTotColMayorCum); 
            //REG
            hshProperties.Add("$TOTCOLREG$", dblTotColReg);
            hshProperties.Add("$TOTCOLREGCUM$", dblTotColRegCum);
            //BTAX
            hshProperties.Add("$TOTCOLBTAX$", dblTotColBTax);
            hshProperties.Add("$TOTCOLBTAXCUM$", dblTotColBTaxCum);

            hshProperties.Add("$PERIOD$", string.Format("{0:MMM dd, yyyy} - {1:MMM dd, yyyy}", dtFrom, dtTo));
            hshProperties.Add("$DATE$", string.Format("{0:MMM dd, yyyy}", AppSettingsManager.GetSystemDate()));
            hshProperties.Add("$BPLO$", AppSettingsManager.GetConfigValue("03"));
            hshProperties.Add("$MAYOR$", AppSettingsManager.GetConfigValue("36"));

            //MCR 20150209 (s)
            hshProperties.Add("$LGU$", AppSettingsManager.GetConfigValue("02"));
            hshProperties.Add("$PROVINCE$", AppSettingsManager.GetConfigValue("08"));
            //MCR 20150209 (e)

            m_intRowIndex = ExcelTemplate.CopyCellSubrange("A1", "K46", m_objTemplate, 0, m_intRowIndex, m_objWorksheet,
                hshProperties, out hshReferences);
        }
    }
}
