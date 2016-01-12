using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class DocumentModels
    {
        private int id;
        private String docCode;
        private String docName;
        private String docSummany;
        private String docContent;
        private int kindId;
        private int departmentId;
        private DateTime dateStartpromulgate;
        private DateTime dateEndPromulgate;
        private String userSign;
        private Boolean activeFlg;
        private String note;
        private String attachFileName;
        private String attachFilePath;
        private String updateUsername;
        private DateTime updateDatetime;
        public HttpPostedFileBase File { get; set; }
        private String documentKindName;
        private String departmentName;

        public String DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }

        public String DocumentKindName
        {
            get { return documentKindName; }
            set { documentKindName = value; }
        }
        public String DocCode
        {
            get { return docCode; }
            set { docCode = value; }
        }
        public int DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        public String UserSign
        {
            get { return userSign; }
            set { userSign = value; }
        }

        public DateTime DateStartpromulgate
        {
            get { return dateStartpromulgate; }
            set { dateStartpromulgate = value; }
        }

        public DateTime DateEndPromulgate
        {
            get { return dateEndPromulgate; }
            set { dateEndPromulgate = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int KindId
        {
            get { return kindId; }
            set { kindId = value; }
        }

        public String DocName
        {
            get { return docName; }
            set { docName = value; }
        }

        public String DocSummany
        {
            get { return docSummany; }
            set { docSummany = value; }
        }

        public String DocContent
        {
            get { return docContent; }
            set { docContent = value; }
        }

        public String Note
        {
            get { return note; }
            set { note = value; }
        }


        public Boolean ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String UpdateUsername
        {
            get { return updateUsername; }
            set { updateUsername = value; }
        }

        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public String AttachFileName
        {
            get { return attachFileName; }
            set { attachFileName = value; }
        }

        public String AttachFilePath
        {
            get { return attachFilePath; }
            set { attachFilePath = value; }
        }
    }
}