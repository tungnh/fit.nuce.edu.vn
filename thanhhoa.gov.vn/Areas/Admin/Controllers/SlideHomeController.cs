using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class SlideHomeController : BaseController
    {
        //
        // GET: /Admin/SlideHome/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            SlideHomeViewhelper slideHomeViewhelper = new SlideHomeViewhelper();
            saveData(slideHomeViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(SlideHomeViewhelper slideHomeViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(slideHomeViewhelper);
            return View();
        }

        public SlideHomeViewhelper saveData(SlideHomeViewhelper slideHomeViewhelper)
        {
            List<gov_slide_home> lstSlideHome = _cnttDB.gov_slide_home.ToList();
            lstSlideHome = setSearchFilter(lstSlideHome, slideHomeViewhelper);
            int totalCount = lstSlideHome.Count;
            slideHomeViewhelper.TotalCount = totalCount;

            if (slideHomeViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                slideHomeViewhelper.TotalPage = totalPage;
                slideHomeViewhelper.Page = pageTransition(slideHomeViewhelper.Direction, slideHomeViewhelper.Page, totalPage);
                slideHomeViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, slideHomeViewhelper.Page);
                slideHomeViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, slideHomeViewhelper.Page, slideHomeViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (slideHomeViewhelper.Page - 1) * take;
                slideHomeViewhelper.LstSlideHome = lstSlideHome.OrderBy(u => u.order_number).Skip(skip).Take(take).ToList();
            }
            ViewData["slideHomeViewhelper"] = slideHomeViewhelper;
            return slideHomeViewhelper;
        }

        public List<gov_slide_home> setSearchFilter(List<gov_slide_home> lstSlideHome, SlideHomeViewhelper slideHomeViewhelper)
        {
            Expression<Func<gov_slide_home, bool>> predicate = PredicateBuilder.False<gov_slide_home>();
            if (!String.IsNullOrWhiteSpace(slideHomeViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.link != null && d.link.ToUpper().Contains(slideHomeViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.description != null && d.description.ToUpper().Contains(slideHomeViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.update_username != null && d.update_username.ToUpper().Contains(slideHomeViewhelper.KeySearch.ToUpper()));
                lstSlideHome = lstSlideHome.Where(predicate.Compile()).ToList();
            }
            return lstSlideHome;
        }

        [HttpGet]
        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(gov_slide_home item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_datetime = DateTime.Now;
            item.entry_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            try
            {
                item = _cnttDB.gov_slide_home.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiSlide, Constant.THEM(Constant.ITEM_SLIDE, Constant.ID, item.id.ToString()));
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
        public ActionResult Edit(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["slideHomeInfo"] = _cnttDB.gov_slide_home.Find(id);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_slide_home item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            gov_slide_home slideHomeInfo = _cnttDB.gov_slide_home.Find(item.id);
            if (slideHomeInfo != null) {
                slideHomeInfo.link = item.link;
                slideHomeInfo.order_number = item.order_number;
                slideHomeInfo.type = item.type;
                slideHomeInfo.attach_file_path = item.attach_file_path;
                slideHomeInfo.description = item.description;
                slideHomeInfo.update_datetime = DateTime.Now;
                slideHomeInfo.update_username = Session.getCurrentUser().username;
                try
                {
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.chinhSuaSlide, Constant.CHINHSUA(Constant.ITEM_SLIDE, Constant.ID, item.id.ToString()));
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
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.SlideTrangChu))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_slide_home slideHomeInnfo = _cnttDB.gov_slide_home.Find(id);
                if (slideHomeInnfo == null)
                    return Redirect("/admin/error/error405");
                else {
                    _cnttDB.gov_slide_home.Remove(slideHomeInnfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaSlide, Constant.XOA(Constant.ITEM_SLIDE, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                }
                
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }
    }
}
