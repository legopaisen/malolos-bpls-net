using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amellar.Common.DataConnector;
using System.Windows.Forms;

namespace Amellar.Modules.RCD
{
    class ValidateOR
    {
        public static bool HasRemainingOR(string sTeller, string sTrnDate)
        {
            OracleResultSet result = new OracleResultSet();
            result.Query = string.Format("select * from or_assigned where teller = '{0}' and trn_date = to_date('{1}', 'MM/dd/yyyy')", sTeller, sTrnDate);
            if (result.Execute())
            {
                while (result.Read())
                {
                    result.Close();
                    MessageBox.Show("You have to return your O.R. before you can proceed.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return true;
                }
                result.Close();
            }
            return false;
        }
    }
}
