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
    public class DistrictServices : BaseServices
    {
        public int insert(DistrictModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_district ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("name, ");
            sqlBuilder.Append("level, ");
            sqlBuilder.Append("decription, ");
            sqlBuilder.Append("link, ");
            sqlBuilder.Append("update_user_id, ");
            sqlBuilder.Append("update_datetime,");
            sqlBuilder.Append("order_number");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@2, @3, @4, @5, @6, @7, @8");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@2", item.Name);
            objCmd.Parameters.AddWithValue("@3", item.Level);
            objCmd.Parameters.AddWithValue("@4", item.Decription);
            objCmd.Parameters.AddWithValue("@5", item.Link);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUserId);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@8", item.OrderNumber);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int Update(DistrictModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_district ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("name = @1, ");
            sqlBuilder.Append("level = @2, ");
            sqlBuilder.Append("decription = @3, ");
            sqlBuilder.Append("link = @4, ");
            sqlBuilder.Append("update_user_id = @5, ");
            sqlBuilder.Append("update_datetime = @6");
            sqlBuilder.Append("order_number = @7");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @8");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@8", item.Id);
            objCmd.Parameters.AddWithValue("@1", item.Name);
            objCmd.Parameters.AddWithValue("@2", item.Level);
            objCmd.Parameters.AddWithValue("@3", item.Decription);
            objCmd.Parameters.AddWithValue("@4", item.Link);
            objCmd.Parameters.AddWithValue("@5", item.UpdateUserId);
            objCmd.Parameters.AddWithValue("@6", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@7", item.OrderNumber);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<DistrictModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_district gdt ");
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
            List<DistrictModels> lstMenu = new List<DistrictModels>();
            DistrictModels item;
            while (dataReader.Read())
            {
                item = new DistrictModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Name = dataReader["name"].ToString();
                item.Level = Convert.ToInt32(dataReader["level"]);
                item.Decription = dataReader["decription"].ToString();
                item.Link = dataReader["link"].ToString();
                item.ShowMap = Convert.ToBoolean(dataReader["show_map"]);
                item.Coordinates = dataReader["coordinates"].ToString();
                if (dataReader["order_number"] != null)
                {
                    item.OrderNumber = Convert.ToInt32(dataReader["order_number"]);
                }
                //item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
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
            sqlBuilder.Append("gov_district gdt ");
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
            if (this.Level != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdt.level = ");
                filterBuilder.Append(Convert.ToInt32(this.Level));
            }
            if (this.Name!= null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdt.name LIKE ");
                filterBuilder.Append(pareLikeString(this.Name, Constant.FILTER_KIND_MIDDLE));
            }
            if (this.showMap != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdt.show_map = ");
                filterBuilder.Append(Convert.ToBoolean(this.showMap));
            }
            if (this.id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdt.id = ");
                filterBuilder.Append(Convert.ToInt32(this.id));
            }
            return filterBuilder.ToString();
        }

        private String level;
        private String name;
        private String showMap;
        private String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        public String ShowMap
        {
            get { return showMap; }
            set { showMap = value; }
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Level
        {
            get { return level; }
            set { level = value; }
        }
    }
}