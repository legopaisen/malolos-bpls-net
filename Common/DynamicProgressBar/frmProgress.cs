using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Amellar.Common.AppSettings;
using Amellar.Common.DataConnector;

namespace Amellar.Common.DynamicProgressBar
{
    /// <summary>
    /// Reusable Progressbar using anonymous delegate implementation JVL20090619 
    /// </summary>
    public partial class frmProgress : Form
    {
        public enum ProgressMode
        {
            LabelMode,           
            StartProgressMode,
            UpdateProgressMode,
            EndProgressMode,
            CancelProgressMode
        }

        public bool Cancel
        {
            get { return m_blnIsCancelled; }
        }
        private bool m_blnIsCancelled;

        public bool ViewCancelButton
        {
            get { return m_blnViewCancelButton; }
            set { m_blnViewCancelButton = value; }
        }
        private bool m_blnViewCancelButton;



/*
 public void DoSomething(int a){
    System.Diagnostics.Debug.WriteLine(string.Format("a: {0}", a));
}

public delegate void DifferentDelegate(int value);

public static void DoSomethingDifferent(int b, DifferentDelegate c) {
 for (int i = 0; i <= b; ++i) {
  c(i); // NOTE: invoked with a parameter
 }
}
//DoSomethingDifferent(3,  DoSomething);
*/

        private double m_dblSeconds;
        public double TimeElapse
        {
            get { return m_dblSeconds; }
        }


        private double m_dblTimeOutTime;
        public double TimeOutTime
        {
            get { return m_dblTimeOutTime; }
            set { m_dblTimeOutTime = value; }
        }


        public void ProgressBarWork(object value, ProgressMode mode)
        {
            if (!this.IsDisposed)
            {
                try
                {
                    DoWork dw = new DoWork(DoWorkImpl);
                    this.Invoke(dw, mode, value);
                }
                catch
                { }
            }
        
        }


        public delegate void DoWork(ProgressMode mode, object value);
        


        public frmProgress()
        {
            InitializeComponent();

            //initialize values
            m_dblTimeOutTime = 0;
            m_dblSeconds = 0;
            m_blnIsCancelled = false;
            m_blnViewCancelButton = true;

        }

        private void frmProgress_Load(object sender, EventArgs e)
        {
            btnCancel.Visible = m_blnIsCancelled;
        }

        public void DoWorkImpl(ProgressMode mode, object value)
        {
            if (mode == ProgressMode.LabelMode)
            {               
                lblProgress.Text = value.ToString();
            }
            else if (mode == ProgressMode.StartProgressMode)
            {
                pgbProgress.Maximum = (int)value;
                pgbProgress.Value = 0;
                m_dblSeconds = 0;
                tmrStatistics.Enabled = true;
                tmrStatistics.Start();

            }
            else if (mode == ProgressMode.UpdateProgressMode)
            {
                pgbProgress.Value = (int)value;
            }

            else if (mode == ProgressMode.EndProgressMode)
            {
                lblProgress.Text = "Done";
                pgbProgress.Value = pgbProgress.Maximum;
                tmrStatistics.Enabled = false;
                tmrStatistics.Stop();

                this.Dispose();
            }
            else if (mode == ProgressMode.CancelProgressMode)
            {
                lblProgress.Text = "Cancelled";
                this.Dispose();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_blnIsCancelled = true;
            //this.Dispose();
            this.Close();
        }

        private void frmProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_blnIsCancelled = true;
            tmrStatistics.Stop();
            this.Dispose();
        }

        private void tmrStatistics_Tick(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} seconds", m_dblSeconds.ToString("0.0"));
            m_dblSeconds += 0.1;

            if (m_dblTimeOutTime > 0 && m_dblSeconds > m_dblTimeOutTime)
            {
                tmrStatistics.Enabled = false;
                tmrStatistics.Stop();
                m_blnIsCancelled = true;
                this.Dispose();
                
            }
        }


    }
}