using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;

namespace Amellar.Modules.InspectorsDetails
{
    public partial class frmBnsInfo : Form
    {
        public frmBnsInfo()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FillData(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            string sOrNo = string.Empty;
            string sTaxYear = string.Empty;
            string sTerm = string.Empty;
            string sOrDate = string.Empty;

            result.Query = "select distinct * from pay_hist where bin = '" + sBin + "' order by or_date desc, tax_year desc";
            if (result.Execute())
            {
                if (result.Read())
                {
                    sOrNo = result.GetString("or_no");
                    sTaxYear = result.GetString("tax_year");
                    sOrDate = string.Format("{0:dd-MMM-yyyy}",result.GetDateTime("or_date"));
                    sTerm = result.GetString("payment_term");
                    if (sTerm == "F")
                        sTerm = "FULL - " + sTaxYear;
                    else
                        sTerm = "INSTALLMENT - " + sTaxYear;
                }
            }
            result.Close();

            lblTerm.Text = sTerm;
            lblOrDate.Text = sOrDate;

            result.Query = "select sum(fees_amtdue) as amount from or_table where or_no = '" + sOrNo + "'";
            if(result.Execute())
            {
                if (result.Read())
                {
                    lblAmount.Text = "P " + string.Format("{0:#,##0.#0}", result.GetDouble("amount"));
                }
            }
            result.Close();




            lblOrNo.Text = sOrNo;
            lblBIN.Text = sBin;
            lblBnsName.Text = AppSettingsManager.GetBnsName(sBin);
            lblBnsOwner.Text = AppSettingsManager.GetBnsOwner(AppSettingsManager.GetOwnCode(sBin));

            result.Query = "select permit_no from businesses where bin = '" + sBin.Trim() + "'";
            if (result.Execute())
            {
                if (result.Read())
                {
                    lblPermitNo.Text = result.GetString("permit_no");
                }
            }
            result.Close();

            if (lblPermitNo.Text.Trim() == string.Empty)
                lblPermitNo.Text = "NO PERMIT YET";

        }
    }
}