
// RMC 20120109 added trim in saving bns schedule
// RMC 20111001 correction in scheds editing, changed from GetInt to GetDouble

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.StringUtilities;
using Oracle.DataAccess.Client;
using Amellar.Common.AuditTrail;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.Utilities
{
    public class Fees:Schedule
    {
        //private ComboBox combobox1 = new ComboBox();

        OracleResultSet pCmd = new OracleResultSet();     // RMC 20111012 modified query commit in scheds

        bool m_boLoadRange = false;
        bool m_bSwBnsType = false;
        int m_iRowVsfaBnsType = 0;
        int m_iActiveRowBnsType = 0;
        string m_strBnsType = string.Empty;
        bool m_boInitialLoad = false;
        
        //MCR 20141111(s)
        int m_iCurrRow; 
        int m_iCurrCol;
        //MCR 20141111(e)

        public Fees(frmSchedule Form)
            : base(Form)
        {
        }
        
        public override void FormLoad()
        {
           // combobox1.Hide();
           // this.ScheduleFrm.LabelOne.Controls.Add(combobox1);

            this.ScheduleFrm.LabelOne.Text = "Business Sub-Categories";
            this.ScheduleFrm.LabelTwo.Text = "Schedules";
            this.EnableControls(false);
            ScheduleFrm.CheckGross.Checked = false;

            m_boInitialLoad = true;

            this.LoadFees(0);   //combo fees
            this.LoadBussType();    //datagrid1
            this.LoadSurchIntDisc();
            this.LoadQtrFeeConfig();
        }

        public override void EnableControls(bool blnEnable)
        {
            this.ScheduleFrm.TextLicMinTax.Enabled = false;
            this.ScheduleFrm.TextLicNewRate.Enabled = false;
            this.ScheduleFrm.TextInterest.Enabled = blnEnable;
            this.ScheduleFrm.TextSurcharge.Enabled = blnEnable;
            this.ScheduleFrm.TextDiscount.Enabled = blnEnable;
            this.ScheduleFrm.TextExcess.Enabled = blnEnable;
            this.ScheduleFrm.TextAddExcess.Enabled = blnEnable;
            this.ScheduleFrm.CheckLicQtrDec.Enabled = false;
            this.ScheduleFrm.ListOne.ReadOnly = !blnEnable;
            this.ScheduleFrm.ListTwo.ReadOnly = !blnEnable;
            this.ScheduleFrm.ButtonConfigQtr.Visible = true;
            this.ScheduleFrm.ButtonConfigQtr.Enabled = blnEnable;

            this.ScheduleFrm.CheckGross.Enabled = false;
            this.ScheduleFrm.TextBnsCode.Enabled = false;
            this.ScheduleFrm.TextRevYear.Enabled = false;
            this.ScheduleFrm.ButtonConfig.Enabled = false;

            this.ScheduleFrm.TextFeesCode.Enabled = false;
            this.ScheduleFrm.ComboFeesDesc.Enabled = true;
            this.ScheduleFrm.CheckFeesInt.Enabled = blnEnable;
            this.ScheduleFrm.CheckFeesQtr.Enabled = blnEnable;
            this.ScheduleFrm.CheckFeesMonth.Enabled = blnEnable;
            this.ScheduleFrm.CheckFeesYear.Enabled = blnEnable;
            this.ScheduleFrm.CheckFeesSurch.Enabled = blnEnable;
            this.ScheduleFrm.TextMinTax.Enabled = blnEnable;
            this.ScheduleFrm.TextMaxTax.Enabled = blnEnable;

            this.ScheduleFrm.LabelNew.Text = "Fees Term";
            this.ScheduleFrm.LabelNew.Visible = true;
            this.ScheduleFrm.LabelNewRate.Visible = false;
            this.ScheduleFrm.TextLicNewRate.Visible = false;
            this.ScheduleFrm.LabelNewMinTax.Visible = false;
            this.ScheduleFrm.TextLicMinTax.Visible = false;
            this.ScheduleFrm.CheckLicQtrDec.Visible = false;
            this.ScheduleFrm.CheckFeesMonth.Visible = true;
            this.ScheduleFrm.CheckFeesQtr.Visible = true;
            this.ScheduleFrm.CheckFeesYear.Visible = true;
        }

        private void LoadFees(int intLoadFees)
        {
            OracleResultSet result = new OracleResultSet();

            this.SetFeesTerm("");

            if (intLoadFees == 0) //OnInitDialog
                result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' order by fees_code", ScheduleFrm.RevYear);
            else if (intLoadFees == 1) //Edit Update Module
                result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and rev_year = '{0}' and fees_code = '{1}'", ScheduleFrm.RevYear, ScheduleFrm.FeesCode);
            else if (intLoadFees == 2 && !m_boInitialLoad) //onselchangefeesdescv
                result.Query = string.Format("select * from tax_and_fees_table where fees_type = 'FS' and fees_desc = '{0}' and rev_year = '{1}'", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper()), ScheduleFrm.RevYear);
            else
                return;
            if(result.Execute())
            {
                if (result.Read())
                {
                    m_boInitialLoad = false;

                    ScheduleFrm.FeesCode = result.GetString("fees_code");
                    ScheduleFrm.ComboFeesDesc.Text = StringUtilities.RemoveApostrophe(result.GetString("fees_desc"));

                    this.SetFeesTerm(result.GetString("fees_term"));

                    if (result.GetString("fees_withpen") == "Y")
                        ScheduleFrm.CheckFeesInt.Checked = true;
                    else
                        ScheduleFrm.CheckFeesInt.Checked = false;

                    if (result.GetString("fees_withsurch") == "Y")
                        ScheduleFrm.CheckFeesSurch.Checked = true;
                    else
                        ScheduleFrm.CheckFeesSurch.Checked = false;

                    if (intLoadFees == 0 || intLoadFees == 1)
                    {
                        result.Query = "delete from tmp_fees_sched";
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
            }
            result.Close();


            if (ScheduleFrm.FeesCode != "")
            {
                result.Query = string.Format("select * from taxdues where tax_code = '{0}'", ScheduleFrm.FeesCode);
                if (result.Execute())
                {
                    if (result.Read())
                        ScheduleFrm.ButtonDelete.Enabled = false;
                    else
                        ScheduleFrm.ButtonDelete.Enabled = true;
                }
                result.Close();
            }

            this.LoadTmpMinMaxTax(ScheduleFrm.FeesCode);    // RMC 20150305 corrections in schedules module
            this.LoadMinMaxTax(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);

            this.LoadRangeConfig();

            
        }

        private void LoadRangeConfig()
        {
            OracleResultSet result = new OracleResultSet();
            string strFeesCode = string.Empty;

            //result.Query = "select * from tax_and_fees_table order by fees_code ";
            result.Query = "select * from tax_and_fees_table where rev_year = '" + AppSettingsManager.GetConfigValue("07") + "' order by fees_code "; //MCR 20141118
            if (result.Execute())
            {
                while (result.Read())
                {
                    strFeesCode = result.GetString("fees_code");

                    result.Query = string.Format("delete from rate_config_tbl_tmp where fees_code = '{0}'", strFeesCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    result.Query = string.Format("delete from rate_config_tbl_ref_tmp where fees_code = '{0}'", strFeesCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    result.Query = string.Format("insert into rate_config_tbl_tmp (select * from rate_config_tbl where fees_code = '{0}')", strFeesCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                    result.Query = string.Format("insert into rate_config_tbl_ref_tmp (select * from rate_config_tbl_ref where fees_code = '{0}')", strFeesCode);
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }
                }
            }
            result.Close();
        }

        private void LoadBussType()
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            StringBuilder strQuery = new StringBuilder();
            double m_dAmount = 0;
            double dblArea = 0;
            double dblGross = 0;
            double dblExcess = 0;
            double dblAddEx = 0;

            string strAmt = string.Empty;

            bool bSwFeesDesc = false;

            this.ClearLists();

            result.Query = string.Format("select distinct(bns_code), bns_desc from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%' order by bns_code", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
			if(result.Execute())
            {
                if(result.Read())
                {
                }
                else
                {
                    result.Close();
                    if(ScheduleFrm.ButtonAdd.Text == "&Add")
			        {
                        strQuery.Append("select distinct(a.bns_code), bns_desc from fees_sched a, bns_table b");
				        strQuery.Append(string.Format(" where a.fees_code = '{0}' and a.fees_code = b.fees_code", ScheduleFrm.FeesCode));
			            strQuery.Append(string.Format(" and a.bns_code = b.bns_code and trim(a.bns_code) like '{0}%'", ScheduleFrm.BnsCode));
					    strQuery.Append(string.Format(" and a.rev_year = '{0}' and b.rev_year = '{0}' order by a.bns_code", ScheduleFrm.RevYear));
                        result.Query = strQuery.ToString();
                        
                    }				
                }
			}
            
            int intRow1 = 0;
            //intRow1 = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;

            if(ScheduleFrm.ButtonAdd.Text == "&Add" || ScheduleFrm.ButtonEdit.Text == "&Update")
			{
                if(result.Execute() && ScheduleFrm.BnsCode != "")
				{
    				ScheduleFrm.ListOne.Rows.Clear();
				
				    //iRowQR = 1;
				    //iRowAR = 1;
                    
                    while(result.Read())
				    {
					    ScheduleFrm.ListOne.Rows.Add("");

                        

                        ScheduleFrm.ListOne[0,intRow1].Value = result.GetString("bns_code");
					    
                        result2.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}'", ScheduleFrm.FeesCode,ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim());
                        if(result2.Execute())
                        {
                            if(result2.Read())
					        {
                                ScheduleFrm.ListOne[1,intRow1].Value = StringUtilities.RemoveApostrophe(result2.GetString("bns_desc"));

                                strQuery = new StringBuilder();

						        strQuery.Append(string.Format("select * from tmp_fees_sched where fees_code = '{0}'", ScheduleFrm.FeesCode));
                                strQuery.Append(string.Format(" and bns_code = '{0}'", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim()));
						        strQuery.Append(" and (select sum(qty1+qty2+area1+area2) from fees_sched");
						        strQuery.Append(string.Format(" where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear));
						        strQuery.Append(string.Format(" and bns_code = '{0}') = 0", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim()));
						        bSwFeesDesc = false;
					        }
					        else
					        {
						        ScheduleFrm.ListOne[1,intRow1].Value = StringUtilities.RemoveApostrophe(result.GetString("bns_desc"));

                                strQuery = new StringBuilder();

						        strQuery.Append(string.Format("select * from fees_sched where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear));
						        strQuery.Append(string.Format(" and bns_code = '{0}'", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim()));
						        strQuery.Append(" and (select sum(qty1+qty2+area1+area2+gr_1+gr_2) from fees_sched");
						        strQuery.Append(string.Format(" where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear));
						        strQuery.Append(string.Format(" and bns_code = '{0}') = 0", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim()));
						        bSwFeesDesc = true;
					        }
                            result2.Close();
                            
					        result2.Query = strQuery.ToString();
                            if(result2.Execute())
                            {
                                int intCtr = 1;
                                
                                while(result2.Read())
                                {
                                    intCtr = 2;
						            break;
                                }
					            
                                if(bSwFeesDesc)
						        {
                                    strQuery = new StringBuilder();

							        strQuery.Append(string.Format("select * from fees_sched where fees_code = '{0}'", ScheduleFrm.FeesCode));
							        strQuery.Append(string.Format(" and bns_code = '{0}' and rev_year = '{1}'", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim(), ScheduleFrm.RevYear));
						        }
						        else
						        {
							        strQuery = new StringBuilder();

							        strQuery.Append(string.Format("select * from tmp_fees_sched where fees_code = '{0}'", ScheduleFrm.FeesCode));
							        strQuery.Append(string.Format(" and bns_code = '{0}' and rev_year = '{1}'", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim(), ScheduleFrm.RevYear));
						        }
                                
                                switch (intCtr)
					            {
                                    case 1:
                                        {
                                            result2.Close();
                                            result2.Query = strQuery.ToString();

                                            if (result2.Execute())
                                            {
                                                int intQty = 0;

                                                if (result2.Read())
                                                {
                                                    string strTmpType = string.Empty;
                                                    if (!bSwFeesDesc)
                                                    {
                                                        m_dAmount = 0;
                                                        m_dAmount = result2.GetDouble("amount");
                                                        strAmt = string.Format("{0:#,##0.00}", m_dAmount);

                                                        strTmpType = result2.GetString("data_type");

                                                        if (strTmpType == "F" || strTmpType == "Q" || strTmpType == "A")
                                                        {
                                                            ScheduleFrm.ListOne[2, intRow1].Value = strTmpType;
                                                            ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
                                                        }
                                                        else
                                                        {
                                                            result2.Close();
                                                            result2.Query = strQuery.ToString();
                                                            if (result2.Execute())
                                                            {
                                                                while (result2.Read())
                                                                {
                                                                    m_dAmount = 0;
                                                                    m_dAmount = result2.GetDouble("amount");
                                                                    strAmt = string.Format("{0:#,##0.00}", m_dAmount);

                                                                    intQty = 0;
                                                                    intQty = result2.GetInt("qty2");
                                                                    if (intQty > 0)
                                                                    {
                                                                        if (intQty == 1)
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "Q";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
                                                                        }
                                                                        else
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "QR";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dblArea = 0;
                                                                        dblArea = result2.GetDouble("area2");

                                                                        if (dblArea > 0)
                                                                        {
                                                                            if (dblArea == 1)
                                                                            {
                                                                                ScheduleFrm.ListOne[2, intRow1].Value = "A";
                                                                                ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
                                                                            }
                                                                            else
                                                                            {
                                                                                ScheduleFrm.ListOne[2, intRow1].Value = "AR";
                                                                                ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                            }
                                                                        }

                                                                    }
                                                                }

                                                                if (strTmpType == "RR")
                                                                {
                                                                    ScheduleFrm.ListOne[2, intRow1].Value = "RR";
                                                                    ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                }
                                                                else if (strTmpType == "QR")
                                                                {
                                                                    ScheduleFrm.ListOne[2, intRow1].Value = "QR";
                                                                    ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                }
                                                                else if (strTmpType == "AR")
                                                                {
                                                                    ScheduleFrm.ListOne[2, intRow1].Value = "AR";
                                                                    ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                }

                                                                if (intRow1 == 1)
                                                                {
                                                                    m_iActiveRowBnsType = 0;
                                                                }
                                                            }
                                                            result2.Close();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result2.Close();
                                                        result2.Query = strQuery.ToString();
                                                        if (result2.Execute())
                                                        {
                                                            while (result2.Read())
                                                            {
                                                                m_dAmount = 0;
                                                                m_dAmount = result2.GetDouble("amount");
                                                                strAmt = string.Format("{0:#,##0.00}", m_dAmount);

                                                                intQty = 0;  //xXx//
                                                                intQty = result2.GetInt("qty2");

                                                                if (intQty > 0)
                                                                {
                                                                    if (intQty == 1)
                                                                    {
                                                                        ScheduleFrm.ListOne[2, intRow1].Value = "Q";
                                                                        ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
                                                                    }
                                                                    else
                                                                    {
                                                                        ScheduleFrm.ListOne[2, intRow1].Value = "QR";
                                                                        ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dblArea = 0;
                                                                    dblArea = result2.GetDouble("area2");

                                                                    if (dblArea > 0)
                                                                    {
                                                                        if (dblArea == 1)
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "A";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
                                                                        }
                                                                        else
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "AR";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dblGross = 0;
                                                                        dblGross = result2.GetDouble("gr_2");

                                                                        if (dblGross > 0)
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "RR";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = "";
                                                                        }
                                                                        else
                                                                        {
                                                                            ScheduleFrm.ListOne[2, intRow1].Value = "F";
                                                                            ScheduleFrm.ListOne[3, intRow1].Value = strAmt;

                                                                        }
                                                                    }
                                                                }

                                                                if (intRow1 == 1)
                                                                {
                                                                    m_iActiveRowBnsType = 0;
                                                                }
                                                            }
                                                        }
                                                        result2.Close();
                                                    }
                                                }
                                            }
                                            break;
                                        }

						            case 2: 
						                {
							                bool bSwRR;
								            if(bSwFeesDesc)
								            {
									            bSwRR = false;
								            }
								            else
								            {
									            bSwRR = true;
								            }
                                            result2.Close();
    								        
                                            result2.Query = strQuery.ToString();
                                            if(result2.Execute())
                                            {
                                                
                                                if(result2.Read())
                                                {
                                                    m_dAmount = 0;
								                    m_dAmount = result2.GetDouble("amount");
                                                    strAmt = string.Format("{0:#,##0.00}", m_dAmount);
    								
                                                    if(bSwRR)
                                                        ScheduleFrm.ListOne[2,intRow1].Value = result2.GetString("data_type");
                								    else
									                    ScheduleFrm.ListOne[2,intRow1].Value = "F";

								                    ScheduleFrm.ListOne[3,intRow1].Value = strAmt;
                                                }
                                            }
                                            result2.Close();
                                            break;
						                }
							    }

					            if(ScheduleFrm.ButtonEdit.Text == "&Update")
					            {
						            result2.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim());
                                    if(result2.Execute())
						            {
                                        if(result2.Read())
                                        {
                                        }
                                        else
						                {
                                            result2.Close();

                                            if(ScheduleFrm.Excess == "0.00" || ScheduleFrm.Excess == "")
								                dblExcess = 0;
							                if(ScheduleFrm.AddExcess == "0.00" || ScheduleFrm.AddExcess == "")
								                dblAddEx = 0;

                                            if(ScheduleFrm.ListOne[3,intRow1].Value.ToString() == "")
							                {
                                                result2.Query = "insert into tmp_fees_sched (fees_code, bns_code, qty1, qty2, area1, area2, gr_1, gr_2, ex_rate, plus_rate, amount, excess_no, excess_amt, data_type, bns_desc, rev_year) "
                                                        + " values(:1, :2, 0, 0, 0, 0, 0, 0, 0, 0, 0, :3, :4, :5, :6, :7) ";
                                                result2.AddParameter(":1", ScheduleFrm.FeesCode);
                                                result2.AddParameter(":2", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim().ToUpper());
                                                result2.AddParameter(":3", string.Format("{0:##.00}", dblExcess));
                                                result2.AddParameter(":4", string.Format("{0:##.00}", dblAddEx));
                                                result2.AddParameter(":5", ScheduleFrm.ListOne[2,intRow1].Value.ToString().Trim().ToUpper());
                                                result2.AddParameter(":6", StringUtilities.HandleApostrophe(ScheduleFrm.ListOne[1, intRow1].Value.ToString().Trim().ToUpper()));
                                                result2.AddParameter(":7", ScheduleFrm.RevYear);
                                            }
							                else
							                {
                                                if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "F")
								                {
                                                    result2.Query = "insert into tmp_fees_sched (fees_code, bns_code, qty1, qty2, area1, area2, gr_1, gr_2, ex_rate, plus_rate, amount, excess_no, excess_amt, data_type, bns_desc, rev_year) "
                                                        + " values(:1, :2, 0, 0, 0, 0, 0, 0, 0, 0, :3, :4, :5, :6, :7, :8) ";        
                                                    result2.AddParameter(":1", ScheduleFrm.FeesCode);
                                                    result2.AddParameter(":2", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":3", string.Format("{0:##.00}", Convert.ToDouble(ScheduleFrm.ListOne[3,intRow1].Value.ToString().Trim().ToUpper())));
                                                    result2.AddParameter(":4", string.Format("{0:##.00}", dblExcess));
                                                    result2.AddParameter(":5", string.Format("{0:##.00}", dblAddEx));
                                                    result2.AddParameter(":6", ScheduleFrm.ListOne[2,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":7", StringUtilities.HandleApostrophe(ScheduleFrm.ListOne[1, intRow1].Value.ToString().Trim().ToUpper()));
                                                    result2.AddParameter(":8", ScheduleFrm.RevYear);
								                }
								                else if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "Q")
								                {
                                                    result2.Query = "insert into tmp_fees_sched (fees_code, bns_code, qty1, qty2, area1, area2, gr_1, gr_2, ex_rate, plus_rate, amount, excess_no, excess_amt, data_type, bns_desc, rev_year) "
                                                        + " values(:1, :2, 1, 1, 0, 0, 0, 0, 0, 0, :3, :4, :5, :6, :7, :8) ";        
                                                    result2.AddParameter(":1", ScheduleFrm.FeesCode);
                                                    result2.AddParameter(":2", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":3", string.Format("{0:##.00}", Convert.ToDouble(ScheduleFrm.ListOne[3,intRow1].Value.ToString().Trim().ToUpper())));
                                                    result2.AddParameter(":4", string.Format("{0:##.00}", dblExcess));
                                                    result2.AddParameter(":5", string.Format("{0:##.00}", dblAddEx));
                                                    result2.AddParameter(":6", ScheduleFrm.ListOne[2,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":7", StringUtilities.HandleApostrophe(ScheduleFrm.ListOne[1, intRow1].Value.ToString().Trim().ToUpper()));
                                                    result2.AddParameter(":8", ScheduleFrm.RevYear);
                								}
								                else if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "A")
								                {
                                                    result2.Query = "insert into tmp_fees_sched (fees_code, bns_code, qty1, qty2, area1, area2, gr_1, gr_2, ex_rate, plus_rate, amount, excess_no, excess_amt, data_type, bns_desc, rev_year) "
                                                        + " values(:1, :2, 0, 0, 1, 1, 0, 0, 0, 0, :3, :4, :5, :6, :7, :8) ";        
                                                    result2.AddParameter(":1", ScheduleFrm.FeesCode);
                                                    result2.AddParameter(":2", ScheduleFrm.ListOne[0,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":3", string.Format("{0:##.00}", Convert.ToDouble(ScheduleFrm.ListOne[3,intRow1].Value.ToString().Trim().ToUpper())));
                                                    result2.AddParameter(":4", string.Format("{0:##.00}", dblExcess));
                                                    result2.AddParameter(":5", string.Format("{0:##.00}", dblAddEx));
                                                    result2.AddParameter(":6", ScheduleFrm.ListOne[2,intRow1].Value.ToString().Trim().ToUpper());
                                                    result2.AddParameter(":7", StringUtilities.HandleApostrophe(ScheduleFrm.ListOne[1, intRow1].Value.ToString().Trim().ToUpper()));
                                                    result2.AddParameter(":8", ScheduleFrm.RevYear);
								                }
							                }

                                            if(result2.ExecuteNonQuery() == 0)
                                            {
                                            }
							                
                                            m_boLoadRange = false;
                                            int intType = 0;

							                if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "RR" || ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "QR" 
                                                || ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "AR")
							                {
								                m_iRowVsfaBnsType = intRow1;
								                m_boLoadRange = true;
								                if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "RR")
									                intType = 0;
								                else if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "QR")
									                intType = 1;
								                else if(ScheduleFrm.ListOne[2,intRow1].Value.ToString() == "AR")
									                intType = 2;

								              //  SaveRange(iType, mv_sFeesCode, pApp->TrimAll(mc_vsfaBnsType.GetTextMatrix(iRow1,0)), pApp->TrimAll(mc_vsfaBnsType.GetTextMatrix(iRow1,1)));
							                }
						                }
                                    }
                                }
                            }
                        }

                        // temp
                        string sTmp = "";
                        string sTmp2 = "";
                        try
                        {
                            sTmp = ScheduleFrm.ListOne[2, intRow1].Value.ToString();
                        }
                        catch
                        {
                            sTmp = "";
                        }
                        sTmp2 = ScheduleFrm.ListOne[2, 0].Value.ToString();
                        // temp

                        intRow1++;
				    }
                    result.Close();

				    m_boLoadRange = false;
				    m_bSwBnsType = true;
                    string strTemp = string.Empty;
                    int w = 0;

                    if (intRow1 > 0)
                    {
                        if (ScheduleFrm.ListOne[0, 0].Value != null)
                        {
                            strTemp = ScheduleFrm.ListOne[0, 0].Value.ToString().Trim();
                            w = strTemp.Length;
                        }

                        if (ScheduleFrm.ListOne[2, 0].Value != null && ScheduleFrm.ListOne[3, 0].Value != null)
                        {
                            if (intRow1 > 1 && ScheduleFrm.ListOne[2, 0].Value.ToString().Trim() == "F" && ScheduleFrm.ListOne[3, 0].Value.ToString().Trim() == "0.00" && w == 2)
                            { }
                            else
                                m_bSwBnsType = false;
                        }
                        else
                            m_bSwBnsType = false;
                    }
                    else
                        m_bSwBnsType = false;

                    if(ScheduleFrm.ButtonEdit.Text == "&Update")
                        ScheduleFrm.ListOne.Rows.Add("");
			    }
			    else
			    {
				    if(ScheduleFrm.ButtonEdit.Text == "&Update")
                        ScheduleFrm.ListOne.Rows.Add("");
			    }
		    } 
		    else
		    {
                if(ScheduleFrm.ButtonEdit.Text == "&Update" && ScheduleFrm.ListOne.Rows.Count == 1 && ScheduleFrm.ListOne[0,intRow1].Value != null)
                {
                    ScheduleFrm.ListOne.Rows.Add("");
                }
	
		        if(ScheduleFrm.ButtonAdd.Text == "&Save")
			    {
                    result.Query = string.Format("select distinct(bns_code) from tmp_fees_sched where fees_code = '{0}' and trim(bns_code) like '{1}%'", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
                    if(result.Execute())
                    {
                        intRow1 = 0;
                        while (result.Read())
                        {
                            result2.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}'", ScheduleFrm.FeesCode, result.GetString("bns_code"));
                            if(result2.Execute())
                            {
                                string strTmpType = string.Empty;
                                string strTmpBnsCode = string.Empty;

                                if(result2.Read())
                                {
						    		m_dAmount = 0;
						            m_dAmount = result2.GetDouble("amount");

						            strAmt = string.Format("{0:##0.00}", m_dAmount);
						            ScheduleFrm.ListOne.Rows.Add("");
						
                                    strTmpType = result2.GetString("data_type");
						            strTmpBnsCode = result2.GetString("bns_code");

                                    ScheduleFrm.ListOne[0,intRow1].Value = strTmpBnsCode;
                                    ScheduleFrm.ListOne[1,intRow1].Value = result2.GetString("bns_desc");
                                    ScheduleFrm.ListOne[2,intRow1].Value = strTmpType;

                                    if (strTmpType == "F" || strTmpType == "Q" || strTmpType == "A")
                                        ScheduleFrm.ListOne[3, intRow1].Value = strAmt;
							            
                                }
                            }
                            result2.Close();

                        }	
					}
					else
					{
                        ScheduleFrm.FeesCode = "";
                    }
                    result.Close();

                    ScheduleFrm.ListOne.Rows.Add("");
                }
				
			}

            ScheduleFrm.SelectedMeans = "";
            ScheduleFrm.SelectedBnsSubCat = "";
            ScheduleFrm.SelectedBnsSubCode = "";

            if (intRow1 > 0)
            {
                if (ScheduleFrm.ListOne[0, 0].Value != null && ScheduleFrm.ListOne[2, 0].Value != null)
                {
                    ScheduleFrm.SelectedMeans = ScheduleFrm.ListOne[2, 0].Value.ToString();
                    ScheduleFrm.SelectedBnsSubCat = ScheduleFrm.ListOne[1, 0].Value.ToString();
                    ScheduleFrm.SelectedBnsSubCode = ScheduleFrm.ListOne[0, 0].Value.ToString();
                }
            }
            
        }

        public override void ClearLists()
        {
            
            ScheduleFrm.ListOne.Columns.Clear();
            ScheduleFrm.ListOne.Columns.Add("BNSSUBCODE", "Code");
            ScheduleFrm.ListOne.Columns.Add("BNSSUBDESC", "Business Description");
            //ScheduleFrm.ListOne.Columns.Add("BNSTYPE", "Type");
            ScheduleFrm.ListOne.Columns.Add("BNSAMOUNT", "Amount");

            DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
            comboBox.HeaderCell.Value = "Type";
            ScheduleFrm.ListOne.Columns.Insert(2, comboBox);
            comboBox.Items.AddRange("F", "Q", "A", "QR", "AR", "RR");
            ScheduleFrm.Means = "F-Fixed; Q-Qty; A-Area; QR-Qty Range; AR-Area Range; RR-Rate Range";
            
            ScheduleFrm.ListOne.RowHeadersVisible = false;
            ScheduleFrm.ListOne.Columns[0].Width = 50;
            ScheduleFrm.ListOne.Columns[1].Width = 175;
            ScheduleFrm.ListOne.Columns[2].Width = 50;
            ScheduleFrm.ListOne.Columns[3].Width = 70;
            ScheduleFrm.ListOne.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                                    
            //this.GenerateSubCode(0);
            ScheduleFrm.ListTwo.Columns.Clear();
            ScheduleFrm.ListTwo.Columns.Add("GR1", "GROSS 1");
            ScheduleFrm.ListTwo.Columns.Add("GR2", "GROSS 2");
            ScheduleFrm.ListTwo.Columns.Add("XRATE", "EX-RATE");
            ScheduleFrm.ListTwo.Columns.Add("PLUS", "%PLUS");
            ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");

            ScheduleFrm.ListTwo.RowHeadersVisible = false;
            ScheduleFrm.ListTwo.Columns[0].Width = 70;
            ScheduleFrm.ListTwo.Columns[1].Width = 80;
            ScheduleFrm.ListTwo.Columns[2].Width = 60;
            ScheduleFrm.ListTwo.Columns[3].Width = 60;
            ScheduleFrm.ListTwo.Columns[4].Width = 60;

            ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

            if ((this.ScheduleFrm.ButtonAdd.Text == "&Save" || this.ScheduleFrm.ButtonEdit.Text == "&Update")
                && ScheduleFrm.CheckGross.Checked == false)
                ScheduleFrm.ButtonConfig.Enabled = true;
            else
                ScheduleFrm.ButtonConfig.Enabled = false;

            if (this.ScheduleFrm.ButtonAdd.Text == "&Save" || this.ScheduleFrm.ButtonEdit.Text == "&Update")
                ScheduleFrm.ListOne.Rows.Add("");
        }

        public override void SelChangeBnsDesc()
        {
            OracleResultSet result = new OracleResultSet();

            result.Query = string.Format("select * from bns_table where fees_code = 'B' and trim(bns_desc) = '{0}' and rev_year = '{1}'", ScheduleFrm.ComboBnsDesc.Text, ScheduleFrm.RevYear);
            if (result.Execute())
            {
                if (result.Read())
                {
                    ScheduleFrm.BnsCode = result.GetString("bns_code");
                }
            }
            result.Close();

            this.LoadBussType();
            this.LoadTmpMinMaxTax(ScheduleFrm.FeesCode);    // RMC 20150305 corrections in schedules module
            this.LoadMinMaxTax(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);
            this.LoadQtrFeeConfig();
        }

        public override void SelChangeFeesDesc()
        {
            this.InitializeButtons();
            this.EnableControls(false);

            this.LoadFees(2);
            this.LoadBussType();
            this.LoadSurchIntDisc();
            this.LoadQtrFeeConfig();
        }

        public override void ClickListOne(int intRow, int intCol)
        {
          /*  if (intCol == 2)
            {
                this.combobox1.Location = this.ScheduleFrm.ListOne.GetCellDisplayRectangle(intCol, intRow, true).Location;
                this.combobox1.SelectedValue = this.ScheduleFrm.ListOne.CurrentCell.Value;
                this.combobox1.Width = this.ScheduleFrm.ListOne.Columns[intCol].Width;
                this.combobox1.Height = this.ScheduleFrm.ListOne.Rows[intRow].Height;
                this.combobox1.BringToFront();
                this.combobox1.Items.Clear();
                this.combobox1.Items.Add("F");
                this.combobox1.Items.Add("Q");
                this.combobox1.Show();
                //, "A", "QR", "AR", "RR");
            }*/

            try
            {
                if (ScheduleFrm.ListOne[2, intRow].Value != null)    // RMC 20161216 corrected data grid view error in schedule
                    ScheduleFrm.SelectedMeans = ScheduleFrm.ListOne[2, intRow].Value.ToString();
                else
                    ScheduleFrm.SelectedMeans = ""; // RMC 20161216 corrected data grid view error in schedule

                if (ScheduleFrm.ListOne[1, intRow].Value != null)    // RMC 20161216 corrected data grid view error in schedule
                    ScheduleFrm.SelectedBnsSubCat = ScheduleFrm.ListOne[1, intRow].Value.ToString();
                else
                    ScheduleFrm.SelectedBnsSubCat = ""; // RMC 20161216 corrected data grid view error in schedule

                if (ScheduleFrm.ListOne[0, intRow].Value != null)    // RMC 20161216 corrected data grid view error in schedule
                    ScheduleFrm.SelectedBnsSubCode = ScheduleFrm.ListOne[0, intRow].Value.ToString();
                else
                    ScheduleFrm.SelectedBnsSubCode = "";    // RMC 20161216 corrected data grid view error in schedule

                //MCR 20141111(s)
                m_iCurrRow = intRow;
                m_iCurrCol = intCol;
                //MCR 20141111(e)
            }
            catch
            {
                ScheduleFrm.SelectedMeans = "";
                ScheduleFrm.SelectedBnsSubCat = "";
                ScheduleFrm.SelectedBnsSubCode = "";
                // RMC 20150305 corrections in schedules module (s)
                m_iCurrRow = intRow;
                m_iCurrCol = intCol;
                // RMC 20150305 corrections in schedules module (e)
            }

            try
            {
                this.LoadFixSched(ScheduleFrm.SelectedBnsSubCode, ScheduleFrm.SelectedMeans);
                this.LoadMinMaxTax(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);

                if (intCol == 0)
                    ScheduleFrm.ListOne.ReadOnly = true;
                else
                {
                    if (ScheduleFrm.ButtonAdd.Text == "&Save" || ScheduleFrm.ButtonEdit.Text == "&Update")
                        ScheduleFrm.ListOne.ReadOnly = false;
                }
            }
            catch { }
        }

        private void LoadFixSched(string strSubCode, string strMeans)
        {
            try
            {
                // RMC 20161216 corrected data grid view error in schedule (s)
                if (strSubCode == "" || strMeans == "")
                    return;
                // RMC 20161216 corrected data grid view error in schedule (e)

                OracleResultSet result = new OracleResultSet();
                OracleResultSet result2 = new OracleResultSet();
                StringBuilder strQuery = new StringBuilder();

                string strEntry1 = string.Empty;
                string strEntry2 = string.Empty;
                string strAmount = string.Empty;
                string strExRate = string.Empty;
                string strPlusRate = string.Empty;
                bool blnWithTemp = false;

                int intTmpRow = 0;

                strQuery.Append(string.Format("select * from tmp_fees_sched where bns_code = '{0}' and fees_code = '{1}' order by gr_1", strSubCode, ScheduleFrm.FeesCode));
                result.Query = strQuery.ToString();
                if (result.Execute())
                {
                    strQuery = new StringBuilder();

                    if (result.Read())
                    {
                        result.Close();
                        strQuery.Append(string.Format("select * from tmp_fees_sched where bns_code = '{0}' and fees_code = '{1}'", strSubCode, ScheduleFrm.FeesCode));
                        result2.Query = string.Format("select * from tmp_fees_sched where bns_code = '{0}' and fees_code = '{1}'", strSubCode, ScheduleFrm.FeesCode);
                        blnWithTemp = true;
                    }
                    else
                    {
                        result.Close();
                        strQuery.Append(string.Format("select * from fees_sched where bns_code = '{0}' and fees_code = '{1}' and rev_year = '{2}'", strSubCode, ScheduleFrm.FeesCode, ScheduleFrm.RevYear));
                        result2.Query = string.Format("select * from excess_sched where bns_code = '{0}' and fees_code = '{1}' and rev_year = '{2}'", strSubCode, ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                        blnWithTemp = false;

                    }
                }

                this.LoadFixSchedHeader(strMeans);

                if (strMeans == "QR")
                {
                    strQuery.Append(" order by qty1");
                    result.Query = strQuery.ToString();
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            ScheduleFrm.ListTwo.Rows.Add("");
                            strEntry1 = string.Format("{0:##0}", result.GetInt("qty1"));
                            strEntry2 = string.Format("{0:##0}", result.GetInt("qty2"));
                            strAmount = string.Format("{0:#,##0.00}", result.GetDouble("amount"));
                            strExRate = string.Format("{0:##0.000000}", result.GetDouble("ex_rate"));
                            strPlusRate = string.Format("{0:##0.000000}", result.GetDouble("plus_rate"));

                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    ScheduleFrm.Excess = Convert.ToString(result2.GetInt("excess_no"));
                                    ScheduleFrm.AddExcess = Convert.ToString(result2.GetInt("excess_amt"));
                                }
                                else
                                {
                                    ScheduleFrm.Excess = "0.00";
                                    ScheduleFrm.AddExcess = "0.00";
                                }
                            }
                            result2.Close();

                            intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                            ScheduleFrm.ListTwo[0, intTmpRow].Value = strEntry1;
                            ScheduleFrm.ListTwo[1, intTmpRow].Value = strEntry2;
                            ScheduleFrm.ListTwo[2, intTmpRow].Value = strExRate;
                            ScheduleFrm.ListTwo[3, intTmpRow].Value = strPlusRate;
                            ScheduleFrm.ListTwo[4, intTmpRow].Value = strAmount;


                        }
                    }
                    result.Close();
                }
                if (strMeans == "RR")
                {
                    strQuery.Append(" order by gr_1");
                    result.Query = strQuery.ToString();
                    if (result.Execute())
                    {

                        while (result.Read())
                        {
                            ScheduleFrm.ListTwo.Rows.Add("");
                            strEntry1 = string.Format("{0:##0.00}", result.GetDouble("gr_1"));
                            strEntry2 = string.Format("{0:##0.00}", result.GetDouble("gr_2"));
                            strAmount = string.Format("{0:#,##0.00}", result.GetDouble("amount"));
                            strExRate = string.Format("{0:##0.000000}", result.GetDouble("ex_rate"));
                            strPlusRate = string.Format("{0:##0.000000}", result.GetDouble("plus_rate"));

                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    ScheduleFrm.Excess = string.Format("{0:##0.00}", result2.GetInt("excess_no"));
                                    ScheduleFrm.AddExcess = string.Format("{0:##0.00}", result2.GetInt("excess_amt"));
                                }
                                else
                                {
                                    ScheduleFrm.Excess = "0.00";
                                    ScheduleFrm.AddExcess = "0.00";
                                }
                            }
                            result2.Close();

                            intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                            ScheduleFrm.ListTwo[0, intTmpRow].Value = strEntry1;
                            ScheduleFrm.ListTwo[1, intTmpRow].Value = strEntry2;
                            ScheduleFrm.ListTwo[2, intTmpRow].Value = strExRate;
                            ScheduleFrm.ListTwo[3, intTmpRow].Value = strPlusRate;
                            ScheduleFrm.ListTwo[4, intTmpRow].Value = strAmount;



                        }
                    }
                    result.Close();
                }
                if (strMeans == "AR")
                {
                    strQuery.Append(" order by area1");
                    result.Query = strQuery.ToString();
                    if (result.Execute())
                    {

                        while (result.Read())
                        {
                            ScheduleFrm.ListTwo.Rows.Add("");
                            strEntry1 = string.Format("{0:##0.00}", result.GetDouble("area1"));
                            strEntry2 = string.Format("{0:##0.00}", result.GetDouble("area2"));
                            strAmount = string.Format("{0:#,##0.00}", result.GetDouble("amount"));
                            strExRate = string.Format("{0:##0.000000}", result.GetDouble("ex_rate"));
                            strPlusRate = string.Format("{0:##0.000000}", result.GetDouble("plus_rate"));

                            if (result2.Execute())
                            {
                                if (result2.Read())
                                {
                                    ScheduleFrm.Excess = string.Format("{0:##0.00}", result2.GetInt("excess_no"));
                                    ScheduleFrm.AddExcess = string.Format("{0:##0.00}", result2.GetInt("excess_amt"));
                                }
                                else
                                {
                                    ScheduleFrm.Excess = "0.00";
                                    ScheduleFrm.AddExcess = "0.00";
                                }
                            }
                            result2.Close();

                            intTmpRow = ScheduleFrm.ListTwo.Rows.Count - 1;

                            ScheduleFrm.ListTwo[0, intTmpRow].Value = strEntry1;
                            ScheduleFrm.ListTwo[1, intTmpRow].Value = strEntry2;
                            ScheduleFrm.ListTwo[2, intTmpRow].Value = strExRate;
                            ScheduleFrm.ListTwo[3, intTmpRow].Value = strPlusRate;
                            ScheduleFrm.ListTwo[4, intTmpRow].Value = strAmount;



                        }
                    }
                    result.Close();
                }

                if (ScheduleFrm.ListTwo.Rows.Count == 0 && ScheduleFrm.SelectedMeans != "")
                {
                    if (strMeans == "F" || strMeans == "Q" || strMeans == "A")
                    {

                    }
                    else
                    {
                        ScheduleFrm.ListTwo.Rows.Add("");
                    }
                }

                // RMC 20140107 (S)
                if (strMeans == "")
                {
                    ScheduleFrm.Excess = "";
                    ScheduleFrm.AddExcess = "";
                }
                // RMC 20140107 (E)
            }
            catch { }
        }

        private void LoadFixSchedHeader(string strMeans)
        {
            try
            {
                if (strMeans == "QR")
                {
                    ScheduleFrm.ListTwo.Columns.Clear();
                    ScheduleFrm.ListTwo.Columns.Add("GR1", "Quantity 1");
                    ScheduleFrm.ListTwo.Columns.Add("GR2", "Quantity 2");
                    ScheduleFrm.ListTwo.Columns.Add("XRATE", "");
                    ScheduleFrm.ListTwo.Columns.Add("PLUS", "");
                    ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                    ScheduleFrm.ListTwo.RowHeadersVisible = false;
                    ScheduleFrm.ListTwo.Columns[0].Width = 100;
                    ScheduleFrm.ListTwo.Columns[1].Width = 100;
                    ScheduleFrm.ListTwo.Columns[2].Visible = false;
                    ScheduleFrm.ListTwo.Columns[3].Visible = false;
                    ScheduleFrm.ListTwo.Columns[4].Width = 100;
                    ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else if (strMeans == "AR")
                {
                    ScheduleFrm.ListTwo.Columns.Clear();
                    ScheduleFrm.ListTwo.Columns.Add("GR1", "Area 1");
                    ScheduleFrm.ListTwo.Columns.Add("GR2", "Area 2");
                    ScheduleFrm.ListTwo.Columns.Add("XRATE", "");
                    ScheduleFrm.ListTwo.Columns.Add("PLUS", "");
                    ScheduleFrm.ListTwo.Columns.Add("AMT", "AMOUNT");
                    ScheduleFrm.ListTwo.RowHeadersVisible = false;
                    ScheduleFrm.ListTwo.Columns[0].Width = 100;
                    ScheduleFrm.ListTwo.Columns[1].Width = 100;
                    ScheduleFrm.ListTwo.Columns[2].Visible = false;
                    ScheduleFrm.ListTwo.Columns[3].Visible = false;
                    ScheduleFrm.ListTwo.Columns[4].Width = 100;
                    ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else if (strMeans == "RR")
                {
                    string strConfig = string.Empty;

                    strConfig = this.GetRateConfig(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);

                    ScheduleFrm.ListTwo.Columns.Clear();
                    if (strConfig == "02")
                    {
                        ScheduleFrm.ListTwo.Columns.Add("PA1", "Paid Amt 1");
                        ScheduleFrm.ListTwo.Columns.Add("PA2", "Paid Amt 2");
                    }
                    else if (strConfig == "03")
                    {
                        ScheduleFrm.ListTwo.Columns.Add("PA1", "Due Amt 1");
                        ScheduleFrm.ListTwo.Columns.Add("PA2", "Due Amt 2");
                    }
                    else
                    {
                        ScheduleFrm.ListTwo.Columns.Add("PA1", "Gross 1");
                        ScheduleFrm.ListTwo.Columns.Add("PA2", "Gross 2");
                    }

                    ScheduleFrm.ListTwo.Columns.Add("XRATE", "ExRate");
                    ScheduleFrm.ListTwo.Columns.Add("PLUS", "%Plus");
                    ScheduleFrm.ListTwo.Columns.Add("AMT", "Amount");
                    ScheduleFrm.ListTwo.RowHeadersVisible = false;
                    ScheduleFrm.ListTwo.Columns[0].Width = 100;
                    ScheduleFrm.ListTwo.Columns[1].Width = 100;
                    ScheduleFrm.ListTwo.Columns[2].Width = 70;
                    ScheduleFrm.ListTwo.Columns[3].Width = 70;
                    ScheduleFrm.ListTwo.Columns[4].Width = 70;
                    ScheduleFrm.ListTwo.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    ScheduleFrm.ListTwo.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    ScheduleFrm.ListTwo.Columns.Clear();
                }

            }
            catch { }


        }

        

        private string GetRateConfig(string strFeesCode, string strDetBussCode)
        {
            OracleResultSet result = new OracleResultSet();
            string strConfig = string.Empty;

            result.Query = string.Format("select * from rate_config_tbl where fees_code = '{0}' and det_buss_code = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);
			if(result.Execute())
            {
                if(result.Read())
				{
					strConfig = result.GetString("config_code");
                }
            }
            result.Close();

            return strConfig;
        }

        public override void ChangedTmpSubCode()
        {
            if (ScheduleFrm.SelectedBnsSubCode != "")    // RMC 20161216 corrected data grid view error in schedule
                this.LoadFixSched(ScheduleFrm.SelectedBnsSubCode, ScheduleFrm.SelectedMeans);
        }

        public override void ChangedMeans()
        {
            if (ScheduleFrm.SelectedBnsSubCode != "")   // RMC 20161216 corrected data grid view error in schedule
                this.LoadFixSched(ScheduleFrm.SelectedBnsSubCode, ScheduleFrm.SelectedMeans);
        }

        public override void Add()
        {
            OracleResultSet result = new OracleResultSet();

            if (ScheduleFrm.ButtonAdd.Text == "&Add")
            {
                ScheduleFrm.ButtonAdd.Text = "&Save";
                ScheduleFrm.ButtonClose.Text = "&Cancel";
                ScheduleFrm.ButtonEdit.Enabled = false;
                ScheduleFrm.ButtonDelete.Enabled = false;
                this.EnableControls(true);
                ScheduleFrm.Excess = "";    // RMC 20140107
                ScheduleFrm.AddExcess = ""; // RMC 20140107

                this.ClearLists();
                this.SetFeesTerm("");
                this.ScheduleFrm.CheckFeesInt.Checked = false;
                this.ScheduleFrm.CheckFeesSurch.Checked = false;
               
                int intCnt = 0;
                result.Query = string.Format("select * from tax_and_fees_table where rev_year = '{0}' order by fees_code desc", ScheduleFrm.RevYear);
                if (result.Execute())
                {
                    if (result.Read())
                        intCnt = Convert.ToInt32(result.GetString("fees_code"));
                }
                result.Close();

                intCnt = intCnt + 1;
                ScheduleFrm.FeesCode = string.Format("{0:0#}", intCnt);
                ScheduleFrm.ComboFeesDesc.Text = "";
                ScheduleFrm.ComboFeesDesc.Focus();
            }
            else
            {
                if (!ValidateQtrFeeConfig())
                    return;

                if(ScheduleFrm.FeesDesc.Trim() == "")
                {
                    MessageBox.Show("Fees Description required input.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    result.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%'", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            bool bSw;
                            bSw = this.ValidateSubCategory();

                            if (bSw == true)  
                            {
                                if (MessageBox.Show("Schedule with Business Description    \n" + m_strBnsType + "\nwill not be save.\nContinue anyway?\n\nYES cancel changes\nNO edit schedule.", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    this.InitializeButtons();
                                    this.EnableControls(false);
                                    this.LoadFees(0);   //combo fees
                                    this.LoadBussType();    //datagrid1
                                }  
                            }
                            else
                            {
                                if (MessageBox.Show("Save schedule for " + ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper() + "?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    // RMC 20111012 modified query commit in scheds (s)
                                    pCmd = new OracleResultSet();
                                    pCmd.Transaction = true;
                                    // RMC 20111012 modified query commit in scheds (e)

                                    this.SaveChanges(true);

                                    this.DeleteTmpTable(0);

                                    // RMC 20111012 modified query commit in scheds (s)
                                    if (!pCmd.Commit())
                                    {
                                        pCmd.Rollback();
                                        pCmd.Close();
                                        MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                                        return;
                                    }

                                    pCmd.Close();
                                    // RMC 20111012 modified query commit in scheds (e)

                                    this.InitialFormLoad();
                                    this.InitializeButtons();
                                    this.EnableControls(false);
                                    this.LoadFees(0);   //combo fees
                                    this.LoadBussType();    //datagrid1
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("No record to save.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }


                    }
                }
            }
        }

        private bool ValidateSubCategory()
        {
            OracleResultSet result = new OracleResultSet();
            string strType = string.Empty;
            string strBnsCode = string.Empty;
            m_strBnsType = "";
            int intCtr = 1;

            for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
            {
                if(ScheduleFrm.ListOne[2, i].Value != null)
                    strType = ScheduleFrm.ListOne[2, i].Value.ToString();
                                    
		    	if(strType == "RR" || strType == "QR" || strType == "AR")
		        {
			        strBnsCode = ScheduleFrm.ListOne[0, i].Value.ToString();

                    result.Query = string.Format("select * from tmp_fees_sched where bns_code = '{0}' and fees_code = '{1}'",strBnsCode,ScheduleFrm.FeesCode);
                    if(result.Execute())
                    {
                        if (result.Read())
                        {
                        }
                        else
                        {
                            if(ScheduleFrm.ListOne[1, i].Value != null)
                                m_strBnsType = ScheduleFrm.ListOne[1, i].Value.ToString();
                            intCtr++;
                        }
			        }
                    result.Close();
		        }

                if (strType == "F" || strType == "Q" || strType == "A")
		        {
                    if(ScheduleFrm.ListOne[0,i].Value.ToString() != "" && ScheduleFrm.ListOne[3,i].Value.ToString() == "") 
			        {
                        m_strBnsType = ScheduleFrm.ListOne[1, i].Value.ToString();
				        intCtr++;
			        }
		        }
	        }

            

	        if(intCtr > 0)
		        return false;
	        else
		        return true;
        }

        private void SaveChanges(bool bSwSaveChanges)
        {
            
            if (bSwSaveChanges) //save
            {
                if (ScheduleFrm.ButtonAdd.Text == "&Save")
                    this.SaveSchedules(1); //add-save
                else
                    this.SaveSchedules(2); //edit-update

                this.SaveRangeConfig();

                this.SaveSurchIntDisc();
                this.SaveMinMaxTax(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);
                this.SaveQtrConfig();

                MessageBox.Show("New schedules saved.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else  //cancel
            {
                if (ScheduleFrm.ButtonAdd.Text == "&Save")
                    this.LoadFees(0);
                else
                    this.LoadFees(1);
                
            }

            /*this.InitializeButtons();
            this.EnableControls(false);
            this.LoadFees(0);   //combo fees
            this.LoadBussType();    //datagrid1*/
            // RMC 20150305 corrections in schedules module, put rem
        }

        private void SaveSchedules(int iModule)
        { 
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            // RMC 20111012 modified query commit in scheds, changed commands to pCmd
            
            string strFeesTerm = string.Empty;
            string strPenalty = string.Empty;
            string strSurch = string.Empty;
            string strFeesCode = string.Empty;
            string strBnsCode = string.Empty;
            string strRevYear = string.Empty;
            
            strFeesTerm = this.GetFeesTerm();

            if(ScheduleFrm.CheckFeesInt.Checked == true)
	            strPenalty = "Y";
	        else
		        strPenalty = "N";

	        if(ScheduleFrm.CheckFeesSurch.Checked == true)
		        strSurch = "Y";
	        else
		        strSurch = "N";

            switch (iModule)
            {
                case 1: //add-save
                    pCmd.Query = "insert into tax_and_fees_table (fees_code, fees_desc, fees_type, fees_term, fees_withsurch, fees_withpen, rev_year) values (:1,:2,:3,:4,:5,:6,:7)";
                    pCmd.AddParameter(":1", ScheduleFrm.FeesCode.Trim());
                    pCmd.AddParameter(":2", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper()));
                    pCmd.AddParameter(":3", "FS");
                    pCmd.AddParameter(":4", strFeesTerm);
                    pCmd.AddParameter(":5", strSurch);
                    pCmd.AddParameter(":6", strPenalty);
                    pCmd.AddParameter(":7", ScheduleFrm.RevYear);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }

                    this.SaveFeesSched("Add");
                    break;

                case 2: //edit-update
                    pCmd.Query = string.Format("update tax_and_fees_table set fees_desc = '{0}', fees_term = '{1}', "
                        + " fees_withsurch = '{2}', fees_withpen = '{3}' where fees_code = '{4}' "
                        + " and rev_year = '{5}'", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper()), strFeesTerm, strSurch, strPenalty, ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }

                    result.Query = "select * from del_sched";
                    if (result.Execute())
                    {
                        while (result.Read())
                        {
                            strFeesCode = result.GetString("fees_code");
                            strBnsCode = result.GetString("bns_code");
                            strRevYear = result.GetString("rev_year");

                            pCmd.Query = string.Format("delete from excess_sched where fees_code = '{0}' and bns_code = '{1}' "
                                + "and rev_year = '{2}' ", strFeesCode, strBnsCode, strRevYear);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }

                            pCmd.Query = string.Format("delete from fees_sched where fees_code = '{0}' and bns_code = '{1}' "
                                + "and rev_year = '{2}' ", strFeesCode, strBnsCode, strRevYear);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }

                            pCmd.Query = string.Format("delete from bns_table where fees_code = '{0}' and bns_code = '{1}' "
                                + "and rev_year = '{2}' ", strFeesCode, strBnsCode, strRevYear);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }
                        }

                        pCmd.Query = "delete from del_sched";
                        if (pCmd.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    result.Close();

                    this.SaveFeesSched("Edit");
                    break;
            }

            pCmd.Query = "delete from tmp_fees_sched";
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }
        }

        private void SaveFeesSched(string strMode)
        {
            // RMC 20111012 modified query commit in scheds, changed commands to pCmd

            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            OracleResultSet result3 = new OracleResultSet();
            string strModCode = string.Empty;
            string strObject = string.Empty;

            string strFeesCode = string.Empty;
		    string strBnsCode = string.Empty;
			string strBnsDesc = string.Empty;
		    string strExcessNo = string.Empty;
		    string strExcessAmt = string.Empty;
		    string strType = string.Empty;
            string strQty1 = string.Empty;
            string strQty2 = string.Empty;
            string strArea1 = string.Empty;
            string strArea2 = string.Empty;
            string strGR1 = string.Empty;
            string strGR2 = string.Empty;
            string strExRate = string.Empty;
            string strPlusRate = string.Empty;
            string strAmt = string.Empty;
            string strRevYear = string.Empty;

            if(strMode == "Add")
                strModCode = "AUTF-A";
            else if(strMode == "Edit")
                strModCode = "AUTF-E";

            if (strMode == "Edit")
            {
                result.Query = string.Format("select distinct(fees_code),bns_code,bns_desc,excess_no,excess_amt,data_type from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%'", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
                if (result.Execute())
                {
                    if (result.Read())
                    {
                    }
                    else
                    {
                        string strTmpBnsDesc = string.Empty;
                        string strTmpBnsCode = string.Empty;

                        // for no edited sched, just edit sub buss desc
                        for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                        {
                            try
                            {
                                strTmpBnsCode = ScheduleFrm.ListOne[0, i].Value.ToString();
                                strTmpBnsDesc = ScheduleFrm.ListOne[1, i].Value.ToString();

                                pCmd.Query = "update bns_table set bns_desc = :1 where fees_code = :2 and bns_code = :3 and rev_year = :4";
                                pCmd.AddParameter(":1", StringUtilities.HandleApostrophe(strTmpBnsDesc));
                                pCmd.AddParameter(":2", ScheduleFrm.FeesCode.Trim());
                                pCmd.AddParameter(":3", strTmpBnsCode);
                                pCmd.AddParameter(":4", ScheduleFrm.RevYear);
                                if (pCmd.ExecuteNonQuery() == 0)
                                {
                                }
                            }
                            catch
                            {
                            }

                        }
                    }
                }
                result.Close();
            }

            result.Query = string.Format("select distinct(fees_code),bns_code,bns_desc,excess_no,excess_amt,data_type from tmp_fees_sched where fees_code = '{0}' and bns_code like '{1}%'", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);		
	        if(result.Execute())
	        {
                while(result.Read())
		        {
                    strFeesCode = result.GetString("fees_code");
			        strBnsCode = result.GetString("bns_code");
			        strBnsDesc = result.GetString("bns_desc");
			        strExcessNo = string.Format("{0:##0.000000}", result.GetDouble("excess_no"));
			        strExcessAmt = string.Format("{0:##0.000000}", result.GetDouble("excess_amt"));
			        strType = result.GetString("data_type");

                    strObject = "Fees: " +strFeesCode + " Bns: " +strBnsCode;
                   
			        if(strType == "RR" || strType == "QR" || strType == "AR")
			        {
                        pCmd.Query = string.Format("delete from excess_sched where fees_code = '{0}' and bns_code = '{1}' "
                            + "and rev_year = '{2}'", strFeesCode, strBnsCode,ScheduleFrm.RevYear);
                        if (pCmd.ExecuteNonQuery() == 0)
                        {
                        }

                        pCmd.Query = "insert into excess_sched (fees_code, bns_code, excess_no, excess_amt, rev_year) values (:1,:2,:3,:4,:5)";
                        pCmd.AddParameter(":1", strFeesCode);
                        pCmd.AddParameter(":2", strBnsCode);
                        pCmd.AddParameter(":3", strExcessNo);
                        pCmd.AddParameter(":4", strExcessAmt);
                        pCmd.AddParameter(":5", ScheduleFrm.RevYear);
                        if (pCmd.ExecuteNonQuery() == 0)
                        {
                        }
			        }

                    pCmd.Query = string.Format("delete from bns_table where fees_code = '{0}' and bns_code = '{1}' "
                        + "and rev_year = '{2}'", strFeesCode, strBnsCode, ScheduleFrm.RevYear);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }

                    pCmd.Query = "insert into bns_table (fees_code, bns_code, bns_desc, rev_year) values (:1, :2, :3, :4)";
                    pCmd.AddParameter(":1", strFeesCode);
                    pCmd.AddParameter(":2", strBnsCode);
                    pCmd.AddParameter(":3", StringUtilities.HandleApostrophe(strBnsDesc).Trim());   // RMC 20120109 added trim in saving bns schedule
                    pCmd.AddParameter(":4", ScheduleFrm.RevYear);
                    if (pCmd.ExecuteNonQuery() == 0)
                    {
                    }

			        result2.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}'", strFeesCode, strBnsCode);
			        if(result2.Execute())
			        {
                        if(result2.Read())
                        {
                            pCmd.Query = string.Format("delete from fees_sched where fees_code = '{0}' and bns_code = '{1}' "
                                + "and rev_year = '{2}' ", ScheduleFrm.FeesCode, strBnsCode, ScheduleFrm.RevYear);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }
                        }
                    }
                    result2.Close();

                    if(result2.Execute()) 
                    {
                        while(result2.Read())
		                {
                            strQty1 = string.Format("{0:##0}", result2.GetInt("qty1"));
                            strQty2 = string.Format("{0:##0}", result2.GetInt("qty2"));
				            strArea1 = string.Format("{0:##0.00}",result2.GetDouble("area1"));
                            strArea2 = string.Format("{0:##0.00}",result2.GetDouble("area2"));
				            strGR1 = string.Format("{0:##0.00}", result2.GetDouble("gr_1"));
                            strGR2 =  string.Format("{0:##0.00}", result2.GetDouble("gr_2"));
				            strExRate = string.Format("{0:##0.000000}",result2.GetDouble("ex_rate"));
                            strPlusRate = string.Format("{0:##0.000000}", result2.GetDouble("plus_rate"));
                            strAmt = string.Format("{0:##0.00}", result2.GetDouble("amount"));
                            strRevYear = result2.GetString("rev_year");

                            pCmd.Query = "insert into fees_sched (fees_code, bns_code, qty1, qty2, area1, area2, gr_1, gr_2, ex_rate, plus_rate, amount, rev_year) values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12)";
                            pCmd.AddParameter(":1", strFeesCode);
                            pCmd.AddParameter(":2", strBnsCode);
                            pCmd.AddParameter(":3", strQty1);
                            pCmd.AddParameter(":4", strQty2);
                            pCmd.AddParameter(":5", strArea1);
                            pCmd.AddParameter(":6", strArea2);
                            pCmd.AddParameter(":7", strGR1);
                            pCmd.AddParameter(":8", strGR2);
                            pCmd.AddParameter(":9", strExRate);
                            pCmd.AddParameter(":10", strPlusRate);
                            pCmd.AddParameter(":11", strAmt);
                            pCmd.AddParameter(":12", strRevYear);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }

                            if (AuditTrail.InsertTrail(strModCode, "fees_sched", StringUtilities.HandleApostrophe(strObject)) == 0)
                            {
                                result.Rollback();
                                result.Close();
                                MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
			                
                        }
                    }
                    result2.Close();
				    
			    }
            }
            result.Close();
        }

        private void SaveRangeConfig()
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            pCmd.Query = string.Format("delete from rate_config_tbl where fees_code = '{0}'", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("delete from rate_config_tbl_ref where fees_code = '{0}'", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("insert into rate_config_tbl (select * from rate_config_tbl_tmp where fees_code = '{0}')", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("insert into rate_config_tbl_ref (select * from rate_config_tbl_ref_tmp where fees_code = '{0}')", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("delete from rate_config_tbl_tmp where fees_code = '{0}'", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("delete from rate_config_tbl_ref_tmp where fees_code = '{0}'", ScheduleFrm.FeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }
        }

        public override void Edit()
        {
            OracleResultSet result = new OracleResultSet();

            if (ScheduleFrm.ButtonEdit.Text == "&Edit")
            {
                if (!this.CheckSched(StringUtilities.Left(ScheduleFrm.BnsCode, 2), ScheduleFrm.FeesCode) || Granted.Grant("AUCS"))
                {
                    ScheduleFrm.ButtonEdit.Text = "&Update";
                    ScheduleFrm.ButtonClose.Text = "&Cancel";
                    ScheduleFrm.ButtonAdd.Enabled = false;
                    ScheduleFrm.ButtonDelete.Enabled = false;
                    ScheduleFrm.CheckFees.Enabled = false;
                    this.EnableControls(true);
                    ScheduleFrm.ButtonConfig.Enabled = true;
                    ScheduleFrm.ListOne.Rows.Add("");
                    //ScheduleFrm.Excess = "";    // RMC 20140107   // RMC 20161213 mods in schedule, put rem       
                    //ScheduleFrm.AddExcess = ""; // RMC 20140107   // RMC 20161213 mods in schedule, put rem
                }
                else
                {
                    MessageBox.Show("You cannot edit this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                               
            }
            else
            {
                if (MessageBox.Show("Do you want to save changes?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateQtrFeeConfig())
                        return;

                    // RMC 20111012 modified query commit in scheds (s)
                    pCmd = new OracleResultSet();
                    pCmd.Transaction = true;
                    // RMC 20111012 modified query commit in scheds (e)

                    this.SaveChanges(true);

                    this.DeleteTmpTable(0);

                    // RMC 20111012 modified query commit in scheds (s)
                    if (!pCmd.Commit())
                    {
                        pCmd.Rollback();
                        pCmd.Close();
                        MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    pCmd.Close();
                    // RMC 20111012 modified query commit in scheds (e)

                    this.InitialFormLoad();
                    this.InitializeButtons();
                    this.EnableControls(false);
                    this.LoadFees(0);   //combo fees
                    this.LoadBussType();    //datagrid1
                    
                }
            }
        }

        public override void Delete()
        {
            if (!this.CheckSched(StringUtilities.Left(ScheduleFrm.BnsCode, 2),ScheduleFrm.FeesCode))
            {
                if (MessageBox.Show("WARNING: This will delete the main fee along with its sub-categories.\nProceed?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (MessageBox.Show("About to Delete?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // RMC 20111012 modified query commit in scheds (s)
                        pCmd = new OracleResultSet();
                        pCmd.Transaction = true;
                        // RMC 20111012 modified query commit in scheds (e)

                        this.DeleteTables(true, "","");

                        // RMC 20111012 modified query commit in scheds (s)
                        if (!pCmd.Commit())
                        {
                            pCmd.Rollback();
                            pCmd.Close();
                            MessageBox.Show("Failed to update records.", string.Empty, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        pCmd.Close();
                        // RMC 20111012 modified query commit in scheds (e)

                        this.InitialFormLoad();
                        MessageBox.Show("Record successfully deleted.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("You cannot delete this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void DeleteTables(bool blnMainBuss, string strSubBnsCode, string strSubBnsDesc)
        {
            OracleResultSet result = new OracleResultSet();
            string strObj = string.Empty;

            if(blnMainBuss)
            {
                result.Query = string.Format("delete from tax_and_fees_table where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                if (AuditTrail.InsertTrail("AUTF-D", "tax_and_fees_table", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result.Query = string.Format("delete from excess_sched where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
                if (AuditTrail.InsertTrail("AUTF-D", "excess_sched", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result.Query = string.Format("delete from fees_sched where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from qtr_fee_config where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
                if (AuditTrail.InsertTrail("AUTF-D", "fees_sched", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result.Query = string.Format("delete from bns_table where fees_code = '{0}' and rev_year = '{1}'", ScheduleFrm.FeesCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
                if (AuditTrail.InsertTrail("AUTF-D", "bns_table", StringUtilities.HandleApostrophe(ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper())) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                strObj = ScheduleFrm.FeesCode;
                strObj += "/" + ScheduleFrm.ComboFeesDesc.Text.Trim().ToUpper();
                strObj += "/" + strSubBnsCode;
                strObj += "/" + strSubBnsDesc;

                result.Query = string.Format("delete from excess_sched where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", ScheduleFrm.FeesCode, strSubBnsCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from fees_sched where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", ScheduleFrm.FeesCode, strSubBnsCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from bns_table where fees_code = '{0}' and bns_code = '{1}' and rev_year = '{2}'", ScheduleFrm.FeesCode, strSubBnsCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = "insert into del_sched (fees_code, bns_code, bns_desc, rev_year) values (:1,:2,:3,:4) ";
                result.AddParameter(":1", ScheduleFrm.FeesCode);
                result.AddParameter(":2", strSubBnsCode);
                result.AddParameter(":3", strSubBnsDesc);
                result.AddParameter(":4", ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }

                result.Query = string.Format("delete from qtr_fee_config where fees_code = '{0}' and det_buss_code = '{1}' and rev_year = '{2}'", ScheduleFrm.FeesCode, strSubBnsCode, ScheduleFrm.RevYear);
                if (result.ExecuteNonQuery() == 0)
                {
                }
                
				if (AuditTrail.InsertTrail("AUTF-D", "mutiple table", StringUtilities.HandleApostrophe(strObj)) == 0)
                {
                    result.Rollback();
                    result.Close();
                    MessageBox.Show(result.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
				
            }

        }

        public override void Close()
        {
            if (ScheduleFrm.ButtonClose.Text == "&Cancel")
            {
                this.DeleteTmpTable(1);
                this.InitializeButtons();
                this.EnableControls(false);
                ScheduleFrm.CheckGross.Checked = false;
                this.InitialFormLoad();
                
                this.LoadBussType();    //datagrid1
            }
            else
            {
                this.DeleteTmpTable(0);

                OracleResultSet result = new OracleResultSet();

                result.Query = string.Format("delete from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code like '{1}%%'", ScheduleFrm.FeesCode.Trim(), ScheduleFrm.BnsCode.Trim());
                if (result.ExecuteNonQuery() == 0)
                {
                }

                ScheduleFrm.Close();
            }

        }

        public override void ListOneComboEdit(string strSelected)
        {
            OracleResultSet result = new OracleResultSet();
            int intTempRow, intTempCol;
            
            string strTemp = string.Empty;
            
            //MCR 20141111 (s)
            //intTempRow = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;
            //intTempCol = ScheduleFrm.ListOne.SelectedCells[0].ColumnIndex;
            intTempRow = m_iCurrRow;
            intTempCol = m_iCurrCol;
            //MCR 20141111 (e)

            if (ScheduleFrm.CheckGross.Checked == false)
            {
                try
                {
                    if (intTempCol == 2)    // RMC 20161216 corrected data grid view error in schedule
                    {
                        if (strSelected == "" && ScheduleFrm.ListOne[2, intTempRow].Value != null)
                            strSelected = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();
                        else  
                            ScheduleFrm.ListOne[2, intTempRow].Value = strSelected;

                        this.m_strMeans = strSelected;

                        if (strSelected != "")
                        {
                            this.LoadFixSchedHeader(strSelected);
                        }
                    }
                }
                catch { }
            }

            this.CheckTmpTable(intTempRow);

            try
            {
                if (ScheduleFrm.ListOne[1, intTempRow].Value != null)   // RMC 20161216 corrected data grid view error in schedule
                    strTemp = ScheduleFrm.ListOne[1, intTempRow].Value.ToString().ToUpper();
                else
                    strTemp = "";
                ScheduleFrm.ListOne[1, intTempRow].Value = strTemp;

                //validate duplicate entry
                if (intTempCol == 1)
                {
                    string strTempRowValue = string.Empty;

                    for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                    {
                        if (intTempRow != i)
                        {
                            if (ScheduleFrm.ListOne[1, i].Value != null)
                            {
                                strTempRowValue = ScheduleFrm.ListOne[1, i].Value.ToString();

                                if (strTemp == strTempRowValue)
                                {
                                    MessageBox.Show("Duplicate business category.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    ScheduleFrm.ListOne[1, intTempRow].Value = "";
                                    return;
                                }
                            }
                        }
                    }
                }

                if (intTempCol == 2)
                {
                    if (ScheduleFrm.ListOne[2, intTempRow].Value != null)
                    {
                        if (ScheduleFrm.ListOne[2, intTempRow].Value.ToString() == "F")
                            ScheduleFrm.ButtonConfig.Enabled = false;
                        else
                        {
                            if ((this.ScheduleFrm.ButtonAdd.Text == "&Save" || this.ScheduleFrm.ButtonEdit.Text == "&Update")
                                && ScheduleFrm.CheckGross.Checked == false)
                                ScheduleFrm.ButtonConfig.Enabled = true;
                        }
                    }
                }
                if (intTempCol == 3)
                {
                    double d = 0;
                    if(ScheduleFrm.ListOne[3, intTempRow].Value != null)    // RMC 20161216 corrected data grid view error in schedule
                        d = Convert.ToDouble(ScheduleFrm.ListOne[3, intTempRow].Value.ToString());
                    ScheduleFrm.ListOne[3, intTempRow].Value = string.Format("{0:#,##0.00}", d);
                }
                // RMC 20161216 corrected data grid view error in schedule (s)
                if (ScheduleFrm.ListOne[0, intTempRow].Value == null)
                    ScheduleFrm.ListOne[0, intTempRow].Value = "";

                if (ScheduleFrm.ListOne[0, intTempRow].Value.ToString() == "")  // RMC 20161216 corrected data grid view error in schedule (e)
                //if (ScheduleFrm.ListOne[0, intTempRow].Value == "" || ScheduleFrm.ListOne[0, intTempRow].Value == null)
                {
                    frmBnsTypeCode.BnsCode = GenerateSubCode(intTempRow);
                    frmBnsTypeCode.Switch = 1;
                    int iCtr = 0;
                    for (int i = 0; i <= ScheduleFrm.ListOne.Rows.Count - 1; i++)
                    {
                        //if (ScheduleFrm.ListOne[0, i].Value != "" && ScheduleFrm.ListOne[0, i].Value != null)
                        if (ScheduleFrm.ListOne[0, i].Value.ToString() != "" && ScheduleFrm.ListOne[0, i].Value != null)    // RMC 20161216 corrected data grid view error in schedule
                        {
                            frmBnsTypeCode.ArraySubCat[i + 1] = ScheduleFrm.ListOne[0, i].Value.ToString().Trim();
                            iCtr++;
                        }

                    }
                    frmBnsTypeCode.Row = iCtr;
                    frmBnsTypeCode.ShowDialog();

                    ScheduleFrm.ListOne[0, intTempRow].Value = frmBnsTypeCode.BnsCode;
                }

                ScheduleFrm.SelectedMeans = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();
                ScheduleFrm.SelectedBnsSubCat = ScheduleFrm.ListOne[1, intTempRow].Value.ToString();
                ScheduleFrm.SelectedBnsSubCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();

                
                
            }
            catch
            {
               ScheduleFrm.SelectedMeans = "";
               ScheduleFrm.SelectedBnsSubCode = "";
               ScheduleFrm.SelectedBnsSubCat = "";
            }

            if (ScheduleFrm.SelectedMeans == "F" || ScheduleFrm.SelectedMeans == "Q" || ScheduleFrm.SelectedMeans == "A")
            {
                string strValue = string.Empty;

                try
                {
                    strValue = ScheduleFrm.ListOne[3, intTempRow].Value.ToString();
                }
                catch
                {
                    strValue = "0.00";
                    ScheduleFrm.ListOne[3, intTempRow].Value = strValue;
                }

                //validate float entry
                try
                {
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
                }
                catch
                {
                    MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ScheduleFrm.ListOne[3, intTempRow].Value = "0.00";
                    return;
                }

                if (ScheduleFrm.ButtonAdd.Text == "&Save" || ScheduleFrm.ButtonEdit.Text == "&Update")
                {
                    this.DeleteTmpTable(1);

                    strValue = ScheduleFrm.ListOne[3, intTempRow].Value.ToString();
                    strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));

                    if (ScheduleFrm.ListOne[2, intTempRow].Value.ToString() == "F")
                    {
                        ValidateQtrFeeConfig(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode, "1", "1", "1", "1", "1", "1", strValue, ScheduleFrm.ListOne[2, intTempRow].Value.ToString(), 1);

                        result.Query = "insert into tmp_fees_sched (FEES_CODE, BNS_CODE, QTY1, QTY2, AREA1, AREA2, "
                            + "GR_1, GR_2, EX_RATE, PLUS_RATE, AMOUNT, EXCESS_NO, EXCESS_AMT, DATA_TYPE, BNS_DESC, REV_YEAR) "
                            + "values (:1,:2,'0','0','0.00','0.00','0','0','0','0',:3,'0.00','0.00',:4,:5,:6) ";
                        result.AddParameter(":1", ScheduleFrm.FeesCode);
                        result.AddParameter(":2", ScheduleFrm.SelectedBnsSubCode);
                        result.AddParameter(":3", strValue);
                        result.AddParameter(":4", ScheduleFrm.ListOne[2, intTempRow].Value.ToString());
                        result.AddParameter(":5", ScheduleFrm.ListOne[1, intTempRow].Value.ToString());
                        result.AddParameter(":6", ScheduleFrm.RevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    else if (ScheduleFrm.ListOne[2, intTempRow].Value.ToString() == "Q")
                    {
                        ValidateQtrFeeConfig(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode, "1", "1", "1", "1", "1", "1", strValue, ScheduleFrm.ListOne[2, intTempRow].Value.ToString(), 1);

					    result.Query = "insert into tmp_fees_sched (FEES_CODE, BNS_CODE, QTY1, QTY2, AREA1, AREA2, "
                            + "GR_1, GR_2, EX_RATE, PLUS_RATE, AMOUNT, EXCESS_NO, EXCESS_AMT, DATA_TYPE, BNS_DESC, REV_YEAR) "
                            + "values (:1,:2,'1','1','0.00','0.00','0','0','0','0',:3,'0.00','0.00',:4,:5,:6) ";
                        result.AddParameter(":1", ScheduleFrm.FeesCode);
                        result.AddParameter(":2", ScheduleFrm.SelectedBnsSubCode);
                        result.AddParameter(":3", strValue);
                        result.AddParameter(":4", ScheduleFrm.ListOne[2, intTempRow].Value.ToString());
                        result.AddParameter(":5", ScheduleFrm.ListOne[1, intTempRow].Value.ToString());
                        result.AddParameter(":6", ScheduleFrm.RevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
						
					}
                    else if (ScheduleFrm.ListOne[2, intTempRow].Value.ToString() == "A")
					{
                        ValidateQtrFeeConfig(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode, "1", "1", "1", "1", "1", "1", strValue, ScheduleFrm.ListOne[2, intTempRow].Value.ToString(), 1);

                        result.Query = "insert into tmp_fees_sched (FEES_CODE, BNS_CODE, QTY1, QTY2, AREA1, AREA2, "
                            + "GR_1, GR_2, EX_RATE, PLUS_RATE, AMOUNT, EXCESS_NO, EXCESS_AMT, DATA_TYPE, BNS_DESC, REV_YEAR) "
                            + "values (:1,:2,'0','0','1.00','1.00','0','0','0','0',:3,'0.00','0.00',:4,:5,:6) ";
                        result.AddParameter(":1", ScheduleFrm.FeesCode);
                        result.AddParameter(":2", ScheduleFrm.SelectedBnsSubCode);
                        result.AddParameter(":3", strValue);
                        result.AddParameter(":4", ScheduleFrm.ListOne[2, intTempRow].Value.ToString());
                        result.AddParameter(":5", ScheduleFrm.ListOne[1, intTempRow].Value.ToString());
                        result.AddParameter(":6", ScheduleFrm.RevYear);
                        if (result.ExecuteNonQuery() == 0)
                        {
                        }
					}
                }
            }

            if (intTempRow == ScheduleFrm.ListOne.Rows.Count - 1)
            {
                try
                {
                    if (ScheduleFrm.ListOne[1, intTempRow].Value.ToString() != "" && strSelected != "")
                        this.GenerateSubCode(intTempRow + 1);
                }
                catch { }
            }

            this.AddListOneRow();
         //   this.AddListTwoRow();

        }

        public override void ListTwoEndEdit(int intCol, int intRow, string strGridPrevValue)
        {
            OracleResultSet result = new OracleResultSet();

            if (strGridPrevValue == "")
                strGridPrevValue = "0";

            string strGridCurrVal = string.Empty;
            strGridPrevValue = string.Format("{0:##0.00}", Convert.ToDouble(strGridPrevValue));

            string strValue = string.Empty;
            double dblSeriesGross;
            string strSeries = string.Empty;
            bool blnaddFlag = false;

            try
            {
                strValue = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                strValue = string.Format("{0:##0.00}", Convert.ToDouble(strValue));
            }
            catch
            {
                MessageBox.Show("Error in Field", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                return;
            }

            string strTempVal = string.Empty;

            try
            {
                if (intCol == 4)
                {
                    for (int i = 0; i <= ScheduleFrm.ListTwo.Rows.Count - 1; i++)
                    {
                        if (intRow != i)
                        {
                            if (ScheduleFrm.ListTwo[intCol, i].Value != null)
                            {
                                strTempVal = ScheduleFrm.ListTwo[intCol, i].Value.ToString();
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                if (strValue == strTempVal)
                                {
                                    if (MessageBox.Show("Duplicate amount detected...\nContinue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                                        return;
                                    }
                                    else
                                        break;
                                }
                            }
                        }

                        
                    }
                }

                double d;
                d = Convert.ToDouble(ScheduleFrm.ListTwo[intCol, intRow].Value.ToString());
                if (ScheduleFrm.SelectedMeans == "QR" && (intCol == 0 || intCol == 1))
                    strGridCurrVal = string.Format("{0:##0}", d);
                else
                {
                    if (intCol == 4)
                        strGridCurrVal = string.Format("{0:#,##0.00}", d);
                    else
                        strGridCurrVal = string.Format("{0:##0.00}", d);
                }
                
                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridCurrVal;

                if (intCol == 0)
                {
                    if (ScheduleFrm.ListTwo[1, intRow].Value != null)
                    {
                        strTempVal = ScheduleFrm.ListTwo[1, intRow].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) > Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00" || strGridPrevValue == "0")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                    }
                    if (intRow != 0)
                    {
                        strTempVal = ScheduleFrm.ListTwo[0, intRow - 1].Value.ToString().Trim();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (Convert.ToDouble(strGridCurrVal) <= Convert.ToDouble(strTempVal) && strTempVal != "")
                        {
                            if (strGridPrevValue == "0.00")
                                ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                            else
                                ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                        }
                        else
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (ScheduleFrm.SelectedMeans == "QR")
                            {
                                dblSeriesGross = Convert.ToDouble(strTempVal) - 1;
                                strSeries = string.Format("{0:##0}", dblSeriesGross);
                            }
                            else
                            {
                                dblSeriesGross = Convert.ToDouble(strTempVal) - .01;
                                strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                            }
                            ScheduleFrm.ListTwo[1, intRow - 1].Value = strSeries;
                        }
                    }

                }
                else if (intCol == 1)
                {
                    bool blnIncr = false;
                    int intCtr = ScheduleFrm.ListTwo.Rows.Count - 1;

                    strTempVal = ScheduleFrm.ListTwo[0, intRow].Value.ToString().Trim();
                    strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                    if (Convert.ToDouble(strGridCurrVal) < Convert.ToDouble(strTempVal) && strTempVal != "" && Convert.ToDouble(strGridCurrVal) != 0)
                    {
                        if (strGridPrevValue == "0.00")
                            ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                        else
                            ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                    }

                    if (intCtr > intRow)
                    {
                        if (ScheduleFrm.ListTwo[intCol, intRow + 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[intCol, intRow + 1].Value.ToString().Trim();
                            strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                            if (Convert.ToDouble(strGridCurrVal) >= Convert.ToDouble(strTempVal) && strTempVal != "")
                            {
                                if (strGridPrevValue == "0.00")
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = "";
                                else
                                    ScheduleFrm.ListTwo[intCol, intRow].Value = strGridPrevValue;
                            }
                        }
                        else
                            blnIncr = true;
                    }

                    if (blnIncr)
                    {
                        strTempVal = ScheduleFrm.ListTwo[intCol, intRow].Value.ToString();
                        strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                        if (ScheduleFrm.SelectedMeans == "QR")
                        {
                            dblSeriesGross = Convert.ToDouble(strTempVal) + 1;
                            strSeries = string.Format("{0:##0}", dblSeriesGross);
                        }
                        else
                        {
                            dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                            strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                        }
                        ScheduleFrm.ListTwo[0, intRow + 1].Value = strSeries;
                    }
                }

                for (int i = 0; i <= ScheduleFrm.ListTwo.Columns.Count - 1; ++i)
                {
                    if (i != 2 && i != 3)
                    {
                        if (ScheduleFrm.ListTwo[i, intRow].Value == null)
                        {
                            blnaddFlag = true;
                            break;

                        }
                    }
                }

                if (!blnaddFlag)
                {
                    this.DeleteTmpTable(1);

                    for (int i = 0; i <= ScheduleFrm.ListTwo.Rows.Count - 1 ; i++)
                    {
                        string strEntQ1 = "";
                        string strEntQ2 = "";
                        string strQty1 = "0";
                        string strQty2 = "0";
                        string strArea1 = "0.00";
                        string strArea2 = "0.00";
                        string strGR1 = "0";
                        string strGR2 = "0";
                        string strExRate = "0.000000";
                        string strPlusRate = "0.000000";
                        string strAmt = "0.00";
                        string strExcess = ScheduleFrm.Excess;
                        string strAddExcess = ScheduleFrm.AddExcess;

                        try
                        {
                            strEntQ1 = ScheduleFrm.ListTwo[0, i].Value.ToString();
                            strEntQ2 = ScheduleFrm.ListTwo[1, i].Value.ToString();
                            strAmt = ScheduleFrm.ListTwo[4, i].Value.ToString();
                        }
                        catch
                        {
                            strEntQ1 = "0";
                            strEntQ2 = "0";
                            strAmt = "0";
                        }

                        try
                        {
                            strExRate = ScheduleFrm.ListTwo[2, i].Value.ToString();
                            strPlusRate = ScheduleFrm.ListTwo[3, i].Value.ToString();
                        }
                        catch
                        {
                            strExRate = "0";
                            strPlusRate = "0";
                        }

                        if(strExcess == "")
                            strExcess = "0";
                        if(strAddExcess == "")
                            strAddExcess = "0";

                        
                        strExRate = string.Format("{0:##0.000000}", Convert.ToDouble(strExRate));
                        strPlusRate = string.Format("{0:##0.000000}", Convert.ToDouble(strPlusRate));
                        strAmt = string.Format("{0:##0.00}", Convert.ToDouble(strAmt));
                        strExcess = string.Format("{0:##0.00}", Convert.ToDouble(strExcess));
                        strAddExcess = string.Format("{0:##0.00}", Convert.ToDouble(strAddExcess));

                        if (strEntQ1 != "" && strEntQ2 != "" && strAmt != "" &&
                            ScheduleFrm.SelectedBnsSubCode != "" && ScheduleFrm.SelectedBnsSubCat != "")
                        {
                            bool blnInsert = true;
                            

                            if (ScheduleFrm.SelectedMeans == "AR")
                            {
                                strEntQ1 = string.Format("{0:##0.00}", Convert.ToDouble(strEntQ1));
                                strEntQ2 = string.Format("{0:##0.00}", Convert.ToDouble(strEntQ2));
                                strArea1 = strEntQ1;
                                strArea2 = strEntQ2;
                                

                                if (strEntQ1 == "0.00" && strEntQ2 == "0.00" && strArea1 == "0.00" && strArea2 == "0.00")
                                    blnInsert = false;
                            }
                            else if (ScheduleFrm.SelectedMeans == "QR")
                            {
                                strEntQ1 = string.Format("{0:##0}", Convert.ToDouble(strEntQ1));
                                strEntQ2 = string.Format("{0:##0}", Convert.ToDouble(strEntQ2));
                                strQty1 = strEntQ1;
                                strQty2 = strEntQ2;
                                

                                if (strEntQ1 == "0" && strEntQ2 == "0" && strQty1 == "0" && strQty2 == "0")
                                    blnInsert = false;
                            }
                            else if (ScheduleFrm.SelectedMeans == "RR")
                            {
                                strEntQ1 = string.Format("{0:##0.00}", Convert.ToDouble(strEntQ1));
                                strEntQ2 = string.Format("{0:##0.00}", Convert.ToDouble(strEntQ2));
                                strGR1 = strEntQ1;
                                strGR2 = strEntQ2;
                                

                                if (strEntQ1 == "0.00" && strEntQ2 == "0.00" && strGR1 == "0.00" && strGR2 == "0.00")
                                    blnInsert = false;

                            }

                            if (blnInsert)
                            {
                                ValidateQtrFeeConfig(ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode, strQty1, strQty2, strArea1, strArea2, strGR1, strGR2, strAmt, ScheduleFrm.SelectedMeans, i+1);

                                result.Query = "insert into tmp_fees_sched (FEES_CODE, BNS_CODE, QTY1, QTY2, AREA1, AREA2, "
                                    + "GR_1, GR_2, EX_RATE, PLUS_RATE, AMOUNT, EXCESS_NO, EXCESS_AMT, DATA_TYPE, BNS_DESC, REV_YEAR) "
                                    + "values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11,:12,:13,:14,:15,:16) ";
                                result.AddParameter(":1", ScheduleFrm.FeesCode);
                                result.AddParameter(":2", ScheduleFrm.SelectedBnsSubCode);
                                result.AddParameter(":3", strQty1);
                                result.AddParameter(":4", strQty2);
                                result.AddParameter(":5", strArea1);
                                result.AddParameter(":6", strArea2);
                                result.AddParameter(":7", strGR1);
                                result.AddParameter(":8", strGR2);
                                result.AddParameter(":9", strExRate);
                                result.AddParameter(":10", strPlusRate);
                                result.AddParameter(":11", strAmt);
                                result.AddParameter(":12", strExcess);
                                result.AddParameter(":13", strAddExcess);
                                result.AddParameter(":14", ScheduleFrm.SelectedMeans);
                                result.AddParameter(":15", StringUtilities.HandleApostrophe(ScheduleFrm.SelectedBnsSubCat));
                                result.AddParameter(":16", ScheduleFrm.RevYear);
                                if (result.ExecuteNonQuery() == 0)
                                {
                                }
                            }
                        }

                    }

                    if (intRow == ScheduleFrm.ListTwo.Rows.Count - 1)
                    {
                        // Enable Series
                        int intRowCount = ScheduleFrm.ListTwo.Rows.Count;
                        if (ScheduleFrm.ListTwo[1, intRowCount - 1].Value != null)
                        {
                            strTempVal = ScheduleFrm.ListTwo[1, intRowCount - 1].Value.ToString();
                            if (strTempVal != "")
                            {
                                strTempVal = string.Format("{0:##0.00}", Convert.ToDouble(strTempVal));

                                if (ScheduleFrm.SelectedMeans == "QR")
                                {
                                    dblSeriesGross = Convert.ToDouble(strTempVal) + 1;
                                    strSeries = string.Format("{0:##0}", dblSeriesGross);
                                }
                                else
                                {
                                    dblSeriesGross = Convert.ToDouble(strTempVal) + .01;
                                    strSeries = string.Format("{0:##0.00}", dblSeriesGross);
                                }
                                ScheduleFrm.ListTwo.Rows.Add("");
                                ScheduleFrm.ListTwo[0, ScheduleFrm.ListTwo.Rows.Count - 1].Value = strSeries;
                            }
                        }
                    }

                }

                this.AddListTwoRow();

            }
            catch (OracleException ex) // catches only Oracle errors
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12545:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex) // catches any error
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteTmpTable(int iStatus)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            if (iStatus == 0)
            {
                pCmd.Query = "delete from tmp_fees_sched";
            }
            else
            {
                pCmd.Query = string.Format("delete from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}' ", ScheduleFrm.FeesCode, ScheduleFrm.SelectedBnsSubCode);
            }

            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            
	
        }

        public override void CheckTmpTable(int intRow)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();

            string strRowBnsCode = string.Empty;
            string strRowBnsDesc = string.Empty;
            string strRowDataType = string.Empty;

            string strBnsDesc = string.Empty;
            string strDataType = string.Empty;
            

            try
            {
                strRowBnsCode = ScheduleFrm.ListOne[0,intRow].Value.ToString().Trim();
                strRowBnsDesc = ScheduleFrm.ListOne[1,intRow].Value.ToString().Trim().ToUpper();
                strRowDataType = ScheduleFrm.ListOne[2,intRow].Value.ToString().Trim();
            }
            catch
            {
                strRowBnsCode = "";
                strRowBnsDesc = "";
                strRowDataType = "";
            }

            result.Query = string.Format("select * from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}'", ScheduleFrm.FeesCode, strRowBnsCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    strBnsDesc = result.GetString("bns_desc").Trim();
                    strDataType = result.GetString("data_type").Trim();

                    if (strDataType != strRowDataType && strRowDataType != "")
                    {
                        if (MessageBox.Show("All record(s) from the selected item will be deleted. Continue anyway?", "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            this.DeleteTmpTable(1);
                        else
                        {
                            ScheduleFrm.ListOne[2, intRow].Value = strDataType;
                        }
                            
                    }

                    if (strBnsDesc != strRowBnsDesc)
                    {
                        result2.Query = string.Format("update tmp_fees_sched set bns_desc = '{0}' where fees_code = '{1}' and bns_code = '{2}'", strRowBnsDesc,ScheduleFrm.FeesCode, StringUtilities.HandleApostrophe(strRowBnsCode));
                        if (result2.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                    
                }
            }
            result.Close();
        }

        public override void ButtonConfig()
        {
            int intTempRow;
            string strTemp = string.Empty;

            intTempRow = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;

            if (ScheduleFrm.ListOne[2, intTempRow].Value.ToString().Trim() == "RR")
            {
                using (frmRangeConfig frmRangeConfig = new frmRangeConfig())
                {
                    if (ScheduleFrm.ListOne[1, intTempRow].Value != null)
                    {
                        frmRangeConfig.RevYear = ScheduleFrm.RevYear;
                        frmRangeConfig.FeesCode = ScheduleFrm.FeesCode;
                        frmRangeConfig.BnsCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();
                        frmRangeConfig.CodeType = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();
                        frmRangeConfig.FeesDesc = ScheduleFrm.ListOne[1, intTempRow].Value.ToString();
                        frmRangeConfig.ShowDialog();
                    }
                }
            }	
            else
            {
                using (frmDefaultCodes frmDefaultCodes = new frmDefaultCodes())
                {
                    if (ScheduleFrm.ListOne[1, intTempRow].Value != null)
                    {
                        frmDefaultCodes.FeesCode = ScheduleFrm.FeesCode;
                        frmDefaultCodes.BnsCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();
                        frmDefaultCodes.FeesDesc = ScheduleFrm.ListOne[1, intTempRow].Value.ToString();
                        frmDefaultCodes.CodeType = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();
                        frmDefaultCodes.RevYear = ScheduleFrm.RevYear;
                        frmDefaultCodes.Switch = 0;
                        frmDefaultCodes.ShowDialog();
                    }
                }
            }

            
        }

        private void LoadSurchIntDisc()
        {
            OracleResultSet result = new OracleResultSet();

            string strFeesCode = "F" + ScheduleFrm.FeesCode;

            ScheduleFrm.Surcharge = "";
            ScheduleFrm.Interest = "";
            ScheduleFrm.Discount = "";

            result.Query = string.Format("select * from surch_sched where rev_year = '{0}' and tax_fees_code = '{1}'", ScheduleFrm.RevYear, strFeesCode);
            if (result.Execute())
            {
                double dsurchrate = 0;
                double dpenrate = 0;

                if (result.Read())
                {
                    dsurchrate = result.GetDouble("surch_rate");
                    dpenrate = result.GetDouble("pen_rate");

                    ScheduleFrm.Surcharge = string.Format("{0:##0.00000}", dsurchrate);
                    ScheduleFrm.Interest = string.Format("{0:##0.00000}", dpenrate);
                }
            }
            result.Close();

        }

        public override void DeleteSubBusiness(int intRow)
        {
            string strSubBnsCode = string.Empty;
            string strSubBnsDesc = string.Empty;

            strSubBnsCode = ScheduleFrm.ListOne[0, intRow].Value.ToString().Trim();
            strSubBnsDesc = ScheduleFrm.ListOne[1, intRow].Value.ToString().Trim();

            if (!this.CheckSched(strSubBnsCode, ScheduleFrm.FeesCode))
            {
                if (MessageBox.Show("Delete "+ strSubBnsDesc + " and its schedules?" , "Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DeleteTables(false, strSubBnsCode, strSubBnsDesc);
                    ScheduleFrm.ListOne.Rows.RemoveAt(ScheduleFrm.ListOne.SelectedCells[0].RowIndex);
                    //this.InitialFormLoad();
                    //this.LoadFees(0);   //combo fees
                    //this.LoadBussType();    //datagrid1
                    //this.LoadSurchIntDisc();
                }
            }
            else
            {
                MessageBox.Show("You cannot delete this schedule. Bns code already been used.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }            
        
        }

        private void SaveSurchIntDisc()
        {
            //OracleResultSet result = new OracleResultSet();       // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            string strFeesCode = "F" + ScheduleFrm.FeesCode;

            pCmd.Query = string.Format("delete from surch_sched where rev_year = '{0}' and tax_fees_code = '{1}'", ScheduleFrm.RevYear, strFeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            if (ScheduleFrm.Surcharge == "")
                ScheduleFrm.Surcharge = "0";

            if (ScheduleFrm.Interest == "")
                ScheduleFrm.Interest = "0";

            if (ScheduleFrm.Discount == "")
                ScheduleFrm.Discount = "0";

            pCmd.Query = "insert into surch_sched (surch_rate, pen_rate, rev_year, tax_fees_code) values (:1,:2,:3,:4) ";
            pCmd.AddParameter(":1", string.Format("{0:##0.00000}", ScheduleFrm.Surcharge));
            pCmd.AddParameter(":2", string.Format("{0:##0.00000}", ScheduleFrm.Interest));
            pCmd.AddParameter(":3", ScheduleFrm.RevYear);
            pCmd.AddParameter(":4", strFeesCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            
        }

        private void SaveMinMaxTax(string strFeesCode, string strBnsCode)
        {
            //OracleResultSet result = new OracleResultSet();   // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd
            /*double dblMinTax = 0;
            double dblMaxTax = 0;

            if(ScheduleFrm.MinTax.ToString().Trim() == "")
                ScheduleFrm.MinTax = "0";

            if(ScheduleFrm.MaxTax.ToString().Trim() == "")
                ScheduleFrm.MaxTax = "0";

            pCmd.Query = string.Format("delete from minmax_tax_table where trim(fees_code) = '{0}' and bns_code = '{1}'", strFeesCode, strBnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            dblMinTax = Convert.ToDouble(ScheduleFrm.MinTax);
            dblMaxTax = Convert.ToDouble(ScheduleFrm.MaxTax);

            ScheduleFrm.MinTax = string.Format("{0:##.00}", dblMinTax);
            ScheduleFrm.MaxTax = string.Format("{0:##.00}", dblMaxTax);

            pCmd.Query = "insert into minmax_tax_table (FEES_CODE, BNS_CODE, MIN_TAX, MAX_TAX, REV_YEAR, DATA_TYPE) values (:1,:2,:3,:4,:5,:6)";
            pCmd.AddParameter(":1", strFeesCode);
            pCmd.AddParameter(":2", strBnsCode);
            pCmd.AddParameter(":3", ScheduleFrm.MinTax);
            pCmd.AddParameter(":4", ScheduleFrm.MaxTax);
            pCmd.AddParameter(":5", ScheduleFrm.RevYear);
            pCmd.AddParameter(":6", "F");
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }*/
            // RMC 20150305 corrections in schedules module, modified

            // RMC 20150305 corrections in schedules module (s)
            pCmd.Query = string.Format("delete from minmax_tax_table where trim(fees_code) = '{0}' and rev_year = '{1}' and bns_code like '{2}%'", strFeesCode, ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = "insert into minmax_tax_table ";
            pCmd.Query += string.Format("select * from tmp_minmax_tax_table where fees_code = '{0}' and rev_year = '{1}' and bns_code like '{2}%'", strFeesCode, ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }

            pCmd.Query = string.Format("delete from tmp_minmax_tax_table where trim(fees_code) = '{0}' and rev_year = '{1}' and bns_code like '{2}%'", strFeesCode, ScheduleFrm.RevYear, ScheduleFrm.BnsCode);
            if (pCmd.ExecuteNonQuery() == 0)
            {
            }
            // RMC 20150305 corrections in schedules module (e)
        }

        public override void SetFeesTerm(string sFeesTerm)
        {
            bool blnMonth = false;
            bool blnQtr = false;
            bool blnYear = false;

            if (sFeesTerm == "M")
                blnMonth = true;
            else if (sFeesTerm == "Q")
                blnQtr = true;
            else
                blnYear = true;

            ScheduleFrm.CheckFeesMonth.Checked = blnMonth;
            ScheduleFrm.CheckFeesQtr.Checked = blnQtr;
            ScheduleFrm.CheckFeesYear.Checked = blnYear;
        }

        private string GetFeesTerm()
        {
            string strFeesTerm = string.Empty;

            if (ScheduleFrm.CheckFeesMonth.Checked == true)
                strFeesTerm = "M";
            else if (ScheduleFrm.CheckFeesQtr.Checked == true)
                strFeesTerm = "Q";
            else
                strFeesTerm = "F";

            return strFeesTerm;
        }

        public override void ButtonQuarterConfig()
        {
            OracleResultSet result = new OracleResultSet();

            int intTempRow;
            bool blnWData = false;
            
            intTempRow = ScheduleFrm.ListOne.SelectedCells[0].RowIndex;

            if (ScheduleFrm.ListOne[0, intTempRow].Value != null &&
                ScheduleFrm.ListOne[2, intTempRow].Value != null)
            {
                
                using (frmQuarterConfig frmQuarterConfig = new frmQuarterConfig())
                {
                    result.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}' and data_type = '{2}'", ScheduleFrm.FeesCode.Trim(), ScheduleFrm.ListOne[0, intTempRow].Value.ToString(), ScheduleFrm.ListOne[2, intTempRow].Value.ToString());
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            blnWData = true;
                            frmQuarterConfig.blnWInitialData = true;
                        }
                        else
                        {
                            result.Close();
                            result.Query = "select * from tmp_fees_sched where fees_code = :1 and bns_code = :2 and rev_year = :3 and data_type = :4 ";
                            result.AddParameter(":1", ScheduleFrm.FeesCode);
                            result.AddParameter(":2", ScheduleFrm.ListOne[0, intTempRow].Value.ToString());
                            result.AddParameter(":3", ScheduleFrm.RevYear);
                            result.AddParameter(":4", ScheduleFrm.ListOne[2, intTempRow].Value.ToString());
                            if (result.Execute())
                            {

                                if (result.Read())
                                {
                                    blnWData = true;
                                    frmQuarterConfig.blnWInitialData = false;
                                }
                            }
                        }

                        frmQuarterConfig.RevYear = ScheduleFrm.RevYear;
                        frmQuarterConfig.FeesCode = ScheduleFrm.FeesCode;
                        frmQuarterConfig.BnsCode = ScheduleFrm.ListOne[0, intTempRow].Value.ToString();
                        frmQuarterConfig.DataType = ScheduleFrm.ListOne[2, intTempRow].Value.ToString();

                        if(blnWData)
                            frmQuarterConfig.ShowDialog();
                        else
                        {
                            MessageBox.Show("No initial values.", "Configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
                
            }
        }

        private void SaveQtrConfig()
        {
            OracleResultSet result = new OracleResultSet();
            //OracleResultSet result2 = new OracleResultSet();  // RMC 20111012 modified query commit in scheds

            // RMC 20111012 modified query commit in scheds, changed result to pCmd

            string strRowBnsCode = string.Empty;
            string strRowDataType = string.Empty;
            string strTmpDataType = string.Empty;

            for (int intRow = 0; intRow < ScheduleFrm.ListOne.Rows.Count; intRow++)
            {
                try
                {
                    strRowBnsCode = ScheduleFrm.ListOne[0, intRow].Value.ToString().Trim();
                    strRowDataType = ScheduleFrm.ListOne[2, intRow].Value.ToString().Trim();

                    result.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}' ", ScheduleFrm.FeesCode.Trim(), strRowBnsCode);
                    if (result.Execute())
                    {
                        if (result.Read())
                        {
                            pCmd.Query = string.Format("delete from qtr_fee_config where fees_code = '{0}' and det_buss_code = '{1}'", ScheduleFrm.FeesCode.Trim(), strRowBnsCode);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }

                            strTmpDataType = result.GetString("data_type").Trim();

                            if (strRowDataType == strTmpDataType)
                            {
                                pCmd.Query = "insert into qtr_fee_config ";
                                pCmd.Query += string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}' and data_type = '{2}'", ScheduleFrm.FeesCode.Trim(), strRowBnsCode, strRowDataType);
                                if (pCmd.ExecuteNonQuery() == 0)
                                {
                                }
                            }

                            pCmd.Query = string.Format("delete from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}'", ScheduleFrm.FeesCode.Trim(), strRowBnsCode);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }

                        }
                        else
                        {
                            // RMC 20110824 (s)
                            pCmd.Query = string.Format("delete from qtr_fee_config where fees_code = '{0}' and det_buss_code = '{1}'", ScheduleFrm.FeesCode.Trim(), strRowBnsCode);
                            if (pCmd.ExecuteNonQuery() == 0)
                            {
                            }
                            // RMC 20110824 (e)
                        }
                    }

                    
                }
                catch
                {
                }
            }
        }

        /*private bool ValidateQtrConfig()
        {
            OracleResultSet result = new OracleResultSet();
        }*/

        private void ValidateQtrFeeConfig(string strFeesCode, string strDetBnsCode, string strQty1, string strQty2, string strArea1, string strArea2, string strGR1, string strGR2, string strAmt, string strDataType, int intRow)
        {
            OracleResultSet result = new OracleResultSet();
            OracleResultSet result2 = new OracleResultSet();
            string strVal1, strVal2;
            string strNewVal1, strNewVal2, strNewAmt;
            double dblTmp = 0;
            
            int intRowCnt = 0;

            result.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}' and data_type = '{2}' order by gr_1", strFeesCode, strDetBnsCode, strDataType);
            if(result.Execute())
            {
                while(result.Read())
                {
                    intRowCnt++;

                    if (strDataType == "QR" || strDataType == "Q")
                    {
                        strVal1 = string.Format("{0:##}", result.GetDouble("gr_1"));    // RMC 20111001 correction in scheds editing, changed from GetInt to GetDouble
                        strVal2 = string.Format("{0:##}", result.GetDouble("gr_2"));    // RMC 20111001 correction in scheds editing, changed from GetInt to GetDouble
                    }
                    else
                    {
                        strVal1 = string.Format("{0:##.00}", result.GetDouble("gr_1"));
                        strVal2 = string.Format("{0:##.00}", result.GetDouble("gr_2"));
                    }

                    // RMC 20111001 correction in scheds editing, changed from GetInt to GetDouble (s)
                    if (strVal1 == "")
                        strVal1 = "0";
                    if (strVal2 == "")
                        strVal2 = "0";
                    // RMC 20111001 correction in scheds editing, changed from GetInt to GetDouble (e)

                    if(intRowCnt == intRow)
                    {
                        result2.Query = "update qtr_fee_config_tmp set gr_1 = :1, gr_2 = :2, amount = :3";
                        result2.Query+= " where gr_1 = :4 and gr_2 = :5";
                        result2.Query+= " and fees_code = :6 and det_buss_code = :7";

                        if (strDataType == "QR" || strDataType == "Q")
                        {
                            result2.AddParameter(":1", strQty1);
                            result2.AddParameter(":2", strQty2);
                            strNewVal1 = strQty1;
                            strNewVal2 = strQty2;
                        }
                        else if(strDataType == "AR" || strDataType == "A")
                        {
                            result2.AddParameter(":1", strArea1);
                            result2.AddParameter(":2", strArea2);
                            strNewVal1 = strArea1;
                            strNewVal2 = strArea2;
                        }
                        else
                        {
                            result2.AddParameter(":1", strGR1);
                            result2.AddParameter(":2", strGR2);
                            strNewVal1 = strGR1;
                            strNewVal2 = strGR2;
                        }

                        result2.AddParameter(":3", strAmt);
                        result2.AddParameter(":4", strVal1);
                        result2.AddParameter(":5", strVal2);
                        result2.AddParameter(":6", strFeesCode);
                        result2.AddParameter(":7", strDetBnsCode);
                        if(result2.ExecuteNonQuery() == 0)
                        {
                            // if record does not exist
                            result2.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}'", strFeesCode, strDetBnsCode);
                            if(result2.Execute())
                            {
                                if(result2.Read())
                                {
                                    result2.Close();

                                    result2.Query = "insert into qtr_fee_config_tmp values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11)";
                                    result2.AddParameter(":1", strFeesCode);
                                    result2.AddParameter(":2", strDetBnsCode);
                                    result2.AddParameter(":3", ScheduleFrm.RevYear);
                                    result2.AddParameter(":4", strDataType);
                                    result2.AddParameter(":5", strNewVal1);
                                    result2.AddParameter(":6", strNewVal2);
                                    result2.AddParameter(":7", strAmt);
                                    result2.AddParameter(":8", "0");
                                    result2.AddParameter(":9", "0");
                                    result2.AddParameter(":10", "0");
                                    result2.AddParameter(":11", "");
                                    if (result2.ExecuteNonQuery() == 0)
                                    {
                                    }
                                }
                            }
                        }

                    }
                    

                }

            }
            result.Close();

            if (intRow > intRowCnt)
            {
                if (strDataType == "QR" || strDataType == "Q")
                {
                    strNewVal1 = strQty1;
                    strNewVal2 = strQty2;
                }
                else if (strDataType == "AR" || strDataType == "A")
                {
                    strNewVal1 = strArea1;
                    strNewVal2 = strArea2;
                }
                else
                {
                    strNewVal1 = strGR1;
                    strNewVal2 = strGR2;
                }

                result2.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}'", strFeesCode, strDetBnsCode);
                if (result2.Execute())
                {
                    if (result2.Read())
                    {
                        result2.Close();
                        result2.Query = "insert into qtr_fee_config_tmp values (:1,:2,:3,:4,:5,:6,:7,:8,:9,:10,:11)";
                        result2.AddParameter(":1", strFeesCode);
                        result2.AddParameter(":2", strDetBnsCode);
                        result2.AddParameter(":3", ScheduleFrm.RevYear);
                        result2.AddParameter(":4", strDataType);
                        result2.AddParameter(":5", strNewVal1);
                        result2.AddParameter(":6", strNewVal2);
                        result2.AddParameter(":7", strAmt);
                        result2.AddParameter(":8", "0");
                        result2.AddParameter(":9", "0");
                        result2.AddParameter(":10", "0");
                        result2.AddParameter(":11", "");
                        if (result2.ExecuteNonQuery() == 0)
                        {
                        }
                    }
                }
            }
        }

        private void LoadQtrFeeConfig()
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("delete from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code like '{1}%%'", ScheduleFrm.FeesCode.Trim(), ScheduleFrm.BnsCode.Trim());
            if (result.ExecuteNonQuery() == 0)
            {
            }

            result.Query = "insert into qtr_fee_config_tmp ";
            result.Query += string.Format(" select * from qtr_fee_config where fees_code = '{0}' and rev_year = '{1}' and det_buss_code like '{2}%%'", ScheduleFrm.FeesCode.Trim(), ScheduleFrm.RevYear.Trim(), ScheduleFrm.BnsCode.Trim());
            if (result.ExecuteNonQuery() == 0)
            {
            }
        }

        private bool ValidateQtrFeeConfig()
        {
            OracleResultSet result = new OracleResultSet();

            //validate qtr config
            result.Query = string.Format("select * from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code like '{1}%%' "
                + " and (amount2 = 0 and amount3 = 0 and amount4 = 0)", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
            if (result.Execute())
            {
                if (result.Read())
                {
                    MessageBox.Show("Incomplete quarter config data.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            result.Close();

            result.Query = string.Format("select count(*) from qtr_fee_config_tmp where fees_code = '{0}' and det_buss_code = '{1}' ", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
            int intCount = 0;
            int.TryParse(result.ExecuteScalar(), out intCount);

            result.Query = string.Format("select count(*) from tmp_fees_sched where fees_code = '{0}' and bns_code = '{1}' ", ScheduleFrm.FeesCode, ScheduleFrm.BnsCode);
            int intFeeCount = 0;
            int.TryParse(result.ExecuteScalar(), out intFeeCount);

            if (intCount != intFeeCount)
            {
                MessageBox.Show("Incomplete quarter config data.", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }



            return true;
        }

        public override void EditMinMax()
        {
            // RMC 20150305 corrections in schedules module

            OracleResultSet result = new OracleResultSet();
            double dMinTax = 0;
            double dMaxTax = 0;

            double.TryParse(ScheduleFrm.MinTax, out dMinTax);
            double.TryParse(ScheduleFrm.MaxTax, out dMaxTax);

            int iCnt = 0;

            if (dMinTax == 0 && dMaxTax == 0)
            {
                result.Query = "delete from tmp_minmax_tax_table where fees_code = '" + ScheduleFrm.FeesCode + "'";
                result.Query += " and bns_code = '" + ScheduleFrm.SelectedBnsSubCode + "' and rev_year = '" + ScheduleFrm.RevYear + "'";
                if (result.ExecuteNonQuery() == 0)
                {
                }
            }
            else
            {
                result.Query = "select count(*) from tmp_minmax_tax_table where fees_code = '" + ScheduleFrm.FeesCode + "'";
                result.Query += " and bns_code = '" + ScheduleFrm.SelectedBnsSubCode + "' and rev_year = '" + ScheduleFrm.RevYear + "'";
                int.TryParse(result.ExecuteScalar(), out iCnt);

                if (iCnt == 0)
                {
                    result.Query = "insert into tmp_minmax_tax_table (FEES_CODE, BNS_CODE, MIN_TAX, MAX_TAX, REV_YEAR, DATA_TYPE) values (:1,:2,:3,:4,:5,:6)";
                    result.AddParameter(":1", ScheduleFrm.FeesCode);
                    result.AddParameter(":2", ScheduleFrm.SelectedBnsSubCode);
                    result.AddParameter(":3", ScheduleFrm.MinTax);
                    result.AddParameter(":4", ScheduleFrm.MaxTax);
                    result.AddParameter(":5", ScheduleFrm.RevYear);
                    result.AddParameter(":6", "F");
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }
                }
                else
                {
                    result.Query = "update tmp_minmax_tax_table set MIN_TAX = " + ScheduleFrm.MinTax + ", ";
                    result.Query += " MAX_TAX = " + ScheduleFrm.MaxTax + " ";
                    result.Query += " where fees_code = '" + ScheduleFrm.FeesCode + "'";
                    result.Query += " and bns_code = '" + ScheduleFrm.SelectedBnsSubCode + "'";
                    result.Query += " and rev_year = '" + ScheduleFrm.RevYear + "'";
                    if (result.ExecuteNonQuery() == 0)
                    {
                    }

                }
            }

        }
    }
}
