using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NguyenPhanHuy_2122110062.Models;
using System.Web;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;

        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(string tab = "Account")
        {
            ViewBag.Active = tab;

            return View();
        }

        public ActionResult PartialAccount()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            return PartialView("PartialAccount", user);
        }

        public ActionResult PartialAddress()
        {
            return PartialView("PartialAddress");
        }

        public ActionResult PartialOrder()
        {
            return PartialView("PartialOrder");
        }

        public ActionResult PartialWishlist()
        {
            return PartialView("PartialWishlist");
        }

        public ActionResult PartialSelling()
        {
            return PartialView("PartialSelling");
        }

        public ActionResult PartialSetting()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            var item = new EditViewModel();
            item.Id = userId;
            item.FullName = user.FullName;
            item.Email = user.Email;
            item.UserName = user.UserName;
            item.PhoneNumber = user.PhoneNumber;
            item.City = user.City;
            item.District = user.District;
            item.Ward = user.Ward;

            return PartialView("PartialSetting", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            var user = UserManager.FindById(model.Id);

            if (user == null)
            {
                return Json(new { success = false, message = "Người dùng không tồn tại." });
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.District = model.District;
            user.Ward = model.Ward;

            var result = UserManager.Update(user);

            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }

            return Json(new { success = false, message = "Cập nhật thất bại!" });
        }
    }
}