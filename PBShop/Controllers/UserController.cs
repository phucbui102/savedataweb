using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBShop.Models;
using System.Net.Mail;

namespace PBShop.Controllers
{
    public class UserController : Controller
    {
        PBShopEntities db = new PBShopEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPW()
        {
            return View();
        }

        [HttpPost]
        public ActionResult resetPW()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RequestResetPX()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestResetPX(FormCollection collection)
        {
            var sEmail = collection["Email"];
                Customer TK = db.Customers.SingleOrDefault(n => n.Email == sEmail );
                if (TK != null)
                {
                ViewBag.ThongBao = "1";
                MailMessage mail = new MailMessage();
                mail.To.Add("testwebpbshop@gmail.com");
                mail.From = new MailAddress("testwebpbshop@gmail.com");
                mail.Subject = "Đặt lại mật khẩu";
                string Body = "Bấm vào đây để <a href='https://localhost:44364/User/ResetPW'>Đặt lại mật Khẩu</a>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("testwebpbshop@gmail.com", "zldo aupo wkff mqvi"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
                else
                {
                ViewBag.ThongBao = " ";
            }
         
            return View();
        }
    }
}