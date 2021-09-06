/////////////////////////

// RMC 20111013 added additional column in Owner's query report
// RMC 20111006 corrected owner's query

/////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;
using Amellar.Common.AppSettings;
using Amellar.Common.StringUtilities;

namespace Amellar.BPLS.SearchAndReplace
{
    public partial class frmOwnerQuery : Form
    {
        OracleResultSet pSet = new OracleResultSet();
        //private PrintInspection PrintClass = null; 
        private string m_strOption = string.Empty;
        private string m_sSource = string.Empty;
        
        public string Option
        {
            get { return m_strOption; }
            set { m_strOption = value; }
        }

        public string Source
        {
            get { return m_sSource; }
            set { m_sSource = value; }
        }

        public frmOwnerQuery()
        {
            InitializeComponent();
            
        }

        private void frmPrintOptions_Load(object sender, EventArgs e)
        {
            LoadBrgy();
            chkName.Checked = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadBrgy()
        {
            OracleResultSet pSet = new OracleResultSet();

            pSet.Query = "SELECT DISTINCT(TRIM(BRGY_NM)) FROM BRGY  ORDER BY TRIM(BRGY_NM) ASC";
            if (pSet.Execute())
            {
                cmbBrgy.Items.Add("ALL");

                while (pSet.Read())
                {
                    cmbBrgy.Items.Add(pSet.GetString(0).Trim());
                }
            }
            pSet.Close();
        }

        private void chkName_CheckStateChanged(object sender, EventArgs e)
        {
            OracleResultSet pSet = new OracleResultSet();

            if (chkName.CheckState.ToString() == "Checked")
            {
                chkBrgy.Checked = false;
                cmbBrgy.Enabled = false; //JARS 20170707

                txtLastName.Enabled = true;
                txtFirstName.Enabled = true;
            }
        }

        private void chkBrgy_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkBrgy.CheckState.ToString() == "Checked")
            {
                chkName.Checked = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false; //JARS 20170707

                cmbBrgy.Enabled = true;
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string sBrgy = "";
            string sSortOrder = "";
            string sFirstName = "";
            string sLastName = "";

            if (cmbBrgy.Text.Trim() == "" && txtLastName.Text.Trim() == "" && txtFirstName.Text.Trim() == "")
            {
                MessageBox.Show("Specify search criteria", "Owner Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // RMC 20111006 corrected owner's query (s)
            if (txtLastName.Text.Trim() == "")
                sLastName = "%%"; //JARS 20170707 FROM txtLastName to sLastName

            if (txtFirstName.Text.Trim() == "")
                sFirstName = "%%"; //JARS 20170707 FROM txtFirstName to sFirstName
            // RMC 20111006 corrected owner's query (e)

            sBrgy = cmbBrgy.Text.Trim();

            //if (cmbBrgy.Text == "ALL" || cmbBrgy.Text == "")
            //    sBrgy = "%%";

            if (sBrgy == "ALL" || sBrgy == "") //JARS 20170707 
                sBrgy = "%%";

            if(chkName.Checked)
                sSortOrder = "own_ln,own_fn";
            else
                sSortOrder = "own_brgy,own_ln,own_fn";

            //pSet.Query = string.Format("select * from own_names where own_ln like '{0}' and own_fn like '{1}' and own_brgy like '{2}' order by {3}", StringUtilities.HandleApostrophe(txtLastName.Text.Trim()), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(txtFirstName.Text.Trim())), sBrgy, sSortOrder);
            pSet.Query = string.Format("select * from own_names where own_ln like '{0}' and own_fn like '{1}' and own_brgy like '{2}' order by {3}", StringUtilities.HandleApostrophe(sLastName.Trim()), StringUtilities.HandleApostrophe(StringUtilities.SetEmptyToSpace(sFirstName.Trim())), sBrgy, sSortOrder);
            if (pSet.Execute())
            {
                if (pSet.Read())
                {
                    pSet.Close();

                    Print PrintClass = new Print();
                    PrintClass.Query = pSet.Query.ToString();
                    if (chkBIN.Checked)
                        PrintClass.PrintBIN = true;
                    else
                        PrintClass.PrintBIN = false;

                    if (chkPrevOwn.Checked)
                        PrintClass.PrintPrevOwnEntry = true;
                    else
                        PrintClass.PrintPrevOwnEntry = false;

                    // RMC 20111013 added additional column in Owner's query report (s)
                    if (chkOwnPlace.Checked)
                        PrintClass.PrintOwnPlaceEntry = true;
                    else
                        PrintClass.PrintOwnPlaceEntry = false;
                    // RMC 20111013 added additional column in Owner's query report (e)

                    PrintClass.FormLoad();
                }
                else
                {
                    MessageBox.Show("No record found.", "Owner Query", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }



        }

        
    }
}