using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Utility
{
    public class TypeAction
    {
        private int id;
        private String typeName;
        public TypeAction(int _id, String _typeName) {
            this.id = _id;
            this.typeName = _typeName;
        }

        public String TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}