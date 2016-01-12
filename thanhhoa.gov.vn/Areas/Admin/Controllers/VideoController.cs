using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;
using System.Linq.Expressions;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class VideoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            VideoViewhelper VideoViewhelper = new VideoViewhelper();
            saveData(VideoViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(VideoViewhelper VideoViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(VideoViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            
            ViewData["videoInfo"] = _cnttDB.gov_video.Find(id);
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_video videoInfo)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            gov_video item = _cnttDB.gov_video.Find(videoInfo.id);
            item.title = videoInfo.title;
            item.description = videoInfo.description;
            item.avatar = videoInfo.avatar;
            item.attach_filepath = videoInfo.attach_filepath;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaVideo, Constant.CHINHSUA(Constant.ITEM_VIDEO, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.EDIT_ERR;
                }
            } catch(Exception ex){
                TempData["err"] = Constant.EDIT_ERR;
            }
            return Redirect("Index");
        }
        public VideoViewhelper saveData(VideoViewhelper videoViewhelper)
        {
            List<gov_video> lstVideo = _cnttDB.gov_video.ToList() ;
            lstVideo = setSearchFilter(lstVideo, videoViewhelper);

            int totalCount = lstVideo.Count;
            videoViewhelper.TotalCount = totalCount;

            if (videoViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                videoViewhelper.TotalPage = totalPage;
                videoViewhelper.Page = pageTransition(videoViewhelper.Direction, videoViewhelper.Page, totalPage);
                videoViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, videoViewhelper.Page);
                videoViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, videoViewhelper.Page, videoViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (videoViewhelper.Page - 1) * take;
                lstVideo = lstVideo.OrderByDescending(v => v.entry_datetime).Take(take).Skip(skip).ToList();
                videoViewhelper.LstVideo = lstVideo;
            }
            ViewData["VideoViewhelper"] = videoViewhelper;
            return videoViewhelper;
        }
        public List<gov_video> setSearchFilter(List<gov_video> lstVideo, VideoViewhelper videoViewhelper)
        {
            Expression<Func<gov_video, bool>> predicate = PredicateBuilder.False<gov_video>();
            if (!String.IsNullOrEmpty(videoViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.title.ToUpper().Contains(videoViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.description.ToUpper().Contains(videoViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.update_username.ToUpper().Contains(videoViewhelper.KeySearch.ToUpper()));
                lstVideo = lstVideo.Where(predicate.Compile()).ToList();
            }
            return lstVideo;
        }

        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            gov_video videoInfo = _cnttDB.gov_video.Find(id);
            if (videoInfo != null)
            {
                _cnttDB.gov_video.Remove(videoInfo);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.xoaVideo, Constant.XOA(Constant.ITEM_VIDEO, Constant.ID, id.ToString()));
                    TempData["message"] = Constant.DELETE_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.DELETE_ERR;
                }
            }
            else
            {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }
        
        public ActionResult Regist(){
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(gov_video item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_MEDIA, Session.getCurrentUser().username, TypeAudit.Video))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                item = _cnttDB.gov_video.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiVideo, Constant.THEM(Constant.ITEM_VIDEO, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.REGIST_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.REGIST_ERR;
                }
            }
            catch (Exception ex) {
                TempData["err"] = Constant.REGIST_ERR;
            }
            return Redirect("Index");
        }
    }
}
