using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NguyenPhanHuy_2122110062.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        // GET: Admin/Account
        public ActionResult Index()
        {
            var items = context.Users.ToList();
            return View(items);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginAdminViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(context.Roles.ToList(), "Id", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Role != null)
                    {
                        foreach (var role in model.Role)
                        {
                            UserManager.AddToRole(user.Id, role);
                        }
                    }
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index");
                }
                ViewBag.Roles = new SelectList(context.Roles.ToList(), "Id", "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Edit(string id)
        {
            var item = UserManager.FindById(id);
            var user = new EditAccountViewModel();
            if (user != null)
            {
                var roleForUser = UserManager.GetRoles(id);
                var roles = new List<string>();
                if (roleForUser != null)
                {
                    foreach (var role in roleForUser)
                    {
                        roles.Add(role);
                    }
                }
                user.FullName = item.FullName;
                user.Email = item.Email;
                user.PhoneNumber = item.PhoneNumber;
                user.UserName = item.UserName;
                user.Role = roles;
            }
            ViewBag.Roles = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.UserName);
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roleForUser = UserManager.GetRoles(user.Id);
                    if (model.Role != null)
                    {
                        foreach (var role in model.Role)
                        {
                            var checkRole = roleForUser.FirstOrDefault(x => x.Equals(role));
                            if (checkRole == null)
                            {
                                UserManager.AddToRole(user.Id, role);
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                ViewBag.Roles = new SelectList(context.Roles.ToList(), "Name", "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAccount(string user, string id)
        {
            var code = new { success = false };
            var item = UserManager.FindByName(user);
            if (item != null)
            {
                var roleForUser = UserManager.GetRoles(id);
                if (roleForUser != null)
                {
                    foreach (var role in roleForUser)
                    {
                        await UserManager.RemoveFromRoleAsync(id, role);
                    }
                }
                var res = await UserManager.DeleteAsync(item);
                code = new { success = res.Succeeded };
            }

            return Json(code);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}