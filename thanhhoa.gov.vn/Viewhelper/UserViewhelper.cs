using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class UserViewhelper : BaseViewhelper
    {
        private List<gov_user> lstUser;
        private String filterActive;

        public String FilterActive
        {
            get { return filterActive; }
            set { filterActive = value; }
        }

        public List<gov_user> LstUser
        {
            get { return lstUser; }
            set { lstUser = value; }
        }
    }
}