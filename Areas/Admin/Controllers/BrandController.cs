using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Brand
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 5;

            if (page == null)
            {
                page = 1;
            }

            IEnumerable<Brand> listBrand = context.Brands.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(search))
            {
                listBrand = listBrand.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listBrand = listBrand.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listBrand);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand model)
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

                context.Brands.Add(model);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var item = context.Brands.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(model.Title);

                context.Brands.Attach(model);
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
            var item = context.Brands.Find(id);
            if (item != null)
            {
                context.Brands.Remove(item);
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
                        var _id = context.Brands.Find(Guid.Parse(item));
                        context.Brands.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }

        [HttpPost]
        public ActionResult IsActive(Guid id)
        {
            var item = context.Brands.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json(new { success = true, isActive = item.IsActive, message = "Đổi trạng thái thành công!" });
            }

            return Json(new { success = false, message = "Đổi trạng thái thất bại!" });
        }
    }
}