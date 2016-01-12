using thanhhoa.gov.vn.Services;
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

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("giang-vien", Precedence = 1)]
    public class DocumentController : BaseController
    {
        //
        // GET: /Document/
        [GET("van-ban/huong-dan-tra-cuu")]
        [HttpGet]
        public ActionResult SearchSkill() {
            return View();
        }

        [GET("van-ban")]
        [HttpGet]
        public ActionResult Index()
        {
            DocumentViewhepler docViewhelper = new DocumentViewhepler();
            docViewhelper.TypeSearch = 1;
            saveData(docViewhelper);
            return View();
        }

        public void saveData(DocumentViewhepler docViewhelper)
        {
            List<gov_doc_draft> lstDocument = _cnttDB.gov_doc_draft.Where(d => d.doc_type == true).ToList();
            int totalCount = lstDocument.Count;
            docViewhelper.TotalCount = totalCount;

            if (docViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                docViewhelper.TotalPage = totalPage;
                docViewhelper.Page = pageTransition(docViewhelper.Direction, docViewhelper.Page, totalPage);
                docViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, docViewhelper.Page);
                docViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, docViewhelper.Page, docViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (docViewhelper.Page - 1) * take;
                docViewhelper.LstDocument = lstDocument.OrderByDescending(d => d.update_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["docViewhelper"] = docViewhelper;
        }

        [HttpPost]
        [POST("van-ban")]
        public ActionResult Index(DocumentViewhepler viewHelper)
        {
            
            if (!String.IsNullOrWhiteSpace(viewHelper.KeySearch))
            {
                List<gov_doc_draft> lstResult = new List<gov_doc_draft>();
                var lucene = new LuceneSerives.LuceneDocuments();
                if (viewHelper.TypeSearch == 1)
                {
                    lstResult = lucene.SearchAdvanced(viewHelper.KeySearch, Boolean.TrueString, "doc_full_text").ToList();
                }
                else
                {
                    lstResult = lucene.SearchAdvanced(viewHelper.KeySearch, Boolean.TrueString, "doc_code_and_summany").ToList();
                }

                int totalCount = lstResult.Count;
                viewHelper.TotalCount = totalCount;
                if (totalCount > 0)
                {
                    int totalPage = pageCalculation(totalCount, Constant.limit);
                    viewHelper.TotalPage = totalPage;
                    viewHelper.Page = pageTransition(viewHelper.Direction, viewHelper.Page, totalPage);
                    viewHelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, viewHelper.Page);
                    viewHelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, viewHelper.Page, viewHelper.FirstPage);
                    int take = Constant.limit;
                    int skip = (viewHelper.Page - 1) * take;
                    viewHelper.LstDocument = lstResult.OrderBy(d => d.score).Skip(skip).Take(take).ToList();
                }
            }
            else
            {
                saveData(viewHelper);
            }

            ViewData["docViewhelper"] = viewHelper;
            return View();
        }

        public FileResult Download(int id)
        {
            gov_doc_draft documentInfo = _cnttDB.gov_doc_draft.Find(id);
            if (documentInfo != null)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("/") + documentInfo.attach_file_path);
                string fileName = documentInfo.attach_file_name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }

        [GET("van-ban/chi-tiet-van-ban-{id:int}")]
        [HttpGet]
        public ActionResult Detail(int id)
        {
            gov_doc_draft documentInfo = _cnttDB.gov_doc_draft.Find(id);
            if (documentInfo == null)
                return Redirect("/error/Error404");
            ViewData["documentInfo"] = documentInfo;
            return View();
        }
    }
}
