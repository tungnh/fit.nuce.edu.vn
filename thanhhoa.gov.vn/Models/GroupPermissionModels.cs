using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class GroupPermissionModels
    {
        private int groupId;
        private int moduleId;
        private int permissionNumber;
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

        public int PermissionNumber
        {
            get { return permissionNumber; }
            set { permissionNumber = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
    }
}