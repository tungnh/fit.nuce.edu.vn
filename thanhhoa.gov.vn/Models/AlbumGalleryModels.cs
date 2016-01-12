using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class AlbumGalleryModels
    {
        private int albumId;
        private int galleryId;

        public int GalleryId
        {
            get { return galleryId; }
            set { galleryId = value; }
        }

        public int AlbumId
        {
            get { return albumId; }
            set { albumId = value; }
        }
    }
}