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
    public class DistrictController : BaseController
    {
        //
        // GET: /Admin/District/

        /*public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoQuanBanNganh))
            {
                return Redirect("/admin/error/error403");
            }
            DistrictViewhelper districtViewhelper = new DistrictViewhelper();
            saveData(districtViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DistrictViewhelper districtViewhelper) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoQuanBanNganh))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(districtViewhelper);
            return View();
        }

        public ActionResult Regist() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoQuanBanNganh))
            {
                return Redirect("/admin/error/error403");
            }
            DistrictLevelServices levelServices = new DistrictLevelServices();
            List<DistrictLevelModels> lstLevel = levelServices.select(-1, -1);
            ViewData["lstLevel"] = lstLevel;
            return View();
        }

        public ActionResult SaveRegist(DistrictModels item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoQuanBanNganh))
            {
                return Redirect("/admin/error/error403");
            }
            DistrictServices districtServices = new DistrictServices();
            item.UpdateUserId = 1;
            item.UpdateDatetime = DateTime.Now;
            int rs = districtServices.insert(item);
            return RedirectToAction("Regist");
        }

        public DistrictViewhelper saveData(DistrictViewhelper districtViewhelper)
        {
            DistrictServices districtServices = new DistrictServices();
            setSearchFilter(districtServices, districtViewhelper);

            int totalCount = districtServices.totalCount();
            districtViewhelper.TotalCount = totalCount;

            if (districtViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                districtViewhelper.TotalPage = totalPage;
                districtViewhelper.Page = pageTransition(districtViewhelper.Direction, districtViewhelper.Page, totalPage);
                districtViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, districtViewhelper.Page);
                districtViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, districtViewhelper.Page, districtViewhelper.FirstPage);
                districtServices.addOrderBy("order_number", "ASC");
                List<DistrictModels> lstDistrict = districtServices.select(districtViewhelper.Page, Constant.limit);
                districtViewhelper.LstDistrict = lstDistrict;
            }
            ViewData["districtViewhelper"] = districtViewhelper;
            return districtViewhelper;
        }

        public void setSearchFilter(DistrictServices districtServices, DistrictViewhelper districtViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(districtViewhelper.Name))
            {
                districtServices.Name = districtViewhelper.Name.Trim();
            }
            if (!String.IsNullOrWhiteSpace(districtViewhelper.Level))
            {
                districtServices.Level = districtViewhelper.Level;
            }
        }*/
    }
}
