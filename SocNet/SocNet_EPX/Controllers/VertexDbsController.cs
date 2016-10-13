using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocNet.Models;

namespace SocNet.Controllers
{
    public class VertexDbsController : Controller
    {
        private Networkv3Entities1 db = new Networkv3Entities1();

        // GET: VertexDbs
        public ActionResult Index()
        {
            var vertexDb = db.VertexDb.Include(v => v.ServiceDb);
            return View(vertexDb.ToList());
        }

        // GET: VertexDbs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexDb vertexDb = db.VertexDb.Find(id);
            if (vertexDb == null)
            {
                return HttpNotFound();
            }
            return View(vertexDb);
        }

        // GET: VertexDbs/Create
        public ActionResult Create()
        {
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name");
            return View();
        }

        // POST: VertexDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,identifier,name,service_id")] VertexDb vertexDb)
        {
            if (ModelState.IsValid)
            {
                db.VertexDb.Add(vertexDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", vertexDb.service_id);
            return View(vertexDb);
        }

        // GET: VertexDbs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexDb vertexDb = db.VertexDb.Find(id);
            if (vertexDb == null)
            {
                return HttpNotFound();
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", vertexDb.service_id);
            return View(vertexDb);
        }

        // POST: VertexDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,identifier,name,service_id")] VertexDb vertexDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vertexDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", vertexDb.service_id);
            return View(vertexDb);
        }

        // GET: VertexDbs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexDb vertexDb = db.VertexDb.Find(id);
            if (vertexDb == null)
            {
                return HttpNotFound();
            }
            return View(vertexDb);
        }

        // POST: VertexDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            VertexDb vertexDb = db.VertexDb.Find(id);
            db.VertexDb.Remove(vertexDb);
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
