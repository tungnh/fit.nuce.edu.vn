using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Services
{
    public class AlbumGalleryServices : BaseServices
    {
        public int insert(AlbumGalleryModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_album_gallery ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("album_id, ");
            sqlBuilder.Append("gallery_id ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.AlbumId);
            objCmd.Parameters.AddWithValue("@2", item.GalleryId);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<AlbumGalleryModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_album_gallery gag ");
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
            List<AlbumGalleryModels> lstMenu = new List<AlbumGalleryModels>();
            AlbumGalleryModels item;
            while (dataReader.Read())
            {
                item = new AlbumGalleryModels();
                item.AlbumId = Convert.ToInt32(dataReader["album_id"]);
                item.GalleryId = Convert.ToInt32(dataReader["gallery_id"]);
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
            sqlBuilder.Append("gov_album_gallery gag ");
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

        public int delete(AlbumGalleryModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_album_gallery ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("album_id=@album_id AND ");
            sqlBuilder.Append("gallery_id=@gallery_id ");

            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@album_id", item.AlbumId);
            objCmd.Parameters.AddWithValue("@gallery_id", item.GalleryId);
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
            if (this.albumId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gag.album_id = ");
                filterBuilder.Append(Convert.ToInt32(this.albumId));
            }
            return filterBuilder.ToString();
        }

        private String albumId;

        public String AlbumId
        {
            get { return albumId; }
            set { albumId = value; }
        }
    }
}