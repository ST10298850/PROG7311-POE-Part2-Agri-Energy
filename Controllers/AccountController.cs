using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        public AccountController(IUserService userService, IApplicationService applicationService)
        {
            _userService = userService;
            _applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, user) = await _userService.LoginAsync(model);
            if (success && user != null)
            {
                // Store session details
                HttpContext.Session.SetString("UserID", user.Id);
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Role", user.Role);

                return user.Role == "Employee"
                    ? RedirectToAction("Dashboard", "Employee")
                    : RedirectToAction("Dashboard", "Farmer");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Application()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Application(FarmerApplicationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _applicationService.SubmitApplicationAsync(model);

            if (success)
            {
                return RedirectToAction("ApplicationSuccess");
            }

            ModelState.AddModelError("", "Failed to submit application. Please try again.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ApplicationSuccess()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}