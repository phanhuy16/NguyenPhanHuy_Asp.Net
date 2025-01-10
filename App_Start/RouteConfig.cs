
using System.Web.Mvc;
using System.Web.Routing;

namespace NguyenPhanHuy_2122110062
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Product",
               url: "san-pham",
               defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
             );
            routes.MapRoute(
               name: "News",
               url: "tin-tuc",
               defaults: new { controller = "News", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
             );
            routes.MapRoute(
              name: "Contact",
              url: "lien-he",
              defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
            );
            routes.MapRoute(
               name: "Article",
               url: "post/{alias}",
               defaults: new { controller = "Article", action = "Index", alias = UrlParameter.Optional },
               namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
             );
            routes.MapRoute(
              name: "Home",
              url: "trang-chu",
              defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
              namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "NguyenPhanHuy_2122110062.Controllers" }
            );
        }
    }
}
