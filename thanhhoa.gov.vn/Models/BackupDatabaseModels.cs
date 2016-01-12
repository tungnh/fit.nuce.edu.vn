using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class BackupDatabaseModels
    {
        private int id;
        private String file_name;
        private DateTime modify_datetime;
        private int file_size;

        public int File_size
        {
            get { return file_size; }
            set { file_size = value; }
        }

        public DateTime Modify_datetime
        {
            get { return modify_datetime; }
            set { modify_datetime = value; }
        }

        public String File_name
        {
            get { return file_name; }
            set { file_name = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}