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
    public class AlbumServices : BaseServices
    {
        public int insert(AlbumModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_album ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("album_title, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("avatar, ");
            sqlBuilder.Append("total_view, ");
            sqlBuilder.Append("order_number, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.AlbumTitle);
            objCmd.Parameters.AddWithValue("@2", item.Description);
            objCmd.Parameters.AddWithValue("@3", item.Avatar);
            objCmd.Parameters.AddWithValue("@4", item.TotalView);
            objCmd.Parameters.AddWithValue("@5", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(AlbumModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_album ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("album_title = @1, ");
            sqlBuilder.Append("description = @2, ");
            sqlBuilder.Append("avatar = @3, ");
            sqlBuilder.Append("total_view = @4, ");
            sqlBuilder.Append("order_number = @5, ");
            sqlBuilder.Append("update_username = @6, ");
            sqlBuilder.Append("update_datetime = @7 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @8");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.AlbumTitle);
            objCmd.Parameters.AddWithValue("@2", item.Description);
            objCmd.Parameters.AddWithValue("@3", item.Avatar);
            objCmd.Parameters.AddWithValue("@4", item.TotalView);
            objCmd.Parameters.AddWithValue("@5", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@8", item.Id);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<AlbumModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_album gab ");
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
            List<AlbumModels> lstMenu = new List<AlbumModels>();
            AlbumModels item;
            while (dataReader.Read())
            {
                item = new AlbumModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.AlbumTitle = dataReader["album_title"].ToString();
                item.Description = dataReader["description"].ToString();
                item.Avatar = dataReader["avatar"].ToString();
                item.TotalView = Convert.ToInt32(dataReader["total_view"]);
                item.OrderNumber = Convert.ToInt32(dataReader["order_number"]);
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                item.UpdateUsername = dataReader["update_username"].ToString();
                lstMenu.Add(item);
            }
            getConnection().Close();
            return lstMenu;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_album gab ");
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

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_album ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@id ");

            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@id", id);
            int rs = objCmd.ExecuteNonQuery();
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
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gab.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gab.album_title Like ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gab.description Like ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
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