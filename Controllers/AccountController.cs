using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.ViewModels;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    /// <summary>
    /// Controller responsible for handling user account-related actions.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        /// <summary>
        /// Initializes a new instance of the AccountController.
        /// </summary>
        /// <param name="userService">The user service for handling user-related operations.</param>
        /// <param name="applicationService">The application service for handling farmer applications.</param>
        public AccountController(IUserService userService, IApplicationService applicationService)
        {
            _userService = userService;
            _applicationService = applicationService;
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles the user login process.
        /// </summary>
        /// <param name="model">The login view model containing user credentials.</param>
        /// <returns>
        /// If login is successful, redirects to the appropriate dashboard based on user role.
        /// If login fails, returns the login view with an error message.
        /// </returns>
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

        /// <summary>
        /// Logs out the current user and clears the session.
        /// </summary>
        /// <returns>Redirects to the login page.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Displays the farmer application form.
        /// </summary>
        /// <returns>The application view.</returns>
        [HttpGet]
        public IActionResult Application()
        {
            return View();
        }

        /// <summary>
        /// Handles the submission of a farmer application.
        /// </summary>
        /// <param name="model">The farmer application view model containing application details.</param>
        /// <returns>
        /// If submission is successful, redirects to the application success page.
        /// If submission fails, returns the application view with an error message.
        /// </returns>
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

        /// <summary>
        /// Displays a success message after a successful application submission and redirects to the login page.
        /// </summary>
        /// <returns>Redirects to the login page.</returns>
        [HttpGet]
        public IActionResult ApplicationSuccess()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}