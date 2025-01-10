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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(string search, int? page)
        {
            var pageSize = 5;

            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> listProduct = context.Products.OrderByDescending(x => x.CreatedDate);

            if (!string.IsNullOrEmpty(search))
            {
                listProduct = listProduct.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listProduct = listProduct.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listProduct);
        }

        public ActionResult Create()
        {
            ViewBag.ProductCategory = new SelectList(context.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(context.Brands.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<string> Images, List<int> Default)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ProductCategory = new SelectList(context.ProductCategories.ToList(), "Id", "Title");
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == Default[0])
                        {
                            product.ProductImage.Add(new ProductImage
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                Image = Images[i].ToString(),
                                IsDefault = true
                            });
                        }
                        else
                        {
                            product.ProductImage.Add(new ProductImage
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                Image = Images[i].ToString(),
                                IsDefault = false
                            });
                        }
                    }
                }
                product.Id = Guid.NewGuid();
                product.CreatedDate = DateTime.Now;
                product.ModifierDate = DateTime.Now;
                product.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(product.Title);

                if (string.IsNullOrEmpty(product.SeoTitle))
                {
                    product.SeoTitle = product.Title;
                }

                if (string.IsNullOrEmpty(product.SeoDescription))
                {
                    product.SeoDescription = product.Description;
                }

                context.Products.Add(product);
                context.SaveChanges();

                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm mới thất bại!";

            return View(product);
        }

        public ActionResult Edit(Guid id)
        {
            ViewBag.ProductCategory = new SelectList(context.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(context.Brands.ToList(), "Id", "Title");
            var item = context.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ModifierDate = DateTime.Now;
                product.Alias = NguyenPhanHuy_2122110062.Common.Filter.FilterChar(product.Title);

                context.Products.Attach(product);
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                TempData["success"] = "Thay đổi thành công!";

                return RedirectToAction("Index");
            }
            TempData["error"] = "Thay đổi thất bại!";

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var item = context.Products.Find(id);
            if (item != null)
            {
                context.Products.Remove(item);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }

        [HttpPost]
        public ActionResult IsActive(Guid id)
        {
            var item = context.Products.Find(id);
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
        public ActionResult IsHome(Guid id)
        {
            var item = context.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json(new { success = true, isHome = item.IsHome, message = "Đổi trạng thái thành công!" });
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
                        var _id = context.Products.Find(Guid.Parse(item));
                        context.Products.Remove(_id);
                        context.SaveChanges();
                    }
                }

                return Json(new { success = true, message = "Xoá tất cả thành công!" });
            }

            return Json(new { success = false, message = "Xoá tất cả thất bại!" });
        }
    }
}