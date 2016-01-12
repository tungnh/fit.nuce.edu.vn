using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Viewhelper
{
    public class BackupDatabaseViewhelper : BaseViewhelper
    {
        private List<FileInfo> lstBackupDatabase;

        public List<FileInfo> LstBackupDatabase
        {
            get { return lstBackupDatabase; }
            set { lstBackupDatabase = value; }
        }
    }
}