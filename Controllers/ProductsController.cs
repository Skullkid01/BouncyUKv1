using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BouncyUKv1.Models;
using System.IO;
using BouncyUKv1.Logic;
using System.Threading.Tasks;

namespace BouncyUK.Controllers
{
    [RequireHttps]
    public class ProductsController : Controller
    {
        private DataContext db = new DataContext();

        ProductLogic pl = new ProductLogic();
        ReferLogic rl = new ReferLogic();

        // GET: Products
        public ActionResult Index(string PName, string PPrice, string PCategory)
        {
            return View(pl.coreSearchAdmin(PName, PPrice, PCategory));
        }

        public ActionResult CastleReference(string refer)
        {
            return View(rl.coreSearchAdmin(refer));
        }
        public ActionResult ProductPage(Product product)
        {

            if (Session["Username"] == null)
            {
                ViewBag.Message = "Login/Register To Book";
            }
            var list = db.Products.ToList();
            return View(list);


        }
        public ActionResult SearchProductPage(string PName, string PPrice, string PCategory)
        {
            return View(pl.coreSearchAdmin(PName, PPrice, PCategory));
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Description,Category,Price,Image,ImageType,ProductRef")] Product product, HttpPostedFileBase file)
        {
            Session["Price"] = product.Price.ToString();

            if (file != null && file.ContentLength > 0)
            {
                product.ImageType = Path.GetExtension(file.FileName);
                product.Image = ConvertToBytes(file);
            }
            if (ModelState.IsValid)
            {
                product.ProductRef = product.GetRef();
                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("ProductPage");
            }


            return View(product);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);
            return reader.ReadBytes((int)file.ContentLength);
        }

        public async Task<FileStreamResult> RenderImage(int id)
        {
            MemoryStream ms = null;
            var item = await db.Products.FindAsync(id);
            if (item != null)
            {
                ms = new MemoryStream(item.Image);
            }
            return new FileStreamResult(ms, item.ImageType);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Description,Category,Price,ProductQuantity,Image,ImageType")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
