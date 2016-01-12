using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [HandleError]
    public class BaseController : Controller
    {
        protected CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
        public BaseController(){
        }

        public void insertHistory(AccessType action, String description)
        {
            gov_access_history accessHistory = new gov_access_history();
            accessHistory.time_access = DateTime.Now;
            accessHistory.type_access = (int)action;
            accessHistory.username_access = Session.getCurrentUser().username;
            accessHistory.description = description;
            accessHistory.client_info = Utils.GetLanIPAddress();
            accessHistory.public_info = Utils.GetPublicIPAddress();
            _cnttDB.gov_access_history.Add(accessHistory);
            _cnttDB.SaveChanges();
        }
        
        public string markMatch(string input, string query)
        {
            return Regex.Replace(input, query, delegate(Match match)
            {
                return "<b style='color:black;background:yellow;'>" + match.ToString() + "</b>";
            }, RegexOptions.IgnoreCase);
        }

        public int pageCalculation(int totalCount, int lineMax)
        {
            int totalPage = totalCount / lineMax;
            if (0 < (totalCount % lineMax))
            {
                ++totalPage;
            }
            return totalPage;
        }

        public int pageTransition(String direction, int page, int totalPage)
        {
            if (direction == null)
            {
                direction = "";
            }

            if (direction.Equals("first"))
            {
                page = 1;
            }
            else if (direction.Equals("ahead"))
            {
                if (1 > (page - 1))
                {
                    page = totalPage;
                }
                else
                {
                    --page;
                }
            }
            else if (direction.Equals("next"))
            {
                if (totalPage < (page + 1))
                {
                    page = 1;
                }
                else
                {
                    ++page;
                }
            }
            else if (direction.Equals("end"))
            {
                page = totalPage;
            }
            else
            {
                if (page > totalPage)
                {
                    page = totalPage;
                }
            }

            return page;
        }

        public int fistPageCalculation(int maxPageLine, int totalPage, int page)
        {
            int firstPage;
            if (totalPage <= maxPageLine)
            {
                firstPage = 1;
            }
            else
            {
                int firstLinePage, lastLinePage;
                int middlePage = maxPageLine / 2;
                if (maxPageLine % 2 != 0)
                {
                    middlePage = middlePage + 1;
                }
                firstLinePage = middlePage - 1;
                lastLinePage = maxPageLine - middlePage;
                if ((page + lastLinePage) >= totalPage)
                {
                    firstPage = totalPage - maxPageLine + 1;
                }
                else
                {
                    if ((page - firstLinePage) > 1)
                    {
                        firstPage = page - firstLinePage;
                    }
                    else
                    {
                        firstPage = 1;
                    }
                }
            }
            return firstPage;
        }

        public int lastPageCalculation(int maxPageLine, int totalPage, int page, int firstPage)
        {
            int lastPage;
            if (totalPage <= maxPageLine)
            {
                lastPage = totalPage;
            }
            else
            {
                if ((firstPage + maxPageLine) > totalPage)
                {
                    lastPage = totalPage;
                }
                else
                {
                    lastPage = firstPage + maxPageLine - 1;
                }
            }
            return lastPage;
        }
    }
}
