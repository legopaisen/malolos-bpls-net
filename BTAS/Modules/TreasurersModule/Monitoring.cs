
// RMC 20111227 added Gross monitoring module for gross >= 200000

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.BPLS.TreasurersModule
{
    public class Monitoring
    {
        protected frmBTaxMonitoring MonitoringFrm = null;
        
        private string m_strSource = string.Empty;

        public string SourceClass
        {
            get { return m_strSource; }
            set { m_strSource = value; }
        }

        public Monitoring(frmBTaxMonitoring Form)
        {
            this.MonitoringFrm = Form;
        }

        public virtual void FormLoad()
        {
        }

        public virtual void UpdateList()
        {
        }

        public virtual void Approve()
        {
        }

        public virtual void Return()
        {
        }

        public virtual void Refresh()
        {
        }

        public void Search()
        {
            int iIndex = -1;
            for (int iRow = 0; iRow < MonitoringFrm.dgvList.Rows.Count; iRow++)
            {
                if (MonitoringFrm.dgvList[0, iRow].Value.ToString().Trim() == MonitoringFrm.bin1.GetBin())
                {
                    iIndex = iRow;
                }
            }

            if (iIndex != -1)
            {
                MonitoringFrm.LoadValues(iIndex);
                MonitoringFrm.dgvList.CurrentCell = MonitoringFrm.dgvList[0, iIndex];
            }
            else
            {
                MessageBox.Show("No record found.");
                this.Clear();
            }

        }

        public void Clear()
        {
            MonitoringFrm.bin1.txtTaxYear.Text = "";
            MonitoringFrm.bin1.txtBINSeries.Text = "";
            MonitoringFrm.txtBnsName.Text = "";
            MonitoringFrm.txtBnsAddress.Text = "";
            MonitoringFrm.txtOwnName.Text = "";
            MonitoringFrm.txtOwnAddress.Text = "";
            MonitoringFrm.txtOrNo.Text = ""; //JARS 20170922
            MonitoringFrm.txtMemo.Text = ""; //JARS 20170922
        }
    }
}
