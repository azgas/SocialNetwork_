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
    public class CredentialsController : Controller
    {
        private Networkv3Entities db = new Networkv3Entities();

        // GET: Credentials
        public ActionResult Index()
        {
            var credentials = db.Credentials.Include(c => c.ServiceDb);
            return View(credentials.ToList());
        }

        // GET: Credentials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            return View(credentials);
        }

        // GET: Credentials/Create
        public ActionResult Create()
        {
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name");
            return View();
        }

        // POST: Credentials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,key,secret,service_id")] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                db.Credentials.Add(credentials);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", credentials.service_id);
            return View(credentials);
        }

        // GET: Credentials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", credentials.service_id);
            return View(credentials);
        }

        // POST: Credentials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,key,secret,service_id")] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credentials).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.service_id = new SelectList(db.ServiceDb, "id", "name", credentials.service_id);
            return View(credentials);
        }

        // GET: Credentials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return HttpNotFound();
            }
            return View(credentials);
        }

        // POST: Credentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Credentials credentials = db.Credentials.Find(id);
            db.Credentials.Remove(credentials);
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
