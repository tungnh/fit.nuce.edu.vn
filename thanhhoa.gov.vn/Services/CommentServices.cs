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
    public class CommentServices : BaseServices
    {
        public int insert(CommentModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_comments ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("user_id, ");
            sqlBuilder.Append("full_name, ");
            sqlBuilder.Append("parent_id, ");
            sqlBuilder.Append("news_id, ");
            sqlBuilder.Append("comments_content, ");
            sqlBuilder.Append("total_like, ");
            sqlBuilder.Append("like_info, ");
            sqlBuilder.Append("active_flg,");
            sqlBuilder.Append("entry_datetime,");
            sqlBuilder.Append("email");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.UserId);
            objCmd.Parameters.AddWithValue("@2", item.FullName);
            objCmd.Parameters.AddWithValue("@3", item.ParentId);
            objCmd.Parameters.AddWithValue("@4", item.NewsId);
            objCmd.Parameters.AddWithValue("@5", item.CommentsContent);
            objCmd.Parameters.AddWithValue("@6", item.TotalLike);
            objCmd.Parameters.AddWithValue("@7", item.LikeInfo);
            objCmd.Parameters.AddWithValue("@8", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@9", item.EntryDatetime);
            objCmd.Parameters.AddWithValue("@10", item.Email);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(CommentModels item) {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_comments ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("user_id = @1, ");
            sqlBuilder.Append("full_name = @2, ");
            sqlBuilder.Append("parent_id = @3, ");
            sqlBuilder.Append("news_id = @4, ");
            sqlBuilder.Append("comments_content = @5, ");
            sqlBuilder.Append("total_like =  @6, ");
            sqlBuilder.Append("like_info = @7, ");
            sqlBuilder.Append("active_flg = @8, ");
            sqlBuilder.Append("entry_datetime = @9, ");
            sqlBuilder.Append("email = @10 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @11 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.UserId);
            objCmd.Parameters.AddWithValue("@2", item.FullName);
            objCmd.Parameters.AddWithValue("@3", item.ParentId);
            objCmd.Parameters.AddWithValue("@4", item.NewsId);
            objCmd.Parameters.AddWithValue("@5", item.CommentsContent);
            objCmd.Parameters.AddWithValue("@6", item.TotalLike);
            objCmd.Parameters.AddWithValue("@7", item.LikeInfo);
            objCmd.Parameters.AddWithValue("@8", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@9", item.EntryDatetime);
            objCmd.Parameters.AddWithValue("@10", item.Email);
            objCmd.Parameters.AddWithValue("@11", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<CommentModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_comments gcm ");
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
            List<CommentModels> lstMenu = new List<CommentModels>();
            CommentModels item;
            while (dataReader.Read())
            {
                item = new CommentModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.UserId = Convert.ToInt32(dataReader["user_id"]);
                item.FullName = dataReader["full_name"].ToString();
                item.Email = dataReader["email"].ToString();
                item.ParentId = Convert.ToInt32(dataReader["parent_id"]);
                item.NewsId = Convert.ToInt32(dataReader["news_id"]);
                item.CommentsContent = dataReader["comments_content"].ToString();
                item.TotalLike = Convert.ToInt32(dataReader["total_like"]);
                item.LikeInfo = dataReader["like_info"].ToString();
                item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                item.EntryDatetime = Convert.ToDateTime(dataReader["entry_datetime"]);
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
            sqlBuilder.Append("gov_comments gcm ");
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
            sqlBuilder.Append("gov_comments ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public List<String> selectKeyBand()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_key_band gkb ");
            //sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<String> lstMenu = new List<String>();
            String item;
            while (dataReader.Read())
            {
                item = dataReader["key_band"].ToString();
                lstMenu.Add(item);
            }
            getConnection().Close();
            return lstMenu;
        }

        public List<KeyBandModels> selectKey(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_key_band gkb ");
            this.Sql = sqlBuilder.ToString();

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
            List<KeyBandModels> lstKey = new List<KeyBandModels>();
            KeyBandModels item;
            while (dataReader.Read())
            {
                item = new KeyBandModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Key = dataReader["key_band"].ToString();
                item.EntryUsername = dataReader["entry_username"].ToString();
                item.EntryDatetime = Convert.ToDateTime(dataReader["entry_datetime"]);
                lstKey.Add(item);
            }
            getConnection().Close();
            return lstKey;
        }

        public int totalCountKeyBand()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_key_band ");
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

        public int insertKeyBand(KeyBandModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_key_band ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("key_band, ");
            sqlBuilder.Append("entry_username, ");
            sqlBuilder.Append("entry_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Key);
            objCmd.Parameters.AddWithValue("@2", item.EntryUsername);
            objCmd.Parameters.AddWithValue("@3", item.EntryDatetime);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int deleteKey(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_key_band ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
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
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gcm.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.parentId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gcm.parent_id = ");
                filterBuilder.Append(Convert.ToInt32(this.parentId));
            }
            if (this.newsId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gcm.news_id = ");
                filterBuilder.Append(Convert.ToInt32(this.newsId));
            }
            if (this.activeFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gcm.active_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.activeFlg));
            }
            if (this.keyLike != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");
                filterBuilder.Append(" false ");

                foreach (String keyLike in selectKeyBand()) {
                    appendOr(filterBuilder);
                    filterBuilder.Append(" gcm.comments_content LIKE ");
                    filterBuilder.Append(pareLikeString(keyLike, Constant.FILTER_KIND_MIDDLE));
                }

                filterBuilder.Append(" ) ");
            }
            if (this.keyNotLike != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");
                filterBuilder.Append(" true ");

                foreach (String keyLike in selectKeyBand())
                {
                    appendAnd(filterBuilder);
                    filterBuilder.Append(" gcm.comments_content NOT LIKE ");
                    filterBuilder.Append(pareLikeString(keyLike, Constant.FILTER_KIND_MIDDLE));
                }

                filterBuilder.Append(" ) ");
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gcm.full_name LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gcm.email LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gcm.comments_content LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String parentId;
        private String newsId;
        private String activeFlg;
        private String keySearch;
        private String id;
        private String keyLike;
        private String keyNotLike;

        public String KeyNotLike
        {
            get { return keyNotLike; }
            set { keyNotLike = value; }
        }

        public String KeyLike
        {
            get { return keyLike; }
            set { keyLike = value; }
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

        public String ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String NewsId
        {
            get { return newsId; }
            set { newsId = value; }
        }

        public String ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
    }
}