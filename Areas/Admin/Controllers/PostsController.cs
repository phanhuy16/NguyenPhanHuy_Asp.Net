using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System.Linq;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using PagedList;
using NguyenPhanHuy_2122110062.Filter;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/Posts
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 5;

            if (page == null)
            {
                page = 1;
            }

            IEnumerable<Posts> listPosts = context.Posts.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(search))
            {
                listPosts = listPosts.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listPosts = listPosts.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listPosts);
        }

        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posts post)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
                post.Id = Guid.NewGuid();
                post.CreatedDate = DateTime.Now;
                post.ModifierDate = DateTime.Now;
                post.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(post.Title);

                if (string.IsNullOrEmpty(post.SeoTitle))
                {
                    post.SeoTitle = post.Title;
                }

                if (string.IsNullOrEmpty(post.SeoDescription))
                {
                    post.SeoDescription = post.Description;
                }

                context.Posts.Add(post);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return View(post);
        }


        public ActionResult Edit(Guid id)
        {
            ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
            var item = context.Posts.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posts post)
        {
            if (ModelState.IsValid)
            {
                post.ModifierDate = DateTime.Now;
                post.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(post.Title);

                context.Posts.Attach(post);
                context.Entry(post).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                TempData["success"] = "Thay đổi thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thay đổi thất bại!";

            return View(post);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.Posts.Find(id);
            if (item != null)
            {
                context.Posts.Remove(item);
                context.SaveChanges();

                TempData["success"] = "Xoá thành công!";

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }

        [HttpPost]
        public ActionResult IsActive(Guid id)
        {
            var item = context.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
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
                        var _id = context.Posts.Find(Guid.Parse(item));
                        context.Posts.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }
    }
}