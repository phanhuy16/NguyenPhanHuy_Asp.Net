using NguyenPhanHuy_2122110062.Filter;
using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class ProductImagesController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/ProductImages
        public ActionResult Index(Guid id)
        {
            ViewBag.ProductId = id;
            var items = context.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(Guid id, string url)
        {
            if (id != Guid.Empty && !string.IsNullOrEmpty(url))
            {
                context.ProductImages.Add(new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ProductId = id,
                    Image = url,
                    IsDefault = false
                });
                context.SaveChanges();

                return Json(new { success = true, message = "Thêm ảnh thành công!" });
            }

            return Json(new { success = false, message = "Thêm ảnh thất bại!" });
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.ProductImages.Find(id);

            if (item != null)
            {
                if (item.IsDefault)
                {
                    return Json(new { success = false, message = "Không được phép xoá!" });
                }
                context.ProductImages.Remove(item);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }
    }
}