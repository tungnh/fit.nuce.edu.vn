using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Services
{
    public class UserServices : BaseServices
    {
        public UserServices()
        {
        }

        public int insert(UserModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT ");
            sqlBuilder.Append("INTO ");
            sqlBuilder.Append("gov_user ");
            sqlBuilder.Append("( ");
            sqlBuilder.Append("username, ");
            sqlBuilder.Append("password, ");
            sqlBuilder.Append("family_name, ");
            sqlBuilder.Append("first_name, ");
            sqlBuilder.Append("birth_day, ");
            sqlBuilder.Append("sex, ");
            sqlBuilder.Append("mobile, ");
            sqlBuilder.Append("phone, ");
            sqlBuilder.Append("address, ");
            sqlBuilder.Append("email, ");
            sqlBuilder.Append("active_flg, ");
            sqlBuilder.Append("hidden_flg, ");
            sqlBuilder.Append("update_user_name, ");
            sqlBuilder.Append("update_datetime, ");
            sqlBuilder.Append("role_id, ");
            sqlBuilder.Append("district_id ");
            sqlBuilder.Append(") ");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @14, @15, @16, @17");
            sqlBuilder.Append(")");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            objCmd.Parameters.AddWithValue("@1", item.Username);
            objCmd.Parameters.AddWithValue("@2", item.Password);
            objCmd.Parameters.AddWithValue("@3", item.FamilyName);
            objCmd.Parameters.AddWithValue("@4", item.FirstName);
            objCmd.Parameters.AddWithValue("@5", item.BirthDay);
            objCmd.Parameters.AddWithValue("@6", item.Sex);
            objCmd.Parameters.AddWithValue("@7", item.Mobile);
            objCmd.Parameters.AddWithValue("@8", item.Phone);
            objCmd.Parameters.AddWithValue("@9", item.Address);
            objCmd.Parameters.AddWithValue("@10", item.Email);
            objCmd.Parameters.AddWithValue("@11", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@12", item.HiddenFlg);
            objCmd.Parameters.AddWithValue("@14", item.UpdateUserName);
            objCmd.Parameters.AddWithValue("@15", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@16", item.RoleId);
            objCmd.Parameters.AddWithValue("@17", item.DistrictId);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public int update(UserModels item)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE ");
            sqlBuilder.Append("gov_user ");
            sqlBuilder.Append("SET ");
            sqlBuilder.Append("password = @1, ");
            sqlBuilder.Append("family_name = @2, ");
            sqlBuilder.Append("first_name = @3, ");
            sqlBuilder.Append("birth_day = @4, ");
            sqlBuilder.Append("sex = @5, ");
            sqlBuilder.Append("mobile = @6, ");
            sqlBuilder.Append("phone = @7, ");
            sqlBuilder.Append("address = @8, ");
            sqlBuilder.Append("email = @9, ");
            sqlBuilder.Append("active_flg = @10, ");
            sqlBuilder.Append("hidden_flg = @11, ");
            sqlBuilder.Append("update_user_name = @13, ");
            sqlBuilder.Append("update_datetime = @14, ");
            sqlBuilder.Append("role_id = @15, ");
            sqlBuilder.Append("district_id = @16 ");
            sqlBuilder.Append("WHERE ");
            sqlBuilder.Append("id = @17 ");
            this.Sql = sqlBuilder.ToString();
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            
            objCmd.Parameters.AddWithValue("@1", item.Password);
            objCmd.Parameters.AddWithValue("@2", item.FamilyName);
            objCmd.Parameters.AddWithValue("@3", item.FirstName);
            objCmd.Parameters.AddWithValue("@4", item.BirthDay);
            objCmd.Parameters.AddWithValue("@5", item.Sex);
            objCmd.Parameters.AddWithValue("@6", item.Mobile);
            objCmd.Parameters.AddWithValue("@7", item.Phone);
            objCmd.Parameters.AddWithValue("@8", item.Address);
            objCmd.Parameters.AddWithValue("@9", item.Email);
            objCmd.Parameters.AddWithValue("@10", item.ActiveFlg);
            objCmd.Parameters.AddWithValue("@11", item.HiddenFlg);
            objCmd.Parameters.AddWithValue("@13", item.UpdateUserName);
            objCmd.Parameters.AddWithValue("@14", item.UpdateDatetime);
            objCmd.Parameters.AddWithValue("@15", item.RoleId);
            objCmd.Parameters.AddWithValue("@16", item.DistrictId);
            objCmd.Parameters.AddWithValue("@17", item.Id);
            int rs = objCmd.ExecuteNonQuery();
            return rs;
        }

        public List<UserModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_user gu ");
            sqlBuilder.Append("INNER JOIN ");
            sqlBuilder.Append("gov_role gr ");
            sqlBuilder.Append("ON ");
            sqlBuilder.Append("gu.role_id = gr.id ");
            sqlBuilder.Append(getBaseSQL());
            if(page > 0 && limit > 0){
                sqlBuilder.Append(" LIMIT @limit ");
                sqlBuilder.Append(" OFFSET @offset ");
            }
            this.Sql = sqlBuilder.ToString();
            
            MySqlCommand objCmd = new MySqlCommand(Sql, getConnection());
            if(page > 0 && limit > 0){
                objCmd.Parameters.AddWithValue("@limit", limit);
                objCmd.Parameters.AddWithValue("@offset", offset);
            }
            
            MySqlDataReader dataReader = objCmd.ExecuteReader();
            List<UserModels> lstDocument = new List<UserModels>();
            UserModels item;
            while (dataReader.Read())
            {
                item = new UserModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Username = dataReader["username"].ToString();
                item.Password = dataReader["password"].ToString();
                item.FamilyName = dataReader["family_name"].ToString();
                item.FirstName = dataReader["first_name"].ToString();
                item.BirthDay = dataReader["birth_day"].ToString();
                item.Sex = Convert.ToBoolean(dataReader["sex"]);
                item.Mobile = dataReader["mobile"].ToString();
                item.Phone = dataReader["phone"].ToString();
                item.Address = dataReader["address"].ToString();
                item.Email = dataReader["email"].ToString();
                item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                item.HiddenFlg = Convert.ToBoolean(dataReader["hidden_flg"]);
                item.UpdateUserName = dataReader["update_user_name"].ToString();
                item.UpdateDatetime = Convert.ToDateTime(dataReader["update_datetime"]);
                item.RoleId = Convert.ToInt32(dataReader["role_id"]);
                item.RoleName = dataReader["role_name"].ToString();
                item.DistrictId = Convert.ToInt32(dataReader["district_id"]);
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
            sqlBuilder.Append("gov_user ");
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
            sqlBuilder.Append("gov_user gu ");
            sqlBuilder.Append("INNER JOIN ");
            sqlBuilder.Append("gov_role gr ");
            sqlBuilder.Append("ON ");
            sqlBuilder.Append("gu.role_id = gr.id ");
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
            if (this.Username != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.username = ");
                filterBuilder.Append( pareLikeString(this.Username, Constant.FILTER_KIND_FULL));
            }
            if (this.Id != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.id = ");
                filterBuilder.Append(Convert.ToInt32 (this.Id));
            }
            if (this.HiddenFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.hidden_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.HiddenFlg));
            }
            if (this.ActiveFlg != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.active_flg = ");
                filterBuilder.Append(Convert.ToBoolean(this.activeFlg));
            }
            if (this.KeySearch != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" ( ");

                filterBuilder.Append(" gu.username LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                appendOr(filterBuilder);
                filterBuilder.Append(" CONCAT ( gu.family_name , gu.first_name ) LIKE ");
                filterBuilder.Append(pareLikeString(this.KeySearch, Constant.FILTER_KIND_MIDDLE));

                filterBuilder.Append(" ) ");
            }
            if (this.usernameNotInGroup != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.username NOT IN ");
                filterBuilder.Append(" ( ");
                filterBuilder.Append(" SELECT username from gov_group_members ");
                filterBuilder.Append(" WHERE ");
                filterBuilder.Append(" group_id = ");
                filterBuilder.Append(Convert.ToInt32(this.usernameNotInGroup));
                filterBuilder.Append(" ) ");
            }
            if (this.usernameInGroup != null)
            {
                appendAnd(filterBuilder);
                filterBuilder.Append(" gu.username IN ");
                filterBuilder.Append(" ( ");
                filterBuilder.Append(" SELECT username from gov_group_members ");
                filterBuilder.Append(" WHERE ");
                filterBuilder.Append(" group_id = ");
                filterBuilder.Append(Convert.ToInt32(this.usernameInGroup));
                filterBuilder.Append(" ) ");
            }
            return filterBuilder.ToString();
        }

        private String keySearch;
        private String id;
        private String hiddenFlg;
        private String activeFlg;
        private String username;
        private String usernameNotInGroup;
        private String usernameInGroup;

        public String UsernameInGroup
        {
            get { return usernameInGroup; }
            set { usernameInGroup = value; }
        }

        public String UsernameNotInGroup
        {
            get { return usernameNotInGroup; }
            set { usernameNotInGroup = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public String ActiveFlg
        {
            get { return activeFlg; }
            set { activeFlg = value; }
        }

        public String HiddenFlg
        {
            get { return hiddenFlg; }
            set { hiddenFlg = value; }
        }

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