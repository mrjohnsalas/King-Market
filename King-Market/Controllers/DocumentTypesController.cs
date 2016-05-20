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
    public class DocumentTypesController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: DocumentTypes
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : String.Empty;
            ViewBag.ClassDocumentSortParm = sortOrder == "Class Document" ? "ClassDocument_Desc" : "Class Document";
            ViewBag.OnlyForEnterpriseSortParm = sortOrder == "Only For Enterprise?" ? "OnlyForEnterprise_Desc" : "Only For Enterprise?";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var documentTypes = db.DocumentTypes.Include(d => d.ClassDocumentType); ;

            if (!String.IsNullOrEmpty(searchString))
                documentTypes = documentTypes.Where(s => s.ClassDocumentType.Name.Contains(searchString) || s.Name.Contains(searchString));

            switch (sortOrder)
            {
                case "Name_Desc":
                    documentTypes = documentTypes.OrderByDescending(p => p.Name);
                    break;
                case "Class Document":
                    documentTypes = documentTypes.OrderBy(p => p.ClassDocumentType.Name);
                    break;
                case "ClassDocument_Desc":
                    documentTypes = documentTypes.OrderByDescending(p => p.ClassDocumentType.Name);
                    break;
                case "Only For Enterprise?":
                    documentTypes = documentTypes.OrderBy(p => p.OnlyForEnterprise);
                    break;
                case "OnlyForEnterprise_Desc":
                    documentTypes = documentTypes.OrderByDescending(p => p.OnlyForEnterprise);
                    break;
                default:
                    documentTypes = documentTypes.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(documentTypes.ToPagedList(pageNumber, pageSize));
        }

        // GET: DocumentTypes/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ClassDocumentTypeId = new SelectList(db.ClassDocumentTypes.OrderBy(d => d.Name), "ClassDocumentTypeId", "Name");
            return View();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DocumentTypeId,Name,OnlyForEnterprise,ClassDocumentTypeId")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                db.DocumentTypes.Add(documentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassDocumentTypeId = new SelectList(db.ClassDocumentTypes.OrderBy(d => d.Name), "ClassDocumentTypeId", "Name", documentType.ClassDocumentTypeId);
            return View(documentType);
        }

        // GET: DocumentTypes/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassDocumentTypeId = new SelectList(db.ClassDocumentTypes.OrderBy(d => d.Name), "ClassDocumentTypeId", "Name", documentType.ClassDocumentTypeId);
            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DocumentTypeId,Name,OnlyForEnterprise,ClassDocumentTypeId")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassDocumentTypeId = new SelectList(db.ClassDocumentTypes.OrderBy(d => d.Name), "ClassDocumentTypeId", "Name", documentType.ClassDocumentTypeId);
            return View(documentType);
        }

        // GET: DocumentTypes/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentType documentType = db.DocumentTypes.Find(id);
            if (documentType == null)
            {
                return HttpNotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentType documentType = db.DocumentTypes.Find(id);
            db.DocumentTypes.Remove(documentType);
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
