using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private string fileSaveFolder = "/Upload/Avatar";
        //
        // GET: /Admin/Home/
        [HttpGet]
        public Boolean ResetMessageSystem()
        {
            Boolean check = true;
            try{
                var userInfo = Session.getCurrentUser();
                if (userInfo != null)
                {
                    var lstMessage = _cnttDB.gov_message_system.Where(m => m.username.Equals(userInfo.username) && m.status == false).ToList();
                    foreach(var item in lstMessage){
                        item.status = true;
                        int rs = _cnttDB.SaveChanges();
                    }
                }
            } catch(Exception ex){
                check = false;
            }
            return check;
        }

        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            ViewData["userInfo"] = Session.getCurrentUser();
            return View();
        }

        [HttpGet]
        public ActionResult account() {
            return View();
        }

        [HttpPost]
        public ActionResult changeProfile(gov_user item) {
            gov_user userInfo = _cnttDB.gov_user.Find(item.username);
            if (Request.Files.Count > 0)
            {
                String dirOld = userInfo.avatar;
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                    var fileFolder = fileSaveFolder;
                    var fileDir = Server.MapPath("/") + fileFolder;
                    if (!System.IO.Directory.Exists(fileDir))
                        System.IO.Directory.CreateDirectory(fileDir);
                    var filePath = fileFolder + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                    System.IO.File.WriteAllBytes(Server.MapPath("/") + filePath, bytes);
                    userInfo.avatar = filePath.Replace("\\", "/");
                    if (System.IO.File.Exists(Server.MapPath("/") + fileFolder + dirOld))
                    {
                        System.IO.File.Delete(Server.MapPath("/") + fileFolder + dirOld);
                    }
                }
            }
            
            userInfo.family_name = item.family_name;
            userInfo.first_name = item.first_name;
            userInfo.birth_day = item.birth_day;
            userInfo.sex = item.sex;
            userInfo.address = item.address;
            userInfo.email = item.email;
            userInfo.phone = item.phone;
            userInfo.mobile = item.mobile;
            userInfo.is_shared = item.is_shared;
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                Session.SetCurrentUser(userInfo);
            }
            ViewData["message"] = "Cập nhật thông tin thành công!";
            ViewData["userInfo"] = Session.getCurrentUser();
            return View("Index");
        }

        [HttpGet]
        public ActionResult changeProfile()
        {
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult ChangePassword(String passwordOld, String passwordNew, String confirmPasswordNew, String username)
        {
            gov_user userInfo = _cnttDB.gov_user.Find(username);
            if (FormsAuthentication.HashPasswordForStoringInConfigFile(passwordOld, "MD5").Equals(userInfo.password))
            {
                if (passwordNew.Equals(confirmPasswordNew))
                {
                    userInfo.password = FormsAuthentication.HashPasswordForStoringInConfigFile(passwordNew, "MD5");
                    int rs = _cnttDB.SaveChanges();
                    ViewData["message"] = "Cập nhật thông tin thành công!";
                }
                else {
                    ViewData["error"] = "Xác nhận mật khẩu không chính xác. Vui lòng thử lại";
                }
                
            }
            else {
                ViewData["error"] = "Mật khẩu nhập vào không chính xác. Vui lòng thử lại";
            }
            ViewData["userInfo"] = Session.getCurrentUser();
            return View("Index");
        }
    }
}
