using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Services
{
    public class NotifyServices : BaseServices
    {
        public int insert(NotifyModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_notify ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("content, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.Content);
            objCmd.Parameters.AddWithValue("@3", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateDatetime);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(NotifyModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_notify ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("title = @1, ");
            sqlBuilder.Append("content = @2, ");
            sqlBuilder.Append("update_username = @3, ");
            sqlBuilder.Append("update_datetime = @4 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @5 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.Content);
            objCmd.Parameters.AddWithValue("@3", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@5", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<NotifyModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_notify gn ");
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
            List<NotifyModels> lstDocument = new List<NotifyModels>();
            NotifyModels item;
            while (dataReader.Read())
            {
                item = new NotifyModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Title = dataReader["title"].ToString();
                item.Content = dataReader["content"].ToString();
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                lstDocument.Add(item);
            }
            getConnection().Close();
            return lstDocument;
        }
        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_notify ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_notify gn ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            int count = 0;
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
            if (this.id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.id = ");
                filterBuilder.Append(Convert.ToInt32(this.id));
            }
            if (this.keySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.title = ");
                filterBuilder.Append(this.keySearch);
            }
            return filterBuilder.ToString();
        }

        private String id;
        private String keySearch;

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}