using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using King_Market.Models;

namespace King_Market.Controllers
{
    public class ProductPhotosController : Controller
    {
        private King_MarketContext db = new King_MarketContext();

        // GET: ProductPhotos
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.ProductPhotos.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}