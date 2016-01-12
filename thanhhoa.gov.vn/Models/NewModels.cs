using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class NewModels
    {
        private int id;
        private int menuId;
        private int typeId;
        private String title;
        private String description;
        private String newContent;
        private String newSource;
        private String avatarImage;
        private Boolean activeFlg;
        private String updateUsername;
        private DateTime updateDatetime;
        private Boolean isHome;
        private String entryUsername;
        private DateTime entryDatetime;
        private int totalView;


        public int TotalView
        {
            get { return totalView; }
            set { totalView = value; }
        }

        public DateTime EntryDatetime
        {
            get { return entryDatetime; }
            set { entryDatetime = value; }
        }

        public String EntryUsername
        {
            get { return entryUsername; }
            set { entryUsername = value; }
        }

        public Boolean IsHome
        {
            get { return isHome; }
            set { isHome = value; }
        }

        public String AvatarImage
        {
            get { return avatarImage; }
            set { avatarImage = value; }
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

        public Boolean ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String NewSource
        {
            get { return newSource; }
            set { newSource = value; }
        }

        public String NewContent
        {
            get { return newContent; }
            set { newContent = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public int MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}