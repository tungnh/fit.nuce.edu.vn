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
    public class DocumentKindServices : BaseServices
    {
        public DocumentKindServices()
        {
        }

        public int insert(DocumentKindModels item) {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_doc_kind ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("kcode, ");
            sqlBuilder.Append("name, ");
            sqlBuilder.Append("description, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.KCode);
            objCmd.Parameters.AddWithValue("@2", item.Name);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@5", item.UpdateDatetime);
            int rs = objCmd.ExecuteNonQuery();

            return rs;
        }

        public int update(DocumentKindModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_doc_kind ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("kcode = @1, ");
            sqlBuilder.Append("name = @2, ");
            sqlBuilder.Append("description = @3, ");
            sqlBuilder.Append("update_username = @4, ");
            sqlBuilder.Append("update_datetime = @5 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("kid = @6 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.KCode);
            objCmd.Parameters.AddWithValue("@2", item.Name);
            objCmd.Parameters.AddWithValue("@3", item.Description);
            objCmd.Parameters.AddWithValue("@4", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@5", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@6", item.Kid);
            int rs = objCmd.ExecuteNonQuery();

            return rs;
        }

        public List<DocumentKindModels> select(int page, int limit) {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_kind gdk ");
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
            List<DocumentKindModels> lstDocumentKind = new List<DocumentKindModels>();
            DocumentKindModels item;
            while (dataReader.Read())
            {
                item = new DocumentKindModels();
                item.Kid = Convert.ToInt32(dataReader["kid"]);
                item.KCode = dataReader["kcode"].ToString();
                //item.ProcessPeriod = Convert.ToInt32(dataReader[""].ToString());
                item.Name = dataReader["name"].ToString();
                item.Description = dataReader["description"].ToString();
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                lstDocumentKind.Add(item);
            }
            getConnection().Close();
            return lstDocumentKind;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_kind ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("kid = @1 ");
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
            int rs = objCmd.ExecuteNonQuery();
            getConnection().Close();
            return rs;
        }

        public int totalCount()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_kind gdk ");
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

        private String getBaseSQL()
        {
            StringBuilder filterBuilder = new StringBuilder();
		    String whereString = getWhereFilter();
		    if (whereString.Length > 0) {
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
                filterBuilder.Append(" gdk.kid = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gdk.name LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gdk.kcode LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

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
    }
}