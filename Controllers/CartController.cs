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
using NguyenPhanHuy_2122110062.Common.Payment;
using static NguyenPhanHuy_2122110062.Common.Payment.VnPayLibrary;
using System.Security.Policy;

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
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
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
            if (user != null)
            {
                ViewBag.User = user;
            }
            return PartialView();
        }
        public ActionResult VnPayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var orderItem = context.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (orderItem != null)
                        {
                            orderItem.Status = 2;
                            context.Orders.Attach(orderItem);
                            context.Entry(orderItem).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;

                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    //displayAmount.InnerText = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }

                ViewBag.Code = orderCode;
                ViewBag.CreatedDate = DateTime.Now;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderModel requied)
        {
            var code = new { success = false, code = 1, Url = "", id = Guid.Empty };
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
                    order.Status = 1;
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

                    code = new { success = true, code = requied.TypePayment, Url = "", id = order.Id };
                    if (requied.TypePayment == 2)
                    {
                        var url = UrlPayment(requied.TypePaymentVN, order.Code);
                        code = new { success = true, code = requied.TypePayment, Url = url, id = order.Id };
                    }
                }
            }
            return Json(code);
        }

        #region Thanh toan VNPay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            string paymentUrl = string.Empty;
            var order = context.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //Response.Redirect(paymentUrl);
            return paymentUrl;
        }

        #endregion
    }
}