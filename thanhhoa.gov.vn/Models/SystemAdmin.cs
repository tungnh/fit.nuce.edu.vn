using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class SystemAdmin
    {
        public String adress { get; set; }
        public String tel { get; set; }
        public String fax { get; set; }
        public String email { get; set; }
        public String time_work { get; set; }
        public String maps_x { get; set; }
        public String maps_y { get; set; }
        public String record_per_page { get; set; }
        public String office_name { get; set; }
        public String banner_file_path { get; set; }
        public String logo_file_path { get; set; }
        public String slide_banner_file_path { get; set; }
        public String password { get; set; }
        public String begin_course { get; set; }
        public String end_course { get; set; }
        public String email_contact { get; set; }
    }
}