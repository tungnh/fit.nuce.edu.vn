using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class AccessHistoryViewhelper : BaseViewhelper
    {
        private List<gov_access_history> lstAccessHistory;
        private List<gov_access_module> lstAccessModule;
        private int accessModule;
        private int accessType;
        private List<gov_access_type> lstAccessType;

        public List<gov_access_type> LstAccessType
        {
            get { return lstAccessType; }
            set { lstAccessType = value; }
        }


        public int AccessType
        {
            get { return accessType; }
            set { accessType = value; }
        }

        public int AccessModule
        {
            get { return accessModule; }
            set { accessModule = value; }
        }

        public List<gov_access_module> LstAccessModule
        {
            get { return lstAccessModule; }
            set { lstAccessModule = value; }
        }
        public List<gov_access_history> LstAccessHistory
        {
            get { return lstAccessHistory; }
            set { lstAccessHistory = value; }
        }
    }
}