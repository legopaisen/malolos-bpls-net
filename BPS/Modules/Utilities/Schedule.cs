using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public class Schedule
    {
        protected frmSchedule ScheduleFrm = null;
        protected string m_strMeans = string.Empty;
        protected frmBnsTypeCode frmBnsTypeCode = new frmBnsTypeCode();    

        public Schedule(frmSchedule Form)
        {
            this.ScheduleFrm = Form;
            
        }

        public void InitialFormLoad()
        {
            OracleResultSet result = new OracleResultSet();

            ScheduleFrm.RevYear = AppSettingsManager.GetConfigObject("07");

            ScheduleFrm.ComboBnsDesc.Items.Clear();

            //result.Query = string.Format("select bns_desc,bns_code from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' and bns_code not in (select bns_code from bns_table where (bns_code like '10%' or bns_code like '11%' or bns_code like '12%' or bns_code like '13%' or bns_code like '14%' or bns_code like '15%' ) and fees_code = 'B') order by bns_code", ScheduleFrm.RevYear); REM MCR 20141106
            result.Query = string.Format("select bns_desc,bns_code from bns_table where fees_code = 'B' and length(trim(bns_code)) = 2 and rev_year = '{0}' order by bns_code", ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.BnsCode = result.GetString(1);
                    ScheduleFrm.ComboBnsDesc.Items.Add(StringUtilities.RemoveApostrophe(result.GetString(0)));

                    while (result.Read())
                    {
                        ScheduleFrm.ComboBnsDesc.Items.Add(StringUtilities.RemoveApostrophe(result.GetString(0)));
                    }

                    ScheduleFrm.ComboBnsDesc.SelectedIndex = 0;
                }
                
            }
            result.Close();

            ScheduleFrm.ComboFeesDesc.Items.Clear();

            result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.FeesCode = result.GetString("fees_code");
                    ScheduleFrm.ComboFeesDesc.Items.Add(StringUtilities.RemoveApostrophe(result.GetString("fees_desc")));

                    while (result.Read())
                    {
                        ScheduleFrm.ComboFeesDesc.Items.Add(StringUtilities.RemoveApostrophe(result.GetString("fees_desc")));
                    }

                    ScheduleFrm.ComboFeesDesc.SelectedIndex = 0;
                }
            }
            result.Close();
        }

        public virtual void FormLoad()
        {
            
        }

        public virtual void SelChangeBnsDesc()
        {
        }

        public virtual void SelChangeFeesDesc()
        {
        }

        public virtual void Add()
        {
        }

        public virtual void Edit()
        {
        }

        public virtual void Delete()
        {
        }

        public virtual void Close()
        {
            
        }

        public virtual void ClickListOne(int intRow, int intCol)
        {
        }

        public virtual void ClearLists()
        {
        }


        public virtual void ListOneComboEdit(string strSelected)
        {
        }

        public virtual void CheckTmpTable(int intRow)
        {
        }

        public virtual void ListTwoEndEdit(int intCol, int intRow, string strGridPrevValue)
        {
        }

        public virtual void ButtonConfig()
        {
        }

        public virtual void ButtonQuarterConfig()
        {
        }

        public virtual void EnableControls(bool blnEnable)
        {
        }

        protected bool CheckSched(string strBnsCode, string strFeesCode)
        {
            OracleResultSet result = new OracleResultSet();

            bool blnWatch = false;

            result.Query = string.Format("select * from btax where bns_code_main like '{0}%'", strBnsCode);
            if(result.Execute())
            {
                if (result.Read())
                {
                    blnWatch = true;
                }
            }
            result.Close();

            if(strFeesCode == "")
                result.Query = string.Format("select * from tax_and_fees where bns_code_main like '{0}%'", strBnsCode);
            else
                result.Query = string.Format("select * from tax_and_fees where bns_code_main like '{0}%' and fees_code = '{1}'", strBnsCode, strFeesCode);
            if(result.Execute())
            {
                if (result.Read())
                {
                    blnWatch = true;
                }
            }
            result.Close();

            if (strFeesCode == "")  // RMC 20111005 mods in validation of sched
            {
                result.Query = string.Format("select * from addl_bns_hist where bns_code_main like '{0}%'", strBnsCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        blnWatch = true;
                    }
                }
                result.Close();

                result.Query = string.Format("select * from addl_bns where bns_code_main like '{0}%'", strBnsCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        blnWatch = true;
                    }
                }
                result.Close();
            }

            if (strFeesCode == "")
                result.Query = string.Format("select * from fire_tax where bns_code_main like '{0}%'", strBnsCode);
            else
                result.Query = string.Format("select * from fire_tax where bns_code_main like '{0}%' and fees_code = '{1}'", strBnsCode, strFeesCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    blnWatch = true;
                }
            }
            result.Close();

            if (strFeesCode == "")
                result.Query = string.Format("select * from fire_tax_dup where bns_code_main like '{0}%'", strBnsCode);
            else
                result.Query = string.Format("select * from fire_tax_dup where bns_code_main like '{0}%' and fees_code = '{1}'", strBnsCode, strFeesCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    blnWatch = true;
                }
            }
            result.Close();

            if (strFeesCode == "")  // RMC 20111005 mods in validation of sched
            {
                // RMC 20110913 added validation in businesses table when deleting sched (s)
                result.Query = string.Format("select * from businesses where bns_code like '{0}%'", strBnsCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                        blnWatch = true;
                    }
                }
                result.Close();
                // RMC 20110913 added validation in businesses table when deleting sched (e)
            }

            //if (sUser == "X" || sUser == "SYS_PROG") for testing purposes
              //  bWatch = false;

            return blnWatch;
        }

        protected void InitializeButtons()
        {
            ScheduleFrm.CheckLicense.Enabled = true;
            ScheduleFrm.CheckFees.Enabled = true;
            ScheduleFrm.ButtonAdd.Text = "&Add";
            ScheduleFrm.ButtonEdit.Text = "&Edit";
            ScheduleFrm.ButtonClose.Text = "&Close";
            ScheduleFrm.CheckGross.Enabled = false;
            ScheduleFrm.ButtonAdd.Enabled = true;
            ScheduleFrm.ButtonEdit.Enabled = true;
            ScheduleFrm.ButtonDelete.Enabled = true;
        }

        public virtual void ChangedTmpSubCode()
        {
        }

        public virtual void ChangedMeans()
        {
        }

        public virtual void ResetDefault()
        {
        }

        public virtual void SetDefault()
        {
        }

        public virtual void ExemptedBuss()
        {
        }

        public virtual void DeleteSubBusiness(int intRow)
        {
        }

        protected string GenerateSubCode(int intRow)
        {
            string strTemp = string.Empty;
            string strSubCode = string.Empty;
            //strSubCode = ScheduleFrm.BnsCode;
            int intTempCode = 0;
            int intTemp = 0;

            // row   cnt
            // 0    01
            // 1    02

            if (ScheduleFrm.ButtonAdd.Text == "&Save" || ScheduleFrm.ButtonEdit.Text == "&Update")
            {
                int i = 0;

                i = intRow - 1;
                
                if(i >= 0)
                {
                    strTemp = ScheduleFrm.ListOne[0, i].Value.ToString();
                    strTemp = StringUtilities.Left(strTemp, 4);

                    strSubCode = StringUtilities.Right(strTemp, 2);

                    int.TryParse(strSubCode, out intTempCode);
                }
                int.TryParse(ScheduleFrm.BnsCode, out intTemp);

                intTempCode = intTempCode + 1;

                strSubCode = string.Format("{0:00}{1:00}", intTemp, intTempCode);
              /*  ScheduleFrm.ListOne.Rows.Add("");
                ScheduleFrm.ListOne[0, intRow].Value = strSubCode;*/

                
            }
            return strSubCode;
        }

        protected void LoadMinMaxTax(string strFeesCode, string strBnsCode)
        {
            OracleResultSet result = new OracleResultSet();

            // RMC 20150305 corrections in schedules module (S)
            result.Query = string.Format("select * from tmp_minmax_tax_table where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", strFeesCode, strBnsCode, ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.MinTax = string.Format("{0:##0.00}", result.GetInt("min_tax"));
                    ScheduleFrm.MaxTax = string.Format("{0:##0.00}", result.GetInt("max_tax"));
                }
                else
                {
                    result.Close();
                    // RMC 20150305 corrections in schedules module (e)

                    result.Query = string.Format("select * from minmax_tax_table where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", strFeesCode, strBnsCode, ScheduleFrm.RevYear);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            ScheduleFrm.MinTax = string.Format("{0:##0.00}", result.GetInt("min_tax"));
                            ScheduleFrm.MaxTax = string.Format("{0:##0.00}", result.GetInt("max_tax"));
                            //m_sMinMaxDataType = GetStrVariant(pRec->GetCollect("data_type"));

                        }
                        else
                        {
                            ScheduleFrm.MinTax = string.Format("{0:##0.00}", 0);
                            ScheduleFrm.MinTax = string.Format("{0:##0.00}", 0);
                            //m_sMinMaxDataType = "F";	
                        }
                    }
                    result.Close();
                }
                
            }
            result.Close();
        }

        protected void LoadTmpMinMaxTax(string strFeesCode)
        {
            // RMC 20150305 corrections in schedules module
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("delete from tmp_minmax_tax_table where fees_code = '{0}' and rev_year = '{1}' and bns_code like '{2}%'", strFeesCode, ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (result.ExecuteNonQuery() == 0)
            { }

            result.Query = "insert into tmp_minmax_tax_table ";
            result.Query += string.Format("select * from minmax_tax_table where fees_code = '{0}' and rev_year = '{1}' and bns_code like '{2}%'", strFeesCode, ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (result.ExecuteNonQuery() == 0)
            { }
        }

        protected void AddListTwoRow()
        {
            string strTempValue = string.Empty;
            int i = ScheduleFrm.ListTwo.Rows.Count - 1;

            if (i >= 0 )
            {
                if (ScheduleFrm.ListTwo[0, i].Value != "")
                {
                    ScheduleFrm.ListTwo.Rows.Add("");
                }
            }
            else
                ScheduleFrm.ListTwo.Rows.Add("");

        }

        protected void AddListOneRow()
        {
            string strTempValue = string.Empty;
            int i = ScheduleFrm.ListOne.Rows.Count - 1;

            if (i >= 0)
            {
                if (ScheduleFrm.ListOne[0, i].Value.ToString() != "")
                {
                    ScheduleFrm.ListOne.Rows.Add("");
                }
            }
            else
                ScheduleFrm.ListOne.Rows.Add("");

        }

        public virtual void SetFeesTerm(string sFeesTerm)
        {
        }

        public virtual void EditMinMax()
        {
            // RMC 20150305 corrections in schedules module
        }
    }
}
