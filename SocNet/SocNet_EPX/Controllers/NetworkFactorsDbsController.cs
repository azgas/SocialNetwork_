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
    public class NetworkFactorsDbsController : Controller
    {
        private Networkv3Entities1 db = new Networkv3Entities1();

        // GET: NetworkFactorsDbs
        public ActionResult Index()
        {
            var networkFactorsDb = db.NetworkFactorsDb.Include(n => n.NetworkDb);
            return View(networkFactorsDb.ToList());
        }

        // GET: NetworkFactorsDbs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkFactorsDb networkFactorsDb = db.NetworkFactorsDb.Find(id);
            if (networkFactorsDb == null)
            {
                return HttpNotFound();
            }
            return View(networkFactorsDb);
        }

        // GET: NetworkFactorsDbs/Create
        public ActionResult Create()
        {
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name");
            return View();
        }

        // POST: NetworkFactorsDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,network_id,date,density,av_betweeness_centrality,av_closeness_centrality,av_indegree_centrality,av_influence_range,av_outdegree_centrality,up_to_date")] NetworkFactorsDb networkFactorsDb)
        {
            if (ModelState.IsValid)
            {
                db.NetworkFactorsDb.Add(networkFactorsDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", networkFactorsDb.network_id);
            return View(networkFactorsDb);
        }

        // GET: NetworkFactorsDbs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkFactorsDb networkFactorsDb = db.NetworkFactorsDb.Find(id);
            if (networkFactorsDb == null)
            {
                return HttpNotFound();
            }
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", networkFactorsDb.network_id);
            return View(networkFactorsDb);
        }

        // POST: NetworkFactorsDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,network_id,date,density,av_betweeness_centrality,av_closeness_centrality,av_indegree_centrality,av_influence_range,av_outdegree_centrality,up_to_date")] NetworkFactorsDb networkFactorsDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(networkFactorsDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.network_id = new SelectList(db.NetworkDb, "id", "name", networkFactorsDb.network_id);
            return View(networkFactorsDb);
        }

        // GET: NetworkFactorsDbs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NetworkFactorsDb networkFactorsDb = db.NetworkFactorsDb.Find(id);
            if (networkFactorsDb == null)
            {
                return HttpNotFound();
            }
            return View(networkFactorsDb);
        }

        // POST: NetworkFactorsDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            NetworkFactorsDb networkFactorsDb = db.NetworkFactorsDb.Find(id);
            db.NetworkFactorsDb.Remove(networkFactorsDb);
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
