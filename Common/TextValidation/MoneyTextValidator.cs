using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.TextValidation
{
    public class MoneyTextValidator
    {
        private bool m_blnIsAdjusting;
        private string m_strOldValue;

        public MoneyTextValidator()
        {
            m_blnIsAdjusting = false;
            m_strOldValue = string.Empty;
        }

        public void HandleKeyPressEvent(object sender, KeyPressEventArgs e)
        {
            if (m_blnIsAdjusting)
                return;
            
            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim(); //current value
            //checks if number of decimals is beyond
            if (Char.IsControl(e.KeyChar) || e.KeyChar == '\b')
            {
            }
            else
            {
                string strNewValue = string.Empty;
                strNewValue = string.Format("{0}{1}{2}", strValue.Substring(0, txt.SelectionStart), e.KeyChar,
                   strValue.Substring(txt.SelectionStart + txt.SelectionLength));
                if (Char.IsDigit(e.KeyChar) || (e.KeyChar == '.' && strValue.IndexOf('.') == -1) )
                {
                }
                else
                {
                    e.Handled = true;
                    return;
                }
                //decimal fix
                int intIndex = strNewValue.IndexOf(".");
                if (intIndex != -1 && strNewValue.Length-1 > intIndex + 2)
                {
                    e.Handled = true;
                    return;
                }
                //zero fix
                if (intIndex == -1 && strNewValue.Length > 1)
                {
                    double dblValue = 0.0;
                    double.TryParse(strNewValue, out dblValue);
                    if (dblValue == 0.0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        public void HandleTextChange(object sender, int intMaxLength)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            //convert to double
            double dblValue = 0.0;
            double.TryParse(strValue, out dblValue);

            m_blnIsAdjusting = true;
            bool blnHasInvalidChars = false;

            for (int i = 0; i < strValue.Length; i++)
            {
                //if (Char.IsDigit(strValue[i]) || strValue[i] == ',' || strValue[i] == '.')
                if (Char.IsDigit(strValue[i]) || strValue[i] == '.' || strValue[i] == ',') //RDO 04082008 added comma validation
                {
                }
                else
                {
                    blnHasInvalidChars = true;
                    break;
                }
            }
            //decimal fix
            if (!blnHasInvalidChars)
            {
                int intIndex = -1;
                intIndex = strValue.IndexOf('.');
                if (intIndex != -1 && strValue.IndexOf('.', intIndex + 1) != -1)
                    blnHasInvalidChars = true;
                if (intIndex != -1 && strValue.Length - 1 > intIndex + 2)
                    blnHasInvalidChars = true;
            }
            //comma fix
            //RDO 04082008 (s) added comma validation
            if (!blnHasInvalidChars)
            {
                int intIndex = -1;
                intIndex = strValue.IndexOf('.');
                if (intIndex == -1)
                    intIndex = strValue.Length - 1; //last index
                //RDO 04112008 (s) fix empty string
                if (intIndex == -1)
                    intIndex = 0;
                //RDO 04112008 (e) fix empty string

                int intTmpIndex = 0;
                int intTmpCounter = 1;
                string strTmpValue = strValue;
                intTmpIndex = strTmpValue.LastIndexOf(',');
                if (intTmpIndex >= intIndex)
                    blnHasInvalidChars = true;
                else
                {
                    while (intTmpIndex != -1)
                    {
                        if ((intIndex - intTmpIndex - intTmpCounter) % 3 != 0)
                        {
                            blnHasInvalidChars = true;
                            break;
                        }
                        intTmpCounter++;
                        strTmpValue = strTmpValue.Substring(0, intTmpIndex);
                        intTmpIndex = strTmpValue.LastIndexOf(',');
                    }
                }
            }
            //RDO 04082008 (e) added comma validation

            /*
            //comma fix
            if (!blnHasInvalidChars)
            {
                int intLastIndex = -1;
                int intIndex = 0;
                while (strValue.IndexOf(',', intLastIndex+1) != -1)
                {
                    intIndex = strValue.IndexOf(',', intLastIndex + 1);
                    if (intIndex == 0) //invalid index
                    {
                        blnHasInvalidChars = true;
                        break;
                    }
                    else if (intIndex - (intLastIndex + 1) > 3)
                    {
                        blnHasInvalidChars = true;
                        break;
                    }
                    intLastIndex = intIndex;
                }
            }
            */
            if (blnHasInvalidChars)
            {
                txt.Text = m_strOldValue; //string.Empty;
            }

            m_strOldValue = txt.Text;

            m_blnIsAdjusting = false;
        }

        public void HandleFormat(object sender)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            //convert to double
            double dblValue = 0.0;
            double.TryParse(strValue, out dblValue);

            m_blnIsAdjusting = true;
            txt.Text = string.Format("{0:#,##0.00}", dblValue);
            m_blnIsAdjusting = false;

        }

    }
}
