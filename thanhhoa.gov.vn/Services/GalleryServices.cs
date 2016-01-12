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
    public class GalleryServices : BaseServices
    {
        public int insert(GalleryModels item) {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_gallery ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("total_view, ");
            sqlBuilder.Append("avatar, ");
            sqlBuilder.Append("attach_filepath, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@2, @3, @4, @5, @6, @7");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.TotalView);
            objCmd.Parameters.AddWithValue("@4", item.Avatar);
            objCmd.Parameters.AddWithValue("@5", item.AttachFilepath);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(GalleryModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_gallery ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("title = @2, ");
            sqlBuilder.Append("total_view = @3, ");
            sqlBuilder.Append("avatar = @4, ");
            sqlBuilder.Append("attach_filepath = @5, ");
            sqlBuilder.Append("update_username = @6, ");
            sqlBuilder.Append("update_datetime = @7 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@8");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.TotalView);
            objCmd.Parameters.AddWithValue("@4", item.Avatar);
            objCmd.Parameters.AddWithValue("@5", item.AttachFilepath);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@8", item.Id);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<GalleryModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_gallery gglr ");
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
            List<GalleryModels> lstMenu = new List<GalleryModels>();
            GalleryModels item;
            while (dataReader.Read())
            {
                item = new GalleryModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Title = dataReader["title"].ToString();
                item.TotalView = Convert.ToInt32(dataReader["total_view"]);
                item.Avatar = dataReader["avatar"].ToString();
                item.AttachFilepath = dataReader["attach_filepath"].ToString();
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
            sqlBuilder.Append("gov_gallery gglr ");
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
            return count++;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_gallery ");
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
            if (this.idNotIn != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gglr.id not in (select gallery_id from gov_album_gallery where album_id=");
                filterBuilder.Append(Convert.ToInt32(this.idNotIn));
                filterBuilder.Append(" ) ");
            }
            if (this.idNotInAlbum != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gglr.id not in (select gallery_id from gov_album_gallery) ");
            }
            if (this.inAlbum != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gglr.id in (select gallery_id from gov_album_gallery WHERE album_id =");
                filterBuilder.Append(Convert.ToInt32(this.inAlbum));
                filterBuilder.Append(" ) ");
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gglr.title LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));
            }
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gglr.id = ");
                filterBuilder.Append(this.id);
            }
            return filterBuilder.ToString();
        }

        private String idNotIn;
        private String idNotInAlbum;
        private String inAlbum;
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

        public String InAlbum
        {
            get { return inAlbum; }
            set { inAlbum = value; }
        }
        public String IdNotInAlbum
        {
            get { return idNotInAlbum; }
            set { idNotInAlbum = value; }
        }

        public String IdNotIn
        {
            get { return idNotIn; }
            set { idNotIn = value; }
        }
    }
}