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
    public class AlbumController : BaseController
    {
        //
        // GET: /Admin/Album/
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            AlbumViewhelper albumViewhelper = new AlbumViewhelper();
            saveData(albumViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(AlbumViewhelper albumViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(albumViewhelper);
            return View();
        }
        public ActionResult regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(gov_album item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            item.total_view = 0;
            item.update_username = "admin";
            item.update_datetime = DateTime.Now;

            item = _cnttDB.gov_album.Add(item);
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiAlbum, Constant.THEM(Constant.ITEM_ALBUM, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.REGIST_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.REGIST_ERR;
                }
            } catch(Exception ex){
                TempData["err"] = Constant.REGIST_ERR;
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            gov_album albumInfo = _cnttDB.gov_album.Find(id);
            if (albumInfo != null)
            {
                try
                {
                    _cnttDB.gov_album.Remove(albumInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaAlbum, Constant.XOA(Constant.ITEM_ALBUM, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                    else
                    {
                        TempData["err"] = Constant.DELETE_ERR;
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/error/error404");
                }
            }
            else
            {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }
        
        [HttpGet]
        public ActionResult AddImages(int albumId)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            GalleryViewhelper galleryViewhelper = new GalleryViewhelper();
            galleryViewhelper.AlbumId = albumId;
            saveAddGallery(galleryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult AddImages(GalleryViewhelper galleryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            saveAddGallery(galleryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult SaveAddImages(GalleryViewhelper galleryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            String content = "";
            foreach (int galleryId in galleryViewhelper.CheckId)
            {
                gov_album_gallery item = new gov_album_gallery();
                item.album_id = galleryViewhelper.AlbumId;
                item.update_username = Session.getCurrentUser().username;
                item.update_datetime = DateTime.Now;
                item.gallery_id = galleryId;
                _cnttDB.gov_album_gallery.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0) {
                    content += Constant.THEM_IN(Constant.ITEM_HINHANH, Constant.ID, galleryId.ToString(), Constant.ITEM_ALBUM, Constant.ID, galleryViewhelper.AlbumId.ToString());
                    content += ".<br/>";
                }
            }
            if (!content.Equals(""))
            {
                insertHistory(AccessType.themAnhVaoAlbum, content);
                TempData["message"] = Constant.REGIST_SUCCESSFULL;
            }
            return Redirect("listimages?albumId=" + galleryViewhelper.AlbumId.ToString());
        }

        [HttpGet]
        public ActionResult ListImages(int albumId) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            GalleryViewhelper galleryViewhelper = new GalleryViewhelper();
            galleryViewhelper.AlbumId = albumId;
            saveListImages(galleryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult ListImages(GalleryViewhelper galleryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            saveListImages(galleryViewhelper);
            return View();
          }

        public ActionResult DeleteImages(GalleryViewhelper galleryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            String content = "";
            foreach (int id in galleryViewhelper.CheckId)
            {
                gov_album_gallery item = _cnttDB.gov_album_gallery.Single(s => s.album_id == galleryViewhelper.AlbumId && s.gallery_id == id);
                _cnttDB.gov_album_gallery.Remove(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    content += Constant.XOA_IN(Constant.ITEM_HINHANH, Constant.ID, id.ToString(), Constant.ITEM_ALBUM, Constant.ID, galleryViewhelper.AlbumId.ToString());
                    content += ".<br/>";
                }
            }
            if (!content.Equals(""))
            {
                insertHistory(AccessType.XoaAnhKhoiAlbum, content);
                TempData["message"] = Constant.DELETE_SUCCESSFULL;
            }
            return Redirect("ListImages?albumid=" + galleryViewhelper.AlbumId.ToString());
        }

        [HttpGet]
        public ActionResult Edit(int albumId) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["albumInfo"] = _cnttDB.gov_album.Find(albumId);
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_album item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Album))
            {
                return Redirect("/admin/error/error403");
            }
            gov_album albumInfo = _cnttDB.gov_album.Find(item.id);
            if (albumInfo == null)
                return Redirect("/admin/error/error405");
            albumInfo.album_title = item.album_title;
            albumInfo.description = item.description;
            albumInfo.avatar = item.avatar;
            albumInfo.order_number = item.order_number;
            albumInfo.update_username = Session.getCurrentUser().username;
            albumInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaAlbum, Constant.CHINHSUA(Constant.ITEM_ALBUM, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                }
                else {
                    TempData["err"] = Constant.EDIT_ERR;
                }
            }
            catch (Exception ex)
            {

            }
            return Redirect("Index");
        }

        public AlbumViewhelper saveData(AlbumViewhelper albumViewhelper)
        {
            List<gov_album> lstAlbum = _cnttDB.gov_album.ToList();
            lstAlbum = setSearchFilter(lstAlbum, albumViewhelper);

            int totalCount = lstAlbum.Count;
            albumViewhelper.TotalCount = totalCount;

            if (albumViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                albumViewhelper.TotalPage = totalPage;
                albumViewhelper.Page = pageTransition(albumViewhelper.Direction, albumViewhelper.Page, totalPage);
                albumViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, albumViewhelper.Page);
                albumViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, albumViewhelper.Page, albumViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (albumViewhelper.Page - 1) * take;
                lstAlbum = lstAlbum.OrderByDescending(a => a.order_number).Skip(skip).Take(take).ToList();
                albumViewhelper.LstAlbum = lstAlbum;
            }
            ViewData["albumViewhelper"] = albumViewhelper;
            return albumViewhelper;
        }
        public List<gov_album> setSearchFilter(List<gov_album> lstAlbum, AlbumViewhelper albumViewhelper)
        {
            Expression<Func<gov_album, bool>> predicate = PredicateBuilder.False<gov_album>();
            if (!String.IsNullOrWhiteSpace(albumViewhelper.KeySearch))
            {
                predicate = predicate.Or(a => a.album_title != null && a.album_title.ToUpper().Contains(albumViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(a => a.description != null && a.description.ToUpper().Contains(albumViewhelper.KeySearch.ToUpper()));
                lstAlbum = lstAlbum.Where(predicate.Compile()).ToList();
            }
            return lstAlbum;
        }
        public void saveAddGallery(GalleryViewhelper galleryViewhelper)
        {
            var lstGallery = (from g in _cnttDB.gov_gallery
                        where !_cnttDB.gov_album_gallery.Any(p => p.gallery_id == g.id && p.album_id == galleryViewhelper.AlbumId)
                        select g).ToList();
            int totalCount = lstGallery.Count;
            galleryViewhelper.TotalCount = totalCount;
            //setSearchFilter1(galleryServices, galleryViewhelper);

            if (galleryViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                galleryViewhelper.TotalPage = totalPage;
                galleryViewhelper.Page = pageTransition(galleryViewhelper.Direction, galleryViewhelper.Page, totalPage);
                galleryViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, galleryViewhelper.Page);
                galleryViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, galleryViewhelper.Page, galleryViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (galleryViewhelper.Page - 1) * take;
                lstGallery = lstGallery.OrderByDescending(s => s.entry_datetime).Skip(skip).Take(take).ToList();
                galleryViewhelper.LstGallery = lstGallery;
            }
            ViewData["galleryViewhelper"] = galleryViewhelper;
        }
        public void setSearchFilter1(GalleryServices galleryServices, GalleryViewhelper galleryViewhelper)
        {
            if (galleryViewhelper.Filter)
                galleryServices.IdNotInAlbum = "TRUE";
        }

        public void saveListImages(GalleryViewhelper galleryViewhelper)
        {
            List<gov_album_gallery> lstGallery = _cnttDB.gov_album_gallery.Where(g => g.album_id == galleryViewhelper.AlbumId).ToList();
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
                lstGallery = lstGallery.OrderByDescending(s => s.update_datetime).Skip(skip).Take(take).ToList();
                galleryViewhelper.LstAlbumGallery = lstGallery;
            }
            ViewData["galleryViewhelper"] = galleryViewhelper;
        }
    }
}
