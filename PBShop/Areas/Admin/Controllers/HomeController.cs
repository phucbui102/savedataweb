using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using PBShop.Models;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Net;

namespace PBShop.Areas.Admin.Controllers
{
   
    public class HomeController : Controller
    {
        PBShopEntities db = new PBShopEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var LP = (from s in db.Products
                      join c in db.Types on s.Id_Type equals c.ID
                      select new GetProduct

                      {
                          ID = s.ID,
                          Name = s.Name,
                          Price = s.Price,
                          Promotional_Price = s.Promotional_Price,
                          Img = s.Img,
                          Describe = s.Describe,
                          DateAdded = s.DateAdded,
                          NameType = c.NameType,
                          Id_Type = c.ID
                      });
            return View(LP.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.Types, "ID", "Nametype");
            ViewBag.CategoryID = new SelectList(db.Products, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "BookID,Title,AuthorID,Price,Images,CategoryID,Description,Published,ViewCount")] Product Product, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Images.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(Images.FileName);
                        string _path = Path.Combine(Server.MapPath("~/bookimages"), _FileName);
                        Images.SaveAs(_path);
                        Product.Img = _FileName;
                    }
                    db.Products.Add(Product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }

            }

            ViewBag.Type = new SelectList(db.Types, "ID", "Nametype", Product.Id_Type);
            ViewBag.CategoryID = new SelectList(db.Products, "CategoryID", "CategoryName", Product.ID);
            return View(Product);
        }


        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DetailsPartial",Product);
        }
        //public ActionResult Details(int id)
        //{
        //    // Fetch the details for the specified item with the given 'id'
        //    // You can query your data source or perform any necessary operations

        //    // Return a partial view with the details
        //     Product Product = db.Products.Find(id);
        //    return PartialView("_DetailsPartial", details);
        //}

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product Product = db.Products.Find(id);
            db.Products.Remove(Product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var sEmail = collection["Email"];
            var sPassword = collection["Password"];
            if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err1"] = "Bạn chưa nhập Email";
            }
            else if (String.IsNullOrEmpty(sPassword))
            {
                ViewData["err2"] = "Bạn chưa nhập mật khẩu";
            }
            else
            {
                Customer kh = db.Customers.SingleOrDefault(n => n.Email == sEmail && n.Password == sPassword);
                if (kh != null)
                {

                    Session["NameCustomers"] = kh.Name;
                    Session["EmailCustomers"] = kh.Email;
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

    }
}