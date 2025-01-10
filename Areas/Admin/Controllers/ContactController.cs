using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Contact
        public ActionResult Index(int? page)
        {
            var pageSize = 5;

            if (page == null)
            {
                page = 1;
            }

            IEnumerable<Contact> items = context.Contacts.OrderByDescending(x => x.CreatedDate).ToList();

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            return View(items);
        }

        public ActionResult Delete(Guid id)
        {
            var item = context.Contacts.Find(id);

            if (item != null)
            {
                context.Contacts.Remove(item);
                context.SaveChanges();

                return Json(new { success = true, message = "Xoá thành công!" });
            }
            return Json(new { success = false, message = "Xoá thất bại!" });
        }
    }
}