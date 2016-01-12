using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class DocumentViewhepler : BaseViewhelper
    {
        private int typeSearch;
        private List<gov_doc_draft> lstDocument;

        public List<gov_doc_draft> LstDocument
        {
            get { return lstDocument; }
            set { lstDocument = value; }
        }

        public int TypeSearch
        {
            get { return typeSearch; }
            set { typeSearch = value; }
        }
    }
}