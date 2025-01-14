using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using NguyenPhanHuy_2122110062.Models;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Article
        public ActionResult Index(string alias)
        {
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            var items = context.Posts.FirstOrDefault(x=>x.Alias == alias);
            return View(items);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}