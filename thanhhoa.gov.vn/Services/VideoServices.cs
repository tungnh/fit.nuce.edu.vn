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
    public class VideoServices : BaseServices
    {
        public int insert(VideoModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_video ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("avatar, ");
            sqlBuilder.Append("attach_filepath, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.Avatar);
            objCmd.Parameters.AddWithValue("@3", item.AttachFilepath);
            objCmd.Parameters.AddWithValue("@4", item.Description);
            objCmd.Parameters.AddWithValue("@5", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@6", item.UpdateDatetime);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(VideoModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_video ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("title = @1, ");
            sqlBuilder.Append("avatar = @2, ");
            sqlBuilder.Append("attach_filepath = @3, ");
            sqlBuilder.Append("description = @4, ");
            sqlBuilder.Append("update_username = @5, ");
            sqlBuilder.Append("update_datetime = @6 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@7");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.Avatar);
            objCmd.Parameters.AddWithValue("@3", item.AttachFilepath);
            objCmd.Parameters.AddWithValue("@4", item.Description);
            objCmd.Parameters.AddWithValue("@5", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@6", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@7", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<VideoModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_video gv ");
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
            List<VideoModels> lstMenu = new List<VideoModels>();
            VideoModels item;
            while (dataReader.Read())
            {
                item = new VideoModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Title = dataReader["title"].ToString();
                item.Avatar = dataReader["avatar"].ToString();
                item.AttachFilepath = dataReader["attach_filepath"].ToString();
                item.Description = dataReader["description"].ToString();
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
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
            sqlBuilder.Append("gov_video gv ");
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
            sqlBuilder.Append("gov_video ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@1 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
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

            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);

                filterBuilder.Append(" ( ");
                filterBuilder.Append(" gv.title LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gv.description LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            if (this.id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gv.id =  ");
                filterBuilder.Append(Convert.ToInt32(this.id));
            }
            return filterBuilder.ToString();
        }

        private String keySearch;
        private String id;

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