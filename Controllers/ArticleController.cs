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
            var items = context.Posts.FirstOrDefault(x=>x.Alias == alias);
            return View(items);
        }
    }
}