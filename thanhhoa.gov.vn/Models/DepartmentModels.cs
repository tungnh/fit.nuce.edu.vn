using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class DepartmentModels
    {
        private int deptId;
        private String deptCode;
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

        public String DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }

        public int DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
    }
}