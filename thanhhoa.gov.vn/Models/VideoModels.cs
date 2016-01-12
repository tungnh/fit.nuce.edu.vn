using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class VideoModels
    {
        private int id;
        private String title;
        private String avatar;
        private String attachFilepath;
        private String description;
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

        public String Description
        {
            get { return description; }
            set { description = value; }
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

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}