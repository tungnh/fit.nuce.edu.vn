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
    public class GroupPermissionServices : BaseServices
    {
        public int insert(GroupPermissionModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_group_permission ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("group_id, ");
            sqlBuilder.Append("module_id, ");
            sqlBuilder.Append("permission_number, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.GroupId);
            objCmd.Parameters.AddWithValue("@2", item.ModuleId);
            objCmd.Parameters.AddWithValue("@3", item.PermissionNumber);
            objCmd.Parameters.AddWithValue("@4", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@5", item.UpdateDatetime);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<GroupPermissionModels> selectPermission(int moduleId, String username)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append(" ggp.* ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append(" gov_group_permission ggp, ");
            sqlBuilder.Append(" gov_group_members ggm ");
            sqlBuilder.Append(" WHERE ");
            sqlBuilder.Append(" ggm.group_id = ggp.group_id ");
            sqlBuilder.Append(" AND ");
            sqlBuilder.Append(" ggm.username = ");
            sqlBuilder.Append(pareLikeString(username, Constant.FILTER_KIND_FULL));
            sqlBuilder.Append(" AND ");
            sqlBuilder.Append(" ggp.module_id = ");
            sqlBuilder.Append(moduleId);
            sqlBuilder.Append(" GROUP BY ");
            sqlBuilder.Append(" permission_number ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<GroupPermissionModels> lstGroupPermission = new List<GroupPermissionModels>();
            GroupPermissionModels item;
            while (dataReader.Read())
            {
                item = new GroupPermissionModels();
                item.GroupId = Convert.ToInt32(dataReader["group_id"]);
                item.ModuleId = Convert.ToInt32(dataReader["module_id"]);
                item.PermissionNumber = Convert.ToInt32(dataReader["permission_number"]);
                lstGroupPermission.Add(item);
            }
            getConnection().Close();
            return lstGroupPermission;
        }

        public List<GroupPermissionModels> select()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append(" * ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append(" gov_group_permission ggp ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<GroupPermissionModels> lstGroupPermission = new List<GroupPermissionModels>();
            GroupPermissionModels item;
            while (dataReader.Read())
            {
                item = new GroupPermissionModels();
                item.GroupId = Convert.ToInt32(dataReader["group_id"]);
                item.ModuleId = Convert.ToInt32(dataReader["module_id"]);
                item.PermissionNumber = Convert.ToInt32(dataReader["permission_number"]);
                lstGroupPermission.Add(item);
            }
            getConnection().Close();
            return lstGroupPermission;
        }

        public int deleteByGroup(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group_permission ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("group_id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        private String getBaseSQL()
        {
            StringBuilder filterBuilder = new StringBuilder();
            String whereString = getWhereFilter();
            if (whereString.Length > 0)
            {
                filterBuilder.Append(" WHERE 1=1 ");
                filterBuilder.Append(whereString);
            }
            if (this.OrderBy != null)
            {
                filterBuilder.Append(" ORDER BY ");
                filterBuilder.Append(this.OrderBy);
            }
            return filterBuilder.ToString();
        }

        private String getWhereFilter()
        {
            StringBuilder filterBuilder = new StringBuilder();
            if (groupId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append("group_id = ");
                filterBuilder.Append(Convert.ToInt32(this.groupId));
            }
            return filterBuilder.ToString();
        }

        private String groupId;

        public String GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
    }
}