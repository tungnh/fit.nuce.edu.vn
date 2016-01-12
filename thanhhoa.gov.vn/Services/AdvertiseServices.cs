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
    public class AdvertiseServices : BaseServices
    {
        public int insert(AdvertiseModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_advertise ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("attach_file, ");
            sqlBuilder.Append("link, ");
            sqlBuilder.Append("type_link, ");
            sqlBuilder.Append("department, ");
            sqlBuilder.Append("price, ");
            sqlBuilder.Append("note, ");
            sqlBuilder.Append("order_number, ");
            sqlBuilder.Append("begin_date, ");
            sqlBuilder.Append("end_date, ");
            sqlBuilder.Append("location, ");
            sqlBuilder.Append("active_flg, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.AttachFile);
            objCmd.Parameters.AddWithValue("@3", item.Link);
            objCmd.Parameters.AddWithValue("@4", item.TypeLink);
            objCmd.Parameters.AddWithValue("@5", item.Department);
            objCmd.Parameters.AddWithValue("@6", item.Price);
            objCmd.Parameters.AddWithValue("@7", item.Note);
            objCmd.Parameters.AddWithValue("@8", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@9", item.StartDate);
            objCmd.Parameters.AddWithValue("@10", item.EndDate);
            objCmd.Parameters.AddWithValue("@11", item.Location);
            objCmd.Parameters.AddWithValue("@12", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@13", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@14", item.UpdateDatetime);

            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int Update(AdvertiseModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_advertise ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("title = @1, ");
            sqlBuilder.Append("attach_file = @2, ");
            sqlBuilder.Append("link = @3, ");
            sqlBuilder.Append("type_link = @4, ");
            sqlBuilder.Append("department = @5, ");
            sqlBuilder.Append("price = @6, ");
            sqlBuilder.Append("note = @7, ");
            sqlBuilder.Append("order_number = @8, ");
            sqlBuilder.Append("begin_date = @9, ");
            sqlBuilder.Append("end_date = @10, ");
            sqlBuilder.Append("location = @11, ");
            sqlBuilder.Append("active_flg = @12, ");
            sqlBuilder.Append("update_username = @13, ");
            sqlBuilder.Append("update_datetime = @14 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @15");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Title);
            objCmd.Parameters.AddWithValue("@2", item.AttachFile);
            objCmd.Parameters.AddWithValue("@3", item.Link);
            objCmd.Parameters.AddWithValue("@4", item.TypeLink);
            objCmd.Parameters.AddWithValue("@5", item.Department);
            objCmd.Parameters.AddWithValue("@6", item.Price);
            objCmd.Parameters.AddWithValue("@7", item.Note);
            objCmd.Parameters.AddWithValue("@8", item.OrderNumber);
            objCmd.Parameters.AddWithValue("@9", item.StartDate);
            objCmd.Parameters.AddWithValue("@10", item.EndDate);
            objCmd.Parameters.AddWithValue("@11", item.Location);
            objCmd.Parameters.AddWithValue("@12", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@13", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@14", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@15", item.Id);

            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_advertise ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public List<AdvertiseModels> select(int page, int limit)
        {

            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_advertise gat ");
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

            List<AdvertiseModels> lstAdvertise = new List<AdvertiseModels>();
            AdvertiseModels item;
            try
            {
                MySqlDataReader dataReader = objCmd.ExecuteReader();
                while (dataReader.Read())
                {
                    item = new AdvertiseModels();
                    item.Id = Convert.ToInt32(dataReader["id"]);
                    item.Title = dataReader["title"].ToString();
                    item.AttachFile = dataReader["attach_file"].ToString();
                    item.Link = dataReader["link"].ToString();
                    item.TypeLink = Convert.ToBoolean(dataReader["type_link"]);
                    item.Department = dataReader["department"].ToString();
                    item.Price = Convert.ToInt32(dataReader["price"]);
                    item.Note = dataReader["note"].ToString();
                    item.OrderNumber = Convert.ToInt32(dataReader["order_number"]);
                    item.StartDate = Convert.ToDateTime(dataReader["begin_date"]);
                    item.EndDate = Convert.ToDateTime(dataReader["end_date"]);
                    item.Location = Convert.ToInt32(dataReader["location"]);
                    item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                    item.UpdateUsername = dataReader["update_username"].ToString();
                    item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                    lstAdvertise.Add(item);
                }
                getConnection().Close();
            }
            catch (Exception ex)
            {
                getConnection().Close();
            }
            return lstAdvertise;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_advertise gat");
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
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gat.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.Location != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gat.location = ");
                filterBuilder.Append(Convert.ToInt32(this.Location));
            }
            if (this.ActiveFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gat.active_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.ActiveFlg));
            }
            if (this.keySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" (");

                filterBuilder.Append(" gat.title LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gat.department LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gat.note LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }

            if (this.DateFrom != null) {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gat.begin_date <= ");
                filterBuilder.Append(pareLikeString(this.DateFrom, Constant.FILTER_KIND_FULL));

                appendAnd(filterBuilder);
                filterBuilder.Append(" gat.end_date >= ");
                filterBuilder.Append(pareLikeString(this.DateFrom, Constant.FILTER_KIND_FULL));
            }
            return filterBuilder.ToString();
        }

        private String id;
        private String location;
        private String keySearch;
        private String dateFrom;
        private String activeFlg;

        public String ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}