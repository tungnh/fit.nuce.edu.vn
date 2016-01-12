using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class StatisticAccessModels
    {
        private int count;
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
    }
}