using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Services
{
    public class MenuServices : BaseServices
    {
        public MenuServices() { 
        }

        public int insert(MenuModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_menu ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("id_parent, ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("order_number, ");
            sqlBuilder.Append("active_flg, ");
            sqlBuilder.Append("link, ");
            sqlBuilder.Append("entry_username, ");
            sqlBuilder.Append("entry_datetime, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime,");
            sqlBuilder.Append("ishome");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.ParentId);
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@5", item.AtiveFlg);
            objCmd.Parameters.AddWithValue("@6", item.Link);
            objCmd.Parameters.AddWithValue("@7", item.EntryUsername);
            objCmd.Parameters.AddWithValue("@8", item.EntryDatetime);
            objCmd.Parameters.AddWithValue("@9", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@10", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@11", item.IsHome);
            objCmd.ExecuteNonQuery();

            objCmd.Parameters.Clear();
            objCmd.CommandText = "SELECT @@IDENTITY";

            int identity = Convert.ToInt32(objCmd.ExecuteScalar());
            return identity;
        }

        public int update(MenuModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_menu ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("id_parent = @1, ");
            sqlBuilder.Append("title = @2, ");
            sqlBuilder.Append("description = @3, ");
            sqlBuilder.Append("order_number = @4, ");
            sqlBuilder.Append("active_flg = @5, ");
            sqlBuilder.Append("link = @6, ");
            sqlBuilder.Append("update_username= @7, ");
            sqlBuilder.Append("update_datetime = @8, ");
            sqlBuilder.Append("entry_username= @9, ");
            sqlBuilder.Append("entry_datetime = @10, ");
            sqlBuilder.Append("ishome = @11 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @12 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.ParentId);
            objCmd.Parameters.AddWithValue("@2", item.Title);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@5", item.AtiveFlg);
            objCmd.Parameters.AddWithValue("@6", item.Link);
            objCmd.Parameters.AddWithValue("@7", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@8", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@9", item.EntryUsername);
            objCmd.Parameters.AddWithValue("@10", item.EntryDatetime);
            objCmd.Parameters.AddWithValue("@11", item.IsHome);
            objCmd.Parameters.AddWithValue("@12", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_menu ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@1 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<MenuModels> select(int page, int limit)
        {
            int start = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_menu gm ");
            sqlBuilder.Append(getBaseSQL());
            if(page > 0 && limit > 0){
                sqlBuilder.Append(" LIMIT @limit ");
                sqlBuilder.Append(" OFFSET @start ");
            }
            this.Sql = sqlBuilder.ToString();
            
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            if(page > 0 && limit > 0){
                objCmd.Parameters.AddWithValue("@limit", limit);
                objCmd.Parameters.AddWithValue("@start", start);
            }
            
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<MenuModels> lstMenu = new List<MenuModels>();
            MenuModels item;
            while (dataReader.Read())
            {
                item = new MenuModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.ParentId = Convert.ToInt32(dataReader["id_parent"]);
                item.Title = dataReader["title"].ToString();
                item.Description = dataReader["description"].ToString();
                item.OrderNumber = Convert.ToInt32(dataReader["order_number"]);
                item.Link = dataReader["link"].ToString();
                item.AtiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                item.IsHome = Convert.ToBoolean(dataReader["ishome"]);
                item.EntryUsername = dataReader["entry_username"].ToString();
                item.EntryDatetime = Convert.ToDateTime(dataReader["entry_datetime"]);
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                lstMenu.Add(item);
            }
            getConnection().Close();
            return lstMenu;
        }

        public int countTotal()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_menu gm ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            int i = 0;
            while (dataReader.Read()) {
                i++;
            }
            getConnection().Close();
            return i;
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
            if(this.OrderBy != null){
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
                filterBuilder.Append(" gm.id = ");
                filterBuilder.Append(int.Parse(this.IdFilter));
            }
            if (this.ActiveFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.active_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.ActiveFlg));
            }
            if (this.IdParentFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.id_parent = ");
                filterBuilder.Append(int.Parse(this.IdParentFilter));
            }

            if (this.isHomeFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.ishome = ");
                filterBuilder.Append(Boolean.Parse(this.isHomeFilter));
            }

            if (this.nameFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.title LIKE ");
                filterBuilder.Append("'%" + this.nameFilter + "%'");
            }

            if (this.notId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.id NOT IN");
                filterBuilder.Append(" ( ");
                filterBuilder.Append(Convert.ToInt32(this.notId));
                filterBuilder.Append(" ) ");
            }
            if (this.MenuInNew != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gm.id IN (select menu_id from gov_news) ");
            }
            return filterBuilder.ToString();
        }

        private String idFilter;
        private String idParentFilter;
        private String isHomeFilter;
        private String nameFilter;
        private String notId;
        private String activeFlg;
        private String menuInNew;

        public String MenuInNew
        {
            get { return menuInNew; }
            set { menuInNew = value; }
        }

        public String ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String NotId
        {
            get { return notId; }
            set { notId = value; }
        }

        public String NameFilter
        {
            get { return nameFilter; }
            set { nameFilter = value; }
        }

        public String IsHomeFilter
        {
            get { return isHomeFilter; }
            set { isHomeFilter = value; }
        }

        public String IdParentFilter
        {
            get { return idParentFilter; }
            set { idParentFilter = value; }
        }

        public String IdFilter
        {
            get { return idFilter; }
            set { idFilter = value; }
        }
    }
}