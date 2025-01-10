using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
