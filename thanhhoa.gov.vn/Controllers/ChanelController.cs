using AttributeRouting;
using AttributeRouting.Web.Mvc;
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
    [RoutePrefix("chanel", Precedence = 1)]
    [RoutePrefix("kenhtin", Precedence = 2)]
    public class ChanelController : BaseController
    {
        //
        // GET: /Chanel/
        [GET("{name}-{chanelId:int}")]
        [HttpGet]
        public ActionResult Index(int chanelId)
        {
            gov_menu menuInfo = _cnttDB.gov_menu.Where(m => m.id == chanelId).FirstOrDefault();
            if (menuInfo == null)
                return Redirect("/error/error404");
            NewViewhelper newViewhelper = new NewViewhelper();
            newViewhelper.MenuInfo = menuInfo;
            newViewhelper.ChanelId = chanelId;
            newViewhelper.Direction = null;
            saveList(newViewhelper);
            ViewData["newViewHelper"] = newViewhelper;
            ViewData["lstMenuChild"] = _cnttDB.gov_menu.Where(m => m.active_flg == true && m.id_parent == chanelId).ToList();
            return View("Index");
        }

        [GET("{name}-{chanelId:int}-trang-{page:int}-{direction}")]
        [HttpGet]
        public ActionResult Pagging(int chanelId, int page, String direction)
        {
            gov_menu menuInfo = _cnttDB.gov_menu.Where(m => m.id == chanelId).FirstOrDefault();
            if (menuInfo == null)
                return Redirect("/error/error404");
            NewViewhelper newViewhelper = new NewViewhelper();
            newViewhelper.MenuInfo = menuInfo;
            newViewhelper.ChanelId = chanelId;
            newViewhelper.Page = page;
            newViewhelper.Direction = direction;
            saveList(newViewhelper);
            ViewData["newViewHelper"] = newViewhelper;
            return View("Index");
        }

        public NewViewhelper saveList(NewViewhelper newViewhelper) {
            List<gov_news> lstNew = _cnttDB.gov_news.Where(n => n.menu_id == newViewhelper.ChanelId && n.active_flg == true).ToList();
            int totalCount = lstNew.Count;
            newViewhelper.TotalCount = totalCount;

            if (newViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                newViewhelper.TotalPage = totalPage;
                newViewhelper.Page = pageTransition(newViewhelper.Direction, newViewhelper.Page, totalPage);
                newViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page);
                newViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page, newViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (newViewhelper.Page - 1) * take;
                newViewhelper.LstNew = lstNew.OrderByDescending(n => n.entry_datetime).Skip(skip).Take(take).ToList();
            }
            return newViewhelper;
        }
    }
}
