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
    public class GroupController : BaseController
    {
        //
        // GET: /Admin/Group/

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            GroupViewhelper groupViewhelper = new GroupViewhelper();
            saveData(groupViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(GroupViewhelper groupViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(groupViewhelper);
            return View();
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        public ActionResult SaveRegist(String groupName, int[] moduleDanhMuc, int[] moduleTinTuc, int[] moduleVanBan, int[] moduleMedia, int[] moduleCuuSinhVien, int[] moduleNguoiDung, int[] moduleHeThong, int[] moduleDiemThi)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            gov_group groupInfo = new gov_group();
            groupInfo.hidden_flg = false;
            groupInfo.group_name = groupName;
            groupInfo.entry_username = Session.getCurrentUser().username;
            groupInfo.entry_datetime = DateTime.Now;
            groupInfo.update_username = Session.getCurrentUser().username;
            groupInfo.update_datetime = DateTime.Now;
            groupInfo = _cnttDB.gov_group.Add(groupInfo);
            int rs = _cnttDB.SaveChanges();
            if (rs > 0)
            {
                //Them moi quyen cho module danh muc
                int permissionNumber = 0;
                if (moduleDanhMuc != null)
                {
                    foreach (var permission in moduleDanhMuc)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 1;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Tin tuc
                permissionNumber = 0;
                if (moduleTinTuc != null)
                {
                    foreach (var permission in moduleTinTuc)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 2;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Van Ban
                permissionNumber = 0;
                if (moduleVanBan != null)
                {
                    foreach (var permission in moduleVanBan)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 3;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Media
                permissionNumber = 0;
                if (moduleMedia != null)
                {
                    foreach (var permission in moduleMedia)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 4;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Cuu Sinh Vien
                permissionNumber = 0;
                if(moduleCuuSinhVien != null){
                    foreach (var permission in moduleCuuSinhVien)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 8;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Nguoi Dung
                permissionNumber = 0;
                if (moduleNguoiDung != null)
                {
                    foreach (var permission in moduleNguoiDung)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 6;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module He thong
                permissionNumber = 0;
                if (moduleHeThong != null)
                {
                    foreach (var permission in moduleHeThong)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 7;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Diem Thi
                permissionNumber = 0;
                if (moduleDiemThi != null)
                {
                    foreach (var permission in moduleDiemThi)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 9;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }
                if (rs > 0)
                {
                    TempData["message"] = "Thêm mới thành công!";
                    insertHistory(AccessType.themMoiGroup, Constant.THEM(Constant.ITEM_GROUP, Constant.ID, groupInfo.id.ToString()));
                }
                else
                {
                    TempData["err"] = "Thêm mới thất bại!";
                }
                
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["groupInfo"] = _cnttDB.gov_group.Where(g => g.id == id && g.hidden_flg == false).FirstOrDefault();
            return View();
        }

        public ActionResult SaveEdit(int id, String groupName, int[] moduleDanhMuc, int[] moduleTinTuc, int[] moduleVanBan, int[] moduleMedia, int[] moduleCuuSinhVien, int[] moduleNguoiDung, int[] moduleHeThong, int[] moduleDiemThi)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            gov_group groupInfo = new gov_group();
            groupInfo = _cnttDB.gov_group.Find(id);
            groupInfo.group_name = groupName;
            groupInfo.update_username = Session.getCurrentUser().username;
            groupInfo.update_datetime = DateTime.Now;
            int rs = _cnttDB.SaveChanges();

            List<gov_group_permission> lstPermission = _cnttDB.gov_group_permission.Where(g => g.group_id == id).ToList();
            foreach (var item in lstPermission)
            {
                _cnttDB.gov_group_permission.Remove(item);
                _cnttDB.SaveChanges();
            }
            {
                //Them moi quyen cho module danh muc
                int permissionNumber = 0;
                if (moduleDanhMuc != null)
                {
                    foreach (var permission in moduleDanhMuc)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 1;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Tin tu
                permissionNumber = 0;
                if (moduleTinTuc != null)
                {
                    foreach (var permission in moduleTinTuc)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 2;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Van Ban
                permissionNumber = 0;
                if (moduleVanBan != null)
                {
                    foreach (var permission in moduleVanBan)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 3;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Media
                permissionNumber = 0;
                if (moduleMedia != null)
                {
                    foreach (var permission in moduleMedia)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 4;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Cuu Sinh Vien
                permissionNumber = 0;
                if (moduleCuuSinhVien != null)
                {
                    foreach (var permission in moduleCuuSinhVien)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 8;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Nguoi Dung
                permissionNumber = 0;
                if (moduleNguoiDung != null)
                {
                    foreach (var permission in moduleNguoiDung)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 6;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module He thong
                permissionNumber = 0;
                if (moduleHeThong != null)
                {
                    foreach (var permission in moduleHeThong)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 7;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }

                //Them moi quyen cho module Diem Thi
                permissionNumber = 0;
                if (moduleDiemThi != null)
                {
                    foreach (var permission in moduleDiemThi)
                    {
                        permissionNumber += permission;
                    }
                }
                if (permissionNumber > 0)
                {
                    gov_group_permission groupPermission = new gov_group_permission();
                    groupPermission.module_id = 9;
                    groupPermission.group_id = groupInfo.id;
                    groupPermission.permission_number = permissionNumber;
                    groupPermission.update_datetime = DateTime.Now;
                    groupPermission.update_username = Session.getCurrentUser().username;
                    _cnttDB.gov_group_permission.Add(groupPermission);
                    _cnttDB.SaveChanges();
                }
                if (rs > 0)
                {
                    TempData["message"] = "Cập nhật thông tin thành công!";
                    insertHistory(AccessType.chinhSuaGroup, Constant.CHINHSUA(Constant.ITEM_GROUP, Constant.ID, groupInfo.id.ToString()));
                }
                else
                {
                    TempData["err"] = "Cập nhật thông tin thất bại!";
                }
                
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            try
            {
                _cnttDB.gov_group.Remove(_cnttDB.gov_group.Find(id));
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.xoaGroup, Constant.XOA(Constant.ITEM_GROUP, Constant.ID, id.ToString()));
                }
            }
            catch (Exception ex)
            {
                return Redirect("/admin/error/error404");
            }
            TempData["message"] = "Xóa thông tin thành công!";
            return Redirect("index");
        }

        public ActionResult ListGroupMember(int groupId)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            GroupMemberViewhelper groupMemberViewhelper = new GroupMemberViewhelper();
            groupMemberViewhelper.GroupId = groupId;
            groupMemberViewhelper.UserInGroup = groupId;
            saveListGroupMember(groupMemberViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult ListGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            groupMemberViewhelper.UserInGroup = groupMemberViewhelper.GroupId;
            saveListGroupMember(groupMemberViewhelper);
            return View();
        }

        [HttpGet]
        public ActionResult AddGroupMember(int groupId)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            GroupMemberViewhelper groupMemberViewhelper = new GroupMemberViewhelper();
            groupMemberViewhelper.GroupId = groupId;
            groupMemberViewhelper.UserNotInGroup = groupId;
            saveListGroupMember(groupMemberViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult AddGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            groupMemberViewhelper.UserNotInGroup = groupMemberViewhelper.GroupId;
            saveListGroupMember(groupMemberViewhelper);
            return View();
        }

        public ActionResult SaveAddGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            String content = "";
            foreach (var user in groupMemberViewhelper.Username)
            {
                gov_group_members groupMember = new gov_group_members();
                groupMember.group_id = groupMemberViewhelper.GroupId;
                groupMember.username = user;
                groupMember.update_datetime = DateTime.Now;
                groupMember.update_username = Session.getCurrentUser().username;
                _cnttDB.gov_group_members.Add(groupMember);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    content += Constant.THEM_IN(Constant.ITEM_USER, Constant.USERNAME, user, Constant.ITEM_GROUP, Constant.ID, groupMemberViewhelper.GroupId.ToString());
                    content += ".<br/>";
                }
            }
            if (!content.Equals(""))
                insertHistory(AccessType.themNguoiDungVaoGroup, content);
            TempData["message"] = "Thêm người dùng vào nhóm thành công!";
            return Redirect("listgroupmember?groupId=" + groupMemberViewhelper.GroupId.ToString());
        }

        public ActionResult DeleteGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_NGUOIDUNG, Session.getCurrentUser().username, TypeAudit.GroupUser))
            {
                return Redirect("/admin/error/error403");
            }
            String content = "";
            foreach (var user in groupMemberViewhelper.Username) {
                try
                {
                    gov_group_members groupMember = _cnttDB.gov_group_members.Where(g => g.group_id == groupMemberViewhelper.GroupId && g.username == user).FirstOrDefault();
                    if (groupMember != null) {
                        _cnttDB.gov_group_members.Remove(groupMember);
                       int rs =  _cnttDB.SaveChanges();
                       if (rs > 0)
                       {
                           content += Constant.XOA_IN(Constant.ITEM_USER, Constant.USERNAME, user, Constant.ITEM_GROUP, Constant.ID, groupMember.group_id.ToString());
                           content += ".<br/>";
                       }
                    }
                }
                catch (Exception ex) {
                    return Redirect("/admin/error/error405");
                }
            }
            if (!content.Equals(""))
                insertHistory(AccessType.xoaNguoiDungKhoiGroup, content);
            TempData["message"] = "Xóa người dùng khỏi nhóm thành công!";
            return Redirect("listgroupmember?groupId=" + groupMemberViewhelper.GroupId.ToString());
        }

        public GroupMemberViewhelper saveListGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            List<gov_user> lstUser = setSearchFilterGroupMember(groupMemberViewhelper);
            int totalCount = lstUser.Count;
            groupMemberViewhelper.TotalCount = totalCount;

            if (groupMemberViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                groupMemberViewhelper.TotalPage = totalPage;
                groupMemberViewhelper.Page = pageTransition(groupMemberViewhelper.Direction, groupMemberViewhelper.Page, totalPage);
                groupMemberViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, groupMemberViewhelper.Page);
                groupMemberViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, groupMemberViewhelper.Page, groupMemberViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (groupMemberViewhelper.Page - 1) * take;
                groupMemberViewhelper.LstUser = lstUser.OrderByDescending(u => u.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["groupMemberViewhelper"] = groupMemberViewhelper;
            return groupMemberViewhelper;
        }

        public List<gov_user> setSearchFilterGroupMember(GroupMemberViewhelper groupMemberViewhelper)
        {
            List<gov_user> lstUser = new List<gov_user>();
            if (groupMemberViewhelper.UserInGroup != 0)
            {
                lstUser = (from u in _cnttDB.gov_user
                           where _cnttDB.gov_group_members.Any(g => g.username == u.username && g.group_id == groupMemberViewhelper.GroupId)
                           select u).ToList();
                var a = lstUser;
            }
            if (groupMemberViewhelper.UserNotInGroup != 0) {
                lstUser = (from u in _cnttDB.gov_user
                           where !_cnttDB.gov_group_members.Any(g => g.username == u.username && g.group_id == groupMemberViewhelper.GroupId)
                           select u).ToList();
                var a = lstUser;
            }
            return lstUser.Where(u => u.hidden_flg == false).ToList();
        }

        public GroupViewhelper saveData(GroupViewhelper groupViewhelper)
        {
            List<gov_group> lstGroup = _cnttDB.gov_group.Where(g => g.hidden_flg == false).ToList();
            lstGroup = setSearchFilter(lstGroup, groupViewhelper);

            int totalCount = lstGroup.Count;
            groupViewhelper.TotalCount = totalCount;

            if (groupViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                groupViewhelper.TotalPage = totalPage;
                groupViewhelper.Page = pageTransition(groupViewhelper.Direction, groupViewhelper.Page, totalPage);
                groupViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, groupViewhelper.Page);
                groupViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, groupViewhelper.Page, groupViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (groupViewhelper.Page - 1) * take;
                groupViewhelper.LstGroup = lstGroup.OrderByDescending(g => g.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["groupViewhelper"] = groupViewhelper;
            return groupViewhelper;
        }

        public List<gov_group> setSearchFilter(List<gov_group> lstGroup, GroupViewhelper groupViewhelper)
        {
            if (!String.IsNullOrEmpty(groupViewhelper.KeySearch))
                lstGroup = lstGroup.Where(g => g.group_name.ToUpper().Contains(groupViewhelper.KeySearch)).ToList();
            return lstGroup;
        }
    }
}
