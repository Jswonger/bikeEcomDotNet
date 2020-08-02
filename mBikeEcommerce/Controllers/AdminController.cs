using mBikeEcommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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

        public ActionResult EditAccounts()
        {
            var users = UserManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            System.Diagnostics.Debug.WriteLine(id);
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