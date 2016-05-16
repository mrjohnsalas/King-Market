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

namespace King_Market.Controllers
{
    public class ProductsController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: Products
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductType).Include(p => p.UnitMeasure);
            return View(products.ToList());
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
            Product product = db.Products.Include(f => f.ProductPhotos).SingleOrDefault(p => p.ProductId == id);
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
