using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index" ,id = UrlParameter.Optional },
                namespaces: new[] { "NguyenPhanHuy_2122110062.Areas.Admin.Controllers" }
            );
        }
    }
}