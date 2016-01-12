using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("video", Precedence = 1)]
    public class VideoController : BaseController
    {
        //
        // GET: /Video/

        [GET("danh-sach-video")]
        public ActionResult Index()
        {
            ViewData["lstVideo"] = _cnttDB.gov_video.OrderByDescending(v => v.update_datetime).ToList();
            return View();
        }

        [GET("{name}-{id:int}")]
        [HttpGet]
        public ActionResult Iframe(int id) {
            ViewData["videoInfo"] = _cnttDB.gov_video.Find(id);
            return View();
        }
    }
}
