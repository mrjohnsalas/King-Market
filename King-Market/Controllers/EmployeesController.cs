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
using PagedList;

namespace King_Market.Controllers
{
    public class EmployeesController : Controller
    {
        private King_MarketContext db = new King_MarketContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();

        // GET: Employees
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstName_Desc" : String.Empty;
            ViewBag.DocumentTypeSortParm = sortOrder == "Document Type" ? "DocumentType_Desc" : "Document Type";
            ViewBag.EmployeeTypeSortParm = sortOrder == "Employee Type" ? "EmployeeType_Desc" : "Employee Type";
            ViewBag.DocumentNumberSortParm = sortOrder == "Document Number" ? "DocumentNumber_Desc" : "Document Number";
            ViewBag.LastNameSortParm = sortOrder == "Last Name" ? "LastName_Desc" : "Last Name";
            ViewBag.SecondLastNameSortParm = sortOrder == "Second Last Name" ? "SecondLastName_Desc" : "Second Last Name";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_Desc" : "Email";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "Phone_Desc" : "Phone";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var employees = db.Employees.Include(e => e.DocumentType).Include(e => e.EmployeeType);

            if (!String.IsNullOrEmpty(searchString))
                employees = employees.Where(s => 
                    s.EmployeeType.Name.Contains(searchString) || 
                    s.DocumentNumber.Contains(searchString) ||
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString) ||
                    s.SecondLastName.Contains(searchString) ||
                    s.Email.Contains(searchString) || 
                    s.Phone.Contains(searchString));

            switch (sortOrder)
            {
                case "FirstName_Desc":
                    employees = employees.OrderByDescending(p => p.FirstName);
                    break;
                case "DocumentType_Desc":
                    employees = employees.OrderByDescending(p => p.DocumentType.Name);
                    break;
                case "Document Type":
                    employees = employees.OrderBy(p => p.DocumentType.Name);
                    break;
                case "EmployeeType_Desc":
                    employees = employees.OrderByDescending(p => p.EmployeeType.Name);
                    break;
                case "Employee Type":
                    employees = employees.OrderBy(p => p.EmployeeType.Name);
                    break;
                case "DocumentNumber_Desc":
                    employees = employees.OrderByDescending(p => p.DocumentNumber);
                    break;
                case "Document Number":
                    employees = employees.OrderBy(p => p.DocumentNumber);
                    break;
                case "LastName_Desc":
                    employees = employees.OrderByDescending(p => p.LastName);
                    break;
                case "Last Name":
                    employees = employees.OrderBy(p => p.LastName);
                    break;
                case "Second Last Name":
                    employees = employees.OrderBy(p => p.SecondLastName);
                    break;
                case "SecondLastName_Desc":
                    employees = employees.OrderByDescending(p => p.SecondLastName);
                    break;
                case "Email":
                    employees = employees.OrderBy(p => p.Email);
                    break;
                case "Email_Desc":
                    employees = employees.OrderByDescending(p => p.Email);
                    break;
                case "Phone":
                    employees = employees.OrderBy(p => p.Phone);
                    break;
                case "Phone_Desc":
                    employees = employees.OrderByDescending(p => p.Phone);
                    break;
                default:
                    employees = employees.OrderBy(p => p.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
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
