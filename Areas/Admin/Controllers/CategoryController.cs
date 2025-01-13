
using NguyenPhanHuy_2122110062.Filter;
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
    [AdminAuthorization]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 3;

            if (page == null)
            {
                page = 1;
            }

            IEnumerable<Category> listCategory = context.Categories.OrderByDescending(x => x.CreatedDate).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                listCategory = listCategory.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listCategory = listCategory.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listCategory);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                category.CreatedDate = DateTime.Now;
                category.ModifierDate = DateTime.Now;
                category.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(category.Title);

                if (string.IsNullOrEmpty(category.SeoTitle))
                {
                    category.SeoTitle = category.Title;
                }

                if (string.IsNullOrEmpty(category.SeoDescription))
                {
                    category.SeoDescription = category.Description;
                }

                context.Categories.Add(category);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            var item = context.Categories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Attach(category);
                category.ModifierDate = DateTime.Now;
                category.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(category.Title);
                context.Entry(category).Property(x => x.Title).IsModified = true;
                context.Entry(category).Property(x => x.Description).IsModified = true;
                context.Entry(category).Property(x => x.Alias).IsModified = true;
                context.Entry(category).Property(x => x.Link).IsModified = true;
                context.Entry(category).Property(x => x.SeoDescription).IsModified = true;
                context.Entry(category).Property(x => x.SeoKeywords).IsModified = true;
                context.Entry(category).Property(x => x.SeoTitle).IsModified = true;
                context.Entry(category).Property(x => x.Position).IsModified = true;
                context.Entry(category).Property(x => x.IsActive).IsModified = true;
                context.Entry(category).Property(x => x.ModifierDate).IsModified = true;
                context.Entry(category).Property(x => x.ModifierBy).IsModified = true;

                context.SaveChanges();

                TempData["success"] = "Thay đổi thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thay đổi thất bại!";

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.Categories.Find(id);

            if (item != null)
            {
                var category = context.Categories.Attach(item);
                context.Categories.Remove(category);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }

        [HttpPost]
        public ActionResult IsActive(Guid id)
        {
            var item = context.Categories.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                context.Entry(item).Property(x => x.IsActive).IsModified = true;
                context.SaveChanges();

                return Json(new { success = true, isActive = item.IsActive, message = "Đổi trạng thái thành công!" });
            }

            return Json(new { success = false, message = "Đổi trạng thái thất bại!" });
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
                        var _id = context.Categories.Find(Guid.Parse(item));
                        context.Categories.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }
    }
}