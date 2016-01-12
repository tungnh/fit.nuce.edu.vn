using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.SchedulerTask;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
        
    public class MvcApplication : System.Web.HttpApplication
    {
        private CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
        protected void Application_Start()
        {
            Groupdocs.Web.UI.Viewer.InitRoutes();
            Groupdocs.Web.UI.Viewer.SetRootStoragePath(FileRepository.RootStorage);
            Groupdocs.Web.UI.Viewer.EnableFileListRequestHandling(true);
            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.DefaultNamespaces.Add("thanhhoa.gov.vn.Controllers");
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            //JobScheduler.Start();
        }

        protected void Application_End()
        {
            //Application.Remove("soluottruycap");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
            String date = DateTime.Now.ToString("dd/MM/yyyy");
            gov_statistic_access statisticInfo = _cnttDB.gov_statistic_access.Find(date);
            if (statisticInfo == null)
            {
                gov_statistic_access statisticInfoAdd = new gov_statistic_access();
                statisticInfoAdd.date = DateTime.Now.ToString("dd/MM/yyyy");
                statisticInfoAdd.count = 1;
                _cnttDB.gov_statistic_access.Add(statisticInfoAdd);
                _cnttDB.SaveChanges();
            }
            else
            {
                statisticInfo.count = statisticInfo.count + 1;
                _cnttDB.SaveChanges();
            }

            Application.Lock();
            if (Application["soluottruycap"] == null)
                Application["soluottruycap"] = 1;
            else
                Application["soluottruycap"] = (int)Application["soluottruycap"] + 1;
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            if (Application["soluottruycap"] != null)
                Application["soluottruycap"] = (int)Application["soluottruycap"] - 1;
            Application.UnLock();
        }
    }
}