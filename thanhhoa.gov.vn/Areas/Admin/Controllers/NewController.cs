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
    public class NewController : BaseController
    {
        //
        // GET: /Admin/New/
        [HttpGet]
        public ActionResult Index() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            NewViewhelper newViewhelper = new NewViewhelper();
            saveListNew(newViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(NewViewhelper newViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            saveListNew(newViewhelper);
            return View();
        }

        public NewViewhelper saveListNew(NewViewhelper newViewhelper)
        {
            List<gov_news> lstNew = _cnttDB.gov_news.ToList();
            lstNew = setSearchFilter(lstNew, newViewhelper);

            int totalCount = lstNew.Count;
            newViewhelper.TotalCount = totalCount;

            if (newViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                newViewhelper.TotalPage = totalPage;
                newViewhelper.Page = pageTransition(newViewhelper.Direction, newViewhelper.Page, totalPage);
                newViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page);
                newViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page, newViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (newViewhelper.Page - 1) * take;
                newViewhelper.LstNew = lstNew.OrderByDescending(n => n.entry_datetime).Skip(skip).Take(take).ToList();
            }
            newViewhelper.LstMenu = _cnttDB.gov_menu.OrderBy(m => m.order_number).ToList();
            ViewData["newViewhelper"] = newViewhelper; 
            return newViewhelper;
        }

        public List<gov_news> setSearchFilter(List<gov_news> lstNew, NewViewhelper newViewhelper)
        {
            Expression<Func<gov_news, bool>> predicate = PredicateBuilder.False<gov_news>();
            if (!String.IsNullOrWhiteSpace(newViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.title != null && d.title.ToUpper().Contains(newViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.description != null && d.description.ToUpper().Contains(newViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.new_content != null && d.new_content.ToUpper().Contains(newViewhelper.KeySearch.ToUpper()));
                lstNew = lstNew.Where(predicate.Compile()).ToList();
            }
            if (newViewhelper.MenuFilter != 0) {
                lstNew = lstNew.Where(n => n.menu_id == newViewhelper.MenuFilter).ToList();
            }
            return lstNew;
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["lstMenu"] = _cnttDB.gov_menu.ToList();
            ViewData["lstTypeNew"] = _cnttDB.gov_type_news.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveRegist(gov_news item){
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.total_view = 0;
            item = _cnttDB.gov_news.Add(item);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0) {
                var luceneNew = new LuceneSerives.LuceneNews();
                luceneNew.AddUpdateLuceneIndex(item);
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiTinTuc, Constant.THEM(Constant.ITEM_TINTUC, Constant.ID, item.id.ToString()));
                    //SEND MAIL
                    //get email pass
                    String smtpUsername = "";
                    String smtpPassword = "";
                    List<gov_system_config> lstSystem = _cnttDB.gov_system_config.ToList();
                    foreach (var item1 in lstSystem)
                    {
                        if (item1.key_config.Equals(Constant.CONFIG_KEY_EMAIL))
                        {
                            smtpUsername = item1.value_config;
                        }
                        if (item1.key_config.Equals(Constant.CONFIG_KEY_PASS))
                        {
                            smtpPassword = item1.value_config;
                        }
                    }
                    var lstUser = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
                    foreach (var user in lstUser)
                    {
                        var c = SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, user.username, TypeAudit.ManagerStudent);
                        if (SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, user.username, TypeAudit.ManagerStudent))
                        {
                            try
                            {
                                if (!String.IsNullOrWhiteSpace(user.email))
                                {
                                    String menuName = "";
                                    var menuInfo = _cnttDB.gov_menu.Find(item.menu_id);
                                    if (menuInfo != null)
                                        menuName = menuInfo.title;
                                    String smtpHost = "smtp.gmail.com";
                                    int smtpPost = 587;
                                    String emailTo = user.email;
                                    String subject = "[Fwd: Fit.nuce.edu.vn] Bài viết mới trên fit.nuce.edu.vn";
                                    String body = "<p><strong>Website Khoa công nghệ thông tin thông báo:</strong></p>";
                                    body += "<p><strong>" + item.entry_username + "</strong> vừa thêm mới bài viết có tiêu đề: <strong>" + item.title + "</strong>; vào danh mục: <strong>" + menuName + "</strong> lúc: <strong>" + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + "</strong>.";
                                    body += "<p>Chi tiết về bài viết: </p>";
                                    body += "<div style='padding:0;min-height:100%;width:100%'>";
                                    body += "<table style='width:600px;' cellspacing='10' cellpadding=''>";
                                    body += "<tbody>";
                                    body += "<tr>";
                                    body += "<td style='padding:30px;font-size:13px;margin:0;border-radius:0px 0px 4px 4px;border:1px solid rgba(218,218,215,0.6);border-bottom:4px solid rgba(218,218,215,0.6)' colspan='2'>";
                                    body += "<h3 style='font-size:17px;font-weight:bold'><span style='color:#003c95'>" + item.title + "</h3></span>";
                                    body += "<h4 style='margin-bottom : 10px;'><p>" + item.description + "</p></h4>";
                                    if (!string.IsNullOrEmpty(item.new_content))
                                        body += item.new_content.Replace("src=\"/Upload/", "src=\"http:fit.nuce.edu.vn/Upload/");
                                    else
                                        body += "";
                                    body += "</td>";
                                    body += "</tr>";
                                    body += "</tbody></table>";
                                    body += "</div>";
                                    body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:" + smtpUsername + "' target='_blank'>" + smtpUsername + "</a></p>";
                                    body += "<p><strong style='font-style: italic;'>Xin cảm ơn!</strong></p>";
                                    body += "<p></p>";
                                    EmailServices services = new EmailServices();
                                    var a = services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
                                    var b = a;
                                }
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                        }
                    }
                    TempData["message"] = "Thêm mới thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Thêm mới thông tin thất bại!";
                }
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["newInfo"] = _cnttDB.gov_news.Single(n => n.id == id);
            ViewData["lstMenu"] = _cnttDB.gov_menu.ToList();
            ViewData["lstTypeNew"] = _cnttDB.gov_type_news.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(gov_news item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            gov_news newInfo = _cnttDB.gov_news.Find(item.id);
            newInfo.active_flg = item.active_flg;
            newInfo.avatar = item.avatar;
            newInfo.description = item.description;
            newInfo.is_shared = item.is_shared;
            newInfo.is_tinlq = item.is_tinlq;
            newInfo.is_comment = item.is_comment;
            newInfo.is_home = item.is_home;
            newInfo.menu_id = item.menu_id;
            newInfo.new_content = item.new_content;
            newInfo.new_source = item.new_source;
            newInfo.title = item.title;
            newInfo.type_id = item.type_id;
            newInfo.meta_desciption = item.meta_desciption;
            newInfo.meta_keyword = item.meta_keyword;
            newInfo.update_username = Session.getCurrentUser().username;
            newInfo.update_datetime = DateTime.Now;
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                var luceneNew = new LuceneSerives.LuceneNews();
                luceneNew.AddUpdateLuceneIndex(newInfo);
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaTinTuc, Constant.CHINHSUA(Constant.ITEM_TINTUC, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Cập nhật thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Cập nhật thông tin thất bại!";
                }
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Copy(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["newInfo"] = _cnttDB.gov_news.Single(n => n.id == id);
            ViewData["lstMenu"] = _cnttDB.gov_menu.ToList();
            ViewData["lstTypeNew"] = _cnttDB.gov_type_news.ToList();
            return View();
        }

        [HttpGet]

        public ActionResult delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            gov_news newInfo = _cnttDB.gov_news.Find(id);
            if (newInfo != null)
            {
                try
                {
                    _cnttDB.gov_news.Remove(newInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs == 0)
                        return Redirect("/admin/error/error404");
                    else {
                        var lucene = new LuceneSerives.LuceneNews();
                        lucene.ClearLuceneIndexRecord(id);
                        insertHistory(AccessType.xoaTinTuc, Constant.XOA(Constant.ITEM_TINTUC, Constant.ID, id.ToString()));
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
            TempData["message"] = "Xóa thông tin thành công!";
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult delete(int[] checkID)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            String content = "";
            foreach (int _id in checkID)
            {
                _cnttDB.gov_news.Remove(_cnttDB.gov_news.Find(_id));
                int rs = _cnttDB.SaveChanges();
                if (rs > 0){
                    content += Constant.XOA(Constant.ITEM_TINTUC, Constant.ID, _id.ToString());
                    content += ".<br/>";
                }
            }
            if(!content.Equals(""))
                insertHistory(AccessType.xoaTinTuc, content);
            TempData["message"] = "Xóa thông tin thành công!";
            return Redirect("Index");
        }

        public ActionResult changeActive(int[] checkID, String changeActive)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Edit)
                ||!SercurityServices.HasPermission((int)TypeModule.MODULE_TINTUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            if (checkID != null)
            {
                String content = "";
                Boolean state;
                foreach (int id in checkID)
                {
                    gov_news item = _cnttDB.gov_news.Find(id);
                    if (item != null)
                    {
                        state = item.active_flg;
                        item.active_flg = Convert.ToBoolean(changeActive);
                        int rs = _cnttDB.SaveChanges();
                        if (rs > 0)
                        {
                            content += Constant.CHUYEN_TRANG_THAI(Constant.ITEM_TINTUC, id.ToString(), state.ToString(), changeActive);
                            content += ".<br/>";
                        }
                    }
                }
                if (!content.Equals(""))
                    insertHistory(AccessType.chuyenTrangThaiTinTuc, content);
            }
            TempData["message"] = "Chuyển trạng thái thành công!";
            return Redirect("Index");
        }
    }
}
