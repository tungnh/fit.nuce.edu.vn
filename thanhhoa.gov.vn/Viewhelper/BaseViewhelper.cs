using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class BaseViewhelper
    {
        private int totalCount;
        private int totalPage;
        private int page = 1;
        private String direction;
        private String keySearch;
        private int firstPage;
        private int lastPage;

        public int LastPage
        {
            get { return lastPage; }
            set { lastPage = value; }
        }

        public int FirstPage
        {
            get { return firstPage; }
            set { firstPage = value; }
        }

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        public int TotalPage
        {
            get { return totalPage; }
            set { totalPage = value; }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
    }
}