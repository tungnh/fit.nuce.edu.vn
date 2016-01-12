using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class BackupDatabaseController : BaseController
    {
        //
        // GET: /Admin/BackupDatabase/
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.BackupDatabase))
            {
                return Redirect("/admin/error/error403");
            }
            BackupDatabaseViewhelper backupDatabaseViewhelper = new BackupDatabaseViewhelper();
            saveData(backupDatabaseViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(BackupDatabaseViewhelper backupDatabaseViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.BackupDatabase))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(backupDatabaseViewhelper);
            return View();
        }

        public BackupDatabaseViewhelper saveData(BackupDatabaseViewhelper backupDatabaseViewhelper)
        {
            //Get file in base dir
            var fileDir = System.AppDomain.CurrentDomain.BaseDirectory + Constant.BACKUP_FOLDER_PATH;
            DirectoryInfo directory = new DirectoryInfo(fileDir);
            if (!System.IO.Directory.Exists(fileDir))
                System.IO.Directory.CreateDirectory(fileDir);
            List<FileInfo> lstBackupDatabase= directory.GetFiles().ToList();

            //lstUser = setSearchFilter(lstUser, backupDatabaseViewhelper);
            int totalCount = lstBackupDatabase.Count;
            backupDatabaseViewhelper.TotalCount = totalCount;

            if (backupDatabaseViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                backupDatabaseViewhelper.TotalPage = totalPage;
                backupDatabaseViewhelper.Page = pageTransition(backupDatabaseViewhelper.Direction, backupDatabaseViewhelper.Page, totalPage);
                backupDatabaseViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, backupDatabaseViewhelper.Page);
                backupDatabaseViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, backupDatabaseViewhelper.Page, backupDatabaseViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (backupDatabaseViewhelper.Page - 1) * take;
                backupDatabaseViewhelper.LstBackupDatabase = lstBackupDatabase.OrderByDescending(f => f.Name).Skip(skip).Take(take).ToList();
            }
            ViewData["backupDatabaseViewhelper"] = backupDatabaseViewhelper;
            return backupDatabaseViewhelper;
        }

        [HttpGet]
        public ActionResult Backup()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.BackupDatabase))
            {
                return Redirect("/admin/error/error403");
            }
            //var fileFolder = Constant.BACKUP_FOLDER_PATH;
            var fileDir = System.AppDomain.CurrentDomain.BaseDirectory + Constant.BACKUP_FOLDER_PATH;
            if (!System.IO.Directory.Exists(fileDir))
                System.IO.Directory.CreateDirectory(fileDir);

            string dbPath = fileDir + "\\" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak";
            using (var db = new CNTTDHXDEntities())
            {
                var cmd = String.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for {0} database';"
                    , "[fit.nuce.edu.vn]", dbPath);
                try
                {
                    int rs = db.Database.ExecuteSqlCommand(cmd);
                    TempData["message"] = "Backup dữ liệu thành công!";
                }
                catch (Exception ex)
                {
                    throw ex;
                    TempData["err"] = "Backup dữ liệu thất bại!";
                }
            }
            return Redirect("Index");
        }

        public ActionResult Download(String file_name)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.BackupDatabase))
            {
                return Redirect("/admin/error/error403");
            }
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + Constant.BACKUP_FOLDER_PATH + "\\" + file_name;
            if (!System.IO.Directory.Exists(filePath))
                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
            else
                return null;
        }

        public ActionResult Remove(String file_name)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.BackupDatabase))
            {
                return Redirect("/admin/error/error403");
            }
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + Constant.BACKUP_FOLDER_PATH + "\\" + file_name;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                TempData["message"] = "Xóa dữ liệu thành công!";
            }
            else
            {
                TempData["err"] = "Xóa dữ liệu thất bại. Dữ liệu không tồn tại!";
            }

            return Redirect("Index");
        }
    }
}
