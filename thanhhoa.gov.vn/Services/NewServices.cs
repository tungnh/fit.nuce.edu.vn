using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Services
{
    public class NewServices : BaseServices
    {
        public int insert(NewModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_news ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("menu_id, ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("new_content, ");
            sqlBuilder.Append("new_source, ");
            sqlBuilder.Append("active_flg, ");
            sqlBuilder.Append("entry_username, ");
            sqlBuilder.Append("entry_datetime, ");
            sqlBuilder.Append("avatar,");
            sqlBuilder.Append("is_home,");
            sqlBuilder.Append("type_id, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime, ");
            sqlBuilder.Append("total_view ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.MenuId);
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.NewContent);
            objCmd.Parameters.AddWithValue("@5", item.NewSource);
            objCmd.Parameters.AddWithValue("@6", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@7", item.EntryUsername);
            objCmd.Parameters.AddWithValue("@8", item.EntryDatetime);
            objCmd.Parameters.AddWithValue("@9", item.AvatarImage);
            objCmd.Parameters.AddWithValue("@10", item.IsHome);
            objCmd.Parameters.AddWithValue("@11", item.TypeId);
            objCmd.Parameters.AddWithValue("@12", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@13", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@14", item.TotalView);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<NewModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_news gn ");
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
            List<NewModels> lstMenu = new List<NewModels>();
            NewModels item;
            while (dataReader.Read())
            {
                item = new NewModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.MenuId = Convert.ToInt32(dataReader["menu_id"]);
                item.TypeId = Convert.ToInt32(dataReader["type_id"]);
                item.Title = dataReader["title"].ToString();
                item.AvatarImage = dataReader["avatar"].ToString();
                item.Description = dataReader["description"].ToString();
                item.NewContent = dataReader["new_content"].ToString();
                item.UpdateDatetime = DateTime.Parse(dataReader["update_datetime"].ToString());
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.EntryDatetime = DateTime.Parse(dataReader["entry_datetime"].ToString());
                item.EntryUsername = dataReader["entry_username"].ToString();
                item.NewSource = dataReader["new_source"].ToString();
                item.IsHome = Convert.ToBoolean(dataReader["is_home"]);
                item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                item.TotalView = Convert.ToInt32(dataReader["total_view"]);
                lstMenu.Add(item);
            }
            getConnection().Close();
            return lstMenu;
        }

        public List<TypeNew> selectTypeNew()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_type_news ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<TypeNew> lstTypeNew = new List<TypeNew>();
            TypeNew item;
            while (dataReader.Read())
            {
                item = new TypeNew();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Name = dataReader["type_name"].ToString();
                lstTypeNew.Add(item);
            }
            getConnection().Close();
            return lstTypeNew;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_news gn ");
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

        public int update(NewModels item) {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_news ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("menu_id = @1, ");
            sqlBuilder.Append("title = @2, ");
            sqlBuilder.Append("description = @3, ");
            sqlBuilder.Append("new_content = @4, ");
            sqlBuilder.Append("new_source = @5, ");
            sqlBuilder.Append("active_flg = @6, ");
            sqlBuilder.Append("update_username = @7, ");
            sqlBuilder.Append("update_datetime = @8, ");
            sqlBuilder.Append("avatar = @9, ");
            sqlBuilder.Append("is_home = @10, ");
            sqlBuilder.Append("type_id = @11, ");
            sqlBuilder.Append("total_view = @12 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @13");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.MenuId);
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.NewContent);
            objCmd.Parameters.AddWithValue("@5", item.NewSource);
            objCmd.Parameters.AddWithValue("@6", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@7", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@8", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@9", item.AvatarImage);
            objCmd.Parameters.AddWithValue("@10", item.IsHome);
            objCmd.Parameters.AddWithValue("@11", item.TypeId);
            objCmd.Parameters.AddWithValue("@12", item.TotalView);
            objCmd.Parameters.AddWithValue("@13", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(int id) {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_news ");
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
            if (this.OrderBy != null) {
                filterBuilder.Append(" ORDER BY ");
                filterBuilder.Append(this.OrderBy);
            }
            return filterBuilder.ToString();
        }

        private String getWhereFilter()
        {
            StringBuilder filterBuilder = new StringBuilder();
            if (this.IdFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.id = ");
                filterBuilder.Append(this.IdFilter);
            }

            if (this.isHomeFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.is_home = ");
                filterBuilder.Append(Boolean.Parse(this.isHomeFilter));
            }

            if (this.activeFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.active_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.activeFlg));
            }

            if (this.menuId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.menu_id = ");
                filterBuilder.Append(Convert.ToInt32(this.menuId));
            }

            if (this.NewNotIn != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.id NOT IN ( ");
                filterBuilder.Append(Convert.ToInt32(this.NewNotIn));
                filterBuilder.Append(" ) ");
            }

            if (this.TypeNew != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gn.type_id = ");
                filterBuilder.Append(Convert.ToInt32(this.TypeNew));
            }

            if (this.KeySearch != null) {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gn.title LIKE ");
                filterBuilder.Append("'%" + this.keySearch + "%'");

                appendOr(filterBuilder);
                filterBuilder.Append(" gn.description LIKE ");
                filterBuilder.Append("'%" + this.keySearch + "%'");

                appendOr(filterBuilder);
                filterBuilder.Append(" gn.new_content LIKE ");
                filterBuilder.Append("'%" + this.keySearch + "%'");

                appendOr(filterBuilder);
                filterBuilder.Append(" gn.new_source LIKE ");
                filterBuilder.Append("'%" + this.keySearch + "%'");

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String idFilter;
        private String isHomeFilter;
        private String activeFlg;
        private String menuId;
        private String newNotIn;
        private String typeNew;
        private String keySearch;

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String TypeNew
        {
            get { return typeNew; }
            set { typeNew = value; }
        }

        public String NewNotIn
        {
            get { return newNotIn; }
            set { newNotIn = value; }
        }

        public String MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }

        public String ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String IsHomeFilter
        {
            get { return isHomeFilter; }
            set { isHomeFilter = value; }
        }

        public String IdFilter
        {
            get { return idFilter; }
            set { idFilter = value; }
        }
    }
}