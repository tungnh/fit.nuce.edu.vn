using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class TypeNew
    {
        private int id;
        private String name;
        private int orderNumber;

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
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