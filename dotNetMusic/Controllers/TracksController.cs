using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using dotNetMusic.Models;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;
using ZXing;
using ZXing.QrCode;
using System.IO;
using System.Drawing;

namespace dotNetMusic.Controllers
{
    public class TracksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tracks
        public ActionResult Index()
        {
            var tracks = db.Tracks.Include(t => t.Playlist);
            return View(tracks.ToList());
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // GET: Tracks/Create
        public ActionResult Create()
        {
            ViewBag.PlaylistId = new SelectList(db.Playlists, "Id", "name");
            return View();
        }
        public byte[] generateQRCode(String URL, int id)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 400,
                    Height = 400
                }
            };
            return(imageToByteArray(barcodeWriter.Write(URL)));

        }
        // POST: Tracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,Title,Artist,AddedDate,EmbedURL,Description,QRcode,PlaylistId")] Track track)
        {
            if (ModelState.IsValid)
            {
                track.QRcode = generateQRCode(track.EmbedURL, track.id);
                track.AddedDate = DateTime.Now;
                db.Tracks.Add(track);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    // reload tracks
                    var reloadeprop = db.Tracks
                                            .Include(x => x.Playlist)
                                            .FirstOrDefault(x => x.id == track.id);
                    return Content(JsonConvert.SerializeObject(reloadeprop, Formatting.Indented,
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.PlaylistId = new SelectList(db.Playlists, "Id", "name", track.PlaylistId);
            return View(track);
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public ActionResult CustomEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track tracksel = db.Tracks.Find(id);
            if (tracksel == null)
            {
                return HttpNotFound();
            }
            var returnData =
            JsonConvert.SerializeObject(tracksel, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Content(returnData);
        }

        [HttpPost]
        public ActionResult ratetrack(int id, int rating)
        {

            Track trackProperty = db.Tracks.Find(id);
            trackProperty.rating = rating;

            if (ModelState.IsValid)
            {
                db.Tracks.AddOrUpdate(trackProperty);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    var returnData =
                    JsonConvert.SerializeObject("", Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return Content(returnData);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CustomEditConfirm(int id, String Title, String Artist, String EmbedURL, String Description, int PlaylistId)
        {

            Track trackProperty = db.Tracks.Find(id);
            trackProperty.Title = Title;
            trackProperty.Artist = Artist;
            trackProperty.EmbedURL = EmbedURL;
            trackProperty.Description = Description;
            trackProperty.PlaylistId = PlaylistId;
            trackProperty.QRcode = generateQRCode(EmbedURL, id);

            if (ModelState.IsValid)
            {
                db.Tracks.AddOrUpdate(trackProperty);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Tracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaylistId = new SelectList(db.Playlists, "Id", "name", track.PlaylistId);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Title,Artist,AddedDate,EmbedURL,Description,QRcode,PlaylistId")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Entry(track).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaylistId = new SelectList(db.Playlists, "Id", "name", track.PlaylistId);
            return View(track);
        }

        // GET: Tracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            Track rs = db.Tracks.Find(id);
            db.Tracks.Remove(rs);
            db.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Content("");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
