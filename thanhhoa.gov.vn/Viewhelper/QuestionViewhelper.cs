using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class QuestionViewhelper : BaseViewhelper
    {
        private List<QuestionModels> lstQuestion;
        private List<QuestionCategoryModels> lstQuestionCategory;
        private int questionCategoryId;
        private String questionCategoryName;
        private int[] checkID;
        private Boolean changeState;
        private String changeFilter;

        public String ChangeFilter
        {
            get { return changeFilter; }
            set { changeFilter = value; }
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

        public String QuestionCategoryName
        {
            get { return questionCategoryName; }
            set { questionCategoryName = value; }
        }

        public int QuestionCategoryId
        {
            get { return questionCategoryId; }
            set { questionCategoryId = value; }
        }

        public List<QuestionCategoryModels> LstQuestionCategory
        {
            get { return lstQuestionCategory; }
            set { lstQuestionCategory = value; }
        }

        public List<QuestionModels> LstQuestion
        {
            get { return lstQuestion; }
            set { lstQuestion = value; }
        }
    }
}