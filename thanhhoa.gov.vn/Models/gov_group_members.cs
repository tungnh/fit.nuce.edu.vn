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
    
    public partial class gov_group_members
    {
        public int group_id { get; set; }
        public string username { get; set; }
        public string update_username { get; set; }
        public System.DateTime update_datetime { get; set; }
    
        public virtual gov_group gov_group { get; set; }
        public virtual gov_user gov_user { get; set; }
    }
}
