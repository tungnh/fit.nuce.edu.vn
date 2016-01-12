using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Controllers
{
    public class AnswerController : BaseController
    {
        private string fileSaveFolder = "Upload/Question";
        //
        // GET: /Answer/
        public ActionResult Index()
        {
            QuestionViewhelper questionViewhelper = new QuestionViewhelper();
            saveData(questionViewhelper);
            return View();
        }
        [HttpPost]
        public ActionResult Index(QuestionViewhelper questionViewhelper)
        {
            saveData(questionViewhelper);
            return View();
        }

        public QuestionViewhelper saveData(QuestionViewhelper questionViewhelper)
        {
            QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
            questionCategoryServices.addOrderBy("order_number", Constant.ORDER_ASC);
            List<QuestionCategoryModels> lstQuestionCategory = questionCategoryServices.select(-1, -1);
            questionViewhelper.LstQuestionCategory = lstQuestionCategory;

            QuestionServices questionServices = new QuestionServices();
            questionServices.HiddenFlg = Boolean.FalseString;
            questionServices.QuestionInAnswer = "TRUE";
            setSearchFilter(questionServices, questionViewhelper);

            int totalCount = questionServices.totalCount();
            questionViewhelper.TotalCount = totalCount;

            if (questionViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                questionViewhelper.TotalPage = totalPage;
                questionViewhelper.Page = pageTransition(questionViewhelper.Direction, questionViewhelper.Page, totalPage);
                questionViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, questionViewhelper.Page);
                questionViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, questionViewhelper.Page, questionViewhelper.FirstPage);
                questionServices.addOrderBy("question_datetime", "DESC");
                List<QuestionModels> lstQuestion = questionServices.select(questionViewhelper.Page, Constant.limit);
                questionViewhelper.LstQuestion = lstQuestion;
            }
            ViewData["questionViewhelper"] = questionViewhelper;
            return questionViewhelper;
        }

        public void setSearchFilter(QuestionServices questionServices, QuestionViewhelper questionViewhelper)
        {
            if (!String.IsNullOrEmpty(questionViewhelper.KeySearch))
                questionServices.KeySearch = questionViewhelper.KeySearch;
            if (questionViewhelper.QuestionCategoryId != 0)
                questionServices.QuestionCategoryId = questionViewhelper.QuestionCategoryId.ToString();
        }

        public ActionResult Regist()
        {
            QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
            questionCategoryServices.addOrderBy("order_number", Constant.ORDER_ASC);
            List<QuestionCategoryModels> lstQuestionCategory = questionCategoryServices.select(-1, -1);
            ViewData["lstQuestionCategory"] = lstQuestionCategory;

            ViewData["questionModel"] = new QuestionModels();
            return View();
        }

        public ActionResult SaveRegist(QuestionModels item, String captcha) {
            ViewData["questionModel"] = item;
            if (captcha != null)
            {
                string getCaptcha = HttpContext.Session["captcha"].ToString();
                if (captcha == getCaptcha)
                    ViewData["captcha"] = "You're right";
                else {
                    QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
                    questionCategoryServices.addOrderBy("order_number", Constant.ORDER_ASC);
                    List<QuestionCategoryModels> lstQuestionCategory = questionCategoryServices.select(-1, -1);
                    ViewData["lstQuestionCategory"] = lstQuestionCategory;
                    ViewData["questionModel"] = item;
                    return View("Regist");
                }
            }
            if (Request.Files.Count > 0)
            {
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    item.AttachFileName = fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                    var fileFolder = fileSaveFolder;
                    var fileDir = Server.MapPath("/") + fileFolder;
                    if (!System.IO.Directory.Exists(fileDir)) System.IO.Directory.CreateDirectory(fileDir);
                    var filePath = fileDir + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                    System.IO.File.WriteAllBytes(filePath, bytes);
                    item.AttachFilePath = filePath.Replace("\\", "/");
                }
            }
            item.HiddenFlg = true;
            item.QuestionDatetime = DateTime.Now;

            QuestionServices questionServices = new QuestionServices();
            questionServices.insert(item);
            return Redirect("Index");
        }

        public ActionResult Detail(int id)
        {
            QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
            questionCategoryServices.addOrderBy("order_number", Constant.ORDER_ASC);
            List<QuestionCategoryModels> lstQuestionCategory = questionCategoryServices.select(-1, -1);
            ViewData["lstQuestionCategory"] = lstQuestionCategory;

            QuestionServices questionServices;
            questionServices = new QuestionServices();

            questionServices.Id = id.ToString();
            questionServices.HiddenFlg = Boolean.FalseString;
            List<QuestionModels> lst = questionServices.select(-1, -1);
            if (lst != null && lst.Count > 0) {
                ViewData["questionInfo"] = lst.First();
                questionServices = new QuestionServices();
                questionServices.QuestionInAnswer = "TRUE";
                questionServices.QuestionCategoryId = lst.First().CategoryId.ToString();
                ViewData["lstQuestion"] = questionServices.select(-1, -1);

                AnswerServices answerServices = new AnswerServices();
                answerServices.QuestionId = lst.First().Id.ToString();
                List<AnswerModels> lstAnswer = answerServices.select(-1, -1);
                //if (lstAnswer != null && lstAnswer.Count > 0) {
                UserServices userServices;
                foreach (var item in lstAnswer)
                {
                    userServices = new UserServices();
                    userServices.Username = item.AnswerUsername;
                    item.UserInfo = userServices.select(-1, -1).FirstOrDefault();
                }
                ViewData["lstAnswer"] = lstAnswer;
            }
            else
            {
                return Redirect("/error/error404");
            }
            return View();
        }

        public CaptchaResult getCaptcha()
        {
            string captchaText = Captcha.GenerateRandomCode();
            HttpContext.Session.Add("captcha", captchaText);
            return new CaptchaResult(captchaText);
        }

        public String checkCaptcha(String captcha)
        {
            return HttpContext.Session["captcha"].ToString();
        }
    }
}
