using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class GalleryViewhelper : BaseViewhelper
    {
        private List<gov_gallery> lstGallery;
        private List<gov_album_gallery> lstAlbumGallery;
        private int albumId;
        private int[] checkId;

        private Boolean filter;

        public int AlbumId
        {
            get { return albumId; }
            set { albumId = value; }
        }
        
        public int[] CheckId
        {
            get { return checkId; }
            set { checkId = value; }
        }

        public Boolean Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public List<gov_gallery> LstGallery
        {
            get { return lstGallery; }
            set { lstGallery = value; }
        }
        public List<gov_album_gallery> LstAlbumGallery
        {
            get { return lstAlbumGallery; }
            set { lstAlbumGallery = value; }
        }
    }
}