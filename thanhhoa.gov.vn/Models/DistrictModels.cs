using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class DistrictModels
    {
        private int id;
        private String name;
        private int level;
        private String decription;
        private String link;
        private int updateUserId;
        private DateTime updateDatetime;
        private int orderNumber;
        private Boolean showMap;
        private String coordinates;

        public String Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        public Boolean ShowMap
        {
            get { return showMap; }
            set { showMap = value; }
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

        public int UpdateUserId
        {
            get { return updateUserId; }
            set { updateUserId = value; }
        }

        public String Link
        {
            get { return link; }
            set { link = value; }
        }

        public String Decription
        {
            get { return decription; }
            set { decription = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}