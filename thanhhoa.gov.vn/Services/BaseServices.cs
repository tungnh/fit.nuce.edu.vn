using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace thanhhoa.gov.vn.Services
{
    public class BaseServices
    {
        public static string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        public static MySqlConnection connection;
        protected String sql;
        private String orderBy;

        public String OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public void addOrderBy(String field, String type) {
            this.orderBy = field + " " + type;
        }

        public MySqlConnection getConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionstring);
            }
            if (connection.State == ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                    return connection;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else
            {
                return connection;
            }
        }

        protected void appendAnd(StringBuilder strBuffer)
        {
            strBuffer.Append(" AND ");
        }

        protected void appendOr(StringBuilder strBuffer)
        {
            strBuffer.Append(" OR ");
        }

        protected String pareLikeString(String str, int type)
        {
            switch(type){
                case 1:
                    str = "'%" + str + "'";
                    break;
                case 2:
                    str = "'%" + str + "%'";
                    break;
                case 3:
                    str = str + "%'";
                    break;
                default:
                    str = "'" + str + "'";
                    break;
            }
            return str;
        }

        protected String Sql
        {
            get { return sql; }
            set { sql = value; }
        }
    }
}