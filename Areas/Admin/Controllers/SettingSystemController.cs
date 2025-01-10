using NguyenPhanHuy_2122110062.Filter;
using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System.Linq;
using System.Web.Mvc;

namespace NguyenPhanHuy_2122110062.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class SettingSystemController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialSetting()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            // Title
            var checkTitle = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if (checkTitle == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
                context.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }

            // Logo
            var checkLogo = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                context.Entry(checkLogo).State = System.Data.Entity.EntityState.Modified;
            }

            // Email
            var checkEmail = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (checkEmail == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkEmail.SettingValue = req.SettingEmail;
                context.Entry(checkEmail).State = System.Data.Entity.EntityState.Modified;
            }

            // Hotline
            var checkHotline = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingHotline"));
            if (checkHotline == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkHotline.SettingValue = req.SettingHotline;
                context.Entry(checkHotline).State = System.Data.Entity.EntityState.Modified;
            }

            // TitleSeo
            var checkTitleSeo = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (checkTitleSeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkTitleSeo.SettingValue = req.SettingTitleSeo;
                context.Entry(checkTitleSeo).State = System.Data.Entity.EntityState.Modified;
            }

            // DesSeo
            var checkDesSeo = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (checkDesSeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkDesSeo.SettingValue = req.SettingDesSeo;
                context.Entry(checkDesSeo).State = System.Data.Entity.EntityState.Modified;
            }

            // KeySeo
            var checkKeySeo = context.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (checkKeySeo == null)
            {
                SystemSetting set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                context.SystemSettings.Add(set);
            }
            else
            {
                checkKeySeo.SettingValue = req.SettingKeySeo;
                context.Entry(checkKeySeo).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();

            return Json(new { success = true, message = "Cập nhật cấu hình thành công!" });
            //return RedirectToAction("Index");

        }
    }
}