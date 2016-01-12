using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {
        //
        // GET: /Admin/System/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CauHinhHeThong))
            {
                return Redirect("/admin/error/error403");
            }
            SystemAdmin sysAdmin = new SystemAdmin();
            List<gov_system_config> lstSystem = _cnttDB.gov_system_config.ToList();
            foreach (var item in lstSystem) {
                if (item.key_config.Equals(Constant.CONFIG_KEY_ADDRESS))
                    sysAdmin.adress = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_BANNER_FILE_PATH))
                    sysAdmin.banner_file_path = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_EMAIL))
                    sysAdmin.email = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_FAX))
                    sysAdmin.fax = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_LOGO_FILE_PATH))
                    sysAdmin.logo_file_path= item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_MAPSX))
                    sysAdmin.maps_x = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_MAPSY))
                    sysAdmin.maps_y = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_NAME))
                    sysAdmin.office_name = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_SLIDE_BANNER_FILE_PATH))
                    sysAdmin.slide_banner_file_path = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_TEL))
                    sysAdmin.tel = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_TIME_WORK))
                    sysAdmin.time_work = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_PASS))
                    sysAdmin.password = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_BEGIN_COURSE))
                    sysAdmin.begin_course = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_END_COURSE))
                    sysAdmin.end_course = item.value_config;
                if (item.key_config.Equals(Constant.CONFIG_KEY_EMAIL_CONTACT))
                    sysAdmin.email_contact = item.value_config;
            }
            ViewData["sysAdmin"] = sysAdmin;
            return View();
        }

        [HttpPost]
        public ActionResult Index(SystemAdmin item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CauHinhHeThong))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_system_config sysConfig = new gov_system_config();
                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_NAME));
                sysConfig.value_config = item.office_name;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_ADDRESS));
                sysConfig.value_config = item.adress;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_EMAIL));
                sysConfig.value_config = item.email;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_PASS));
                sysConfig.value_config = item.password;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_TEL));
                sysConfig.value_config = item.tel;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_FAX));
                sysConfig.value_config = item.fax;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_TIME_WORK));
                sysConfig.value_config = item.time_work;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_MAPSX));
                sysConfig.value_config = item.maps_x;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_MAPSY));
                sysConfig.value_config = item.maps_y;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_LOGO_FILE_PATH));
                sysConfig.value_config = item.logo_file_path;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_BANNER_FILE_PATH));
                sysConfig.value_config = item.banner_file_path;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_SLIDE_BANNER_FILE_PATH));
                sysConfig.value_config = item.slide_banner_file_path;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_BEGIN_COURSE));
                sysConfig.value_config = item.begin_course;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_END_COURSE));
                sysConfig.value_config = item.end_course;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                sysConfig = _cnttDB.gov_system_config.Single(s => s.key_config.Equals(Constant.CONFIG_KEY_EMAIL_CONTACT));
                sysConfig.value_config = item.email_contact;
                sysConfig.update_datetime = DateTime.Now;
                sysConfig.update_username = Session.getCurrentUser().username;
                _cnttDB.SaveChanges();

                insertHistory(AccessType.cauHinhHeThong, "Cấu hình thông tin hệ thống");
                TempData["message"] = Constant.SYSTEM_CONFIG_SUCCESSFULL;
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.SYSTEM_CONFIG_SUCCESSFULL;
            }
            return Redirect("Index");
        }
    }
}
