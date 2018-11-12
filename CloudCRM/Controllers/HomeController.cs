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
    /// <summary>
    /// This is the home controller and here are all actions needed for main layout
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// The user manager
        /// </summary>
        private UserManager<ApplicationUser> userManager;
        /// <summary>
        /// The sign in manager
        /// </summary>
        private SignInManager<ApplicationUser> signInManager;
        /// <summary>
        /// The localizer
        /// </summary>
        private readonly IStringLocalizer<HomeController> _localizer;
        /// <summary>
        /// The email sender
        /// </summary>
        private IEmailSender _emailSender;


        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userMgr">The user MGR.</param>
        /// <param name="signInMgr">The sign in MGR.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="emailSender">The email sender.</param>
        public HomeController(UserManager<ApplicationUser> userMgr,
                SignInManager<ApplicationUser> signInMgr, IStringLocalizer<HomeController> localizer, IEmailSender emailSender)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            _localizer = localizer;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Profiles this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            return View(user);
        }

        /// <summary>
        /// Profiles the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Passwords the change.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}