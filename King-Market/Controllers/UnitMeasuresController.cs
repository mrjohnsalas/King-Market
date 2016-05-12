using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using King_Market.Models;

namespace King_Market.Controllers
{
    public class UnitMeasuresController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: UnitMeasures
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.UnitMeasures.ToList());
        }

        // GET: UnitMeasures/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // GET: UnitMeasures/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitMeasures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "UnitMeasureId,ShortName,Name")] UnitMeasure unitMeasure)
        {
            if (ModelState.IsValid)
            {
                db.UnitMeasures.Add(unitMeasure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitMeasure);
        }

        // GET: UnitMeasures/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // POST: UnitMeasures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "UnitMeasureId,ShortName,Name")] UnitMeasure unitMeasure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitMeasure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitMeasure);
        }

        // GET: UnitMeasures/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            if (unitMeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitMeasure);
        }

        // POST: UnitMeasures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            UnitMeasure unitMeasure = db.UnitMeasures.Find(id);
            db.UnitMeasures.Remove(unitMeasure);
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
