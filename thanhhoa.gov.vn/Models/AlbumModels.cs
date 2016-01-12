using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class AlbumModels
    {
        private int id;
        private String albumTitle;
        private String description;
        private String avatar;
        private int totalView;
        private int orderNumber;
        private String updateUsername;
        private DateTime updateDatetime;
        private int totalGallery;

        public int TotalGallery
        {
            get { return totalGallery; }
            set { totalGallery = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }
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

        public int TotalView
        {
            get { return totalView; }
            set { totalView = value; }
        }

        public String Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String AlbumTitle
        {
            get { return albumTitle; }
            set { albumTitle = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}