using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class CalendarViewhelper : BaseViewhelper
    {
        private List<CalendarModels> lstCalendar;
        private int weekNum;
        private DateTime toDate;
        private DateTime fromDate;
        private int typeView = 1;
        private int mounthNum;
        private int yearNum;

        public int YearNum
        {
            get { return yearNum; }
            set { yearNum = value; }
        }

        public int MounthNum
        {
            get { return mounthNum; }
            set { mounthNum = value; }
        }

        public int TypeView
        {
            get { return typeView; }
            set { typeView = value; }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        public int WeekNum
        {
            get { return weekNum; }
            set { weekNum = value; }
        }

        public List<CalendarModels> LstCalendar
        {
            get { return lstCalendar; }
            set { lstCalendar = value; }
        }
    }
}