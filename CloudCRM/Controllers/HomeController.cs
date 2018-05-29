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
        private IEmailSender _emailSender;

        //Depedency injection
        public HomeController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr, IStringLocalizer<HomeController> localizer, IEmailSender emailSender)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _localizer = localizer;
            _emailSender = emailSender;
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
                ViewBag.Message = "ChangesSaved";
            }
            return View(user);
        }

        public async Task<IActionResult> PasswordChange()
        {
            //Password Generator
            var passwordLength = 12;
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            //Changing user password
            string password = new string(chars);
            var user = await userManager.GetUserAsync(HttpContext.User);
            var deletepass = await userManager.RemovePasswordAsync(user);
            var createpass = await userManager.AddPasswordAsync(user, password);

            var message = _localizer["yourNewPassword"]+password;
            //Sending mail
            var mailsucceed = _emailSender.SendMail(user.Email, _localizer["passwordChanged"], message);
            //Logout the user
            await signInManager.SignOutAsync();
            
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}