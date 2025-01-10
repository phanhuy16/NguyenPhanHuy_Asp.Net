using NguyenPhanHuy_2122110062.Filter;
using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 5;

            if (page == null)
            {
                page = 1;
            }

            IEnumerable<News> listNews = context.News.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(search))
            {
                listNews = listNews.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listNews = listNews.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listNews);
        }

        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
                news.Id = Guid.NewGuid();
                news.CreatedDate = DateTime.Now;
                news.ModifierDate = DateTime.Now;
                news.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(news.Title);

                if (string.IsNullOrEmpty(news.SeoTitle))
                {
                    news.SeoTitle = news.Title;
                }

                if (string.IsNullOrEmpty(news.SeoDescription))
                {
                    news.SeoDescription = news.Description;
                }

                context.News.Add(news);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return View(news);
        }


        public ActionResult Edit(Guid id)
        {
            ViewBag.Category = new SelectList(context.Categories.ToList(), "Id", "Title");
            var item = context.News.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                news.ModifierDate = DateTime.Now;
                news.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(news.Title);

                context.News.Attach(news);
                context.Entry(news).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                TempData["success"] = "Thay đổi thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thay đổi thất bại!";

            return View(news);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.News.Find(id);
            if (item != null)
            {
                context.News.Remove(item);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }

        [HttpPost]
        public ActionResult IsActive(Guid id)
        {
            var item = context.News.Find(id);
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
                        var _id = context.News.Find(Guid.Parse(item));
                        context.News.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }
    }
}