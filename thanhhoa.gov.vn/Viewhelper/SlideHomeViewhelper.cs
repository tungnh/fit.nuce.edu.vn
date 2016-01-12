using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class SlideHomeViewhelper : BaseViewhelper
    {
        private List<gov_slide_home> lstSlideHome;

        public List<gov_slide_home> LstSlideHome
        {
            get { return lstSlideHome; }
            set { lstSlideHome = value; }
        }
    }
}