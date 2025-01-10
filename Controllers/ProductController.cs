using NguyenPhanHuy_2122110062.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index(Guid? id, string type, decimal? minPrice, decimal? maxPrice)
        {
            var items = context.Products.AsQueryable();

            if (id != null)
            {
                if (type == "category")
                {
                    items = items.Where(x => x.ProductCategoryId == id);
                }
                else if (type == "brand")
                {
                    items = items.Where(x => x.BrandId == id);
                }
            }

            if (minPrice != null)
            {
                items = items.Where(x => x.PriceSale >= minPrice || x.Price >= minPrice);
            }

            if (maxPrice != null)
            {
                items = items.Where(x => x.PriceSale <= maxPrice || x.Price <= maxPrice);
            }

            var cate = context.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            var brand = context.Brands.Find(id);
            if (brand != null)
            {
                ViewBag.BrandName = brand.Title;
            }

            if (Request.IsAjaxRequest())
            {
                // Nếu là AJAX, chỉ render Partial View
                return PartialView("PartialProductList", items.OrderByDescending(x => x.CreatedDate).ToList());
            }

            return View(items.OrderByDescending(x => x.CreatedDate).ToList());
        }

        public ActionResult Detail(string alias, Guid id)
        {
            var items = context.Products.Find(id);
            if (items != null)
            {
                context.Products.Attach(items);
                items.ViewCount = items.ViewCount + 1;
                context.Entry(items).Property(x => x.ViewCount).IsModified = true;
                context.SaveChanges();
            }
            return View(items);
        }

        public ActionResult PartialItemsCategory(Guid CateId)
        {
            var items = context.Products.Where(x => x.ProductCategoryId == CateId && x.IsHome && x.IsActive).Take(8).ToList();
            return PartialView(items);
        }

        public ActionResult PartialProductItems()
        {
            var items = context.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult PartialProducSale()
        {
            var items = context.Products.Where(x => x.IsActive && x.IsSale).Take(5).ToList();
            return PartialView(items);
        }

        public ActionResult PartialProductPopular()
        {
            var items = context.Products.Where(x => x.IsHot).Take(3).ToList();
            return PartialView(items);
        }
    }
}