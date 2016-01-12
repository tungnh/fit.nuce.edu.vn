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
    public class CalendarServices : BaseServices
    {
        public int insert(CalendarModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_work_schedule ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("date, ");
            sqlBuilder.Append("time, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("location, ");
            sqlBuilder.Append("person_execute, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Date);
            objCmd.Parameters.AddWithValue("@2", item.Time);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.Location);
            objCmd.Parameters.AddWithValue("@5", item.PersonExecute);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
           
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int Update(CalendarModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_work_schedule ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("date = @1, ");
            sqlBuilder.Append("time = @2, ");
            sqlBuilder.Append("description = @3, ");
            sqlBuilder.Append("location = @4, ");
            sqlBuilder.Append("person_execute = @5, ");
            sqlBuilder.Append("update_username = @6, ");
            sqlBuilder.Append("update_datetime = @7 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @8");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            
            objCmd.Parameters.AddWithValue("@1", item.Date);
            objCmd.Parameters.AddWithValue("@2", item.Time);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.Location);
            objCmd.Parameters.AddWithValue("@5", item.PersonExecute);
            objCmd.Parameters.AddWithValue("@6", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@7", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@8", item.Id);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_work_schedule ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public List<CalendarModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("gws.*, gt.name ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_work_schedule gws ");
            sqlBuilder.Append("INNER JOIN ");
            sqlBuilder.Append("gov_work_time gt ");
            sqlBuilder.Append("ON ");
            sqlBuilder.Append("gws.time = gt.id ");
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
            List<CalendarModels> lstCalendar= new List<CalendarModels>();
            CalendarModels item;
            while (dataReader.Read())
            {
                item = new CalendarModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Date = Convert.ToDateTime(dataReader["date"]);
                item.Time = dataReader["name"].ToString();
                item.TimeRf = Convert.ToInt32(dataReader["time"]);
                item.Description = dataReader["description"].ToString();
                item.Location = dataReader["location"].ToString();
                item.PersonExecute = dataReader["person_execute"].ToString();
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                lstCalendar.Add(item);
            }
            getConnection().Close();
            return lstCalendar;
        }

        public List<WorktimeModels> selectTime()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("*");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_work_time gwt ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<WorktimeModels> lstTime = new List<WorktimeModels>();
            WorktimeModels item;
            while (dataReader.Read())
            {
                item = new WorktimeModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Name = dataReader["name"].ToString();
                lstTime.Add(item);
            }
            getConnection().Close();
            return lstTime;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_work_schedule gws ");
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
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gws.description LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gws.location LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gws.person_execute LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gws.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.FromDate != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gws.date >= ");
                filterBuilder.Append(pareLikeString(this.FromDate, Constant.FILTER_KIND_FULL));
            }
            if (this.ToDate != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gws.date <= ");
                filterBuilder.Append(pareLikeString(this.ToDate, Constant.FILTER_KIND_FULL));
            }
            return filterBuilder.ToString();
        }

        private String fromDate;
        private String toDate;
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

        public String ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        public String FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
    }
}