using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace thanhhoa.gov.vn.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Admin/Error/

        public ActionResult Error404()
        {
            return View();
        }

    }
}
