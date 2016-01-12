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
    [Authorize]
    public class QuestionCategoryController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionCategoryViewhelper questionCategoryViewhelper = new QuestionCategoryViewhelper();
            saveData(questionCategoryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(QuestionCategoryViewhelper questionCategoryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(questionCategoryViewhelper);
            return View();
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(QuestionCategoryModels item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionCategoryServices serives = new QuestionCategoryServices();
            item.UpdateUsername = "admin";
            item.UpdateDatetime = DateTime.Now;
            int rs = serives.insert(item);
            return RedirectToAction("Regist");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionCategoryServices serives = new QuestionCategoryServices();
            serives.Id = id.ToString();
            List<QuestionCategoryModels> list = serives.select(-1, -1);
            if (list != null && list.Count > 0)
            {
                ViewData["questionCategory"] = list.First();
            }
            return View();
        }

        public ActionResult SaveEdit(QuestionCategoryModels item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionCategoryServices serives = new QuestionCategoryServices();
            item.UpdateUsername = "admin";
            item.UpdateDatetime = DateTime.Now;
            int rs = serives.update(item);
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HOIDAP, Session.getCurrentUser().username, TypeAudit.QuestionCategory))
            {
                return Redirect("/admin/error/error403");
            }
            QuestionCategoryServices serives = new QuestionCategoryServices();
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

        public QuestionCategoryViewhelper saveData(QuestionCategoryViewhelper questionCategoryViewhelper)
        {
            QuestionCategoryServices questionCategoryServices = new QuestionCategoryServices();
            setSearchFilter(questionCategoryServices, questionCategoryViewhelper);

            int totalCount = questionCategoryServices.totalCount();
            questionCategoryViewhelper.TotalCount = totalCount;

            if (questionCategoryViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                questionCategoryViewhelper.TotalPage = totalPage;
                questionCategoryViewhelper.Page = pageTransition(questionCategoryViewhelper.Direction, questionCategoryViewhelper.Page, totalPage);
                questionCategoryViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, questionCategoryViewhelper.Page);
                questionCategoryViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, questionCategoryViewhelper.Page, questionCategoryViewhelper.FirstPage);
                questionCategoryServices.addOrderBy("order_number", "ASC");
                List<QuestionCategoryModels> lstQuestionCategory = questionCategoryServices.select(questionCategoryViewhelper.Page, Constant.limit);
                questionCategoryViewhelper.LstQuestionCategory = lstQuestionCategory;
            }
            ViewData["questionCategoryViewhelper"] = questionCategoryViewhelper;
            return questionCategoryViewhelper;
        }

        public void setSearchFilter(QuestionCategoryServices QuestionCategoryServices, QuestionCategoryViewhelper QuestionCategoryViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(QuestionCategoryViewhelper.KeySearch))
                QuestionCategoryServices.KeySearch = QuestionCategoryViewhelper.KeySearch;
        }

    }
}
