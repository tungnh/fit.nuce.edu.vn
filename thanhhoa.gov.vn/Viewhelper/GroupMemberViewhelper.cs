using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class GroupMemberViewhelper : BaseViewhelper
    {
        private List<gov_user> lstGroupMember;
        private int groupId;
        private int userNotInGroup;
        private int userInGroup;
        private List<gov_user> lstUser;
        private String[] username;

        public String[] Username
        {
            get { return username; }
            set { username = value; }
        }

        public List<gov_user> LstUser
        {
            get { return lstUser; }
            set { lstUser = value; }
        }

        public int UserInGroup
        {
            get { return userInGroup; }
            set { userInGroup = value; }
        }

        public int UserNotInGroup
        {
            get { return userNotInGroup; }
            set { userNotInGroup = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public List<gov_user> LstGroupMember
        {
            get { return lstGroupMember; }
            set { lstGroupMember = value; }
        }
    }
}