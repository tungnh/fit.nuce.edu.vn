using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class DocumentKindModels
    {
        private int kid;
        private String kCode;
        private int processPeriod;
        private String name;
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

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ProcessPeriod
        {
            get { return processPeriod; }
            set { processPeriod = value; }
        }

        public String KCode
        {
            get { return kCode; }
            set { kCode = value; }
        }

        public int Kid
        {
            get { return kid; }
            set { kid = value; }
        }
    }
}