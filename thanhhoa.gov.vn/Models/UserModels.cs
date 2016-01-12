using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class UserModels
    {
        private int id;
        private String username;
        private String password;
        private String familyName;
        private String firstName;
        private String birthDay;
        private Boolean sex;
        private String mobile;
        private String phone;
        private String address;
        private String email;
        private Boolean activeFlg;
        private Boolean hiddenFlg;
        private String updateUserName;
        private DateTime updateDatetime;
        private int roleId;
        private String roleName;
        private int districtId;

        public int DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }

        public String RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        public Boolean HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }


        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public String UpdateUserName
        {
            get { return updateUserName; }
            set { updateUserName = value; }
        }

        public Boolean ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        public String Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public Boolean Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public String BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

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