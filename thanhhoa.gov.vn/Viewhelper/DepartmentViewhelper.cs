using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class DepartmentViewhelper : BaseViewhelper
    {
        private List<gov_department> lstDepartment;

        public List<gov_department> LstDepartment
        {
            get { return lstDepartment; }
            set { lstDepartment = value; }
        }
    }
}