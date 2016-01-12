using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Services
{
    public class DistrictLevelServices : BaseServices
    {
        public List<DistrictLevelModels> select(int page, int limit)
        {
            int offset = (page - 1) * limit;
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");
            sqlBuilder.Append("* ");
            sqlBuilder.Append("FROM ");
            sqlBuilder.Append("gov_level_district gdtlv ");
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
            List<DistrictLevelModels> lstMenu = new List<DistrictLevelModels>();
            DistrictLevelModels item;
            while (dataReader.Read())
            {
                item = new DistrictLevelModels();
                item.Id = Convert.ToInt32(dataReader["id"]);
                item.Name = dataReader["name"].ToString();
                //item.OrderNumber = Convert.ToInt32(dataReader["order_number"]);
                //item.Decription = dataReader["decription"].ToString();
                //item.Link = dataReader["link"].ToString();
                //item.ActiveFlg = Convert.ToBoolean(dataReader["active_flg"]);
                //item.EntryDatetime = Convert.ToDateTime(dataReader["entry_datetime"]);
                lstMenu.Add(item);
            }
            getConnection().Close();
            return lstMenu;
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
            return filterBuilder.ToString();
        }
    }
}