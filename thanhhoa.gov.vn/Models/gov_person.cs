//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace thanhhoa.gov.vn.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class gov_person
    {
        public int id { get; set; }
        public string full_name { get; set; }
        public string lop { get; set; }
        public int specialized_id { get; set; }
        public int course_id { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public bool active_flg { get; set; }
        public string department { get; set; }
        public string description { get; set; }
        public int news_id { get; set; }
        public System.DateTime update_datetime { get; set; }
        public bool show_email { get; set; }
        public bool show_tel { get; set; }
        public bool show_address { get; set; }
        public bool show_department { get; set; }
        public bool show_shared { get; set; }
        public string code { get; set; }
        public System.DateTime entry_datetime { get; set; }
    
        public virtual gov_course gov_course { get; set; }
        public virtual gov_specialized gov_specialized { get; set; }
    }
}
