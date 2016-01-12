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
    public class MenuController : BaseController
    {
        //
        // GET: /Admin/Menu/
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }

            MenuViewhelper menuViewhelper = new MenuViewhelper();
            saveListMenu(menuViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(MenuViewhelper menuViewhelper) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            saveListMenu(menuViewhelper);
            return View();
        }

        public ActionResult Regist() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            List<gov_menu> lstMenu = new List<gov_menu>();
            lstMenu = _cnttDB.gov_menu.OrderBy(m => m.order_number).ToList();
            return View(lstMenu);
        }

        public ActionResult SaveRegist(gov_menu item, Boolean typeLink)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            item.active_flg = true;
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                _cnttDB.gov_menu.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (typeLink)
                {
                    item.link = "/chanel/index?chanelId=" + item.id.ToString();
                    _cnttDB.SaveChanges();
                }
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiDanhMuc, Constant.THEM(Constant.ITEM_DANHMUC, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Thêm mới thông tin thành công!";
                } else
                    TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
            }
            catch(Exception ex){
                TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["menu"] = _cnttDB.gov_menu.Where(m => m.id == id).FirstOrDefault();
            ViewData["lstMenu"] = _cnttDB.gov_menu.Where(m => m.id != id).OrderBy(m => m.order_number).ToList();
            return View();
        }

        public ActionResult SaveEdit(gov_menu item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            gov_menu menuInfo = _cnttDB.gov_menu.Find(item.id);
            menuInfo.avatar = item.avatar;
            menuInfo.description = item.description;
            menuInfo.title = item.title;
            menuInfo.id_parent = item.id_parent;
            menuInfo.link = item.link;
            menuInfo.ishome = item.ishome;
            menuInfo.isleft = item.isleft;
            menuInfo.order_number = item.order_number;
            menuInfo.update_datetime = DateTime.Now;
            menuInfo.update_username = Session.getCurrentUser().username;
            menuInfo.active_flg = item.active_flg;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaDanhMuc, Constant.CHINHSUA(Constant.ITEM_DANHMUC, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Cập nhật thông tin thành công!";
                }
                else
                {
                    TempData["message"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
                }
            } catch(Exception ex){
                TempData["err"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.DANHMUC, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            gov_menu menuInfo = _cnttDB.gov_menu.Find(id);
            if (menuInfo != null)
            {
                try {
                    _cnttDB.gov_menu.Remove(menuInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaDanhMuc, Constant.XOA(Constant.ITEM_DANHMUC, Constant.ID, id.ToString()));
                        TempData["message"] = "Xóa thông tin thành công!";
                    }
                    else
                    {
                        TempData["err"] = "Đã có lỗi xảy ra. Xóa thông tin thất bại!";
                    }
                }
                catch (Exception ex) {
                    return Redirect("/admin/error/error404");
                }
            }
            else
            {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }

        public MenuViewhelper saveListMenu(MenuViewhelper menuViewhelper) {
            List<gov_menu> lstMenu = _cnttDB.gov_menu.ToList();
            lstMenu = setSearchFilter(lstMenu, menuViewhelper);

            int totalCount = lstMenu.Count;
            menuViewhelper.TotalCount = totalCount;

            if (menuViewhelper.TotalCount > 0) {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                menuViewhelper.TotalPage = totalPage;
                menuViewhelper.Page = pageTransition(menuViewhelper.Direction, menuViewhelper.Page, totalPage);
                menuViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, menuViewhelper.Page);
                menuViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, menuViewhelper.Page, menuViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (menuViewhelper.Page - 1) * take;
                menuViewhelper.LstMenuModels = lstMenu.OrderBy(m => m.order_number).Skip(skip).Take(take).ToList();
            }
            ViewData["menuViewhelper"] = menuViewhelper; 
            return menuViewhelper;
        }

        public List<gov_menu> setSearchFilter(List<gov_menu> lstMenu, MenuViewhelper menuViewhelper)
        {
            Expression<Func<gov_menu, bool>> predicate = PredicateBuilder.False<gov_menu>();
            if (!String.IsNullOrWhiteSpace(menuViewhelper.KeySearch)) {
                predicate = predicate.Or(d => d.title != null && d.title.ToUpper().Contains(menuViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.description != null && d.description.ToUpper().Contains(menuViewhelper.KeySearch.ToUpper()));
                lstMenu = lstMenu.Where(predicate.Compile()).ToList();
            }
            return lstMenu;
        }
    }
}
