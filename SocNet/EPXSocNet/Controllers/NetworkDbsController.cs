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
    public class NetworkDbsController : Controller
    {
        private Networkv3Entities db = new Networkv3Entities();

        // GET: NetworkDbs
        public ActionResult Index()
        {
            var networkDb = db.NetworkDb.Include(n => n.ServiceDb);
            return View(networkDb.ToList());
        }

        // GET: NetworkDbs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkDb networkDb = db.NetworkDb.Find(id);
            if (networkDb == null)
            {
                return HttpNotFound();
            }
            return View(networkDb);
        }

        // GET: NetworkDbs/Create
        public ActionResult Create()
        {
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name");
            return View();
        }

        // POST: NetworkDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,date_created,last_modified,service_id,directed")] NetworkDb networkDb)
        {
            if (ModelState.IsValid)
            {
                db.NetworkDb.Add(networkDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", networkDb.service_id);
            return View(networkDb);
        }

        // GET: NetworkDbs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkDb networkDb = db.NetworkDb.Find(id);
            if (networkDb == null)
            {
                return HttpNotFound();
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", networkDb.service_id);
            return View(networkDb);
        }

        // POST: NetworkDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,date_created,last_modified,service_id,directed")] NetworkDb networkDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(networkDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", networkDb.service_id);
            return View(networkDb);
        }

        // GET: NetworkDbs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkDb networkDb = db.NetworkDb.Find(id);
            if (networkDb == null)
            {
                return HttpNotFound();
            }
            return View(networkDb);
        }

        // POST: NetworkDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            NetworkDb networkDb = db.NetworkDb.Find(id);
            db.NetworkDb.Remove(networkDb);
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
