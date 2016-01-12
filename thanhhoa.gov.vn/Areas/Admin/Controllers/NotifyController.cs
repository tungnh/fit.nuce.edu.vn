using System;
using System.Collections.Generic;
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
    public class NotifyController : BaseController
    {
        //
        // GET: /Admin/Notify/

        /*[HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            NotifyViewhelper notifyViewhelper = new NotifyViewhelper();
            saveData(notifyViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(NotifyViewhelper notifyViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(notifyViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            NotifyServices notifyServices = new NotifyServices();
            notifyServices.Id = id.ToString();
            List<NotifyModels> lstNotify = notifyServices.select(-1, -1);
            NotifyModels NotifyInfo = null;
            if (lstNotify != null && lstNotify.Count > 0)
            {
                NotifyInfo = lstNotify.First();
                ViewData["notifyInfo"] = NotifyInfo;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(NotifyModels item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            NotifyServices notifyServices = new NotifyServices();
            item.UpdateUsername = "admin";
            item.UpdateDatetime = DateTime.Now;
            int rs = notifyServices.update(item);
            return Redirect("Index");
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(NotifyModels item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            NotifyServices notifyServices = new NotifyServices();
            item.UpdateUsername = "admin";
            item.UpdateDatetime = DateTime.Now;
            int rs = notifyServices.insert(item);
            return Redirect("Regist");
        }

        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            NotifyServices notifyServices = new NotifyServices();
            int rs = notifyServices.delete(id);
            return Redirect("Index");
        }

        public NotifyViewhelper saveData(NotifyViewhelper notifyViewhelper)
        {
            NotifyServices notifyServices = new NotifyServices();
            setSearchFilter(notifyServices, notifyViewhelper);

            int totalCount = notifyServices.totalCount();
            notifyViewhelper.TotalCount = totalCount;

            if (notifyViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                notifyViewhelper.TotalPage = totalPage;
                notifyViewhelper.Page = pageTransition(notifyViewhelper.Direction, notifyViewhelper.Page, totalPage);
                notifyViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, notifyViewhelper.Page);
                notifyViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, notifyViewhelper.Page, notifyViewhelper.FirstPage);
                notifyServices.addOrderBy("update_datetime", "ASC");
                List<NotifyModels> lstNotify = notifyServices.select(notifyViewhelper.Page, Constant.limit);
                notifyViewhelper.LstNotify = lstNotify;
            }
            ViewData["notifyViewhelper"] = notifyViewhelper;
            return notifyViewhelper;
        }

        public void setSearchFilter(NotifyServices notifyServices, NotifyViewhelper notifyViewhelper)
        {
            if (!String.IsNullOrEmpty(notifyViewhelper.KeySearch))
                notifyServices.KeySearch = notifyViewhelper.KeySearch;
        }*/

    }
}
