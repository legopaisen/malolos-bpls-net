using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace Amellar.Common.BIN
{
    public partial class BIN : UserControl
    {
        public BIN()
        {
            InitializeComponent();
        }
        public string GetLGUCode
        {
            get { return txtLGUCode.Text; }
            set { txtLGUCode.Text = value; }
        }
        public string GetDistCode
        {
            get { return txtDistCode.Text; }
            set { txtDistCode.Text = value; }
        }

        private void BIN_Load(object sender, EventArgs e)
        {

        }

        
    }
}
