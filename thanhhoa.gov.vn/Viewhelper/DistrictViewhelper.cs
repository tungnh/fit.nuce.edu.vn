using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class DistrictViewhelper : BaseViewhelper
    {
        private List<DistrictModels> lstDistrict;
        private String level;
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Level
        {
            get { return level; }
            set { level = value; }
        }

        public List<DistrictModels> LstDistrict
        {
            get { return lstDistrict; }
            set { lstDistrict = value; }
        }
    }
}