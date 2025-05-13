using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AgriEnergyConnect.Data;

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
        /// Displays the login page for user authentication.
        /// </summary>
        /// <returns>
        /// Returns an IActionResult that renders the login view.
        /// </returns>
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Application(FarmerApplicationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // var userId = HttpContext.Session.GetString("UserID");

            var application = new FarmerApplication
            {
                FarmName = model.FarmName,
                Location = model.Location,
                FarmType = model.FarmType,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                SubmissionDate = DateTime.Now,
                Status = "Pending",

            };

            _context.FarmerApplications.Add(application);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account"); // Redirect to login page
        }

        // GET: /Account/ApplicationSuccess
        public IActionResult ApplicationSuccess()
        {
            return View();
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();         // ASP.NET Identity logout
            HttpContext.Session.Clear();                 // Clear session variables
            return RedirectToAction("Login", "Account"); // Redirect to login page
        }
    }
}