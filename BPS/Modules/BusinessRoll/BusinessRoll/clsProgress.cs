using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BusinessRoll
{
    class clsProgress
    {

        private static Thread th = new Thread(new ThreadStart(showProgressForm));
        public void startProgress()
        {
            th = new Thread(new ThreadStart(showProgressForm));
            th.Name = "first";
            th.Start();
        }

        private static void showProgressForm()
        {
            frmProgressBnsRoll Form = new frmProgressBnsRoll();
            Form.ShowDialog();
        }

        public void stopProgress()
        {
            th.Abort();
            th = null;
        }

    }
}
