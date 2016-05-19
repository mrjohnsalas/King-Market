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
    public class CustomersController : Controller
    {
        private King_MarketContext db = new King_MarketContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Customers
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.BusinessNameSortParm = String.IsNullOrEmpty(sortOrder) ? "BusinessName_Desc" : String.Empty;
            ViewBag.DocumentTypeSortParm = sortOrder == "Document Type" ? "DocumentType_Desc" : "Document Type";
            ViewBag.DocumentNumberSortParm = sortOrder == "Document Number" ? "DocumentNumber_Desc" : "Document Number";
            ViewBag.FirstNameSortParm = sortOrder == "First Name" ? "FirstName_Desc" : "First Name";
            ViewBag.LastNameSortParm = sortOrder == "Last Name" ? "LastName_Desc" : "Last Name";
            ViewBag.SecondLastNameSortParm = sortOrder == "Second Last Name" ? "SecondLastName_Desc" : "Second Last Name";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_Desc" : "Phone";

            var customers = db.Customers.Include(c => c.DocumentType);

            if (!String.IsNullOrEmpty(searchString))
                customers = customers.Where(s =>
                    s.DocumentType.Name.Contains(searchString) ||
                    s.DocumentNumber.Contains(searchString) ||
                    s.BusinessName.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString) ||
                    s.SecondLastName.Contains(searchString) ||
                    s.Phone.Contains(searchString));

            switch (sortOrder)
            {
                case "BusinessName_Desc":
                    customers = customers.OrderByDescending(p => p.BusinessName);
                    break;
                case "Document Type":
                    customers = customers.OrderBy(p => p.DocumentType.Name);
                    break;
                case "DocumentType_Desc":
                    customers = customers.OrderByDescending(p => p.DocumentType.Name);
                    break;
                case "Document Number":
                    customers = customers.OrderBy(p => p.DocumentNumber);
                    break;
                case "DocumentNumber_Desc":
                    customers = customers.OrderByDescending(p => p.DocumentNumber);
                    break;
                case "First Name":
                    customers = customers.OrderBy(p => p.FirstName);
                    break;
                case "FirstName_Desc":
                    customers = customers.OrderByDescending(p => p.FirstName);
                    break;
                case "Last Name":
                    customers = customers.OrderBy(p => p.LastName);
                    break;
                case "LastName_Desc":
                    customers = customers.OrderByDescending(p => p.LastName);
                    break;
                case "Second Last Name":
                    customers = customers.OrderBy(p => p.SecondLastName);
                    break;
                case "SecondLastName_Desc":
                    customers = customers.OrderByDescending(p => p.SecondLastName);
                    break;
                case "Phone":
                    customers = customers.OrderBy(p => p.Phone);
                    break;
                case "Phone_Desc":
                    customers = customers.OrderByDescending(p => p.Phone);
                    break;
                default:
                    customers = customers.OrderBy(p => p.BusinessName);
                    break;
            }

            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CustomerId,DocumentTypeId,DocumentNumber,BusinessName,FirstName,LastName,SecondLastName,Address,Email,Web,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();

                //Create User
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
                var user = userManager.FindByName(customer.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = customer.Email,
                        Email = customer.Email
                    };
                    userManager.Create(user, customer.DocumentNumber);
                    //AddRole
                    userManager.AddToRole(user.Id, "Customer");
                }

                return RedirectToAction("Index");
            }

            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", customer.DocumentTypeId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", customer.DocumentTypeId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CustomerId,DocumentTypeId,DocumentNumber,BusinessName,FirstName,LastName,SecondLastName,Address,Email,Web,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1)), "DocumentTypeId", "Name", customer.DocumentTypeId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
            var user = userManager.FindByName(customer.Email);
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
