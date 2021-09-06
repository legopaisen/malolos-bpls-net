using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.Message_Box
{
    public partial class frmMsgBoxOptions : Form
    {
        public frmMsgBoxOptions()
        {
            InitializeComponent();
        }

        private string m_sMsgTitle = "Message Box";
        private string m_sMsgCaption = String.Empty;

        private string m_sSelectedString = String.Empty;

        private string m_MsgRDOYes = String.Empty;
        private string m_MsgRDONo = String.Empty;
        private string m_MsgRDOBoth = String.Empty;

        private int m_iRadioType = 0;

        public string MessageCaption
        {
            set { m_sMsgCaption = value; }
        }
        public string SelectedText 
        {
            get { return m_sSelectedString; }
            set { m_sSelectedString = value; }
        }
        public string RadioYes
        {
            set { m_MsgRDOYes = value; }
        }
        public string RadioNo
        {
            set { m_MsgRDONo = value; }
        }
        public string RadioBoth
        {
            set { m_MsgRDOBoth = value; }
        }
        public int RadioType
        {
            set { m_iRadioType = value; }
        }
        
        private void frmMesgBoxOptions_Load(object sender, EventArgs e)
        {
            this.Text = m_sMsgTitle;
            lblMsg.Text = m_sMsgCaption;

            if (m_iRadioType == 0)
                pnlYN.BringToFront();
            else
                pnlYNB.BringToFront();

            rdoPnlYNYes.Text = m_MsgRDOYes;
            rdoPnlYNNo.Text = m_MsgRDONo;
            rdopnlYNBYes.Text = m_MsgRDOYes;
            rdopnlYNBNo.Text = m_MsgRDONo;
            rdopnlYNBBoth.Text = m_MsgRDOBoth;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            m_sSelectedString = String.Empty;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (m_sSelectedString == String.Empty)
                return;

            this.Close();
        }

        private void rdopnlYNBYes_Click(object sender, EventArgs e)
        {
            m_sSelectedString = "Yes";
        }

        private void rdopnlYNBNo_Click(object sender, EventArgs e)
        {
            m_sSelectedString = "No";
        }

        private void rdopnlYNBBoth_Click(object sender, EventArgs e)
        {
            m_sSelectedString = "Both";
        }

        private void rdoPnlYNYes_Click(object sender, EventArgs e)
        {
            m_sSelectedString = "Yes";
        }

        private void rdoPnlYNNo_Click(object sender, EventArgs e)
        {
            m_sSelectedString = "No";
        }

    }
}