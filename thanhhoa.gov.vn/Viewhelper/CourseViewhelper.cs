using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class CourseViewhelper : BaseViewhelper
    {
        private List<gov_course> lstCourse;

        public List<gov_course> LstCourse
        {
            get { return lstCourse; }
            set { lstCourse = value; }
        }
    }
}