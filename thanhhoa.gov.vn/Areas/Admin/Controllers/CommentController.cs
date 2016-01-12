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
    public class CommentController : BaseController
    {
        
        //
        // GET: /Admin/Comment/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            CommentViewhelper commentViewhelper = new CommentViewhelper();
            saveData(commentViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(CommentViewhelper commentViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(commentViewhelper);
            return View();
        }

        public ActionResult ChangeState(CommentViewhelper commentViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            if (commentViewhelper.CheckID != null && commentViewhelper.CheckID.Length > 0) {
                //get email pass
                String smtpUsername = "";
                String smtpPassword = "";
                List<gov_system_config> lstSystem = _cnttDB.gov_system_config.ToList();
                foreach (var item in lstSystem)
                {
                    if (item.key_config.Equals(Constant.CONFIG_KEY_EMAIL))
                    {
                        smtpUsername = item.value_config;
                    }
                    if (item.key_config.Equals(Constant.CONFIG_KEY_PASS))
                    {
                        smtpPassword = item.value_config;
                    }
                }
                String content = "";
                Boolean state;
                foreach (var id in commentViewhelper.CheckID) {
                    gov_comments commentInfo = _cnttDB.gov_comments.Find(id);
                    state = commentInfo.active_flg;
                    commentInfo.active_flg = commentViewhelper.ChangeState;
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0) {
                        if (state == false && Convert.ToBoolean(commentViewhelper.ChangeState))
                        {

                            String smtpHost = "smtp.gmail.com";
                            int smtpPost = 25;
                            String emailTo = commentInfo.email;
                            String subject = "Bình luận của bạn " + commentInfo.full_name + " đã được duyệt";
                            String body = "<p><strong>Website Khoa Công Nghệ Thông Tin - ĐHXD thông báo:</strong> Bình luận của bạn đã được duyệt.</p>";
                            body += "<p><strong><a href='http://fit.nuce.edu.vn" + Utils.getLinkDefault(commentInfo.news_id, TypeLink.tintuc) + "' target='_blank'>BẤM ĐÂY</a><strong> để xem thông tin bình luận của bạn</p>";
                            body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:" + smtpUsername + "' target='_blank'>" + smtpUsername + "</a></p>";
                            body += "<p><strong style='font-style: italic;'>Xin cảm ơn!</strong></p>";
                            body += "<p></p>";
                            EmailServices services = new EmailServices();
                            services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
                        }
                        content += Constant.CHUYEN_TRANG_THAI(Constant.ITEM_BINHLUAN, id.ToString(), state.ToString(), commentViewhelper.ChangeState.ToString());
                        content += ".<br/>";
                    }
                }
                if (!content.Equals(""))
                {
                    insertHistory(AccessType.chuyenTrangThaiBinhLuan, content);
                    TempData["message"] = Constant.CHANGE_STATE_SUCCESSFULL;
                }
            }
            saveData(commentViewhelper);
            return View("Index");
        }

        public ActionResult Filter(CommentViewhelper commentViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(commentViewhelper);
            return View("Index");
        }

        [HttpGet]
        public ActionResult Regist() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            CommentViewhelper item = new CommentViewhelper();
            saveDataKey(item);
            return View();
        }

        [HttpPost]
        public ActionResult Regist(CommentViewhelper item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            saveDataKey(item);
            return View();
        }

        public ActionResult deleteKey(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            gov_key_band keyBandInfo = _cnttDB.gov_key_band.Find(id);
            if (keyBandInfo != null)
            {
                _cnttDB.gov_key_band.Remove(keyBandInfo);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.xoaKeyBinhLuan, Constant.XOA(Constant.ITEM_BANDBINHLUAN, Constant.ID, id.ToString()));
                    TempData["message"] = Constant.DELETE_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.DELETE_ERR;
                }
            }
            
            return Redirect("regist");
        }
        
        [HttpPost]
        public ActionResult SaveRegist(gov_key_band item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item = _cnttDB.gov_key_band.Add(item);
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themKeyBinhLuan, Constant.THEM(Constant.ITEM_BANDBINHLUAN, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.DELETE_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.DELETE_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.DELETE_ERR;
            }
            return Redirect("Regist");
        }

        public ActionResult delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.ManagerComment))
            {
                return Redirect("/admin/error/error403");
            }
            gov_comments commentInfo = _cnttDB.gov_comments.Find(id);
            _cnttDB.gov_comments.Remove(commentInfo);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                insertHistory(AccessType.xoaBinhLuan, Constant.XOA(Constant.ITEM_BINHLUAN, Constant.ID, id.ToString()));
                TempData["message"] = Constant.DELETE_SUCCESSFULL;
            }
            else
            {
                TempData["err"] = Constant.DELETE_ERR;
            }
            return Redirect("Index");
        }

        public CommentViewhelper saveDataKey(CommentViewhelper commentViewhelper)
        {
            List<gov_key_band> lstKeyBand = _cnttDB.gov_key_band.ToList();
            int totalCount = lstKeyBand.Count();
            commentViewhelper.TotalCount = totalCount;

            if (commentViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                commentViewhelper.TotalPage = totalPage;
                commentViewhelper.Page = pageTransition(commentViewhelper.Direction, commentViewhelper.Page, totalPage);
                commentViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, commentViewhelper.Page);
                commentViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, commentViewhelper.Page, commentViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (commentViewhelper.Page - 1) * take;
                commentViewhelper.LstKeyBand = lstKeyBand.OrderByDescending(c => c.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["commentViewhelper"] = commentViewhelper;
            return commentViewhelper;
        }

        public CommentViewhelper saveData(CommentViewhelper commentViewhelper)
        {
            List<gov_comments> lstComment = _cnttDB.gov_comments.ToList();
            lstComment = setSearchFilter(lstComment, commentViewhelper);

            int totalCount = lstComment.Count();
            commentViewhelper.TotalCount = totalCount;

            if (commentViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                commentViewhelper.TotalPage = totalPage;
                commentViewhelper.Page = pageTransition(commentViewhelper.Direction, commentViewhelper.Page, totalPage);
                commentViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, commentViewhelper.Page);
                commentViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, commentViewhelper.Page, commentViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (commentViewhelper.Page - 1) * take;
                commentViewhelper.LstComment = lstComment.OrderByDescending(c => c.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["commentViewhelper"] = commentViewhelper;
            return commentViewhelper;
        }

        public List<gov_comments> setSearchFilter(List<gov_comments> lstComment, CommentViewhelper commentViewhelper)
        {
            Expression<Func<gov_comments, bool>> predicate = PredicateBuilder.False<gov_comments>();
            if (!String.IsNullOrWhiteSpace(commentViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.email != null && d.email.ToUpper().Contains(commentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.full_name != null && d.full_name.ToUpper().Contains(commentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.comments_content != null && d.comments_content.ToUpper().Contains(commentViewhelper.KeySearch.ToUpper()));
                lstComment = lstComment.Where(predicate.Compile()).ToList();
            }
            if (commentViewhelper.FilterActive == 1)
            {
                lstComment = lstComment.Where(c => c.active_flg == true).ToList();
            }
            else if (commentViewhelper.FilterActive == 2)
            {
                lstComment = lstComment.Where(c => c.active_flg == false).ToList();
            }
            if (commentViewhelper.FilterKey == 1)
            {
                List<gov_comments> lst = new List<gov_comments>();
                foreach (var item in lstComment) {
                    bool check = true;
                    foreach (var itemKeyBand in _cnttDB.gov_key_band.ToList()) {
                        if (item.full_name.Contains(itemKeyBand.key_band)
                            || item.email.Contains(itemKeyBand.key_band)
                            || item.comments_content.Contains(itemKeyBand.key_band))
                        {
                            if (check)
                            {
                                lst.Add(item);
                                check = false;
                            }
                            lst.Last().full_name = lst.Last().full_name.Replace(itemKeyBand.key_band, "<b style='color:black;background:yellow;'>" + itemKeyBand.key_band + "</b>");
                            lst.Last().email = lst.Last().email.Replace(itemKeyBand.key_band, "<b style='color:black;background:yellow;'>" + itemKeyBand.key_band + "</b>");
                            lst.Last().comments_content = lst.Last().comments_content.Replace(itemKeyBand.key_band, "<b style='color:black;background:yellow;'>" + itemKeyBand.key_band + "</b>");
                        }
                        
                    }
                }
                lstComment = lst;
            }
            else if (commentViewhelper.FilterKey == 2) {
                List<gov_comments> lst = new List<gov_comments>();
                foreach (var item in lstComment)
                {
                    bool check = true;
                    foreach (var itemKeyBand in _cnttDB.gov_key_band.ToList())
                    {
                        if (item.full_name.Contains(itemKeyBand.key_band)
                            || item.email.Contains(itemKeyBand.key_band)
                            || item.comments_content.Contains(itemKeyBand.key_band))
                        {
                            check = false;
                        }

                    }
                    if(check)
                        lst.Add(item);
                }
                lstComment = lst;
            }
            return lstComment;
        }
    }
}
