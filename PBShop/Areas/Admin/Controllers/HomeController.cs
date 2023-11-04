using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBShop.Models;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace PBShop.Areas.Admin.Controllers
{
   
    public class HomeController : Controller
    {
        PBShopEntities db = new PBShopEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
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