using thanhhoa.gov.vn.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Services
{
    public class DocumentsServies : BaseServices
    {
        public DocumentsServies() {
        }

        public int insert(DocumentModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_doc_draft ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("doc_code, ");
            sqlBuilder.Append("doc_name, ");
            sqlBuilder.Append("doc_summany, ");
            sqlBuilder.Append("doc_content, ");
            sqlBuilder.Append("kind_id, ");
            sqlBuilder.Append("department_id, ");
            sqlBuilder.Append("date_start_promulgate, ");
            sqlBuilder.Append("date_end_promulgate, ");
            sqlBuilder.Append("user_sign, ");
            sqlBuilder.Append("active_flg, ");
            sqlBuilder.Append("note, ");
            sqlBuilder.Append("attach_file_name, ");
            sqlBuilder.Append("attach_file_path, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("update_datetime ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15");
            sqlBuilder.Append(")");
            
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.DocCode);
            objCmd.Parameters.AddWithValue("@2", item.DocName);
            objCmd.Parameters.AddWithValue("@3", item.DocSummany);
            objCmd.Parameters.AddWithValue("@4", null);
            objCmd.Parameters.AddWithValue("@5", item.KindId);
            objCmd.Parameters.AddWithValue("@6", item.DepartmentId);
            objCmd.Parameters.AddWithValue("@7", item.DateStartpromulgate);
            objCmd.Parameters.AddWithValue("@8", item.DateEndPromulgate);
            objCmd.Parameters.AddWithValue("@9", item.UserSign);
            objCmd.Parameters.AddWithValue("@10", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@11", item.Note);
            objCmd.Parameters.AddWithValue("@12", item.AttachFileName);
            objCmd.Parameters.AddWithValue("@13", item.AttachFilePath);
            objCmd.Parameters.AddWithValue("@14", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@15", item.UpdateDatetime);
            objCmd.ExecuteNonQuery();

            objCmd.Parameters.Clear();
            objCmd.CommandText = "SELECT @@IDENTITY";

            int identity = Convert.ToInt32(objCmd.ExecuteScalar());
            return identity;
        }

        public int update(DocumentModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_doc_draft ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("doc_code = @1, ");
            sqlBuilder.Append("doc_name = @2, ");
            sqlBuilder.Append("doc_summany = @3, ");
            sqlBuilder.Append("doc_content = @4, ");
            sqlBuilder.Append("kind_id = @5, ");
            sqlBuilder.Append("department_id = @6, ");
            sqlBuilder.Append("date_start_promulgate = @7, ");
            sqlBuilder.Append("date_end_promulgate = @8, ");
            sqlBuilder.Append("user_sign = @9, ");
            sqlBuilder.Append("active_flg = @10, ");
            sqlBuilder.Append("note = @11, ");
            sqlBuilder.Append("attach_file_name = @12, ");
            sqlBuilder.Append("attach_file_path = @13, ");
            sqlBuilder.Append("update_username = @14, ");
            sqlBuilder.Append("update_datetime = @15 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @16");

            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.DocCode);
            objCmd.Parameters.AddWithValue("@2", item.DocName);
            objCmd.Parameters.AddWithValue("@3", item.DocSummany);
            objCmd.Parameters.AddWithValue("@4", null);
            objCmd.Parameters.AddWithValue("@5", item.KindId);
            objCmd.Parameters.AddWithValue("@6", item.DepartmentId);
            objCmd.Parameters.AddWithValue("@7", item.DateStartpromulgate);
            objCmd.Parameters.AddWithValue("@8", item.DateEndPromulgate);
            objCmd.Parameters.AddWithValue("@9", item.UserSign);
            objCmd.Parameters.AddWithValue("@10", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@11", item.Note);
            objCmd.Parameters.AddWithValue("@12", item.AttachFileName);
            objCmd.Parameters.AddWithValue("@13", item.AttachFilePath);
            objCmd.Parameters.AddWithValue("@14", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@15", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@16", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<DocumentModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            DocumentModels item;
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_draft gdd ");
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
            List<DocumentModels> lstDocument = new List<DocumentModels>();
            while (dataReader.Read())
            {
                item = new DocumentModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.DocName = dataReader["doc_name"].ToString();
                item.DocCode = dataReader["doc_code"].ToString();
                item.DocContent = dataReader["doc_content"].ToString();
                item.DocSummany = dataReader["doc_summany"].ToString();
                item.DepartmentId = Convert.ToInt32(dataReader["department_id"]);
                item.KindId = Convert.ToInt32(dataReader["kind_id"]);
                item.UserSign = dataReader["user_sign"].ToString();
                item.DateStartpromulgate = Convert.ToDateTime(dataReader["date_start_promulgate"]);
                item.DateEndPromulgate = Convert.ToDateTime(dataReader["date_end_promulgate"]);
                item.AttachFileName = dataReader["attach_file_name"].ToString();
                item.AttachFilePath = dataReader["attach_file_path"].ToString();
                lstDocument.Add(item);
            }
            getConnection().Close();
            return lstDocument;
        }

        //public List<DocumentModels> selectById(int id)
        //{
        //    StringBuilder sqlBuilder = new StringBuilder();
        //    DocumentModels item;
        //    sqlBuilder.Append("SELECT ");
        //    sqlBuilder.Append("attach_file_name, ");
        //    sqlBuilder.Append("attach_file_path, ");
        //    sqlBuilder.Append("doc_content ");
        //    sqlBuilder.Append("FROM ");
        //    sqlBuilder.Append("gov_doc_draft ");
        //    sqlBuilder.Append("WHERE ");
        //    sqlBuilder.Append("id = @1");
        //    this.Sql = sqlBuilder.ToString();

        //    MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());

        //    objCmd.Parameters.AddWithValue("@1", id);
        //    MySqlDataReader dataReader = objCmd.ExecuteReader();
        //    List<DocumentModels> lstDocument = new List<DocumentModels>();
        //    while (dataReader.Read())
        //    {
        //        item = new DocumentModels();
        //        item.AttachFileName = dataReader.GetValue(0).ToString();
        //        item.AttachFilePath = dataReader.GetValue(1).ToString();
        //        item.DocContent = CreateIndex.parseToFormat(dataReader.GetValue(2).ToString());
        //        lstDocument.Add(item);
        //    }
        //    getConnection().Close();
        //    return lstDocument;
        //}

        public int totalCount() {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("COUNT(*) ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_draft gdd ");
            sqlBuilder.Append(getBaseSQL());
            this.Sql = sqlBuilder.ToString();

            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            int total = 0;
            while (dataReader.Read())
            {
                total = Convert.ToInt32(dataReader.GetValue(0));
            }
            getConnection().Close();
            return total;
        }

        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_doc_draft ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id=@1 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", id);
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
            if (this.idFilter != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdd.id = ");
                filterBuilder.Append(Convert.ToInt32(this.idFilter));
            }
            if (this.documentKindId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdd.kind_id = ");
                filterBuilder.Append(Convert.ToInt32(this.documentKindId));
            }
            if (this.departmentId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gdd.department_id = ");
                filterBuilder.Append(Convert.ToInt32(this.departmentId));
            }
            if (this.keySearch != null)
            {
                appendAnd(filterBuilder);

                filterBuilder.Append(" ( ");
                filterBuilder.Append(" gdd.doc_name LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gdd.doc_summany LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gdd.user_sign LIKE ");
                filterBuilder.Append(pareLikeString(this.keySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String idFilter;
        private String nameFilter;
        private String documentKindId;
        private String departmentId;
        private String keySearch;

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        public String DocumentKindId
        {
            get { return documentKindId; }
            set { documentKindId = value; }
        }

        public String NameFilter
        {
            get { return nameFilter; }
            set { nameFilter = value; }
        }

        public String IdFilter
        {
            get { return idFilter; }
            set { idFilter = value; }
        }
    }
}