using NguyenPhanHuy_2122110062.Filter;
using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            var listItems = context.ProductCategories.OrderByDescending(x => x.CreatedDate).ToList();
            return View(listItems);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                model.CreatedDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(model.Title);

                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }

                if (string.IsNullOrEmpty(model.SeoDescription))
                {
                    model.SeoDescription = model.Description;
                }

                context.ProductCategories.Add(model);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var item = context.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(model.Title);

                context.ProductCategories.Attach(model);
                context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                TempData["success"] = "Thay đổi thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thay đổi thất bại!";

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.ProductCategories.Find(id);
            if (item != null)
            {
                context.ProductCategories.Remove(item);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var _id = context.ProductCategories.Find(Guid.Parse(item));
                        context.ProductCategories.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }
    }
}