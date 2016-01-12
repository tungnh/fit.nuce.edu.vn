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
    public class AdvertiseController : BaseController
    {
        //
        // GET: /Admin/Advertise/
        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            AdvertiseViewhelper advertiseViewhelper = new AdvertiseViewhelper();
            saveData(advertiseViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdvertiseViewhelper advertiseViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(advertiseViewhelper);
            return View();
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }


        public ActionResult SaveRegist(gov_advertise item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            item.active_flg = true;
            item.entry_username = Session.getCurrentUser().username;
            item.entry_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item = _cnttDB.gov_advertise.Add(item);
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiQuangCao, Constant.THEM(Constant.ITEM_QUANGCAO, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Thêm mới thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
            }
            return Redirect("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["advertiseInfo"] = _cnttDB.gov_advertise.Find(id);
            return View();
        }

        public ActionResult SaveEdit(gov_advertise item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            gov_advertise advertiseInfo = _cnttDB.gov_advertise.Find(item.id);
            advertiseInfo.link = item.link;
            advertiseInfo.location = item.location;
            advertiseInfo.note = item.note;
            advertiseInfo.order_number = item.order_number;
            advertiseInfo.title = item.title;
            advertiseInfo.type_link = item.type_link;
            advertiseInfo.end_date = item.end_date;
            advertiseInfo.department = item.department;
            advertiseInfo.begin_date = item.begin_date;
            advertiseInfo.attach_file = item.attach_file;
            advertiseInfo.active_flg = item.active_flg;
            advertiseInfo.update_datetime = DateTime.Now;
            advertiseInfo.update_username = Session.getCurrentUser().username;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaQuangCao, Constant.CHINHSUA(Constant.ITEM_QUANGCAO, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Cập nhật thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
                }
            } catch(Exception ex){
                TempData["err"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_HETHONG, Session.getCurrentUser().username, TypeAudit.QuangCao))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                gov_advertise advertiseInnfo = _cnttDB.gov_advertise.Find(id);
                if (advertiseInnfo == null)
                    return Redirect("/admin/error/error405");
                _cnttDB.gov_advertise.Remove(advertiseInnfo);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.xoaQuangCao, Constant.XOA(Constant.ITEM_QUANGCAO, Constant.ID, id.ToString()));
                    TempData["message"] = "Xóa thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Đã có lỗi xảy ra. Xóa thông tin thất bại!";
                }
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            return Redirect("Index");
        }

        public AdvertiseViewhelper saveData(AdvertiseViewhelper advertiseViewhelper)
        {
            List<gov_advertise> lstAdvertise = _cnttDB.gov_advertise.ToList();
            lstAdvertise = setSearchFilter(lstAdvertise, advertiseViewhelper);
            int totalCount = lstAdvertise.Count;
            advertiseViewhelper.TotalCount = totalCount;

            if (advertiseViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                advertiseViewhelper.TotalPage = totalPage;
                advertiseViewhelper.Page = pageTransition(advertiseViewhelper.Direction, advertiseViewhelper.Page, totalPage);
                advertiseViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, advertiseViewhelper.Page);
                advertiseViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, advertiseViewhelper.Page, advertiseViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (advertiseViewhelper.Page - 1) * take;
                advertiseViewhelper.LstAdvertise = lstAdvertise.OrderBy(u => u.order_number).Skip(skip).Take(take).ToList();
            }
            ViewData["advertiseViewhelper"] = advertiseViewhelper;
            return advertiseViewhelper;
        }

        public List<gov_advertise> setSearchFilter(List<gov_advertise> lstAdvertise, AdvertiseViewhelper advertiseViewhelper)
        {
            Expression<Func<gov_advertise, bool>> predicate = PredicateBuilder.False<gov_advertise>();
            if (!String.IsNullOrWhiteSpace(advertiseViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.title != null && d.title.ToUpper().Contains(advertiseViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.link != null && d.link.ToUpper().Contains(advertiseViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.department != null && d.department.ToUpper().Contains(advertiseViewhelper.KeySearch.ToUpper()));
                lstAdvertise = lstAdvertise.Where(predicate.Compile()).ToList();
            }
            return lstAdvertise;
        }
    }
}
