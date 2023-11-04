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
        }

        public ActionResult ShopCart()
        {
            List<ProductModel> lstGioHang = LayGioHang();
            //if (lstGioHang.Count == 0)
            //{
            //    return RedirectToAction("Index", "PBSopHome");
            //}
            ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
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
            return Content(Thanhtien.ToString());
        }


        public ActionResult GiamXuong(int ms)
        {
            List<ProductModel> lstGioHang = LayGioHang();
            ProductModel sp = lstGioHang.Find(n => n.IDP == ms);
            sp.SoLuongP--;
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
            List<ProductModel> lstGioHang = Session["GioHang"] as List<ProductModel>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
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
           


            //ViewBag.TongSoLuong = TongSoLuong();
            //ViewBag.TongTien = TongTien();
            return View("DatHang",lstSPThanhToan);
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            List<ProductModel> lstSPThanhToan = LayDsSpThanhToan();
            return View(lstSPThanhToan);
        }

        [HttpPost]
        public ActionResult Dat()
        {
            DONDATHANG ddh = new DONDATHANG();
            Customer kh = (Customer)Session["Customers"];
            //List<ProductModel> lstGioHang = LayGioHang();
            //ddh.MaKH = 1;
            ddh.NgayDH = DateTime.Now;
            var NgayGiao = DateTime.Now;
            ddh.NgayGiaoHang = NgayGiao;
            ddh.HTGiaoHang = true;
            ddh.HTThanhToan = false;
            //db.DONDATHANGs.InsertOnSubmit(ddh);
            //db.SubmitChanges();
            db.DONDATHANGs.Add(ddh);
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
            Session["DSThanhToan"] = null;
            return View();
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
