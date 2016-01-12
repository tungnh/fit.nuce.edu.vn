using System.Web.Mvc;

namespace thanhhoa.gov.vn.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, new string[] { "thanhhoa.gov.vn.Areas.Admin.Controllers" }
            );
        }
    }
}
