using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class DocumentKindViewhelper : BaseViewhelper
    {
        private List<DocumentKindModels> lstDocumentKind;

        public List<DocumentKindModels> LstDocumentKind
        {
            get { return lstDocumentKind; }
            set { lstDocumentKind = value; }
        }
    }
}