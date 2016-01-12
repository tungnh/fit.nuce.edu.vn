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
    public class NotebookController : BaseController
    {
        //
        // GET: /Admin/Notebook/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            NotebookViewhelper notebookViewhelper = new NotebookViewhelper();
            saveData(notebookViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(NotebookViewhelper notebookViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(notebookViewhelper);
            return View();
        }

        public NotebookViewhelper saveData(NotebookViewhelper notebookViewhelper)
        {
            List<gov_notebook> lstnotebook = _cnttDB.gov_notebook.ToList();
            lstnotebook = setSearchFilter(lstnotebook, notebookViewhelper);
            int totalCount = lstnotebook.Count;
            notebookViewhelper.TotalCount = totalCount;

            if (notebookViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                notebookViewhelper.TotalPage = totalPage;
                notebookViewhelper.Page = pageTransition(notebookViewhelper.Direction, notebookViewhelper.Page, totalPage);
                notebookViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, notebookViewhelper.Page);
                notebookViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, notebookViewhelper.Page, notebookViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (notebookViewhelper.Page - 1) * take;
                notebookViewhelper.LstNotebook = lstnotebook.OrderByDescending(u => u.entry_datetime).Skip(skip).Take(take).ToList();
            }
            notebookViewhelper.LstCourse = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            notebookViewhelper.LstSpecialized = _cnttDB.gov_specialized.OrderBy(s => s.specialized_name).ToList();
            ViewData["notebookViewhelper"] = notebookViewhelper;
            return notebookViewhelper;
        }
        public List<gov_notebook> setSearchFilter(List<gov_notebook> lstnotebook, NotebookViewhelper notebookViewhelper)
        {
            Expression<Func<gov_notebook, bool>> predicate = PredicateBuilder.False<gov_notebook>();
            if (!String.IsNullOrWhiteSpace(notebookViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.title != null && d.title.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.teacher != null && d.teacher.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.remember != null && d.remember.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.note != null && d.note.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.dream != null && d.dream.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                lstnotebook = lstnotebook.Where(predicate.Compile()).ToList();
            }
            if (notebookViewhelper.CourseFilter != 0) {
                lstnotebook = lstnotebook.Where(n => n.course_id == notebookViewhelper.CourseFilter).ToList();
            }
            if (notebookViewhelper.SpecializedFilter != 0)
            {
                lstnotebook = lstnotebook.Where(n => n.specialized_id == notebookViewhelper.SpecializedFilter).ToList();
            }
            if (!String.IsNullOrWhiteSpace(notebookViewhelper.FilterActive))
            {
                lstnotebook = lstnotebook.Where(n => n.active_flg == Boolean.Parse(notebookViewhelper.FilterActive)).ToList();
            }
            return lstnotebook;
        }

        [HttpPost]
        public ActionResult changeActive(int[] ids, String changeActive)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            if (ids != null)
            {
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
                foreach (int id in ids)
                {
                    gov_notebook item = _cnttDB.gov_notebook.Find(id);
                    if (item != null)
                    {
                        state = item.active_flg;
                        item.active_flg = Convert.ToBoolean(changeActive);
                        int rs = _cnttDB.SaveChanges();
                        if (rs > 0)
                        {
                            if (state == false && Convert.ToBoolean(changeActive))
                            {

                                String smtpHost = "smtp.gmail.com";
                                int smtpPost = 587;
                                String emailTo = item.email;
                                String subject = "Thông tin đăng ký lưu bút ra trường của bạn đã được duyệt";
                                String body = "<p><strong>Website Khoa công nghệ thông tin thông báo:</strong> Thông tin lưu bút ra trường của bạn đã được duyệt.</p>";
                                body += "<p><strong><a href='fit.nuce.edu.vn/cuu-sinh-vien/luu-but-ra-truong' target='_blank'>BẤM ĐÂY</a></strong> để xem danh sách lưu bút ra trường của website</p>";
                                body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:" + smtpUsername + "' target='_blank'>" + smtpUsername + "</a></p>";
                                body += "<p><strong style='font-style: italic;'>Xin cảm ơn!</strong></p>";
                                body += "<p></p>";
                                EmailServices services = new EmailServices();
                                services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
                            }
                            content += Constant.CHUYEN_TRANG_THAI(Constant.ITEM_LUUBUTRATRUONG, id.ToString(), state.ToString(), changeActive);
                            content += ".<br/>";
                        }
                    }
                }
                if (!content.Equals(""))
                {
                    insertHistory(AccessType.chuyenTrangThaiLuuBut, content);
                    TempData["message"] = Constant.CHANGE_STATE_SUCCESSFULL;
                }
            }
            return Redirect("Index");
        }

        public ActionResult delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            gov_notebook item = new gov_notebook();
            item = _cnttDB.gov_notebook.Find(id);
            if (item != null)
            {
                try
                {
                    _cnttDB.gov_notebook.Remove(item);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaLuuBut, Constant.XOA(Constant.ITEM_LUUBUTRATRUONG, Constant.ID, id.ToString()));
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

        public ActionResult Edit(int id){
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["notebookInfo"] = _cnttDB.gov_notebook.Find(id);
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(s => s.specialized_name).ToList();
            return View();
        }

        public ActionResult SaveEdit(gov_notebook item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerNotebook))
            {
                return Redirect("/admin/error/error403");
            }
            gov_notebook notebookInfo = _cnttDB.gov_notebook.Find(item.id);
            notebookInfo.email = item.email;
            notebookInfo.lop = item.lop;
            notebookInfo.specialized_id = item.specialized_id;
            notebookInfo.course_id = item.course_id;
            notebookInfo.note = item.note;
            notebookInfo.remember = item.remember;
            notebookInfo.teacher = item.teacher;
            notebookInfo.title = item.title;
            notebookInfo.dream = item.dream;
            notebookInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaLuuBut, Constant.CHINHSUA(Constant.ITEM_LUUBUTRATRUONG, Constant.ID, item.id.ToString()));
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
    }
}
