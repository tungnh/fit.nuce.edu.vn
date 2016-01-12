using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class CommentViewhelper : BaseViewhelper
    {
        private List<gov_comments> lstComment;
        private int[] checkID;
        private Boolean changeState;
        private int filterActive;
        private int filterKey;
        private List<gov_key_band> lstKeyBand;

        public List<gov_key_band> LstKeyBand
        {
            get { return lstKeyBand; }
            set { lstKeyBand = value; }
        }

        public int FilterKey
        {
            get { return filterKey; }
            set { filterKey = value; }
        }

        public int FilterActive
        {
            get { return filterActive; }
            set { filterActive = value; }
        }

        public Boolean ChangeState
        {
            get { return changeState; }
            set { changeState = value; }
        }

        public int[] CheckID
        {
            get { return checkID; }
            set { checkID = value; }
        }

        public List<gov_comments> LstComment
        {
            get { return lstComment; }
            set { lstComment = value; }
        }
    }
}