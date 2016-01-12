using AttributeRouting;
using AttributeRouting.Web.Mvc;
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

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("cuu-sinh-vien")]
    public class NotebookController : BaseController
    {
        //
        // GET: /Notebook/

        [GET("luu-but-ra-truong")]
        [HttpGet]
        public ActionResult Index()
        {
            NotebookViewhelper notebookViewhelper = new NotebookViewhelper();
            saveData(notebookViewhelper);
            return View();
        }

        [POST("luu-but-ra-truong")]
        [HttpPost]
        public ActionResult Index(NotebookViewhelper notebookViewhelper)
        {
            saveData(notebookViewhelper);
            return View("Index");
        }

        public void saveData(NotebookViewhelper notebookViewhelper)
        {
            List<gov_notebook> lstnotebook = _cnttDB.gov_notebook.Where(u => u.active_flg == true).ToList();
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
        }
        public List<gov_notebook> setSearchFilter(List<gov_notebook> lstnotebook, NotebookViewhelper notebookViewhelper)
        {
            Expression<Func<gov_notebook, bool>> predicate = PredicateBuilder.False<gov_notebook>();
            if (!String.IsNullOrWhiteSpace(notebookViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.teacher != null && d.teacher.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.remember != null &&  d.remember.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.note != null && d.note.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.dream != null && d.dream.ToUpper().Contains(notebookViewhelper.KeySearch.ToUpper()));
                lstnotebook = lstnotebook.Where(predicate.Compile()).ToList();
            }
            if (notebookViewhelper.CourseFilter > 0)
            {
                lstnotebook = lstnotebook.Where(n => n.course_id == notebookViewhelper.CourseFilter).ToList();
            }
            if (notebookViewhelper.SpecializedFilter > 0)
            {
                lstnotebook = lstnotebook.Where(n => n.specialized_id == notebookViewhelper.SpecializedFilter).ToList();
            }
            return lstnotebook;
        }

        [GET("luu-but-ra-truong/dang-ky-thong-tin")]
        [HttpGet]
        public ActionResult Regist()
        {
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(c => c.specialized_name).ToList();
            return View();
        }

        [POST("luu-but-ra-truong/dang-ky-thong-tin")]
        [HttpPost]
        public ActionResult SaveRegist(gov_notebook item)
        {
            item.entry_datetime = DateTime.Now;
            item.update_datetime = DateTime.Now;
            _cnttDB.gov_notebook.Add(item);
            try {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    var lstUser = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
                    foreach (var user in lstUser)
                    {
                        if (SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, user.username, TypeAudit.ManagerNotebook))
                        {
                            try
                            {
                                gov_message_system messageInfo = new gov_message_system();
                                messageInfo.entry_datetime = DateTime.Now;
                                messageInfo.type_message = Constant.MESSAGE_TYPE_NOTEBOOK;
                                messageInfo.status = false;
                                messageInfo.content_message = "<strong>" + item.email + "</strong>" + Constant.MESSAGE_COMMENT_NOTEBOOK;
                                messageInfo.username = user.username;
                                messageInfo.link = "/admin/notebook/index";
                                _cnttDB.gov_message_system.Add(messageInfo);
                                _cnttDB.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ViewData["error"] = "Đã có lỗi xảy ra. Đăng ký thông tin thất bại!";
                            }
                        }
                    }
                    ViewData["message"] = "Đăng ký lưu bút ra trường thành công. Lưu bút của bạn sẽ được kiểm duyệt trước khi cho hiển thị lên website!";
                }
                else
                {
                    ViewData["error"] = "Đăng ký thất bại. Vui lòng thử lại";
                }
            } catch(Exception ex){
                ViewData["error"] = "Đăng ký thất bại. Vui lòng thử lại";
            }
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(c => c.specialized_name).ToList();
            return View("Regist");
        }

        [GET("luu-but-ra-truong/get-captcha")]
        public CaptchaResult getCaptcha()
        {
            string captchaText = Captcha.GenerateRandomCode();
            HttpContext.Session.Add("captcha", captchaText);
            return new CaptchaResult(captchaText);
        }

        [GET("luu-but-ra-truong/check-captcha")]
        public String checkCaptcha(String captcha)
        {
            return HttpContext.Session["captcha"].ToString();
        }

        [GET("luu-but-ra-truong/check-email")]
        public bool checkEmail(String email)
        {
            var rs = true;
            var lstNotebook = _cnttDB.gov_notebook.Where(p => p.email.ToUpper().Equals(email.ToUpper())).ToList();
            if (lstNotebook != null && lstNotebook.Count > 0)
                rs = false;
            return rs;
        }
    }
}
