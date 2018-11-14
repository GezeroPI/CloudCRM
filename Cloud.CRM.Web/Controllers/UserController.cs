using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloud.CRM.Web.Models;
using Cloud.CRM.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Cloud.CRM.Web.Controllers
{
    /// <summary>
    /// This is the user controller with all action needed
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// The user manager
        /// </summary>
        private UserManager<ApplicationUser> userManager;
        /// <summary>
        /// The role manager
        /// </summary>
        private RoleManager<ApplicationRole> roleManager;
        /// <summary>
        /// The sign in manager
        /// </summary>
        private SignInManager<ApplicationUser> signInManager;
        /// <summary>
        /// The localizer
        /// </summary>
        private readonly IStringLocalizer<UserController> _localizer;

        //Depedency injection
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userMgr">The user MGR.</param>
        /// <param name="signInMgr">The sign in MGR.</param>
        /// <param name="localizer">The localizer.</param>
        public UserController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr, IStringLocalizer<UserController> localizer)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _localizer = localizer;
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ViewResult Login()
        {
                return View("Login", new LoginViewModel());
        }

        /// <summary>
        /// Logins the specified login model.
        /// </summary>
        /// <param name="loginModel">The login model.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Logouts the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Collaboratorses this instance.
        /// </summary>
        /// <returns></returns>
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