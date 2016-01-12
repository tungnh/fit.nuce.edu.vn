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
    public class StatisticAccessServices : BaseServices
    {
        public int insert(StatisticAccessModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_statistic_access ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("date, ");
            sqlBuilder.Append("count ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Date);
            objCmd.Parameters.AddWithValue("@2", item.Count);
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int Update(StatisticAccessModels item)
        {
            int rs = 0;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" UPDATE ");
            sqlBuilder.Append(" gov_statistic_access ");
            sqlBuilder.Append(" SET ");
            sqlBuilder.Append(" count = ");
            sqlBuilder.Append(item.Count);
            sqlBuilder.Append(" WHERE ");
            sqlBuilder.Append(" date = '" + item.Date.ToString("yyyy/MM/dd")  + "'");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int selectCount(String date)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("count ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_statistic_access ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("date = ");
            sqlBuilder.Append(pareLikeString(date, Constant.FILTER_KIND_FULL));
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

            MySqlDataReader dataReader = objCmd.ExecuteReader();
            int count = 0;
            while (dataReader.Read())
            {
                count += Convert.ToInt32(dataReader["count"]);
            }
            getConnection().Close();
            return count;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_statistic_access gsa ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            int totalCount = 0;
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            while (dataReader.Read())
            {
                totalCount += Convert.ToInt32(dataReader["count"]);
            }
            getConnection().Close();
            return totalCount;
        }
    }
}