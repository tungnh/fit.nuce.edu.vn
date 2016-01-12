using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class VideoViewhelper : BaseViewhelper
    {
        private List<gov_video> lstVideo;

        public List<gov_video> LstVideo
        {
            get { return lstVideo; }
            set { lstVideo = value; }
        }
    }
}