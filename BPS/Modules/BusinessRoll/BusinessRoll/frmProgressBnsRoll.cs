using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BusinessRoll
{
    public partial class frmProgressBnsRoll : Form
    {
        public frmProgressBnsRoll()
        {
            InitializeComponent();
        }

        public static frmProgressBnsRoll frmprogress = null;
        public static frmProgressBnsRoll Instance()
        {
            if (frmprogress == null) { frmprogress = new frmProgressBnsRoll(); }

            return frmprogress;
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }

        double myDoubleTmp = 0;
        public void UpdateProgress(int iMax, int iValue)
        {
            progressBar1.Maximum = iMax;
            progressBar1.Value = iValue;

            double dPercent = 0;
            dPercent = (progressBar1.Maximum * 1) / 100;
            double myDouble = 0;
            myDouble = progressBar1.Value / dPercent;            

            //if (myDouble > myDoubleTmp)
            //{
            //    if (this.Text == "Loading, please wait")
            //        this.Text = "Loading, please wait.";
            //    else if (this.Text == "Loading, please wait.")
            //        this.Text = "Loading, please wait..";
            //    else if (this.Text == "Loading, please wait..")
            //        this.Text = "Loading, please wait...";
            //    else if (this.Text == "Loading, please wait...")
            //        this.Text = "Loading, please wait";


            //    myDoubleTmp = myDouble;
            //}

            lblProgress.Text = iValue + " of " + iMax;

            if (progressBar1.Value == progressBar1.Maximum)
            {
                this.Close();
            }

        }
    }
}
