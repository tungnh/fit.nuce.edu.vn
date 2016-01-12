using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            UserViewhelper UserViewhelper = new UserViewhelper();
            saveData(UserViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserViewhelper UserViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(UserViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult Delete(String username)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            try {
                gov_user userInnfo = _cnttDB.gov_user.Find(username);
                if (userInnfo == null)
                    return Redirect("/admin/error/error405");
                _cnttDB.gov_user.Remove(userInnfo);
                int rs = _cnttDB.SaveChanges();
                if(rs > 0){
                    insertHistory(AccessType.xoaUser, Constant.XOA(Constant.ITEM_USER, Constant.USERNAME, username));
                    TempData["message"] = Constant.DELETE_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.EDIT_ERR;
                }
            } catch(Exception ex){
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["lstRole"] = _cnttDB.gov_role.ToList();
            ViewData["lstDepartment"] = _cnttDB.gov_department.ToList();
            return View();
        }

        public ActionResult SaveRegist(gov_user item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            item.password = FormsAuthentication.HashPasswordForStoringInConfigFile(item.password, "MD5");
            item.entry_datetime = DateTime.Now;
            item.entry_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item.update_user_name = Session.getCurrentUser().username;
            item.active_flg = true;
            item.hidden_flg = false;
            try
            {
                _cnttDB.gov_user.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiUser, Constant.THEM(Constant.ITEM_USER, Constant.USERNAME, item.username));
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

        public String checkUsername(String username)
        {
            gov_user userInfo = _cnttDB.gov_user.Where(u => u.username.Equals(username)).FirstOrDefault();
            if (userInfo != null)
                return null;
            return username;
        }

        public ActionResult Edit(String id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["lstRole"] = _cnttDB.gov_role.ToList();
            ViewData["userInfo"] = _cnttDB.gov_user.Find(id);
            ViewData["lstDepartment"] = _cnttDB.gov_department.ToList();
            return View();
        }

        public ActionResult SaveEdit(gov_user item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            gov_user userInfo = _cnttDB.gov_user.Find(item.username);
            userInfo.address = item.address;
            userInfo.birth_day = item.birth_day;
            userInfo.department_id = item.department_id;
            userInfo.email = item.email;
            userInfo.family_name = item.family_name;
            userInfo.first_name = item.first_name;
            userInfo.mobile = item.mobile;
            userInfo.active_flg = item.active_flg;
            userInfo.role_id = item.role_id;
            userInfo.phone = item.phone;
            userInfo.sex = item.sex;
            userInfo.is_shared = item.is_shared;
            userInfo.avatar = item.avatar;
            if(!userInfo.password.Equals(item.password))
                userInfo.password = FormsAuthentication.HashPasswordForStoringInConfigFile(item.password, "MD5");
            userInfo.update_datetime = DateTime.Now;
            userInfo.update_user_name = Session.getCurrentUser().username;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaUser, Constant.CHINHSUA(Constant.ITEM_USER, Constant.USERNAME, item.username));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.EDIT_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.EDIT_ERR;
            }
            return Redirect("Index");
        }

        public UserViewhelper saveData(UserViewhelper userViewhelper)
        {
            List<gov_user> lstUser = _cnttDB.gov_user.Where(u => u.hidden_flg == false).ToList();
            lstUser = setSearchFilter(lstUser, userViewhelper);
            int totalCount = lstUser.Count;
            userViewhelper.TotalCount = totalCount;

            if (userViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                userViewhelper.TotalPage = totalPage;
                userViewhelper.Page = pageTransition(userViewhelper.Direction, userViewhelper.Page, totalPage);
                userViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, userViewhelper.Page);
                userViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, userViewhelper.Page, userViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (userViewhelper.Page - 1) * take;
                userViewhelper.LstUser = lstUser.OrderByDescending(u => u.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["userViewhelper"] = userViewhelper;
            return userViewhelper;
        }
        public List<gov_user> setSearchFilter(List<gov_user> lstUser, UserViewhelper userViewhelper)
        {
            Expression<Func<gov_user, bool>> predicate = PredicateBuilder.False<gov_user>();
            if (!String.IsNullOrWhiteSpace(userViewhelper.KeySearch)) {
                predicate = predicate.Or(d => d.username != null && d.username.ToUpper().Contains(userViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => (d.family_name  + " " + d.first_name).ToUpper().Contains(userViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.email != null && d.email.ToUpper().Contains(userViewhelper.KeySearch.ToUpper()));
                lstUser = lstUser.Where(predicate.Compile()).ToList();
            }
            if (!String.IsNullOrWhiteSpace(userViewhelper.FilterActive)) {
                lstUser = lstUser.Where(u => u.active_flg == Convert.ToBoolean(userViewhelper.FilterActive)).ToList();
            }
            return lstUser;
        }
    }
}
