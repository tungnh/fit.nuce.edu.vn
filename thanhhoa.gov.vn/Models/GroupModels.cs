using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class GroupModels
    {
        private int id;
        private String name;
        private String updateUsername;
        private DateTime updateDatetime;
        private List<GroupPermissionModels> lstGroupPermission;
        private Boolean hiddenFlg;

        public Boolean HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public List<GroupPermissionModels> LstGroupPermission
        {
            get { return lstGroupPermission; }
            set { lstGroupPermission = value; }
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