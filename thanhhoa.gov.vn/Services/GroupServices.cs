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
    public class GroupServices : BaseServices
    {
        public int insert(GroupModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_group ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("group_name, ");
            sqlBuilder.Append("hidden_flg, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Name);
            objCmd.Parameters.AddWithValue("@2", item.HiddenFlg);
            objCmd.Parameters.AddWithValue("@3", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateDatetime);

            objCmd.ExecuteNonQuery();
            objCmd.Parameters.Clear();
            objCmd.CommandText = "SELECT @@IDENTITY";

            int identity = Convert.ToInt32(objCmd.ExecuteScalar());
            return identity;
        }

        public int update(GroupModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_group ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("group_name = @1, ");
            sqlBuilder.Append("update_username = @2, ");
            sqlBuilder.Append("update_datetime = @3 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = ");
            sqlBuilder.Append("@4");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Name);
            objCmd.Parameters.AddWithValue("@2", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@3", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@4", item.Id);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public List<GroupModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group gg ");
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
            List<GroupModels> lstGroup = new List<GroupModels>();
            GroupModels item;
            while (dataReader.Read())
            {
                item = new GroupModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Name = dataReader["group_name"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                item.UpdateUsername = dataReader["update_username"].ToString();
                lstGroup.Add(item);
            }
            getConnection().Close();
            return lstGroup;
        }


        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_group gg ");
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
            if (this.keySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gg.group_name LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));
            }
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gg.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.hiddenFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gg.hidden_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.hiddenFlg));
            }
            return filterBuilder.ToString();
        }

        private String keySearch;
        private String id;
        private String hiddenFlg;

        public String HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }
    }
}