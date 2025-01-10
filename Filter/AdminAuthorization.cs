
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Filter
{
    public class AdminAuthorization : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // Trang đăng nhập của Admin
            var loginPath = "~/Admin/Account/Login";

            // Kiểm tra người dùng đã đăng nhập chưa
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(loginPath);
                return;
            }

            // Kiểm tra người dùng có vai trò Admin không
            if (!filterContext.HttpContext.User.IsInRole("Admin") && !filterContext.HttpContext.User.IsInRole("Employee"))
            {
                filterContext.Result = new RedirectResult(loginPath);
                return;
            }
        }
    }
}