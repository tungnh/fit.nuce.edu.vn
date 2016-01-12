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
    public class CourseController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }
            CourseViewhelper courseViewhelper = new CourseViewhelper();
            saveData(courseViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(CourseViewhelper courseViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(courseViewhelper);
            return View();
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(gov_course item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                item = _cnttDB.gov_course.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiKhoaHoc, Constant.THEM(Constant.ITEM_COURSE, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.REGIST_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.REGIST_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.REGIST_ERR;
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }

            ViewData["Course"] = _cnttDB.gov_course.Find(id);
            return View();
        }

        public ActionResult SaveEdit(gov_course item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }

            gov_course courseInfo = _cnttDB.gov_course.Find(item.id);
            if (courseInfo == null)
                return Redirect("/admin/error/error405");
            courseInfo.course_name = item.course_name;
            courseInfo.update_username = Session.getCurrentUser().username;
            courseInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaKhoaHoc, Constant.CHINHSUA(Constant.ITEM_COURSE, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.EDIT_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.EDIT_ERR;
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.Course))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_course courseInfo = _cnttDB.gov_course.Find(id);
                if (courseInfo == null)
                    return Redirect("/admin/error/error405");
                else
                {
                    _cnttDB.gov_course.Remove(courseInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaKhoaHoc, Constant.XOA(Constant.ITEM_COURSE, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                    else
                    {
                        TempData["err"] = Constant.DELETE_ERR;
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        public CourseViewhelper saveData(CourseViewhelper courseViewhelper)
        {
            List<gov_course> lstCourse = _cnttDB.gov_course.ToList();
            lstCourse = setSearchFilter(lstCourse, courseViewhelper);

            int totalCount = lstCourse.Count;
            courseViewhelper.TotalCount = totalCount;

            if (courseViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                courseViewhelper.TotalPage = totalPage;
                courseViewhelper.Page = pageTransition(courseViewhelper.Direction, courseViewhelper.Page, totalPage);
                courseViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, courseViewhelper.Page);
                courseViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, courseViewhelper.Page, courseViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (courseViewhelper.Page - 1) * take;
                courseViewhelper.LstCourse = lstCourse.OrderBy(d => d.course_name).Skip(skip).Take(take).ToList();
            }
            ViewData["courseViewhelper"] = courseViewhelper;
            return courseViewhelper;
        }

        public List<gov_course> setSearchFilter(List<gov_course> lstCourse, CourseViewhelper courseViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(courseViewhelper.KeySearch)) {
                lstCourse = lstCourse.Where(u => u.course_name == courseViewhelper.KeySearch).ToList();
            }
            return lstCourse;
        }

    }
}
