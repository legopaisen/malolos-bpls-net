//JHMN 20170105 no business certificate

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.Reports;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.BusinessReports
{
    public partial class frmNoBusiness : Form
    {
        public frmNoBusiness()
        {
            InitializeComponent();
        }

        public string sOwnerFN = string.Empty;
        public string sOwnerLN = string.Empty;
        public string sOwnerMN = string.Empty;
        public string sOwnerAddress = string.Empty;
        public string sOwnerCode = string.Empty; 
        public string sPurpose = string.Empty; //JHB 20180511 

        private void frmNoBusiness_Load(object sender, EventArgs e)
        {
        
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            OracleResultSet orcResult = new OracleResultSet();
            OracleResultSet orcResult2 = new OracleResultSet();
            int iNumRows;
            sOwnerLN = txtLastName.Text.Trim();
            sOwnerFN = txtFirstName.Text.Trim();
            sOwnerMN = txtMI.Text.Trim();
            sOwnerAddress = txtAddress.Text.Trim();
            sPurpose = txtPurpose.Text.Trim();

            if (sOwnerLN.Trim() == "" || sOwnerFN.Trim() == "" || sOwnerMN.Trim() == "" || sOwnerAddress.Trim() == "" || txtRequestedBy.Text.Trim() == "" || txtPurpose.Text.Trim() == "") //JHB 20180511
            {
                MessageBox.Show("Complete all the input fields first","Certification",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            orcResult.Query = "SELECT * FROM own_names WHERE own_ln = '" + StringUtilities.HandleApostrophe(sOwnerLN.Trim()) + "' ";
            orcResult.Query += "AND own_fn = '" + StringUtilities.HandleApostrophe(sOwnerFN.Trim()) + "' ";
            orcResult.Query += "and own_mi = '" + StringUtilities.HandleApostrophe(sOwnerMN.Trim()) + "'";  //JHMN 20170105 filter person first name and last name
            if (orcResult.Execute())
            {
                while (orcResult.Read()) //JHMN 20170105 for multiple returned rows
                {
                    sOwnerCode = orcResult.GetString("own_code");

                    orcResult2.Query = "SELECT own_code FROM businesses WHERE own_code = '" + sOwnerCode + "' UNION ALL SELECT own_code FROM business_que WHERE own_code = '" + sOwnerCode + "' UNION ALL SELECT own_code FROM buss_hist WHERE own_code = '" + sOwnerCode + "'";
                    if (orcResult2.Execute())
                    {
                        if (orcResult2.Read())
                        {
                            MessageBox.Show("Owner has business record", "No Business", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            orcResult2.Close();
                            return;
                        }
                    }
                }
            }
            orcResult.Close();
            // JAV 20170914 (s)
            frmBussReport form = new frmBussReport();
            string sOwnName = string.Empty;

            //sOwnName = sOwnerFN.Trim();
            //if (sOwnerMN.Trim() != "" && sOwnerMN.Trim() != ".")
            //    sOwnName += " " + sOwnerMN + ". ";
            //sOwnName += sOwnerLN;

            form.ReportSwitch = "BussCertificate";
            form.sMode = "No Record";
            form.OwnLastName = sOwnerLN;
            form.OwnFirstName = sOwnerFN;
            form.OwnMI = sOwnerMN;
            form.OwnAddress = sOwnerAddress;
            form.RequestedBy = txtRequestedBy.Text.Trim();
            form.Purpose = txtPurpose.Text.Trim();

            //form.ReportSwitch = "NoBusiness";

            // RMC 20171128 correction error in printing No Business certificate (s) -- no payment accdg to Nori Malolos
            form.ORNo = ".";
            form.ORDate = "";
            form.ORAmount = "";
            // RMC 20171128 correction error in printing No Business certificate (e)

            //btnPrint.Enabled = false;
            form.ShowDialog();
            //form.Dispose();
            //this.Dispose();
            // JAV 20170914 (e)



            //btnPrint.Enabled = false; 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}