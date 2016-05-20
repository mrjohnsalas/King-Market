using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using King_Market.Models;
using PagedList;

namespace King_Market.Controllers
{
    public class UnitMeasuresController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: UnitMeasures
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : String.Empty;
            ViewBag.ShortNameSortParm = sortOrder == "Short Name" ? "ShortName_Desc" : "Short Name";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var unitMeasures = 
                String.IsNullOrEmpty(searchString) ?
                db.UnitMeasures.OrderBy(c => c.Name) :
                db.UnitMeasures.OrderBy(c => c.Name).Where(s => 
                    s.ShortName.Contains(searchString) ||
                    s.Name.Contains(searchString));

            switch (sortOrder)
            {
                case "Name_Desc":
                    unitMeasures = unitMeasures.OrderByDescending(p => p.Name);
                    break;
                case "Short Name":
                    unitMeasures = unitMeasures.OrderBy(p => p.ShortName);
                    break;
                case "ShortName_Desc":
                    unitMeasures = unitMeasures.OrderByDescending(p => p.ShortName);
                    break;
                default:
                    unitMeasures = unitMeasures.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(unitMeasures.ToPagedList(pageNumber, pageSize));
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
