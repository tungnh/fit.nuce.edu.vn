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
    public class RoleController : BaseController
    {
        //
        // GET: /Admin/Role/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            RoleViewhelper item = new RoleViewhelper();
            saveDataRole(item);
            return View();
        }

        [HttpPost]
        public ActionResult Index(RoleViewhelper item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            saveDataRole(item);
            return View();
        }

        [HttpGet]
        public ActionResult Regist() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(gov_role item)
        {
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                item = _cnttDB.gov_role.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiChucVu, Constant.THEM(Constant.ITEM_CHUCVU, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.REGIST_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.REGIST_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.REGIST_ERR;
            }
            return Redirect("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["roleInfo"] = _cnttDB.gov_role.Find(id);
            return View();
        }
        [HttpPost]
        public ActionResult Edit(gov_role item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            gov_role roleInfo = _cnttDB.gov_role.Find(item.id);
            if (roleInfo != null)
            {
                roleInfo.role_name = item.role_name;
                roleInfo.update_datetime = DateTime.Now;
                roleInfo.update_username = Session.getCurrentUser().username;
                try
                {
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.chinhSuaChuVu, Constant.CHINHSUA(Constant.ITEM_CHUCVU, Constant.ID, item.id.ToString()));
                        TempData["message"] = Constant.EDIT_SUCCESSFULL;
                    }
                    else
                    {
                        TempData["err"] = Constant.EDIT_ERR;
                    }
                } catch(Exception ex){
                    TempData["err"] = Constant.EDIT_ERR;
                }
            }
            return Redirect("Index");
        }

        public ActionResult DeleteRole(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.ChucVu))
            {
                return Redirect("/admin/error/error403");
            }
            gov_role roleInfo = _cnttDB.gov_role.Find(id);
            if(roleInfo != null){
                try
                {
                    _cnttDB.gov_role.Remove(roleInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaChucVu, Constant.XOA(Constant.ITEM_CHUCVU, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                    else
                    {
                        TempData["err"] = Constant.DELETE_ERR;
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/error/error404");
                }
            }
            else
            {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }

        public RoleViewhelper saveDataRole(RoleViewhelper roleViewhelper)
        {
            List<gov_role> lstRole = _cnttDB.gov_role.ToList();
            lstRole = setSearchFilter(lstRole, roleViewhelper);
            int totalCount = lstRole.Count;
            roleViewhelper.TotalCount = totalCount;

            if (roleViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                roleViewhelper.TotalPage = totalPage;
                roleViewhelper.Page = pageTransition(roleViewhelper.Direction, roleViewhelper.Page, totalPage);
                roleViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, roleViewhelper.Page);
                roleViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, roleViewhelper.Page, roleViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (roleViewhelper.Page - 1) * take;
                roleViewhelper.LstRole = lstRole.OrderBy(u => u.role_name).Skip(skip).Take(take).ToList();
            }
            ViewData["roleViewhelper"] = roleViewhelper;
            return roleViewhelper;
        }

        public List<gov_role> setSearchFilter(List<gov_role> lstRole, RoleViewhelper roleViewhelper)
        {
            Expression<Func<gov_role, bool>> predicate = PredicateBuilder.False<gov_role>();
            if (!String.IsNullOrWhiteSpace(roleViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.role_name != null && d.role_name.ToUpper().Contains(roleViewhelper.KeySearch.ToUpper()));
                lstRole = lstRole.Where(predicate.Compile()).ToList();
            }
            return lstRole;
        }
    }
}
