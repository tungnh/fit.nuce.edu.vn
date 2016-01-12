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
    public class GalleryController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            GalleryViewhelper galleryViewhelper = new GalleryViewhelper();
            saveData(galleryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(GalleryViewhelper galleryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(galleryViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(gov_gallery item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item.total_view = 0;
            try
            {
                item = _cnttDB.gov_gallery.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiAnh, Constant.THEM(Constant.ITEM_HINHANH, Constant.ID, item.id.ToString()));
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
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["galleryInfo"] = _cnttDB.gov_gallery.Find(id);
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_gallery item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            gov_gallery galleryInfo = _cnttDB.gov_gallery.Find(item.id);
            if (galleryInfo == null)
                return Redirect("/admin/error/error405");
            galleryInfo.title = item.title;
            galleryInfo.attach_filepath = item.attach_filepath;
            galleryInfo.update_username = Session.getCurrentUser().username;
            galleryInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaAnh, Constant.CHINHSUA(Constant.ITEM_HINHANH, Constant.ID, item.id.ToString()));
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
        public GalleryViewhelper saveData(GalleryViewhelper galleryViewhelper)
        {
            List<gov_gallery> lstGallery = _cnttDB.gov_gallery.ToList();
            lstGallery = setSearchFilter(lstGallery, galleryViewhelper);

            int totalCount = lstGallery.Count;
            galleryViewhelper.TotalCount = totalCount;

            if (galleryViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                galleryViewhelper.TotalPage = totalPage;
                galleryViewhelper.Page = pageTransition(galleryViewhelper.Direction, galleryViewhelper.Page, totalPage);
                galleryViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, galleryViewhelper.Page);
                galleryViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, galleryViewhelper.Page, galleryViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (galleryViewhelper.Page - 1) * take;
                lstGallery = lstGallery.OrderByDescending(g => g.entry_datetime).Skip(skip).Take(take).ToList();
                galleryViewhelper.LstGallery = lstGallery;
            }
            ViewData["galleryViewhelper"] = galleryViewhelper;
            return galleryViewhelper;
        }

        public ActionResult Delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Gallery))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_gallery galleryInfo = _cnttDB.gov_gallery.Find(id);
                if (galleryInfo == null)
                    return Redirect("/admin/error/error405");
                _cnttDB.gov_gallery.Remove(galleryInfo);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.xoaAnh, Constant.XOA(Constant.ITEM_HINHANH, Constant.ID, id.ToString()));
                    TempData["message"] = Constant.DELETE_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.DELETE_SUCCESSFULL;
                }
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        public List<gov_gallery> setSearchFilter(List<gov_gallery> lstGallery, GalleryViewhelper galleryViewhelper)
        {
            Expression<Func<gov_gallery, bool>> predicate = PredicateBuilder.False<gov_gallery>();
            if (!String.IsNullOrEmpty(galleryViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.title != null && d.title.ToUpper().Contains(galleryViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.update_username != null && d.update_username.ToUpper().Contains(galleryViewhelper.KeySearch.ToUpper()));
                lstGallery = lstGallery.Where(predicate.Compile()).ToList();
            }
            return lstGallery;
        }

    }
}
