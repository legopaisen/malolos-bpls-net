using System;
using System.Collections.Generic;
using System.Text;

namespace TellerSetup
{
    class EncryptPass
    {
        public string EncryptPassword(string sPass)
        {
            int intCtr = 0;
            int intLetter = 0;
            int intMediator = 0;
            string strLetter = string.Empty;
            string strConvPass = string.Empty;

            intMediator = sPass.Length;
            StringBuilder strPasswordBuild = new StringBuilder();

            for (intCtr = 0; intCtr < intMediator; intCtr++)
            {

                if ((intCtr % 2) == 0)
                    intLetter = sPass[intCtr] + intMediator + intCtr;
                else
                    intLetter = sPass[intCtr] - intMediator - intCtr;

                if (intLetter < 32)
                    intLetter = 32 + intMediator + intCtr;
                if (intLetter > 126)
                    intLetter = 126 - intMediator - intCtr;

                strConvPass = string.Format("{0}{1}", strConvPass, (char)intLetter);
            }

            sPass = strConvPass;


            return sPass;
        }
    }
}
