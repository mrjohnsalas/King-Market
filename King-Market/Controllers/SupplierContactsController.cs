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
    public class SupplierContactsController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: SupplierContacts
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.BusinessNameSortParm = String.IsNullOrEmpty(sortOrder) ? "BusinessName_Desc" : String.Empty;
            ViewBag.DocumentTypeSortParm = sortOrder == "Document Type" ? "DocumentType_Desc" : "Document Type";
            ViewBag.DocumentNumberSortParm = sortOrder == "Document Number" ? "DocumentNumber_Desc" : "Document Number";
            ViewBag.FirstNameSortParm = sortOrder == "First Name" ? "FirstName_Desc" : "First Name";
            ViewBag.LastNameSortParm = sortOrder == "Last Name" ? "LastName_Desc" : "Last Name";
            ViewBag.SecondLastNameSortParm = sortOrder == "Second Last Name" ? "SecondLastName_Desc" : "Second Last Name";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_Desc" : "Email";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_Desc" : "Phone";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var supplierContacts = db.SupplierContacts.Include(s => s.DocumentType).Include(s => s.Supplier);

            if (!String.IsNullOrEmpty(searchString))
                supplierContacts = supplierContacts.Where(s =>
                    s.Supplier.BusinessName.Contains(searchString) ||
                    s.DocumentType.Name.Contains(searchString) ||
                    s.DocumentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString) ||
                    s.SecondLastName.Contains(searchString) ||
                    s.Email.Contains(searchString) ||
                    s.Phone.Contains(searchString));

            switch (sortOrder)
            {
                case "BusinessName_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.Supplier.BusinessName);
                    break;
                case "DocumentType_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.DocumentType.Name);
                    break;
                case "Document Type":
                    supplierContacts = supplierContacts.OrderBy(p => p.DocumentType.Name);
                    break;
                case "DocumentNumber_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.DocumentNumber);
                    break;
                case "Document Number":
                    supplierContacts = supplierContacts.OrderBy(p => p.DocumentNumber);
                    break;
                case "FirstName_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.FirstName);
                    break;
                case "First Name":
                    supplierContacts = supplierContacts.OrderBy(p => p.FirstName);
                    break;
                case "LastName_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.LastName);
                    break;
                case "Last Name":
                    supplierContacts = supplierContacts.OrderBy(p => p.LastName);
                    break;
                case "Second Last Name":
                    supplierContacts = supplierContacts.OrderBy(p => p.SecondLastName);
                    break;
                case "SecondLastName_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.SecondLastName);
                    break;
                case "Email":
                    supplierContacts = supplierContacts.OrderBy(p => p.Email);
                    break;
                case "Email_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.Email);
                    break;
                case "Phone":
                    supplierContacts = supplierContacts.OrderBy(p => p.Phone);
                    break;
                case "Phone_Desc":
                    supplierContacts = supplierContacts.OrderByDescending(p => p.Phone);
                    break;
                default:
                    supplierContacts = supplierContacts.OrderBy(p => p.Supplier.BusinessName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(supplierContacts.ToPagedList(pageNumber, pageSize));
        }

        // GET: SupplierContacts/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierContact supplierContact = db.SupplierContacts.Find(id);
            if (supplierContact == null)
            {
                return HttpNotFound();
            }
            return View(supplierContact);
        }

        // GET: SupplierContacts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "BusinessName");
            return View();
        }

        // POST: SupplierContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SupplierContactId,SupplierId,DocumentTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] SupplierContact supplierContact)
        {
            if (ModelState.IsValid)
            {
                db.SupplierContacts.Add(supplierContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", supplierContact.DocumentTypeId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "DocumentNumber", supplierContact.SupplierId);
            return View(supplierContact);
        }

        // GET: SupplierContacts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierContact supplierContact = db.SupplierContacts.Find(id);
            if (supplierContact == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", supplierContact.DocumentTypeId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "DocumentNumber", supplierContact.SupplierId);
            return View(supplierContact);
        }

        // POST: SupplierContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SupplierContactId,SupplierId,DocumentTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] SupplierContact supplierContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", supplierContact.DocumentTypeId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "DocumentNumber", supplierContact.SupplierId);
            return View(supplierContact);
        }

        // GET: SupplierContacts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierContact supplierContact = db.SupplierContacts.Find(id);
            if (supplierContact == null)
            {
                return HttpNotFound();
            }
            return View(supplierContact);
        }

        // POST: SupplierContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplierContact supplierContact = db.SupplierContacts.Find(id);
            db.SupplierContacts.Remove(supplierContact);
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
