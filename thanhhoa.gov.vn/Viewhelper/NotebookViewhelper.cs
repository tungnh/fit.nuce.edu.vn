using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class NotebookViewhelper : BaseViewhelper
    {
        private List<gov_notebook> lstNotebook;
        private List<gov_course> lstCourse;
        private List<gov_specialized> lstSpecialized;
        private int courseFilter;
        private int specializedFilter;
        private String filterActive;

        public String FilterActive
        {
            get { return filterActive; }
            set { filterActive = value; }
        }

        public int SpecializedFilter
        {
            get { return specializedFilter; }
            set { specializedFilter = value; }
        }

        public int CourseFilter
        {
            get { return courseFilter; }
            set { courseFilter = value; }
        }

        public List<gov_specialized> LstSpecialized
        {
            get { return lstSpecialized; }
            set { lstSpecialized = value; }
        }

        public List<gov_course> LstCourse
        {
            get { return lstCourse; }
            set { lstCourse = value; }
        }

        public List<gov_notebook> LstNotebook
        {
            get { return lstNotebook; }
            set { lstNotebook = value; }
        }
    }
}