using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class TestscoreViewhelper : BaseViewhelper
    {
        private List<gov_testscore> lstTestscore;

        public List<gov_testscore> LstTestscore
        {
            get { return lstTestscore; }
            set { lstTestscore = value; }
        }
    }
}