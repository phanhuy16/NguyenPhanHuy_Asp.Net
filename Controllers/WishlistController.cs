using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    [AllowAnonymous]
    public class WishlistController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Wishlist
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddWishlist(Guid productId)
        {
            if (Request.IsAuthenticated == false)
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập" });
            }

            var checkitem = context.Wishlists.FirstOrDefault(x => x.ProductId == productId && x.UserName == User.Identity.Name);
            if (checkitem != null)
            {
                return Json(new { success = false, message = "Sản phẩm này đã được yêu thích!" });
            }

            var item = new Wishlist();

            item.Id = Guid.NewGuid();
            item.ProductId = productId;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;

            context.Wishlists.Add(item);
            context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteWishlist(Guid id)
        {
            var checkitem = context.Wishlists.FirstOrDefault(x => x.ProductId == id && x.UserName == User.Identity.Name);

            if (checkitem != null)
            {
                context.Wishlists.Remove(checkitem);
                context.SaveChanges();
                return Json(new { success = true, message = "Bỏ yêu thích thành công!" });
            }
            return Json(new { success = false, message = "Bỏ yêu thích thất bại!" });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}