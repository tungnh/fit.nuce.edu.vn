using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;
using AttributeRouting.Web.Mvc;
using AttributeRouting;
using System.Xml.Linq;
using System.IO;

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("trang-chu", Precedence = 1)]
    public class HomeController : BaseController
    {
        public ActionResult SentMail()
        {
            String smtpUsername = "nguyenhuutung.nuce@gmail.com";
            String smtpPassword = "rmqsbraakqrtzsrw";
            String smtpHost = "smtp.gmail.com";
            int smtpPost = 25;

            String emailTo = "nguyenhuutung.nuce@gmail.com";
            String subject = "Thông tin đăng ký cựu sinh viên của bạn đã được duyệt";
            String body = "<p><strong>Website Khoa công nghệ thông tin thông báo:</strong> Thông tin cựu sinh viên của bạn đã được duyệt.</p>";
            body += "<p><strong><a href='fit.nuce.edu.vn' target='_blank'>BẤM ĐÂY</a><strong> để xem thông tin cựu sinh viên của bạn</p>";
            body += "<p>Mọi ý kiến thắc xin gửi về: <a href='mailto:vieclam24h@timviecnhanh.com' target='_blank'>vieclam24h@timviecnhanh.com</a></p>";
            body += "<p></p>";
            EmailServices services = new EmailServices();
            bool rs = services.Send(smtpUsername, smtpPassword, smtpHost, smtpPost, emailTo, subject, body);
            if (rs)
                ViewData["message"] = "Thanh cong";
            else
                ViewData["message"] = "That bai";
            return View();
        }
        public ActionResult Index()
        {
            ViewData["lstDepartment"] = _cnttDB.gov_department.OrderBy(s => s.order_number).Where(d => d.is_home == true).ToList();
            var listMenuHome = _cnttDB.gov_menu.Where(m => m.active_flg == true).Where(m => m.ishome == true).OrderBy(m => m.entry_datetime).ToList();
            foreach (var menuHome in listMenuHome)
            {
                menuHome.gov_news = menuHome.gov_news.Where(n => n.active_flg == true).OrderByDescending(n => n.update_datetime).ToList();
            }
            ViewData["lstMenuHome"] = listMenuHome;
            ViewData["newHome"] = _cnttDB.gov_news.Where(n => n.is_home == true && n.active_flg == true).OrderByDescending(n => n.update_datetime).FirstOrDefault();
            ViewData["lstSlideHome"] = _cnttDB.gov_slide_home.OrderBy(x => x.order_number).ToList();
            return View();
        }

        public ActionResult Index2()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(NewViewhelper newViewhelper)
        {
            var luceneNew = new LuceneSerives.LuceneNews();
            var lstNew = luceneNew.Search(newViewhelper.KeySearch).ToList();
            int totalCount = lstNew.Count;
            newViewhelper.TotalCount = totalCount;

            if (newViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                newViewhelper.TotalPage = totalPage;
                newViewhelper.Page = pageTransition(newViewhelper.Direction, newViewhelper.Page, totalPage);
                newViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page);
                newViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, newViewhelper.Page, newViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (newViewhelper.Page - 1) * take;
                newViewhelper.LstNew = lstNew.OrderByDescending(n => n.score).Skip(skip).Take(take).ToList();
            }
            ViewData["newViewhelper"] = newViewhelper;
            return View("Search");
        }

        public void setSearchFilter(NewServices newServices, NewViewhelper newViewhelper)
        {
            if (!String.IsNullOrWhiteSpace(newViewhelper.KeySearch))
                newServices.KeySearch = newViewhelper.KeySearch;
        }
        
        public ActionResult Gmap()
        {
            DistrictServices districtServices = new DistrictServices();
            districtServices.ShowMap = Boolean.TrueString;
            districtServices.addOrderBy("name", Constant.ORDER_ASC);
            List<DistrictModels> lstDistrict = districtServices.select(-1, -1);
            ViewData["lstDistrict"] = lstDistrict;
            return View();
        }

        public String GetJsonMap(int id)
        {
            
            String rs = "{\"type\": \"FeatureCollection\",";
            rs += "\"features\": [";
            DistrictServices districtServices = new DistrictServices();
            districtServices.ShowMap = Boolean.TrueString;
            districtServices.Id = id.ToString();
            List<DistrictModels> lstDistrict = districtServices.select(-1, -1);
            DistrictModels districtInfo = new DistrictModels();
            if(lstDistrict != null && lstDistrict.Count > 0){
                districtInfo = lstDistrict.First();
            }
            String coordinates = districtInfo.Coordinates;
            String[] lstCoordinates = coordinates.Split(',');
            for (int i = 0; i < lstCoordinates.Length; i++ )
            {
                String[] coordinates1 = lstCoordinates[i].Trim().Split(' ');
                rs += "{ \"type\": \"Feature\", \"id\": \"" + (i + 1).ToString() +"\", \"geometry\": { \"type\": \"Point\", \"coordinates\": [" + coordinates1[0] + "," + coordinates1[1] + "] } }";
                if(i != (lstCoordinates.Length - 1))
                    rs += ",";
            }

            rs += "]}";
            return rs;
        }

        [HttpGet]
        public ActionResult Calendar()
        {
            CalendarViewhelper calendarViewHelper = new CalendarViewhelper();
            calendarViewHelper.WeekNum = getWeekOfYear(DateTime.Now);
            SaveCalendar(calendarViewHelper);
            return View();
        }

        [HttpPost]
        public ActionResult Calendar(CalendarViewhelper calendarViewhelper)
        {
            SaveCalendar(calendarViewhelper);
            return View();
        }

        public void SaveCalendar(CalendarViewhelper calendarViewhelper)
        {
            NewServices newServices;
            newServices = new NewServices();
            newServices.ActiveFlg = Boolean.TrueString;
            newServices.addOrderBy("total_view", "DESC");
            List<NewModels> lstNewXemNhieu = newServices.select(1, 15);
            ViewData["lstNewXemNhieu"] = lstNewXemNhieu;

            if (calendarViewhelper.TypeView == 1) {
                calendarViewhelper.FromDate = getFirstDayOfWeek(calendarViewhelper.WeekNum);
                calendarViewhelper.ToDate = calendarViewhelper.FromDate.AddDays(6);

                CalendarServices calendarServices = new CalendarServices();
                calendarServices.FromDate = calendarViewhelper.FromDate.ToString("yyyy-MM-dd");
                calendarServices.ToDate = calendarViewhelper.ToDate.ToString("yyyy-MM-dd");
                List<CalendarModels> lstCalendar = calendarServices.select(-1, -1);
                calendarViewhelper.LstCalendar = lstCalendar;

                calendarServices = new CalendarServices();
                List<WorktimeModels> lstWorktime = calendarServices.selectTime();
                ViewData["lstWorktime"] = lstWorktime;
                ViewData["calendarViewhelper"] = calendarViewhelper;
            }
            else if(calendarViewhelper.TypeView == 2){ 
            
            }
        }

        [GET("so-do-trang")]
        public ActionResult SiteMap() {
            ViewData["lstSiteMap"] = _cnttDB.gov_menu.Where(m => m.active_flg == true).ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [GET("lien-he")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewData["data"] = _cnttDB.gov_system_config.ToList();
            return View();
        }

        [GET("RSS")]
        public ActionResult RSS()
        {
            ViewData["lstRSS"] = _cnttDB.gov_menu.Where(m => m.active_flg == true && m.isrss == true).OrderBy(m => m.order_number).ToList();
            return View();
        }

        public ActionResult GetRss(int cid)
        {
            List<gov_news> lstNew = _cnttDB.gov_news.Where(m => m.menu_id == cid && m.active_flg == true).OrderBy(m => m.update_datetime).ToList();

            SyndicationFeed feed = null;
            string siteTitle, description, siteUrl;
            siteTitle = "RSS - Khoa Công Nghệ Thông Tin - Trường Đại Học Xây Dựng";
            siteUrl = "http://fit.nuce.edu.vn";
            description = "Chào mừng bạn đến với RSS - Khoa Công Nghệ Thông Tin - Trường Đại Học Xây Dựng. Các nguồn kênh tin được cung cấp miễn phí cho các cá nhân và các tổ chức phi lợi nhuận. Chúng tôi yêu cầu bạn cung cấp rõ các thông tin cần thiết khi bạn sử dụng các nguồn kênh tin này từ fit.nuce.edu.vn. Fit.nuce.edu.vn có quyền yêu cầu bạn ngừng cung cấp và phân tán thông tin dưới dạng này ở bất kỳ thời điểm nào và với bất kỳ lý do nào.";

                List<SyndicationItem> items = new List<SyndicationItem>();
                //Last 10 Post
                foreach (var i in lstNew)
                {
                    SyndicationItem item = new SyndicationItem
                    {
                        Title = new TextSyndicationContent(i.title),
                        Content = new TextSyndicationContent(GetPlainText(i.description, 200)), //here content may be Html content so we should use plain text
                        PublishDate = i.entry_datetime,
                    };
                    item.Links.Add(new SyndicationLink(new Uri(Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/new/index?id=" + i.id)));
                    items.Add(item);
                }

                feed = new SyndicationFeed(siteTitle, description, new Uri(siteUrl));
                feed.Items = items;

            return new RSSResult { feedData = feed };
        }

        [GET("text")]
        public ActionResult text()
        {
            XElement root = new XElement("urlset");
            root.Add(new XElement("element1"));
            root.Add(new XElement("element2"));
            root.Add(new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"));
            return new XmlResult(root);
        }

        private string GetPlainText(string htmlContent, int length = 0)
        {
            if (String.IsNullOrWhiteSpace(htmlContent))
                return "";
            string HTML_TAG_PATTERN = "<.*?>";
            string plainText = Regex.Replace(htmlContent, HTML_TAG_PATTERN, string.Empty);
            return length > 0 && plainText.Length > length ? plainText.Substring(0, length) : plainText;
        }

        public ActionResult LichTiepDan()
        {
            NotifyServices notifyServices = new NotifyServices();
            notifyServices.addOrderBy("update_datetime", Constant.ORDER_DESC);
            List<NotifyModels> lstNotify = notifyServices.select(-1, -1);
            NotifyModels NotifyInfo = null;
            if (lstNotify != null && lstNotify.Count > 0)
            {
                NotifyInfo = lstNotify.First();
                ViewData["notifyInfo"] = NotifyInfo;
            }
            return View();
        }

        [GET("danh-ba")]
        public ActionResult ContactAndEmail() {
            ViewData["lstUser"] = _cnttDB.gov_user.Where(u => u.active_flg == true).ToList();
            ViewData["lstDepartment"] = _cnttDB.gov_department.OrderBy(d => d.order_number).ToList();
            return View();
        }
    }
}
