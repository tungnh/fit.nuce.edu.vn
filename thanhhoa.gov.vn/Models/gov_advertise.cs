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
    
    public partial class gov_advertise
    {
        public int id { get; set; }
        public string title { get; set; }
        public string attach_file { get; set; }
        public string link { get; set; }
        public Nullable<bool> type_link { get; set; }
        public string department { get; set; }
        public Nullable<int> price { get; set; }
        public string note { get; set; }
        public Nullable<int> order_number { get; set; }
        public System.DateTime begin_date { get; set; }
        public System.DateTime end_date { get; set; }
        public Nullable<int> location { get; set; }
        public bool active_flg { get; set; }
        public string update_username { get; set; }
        public System.DateTime update_datetime { get; set; }
        public string entry_username { get; set; }
        public System.DateTime entry_datetime { get; set; }
    }
}
