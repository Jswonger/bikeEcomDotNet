using mBikeEcommerce.DAL;
using mBikeEcommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mBikeEcommerce.Controllers
{
    [Authorize( Roles = "admin" )]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private ProductContext dbp = new ProductContext();
        private ContactContext dbc = new ContactContext();

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product product, HttpPostedFileBase imgFile)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            string filename = Path.GetFileName(imgFile.FileName);

            var newImgPath = "~/Content/images/" + filename;

            var newProduct = new Product { 
                brand = product.brand, type = product.type, 
                name = product.name, imgPath = newImgPath, price = product.price, 
                description = product.description 
            };

            filename = Path.Combine(Server.MapPath("~/Content/images/" + filename));
            imgFile.SaveAs(filename);

            dbp.Products.Add(newProduct);
            dbp.SaveChanges();

            return RedirectToAction("ProductList");
        }

        public ActionResult ProductList()
        {
            return View(dbp.Products.ToList());
        }

        [HttpGet]
        public async Task<ActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ProductList");
            }
            var product = await dbp.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> PostDeleteProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("DeleteProduct");
            }
            var product = await dbp.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            dbp.Products.Remove(product);
            dbp.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public ActionResult ContactList()
        {
            return View(dbc.Contacts.ToList());
        }

        [HttpGet]
        public async Task<ActionResult> DeleteContact(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("ContactList");
            }
            var contact = await dbc.Contacts.FindAsync(id);
            if(contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public async Task<ActionResult> PostDeleteContact(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ContactList");
            }
            var contact = await dbc.Contacts.FindAsync(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            dbc.Contacts.Remove(contact);
            dbc.SaveChanges();
            return RedirectToAction("ContactList");
        }

        public ActionResult EditAccounts()
        {
            var users = UserManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            System.Diagnostics.Debug.WriteLine(user);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("EditAccounts");
                else
                    return RedirectToAction("Delete");
            }else
            {
                ModelState.AddModelError("","User Not Found");
            }
            System.Diagnostics.Debug.WriteLine("Getting here");
            return RedirectToAction("Delete");
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.userid = id;

            var user = await UserManager.FindByIdAsync(id);
            var userRoles = await UserManager.GetRolesAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            var roles = RoleManager.Roles;

            foreach (var mrole in roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = mrole.Id,
                    RoleName = mrole.Name
                };

                if (userRoles.Contains(mrole.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit( List<UserRolesViewModel> model, string userId )
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("EditAccounts");
            }

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return RedirectToAction("EditAccounts");
            }

            var roles = await UserManager.GetRolesAsync(user.Id.ToString());

            var result = await UserManager.RemoveFromRolesAsync(user.Id.ToString(), roles.ToArray());

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await UserManager.AddToRolesAsync(user.Id.ToString(),
                model.Where(x => x.IsSelected).Select(y => y.RoleName).ToArray());

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditAccounts");
        }
    }

    
}