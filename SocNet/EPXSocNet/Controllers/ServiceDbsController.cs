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
    public class ServiceDbsController : Controller
    {
        private Networkv3Entities db = new Networkv3Entities();

        // GET: ServiceDbs
        public ActionResult Index()
        {
            return View(db.ServiceDb.ToList());
        }

        // GET: ServiceDbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDb serviceDb = db.ServiceDb.Find(id);
            if (serviceDb == null)
            {
                return HttpNotFound();
            }
            return View(serviceDb);
        }

        // GET: ServiceDbs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] ServiceDb serviceDb)
        {
            if (ModelState.IsValid)
            {
                db.ServiceDb.Add(serviceDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(serviceDb);
        }

        // GET: ServiceDbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDb serviceDb = db.ServiceDb.Find(id);
            if (serviceDb == null)
            {
                return HttpNotFound();
            }
            return View(serviceDb);
        }

        // POST: ServiceDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] ServiceDb serviceDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(serviceDb);
        }

        // GET: ServiceDbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDb serviceDb = db.ServiceDb.Find(id);
            if (serviceDb == null)
            {
                return HttpNotFound();
            }
            return View(serviceDb);
        }

        // POST: ServiceDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceDb serviceDb = db.ServiceDb.Find(id);
            db.ServiceDb.Remove(serviceDb);
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
