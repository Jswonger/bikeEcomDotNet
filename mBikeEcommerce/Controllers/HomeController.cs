using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mBikeEcommerce.DAL;
using mBikeEcommerce.Models;

namespace mBikeEcommerce.Controllers
{
    public class HomeController : Controller
    {

        private ContactContext db = new ContactContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if(!ModelState.IsValid)
            {
                return View(contact);
            }

            var newContact = new Contact();

            newContact.name = contact.name;
            newContact.email = contact.email;
            newContact.description = contact.description;

            db.Contacts.Add(newContact);
            db.SaveChanges();

            return View();
        }
    }
}