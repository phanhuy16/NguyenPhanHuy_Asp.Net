using NguyenPhanHuy_2122110062.Models;
using NguyenPhanHuy_2122110062.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;

namespace NguyenPhanHuy_2122110062.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.Visitors_online = HttpContext.Application["visitors_online"];
            return View();
        }

        public ActionResult PartialSendMessage()
        {
            return PartialView();
        }

        public ActionResult SendMessage(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Id = Guid.NewGuid();
                contact.Name = contact.Name;
                contact.Email = contact.Email;
                contact.Website = contact.Website;
                contact.Message = contact.Message;
                contact.CreatedDate = DateTime.Now;
                contact.ModifierDate = DateTime.Now;

                context.Contacts.Add(contact);
                context.SaveChanges();

                return Json(new { success = true, message = "Gủi tin nhắn thành công!" });
            }
            return View("PartialSendMessage");
        }
    }
}