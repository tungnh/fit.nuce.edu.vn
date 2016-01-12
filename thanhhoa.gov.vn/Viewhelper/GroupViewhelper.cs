using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class GroupViewhelper : BaseViewhelper
    {
        private List<gov_group> lstGroup;

        public List<gov_group> LstGroup
        {
            get { return lstGroup; }
            set { lstGroup = value; }
        }
    }
}