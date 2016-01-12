using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Models
{
    public class DistrictLevelModels
    {
        private int id;
        private String name;
        private int orderNumber;
        private String updateUserId;
        private DateTime updateDatetime;
        private List<DistrictModels> lstDistrict;

        public List<DistrictModels> LstDistrict
        {
            get { return lstDistrict; }
            set { lstDistrict = value; }
        }

        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public String UpdateUserId
        {
            get { return updateUserId; }
            set { updateUserId = value; }
        }

        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}