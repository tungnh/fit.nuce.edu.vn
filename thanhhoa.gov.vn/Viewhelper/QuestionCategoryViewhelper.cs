using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class QuestionCategoryViewhelper : BaseViewhelper
    {
        private List<QuestionCategoryModels> lstQuestionCategory;

        public List<QuestionCategoryModels> LstQuestionCategory
        {
            get { return lstQuestionCategory; }
            set { lstQuestionCategory = value; }
        }
    }
}