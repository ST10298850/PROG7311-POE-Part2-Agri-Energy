using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(IUserService userService, SignInManager<User> signInManager, AppDbContext context)
        {
            _userService = userService;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Processes login credentials and redirects based on user role.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userService.ValidateCredentialsAsync(model.Email!, model.Password!))
            {
                var user = await _userService.GetUserByEmailAsync(model.Email!);
                if (user != null)
                {
                    // Store session details
                    HttpContext.Session.SetString("UserID", user.Id);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Role", user.Role);

                    return user.Role == "Employee"
                        ? RedirectToAction("Dashboard", "Employee")
                        : RedirectToAction("Dashboard", "Farmer");
                }
            }

            ViewData["ErrorMessage"] = "Invalid login credentials";
            return View(model);
        }

        /// <summary>
        /// Logs the user out and clears the session.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Displays the farmer application form.
        /// </summary>
        [HttpGet]
        public IActionResult Application()
        {
            return View();
        }

        /// <summary>
        /// Submits a new farmer application.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Application(FarmerApplicationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var application = new FarmerApplication
            {
                FarmName = model.FarmName,
                Location = model.Location,
                FarmType = model.FarmType,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                SubmissionDate = DateTime.Now,
                Status = "Pending"
            };

            _context.FarmerApplications.Add(application);
            await _context.SaveChangesAsync();

            return RedirectToAction("ApplicationSuccess", "Account");
        }

        /// <summary>
        /// Displays the application success confirmation.
        /// </summary>
        [HttpGet]
        public IActionResult ApplicationSuccess()
        {
            return View();
        }
    }
}