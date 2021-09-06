using System;
using System.Collections.Generic;
using System.Text;

namespace Amellar.Modules.PermitUpdate
{
    public class TransferApp
    {
        protected frmTransferApp TransferAppFrm = null;
        protected string m_strTransaction = string.Empty;
        protected string m_strBnsCode = string.Empty;
        protected string m_strOldOwnCode = string.Empty;

        public string TransactionName
        {
            get { return m_strTransaction; }
            set { m_strTransaction = value; }
        }
        
        public TransferApp(frmTransferApp Form)
        {
            this.TransferAppFrm = Form;
            
        }

        public virtual void FormLoad()
        {

        }

        public virtual void Save()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void SearchNew()
        {
        }
    }
}
