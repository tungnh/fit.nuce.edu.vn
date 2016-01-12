using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class RoleViewhelper : BaseViewhelper
    {
        private List<gov_role> lstRole;

        public List<gov_role> LstRole
        {
            get { return lstRole; }
            set { lstRole = value; }
        }
    }
}