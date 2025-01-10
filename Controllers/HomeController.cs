using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            var item = context.ProductCategories.OrderBy(x => x.Id).ToList();
            return View(item);
        }

        public ActionResult PartialSubscribe()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                context.Subscribes.Add(new Subscribe
                {
                    Id = Guid.NewGuid(),
                    Email = req.Email,
                    CreatedDate = DateTime.Now,
                });
                context.SaveChanges();
                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            return View("PartialSubscribe");
        }

        public ActionResult Refresh()
        {
            var item = new StatisticalModel();

            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];

            item.HomNay = HttpContext.Application["HomNay"].ToString();
            item.HomQua = HttpContext.Application["HomQua"].ToString();
            item.TuanNay = HttpContext.Application["TuanNay"].ToString();
            item.TuanTruoc = HttpContext.Application["TuanTruoc"].ToString();
            item.ThangNay = HttpContext.Application["ThangNay"].ToString();
            item.ThangTruoc = HttpContext.Application["ThangTruoc"].ToString();
            item.TatCa = HttpContext.Application["TatCa"].ToString();

            return PartialView(item);
        }
    }
}