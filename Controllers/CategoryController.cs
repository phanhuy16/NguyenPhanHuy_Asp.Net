using NguyenPhanHuy_2122110062.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Category
        public ActionResult Index()
        {
            var items = context.ProductCategories.OrderByDescending(x => x.CreatedDate).ToList();
            return View(items);
        }

        public ActionResult ParitalItemCategoryProduct(Guid id)
        {
            var items = context.Products.Where(x => x.Id == id).OrderByDescending(x => x.CreatedDate).ToList();
            return PartialView("ParitalItemCategoryProduct", items);
        }
    }
}