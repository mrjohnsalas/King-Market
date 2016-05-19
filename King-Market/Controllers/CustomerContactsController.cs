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
    public class CustomerContactsController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: CustomerContacts
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

            var customerContacts = db.CustomerContacts.Include(c => c.Customer).Include(c => c.DocumentType);

            if (!String.IsNullOrEmpty(searchString))
                customerContacts = customerContacts.Where(s =>
                    s.Customer.BusinessName.Contains(searchString) ||
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
                    customerContacts = customerContacts.OrderByDescending(p => p.Customer.BusinessName);
                    break;
                case "DocumentType_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.DocumentType.Name);
                    break;
                case "Document Type":
                    customerContacts = customerContacts.OrderBy(p => p.DocumentType.Name);
                    break;
                case "DocumentNumber_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.DocumentNumber);
                    break;
                case "Document Number":
                    customerContacts = customerContacts.OrderBy(p => p.DocumentNumber);
                    break;
                case "FirstName_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.FirstName);
                    break;
                case "First Name":
                    customerContacts = customerContacts.OrderBy(p => p.FirstName);
                    break;
                case "LastName_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.LastName);
                    break;
                case "Last Name":
                    customerContacts = customerContacts.OrderBy(p => p.LastName);
                    break;
                case "Second Last Name":
                    customerContacts = customerContacts.OrderBy(p => p.SecondLastName);
                    break;
                case "SecondLastName_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.SecondLastName);
                    break;
                case "Email":
                    customerContacts = customerContacts.OrderBy(p => p.Email);
                    break;
                case "Email_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.Email);
                    break;
                case "Phone":
                    customerContacts = customerContacts.OrderBy(p => p.Phone);
                    break;
                case "Phone_Desc":
                    customerContacts = customerContacts.OrderByDescending(p => p.Phone);
                    break;
                default:
                    customerContacts = customerContacts.OrderBy(p => p.Customer.BusinessName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(customerContacts.ToPagedList(pageNumber, pageSize));
        }

        // GET: CustomerContacts/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact customerContact = db.CustomerContacts.Find(id);
            if (customerContact == null)
            {
                return HttpNotFound();
            }
            return View(customerContact);
        }

        // GET: CustomerContacts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name");
            return View();
        }

        // POST: CustomerContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CustomerContactId,CustomerId,DocumentTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] CustomerContact customerContact)
        {
            if (ModelState.IsValid)
            {
                db.CustomerContacts.Add(customerContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "DocumentNumber", customerContact.CustomerId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", customerContact.DocumentTypeId);
            return View(customerContact);
        }

        // GET: CustomerContacts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact customerContact = db.CustomerContacts.Find(id);
            if (customerContact == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "DocumentNumber", customerContact.CustomerId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", customerContact.DocumentTypeId);
            return View(customerContact);
        }

        // POST: CustomerContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CustomerContactId,CustomerId,DocumentTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] CustomerContact customerContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "DocumentNumber", customerContact.CustomerId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", customerContact.DocumentTypeId);
            return View(customerContact);
        }

        // GET: CustomerContacts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerContact customerContact = db.CustomerContacts.Find(id);
            if (customerContact == null)
            {
                return HttpNotFound();
            }
            return View(customerContact);
        }

        // POST: CustomerContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerContact customerContact = db.CustomerContacts.Find(id);
            db.CustomerContacts.Remove(customerContact);
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
