﻿using System;
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
    public class ClassDocumentTypesController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: ClassDocumentTypes
        public ActionResult Index()
        {
            return View(db.ClassDocumentTypes.ToList());
        }

        // GET: ClassDocumentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassDocumentType classDocumentType = db.ClassDocumentTypes.Find(id);
            if (classDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(classDocumentType);
        }

        // GET: ClassDocumentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassDocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassDocumentTypeId,Name")] ClassDocumentType classDocumentType)
        {
            if (ModelState.IsValid)
            {
                db.ClassDocumentTypes.Add(classDocumentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(classDocumentType);
        }

        // GET: ClassDocumentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassDocumentType classDocumentType = db.ClassDocumentTypes.Find(id);
            if (classDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(classDocumentType);
        }

        // POST: ClassDocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassDocumentTypeId,Name")] ClassDocumentType classDocumentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classDocumentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classDocumentType);
        }

        // GET: ClassDocumentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassDocumentType classDocumentType = db.ClassDocumentTypes.Find(id);
            if (classDocumentType == null)
            {
                return HttpNotFound();
            }
            return View(classDocumentType);
        }

        // POST: ClassDocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassDocumentType classDocumentType = db.ClassDocumentTypes.Find(id);
            db.ClassDocumentTypes.Remove(classDocumentType);
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