using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class GalleryModels
    {
        private int id;
        private int albumId;
        private String title;
        private int totalView;
        private String avatar;
        private String attachFilepath;
        private String updateUsername;
        private DateTime updateDatetime;

        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public String UpdateUsername
        {
            get { return updateUsername; }
            set { updateUsername = value; }
        } 

        public String AttachFilepath
        {
            get { return attachFilepath; }
            set { attachFilepath = value; }
        }

        public String Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }

        public int TotalView
        {
            get { return totalView; }
            set { totalView = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public int AlbumId
        {
            get { return albumId; }
            set { albumId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}