using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class AdvertiseModels
    {
        private int id;
        private String title;
        private String department;
        private String attachFile;
        private String link;
        private Boolean typeLink;
        private int price;
        private DateTime startDate;
        private DateTime endDate;
        private int orderNumber;
        private String note;
        private String updateUsername;
        private DateTime updateDatetime;
        private int location;
        private Boolean activeFlg;

        public Boolean ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public int Location
        {
            get { return location; }
            set { location = value; }
        }

        public String Link
        {
            get { return link; }
            set { link = value; }
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

        public String Note
        {
            get { return note; }
            set { note = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public Boolean TypeLink
        {
            get { return typeLink; }
            set { typeLink = value; }
        }

        public String AttachFile
        {
            get { return attachFile; }
            set { attachFile = value; }
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Department
        {
            get { return department; }
            set { department = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }
    }
}