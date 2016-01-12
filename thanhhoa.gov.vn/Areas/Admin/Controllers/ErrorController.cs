using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Admin/Error/

        public ActionResult Error403()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error405()
        {
            return View();
        }
    }
}
