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
    public class StudentController : BaseController
    {
        //
        // GET: /Admin/Student/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            StudentViewhelper studentViewhelper = new StudentViewhelper();
            saveData(studentViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(StudentViewhelper studentViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(studentViewhelper);
            return View();
        }

        public StudentViewhelper saveData(StudentViewhelper studentViewhelper)
        {
            List<gov_person> lstStudent = _cnttDB.gov_person.ToList();
            lstStudent = setSearchFilter(lstStudent, studentViewhelper);
            int totalCount = lstStudent.Count;
            studentViewhelper.TotalCount = totalCount;

            if (studentViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                studentViewhelper.TotalPage = totalPage;
                studentViewhelper.Page = pageTransition(studentViewhelper.Direction, studentViewhelper.Page, totalPage);
                studentViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, studentViewhelper.Page);
                studentViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, studentViewhelper.Page, studentViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (studentViewhelper.Page - 1) * take;
                studentViewhelper.LstStudent = lstStudent.OrderByDescending(u => u.entry_datetime).Skip(skip).Take(take).ToList();
            }
            studentViewhelper.LstCourse = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            studentViewhelper.LstSpecialized = _cnttDB.gov_specialized.OrderBy(s => s.specialized_name).ToList();
            ViewData["studentViewhelper"] = studentViewhelper;
            return studentViewhelper;
        }
        public List<gov_person> setSearchFilter(List<gov_person> lstStudent, StudentViewhelper studentViewhelper)
        {
            Expression<Func<gov_person, bool>> predicate = PredicateBuilder.False<gov_person>();
            if (!String.IsNullOrWhiteSpace(studentViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.full_name != null && d.full_name.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => (d.department).ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.email != null && d.email.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                lstStudent = lstStudent.Where(predicate.Compile()).ToList();
            }
            if (!String.IsNullOrWhiteSpace(studentViewhelper.FilterActive))
            {
                lstStudent = lstStudent.Where(u => u.active_flg == Convert.ToBoolean(studentViewhelper.FilterActive)).ToList();
            }
            if (studentViewhelper.CourseFilter > 0)
            {
                lstStudent = lstStudent.Where(n => n.course_id == studentViewhelper.CourseFilter).ToList();
            }
            if (studentViewhelper.SpecializedFilter > 0)
            {
                lstStudent = lstStudent.Where(n => n.specialized_id == studentViewhelper.SpecializedFilter).ToList();
            }
            return lstStudent;
        }

        public ActionResult delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            gov_person item = new gov_person();
            item = _cnttDB.gov_person.Find(id);
            if (item != null)
            {
                try {
                    _cnttDB.gov_person.Remove(item);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaCuuSinhVien, Constant.XOA(Constant.ITEM_CUUSINHVIEN, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                    else
                    {
                        TempData["err"] = Constant.DELETE_ERR;
                    }
                } catch(Exception ex){
                    return Redirect("/admin/error/error404");
                }
            }
            else {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }

        public ActionResult changeActive(int[] ids, String changeActive) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            if (ids != null) {

                //get email pass
                String smtpUsername = "";
                String smtpPassword = "";
                List<gov_system_config> lstSystem = _cnttDB.gov_system_config.ToList();
                foreach (var item in lstSystem) {
                    if (item.key_config.Equals(Constant.CONFIG_KEY_EMAIL)){
                        smtpUsername = item.value_config;
                    }
                    if (item.key_config.Equals(Constant.CONFIG_KEY_PASS))
                    {
                        smtpPassword = item.value_config;
                    }
                }
                String content = "";
                Boolean state;
                foreach (int id in ids) {
                    gov_person item = _cnttDB.gov_person.Find(id);
                    if (item != null) {
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
                                String subject = "Thông tin đăng ký cựu sinh viên của bạn đã được duyệt";
                                String body = "<p><strong>Website Khoa công nghệ thông tin thông báo:</strong> Thông tin cựu sinh viên của bạn đã được duyệt.</p>";
                                body += "<p><strong>Mật mã</strong> dùng để phục vụ thay đổi thông tin của bạn sau này là: <strong>" + item.code + "</strong></p>";
                                body += "<p><strong style='font-style: italic;'>Bạn có thể tiếp tục với các lựa chọn sau:</strong></p>";
                                body += "<p>- <strong><a href='fit.nuce.edu.vn/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu/thay-doi-mat-ma' target='_blank'>BẤM ĐÂY</a></strong> để thay đổi mật mã của bạn</p>";
                                body += "<p>- <strong><a href='fit.nuce.edu.vn/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu' target='_blank'>BẤM ĐÂY</a></strong> để xem danh sách cựu sinh viên</p>";
                                body += "<p>- Xem thông tin cựu sinh viên của bạn <strong><a href='fit.nuce.edu.vn/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu/"  + Utils.ConvertToUnSign(item.full_name) + "-" + item.id + "' target='_blank'>Xem chi tiết</a></strong> </p>";
                                body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:" + smtpUsername + "' target='_blank'>" + smtpUsername + "</a></p>";
                                body += "<p><strong style='font-style: italic;'>Xin cảm ơn!</strong></p>";
                                body += "<p></p>";
                                EmailServices services = new EmailServices();
                                services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
                            }

                            content += Constant.CHUYEN_TRANG_THAI(Constant.ITEM_CUUSINHVIEN, id.ToString(), state.ToString(), changeActive);
                            content += ".<br/>";
                        }
                    }
                }
                if (!content.Equals(""))
                {
                    insertHistory(AccessType.chuyenTrangThaiCuuSinhVien, content);
                    TempData["message"] = Constant.CHANGE_STATE_SUCCESSFULL;
                }
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult regist() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(s => s.specialized_name).ToList();
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult saveRegist(gov_person item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            item.active_flg = true;
            item.update_datetime = DateTime.Now;
            _cnttDB.gov_person.Add(item);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                insertHistory(AccessType.themMoiCuuSinhVien, Constant.THEM(Constant.ITEM_CUUSINHVIEN, Constant.ID, item.id.ToString()));
            }
            return Redirect("Index");
        }

        public ActionResult Edit(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            gov_person personInfo = _cnttDB.gov_person.Find(id);
            ViewData["personInfo"] = personInfo;
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(s => s.specialized_name).ToList();
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_person item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, Session.getCurrentUser().username, TypeAudit.ManagerStudent))
            {
                return Redirect("/admin/error/error403");
            }
            gov_person personInfo = _cnttDB.gov_person.Find(item.id);
            personInfo.full_name = item.full_name;
            personInfo.specialized_id = item.specialized_id;
            personInfo.course_id = item.course_id;
            personInfo.lop = item.lop;
            personInfo.phone = item.phone;
            personInfo.email = item.email;
            personInfo.address = item.address;
            personInfo.avatar = item.avatar;
            personInfo.department = item.department;
            personInfo.description = item.description;
            personInfo.show_address = item.show_address;
            personInfo.show_department = item.show_department;
            personInfo.show_email = item.show_email;
            personInfo.show_shared = item.show_shared;
            personInfo.show_tel = item.show_tel;
            personInfo.code = item.code;
            personInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaCuuSinhVien, Constant.CHINHSUA(Constant.ITEM_CUUSINHVIEN, Constant.ID, item.id.ToString()));
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
    }
}
