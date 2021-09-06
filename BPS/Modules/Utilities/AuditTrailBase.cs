using System;
using System.Collections.Generic;
using System.Text;
using Amellar.Common.SearchBusiness;
using Amellar.Common.DataConnector;

namespace Amellar.Modules.Utilities
{
    public class AuditTrailBase
    {
        protected frmAuditTrail AuditTrailFrm = null;
        
        

        public AuditTrailBase(frmAuditTrail Form)
        {
            this.AuditTrailFrm = Form;
            
        }

        public virtual void FormLoad()
        {
        }

        public virtual void SearchBin()
        {
        }

        public virtual void Generate()
        {
        }

        public virtual void UserChange()
        {
        }

        public virtual void ModuleChange()
        {
        }
    }
}
