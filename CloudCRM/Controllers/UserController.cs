using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudCRM.Models;
using CloudCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CloudCRM.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<UserController> _localizer;
        
        //Depedency injection
        public UserController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr, IStringLocalizer<UserController> localizer)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public ViewResult Login()
        {
                return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(loginModel.UserName);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                            loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username or password");

            return View(loginModel);

        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Collaborators()
        {
            //searching and returning users with the same role and collaborator id of the user who call this action
            var user = await userManager.GetUserAsync(HttpContext.User);
            var role = await userManager.GetRolesAsync(user);
            var usersWithSameRole = await userManager.GetUsersInRoleAsync(string.Join(",", role.ToArray()));
            List<ApplicationUser> collaborators = usersWithSameRole.Where(c => c.Collaborator.Id == user.Collaborator.Id).ToList();
            collaborators.Remove(user);
            
            return View(collaborators);
        }
    }
}