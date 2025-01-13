using NguyenPhanHuy_2122110062.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            return View();
        }

        public ActionResult MenuTop()
        {
            var items = context.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }

        public ActionResult MenuProductCategory()
        {
            var items = context.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }

        public ActionResult ProductCategory()
        {
            var items = context.ProductCategories.Where(x => x.IsHome).ToList();
            return PartialView("_ProductCategory", items);
        }

        public ActionResult MenuCategoryLeft()
        {
            var items = context.ProductCategories.OrderByDescending(x=>x.CreatedDate).ToList();
            return PartialView("_MenuCategoryLeft", items);
        }

        public ActionResult MenuBrandLeft()
        {
            var items = context.Brands.OrderByDescending(x=>x.CreatedDate).ToList();
            return PartialView("_MenuBrandLeft", items);
        }
    }
}