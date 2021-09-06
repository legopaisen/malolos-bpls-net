using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.StringUtilities;

namespace Amellar.Modules.Utilities
{
    public partial class frmBnsTypeCode : Form
    {
        private string [] m_sArraySubCat = new string[1000];
        private int m_intSwitch;
        private int m_intRow;
        private string m_sTmpCode = string.Empty;

        public string BnsCode
        {
            get { return txtCode.Text; }
            set { txtCode.Text = value; }
        }

        public string [] ArraySubCat
        {
            get { return m_sArraySubCat; }
            set { m_sArraySubCat = value; }
        }

        public int Switch
        {
            get { return m_intSwitch; }
            set { m_intSwitch = value; }
        }

        public int Row
        {
            get { return m_intRow; }
            set { m_intRow = value; }
        }

        public frmBnsTypeCode()
        {
            InitializeComponent();
        }

        private void frmBnsTypeCode_Load(object sender, EventArgs e)
        {
            m_sTmpCode = txtCode.Text.ToString().Trim();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool bSw;
            string strCode = txtCode.Text.ToString().Trim();

            int iLen = txtCode.Text.ToString().Length;

            if ((iLen % 2) == 0 && txtCode.Text.ToString() != "")
            {
                bSw = false;
                bool xy;
                xy = false;

                if (m_sTmpCode != strCode)
                {
                    for (int x = 1; x <= m_intRow; x++)
                    {
                        if (strCode == m_sArraySubCat[x])
                        {
                            MessageBox.Show("Business Type Code Already exists...", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            xy = true;
                            txtCode.Text = m_sTmpCode;
                            break;
                        }
                    }
                }

                if (!xy)
                {
                    if(StringUtilities.Left(m_sTmpCode,2) != StringUtilities.Left(strCode,2))
                    {
                        MessageBox.Show("Invalid Business Type Code... Should start with " + StringUtilities.Left(m_sTmpCode, 2), "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtCode.Text = m_sTmpCode;
                    }
                    else
                    {
                        m_sTmpCode = strCode;
                        bSw = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid Business Type Code... Should Be Divisible By 2...", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                bSw = false;  
            }


            if (bSw)
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.txtCode.Text = "";
            this.Close();
        }
    }
}