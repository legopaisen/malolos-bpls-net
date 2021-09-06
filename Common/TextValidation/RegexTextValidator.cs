using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.TextValidation
{
    public class RegexTextValidator
    {
        private string m_strPattern;

        public RegexTextValidator()
        {
            m_strPattern = string.Empty;
        }

        public void SetPattern(string strPattern)
        {
            m_strPattern = strPattern;
        }

        public void HandleKeyPressEvent(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();

            //get new value and check 
            if (Char.IsControl(e.KeyChar) || e.KeyChar == '\b')
            {
            }

        }

    }
}
