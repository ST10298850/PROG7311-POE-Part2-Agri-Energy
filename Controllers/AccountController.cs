using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserService userService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userService.ValidateCredentialsAsync(model.Email!, model.Password!))
            {
                var user = await _userService.GetUserByEmailAsync(model.Email!);

                if (user != null)
                {
                    // Store session
                    HttpContext.Session.SetString("UserID", user.Id);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Role", user.Role);

                    return user.Role == "Employee"
                        ? RedirectToAction("Dashboard", "Employee")
                        : RedirectToAction("Dashboard", "Farmer");
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        // GET: /Account/Logout
        // public async Task<IActionResult> Logout()
        // {
        //     await _signInManager.SignOutAsync();
        //     HttpContext.Session.Clear();
        //     return RedirectToAction("Login");
        // }

        // GET: /Account/Application
        public IActionResult Application()
        {
            return View();
        }

        // POST: /Account/Application
        // [HttpPost]
        // public async Task<IActionResult> Application(FarmerApplicationViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);

        //     // Process the farmer application
        //     var result = await _userService.ProcessFarmerApplicationAsync(model);

        //     if (result.Succeeded)
        //     {
        //         // Redirect to a success page or login page
        //         return RedirectToAction("ApplicationSuccess");
        //     }

        //     // If there are errors, add them to ModelState
        //     foreach (var error in result.Errors)
        //     {
        //         ModelState.AddModelError("", error);
        //     }

        //     return View(model);
        // }

        // // GET: /Account/ApplicationSuccess
        // public IActionResult ApplicationSuccess()
        // {
        //     return View();
        // }
    }
}