using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class RoleModels
    {
        private int id;
        private String roleName;
        private String entryUsername;

        public String EntryUsername
        {
            get { return entryUsername; }
            set { entryUsername = value; }
        }
        private DateTime entryDatetime;

        public DateTime EntryDatetime
        {
            get { return entryDatetime; }
            set { entryDatetime = value; }
        }

        public String RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}