using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBShop.Models;


namespace PBShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        PBShopEntities db = new PBShopEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }

        public List<ProductModel> LayGioHang()
        {
            List<ProductModel> lstGioHang = Session["GioHang"] as List<ProductModel>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<ProductModel>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public List<ProductModel> LayDsSpThanhToan()
        {
            List<ProductModel> lstSPThanhToan = Session["DSThanhToan"] as List<ProductModel>;
            if (lstSPThanhToan == null)
            {
                lstSPThanhToan = new List<ProductModel>();
                Session["DSThanhToan"] = lstSPThanhToan;
            }
            return lstSPThanhToan;
        }

        public ActionResult ThemGioHang(int ms, string url)
        {
            //if(Session["Customers"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //} 
            //else
            //{
                List<ProductModel> lstGioHang = LayGioHang();
                ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
                if (sp == null)
                {
                    sp = new ProductModel(ms);
                    lstGioHang.Add(sp);
                }
                else
                {
                    sp.SoLuongP++;
                }
                return Redirect(url);
            //}    
           
        }

        public ActionResult ShopCart()
        {
            List<ProductModel> lstGioHang = LayGioHang();
            //if (lstGioHang.Count == 0)
            //{
            //    return RedirectToAction("Index", "PBSopHome");
            //}
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView(lstGioHang);
        }


        public ActionResult GioHangPartial()
        {
            List<ProductModel> lstGioHang = LayGioHang();
            //if (lstGioHang.Count == 0)
            //{
            //    return RedirectToAction("Index", "PBSopHome");
            //}
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView(lstGioHang);
        }

        public ActionResult XoaSPKhoiGioHang(int ID, string url)
        {
            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.SingleOrDefault(n => n.IDP == ID);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.IDP == ID);
                //if (lstGioHang.Count == 0)
                //{
                //    return RedirectToAction("Index", "SachOnline");
                //}
            }

            //return View();
            //return RedirectToAction("ShopCart");
            return Redirect(url);
        }

        private void CapNhatSoLuongP(int ms)
        { 
            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
            sp.SoLuongP++;
        }
        public ActionResult TangLen(int ms)
        {
            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
            sp.SoLuongP++;
            var Thanhtien = sp.dThanhTien;
            return Content(string.Format("{0:#,##0,0}", Thanhtien));
        }


        public ActionResult GiamXuong(int ms)
        {
            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
            if(sp.SoLuongP>1)
            {
                sp.SoLuongP--;
            }    
            
            var Thanhtien = sp.dThanhTien;
            return Content(Thanhtien.ToString());
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            
            List<ProductModel> lstGioHang = Session["GioHang"] as List<ProductModel>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.SoLuongP);

            }
            return iTongSoLuong;
        }


        private void CapNhatTongSoLuongP(int quantity ,int ms)
        {

            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
            sp.SoLuongP = quantity;
            ViewBag.TongSoLuong = TongSoLuong();
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<ProductModel> lstGioHang = Session["DSThanhToan"] as List<ProductModel>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }


        public ActionResult hehe()
        {
            double dTongTien = 0;
            dTongTien = TongTien();
            return Content(string.Format("{0:#,##0,0}", dTongTien));
        }




        public ActionResult XoaGioHang(string url)
        {
            List<ProductModel> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("ShopCart", "ShoppingCart");
        }
        //public ActionResult GioHangPartial()
        //{
        //    ViewBag.TongSoLuong = TongSoLuong();
        //    ViewBag.TongTien = TongTien();
        //    return PartialView();
        //}

        //[HttpGet]
        //public ActionResult test( int id)
        //{
        //    var Products = from s in db.Products where s.ID == id select s;

        //    return View(Products.SingleOrDefault());
        //}

        [HttpPost]
        public ActionResult Taodstt(List<int> selectedValues)
        {
            double dTongTien = 0;
            Session["DSThanhToan"] = null;
            List<ProductModel> lstSPThanhToan = LayDsSpThanhToan();
            List<ProductModel> lstGioHang = LayGioHang();

            if (selectedValues != null)
            {
                foreach (var id in selectedValues)
                {
                    ProductModel sp = lstGioHang.Find(n => n.IDP == id);
                    lstSPThanhToan.Add(sp);
                }
            }

            dTongTien = TongTien();


            return Content(string.Format("{0:#,##0,0}", dTongTien));
        }

        [HttpPost]
        public ActionResult DatHang(List<int> selectedValues)
        {
            if (Session["Customers"] == null || Session["Customers"].ToString() == "")
            {
                //return RedirectToAction("Login", "PBShopHome");
            }
            if (Session["GioHang"] == null)
            {
                //return RedirectToAction("Shop", "ShoppingCart");
            }
            List<ProductModel> lstSPThanhToan = LayDsSpThanhToan();
            List<ProductModel> lstGioHang = LayGioHang();

            if(selectedValues!=null)
            { 
                foreach (var id in selectedValues)
                {
                    ProductModel sp = lstGioHang.Find(n => n.IDP == id);
                    lstSPThanhToan.Add(sp);
                }
            }
           


            
            return View("DatHang",lstSPThanhToan);
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            List<ProductModel> lstSPThanhToan = LayDsSpThanhToan();
            return View(lstSPThanhToan);
        }

        //[HttpGet]
        //public ActionResult Dat()
        //{
        //    return View();
        //}

        //[HttpPost]
        public ActionResult Dat()
        {
            Order order = new Order();
            Customer kh = (Customer)Session["Customers"];
            List<ProductModel> lstGioHang = LayGioHang();
            List<ProductModel> lstSPThanhToan = LayDsSpThanhToan();
            order.MaKH = kh.ID;
            order.NgayDH = DateTime.Now;
            var NgayGiao = DateTime.Now;
            order.NgayGiaoHang = NgayGiao;
            order.HTGiaoHang = true;
            order.HTThanhToan = false;
            //db.DONDATHANGs.InsertOnSubmit(ddh);
            //db.SubmitChanges();
            db.Orders.Add(order);
            db.SaveChanges();
            //var MaOder = 
            foreach (var item in lstGioHang)
            {
                DetailsOder ctdh = new DetailsOder();
                //ctdh.ID_Oder = ddh.SoDH;
                ctdh.ID_Product = item.IDP;
                ctdh.ID_Oder = item.;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            ViewBag.mess = "Đặt Hàng Thành công";
            return RedirectToAction("Index","PBSopHome");
           
        }


        //[HttpPost]
        //public ActionResult DatHang(FormCollection f)
        //{
        //    //DONDATHANG ddh = new DONDATHANG();
        //    //Customer kh = (Customer)Session["TaiKhoan"];
        //    //List<ProductModel> lstGioHang = LayGioHang();
        //    //ddh.MaKH = kh.ID;
        //    //ddh.NgayDH = DateTime.Now;
        //    //var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
        //    //ddh.NgayGiaoHang = DateTime.Parse(NgayGiao);
        //    //ddh.HTGiaoHang = true;
        //    //ddh.HTThanhToan = false;
        //    ////db.DONDATHANGs.InsertOnSubmit(ddh);
        //    ////db.SubmitChanges();
        //    //db.DONDATHANGs.Add(ddh);
        //    //db.SaveChanges();
        //    ////foreach (var item in lstGioHang)
        //    ////{
        //    ////    ct ctdh = new CTDATHANG();
        //    ////    ctdh.SoDH = ddh.SoDH;
        //    ////    ctdh.MaSach = item.iMaSach;
        //    ////    ctdh.SoLuong = item.iSoLuong;
        //    ////    ctdh.DonGia = (decimal)item.dDonGia;
        //    ////    db.CTDATHANGs.InsertOnSubmit(ctdh);
        //    ////}
        //    ////db.SubmitChanges();
        //    //Session["GioHang"] = null;
        //    return RedirectToAction("XacNhanDonHang", "GioHang");
        //}

        public ActionResult XacNhanDonHang()
        {
            return View();
        }




        //thanh toán vnpay


        //    protected void btnPay_Click(object sender, EventArgs e)
        //{
        //    //Get Config Info
        //    string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
        //    string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
        //    string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
        //    string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

        //    //Get payment input
        //    //OrderInfo order = new OrderInfo();
        //    //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        //    //order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
        //    //order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
        //    //order.CreatedDate = DateTime.Now;
        //    //Save order to db

        //    //Build URL for VNPAY
        //    VnPayLibrary vnpay = new VnPayLibrary();

        //    vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        //    vnpay.AddRequestData("vnp_Command", "pay");
        //    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        //    vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
        //    if (bankcode_Vnpayqr.Checked == true)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
        //    }
        //    else if (bankcode_Vnbank.Checked == true)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "VNBANK");
        //    }
        //    else if (bankcode_Intcard.Checked == true)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "INTCARD");
        //    }

        //    vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
        //    vnpay.AddRequestData("vnp_CurrCode", "VND");
        //    vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

        //    if (locale_Vn.Checked == true)
        //    {
        //        vnpay.AddRequestData("vnp_Locale", "vn");
        //    }
        //    else if (locale_En.Checked == true)
        //    {
        //        vnpay.AddRequestData("vnp_Locale", "en");
        //    }
        //    vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
        //    vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

        //    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        //    vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        //    //Add Params of 2.1.0 Version
        //    //Billing

        //    string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        //    log.InfoFormat("VNPAY URL: {0}", paymentUrl);
        //    Response.Redirect(paymentUrl);
        //}
    }
}
