using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("new", Precedence = 1)]
    [RoutePrefix("kenh-tin", Precedence = 2)]
    public class NewController : BaseController
    {
        //
        // GET: /New/
        [GET("{menu}/{name}-{id:int}")]
        public ActionResult Index(int id)
        {
            gov_news newInfo = new gov_news();
            newInfo = _cnttDB.gov_news.Where(n => n.active_flg == true && n.id == id).FirstOrDefault();
            ViewData["new"] = newInfo;
            if(newInfo != null){
                ViewData["listNew"] = _cnttDB.gov_news.Where(n => n.active_flg == true && n.menu_id == newInfo.menu_id && n.id != id).OrderBy(n => n.entry_datetime).Skip(0).Take(7).ToList();
            }
            ViewData["lstComment"] = _cnttDB.gov_comments.Where(c => c.active_flg == true && c.news_id == id).OrderBy(c => c.entry_datetime).ToList();
            return View();
        }

        public ActionResult Index2(int id)
        {
            //View new
            NewServices newServices = new NewServices();
            newServices.ActiveFlg = Boolean.TrueString;
            newServices.IdFilter = id.ToString();
            NewModels newModel = newServices.select(-1, -1).FirstOrDefault();
            ViewData["new"] = newModel;
            if (newModel != null)
            {
                newServices = new NewServices();
                newModel.TotalView = newModel.TotalView + 1;
                newServices.update(newModel);

                newServices = new NewServices();
                newServices.MenuId = newModel.MenuId.ToString();
                newServices.NewNotIn = newModel.Id.ToString();
                ViewData["listNew"] = newServices.select(1, 7);
            }


            newServices = new NewServices();
            newServices.TypeNew = "3";
            newServices.ActiveFlg = Boolean.TrueString;
            newServices.addOrderBy("entry_datetime", "DESC");
            List<NewModels> lstNewTieuDiem = newServices.select(1, 7);
            ViewData["lstNewTieuDiem"] = lstNewTieuDiem;

            newServices = new NewServices();
            newServices.TypeNew = "2";
            newServices.ActiveFlg = Boolean.TrueString;
            newServices.addOrderBy("entry_datetime", "DESC");
            List<NewModels> lstNewNoiBat = newServices.select(1, 7);
            ViewData["lstNewNoiBat"] = lstNewNoiBat;



            newServices = new NewServices();
            newServices.ActiveFlg = Boolean.TrueString;
            newServices.addOrderBy("total_view", "DESC");
            List<NewModels> lstNewXemNhieu = newServices.select(1, 15);
            ViewData["lstNewXemNhieu"] = lstNewXemNhieu;
            //View Comment
            CommentServices commentSerives = new CommentServices();
            commentSerives.NewsId = id.ToString();
            commentSerives.ActiveFlg = "TRUE";
            List<CommentModels> lstComment = commentSerives.select(-1, -1);
            ViewData["lstComment"] = lstComment;
            return View();
        }

        //public ActionResult comment(CommentModels commentModels)
        //{
        //    CommentServices commentServices = new CommentServices();
        //    commentModels.ActiveFlg = true;
        //    commentModels.EntryDatetime = DateTime.Now;
        //    commentServices.insert(commentModels);
        //    return Redirect("Index?id=" + commentModels.NewsId);
        //}

        public ActionResult loadComment(int id)
        {
            //View Comment
            ViewData["lstComment"] = _cnttDB.gov_comments.Where(c => c.news_id == id && c.active_flg == true).OrderBy(c => c.entry_datetime).ToList();
            return View("Comment");
        }

        public Boolean comment(String fullName, String email, String commentContent, int newsId, int idParent)
        {
            /*CommentServices commentServices = new CommentServices();
            CommentModels commentModels = new CommentModels();
            commentModels.ParentId = idParent;
            commentModels.NewsId = newsId;
            commentModels.FullName = fullName;
            commentModels.Email = email;
            commentModels.CommentsContent = commentContent;
            commentModels.TotalLike = 0;
            commentModels.ActiveFlg = true;
            commentModels.EntryDatetime = DateTime.Now;
            int rs = commentServices.insert(commentModels);*/
            gov_comments commentInfo = new gov_comments();
            commentInfo.parent_id = idParent;
            commentInfo.news_id = newsId;
            commentInfo.full_name= fullName;
            commentInfo.email = email;
            commentInfo.comments_content = commentContent;
            commentInfo.total_like = 0;
            commentInfo.active_flg = false;
            commentInfo.entry_datetime = DateTime.Now;
            commentInfo = _cnttDB.gov_comments.Add(commentInfo);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                var lstUser = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
                foreach (var user in lstUser)
                {
                    if (SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, user.username, TypeAudit.ManagerComment))
                    {
                        try
                        {
                            gov_message_system messageInfo = new gov_message_system();
                            messageInfo.entry_datetime = DateTime.Now;
                            messageInfo.type_message = Constant.MESSAGE_TYPE_COMMENT;
                            messageInfo.status = false;
                            messageInfo.content_message = "<strong>" + fullName + "</strong>" + Constant.MESSAGE_COMMENT_CONTENT;
                            messageInfo.username = user.username;
                            messageInfo.link = "/admin/comment/index";
                            _cnttDB.gov_message_system.Add(messageInfo);
                            _cnttDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public int likeComment(int idComment) {
            gov_comments commentInfo = _cnttDB.gov_comments.Find(idComment);
            if (commentInfo != null)
            {
                if (Request.Cookies["Cookie_Comment_" + idComment.ToString()] == null)
                {
                    commentInfo.total_like = commentInfo.total_like + 1;
                    int rs = _cnttDB.SaveChanges();
                    if (rs != 0) {
                        HttpCookie cookieNew = new HttpCookie("Cookie_Comment_" + idComment.ToString());
                        cookieNew.Value = "True";
                        cookieNew.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(cookieNew);
                        return 1;
                    }
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 3;
            }
            return 3;
        }

        public ActionResult Print(int id)
        {
            //View new
            gov_news newInfo = _cnttDB.gov_news.Find(id);
            if (newInfo != null)
                ViewData["new"] = newInfo;
            return View();
        }
    }
}
