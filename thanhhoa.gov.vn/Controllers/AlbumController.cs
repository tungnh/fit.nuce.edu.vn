using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;

namespace thanhhoa.gov.vn.Controllers
{
    [RoutePrefix("album", Precedence = 1)]
    public class AlbumController : BaseController
    {
        //
        // GET: /Album/
        [GET("{name}-{albumId:int}")]
        public ActionResult Index(int albumId)
        {
            gov_album albumModel = _cnttDB.gov_album.Find(albumId);
            if (albumModel == null)
                return Redirect("/error/error404");

            albumModel.total_view = albumModel.total_view + 1;
            _cnttDB.SaveChanges();
            ViewData["albumModel"] = albumModel;
            return View();
        }

        public ActionResult Preview()
        {
            //List album
            ViewData["lstAlbum"] = _cnttDB.gov_album.ToList();
            return View();
        }
    }
}
