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
            var userm = await userManager.GetUserAsync(HttpContext.User);
            var user = new ApplicationUser();
            user = userm;
            return View(user);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}