using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Controllers;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class DepartmentController : BaseController
    {
        //
        // GET: /Admin/Department/
        [HttpGet]
        public ActionResult Index() {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            DepartmentViewhelper departmentViewhelper = new DepartmentViewhelper();
            saveData(departmentViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DepartmentViewhelper departmentViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(departmentViewhelper);
            return View();
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(gov_department item) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            try
            {
                item = _cnttDB.gov_department.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiCoCauToChuc, Constant.THEM(Constant.ITEM_COCAUTOCHUC, Constant.ID, item.id.ToString()));
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
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            
            ViewData["department"] = _cnttDB.gov_department.Find(id);
            return View();
        }

        public ActionResult SaveEdit(gov_department item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }

            gov_department departmentInfo = _cnttDB.gov_department.Find(item.id);
            if(departmentInfo == null)
                return Redirect("/admin/error/error405");
            departmentInfo.description = item.description;
            departmentInfo.is_home = item.is_home;
            departmentInfo.link = item.link;
            departmentInfo.name = item.name;
            departmentInfo.order_number = item.order_number;
            departmentInfo.update_username = Session.getCurrentUser().username;
            departmentInfo.update_datetime = DateTime.Now;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaCoCauToChuc, Constant.CHINHSUA(Constant.ITEM_COCAUTOCHUC, Constant.ID, item.id.ToString()));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                } else{
                    TempData["err"] = Constant.EDIT_ERR;
                }
            } catch(Exception re){
                TempData["err"] = Constant.EDIT_ERR;
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.CoCauToChuc))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_department departmentInfo = _cnttDB.gov_department.Find(id);
                if (departmentInfo == null)
                    return Redirect("/admin/error/error405");
                else
                {
                    _cnttDB.gov_department.Remove(departmentInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        insertHistory(AccessType.xoaCoCauToChuc, Constant.XOA(Constant.ITEM_COCAUTOCHUC, Constant.ID, id.ToString()));
                        TempData["err"] = Constant.DELETE_SUCCESSFULL;
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        public DepartmentViewhelper saveData(DepartmentViewhelper departmentViewhelper)
        {
            List<gov_department> lstDepartment = _cnttDB.gov_department.ToList();
            lstDepartment = setSearchFilter(lstDepartment, departmentViewhelper);

            int totalCount = lstDepartment.Count;
            departmentViewhelper.TotalCount = totalCount;

            if (departmentViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                departmentViewhelper.TotalPage = totalPage;
                departmentViewhelper.Page = pageTransition(departmentViewhelper.Direction, departmentViewhelper.Page, totalPage);
                departmentViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, departmentViewhelper.Page);
                departmentViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, departmentViewhelper.Page, departmentViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (departmentViewhelper.Page - 1) * take;
                departmentViewhelper.LstDepartment = lstDepartment.OrderBy(d => d.order_number).Skip(skip).Take(take).ToList();
            }
            ViewData["departmentViewhelper"] = departmentViewhelper;
            return departmentViewhelper;
        }

        public List<gov_department> setSearchFilter(List<gov_department> lstDepartment, DepartmentViewhelper departmentViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(departmentViewhelper.KeySearch)) { }
            return lstDepartment;
        }
    }
}
