using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.Delete)
                || !SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.AnswerQuestion))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionViewhelper questionViewhelper = new QuestionViewhelper();
            saveData(questionViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(QuestionViewhelper questionViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.Delete)
                || !SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.AnswerQuestion))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(questionViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            QuestionServices serives = new QuestionServices();
            serives.Id = id.ToString();
            List<QuestionModels> list = serives.select(-1, -1);
            if (list != null && list.Count > 0)
            {
                ViewData["question"] = list.First();
            }
            return View();
        }

        public ActionResult SaveEdit(QuestionModels item)
        {
            QuestionServices serives = new QuestionServices();
            int rs = serives.update(item);
            return Redirect("Index");
        }
        public ActionResult ChangeState(){
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult ChangeState(QuestionViewhelper questionViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionServices questionService;
            if (questionViewhelper.CheckID != null && questionViewhelper.CheckID.Length > 0)
            {
                foreach (var id in questionViewhelper.CheckID)
                {
                    questionService = new QuestionServices();
                    questionService.Id = id.ToString();
                    List<QuestionModels> lstCmt = questionService.select(-1, -1);
                    if (lstCmt != null && lstCmt.Count > 0)
                    {
                        QuestionModels item = lstCmt.First();
                        item.HiddenFlg = questionViewhelper.ChangeState;
                        questionService.update(item);
                    }
                }
            }
            saveData(questionViewhelper);
            return View("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionServices serives = new QuestionServices();
            try
            {
                int rs = serives.delete(id);
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        [HttpPost]
        public ActionResult changeStateAnswer(int[] checkID, Boolean state, int questionId){
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            AnswerServices answerServices;
            if (checkID != null && checkID.Length > 0) {
                foreach (var id in checkID)
                {
                    answerServices = new AnswerServices();
                    answerServices.Id = id.ToString();
                    List<AnswerModels> lst = answerServices.select(-1, -1);
                    if(lst != null && lst.Count > 0){
                        AnswerModels answerModel = lst.First();
                        answerModel.HiddenFlg = state;
                        answerModel.UpdateUsername = "admin";
                        answerModel.UpdateDatetime = DateTime.Now;
                        answerServices.update(answerModel);
                    }
                }
            }
            return Redirect("answer?questionid=" + questionId.ToString());
        }

        public ActionResult Answer(int questionId)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.AnswerQuestion))
            {
                return Redirect("/admin/error/error403");
            }
            AnswerServices answerServices = new AnswerServices();
            answerServices.QuestionId = questionId.ToString();
            List<AnswerModels> lstAnswer = answerServices.select(-1, -1);
            ViewData["lstAnswer"] = lstAnswer;
            ViewData["questionId"] = questionId;
            return View();
        }

        public QuestionViewhelper saveData(QuestionViewhelper questionViewhelper)
        {
            QuestionServices questionServices = new QuestionServices();
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
                foreach (var item in lstQuestion)
                {
                    QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
                    questionCategoryServices.Id = item.CategoryId.ToString();
                    List<QuestionCategoryModels> lst = questionCategoryServices.select(-1, -1);
                    if (lst != null && lst.Count > 0)
                    {
                        item.CategoryName = lst.First().Name;
                    }

                    AnswerServices answerServices = new AnswerServices();
                    answerServices.QuestionId = item.Id.ToString();
                    item.CountAnswer = answerServices.totalCount();
                }
                questionViewhelper.LstQuestion = lstQuestion;
            }
            QuestionCategoryServices questionCategoryServices1 = new QuestionCategoryServices();
            questionViewhelper.LstQuestionCategory = questionCategoryServices1.select(-1, -1);
            ViewData["questionViewhelper"] = questionViewhelper;
            return questionViewhelper;
        }

        public void setSearchFilter(QuestionServices questionServices, QuestionViewhelper questionViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(questionViewhelper.KeySearch))
                questionServices.KeySearch = questionViewhelper.KeySearch;
            if (!String.IsNullOrWhiteSpace(questionViewhelper.ChangeFilter))
                questionServices.QuestionCategoryId = questionViewhelper.ChangeFilter;
        }

    }
}
