using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class MenuViewhelper : BaseViewhelper
    {
        private List<gov_menu> lstMenuModels;

        public List<gov_menu> LstMenuModels
        {
            get { return lstMenuModels; }
            set { lstMenuModels = value; }
        }
    }
}