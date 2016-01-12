using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class GroupMemberModels
    {
        private int groupId;
        private String username;
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

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
    }
}