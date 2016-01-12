using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using System.Linq.Expressions;
using thanhhoa.gov.vn.Services;

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("sinh-vien", Precedence = 1)]
    [RoutePrefix("cuu-sinh-vien", Precedence=2)]
    public class StudentController : BaseController
    {
        private string fileSaveFolder = "/Upload/Student";
        [GET("cuu-sinh-vien-tieu-bieu/get-captcha")]
        public CaptchaResult getCaptcha()
        {
            string captchaText = Captcha.GenerateRandomCode();
            HttpContext.Session.Add("captcha", captchaText);
            return new CaptchaResult(captchaText);
        }

        [GET("cuu-sinh-vien-tieu-bieu/check-captcha")]
        public String checkCaptcha(String captcha)
        {
            return HttpContext.Session["captcha"].ToString();
        }

        [GET("cuu-sinh-vien-tieu-bieu/check-email")]
        public bool checkEmail(String email)
        {
            var rs = true;
            var lstStudent = _cnttDB.gov_person.Where(p => p.email.ToUpper().Equals(email.ToUpper())).ToList();
            if (lstStudent != null && lstStudent.Count > 0)
                rs = false;
            return rs;
        }

        [GET("cuu-sinh-vien-tieu-bieu")]
        [HttpGet]
        public ActionResult Index()
        {
            StudentViewhelper studentViewhelper = new StudentViewhelper();
            saveStudentData(studentViewhelper);
            return View();
        }

        [POST("cuu-sinh-vien-tieu-bieu")]
        [HttpPost]
        public ActionResult Index(StudentViewhelper studentViewhelper)
        {
            saveStudentData(studentViewhelper);
            return View();
        }

        public void saveStudentData(StudentViewhelper studentViewhelper)
        {
            List<gov_person> lstStudent = _cnttDB.gov_person.Where(u => u.active_flg == true).ToList();
            lstStudent = setSearchFilterStudent(lstStudent, studentViewhelper);
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
        }

        public List<gov_person> setSearchFilterStudent(List<gov_person> lststudent, StudentViewhelper studentViewhelper)
        {
            Expression<Func<gov_person, bool>> predicate = PredicateBuilder.False<gov_person>();
            if (studentViewhelper.CourseFilter > 0)
            {
                lststudent = lststudent.Where(n => n.course_id == studentViewhelper.CourseFilter).ToList();
            }
            if (studentViewhelper.SpecializedFilter > 0)
            {
                lststudent = lststudent.Where(n => n.specialized_id == studentViewhelper.SpecializedFilter).ToList();
            }
            if (!String.IsNullOrWhiteSpace(studentViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.full_name != null && d.full_name.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.lop != null && d.lop.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.email != null && d.email.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.phone != null && d.phone.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.department != null && d.department.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.address != null && d.address.ToUpper().Contains(studentViewhelper.KeySearch.ToUpper()));
                lststudent = lststudent.Where(predicate.Compile()).ToList();
            }
            return lststudent;
        }

        [GET("cuu-sinh-vien-tieu-bieu/dang-ky-cuu-sinh-vien")]
        [HttpGet]
        public ActionResult Regist() {
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(c => c.specialized_name).ToList();
            return View();
        }

        [POST("cuu-sinh-vien-tieu-bieu/dang-ky-cuu-sinh-vien")]
        [HttpPost]
        public ActionResult Regist(gov_person personInfo, String captcha)
        {
            if (Request.Files.Count > 0)
            {
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                    var fileFolder = fileSaveFolder;
                    var fileDir = Server.MapPath("/") + fileFolder;
                    if (!System.IO.Directory.Exists(fileDir))
                        System.IO.Directory.CreateDirectory(fileDir);
                    var filePath = fileFolder + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                    System.IO.File.WriteAllBytes(Server.MapPath("/") + filePath, bytes);
                    personInfo.avatar = filePath.Replace("\\", "/");
                }
            }

            personInfo.code = captcha;
            personInfo.entry_datetime = DateTime.Now;
            personInfo.update_datetime = DateTime.Now;
            personInfo.active_flg = false;
            personInfo = _cnttDB.gov_person.Add(personInfo);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                var lstUser = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
                foreach (var user in lstUser)
                {
                    if (SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, user.username, TypeAudit.ManagerStudent))
                    {
                        try
                        {
                            gov_message_system messageInfo = new gov_message_system();
                            messageInfo.entry_datetime = DateTime.Now;
                            messageInfo.type_message = Constant.MESSAGE_TYPE_STUDENT;
                            messageInfo.status = false;
                            messageInfo.content_message = "<strong>" + personInfo.full_name + "</strong>" + Constant.MESSAGE_COMMENT_STUDENT;
                            messageInfo.username = user.username;
                            messageInfo.link = "/admin/student/index";
                            _cnttDB.gov_message_system.Add(messageInfo);
                            _cnttDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ViewData["message"] = "Đã có lỗi xảy ra. Đăng ký thông tin thất bại!";
                        }
                    }
                }
                ViewData["message"] = "Đăng ký thông tin thành công. Thông tin cựu sinh viên của bạn sẽ được kiểm duyệt trước khi cho hiển thị lên website!";
            }
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(c => c.specialized_name).ToList();
            return View();
        }

        [GET("cuu-sinh-vien-tieu-bieu/{name}-{id:int}")]
        [HttpGet]
        public ActionResult Detail(int id)
        {
            ViewData["studentInfo"] = _cnttDB.gov_person.Find(id);
            return View();
        }

        

        [GET("cuu-sinh-vien-tieu-bieu/thay-doi-mat-ma")]
        [HttpGet]
        public ActionResult ChangeCode()
        {
            return View();
        }

        [POST("cuu-sinh-vien-tieu-bieu/thay-doi-mat-ma")]
        [HttpPost]
        public ActionResult ChangeCode(String email, String code_old, String code_new, String re_code_new, String captcha)
        {
            gov_person personInfo = _cnttDB.gov_person.Where(p => p.email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
            if (personInfo != null)
            {
                if (!personInfo.code.Equals(code_old))
                {
                    ViewData["email"] = email;
                    ViewData["err"] = "Có lỗi xảy ra. Mật khẩu nhập vào không chính xác!";
                }
                else
                {
                    personInfo.code = code_new;
                    _cnttDB.SaveChanges();
                }
            }
            else
            {
                ViewData["err"] = "Có lỗi xảy ra. Email nhập vào không chính xác!";
            }
            return View();
        }

        [GET("cuu-sinh-vien-tieu-bieu/{id:int}/xac-nhan-mat-ma")]
        [HttpGet]
        public ActionResult ConfirmCode()
        {
            return View();
        }

        [POST("cuu-sinh-vien-tieu-bieu/{id:int}/xac-nhan-mat-ma")]
        [HttpPost]
        public ActionResult SaveConfirmCode(String email, String code)
        {
            gov_person personInfo = _cnttDB.gov_person.Where(p => p.email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
            if (personInfo != null)
            {
                if (personInfo.code.ToUpper().Equals(code.ToUpper()))
                {
                    //Add cookiee
                    if (Request.Cookies["Cookie_Student_" + personInfo.id.ToString()] == null)
                    {
                        HttpCookie cookieStudent = new HttpCookie("Cookie_Student_" + personInfo.id.ToString());
                        cookieStudent.Value = "True";
                        cookieStudent.Expires = DateTime.Now.AddMinutes(5);
                        Response.Cookies.Add(cookieStudent);
                    }
                    return Redirect("/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu/" + personInfo.id + "/thay-doi-thong-tin");
                }
                else
                {
                    ViewData["err"] = "Đã có lỗi xảy ra. Email hoặc mật mã nhập vào không chính xác!";
                }
            }
            else
            {
                ViewData["err"] = "Đã có lỗi xảy ra. Email hoặc mật mã nhập vào không chính xác!";
                //Sai Email
            }
            return View("ConfirmCode");
        }

        [GET("cuu-sinh-vien-tieu-bieu/{id:int}/thay-doi-thong-tin")]
        [HttpGet]
        public ActionResult ChangeProfile(int id)
        {
            if (Request.Cookies["Cookie_Student_" + id.ToString()] == null)
            {
                return Redirect("/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu/" + id.ToString() + "/xac-nhan-mat-ma");
            }
            ViewData["lstCourse"] = _cnttDB.gov_course.OrderByDescending(c => c.course_name).ToList();
            ViewData["lstSpecialized"] = _cnttDB.gov_specialized.OrderBy(c => c.specialized_name).ToList();
            ViewData["studentInfo"] = _cnttDB.gov_person.Find(id);
            return View();
        }

        [POST("cuu-sinh-vien-tieu-bieu/{id:int}/thay-doi-thong-tin")]
        [HttpPost]
        public ActionResult ChangeProfile(gov_person item)
        {
            if (Request.Cookies["Cookie_Student_" + item.id.ToString()] == null)
            {
                return Redirect("/cuu-sinh-vien/cuu-sinh-vien-tieu-bieu/" + item.id.ToString() + "/xac-nhan-mat-ma");
            }
            gov_person personInfo = _cnttDB.gov_person.Find(item.id);
            personInfo.full_name = item.full_name;
            personInfo.specialized_id = item.specialized_id;
            personInfo.course_id = item.course_id;
            personInfo.lop = item.lop;
            personInfo.phone = item.phone;
            personInfo.email = item.email;
            personInfo.address = item.address;
            personInfo.department = item.department;
            personInfo.description = item.description;
            personInfo.show_address = item.show_address;
            personInfo.show_department = item.show_department;
            personInfo.show_email = item.show_email;
            personInfo.show_shared = item.show_shared;
            personInfo.show_tel = item.show_tel;
            personInfo.update_datetime = DateTime.Now;
            personInfo.active_flg = false;
            if (Request.Files.Count > 0)
            {
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    String dirAvatarOld = personInfo.avatar;
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                    var fileFolder = fileSaveFolder;
                    var fileDir = Server.MapPath("/") + fileFolder;
                    if (!System.IO.Directory.Exists(fileDir))
                        System.IO.Directory.CreateDirectory(fileDir);
                    var filePath = fileFolder + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                    System.IO.File.WriteAllBytes(Server.MapPath("/") + filePath, bytes);
                    personInfo.avatar = filePath.Replace("\\", "/");
                    if (System.IO.File.Exists(Server.MapPath(dirAvatarOld)) && !dirAvatarOld.Equals(Constant.AVATAR_DEFAULT))
                    {
                        System.IO.File.Delete(Server.MapPath(dirAvatarOld));
                    }
                }
            }
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                var lstUser = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
                foreach (var user in lstUser)
                {
                    if (SercurityServices.HasPermission((int)TypeModule.MODULE_CUUSINHVIEN, user.username, TypeAudit.ManagerStudent))
                    {
                        try
                        {
                            gov_message_system messageInfo = new gov_message_system();
                            messageInfo.entry_datetime = DateTime.Now;
                            messageInfo.type_message = Constant.MESSAGE_TYPE_STUDENT;
                            messageInfo.status = false;
                            messageInfo.content_message = "<strong>" + personInfo.full_name + "</strong>" + Constant.MESSAGE_EDIT_STUDENT;
                            messageInfo.username = user.username;
                            messageInfo.link = "/admin/student/index";
                            _cnttDB.gov_message_system.Add(messageInfo);
                            _cnttDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ViewData["err"] = "Đã có lỗi xảy ra. Đăng ký thông tin thất bại!";
                        }
                    }
                }
                ViewData["message"] = "Thay đổi thông tin cựu sinh viên thành công! Thông tin cựu sinh viên của bạn sẽ được kiểm duyệt lại trước khi cho hiển thị lên website!";
                var c = new HttpCookie("Cookie_Student_" + item.id.ToString());
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return View("Success");
        }

        [GET("cuu-sinh-vien-tieu-bieu/{id:int}/lay-lai-mat-ma")]
        [HttpGet]
        public ActionResult ReturnCode()
        {
            return View("LostCode");
        }

        [POST("cuu-sinh-vien-tieu-bieu/{id:int}/lay-lai-mat-ma")]
        [HttpPost]
        public ActionResult ReturnCode(String email)
        {
            var studentInfo = _cnttDB.gov_person.Where(p => p.email.ToUpper().Equals(email.ToUpper())).FirstOrDefault();
            if (studentInfo != null)
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
                String smtpHost = "smtp.gmail.com";
                int smtpPost = 587;
                String emailTo = email;
                String subject = "Lấy lại mật mã cựu sinh viên";
                String body = "<p><strong>Mật mã</strong> dùng để thay đổi thông tin cựu sinh viên của bạn là: <strong>" + studentInfo.code + "</strong>.</p>";
                body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:" + smtpUsername + "' target='_blank'>" + smtpUsername + "</a></p>";
                body += "<p><strong style='font-style: italic;'>Xin cảm ơn!</strong></p>";
                body += "<p></p>";
                EmailServices services = new EmailServices();
                services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
                ViewData["message"] = "Mật mã đã được gửi về mail của bạn. Vui lòng kiểm tra lại và tiếp tục truy cập vào hệ thống!";
            }
            else
            {
                ViewData["err"] = "Email nhập vào không tồn tại trong hệ thống. Vui lòng kiểm tra lại!";
                return View("LostCode");
            }
            
            return View("ConfirmCode");
        }
    }
}
