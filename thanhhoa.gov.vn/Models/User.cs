using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class User
    {
        private int id;
        private String username;
        private String password;

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}