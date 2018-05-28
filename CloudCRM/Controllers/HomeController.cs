using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudCRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CloudCRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<HomeController> _localizer;

        //Depedency injection
        public HomeController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr, IStringLocalizer<HomeController> localizer)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ApplicationUser profile)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            user.Email = profile.Email;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.PhoneNumber = profile.PhoneNumber;

            var updatedUser = await userManager.UpdateAsync(user);
            if (updatedUser.Succeeded)
            {
                ViewBag.Message = "saved";
            }
            return View(user);
        }

        public IActionResult PasswordChange()
        {

            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}