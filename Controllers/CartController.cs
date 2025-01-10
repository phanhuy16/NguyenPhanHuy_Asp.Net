using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Linq;
using System.Web.Mvc;
using NguyenPhanHuy_2122110062.Common;
using System.Configuration;
using NguyenPhanHuy_2122110062.Filter;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace NguyenPhanHuy_2122110062.Controllers
{
    [AuthenticationFilter]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public CartController()
        {
        }

        public CartController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Cart
        public ActionResult Index()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart.Items;
            }
            return View();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        public ActionResult PartialCartItem()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public ActionResult PartialCartCheckOut()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddToCart(Guid id, int qty)
        {
            var code = new { success = false, message = "Thêm vào giỏ hàng không thành công!", code = -1, count = 0 };

            var checkProduct = context.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = Session["Cart"] as ShoppingCart;

                if (cart == null)
                {
                    cart = new ShoppingCart();
                }

                CartItem item = new CartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(checkProduct.Title),
                    Quantity = qty
                };

                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImage = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }

                if (checkProduct.PriceSale > 0)
                {
                    item.Price = checkProduct.PriceSale;
                }
                else
                {
                    item.Price = checkProduct.Price;
                }

                item.TotalPrice = item.Price * item.Quantity;
                cart.AddToCart(item, qty);
                Session["Cart"] = cart;

                code = new { success = true, message = "Thêm vào giỏ hàng thành công!", code = 1, count = cart.Items.Count };
            }
            return Json(code);
        }

        public ActionResult ShowCount()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;

            if (cart != null)
            {
                return Json(new { success = true, count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, count = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var code = new { success = false, message = "Xoá không thành công!", code = -1, count = 0 };

            ShoppingCart cart = Session["Cart"] as ShoppingCart;

            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Delete(id);
                    code = new { success = true, message = "Xoá thành công!", code = 1, count = cart.Items.Count };
                }
            }

            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { success = true, message = "Xoá thành công!" });
            }
            return Json(new { success = false, message = "Xoá không thành công!" });
        }

        [HttpPost]
        public ActionResult Update(Guid id, int qty)
        {
            ShoppingCart cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.Update(id, qty);
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            return Json(new { success = false, message = "Cập nhật không thành công!" });
        }

        public ActionResult PartialCheckOut()
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if(user != null)
            {
                ViewBag.User = user;
            }
            return PartialView();
        }

        public ActionResult CheckOutSuccess(Guid id)
        {
            var order = context.Orders.OrderByDescending(x => x.CreatedDate).FirstOrDefault(x=>x.Id == id);
           
            ViewBag.Order = order;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderModel requied)
        {
            if (ModelState.IsValid)
            {
                ShoppingCart cart = Session["Cart"] as ShoppingCart;
                if (cart != null && cart.Items.Any())
                {
                    Order order = new Order();
                    order.Id = Guid.NewGuid();
                    order.CustomerName = requied.CustomerName;
                    order.Phone = requied.Phone;
                    order.Address = requied.Address;
                    order.City = requied.City;
                    order.District = requied.District;
                    order.Ward = requied.Ward;
                    order.Email = requied.Email;
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));

                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.TypePayment = requied.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = requied.CustomerName;
                    order.ModifierDate = DateTime.Now;
                    Random random = new Random();
                    order.Code = "DH" + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9) + random.Next(0, 9);

                    context.Orders.Add(order);
                    context.SaveChanges();

                    // Gửi mail thông báo đơn hàng
                    var strProduct = "";
                    var intoMoney = decimal.Zero;
                    var totalMoney = decimal.Zero;
                    foreach (var item in cart.Items)
                    {
                        strProduct += "<tr>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word\">" + item.ProductName + "</td>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + item.Quantity + "</td>";
                        strProduct += "<td style=\"color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif\">" + "<span>" + Format.FormatNumber(item.TotalPrice) + "&nbsp;<span>VND</span></span>" + "</td>";
                        strProduct += "</tr>";
                        intoMoney += item.Price * item.Quantity;
                    }
                    totalMoney = intoMoney;
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    content = content.Replace("{{MaDon}}", order.Code);
                    content = content.Replace("{{SanPham}}", strProduct);
                    content = content.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    content = content.Replace("{{TenKhachHang}}", order.CustomerName);
                    content = content.Replace("{{Phone}}", order.Phone);
                    content = content.Replace("{{Email}}", requied.Email);
                    content = content.Replace("{{DiaChi}}", order.Address);
                    content = content.Replace("{{Phuong}}", order.Ward);
                    content = content.Replace("{{Quan}}", order.District);
                    content = content.Replace("{{Tinh}}", order.City);
                    content = content.Replace("{{ThanhTien}}", Format.FormatNumber(intoMoney));
                    content = content.Replace("{{TongTien}}", Format.FormatNumber(totalMoney));
                    Common.Common.SendMail("Alistyle", "Đơn hàng #" + order.Code, content.ToString(), requied.Email);

                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strProduct);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", requied.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChi}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{Phuong}}", order.Ward);
                    contentAdmin = contentAdmin.Replace("{{Quan}}", order.District);
                    contentAdmin = contentAdmin.Replace("{{Tinh}}", order.City);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", Format.FormatNumber(intoMoney));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", Format.FormatNumber(totalMoney));
                    Common.Common.SendMail("Alistyle", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["Email"]);

                    cart.ClearCart();

                    var RedirectUrl = Url.Action("CheckOutSuccess", "Cart", new { id = order.Id });
                    return Json(new { success = true, redirectUrl = RedirectUrl });
                }
            }
            return Json(new { success = false, message = "Thanh toán không thành công!" });
        }
    }
}