using System.Web;
using System.Web.Mvc;

namespace thanhhoa.gov.vn
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}