using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using King_Market.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace King_Market.Controllers
{
    public class ProductsController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: Products
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : String.Empty;
            ViewBag.ProductTypeSortParm = sortOrder == "Product Type" ? "ProductType_Desc" : "Product Type";
            ViewBag.UnitMeasureSortParm = sortOrder == "Unit Measure" ? "UnitMeasure_Desc" : "Unit Measure";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_Desc" : "Description";
            ViewBag.UnitPriceSortParm = sortOrder == "Unit Price" ? "UnitPrice_Desc" : "Unit Price";
            ViewBag.UnitCostSortParm = sortOrder == "Unit Cost" ? "UnitCost_Desc" : "Unit Cost";
            ViewBag.StockSortParm = sortOrder == "Stock" ? "Stock_Desc" : "Stock";
            ViewBag.MinStockSortParm = sortOrder == "Min Stock" ? "MinStock_Desc" : "Min Stock";
            ViewBag.MaxStockSortParm = sortOrder == "Max Stock" ? "MaxStock_Desc" : "Max Stock";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var products = db.Products.Include(p => p.ProductType).Include(p => p.UnitMeasure);

            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(s => 
                    s.Name.Contains(searchString) || 
                    s.Description.Contains(searchString) || 
                    s.UnitMeasure.ShortName.Contains(searchString));

            switch (sortOrder)
            {
                case "Name_Desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Product Type":
                    products = products.OrderBy(p => p.ProductType.Name);
                    break;
                case "ProductType_Desc":
                    products = products.OrderByDescending(p => p.ProductType.Name);
                    break;
                case "Unit Measure":
                    products = products.OrderBy(p => p.UnitMeasure.ShortName);
                    break;
                case "UnitMeasure_Desc":
                    products = products.OrderByDescending(p => p.UnitMeasure.ShortName);
                    break;
                case "Description":
                    products = products.OrderBy(p => p.Description);
                    break;
                case "Description_Desc":
                    products = products.OrderByDescending(p => p.Description);
                    break;
                case "Unit Price":
                    products = products.OrderBy(p => p.UnitPrice);
                    break;
                case "UnitPrice_Desc":
                    products = products.OrderByDescending(p => p.UnitPrice);
                    break;
                case "Unit Cost":
                    products = products.OrderBy(p => p.UnitCost);
                    break;
                case "UnitCost_Desc":
                    products = products.OrderByDescending(p => p.UnitCost);
                    break;
                case "Stock":
                    products = products.OrderBy(p => p.Stock);
                    break;
                case "Stock_Desc":
                    products = products.OrderByDescending(p => p.Stock);
                    break;
                case "Min Stock":
                    products = products.OrderBy(p => p.MinStock);
                    break;
                case "MinStock_Desc":
                    products = products.OrderByDescending(p => p.MinStock);
                    break;
                case "Max Stock":
                    products = products.OrderBy(p => p.MaxStock);
                    break;
                case "MaxStock_Desc":
                    products = products.OrderByDescending(p => p.MaxStock);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(f => f.ProductPhotos).SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name");
            ViewBag.UnitMeasureId = new SelectList(db.UnitMeasures, "UnitMeasureId", "ShortName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductId,Name,Description,UnitPrice,UnitCost,ProductTypeId,Stock,MinStock,MaxStock,UnitMeasureId")] Product product)
        {
            if (ModelState.IsValid)
            {
                List<ProductPhoto> productPhotos = new List<ProductPhoto>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var photo = new ProductPhoto
                        {
                            FileName = Path.GetFileName(file.FileName),
                            FileType = FileType.Photo,
                            ContentType = file.ContentType
                        };
                        using (var reader = new BinaryReader(file.InputStream))
                            photo.Content = reader.ReadBytes(file.ContentLength);
                        productPhotos.Add(photo);
                    }
                }
                product.ProductPhotos = productPhotos;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);
            ViewBag.UnitMeasureId = new SelectList(db.UnitMeasures, "UnitMeasureId", "ShortName", product.UnitMeasureId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(f => f.ProductPhotos).SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);
            ViewBag.UnitMeasureId = new SelectList(db.UnitMeasures, "UnitMeasureId", "ShortName", product.UnitMeasureId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductId,Name,Description,UnitPrice,UnitCost,ProductTypeId,Stock,MinStock,MaxStock,UnitMeasureId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductPhotos == null)
                    product.ProductPhotos = new List<ProductPhoto>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var photo = new ProductPhoto
                        {
                            FileName = Path.GetFileName(file.FileName),
                            FileType = FileType.Photo,
                            ContentType = file.ContentType,
                            ProductId = product.ProductId
                        };
                        using (var reader = new BinaryReader(file.InputStream))
                            photo.Content = reader.ReadBytes(file.ContentLength);
                        product.ProductPhotos.Add(photo);
                        db.Entry(photo).State = EntityState.Added;
                    }
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "ProductTypeId", "Name", product.ProductTypeId);
            ViewBag.UnitMeasureId = new SelectList(db.UnitMeasures, "UnitMeasureId", "ShortName", product.UnitMeasureId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            var photos = db.ProductPhotos.ToList().FindAll(p => p.ProductId.Equals(id));
            foreach (var photo in photos)
                db.ProductPhotos.Remove(photo);

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteFile(int? id)
        {
            if (!id.HasValue)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new {Result = "Error"});
            }
            try
            {
                ProductPhoto photo = db.ProductPhotos.Find(id);
                if (photo == null)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                db.ProductPhotos.Remove(photo);
                db.SaveChanges();

                return Json(new {Result = "Ok"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public FileResult Download(int? id)
        {
            ProductPhoto photo = db.ProductPhotos.Find(id);
            return File(photo.Content, System.Net.Mime.MediaTypeNames.Application.Octet, photo.FileName);
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
