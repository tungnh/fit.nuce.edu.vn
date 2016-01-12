using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class MenuModels
    {
        private int id;
        private int parentId;
        private String title;
        private int orderNumber;
        private String link;
        private Boolean ativeFlg;
        private String entryUsername;
        private DateTime entryDatetime;
        private String updateUsername;
        private DateTime updateDatetime;
        private String description;
        private List<NewModels> lstNewInMenu;
        private String parentName;
        private Boolean typeLink;
        private Boolean isHome;

        public Boolean IsHome
        {
            get { return isHome; }
            set { isHome = value; }
        }

        public Boolean TypeLink
        {
            get { return typeLink; }
            set { typeLink = value; }
        }

        public String ParentName
        {
            get { return parentName; }
            set { parentName = value; }
        }

        public List<NewModels> LstNewInMenu
        {
            get { return lstNewInMenu; }
            set { lstNewInMenu = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
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

        public Boolean AtiveFlg
        {
            get { return ativeFlg; }
            set { ativeFlg = value; }
        }

        public String Link
        {
            get { return link; }
            set { link = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}