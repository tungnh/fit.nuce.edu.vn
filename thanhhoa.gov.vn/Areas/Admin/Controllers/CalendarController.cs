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
    public class CalendarController : BaseController
    {
        //
        // GET: /Admin/Calendar/

        /*[HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            CalendarViewhelper CalendarViewhelper = new CalendarViewhelper();
            saveData(CalendarViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(CalendarViewhelper CalendarViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(CalendarViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            CalendarServices calendarServices = new CalendarServices();
            calendarServices.Id = id.ToString();
            List<CalendarModels> lstCalendar = calendarServices.select(-1, -1);
            CalendarModels calendarInfo = null;
            if (lstCalendar != null && lstCalendar.Count > 0) {
                calendarInfo = lstCalendar.First();
                ViewData["calendarInfo"] = calendarInfo;
            }
            calendarServices = new CalendarServices();
            List<WorktimeModels> lstTime = calendarServices.selectTime();
            ViewData["lstTime"] = lstTime;
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(CalendarModels item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            CalendarServices calendarServices = new CalendarServices();
            item.UpdateUsername = Session.getCurrentUser().username;
            item.UpdateDatetime = DateTime.Now;
            int rs = calendarServices.Update(item);
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
            CalendarServices services = new CalendarServices();
            List<WorktimeModels> lstTime = services.selectTime();
            ViewData["lstTime"] = lstTime;
            return View();
        }

        public ActionResult SaveRegist(CalendarModels item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            CalendarServices calendarServices = new CalendarServices();
            item.UpdateUsername = Session.getCurrentUser().username;
            item.UpdateDatetime = DateTime.Now;
            int rs = calendarServices.insert(item);
            return RedirectToAction("Regist");
        }

        public ActionResult Delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.LichCongTac))
            {
                return Redirect("/admin/error/error403");
            }
            CalendarServices calendarServices = new CalendarServices();
            int rs = calendarServices.delete(id);
            return Redirect("Index");
        }

        public CalendarViewhelper saveData(CalendarViewhelper calendarViewhelper)
        {
            CalendarServices calendarServices = new CalendarServices();
            setSearchFilter(calendarServices, calendarViewhelper);

            int totalCount = calendarServices.totalCount();
            calendarViewhelper.TotalCount = totalCount;

            if (calendarViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                calendarViewhelper.TotalPage = totalPage;
                calendarViewhelper.Page = pageTransition(calendarViewhelper.Direction, calendarViewhelper.Page, totalPage);
                calendarViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, calendarViewhelper.Page);
                calendarViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, calendarViewhelper.Page, calendarViewhelper.FirstPage);
                calendarServices.addOrderBy("date", "ASC");
                List<CalendarModels> lstCalendar = calendarServices.select(calendarViewhelper.Page, Constant.limit);
                calendarViewhelper.LstCalendar = lstCalendar;
            }
            ViewData["calendarViewhelper"] = calendarViewhelper;
            return calendarViewhelper;
        }

        public void setSearchFilter(CalendarServices calendarServices, CalendarViewhelper calendarViewhelper)
        {
            if (!String.IsNullOrEmpty(calendarViewhelper.KeySearch))
                calendarServices.KeySearch = calendarViewhelper.KeySearch;
        }
        */
    }
}
