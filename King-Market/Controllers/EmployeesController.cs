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
    public class EmployeesController : Controller
    {
        private King_MarketContext db = new King_MarketContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Employees
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.DocumentType).Include(e => e.EmployeeType);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name");
            ViewBag.EmployeeTypeId = new SelectList(db.EmployeeTypes, "EmployeeTypeId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "EmployeeId,DocumentTypeId,EmployeeTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                //Create User
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
                var user = userManager.FindByName(employee.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = employee.Email,
                        Email = employee.Email
                    };
                    userManager.Create(user, employee.DocumentNumber);
                    //AddRole
                    if (employee.EmployeeTypeId.Equals(1))
                        userManager.AddToRole(user.Id, "Buyer");
                    else
                        userManager.AddToRole(user.Id, "Grocer");
                }

                return RedirectToAction("Index");
            }

            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", employee.DocumentTypeId);
            ViewBag.EmployeeTypeId = new SelectList(db.EmployeeTypes, "EmployeeTypeId", "Name", employee.EmployeeTypeId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", employee.DocumentTypeId);
            ViewBag.EmployeeTypeId = new SelectList(db.EmployeeTypes, "EmployeeTypeId", "Name", employee.EmployeeTypeId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "EmployeeId,DocumentTypeId,EmployeeTypeId,DocumentNumber,FirstName,LastName,SecondLastName,Email,Phone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                //Update Role
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
                var user = userManager.FindByName(employee.Email);
                if (user != null)
                {
                    if (employee.EmployeeTypeId.Equals(1))
                    {
                        if (userManager.IsInRole(user.Id, "Grocer"))
                            userManager.RemoveFromRole(user.Id, "Grocer");
                        if (!userManager.IsInRole(user.Id, "Buyer"))
                            userManager.AddToRole(user.Id, "Buyer");
                    }
                    else
                    {
                        if (userManager.IsInRole(user.Id, "Buyer"))
                            userManager.RemoveFromRole(user.Id, "Buyer");
                        if (!userManager.IsInRole(user.Id, "Grocer"))
                            userManager.AddToRole(user.Id, "Grocer");
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes.ToList().FindAll(d => d.ClassDocumentTypeId.Equals(1) && !d.OnlyForEnterprise), "DocumentTypeId", "Name", employee.DocumentTypeId);
            ViewBag.EmployeeTypeId = new SelectList(db.EmployeeTypes, "EmployeeTypeId", "Name", employee.EmployeeTypeId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db2));
            var user = userManager.FindByName(employee.Email);
            userManager.Delete(user);

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
