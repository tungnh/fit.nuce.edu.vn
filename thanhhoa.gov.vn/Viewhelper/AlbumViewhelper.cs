using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class AlbumViewhelper : BaseViewhelper
    {
        private List<gov_album> lstAlbum;

        public List<gov_album> LstAlbum
        {
            get { return lstAlbum; }
            set { lstAlbum = value; }
        }
    }
}