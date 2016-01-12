using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class NewViewhelper : BaseViewhelper
    {
        private int chanelId;
        private List<gov_news> lstNew;
        private gov_menu menuInfo;
        private List<gov_menu> lstMenu;
        private int menuFilter;
        private String filterActive;

        public String FilterActive
        {
            get { return filterActive; }
            set { filterActive = value; }
        }

        public int MenuFilter
        {
            get { return menuFilter; }
            set { menuFilter = value; }
        }

        public List<gov_menu> LstMenu
        {
            get { return lstMenu; }
            set { lstMenu = value; }
        }

        public gov_menu MenuInfo
        {
            get { return menuInfo; }
            set { menuInfo = value; }
        }

        public List<gov_news> LstNew
        {
            get { return lstNew; }
            set { lstNew = value; }
        }

        public int ChanelId
        {
            get { return chanelId; }
            set { chanelId = value; }
        }
    }
}