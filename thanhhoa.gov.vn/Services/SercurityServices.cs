using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Services
{
    public class SercurityServices
    {
        public static bool HasPermission(int moduleId, String username, TypeAudit audit)
        {
            var _cnttDB = new CNTTDHXDEntities();

            /*GroupPermissionServices groupPermissionServices = new GroupPermissionServices();
            List<GroupPermissionModels> lstGroupPermission = groupPermissionServices.selectPermission(moduleId, username);
            foreach (var item in lstGroupPermission)
            {
                if (Utils.HassPermission(audit, item.PermissionNumber))
                    return true;
            }*/
            //List<gov_group_permission> lstGroupPermission = select from 
            var lstGroupPermission = (from ggp in _cnttDB.gov_group_permission
                                      join ggm in _cnttDB.gov_group_members
                                          on ggp.group_id equals ggm.group_id
                                      where (ggm.username.Equals(username) && ggp.module_id == moduleId)
                                      select ggp).ToList();
            var a = lstGroupPermission.Count();
            foreach (var item in lstGroupPermission)
            {
                if (Utils.HassPermission(audit, item.permission_number))
                    return true;
            }
            return false;
        }
    }
}