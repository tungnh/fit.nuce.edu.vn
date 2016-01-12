using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using thanhhoa.gov.vn.Controllers;
using thanhhoa.gov.vn.Filters;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;
using WebMatrix.WebData;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
        //
        // GET: /Admin/Account/
        [AllowAnonymous]
        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LoginModel model, string returnUrl)
        {
            //if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            //{
            if (ModelState.IsValid)
            {
                gov_user userInfo = _cnttDB.gov_user.Where(u => u.username.Equals(model.UserName)).FirstOrDefault();
                if (userInfo != null)
                {
                    if (userInfo.password.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5")))
                    {
                        Session.SetCurrentUser(userInfo);
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        gov_access_history accessHistory = new gov_access_history();
                        accessHistory.time_access = DateTime.Now;
                        accessHistory.type_access = (int)AccessType.dangNhap;
                        accessHistory.username_access = userInfo.username;
                        accessHistory.description = Constant.DANG_NHAP;
                        accessHistory.client_info = Utils.GetLanIPAddress();
                        accessHistory.public_info = Utils.GetPublicIPAddress();
                        _cnttDB.gov_access_history.Add(accessHistory);
                        _cnttDB.SaveChanges();
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu nhập vào không chính xác.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu nhập vào không chính xác.");
                    return View(model);
                }
            }
            //}

            // If we got this far, something failed, redisplay form
            //ModelState.AddModelError("", "Tài khoản và mật khẩu không được để trống!");
            return View(model);
        }

        [Authorize]
        public ActionResult LogOut()
        {
            ViewBag.ReturnUrl = null;
            FormsAuthentication.SignOut();
            gov_access_history accessHistory = new gov_access_history();
            if (Session.getCurrentUser() != null) {
                accessHistory.time_access = DateTime.Now;
                accessHistory.type_access = (int)AccessType.dangXuat;
                accessHistory.username_access = Session.getCurrentUser().username;
                accessHistory.description = Constant.DANG_XUAT;
                accessHistory.client_info = Utils.GetLanIPAddress();
                accessHistory.public_info = Utils.GetPublicIPAddress();
                _cnttDB.gov_access_history.Add(accessHistory);
                _cnttDB.SaveChanges();
            }
            Session.SetCurrentUser(null);
            return Redirect("Logon");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/Admin/Home/Index");
            }
        }
    }
}
