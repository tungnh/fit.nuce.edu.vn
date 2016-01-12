using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class AdvertiseViewhelper : BaseViewhelper
    {
        private List<gov_advertise> lstAdvertise;

        public List<gov_advertise> LstAdvertise
        {
            get { return lstAdvertise; }
            set { lstAdvertise = value; }
        }
    }
}