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
    public class VertexFactorsDbsController : Controller
    {
        private Networkv3Entities1 db = new Networkv3Entities1();

        // GET: VertexFactorsDbs
        public ActionResult Index()
        {
            var vertexFactorsDb = db.VertexFactorsDb.Include(v => v.VertexDb);
            return View(vertexFactorsDb.ToList());
        }

        // GET: VertexFactorsDbs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexFactorsDb vertexFactorsDb = db.VertexFactorsDb.Find(id);
            if (vertexFactorsDb == null)
            {
                return HttpNotFound();
            }
            return View(vertexFactorsDb);
        }

        // GET: VertexFactorsDbs/Create
        public ActionResult Create()
        {
            ViewBag.vertex_id = new SelectList(db.VertexDb, "id", "identifier");
            return View();
        }

        // POST: VertexFactorsDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "vertex_id,id,date,betweeness_centrality,closeness_centrality,indegree_centrality,influence_range,outdegree_centrality,up_to_date")] VertexFactorsDb vertexFactorsDb)
        {
            if (ModelState.IsValid)
            {
                db.VertexFactorsDb.Add(vertexFactorsDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.vertex_id = new SelectList(db.VertexDb, "id", "identifier", vertexFactorsDb.vertex_id);
            return View(vertexFactorsDb);
        }

        // GET: VertexFactorsDbs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexFactorsDb vertexFactorsDb = db.VertexFactorsDb.Find(id);
            if (vertexFactorsDb == null)
            {
                return HttpNotFound();
            }
            ViewBag.vertex_id = new SelectList(db.VertexDb, "id", "identifier", vertexFactorsDb.vertex_id);
            return View(vertexFactorsDb);
        }

        // POST: VertexFactorsDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vertex_id,id,date,betweeness_centrality,closeness_centrality,indegree_centrality,influence_range,outdegree_centrality,up_to_date")] VertexFactorsDb vertexFactorsDb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vertexFactorsDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.vertex_id = new SelectList(db.VertexDb, "id", "identifier", vertexFactorsDb.vertex_id);
            return View(vertexFactorsDb);
        }

        // GET: VertexFactorsDbs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VertexFactorsDb vertexFactorsDb = db.VertexFactorsDb.Find(id);
            if (vertexFactorsDb == null)
            {
                return HttpNotFound();
            }
            return View(vertexFactorsDb);
        }

        // POST: VertexFactorsDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            VertexFactorsDb vertexFactorsDb = db.VertexFactorsDb.Find(id);
            db.VertexFactorsDb.Remove(vertexFactorsDb);
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
