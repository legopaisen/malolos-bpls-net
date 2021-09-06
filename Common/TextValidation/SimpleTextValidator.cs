using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Amellar.Common.TextValidation
{
    public class SimpleTextValidator
    {

        private bool m_blnIsAdjusting;
        private List<char> m_lstCharacterSet;
        private List<int> m_lstCharacterFreq;


        public SimpleTextValidator()
        {
            m_blnIsAdjusting = false;
            m_lstCharacterSet = new List<char>();
            m_lstCharacterFreq = new List<int>();
        }

        private void AddCharacter(char chChar, int intFreq)
        {
            m_lstCharacterSet.Add(chChar);
            if (intFreq < 0)
            {
                m_lstCharacterFreq.Add(0);
            }
            else
            {
                m_lstCharacterFreq.Add(intFreq);
            }
        }

        public void SetUnsignedIntCharacterSet()
        {
            m_lstCharacterSet.Clear();
            m_lstCharacterFreq.Clear();
            this.AddCharacter('0', 0);
            this.AddCharacter('1', 0);
            this.AddCharacter('2', 0);
            this.AddCharacter('3', 0);
            this.AddCharacter('4', 0);
            this.AddCharacter('5', 0);
            this.AddCharacter('6', 0);
            this.AddCharacter('7', 0);
            this.AddCharacter('8', 0);
            this.AddCharacter('9', 0);
        }

        public void SetIntCharacterSet()
        {
            m_lstCharacterSet.Clear();
            m_lstCharacterFreq.Clear();
            this.AddCharacter('-', 1);
            this.AddCharacter('0', 0);
            this.AddCharacter('1', 0);
            this.AddCharacter('2', 0);
            this.AddCharacter('3', 0);
            this.AddCharacter('4', 0);
            this.AddCharacter('5', 0);
            this.AddCharacter('6', 0);
            this.AddCharacter('7', 0);
            this.AddCharacter('8', 0);
            this.AddCharacter('9', 0);
        }

        public void SetDoubleCharacterSet()
        {
            m_lstCharacterSet.Clear();
            m_lstCharacterFreq.Clear();
            this.AddCharacter('.', 1);
            this.AddCharacter('0', 0);
            this.AddCharacter('1', 0);
            this.AddCharacter('2', 0);
            this.AddCharacter('3', 0);
            this.AddCharacter('4', 0);
            this.AddCharacter('5', 0);
            this.AddCharacter('6', 0);
            this.AddCharacter('7', 0);
            this.AddCharacter('8', 0);
            this.AddCharacter('9', 0);
        }

        private int GetCharacterFreq(char chChar)
        {
            int intIndex = m_lstCharacterSet.IndexOf(chChar);
            if (intIndex != -1)
            {
                return m_lstCharacterFreq[intIndex];
            }
            return -1;
        }

        public int GetCharacterFreq(string strValue, char chChar)
        {
            int intCountFreq = 0;
            int intCount = strValue.Length;
            for (int i = 0; i < intCount; i++)
            {
                if (strValue[i] == chChar)
                {
                    intCountFreq++;
                }
            }
            return intCountFreq;
        }

        public void HandleKeyPressEvent(object sender, KeyPressEventArgs e, int intMaxSize)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            
            if (strValue.Length - txt.SelectionLength >= intMaxSize && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
            


            
            int intFreq = this.GetCharacterFreq(e.KeyChar);
            if (intFreq == -1)
            {
                if (Char.IsControl(e.KeyChar) || e.KeyChar == '\b')
                    //||(e.KeyChar == ' ' && txt.SelectionLength != 0))
                {
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (intFreq == 0)
            {
            }
            else if (GetCharacterFreq(strValue, e.KeyChar) >= intFreq)
            {
                e.Handled = true;
            }
            
        }


        private bool ValidateString(string strValue, int intMaxLength)
        {
            int intCount = strValue.Length;
            if (intCount > intMaxLength)
                return false;

            int intCharFreq = 0;
            List<char> lstCharSet = new List<char>();
            for (int i = 0; i < intCount; i++)
            {
                
                if (lstCharSet.IndexOf(strValue[i]) == -1)
                {
                    intCharFreq = this.GetCharacterFreq(strValue[i]);
                    if (intCharFreq == -1)
                    {
                        return false;
                    }
                    else if (intCharFreq != 0 && (this.GetCharacterFreq(strValue, strValue[i]) > intCharFreq))
                    {
                        return false;
                    }
                    lstCharSet.Add(strValue[i]);
                }
            }
            return true;
        }

        public void HandleIntFormat(object sender, string strFormat, int intMaxLength)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            int intValue = 0;
            int.TryParse(strValue, out intValue);
            strValue = string.Format(strFormat, intValue);

            m_blnIsAdjusting = true;
            if (this.ValidateString(strValue, intMaxLength) && intValue > 0.0)
                txt.Text = strValue;
            else
                txt.Text = string.Empty;
            m_blnIsAdjusting = false;
        }

        public void HandleTextChange(object sender, int intMaxLength)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            m_blnIsAdjusting = true;
            if (!this.ValidateString(strValue, intMaxLength))
                txt.Text = string.Empty;
            m_blnIsAdjusting = false;
        }

        public void HandleDoubleFormat(object sender, string strFormat, int intMaxLength)
        {
            if (m_blnIsAdjusting)
                return;
            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();

            double dblValue = 0;
            double.TryParse(strValue, out dblValue);
            strValue = string.Format(strFormat, dblValue);
            m_blnIsAdjusting = true;
            if (this.ValidateString(strValue, intMaxLength) && dblValue > 0.0)
                txt.Text = strValue;
            else
                txt.Text = string.Empty;
            m_blnIsAdjusting = false;
        }

        public void HandleStringFormat(object sender, string strFormat, int intMaxLength)
        {
            if (m_blnIsAdjusting)
                return;

            TextBox txt = (TextBox)sender;
            string strValue = txt.Text.Trim();
            m_blnIsAdjusting = true;
            if (this.ValidateString(strValue, intMaxLength))
                txt.Text = string.Format(strFormat, strValue);
            else
                txt.Text = string.Empty;
            m_blnIsAdjusting = false;
        }
    }
}
