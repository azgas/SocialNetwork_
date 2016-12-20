using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EPXSocNet.Models;

namespace EPXSocNet.Controllers
{
    public class LinkDbsController : Controller
    {
        private Networkv3Entities db = new Networkv3Entities();

        // GET: LinkDbs
        public ActionResult Index()
        {
            var linkDb = db.LinkDb.Include(l => l.NetworkDb).Include(l => l.VertexDb).Include(l => l.VertexDb1);
            return View(linkDb.ToList());
        }

        // GET: LinkDbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkDb linkDb = db.LinkDb.Find(id);
            if (linkDb == null)
            {
                return HttpNotFound();
            }
            return View(linkDb);
        }

        // GET: LinkDbs/Create
        public ActionResult Create()
        {
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name");
            ViewBag.source_id = new SelectList(db.VertexDb, "id", "identifier");
            ViewBag.target_id = new SelectList(db.VertexDb, "id", "identifier");
            return View();
        }

        // POST: LinkDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,network_id,source_id,target_id,date_modified")] LinkDb linkDb)
        {
            if (ModelState.IsValid)
            {
                db.LinkDb.Add(linkDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", linkDb.network_id);
            ViewBag.source_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.source_id);
            ViewBag.target_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.target_id);
            return View(linkDb);
        }

        // GET: LinkDbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkDb linkDb = db.LinkDb.Find(id);
            if (linkDb == null)
            {
                return HttpNotFound();
            }
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", linkDb.network_id);
            ViewBag.source_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.source_id);
            ViewBag.target_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.target_id);
            return View(linkDb);
        }

        // POST: LinkDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,network_id,source_id,target_id,date_modified")] LinkDb linkDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(linkDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", linkDb.network_id);
            ViewBag.source_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.source_id);
            ViewBag.target_id = new SelectList(db.VertexDb, "id", "identifier", linkDb.target_id);
            return View(linkDb);
        }

        // GET: LinkDbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinkDb linkDb = db.LinkDb.Find(id);
            if (linkDb == null)
            {
                return HttpNotFound();
            }
            return View(linkDb);
        }

        // POST: LinkDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LinkDb linkDb = db.LinkDb.Find(id);
            db.LinkDb.Remove(linkDb);
            db.SaveChanges();
            return RedirectToAction("Index");
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
