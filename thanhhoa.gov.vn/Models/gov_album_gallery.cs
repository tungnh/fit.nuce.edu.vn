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
    
    public partial class gov_album_gallery
    {
        public int album_id { get; set; }
        public int gallery_id { get; set; }
        public string update_username { get; set; }
        public System.DateTime update_datetime { get; set; }
    
        public virtual gov_gallery gov_gallery { get; set; }
        public virtual gov_album gov_album { get; set; }
    }
}
