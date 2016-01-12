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
    public class GroupMemberServices : BaseServices
    {
        public int insert(GroupMemberModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_group_members ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("group_id, ");
            sqlBuilder.Append("username, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.GroupId);
            objCmd.Parameters.AddWithValue("@2", item.Username);
            objCmd.Parameters.AddWithValue("@3", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateDatetime);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(GroupMemberModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group_members ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("group_id = @1 ");
            sqlBuilder.Append("AND ");
            sqlBuilder.Append("username = @2 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.GroupId);
            objCmd.Parameters.AddWithValue("@2", item.Username);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public List<GroupMemberModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group_members ggm ");
            sqlBuilder.Append(getBaseSQL());
            if (page > 0 && limit > 0)
            {
                sqlBuilder.Append(" LIMIT @limit ");
                sqlBuilder.Append(" OFFSET @offset ");
            }
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            if (page > 0 && limit > 0)
            {
                objCmd.Parameters.AddWithValue("@limit", limit);
                objCmd.Parameters.AddWithValue("@offset", offset);
            }

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<GroupMemberModels> lstGroupMember = new List<GroupMemberModels>();
            GroupMemberModels item;
            while (dataReader.Read())
            {
                item = new GroupMemberModels();
                item.GroupId = Convert.ToInt32(dataReader["group_id"]);
                item.Username = dataReader["username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                item.UpdateUsername = dataReader["update_username"].ToString();
                lstGroupMember.Add(item);
            }
            getConnection().Close();
            return lstGroupMember;
        }


        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group_members ggm ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            int count = 0;
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            while (dataReader.Read())
            {
                count++;
            }
            getConnection().Close();
            return count;
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
            if (this.GroupId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ggm.group_id = ");
                filterBuilder.Append(Convert.ToInt32(this.GroupId));
            }
            if (this.username != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ggm.username = ");
                filterBuilder.Append(pareLikeString(this.username, Constant.FILTER_KIND_FULL));
            }
            return filterBuilder.ToString();
        }

        private String groupId;
        private String username;

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public String GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        
    }
}