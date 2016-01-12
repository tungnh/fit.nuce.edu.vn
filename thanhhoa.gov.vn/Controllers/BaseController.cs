using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Controllers
{
    [ValidateInput(false)]
    [HandleError]
    public class BaseController : Controller
    {
        protected CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
        public BaseController(){
            showAdvertise();
            showStatistic();
            showDefaultPage();
        }

        public void showDefaultPage()
        {
            ViewData["lstMenu"] = _cnttDB.gov_menu.Where(m => m.active_flg == true).OrderBy(m => m.order_number).ToList();
            //Show menu left
            ViewData["lstMenuLeft"] = _cnttDB.gov_menu.Where(s => s.isleft == true && s.active_flg == true).OrderBy(s => s.order_number).ToList();
            ViewData["lstAlbum"] = _cnttDB.gov_album.OrderByDescending(s => s.update_datetime).ToList();
            ViewData["lstVideo"] = _cnttDB.gov_video.OrderByDescending(s => s.update_datetime).Skip(0).Take(5).ToList();

            //SystemCOnfig
            ViewData["sysConfig"] = _cnttDB.gov_system_config.ToList();
        }

        public void showAdvertise() {
            DateTime date = DateTime.Now;
            ViewData["lstQuangCaoGiua"] = _cnttDB.gov_advertise.Where(a => a.begin_date <= date && date <= a.end_date && a.active_flg == true && a.location == 1).OrderBy(a => a.order_number).ToList();
            ViewData["lstQuangCaoBenPhai"] = _cnttDB.gov_advertise.Where(a => a.begin_date <= date && date <= a.end_date && a.active_flg == true && a.location == 2).OrderBy(a => a.order_number).ToList();
        }

        public void showStatistic() {
            int totalCount = 0;
            int totalCountInDay = 0;
            List<gov_statistic_access> lstStatistic = _cnttDB.gov_statistic_access.ToList();
            foreach (var item in lstStatistic)
            {
                totalCount += item.count;
            }
            ViewData["tongsotruycap"] = totalCount;
            String date = DateTime.Now.ToString("dd/MM/yyyy");
            gov_statistic_access statisticInDay = _cnttDB.gov_statistic_access.Where(s => s.date.Equals(date)).FirstOrDefault();
            if (statisticInDay != null)
                totalCountInDay = statisticInDay.count;
            ViewData["soluottruycaptrongngay"] = totalCountInDay;
        }

        protected string convertToUnSign3(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = input.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace('\u00D0', 'D').Replace('\u0089', 'D');
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

        public int getWeekOfYear(DateTime d) {
            int year = DateTime.Now.Year;
            int number = 1;
            
            while (d.DayOfWeek == DayOfWeek.Monday) {
                d = d.AddDays(-1);
            }
            while (d.Year == year) {
                number += 1;
                d = d.AddDays(-7);
            }
            return number -1;
        }

        public DateTime getFirstDayOfWeek(int weekNum)
        {
            int year = DateTime.Now.Year;
            DateTime d1;
            DateTime.TryParse("01/01/" + year, out d1);
            d1 = d1.AddDays(7 * weekNum);
            while (d1.DayOfWeek != DayOfWeek.Monday)
            {
                d1 = d1.AddDays(-1);
            }
            return d1;
        }
    }
}
