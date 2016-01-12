using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class QuestionModels
    {
        private int id;
        private String fullName;
        private String email;
        private String phone;
        private String address;
        private String department;
        private String title;
        private int categoryId;
        private String attachFileName;
        private String attachFilePath;
        private String questionContent;
        private DateTime questionDatetime;
        private Boolean hiddenFlg;
        private String categoryName;
        private int countAnswer = 0;

        public int CountAnswer
        {
            get { return countAnswer; }
            set { countAnswer = value; }
        }

        public String CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        public HttpPostedFileBase File { get; set; }
        public Boolean HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public DateTime QuestionDatetime
        {
            get { return questionDatetime; }
            set { questionDatetime = value; }
        }

        public String QuestionContent
        {
            get { return questionContent; }
            set { questionContent = value; }
        }

        public String AttachFilePath
        {
            get { return attachFilePath; }
            set { attachFilePath = value; }
        }

        public String AttachFileName
        {
            get { return attachFileName; }
            set { attachFileName = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Department
        {
            get { return department; }
            set { department = value; }
        }

        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}