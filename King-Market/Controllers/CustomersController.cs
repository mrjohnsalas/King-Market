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
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.DocumentType);
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
