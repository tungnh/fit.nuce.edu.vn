using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        //
        // GET: /Admin/Answer/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Regist(int questionId) {
            QuestionServices questionServices = new QuestionServices();
            questionServices.Id = questionId.ToString();
            List<QuestionModels> lst = questionServices.select(-1, -1);
            if (lst != null && lst.Count > 0) {
                ViewData["questionInfo"] = lst.First();
            }

            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(AnswerModels item) {
            item.HiddenFlg = false;
            item.AnswerUsername = "admin";
            item.AnswerDatetime = DateTime.Now;
            item.UpdateDatetime = DateTime.Now;
            item.UpdateUsername = "admin";

            AnswerServices answerServices = new AnswerServices();
            answerServices.insert(item);

            QuestionServices questionServices = new QuestionServices();
            questionServices.Id = item.QuestionId.ToString();
            List<QuestionModels> lst = questionServices.select(-1, -1);
            if(lst != null && lst.Count > 0){
                QuestionModels questionInfo = new QuestionModels();
                questionInfo =  lst.First();
                questionInfo.HiddenFlg = false;
                questionInfo.QuestionDatetime = DateTime.Now;
                questionServices = new QuestionServices();
                questionServices.update(questionInfo);
            }

            return Redirect("/admin/question/answer?questionid=" + item.QuestionId.ToString());
        }
    }
}
