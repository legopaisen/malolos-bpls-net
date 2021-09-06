using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace ReturnDeclareOR
{
    public partial class frmDBCRConverter : Form
    {
        //JARS 20171024 ON SITE: CONVERTER FOR THE TABLE DBCR_MEMO,dbcr_memo. DI KAYA SA IISANG QUERY LANG EH KAYA ETO
        public frmDBCRConverter()
        {
            InitializeComponent();
        }

        OracleResultSet pSet = new OracleResultSet();
        OracleResultSet pSet2 = new OracleResultSet();
        OracleResultSet pSet3 = new OracleResultSet();
        string sOrNo = string.Empty;
        string sBin = string.Empty;
        string sQuery = string.Empty;

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want to transfer Data to dbcr_memo?", "DBCR_MEMO CONVERTER", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
                pSet.Query = "select * from DBCR_MEMO order by own_code asc";

                if (pSet.Execute())
                {
                    while (pSet.Read())
                    {
                        sOrNo = pSet.GetString("or_no");
                        pSet2.Query = "select bin from pay_hist where or_no = '"+ sOrNo +"'";
                        if(pSet2.Execute())
                        {
                            if (pSet2.Read())
                            {
                                sBin = pSet2.GetString("bin");
                            }
                            else
                            {
                                pSet2.Close();
                                pSet2.Query = "select bin from cancelled_payment where or_no = '" + sOrNo + "'";
                                if(pSet2.Execute())
                                {
                                    if(pSet2.Read())
                                    {
                                        sBin = pSet2.GetString("bin");
                                    }
                                }
                            }
                        }
                        pSet2.Close();

                        sQuery = "insert into dbcr_memo_temp values "; //JARS 20171010
                        sQuery += "('" + sBin + "', ";
                        sQuery += "'"+ sOrNo + "', ";
                        sQuery += "to_date('"+ pSet.GetDateTime("or_date").ToString("MM/dd/yyyy") +"', 'MM/dd/YYYY'), ";
                        sQuery += "'"+ pSet.GetDouble("debit") +"', ";
                        sQuery += "'"+ pSet.GetDouble("credit") +"', ";
                        sQuery += "'"+ pSet.GetDouble("balance") +"', ";
                        sQuery += "'" + pSet.GetString("memo")+ "', ";
                        sQuery += "' ',";
                        sQuery += "'"+ pSet.GetString("teller")+ "', ";
                        sQuery += "to_date('" + pSet.GetDateTime("date_created").ToString("MM/dd/yyyy") + "', 'MM/dd/YYYY'), ";
                        sQuery += "'"+ pSet.GetString("time_created")+ "', ";
                        sQuery += "'"+ pSet.GetString("served") + "',";
                        sQuery += "'"+ pSet.GetString("chk_no")+ "',";
                        sQuery += "'"+ pSet.GetString("multi_pay") + "','0')";

                        pSet3.Query = sQuery;
                        pSet3.ExecuteNonQuery();
                    }
                }
                pSet.Close();
                
                pSet.Query = "select * from DBCR_MEMO_HIST order by own_code asc";

                if(pSet.Execute())
                {
                    while(pSet.Read())
                    {
                        sOrNo = pSet.GetString("or_no");
                        pSet2.Query = "select bin from pay_hist where or_no = '" + sOrNo + "'";
                        if (pSet2.Execute())
                        {
                            if (pSet2.Read())
                            {
                                sBin = pSet2.GetString("bin");
                            }
                            else
                            {
                                pSet2.Close();
                                pSet2.Query = "select bin from cancelled_payment where or_no = '" + sOrNo + "'";
                                if (pSet2.Execute())
                                {
                                    if (pSet2.Read())
                                    {
                                        sBin = pSet2.GetString("bin");
                                    }
                                }
                            }
                        }
                        pSet2.Close();

                        sQuery = "insert into dbcr_memo_hist_temp values "; //JARS 20171010
                        sQuery += "('" + sBin + "', ";
                        sQuery += "'" + sOrNo + "', ";
                        sQuery += "to_date('" + pSet.GetDateTime("or_date").ToString("MM/dd/yyyy") + "', 'MM/dd/YYYY'), ";
                        sQuery += "'" + pSet.GetDouble("debit") + "', ";
                        sQuery += "'" + pSet.GetDouble("credit") + "', ";
                        sQuery += "'" + pSet.GetDouble("balance") + "', ";
                        sQuery += "'" + pSet.GetString("memo") + "', ";
                        sQuery += "' ',";
                        sQuery += "'" + pSet.GetString("teller") + "', ";
                        sQuery += "to_date('" + pSet.GetDateTime("date_created").ToString("MM/dd/yyyy") + "', 'MM/dd/YYYY'), ";
                        sQuery += "'" + pSet.GetString("time_created") + "', ";
                        sQuery += "'" + pSet.GetString("served") + "',";
                        sQuery += "'" + pSet.GetString("chk_no") + "',";
                        sQuery += "'" + pSet.GetString("multi_pay") + "','0')";

                        pSet3.Query = sQuery;
                        pSet3.ExecuteNonQuery();

                    }
                }
                pSet.Close();
                MessageBox.Show("TRANSFER COMPLETE");
            }
            else
            {
                return;
            }
        }
    }
}