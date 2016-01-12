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
    public class QuestionServices : BaseServices
    {
        public int insert(QuestionModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_question ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("full_name, ");
            sqlBuilder.Append("email, ");
            sqlBuilder.Append("phone, ");
            sqlBuilder.Append("address, ");
            sqlBuilder.Append("department, ");
            sqlBuilder.Append("title, ");
            sqlBuilder.Append("category_id, ");
            sqlBuilder.Append("attach_file_name, ");
            sqlBuilder.Append("attach_file_path, ");
            sqlBuilder.Append("question_content, ");
            sqlBuilder.Append("question_datetime, ");
            sqlBuilder.Append("hidden_flg ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.FullName);
            objCmd.Parameters.AddWithValue("@2", item.Email);
            objCmd.Parameters.AddWithValue("@3", item.Phone);
            objCmd.Parameters.AddWithValue("@4", item.Address);
            objCmd.Parameters.AddWithValue("@5", item.Department);
            objCmd.Parameters.AddWithValue("@6", item.Title);
            objCmd.Parameters.AddWithValue("@7", item.CategoryId);
            objCmd.Parameters.AddWithValue("@8", item.AttachFileName);
            objCmd.Parameters.AddWithValue("@9", item.AttachFilePath);
            objCmd.Parameters.AddWithValue("@10", item.QuestionContent);
            objCmd.Parameters.AddWithValue("@11", item.QuestionDatetime);
            objCmd.Parameters.AddWithValue("@12", item.HiddenFlg);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(QuestionModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_question ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("full_name = @1, ");
            sqlBuilder.Append("email = @2, ");
            sqlBuilder.Append("phone = @3, ");
            sqlBuilder.Append("address = @4, ");
            sqlBuilder.Append("department = @5, ");
            sqlBuilder.Append("title = @6, ");
            sqlBuilder.Append("category_id = @7, ");
            sqlBuilder.Append("attach_file_name = @8, ");
            sqlBuilder.Append("attach_file_path = @9, ");
            sqlBuilder.Append("question_content = @10, ");
            sqlBuilder.Append("question_datetime = @11, ");
            sqlBuilder.Append("hidden_flg = @13 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @12 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.FullName);
            objCmd.Parameters.AddWithValue("@2", item.Email);
            objCmd.Parameters.AddWithValue("@3", item.Phone);
            objCmd.Parameters.AddWithValue("@4", item.Address);
            objCmd.Parameters.AddWithValue("@5", item.Department);
            objCmd.Parameters.AddWithValue("@6", item.Title);
            objCmd.Parameters.AddWithValue("@7", item.CategoryId);
            objCmd.Parameters.AddWithValue("@8", item.AttachFileName);
            objCmd.Parameters.AddWithValue("@9", item.AttachFilePath);
            objCmd.Parameters.AddWithValue("@10", item.QuestionContent);
            objCmd.Parameters.AddWithValue("@11", item.QuestionDatetime);
            objCmd.Parameters.AddWithValue("@13", item.HiddenFlg);
            objCmd.Parameters.AddWithValue("@12", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<QuestionModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_question gq");
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
            List<QuestionModels> lstDocument = new List<QuestionModels>();
            QuestionModels item;
            while (dataReader.Read())
            {
                item = new QuestionModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.FullName = dataReader["full_name"].ToString();
                item.Email = dataReader["email"].ToString();
                item.Phone = dataReader["phone"].ToString();
                item.Address = dataReader["address"].ToString();
                item.Department = dataReader["department"].ToString();
                item.Title = dataReader["title"].ToString();
                item.CategoryId = Convert.ToInt32(dataReader["category_id"]);
                item.AttachFileName = dataReader["attach_file_name"].ToString();
                item.AttachFilePath = dataReader["attach_file_path"].ToString();
                item.QuestionContent = dataReader["question_content"].ToString();
                item.QuestionDatetime = Convert.ToDateTime(dataReader["question_datetime"]);
                item.HiddenFlg = Convert.ToBoolean(dataReader["hidden_flg"]);
                lstDocument.Add(item);
            }
            getConnection().Close();
            return lstDocument;
        }
        public int delete(int id)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_question ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @1 ");
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
            sqlBuilder.Append("gov_question gq ");
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
                filterBuilder.Append(" gq.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.QuestionInAnswer != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gq.id IN ( SELECT question_id FROM gov_answer ) ");
            }
            if (this.HiddenFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gq.hidden_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.HiddenFlg));
            }
            if (this.QuestionCategoryId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gq.category_id = ");
                filterBuilder.Append(Convert.ToInt32(this.QuestionCategoryId));
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gq.question_content LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gq.title LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String id;
        private String keySearch;
        private String questionInAnswer;
        private String hiddenFlg;
        private String questionCategoryId;

        public String QuestionCategoryId
        {
            get { return questionCategoryId; }
            set { questionCategoryId = value; }
        }

        public String HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public String QuestionInAnswer
        {
            get { return questionInAnswer; }
            set { questionInAnswer = value; }
        }

        public String KeySearch
        {
            get { return keySearch; }
            set { keySearch = value; }
        }

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}