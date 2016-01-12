using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class KeyBandModels
    {
        private int id;
        private String key;
        private String entryUsername;

        public String EntryUsername
        {
            get { return entryUsername; }
            set { entryUsername = value; }
        }
        private DateTime entryDatetime;

        public DateTime EntryDatetime
        {
            get { return entryDatetime; }
            set { entryDatetime = value; }
        }

        public String Key
        {
            get { return key; }
            set { key = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}