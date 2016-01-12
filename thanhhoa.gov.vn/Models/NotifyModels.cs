using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class NotifyModels
    {
        private int id;
        private String title;
        private String content;
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

        public String Content
        {
            get { return content; }
            set { content = value; }
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