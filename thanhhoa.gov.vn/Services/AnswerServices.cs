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
    public class AnswerServices : BaseServices
    {
        public int insert(AnswerModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_answer ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("question_id, ");
            sqlBuilder.Append("answer_content, ");
            sqlBuilder.Append("answer_username, ");
            sqlBuilder.Append("update_username, ");
            sqlBuilder.Append("answer_datetime, ");
            sqlBuilder.Append("update_datetime, ");
            sqlBuilder.Append("hidden_flg ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.QuestionId);
            objCmd.Parameters.AddWithValue("@2", item.AnswerContent);
            objCmd.Parameters.AddWithValue("@3", item.AnswerUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@5", item.AnswerDatetime);
            objCmd.Parameters.AddWithValue("@6", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@7", item.HiddenFlg);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(AnswerModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_answer ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("question_id = @1, ");
            sqlBuilder.Append("answer_content = @2, ");
            sqlBuilder.Append("answer_username = @3, ");
            sqlBuilder.Append("update_username = @4, ");
            sqlBuilder.Append("answer_datetime = @5, ");
            sqlBuilder.Append("update_datetime = @6, ");
            sqlBuilder.Append("hidden_flg = @7 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @8 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.QuestionId);
            objCmd.Parameters.AddWithValue("@2", item.AnswerContent);
            objCmd.Parameters.AddWithValue("@3", item.AnswerUsername);
            objCmd.Parameters.AddWithValue("@4", item.UpdateUsername);
            objCmd.Parameters.AddWithValue("@5", item.AnswerDatetime);
            objCmd.Parameters.AddWithValue("@6", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@7", item.HiddenFlg);
            objCmd.Parameters.AddWithValue("@8", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<AnswerModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_answer gaw");
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
            List<AnswerModels> lstDocument = new List<AnswerModels>();
            AnswerModels item;
            while (dataReader.Read())
            {
                item = new AnswerModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.AnswerContent = dataReader["answer_content"].ToString();
                item.QuestionId = Convert.ToInt32(dataReader["question_id"]);
                item.UpdateUsername = dataReader["update_username"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                item.AnswerDatetime = Convert.ToDateTime(dataReader["answer_datetime"]);
                item.AnswerUsername = dataReader["answer_username"].ToString();
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
            sqlBuilder.Append("gov_answer ");
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
            sqlBuilder.Append("gov_answer gaw ");
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
                filterBuilder.Append(" gaw.id = ");
                filterBuilder.Append(Convert.ToInt32(this.Id));
            }
            if (this.QuestionId != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gaw.question_id = ");
                filterBuilder.Append(Convert.ToInt32(this.QuestionId));
            }
            if (this.HiddenFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gaw.hidden_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.HiddenFlg));
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gaw.category_name LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" gaw.description LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String id;
        private String keySearch;
        private String questionId;
        private String hiddenFlg;

        public String HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

        public String QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
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