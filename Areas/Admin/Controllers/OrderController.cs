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
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var pageSize = 10;

            if (page == null)
                page = 1;

            IEnumerable<Order> listOrders = context.Orders.OrderByDescending(x => x.CreatedDate);

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            listOrders = listOrders.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(listOrders);
        }

        public ActionResult View(Guid id)
        {
            var order = context.Orders.Find(id);
            return View(order);
        }

        public ActionResult PartialProduct(Guid id)
        {
            var item = context.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdatePayment(Guid id, int type)
        {
            var order = context.Orders.Find(id);

            if (order != null)
            {
                context.Orders.Attach(order);
                order.TypePayment = type;
                context.Entry(order).Property(x => x.TypePayment).IsModified = true;
                context.SaveChanges();

                return Json(new { success = true, message = "Cập nhật thanh toán thành công!", id = order.Id });
            }
            return Json(new { success = false, message = "Cập nhật thanh toán thất bại!" });
        }
    }
}