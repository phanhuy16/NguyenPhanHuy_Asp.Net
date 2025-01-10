using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: News
        public ActionResult Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = context.News.OrderByDescending(x => x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult PartialNewsItem()
        {
            var items = context.News.Take(4).OrderByDescending(x => x.CreatedDate).ToList();
            return PartialView(items);
        }

        public ActionResult Detail(Guid id)
        {
            var items = context.News.Find(id);
            return View(items);
        }
    }
}