using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class NotifyViewhelper : BaseViewhelper
    {
        private List<NotifyModels> lstNotify;

        public List<NotifyModels> LstNotify
        {
            get { return lstNotify; }
            set { lstNotify = value; }
        }
    }
}