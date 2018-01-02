using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.QrCode;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using dotNetMusic.Models;

namespace dotNetMusic.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoadProp()
        {
            var alltracks = db.Tracks.OrderBy(x => x.id).ToList().Select(x => new {
                Track = x,
                x.Title,
                x.id,
                x.Artist,
                x.Description,
                x.EmbedURL,
                x.Playlist,
                x.PlaylistId,
                x.rating,
                Image = Convert.ToBase64String(x.QRcode)
            });
            var jsonString = JsonConvert.SerializeObject(alltracks, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(jsonString);
        }
        public ActionResult loadPlaylists()
        {
            var playlist = db.Playlists.ToList();


               /* db.Tracks.OrderBy(x => x.id).ToList().Select(x => new {
                Track = x,
                x.Title,
                x.id,
                x.Artist,
                x.Description,
                x.EmbedURL,
                x.Playlist,
                x.PlaylistId,
                Image = Convert.ToBase64String(x.QRcode)
            });*/
            var jsonString = JsonConvert.SerializeObject(playlist, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(jsonString);
        }

    }
}