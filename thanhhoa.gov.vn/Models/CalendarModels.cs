using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class CalendarModels
    {
        private int id;
        private DateTime date;
        private String time;
        private int timeRf;
        private String description;
        private String location;
        private String personExecute;
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

        public String PersonExecute
        {
            get { return personExecute; }
            set { personExecute = value; }
        }

        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int TimeRf
        {
            get { return timeRf; }
            set { timeRf = value; }
        }

        public String Time
        {
            get { return time; }
            set { time = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}