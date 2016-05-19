using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using King_Market.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace King_Market.Controllers
{
    public class SuppliersController : Controller
    {
        private King_MarketContext db = new King_MarketContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Suppliers
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.BusinessNameSortParm = String.IsNullOrEmpty(sortOrder) ? "BusinessName_Desc" : String.Empty;
            ViewBag.DocumentTypeSortParm = sortOrder == "Document Type" ? "DocumentType_Desc" : "Document Type";
            ViewBag.DocumentNumberSortParm = sortOrder == "Document Number" ? "DocumentNumber_Desc" : "Document Number";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_Desc" : "Phone";

            var suppliers = db.Suppliers.Include(s => s.DocumentType);

            if (!String.IsNullOrEmpty(searchString))
                suppliers = suppliers.Where(s =>
                    s.DocumentNumber.Contains(searchString) ||
                    s.BusinessName.Contains(searchString) ||
                    s.Phone.Contains(searchString));

            switch (sortOrder)
            {
                case "BusinessName_Desc":
                    suppliers = suppliers.OrderByDescending(p => p.BusinessName);
                    break;
                case "Document Type":
                    suppliers = suppliers.OrderBy(p => p.DocumentType.Name);
                    break;
                case "DocumentType_Desc":
                    suppliers = suppliers.OrderByDescending(p => p.DocumentType.Name);
                    break;
                case "Document Number":
                    suppliers = suppliers.OrderBy(p => p.DocumentNumber);
                    break;
                case "DocumentNumber_Desc":
                    suppliers = suppliers.OrderByDescending(p => p.DocumentNumber);
                    break;
                case "Phone":
                    suppliers = suppliers.OrderBy(p => p.Phone);
                    break;
                case "Phone_Desc":
                    suppliers = suppliers.OrderByDescending(p => p.Phone);
                    break;
                default:
                    suppliers = suppliers.OrderBy(p => p.BusinessName);
                    break;
            }

            return View(suppliers.ToList());
        }

        // GET: Suppliers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name");
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SupplierId,DocumentTypeId,DocumentNumber,BusinessName,Address,Email,Web,Phone")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();

                //Create User
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
                var user = userManager.FindByName(supplier.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = supplier.Email,
                        Email = supplier.Email
                    };
                    userManager.Create(user, supplier.DocumentNumber);
                    //AddRole
                    userManager.AddToRole(user.Id, "Supplier");
                }

                return RedirectToAction("Index");
            }

            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", supplier.DocumentTypeId);
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", supplier.DocumentTypeId);
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SupplierId,DocumentTypeId,DocumentNumber,BusinessName,Address,Email,Web,Phone")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", supplier.DocumentTypeId);
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
            var user = userManager.FindByName(supplier.Email);
            userManager.Delete(user);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db2.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
