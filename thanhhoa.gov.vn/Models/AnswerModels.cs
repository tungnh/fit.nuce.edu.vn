using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class AnswerModels
    {
        private int id;
        private int questionId;
        private String answerContent;
        private String answerUsername;
        private String updateUsername;
        private DateTime answerDatetime;
        private DateTime updateDatetime;
        private Boolean hiddenFlg;
        private UserModels userInfo;

        public UserModels UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; }
        }

        public Boolean HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public DateTime AnswerDatetime
        {
            get { return answerDatetime; }
            set { answerDatetime = value; }
        }

        public String UpdateUsername
        {
            get { return updateUsername; }
            set { updateUsername = value; }
        }

        public String AnswerUsername
        {
            get { return answerUsername; }
            set { answerUsername = value; }
        }

        public String AnswerContent
        {
            get { return answerContent; }
            set { answerContent = value; }
        }

        public int QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}