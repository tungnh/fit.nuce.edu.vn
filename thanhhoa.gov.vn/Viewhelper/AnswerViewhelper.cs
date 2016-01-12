using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class AnswerViewhelper : BaseViewhelper
    {
        private List<AnswerModels> lstAnswer;

        public List<AnswerModels> LstAnswer
        {
            get { return lstAnswer; }
            set { lstAnswer = value; }
        }
    }
}