using Globomatics.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globomatics.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            var result = await userManager.CreateAsync(
                new IdentityUser { UserName = model.Email, Email = model.Email}, model.Password);

            if (result.Succeeded)
            {
                //return View("RegistrationConfirmation");
                TempData["Registered"] = "You are registered";
                return RedirectToAction("Index", "Conference");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("error", error.Description);
            }
            return View(model);
        }
    }
}
