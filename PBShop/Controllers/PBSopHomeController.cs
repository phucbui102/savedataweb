using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBShop.Models;
using PagedList;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace PBShop.Controllers
{
    public class PBSopHomeController : Controller
     {
        
        PBShopEntities db = new PBShopEntities();
        // GET: PBSopHome
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


            ViewBag.NewProduct = LP.OrderBy("DateAdded" + " desc");

            ViewBag.BALProduct = LP.OrderBy("DateAdded" + " desc");

            ViewBag.OSProduct = LP.OrderBy("DateAdded" + " desc");

            return View();




        }
        public ActionResult Contact()
        {
           
            return View();

        }
        public ActionResult PartialMenu()
        {
            var ListTypeP = from LTP in db.Types select LTP;
            return PartialView(ListTypeP);
        }
        public ActionResult FindProduct()
        {
            var ListType = from a in db.Types select a;
            return View(ListType);

        }

        public ActionResult ProductDetails(int id)
        {
            var Products = from s in db.Products where s.ID == id select s;
            return View(Products.Single());
        }
        public ActionResult PartialProductDetails(int id)
        {
            var Products = from s in db.Products where s.ID == id select s;
            return PartialView(Products.Single());
        }
        public ActionResult shop(int? size, int? page, int? IDType, string searchString, string Pr_From, string Pr_To, string sortOrder = "", int categoryID = 0)
        {

            
           

            //1.2.Tạo câu truy vấn kết 3 bảng Book, Author, Category
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
                      }) ;

            //tìn kiếm theo giá
           
            if (!String.IsNullOrEmpty(Pr_To) )
            {
                //float F = float.Parse(Pr_From);
                float F = 0;
                float T = float.Parse(Pr_To);
                if (String.IsNullOrEmpty(Pr_From))
                {
                    LP = LP.Where(a => a.Price <= T);
                }
                //LP = LP.Where(a => a.Price >= F && a.Price <= T);
                ViewBag.PrFrom = F;
                ViewBag.PrTo = T; 
            }

           


            if (IDType != 0 && IDType != null)
            {
                LP = LP.Where(a => a.Id_Type == IDType);
            }

            //1.2 Lưu chủ đề tìm kiếm
            ViewBag.Subject = IDType;

            // 1. Đoạn code dùng để tìm kiếm
            // 1.1. Lưu tư khóa tìm kiếm
            ViewBag.Keyword = searchString;
            //3. Tìm kiếm theo searchString
            if (!String.IsNullOrEmpty(searchString))
                //LPP =  LPP.Where(b => b.Name.Contains(searchString));
                LP = LP.Where(b => b.Name.Contains(searchString));

            //4. Tìm kiếm theo CategoryID
            if (categoryID != 0 && categoryID != null)
            {
                LP = LP.Where(b => b.Id_Type == categoryID);
            }
            
            


            // 2 Đoạn code này dùng để sắp xếp
            // 2.1. Tạo biến ViewBag gồm sortOrder, searchValue, sortProperty và page
            if (sortOrder == "asc") ViewBag.SortOrder = "asc";
            if (sortOrder == "desc") ViewBag.SortOrder = "desc";
            //if (sortOrder == "") ViewBag.SortOrder = "asc";
            // 2.1. Tạo thuộc tính sắp xếp mặc định là "giá"

            string sortProperty = "Price";

            // 2.2. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
            if (sortOrder == "desc")
                LP = LP.OrderBy(sortProperty + " desc");
            else
                LP = LP.OrderBy(sortProperty);




            // 3 Đoạn code sau dùng để phân trang
            ViewBag.Page = page;

            //// 3.1. Tạo danh sách chọn số trang
            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem { Text = "5", Value = "5" });
            //items.Add(new SelectListItem { Text = "10", Value = "10" });
            //items.Add(new SelectListItem { Text = "20", Value = "20" });
            //items.Add(new SelectListItem { Text = "25", Value = "25" });
            //items.Add(new SelectListItem { Text = "50", Value = "50" });
            //items.Add(new SelectListItem { Text = "100", Value = "100" });
            //items.Add(new SelectListItem { Text = "200", Value = "200" });

            // 3.2. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
            //foreach (var item in items)
            //{
            //    if (item.Value == size.ToString()) item.Selected = true;
            //}
            //ViewBag.Size = items;
            //ViewBag.CurrentSize = size;
            // 3.3. Nếu page = null thì đặt lại là 1.
            page = page ?? 1; //if (page == null) page = 1;

            // 3.4. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 3);

            ViewBag.pageSize = pageSize;

            // 3.5. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 3.6 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
            //int checkTotal = (int)(books.ToList().Count / pageSize) + 1;
            // Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
            //if (pageNumber > checkTotal) pageNumber = checkTotal;

            // 4. Trả kết quả về Views

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Giá: Tăng Dần", Value = "asc" });
            items.Add(new SelectListItem { Text = "Giá: Giảm Dần", Value = "desc" });
            ViewBag.SortValue = items;
            return View(LP.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult FindPartial(string searchString)
        {

            //var books = from a in db.Products join b in db.Types on a.Id_Type equals b.ID select new { a, b };
            //var LP = (from s in db.Products select s);
            ViewBag.Keyword = searchString;

            ////3. Tìm kiếm theo searchString
            //if (!String.IsNullOrEmpty(searchString))
            //    LP = LP.Where(b => b.Name.Contains(searchString));

            ////4. Tìm kiếm theo CategoryID
            //if (category != 0)
            //{
            //    LP = LP.Where(c => c.Id_Type == category);
            //}
            //5. Tạo danh sách danh mục để hiển thị ở giao diện View thông qua DropDownList
            ViewBag.categoryID = new SelectList(db.Types, "ID", "NameType"); // danh sách Category
            return PartialView();
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
                Customer kh =  db.Customers.SingleOrDefault(n => n.Email == sEmail && n.Password == sPassword);
                if (kh != null)
                {
                    
                    Session["NameCustomers"] = kh.Name;
                    Session["EmailCustomers"] = kh.Email;
                    Session["Customers"] = kh;
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


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, Customer kh)
        {

            var sHoTen = collection["Name"];
            var sMatKhau = collection["Password"];
            var sEmail = collection["Email"];
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else
            {
                kh.Name = sHoTen;
                kh.Password = sMatKhau.ToString();
                kh.Email = sEmail.ToString();
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Customers.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return this.Register();
        }

    }
}