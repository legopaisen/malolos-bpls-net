using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amellar.Common.DataConnector;

namespace NewPayment
{
    public partial class QtrSelect : Form
    {
        public string strBin = string.Empty;
        public QtrSelect()
        {
            InitializeComponent();
        }

        private void LoadBinValues(string sBin)
        {
            OracleResultSet result = new OracleResultSet();
            
            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            TreeNode treeNode = new TreeNode("Dot Net Perls", array);
            trvBin.Nodes.Add(treeNode);
            
        }


        private void QtrSelect_Load(object sender, EventArgs e)
        {
            LoadBinValues(strBin);
        }
    }
}