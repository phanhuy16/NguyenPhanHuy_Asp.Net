using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NguyenPhanHuy_2122110062.Filter;
using NguyenPhanHuy_2122110062.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/Role
        public ActionResult Index()
        {
            var item = context.Roles.ToList();
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                roleManager.Create(model);

                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction("Index");
            }

            TempData["erorr"] = "Thêm mới thất baij!";
            return View(model);
        }


        public ActionResult Edit(string id)
        {
            var item = context.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                roleManager.Update(model);

                TempData["success"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            TempData["erorr"] = "Cập nhật thất baij!";
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role = roleManager.FindById(id);
            if (role != null)
            {
                roleManager.Delete(role);

                return Json(new { success = true, message = "Xoá thành công!" });
            }

            return Json(new { success = false, message = "Xoá thất bại!" });
        }
    }
}