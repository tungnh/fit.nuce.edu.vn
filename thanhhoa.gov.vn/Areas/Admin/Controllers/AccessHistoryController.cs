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
    public class AccessHistoryController : BaseController
    {
        //
        // GET: /Admin/AccessHistory/
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.TruyCapHeThong))
            {
                return Redirect("/admin/error/error403");
            }
            AccessHistoryViewhelper accessHistoryViewhelper = new AccessHistoryViewhelper();
            saveData(accessHistoryViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(AccessHistoryViewhelper accessHistoryViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.TruyCapHeThong))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(accessHistoryViewhelper);
            return View();
        }

        public AccessHistoryViewhelper saveData(AccessHistoryViewhelper accessHistoryViewhelper)
        {
            List<gov_access_module> lstModule = _cnttDB.gov_access_module.ToList();
            List<gov_access_type> lstType = _cnttDB.gov_access_type.ToList();
            if (accessHistoryViewhelper.AccessModule != 0) {
                lstType = _cnttDB.gov_access_module.Find(accessHistoryViewhelper.AccessModule).gov_access_type.ToList();
            }
            accessHistoryViewhelper.LstAccessType = lstType;
            accessHistoryViewhelper.LstAccessModule = lstModule;

            List<gov_access_history> lstAccess = _cnttDB.gov_access_history.ToList();
            lstAccess = setSearchFilter(lstAccess, accessHistoryViewhelper);
            int totalCount = lstAccess.Count;
            accessHistoryViewhelper.TotalCount = totalCount;

            if (accessHistoryViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                accessHistoryViewhelper.TotalPage = totalPage;
                accessHistoryViewhelper.Page = pageTransition(accessHistoryViewhelper.Direction, accessHistoryViewhelper.Page, totalPage);
                accessHistoryViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, accessHistoryViewhelper.Page);
                accessHistoryViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, accessHistoryViewhelper.Page, accessHistoryViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (accessHistoryViewhelper.Page - 1) * take;
                accessHistoryViewhelper.LstAccessHistory = lstAccess.OrderByDescending(u => u.time_access).Skip(skip).Take(take).ToList();
            }
            ViewData["accessHistoryViewhelper"] = accessHistoryViewhelper;
            return accessHistoryViewhelper;
        }
        public List<gov_access_history> setSearchFilter(List<gov_access_history> lstAccess, AccessHistoryViewhelper accessHistoryViewhelper)
        {
            Expression<Func<gov_access_history, bool>> predicate = PredicateBuilder.False<gov_access_history>();
            if (!String.IsNullOrWhiteSpace(accessHistoryViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.description != null && d.description.ToUpper().Contains(accessHistoryViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.client_info != null && d.client_info.ToUpper().Contains(accessHistoryViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.public_info != null && d.public_info.ToUpper().Contains(accessHistoryViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.username_access != null && d.username_access.ToUpper().Contains(accessHistoryViewhelper.KeySearch.ToUpper()));
                lstAccess = lstAccess.Where(predicate.Compile()).ToList();
            }
            if (accessHistoryViewhelper.AccessModule != 0)
            {
                var lst = (from a in _cnttDB.gov_access_history
                           join t in _cnttDB.gov_access_type on a.type_access equals t.id
                           join m in _cnttDB.gov_access_module on t.module_id equals m.id
                           where m.id == accessHistoryViewhelper.AccessModule
                           select a);
                lstAccess = lst.ToList();
            }
            if (accessHistoryViewhelper.AccessType != 0) {
                gov_access_type aT = _cnttDB.gov_access_type.Find(accessHistoryViewhelper.AccessType);
                if (aT != null) { 
                    if(aT.gov_access_module.id == accessHistoryViewhelper.AccessModule || accessHistoryViewhelper.AccessModule == 0)
                        lstAccess = lstAccess.Where(a => a.type_access == accessHistoryViewhelper.AccessType).ToList();
                }
            }
            
            return lstAccess;
        }
    }
}
