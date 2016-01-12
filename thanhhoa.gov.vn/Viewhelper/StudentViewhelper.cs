using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class StudentViewhelper: BaseViewhelper
    {
        private List<gov_person> lstStudent;
        private String filterActive;
        private String changeActive;
        private int courseFilter;
        private int specializedFilter;
        private List<gov_specialized> lstSpecialized;
        private List<gov_course> lstCourse;

        public List<gov_course> LstCourse
        {
            get { return lstCourse; }
            set { lstCourse = value; }
        }

        public List<gov_specialized> LstSpecialized
        {
            get { return lstSpecialized; }
            set { lstSpecialized = value; }
        }

        public int CourseFilter
        {
            get { return courseFilter; }
            set { courseFilter = value; }
        }

        public int SpecializedFilter
        {
            get { return specializedFilter; }
            set { specializedFilter = value; }
        }

        public String ChangeActive
        {
            get { return changeActive; }
            set { changeActive = value; }
        }

        public List<gov_person> LstStudent
        {
            get { return lstStudent; }
            set { lstStudent = value; }
        }

        public String FilterActive
        {
            get { return filterActive; }
            set { filterActive = value; }
        }
    }
}