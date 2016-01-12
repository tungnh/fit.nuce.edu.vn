using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.SchedulerTask
{
    public class BackupJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var fileFolder = Constant.BACKUP_FOLDER_PATH;
            var fileDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + Constant.BACKUP_FOLDER_PATH;
            if (!System.IO.Directory.Exists(fileDir))
                System.IO.Directory.CreateDirectory(fileDir);

            String filePath = fileDir + "\\" + "TAKS_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak";
            using (var db = new CNTTDHXDEntities())
            {
                var cmd = String.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for {0} database';"
                    , "CNTTDHXD", filePath);
                try
                {
                    int rs = db.Database.ExecuteSqlCommand(cmd);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //nothing
                }
            }
        }
    }
}