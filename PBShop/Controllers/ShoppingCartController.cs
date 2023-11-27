using System;
using System.Collections.Generic;
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
            //foreach (var item in lstGioHang)
            //{
            //    ct ctdh = new CTDATHANG();
            //    ctdh.SoDH = ddh.SoDH;
            //    ctdh.MaSach = item.iMaSach;
            //    ctdh.SoLuong = item.iSoLuong;
            //    ctdh.DonGia = (decimal)item.dDonGia;
            //    db.CTDATHANGs.InsertOnSubmit(ctdh);
            //}
            //db.SubmitChanges();
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
    }
}
