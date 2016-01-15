using Groupdocs.Web.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Controllers
{
    public class TestScoreController : BaseController
    {
        //
        // GET: /TestScore/
        [HttpGet]
        public ActionResult Index()
        {
            TestscoreViewhelper testscoreViewhelper = new TestscoreViewhelper();
            saveData(testscoreViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(TestscoreViewhelper testscoreViewhelper)
        {
            saveData(testscoreViewhelper);
            return View();
        }

        public ActionResult ViewBrowser() {
            return View();
        }

        public TestscoreViewhelper saveData(TestscoreViewhelper testscoreViewhelper)
        {
            List<gov_testscore> lstTestscore = _cnttDB.gov_testscore.ToList();
            lstTestscore = setSearchFilter(lstTestscore, testscoreViewhelper);

            int totalCount = lstTestscore.Count;
            testscoreViewhelper.TotalCount = totalCount;

            if (testscoreViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                testscoreViewhelper.TotalPage = totalPage;
                testscoreViewhelper.Page = pageTransition(testscoreViewhelper.Direction, testscoreViewhelper.Page, totalPage);
                testscoreViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, testscoreViewhelper.Page);
                testscoreViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, testscoreViewhelper.Page, testscoreViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (testscoreViewhelper.Page - 1) * take;
                testscoreViewhelper.LstTestscore = lstTestscore.OrderByDescending(m => m.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["testscoreViewhelper"] = testscoreViewhelper;
            return testscoreViewhelper;
        }

        public List<gov_testscore> setSearchFilter(List<gov_testscore> lstTestscore, TestscoreViewhelper TestscoreViewhelper)
        {
            Expression<Func<gov_testscore, bool>> predicate = PredicateBuilder.False<gov_testscore>();
            if (!String.IsNullOrWhiteSpace(TestscoreViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.test_class != null && d.test_class.ToUpper().Contains(TestscoreViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.test_name != null && d.test_name.ToUpper().Contains(TestscoreViewhelper.KeySearch.ToUpper()));
                lstTestscore = lstTestscore.Where(predicate.Compile()).ToList();
            }
            return lstTestscore;
        }

        public ActionResult Viewer(String Doc)
        {
            var filePath = FileRepository.RootStorage + "\\vs.bin";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            if (String.IsNullOrWhiteSpace(Doc))
            {
                throw new HttpException(403, "Document name is invalid");
            }
            if (FileRepository.IsPresent(Doc) == false)
            {
                throw new HttpException(404, "Document is absent");
            }
            ViewData["fileName"] = Doc;
            return View();
        }

        public ActionResult ViewerImages(int id)
        {
            ViewData["lstImages"] = _cnttDB.gov_testscore.Find(id);
            return View();
        }
    }
}
