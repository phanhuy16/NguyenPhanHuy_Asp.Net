using NguyenPhanHuy_2122110062.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class DealsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Deals
        public ActionResult Index()
        {
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            var items = context.Products.Where(x => x.IsSale).OrderByDescending(x => x.CreatedDate).ToList();
            return View(items);
        }
    }
}