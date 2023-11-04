using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using PBShop.Models;
namespace PBShop.Controllers
{
    public class SendMailerController : Controller
    {
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(PBShop.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add("testwebpbshop@gmail.com");
                mail.From = new MailAddress("testwebpbshop@gmail.com");
                mail.Subject = "Đặt lại mật khẩu";
                string Body = "Bấm vào đây để <a href='https://localhost:44364/PBSopHome/shop'>Đặt lại mật Khẩu</a>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("testwebpbshop@gmail.com", "zldo aupo wkff mqvi"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
    }
}