using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class CommentModels
    {
        private int id;
        private int userId;
        private int parentId;
        private int newsId;
        private String commentsContent;
        private int totalLike;
        private String likeInfo;
        private String fullName;
        private Boolean activeFlg;
        private DateTime entryDatetime;
        private String email;

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public DateTime EntryDatetime
        {
            get { return entryDatetime; }
            set { entryDatetime = value; }
        }

        public Boolean ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public String LikeInfo
        {
            get { return likeInfo; }
            set { likeInfo = value; }
        }

        public int TotalLike
        {
            get { return totalLike; }
            set { totalLike = value; }
        }


        public String CommentsContent
        {
            get { return commentsContent; }
            set { commentsContent = value; }
        } 

        public int NewsId
        {
            get { return newsId; }
            set { newsId = value; }
        }


        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}