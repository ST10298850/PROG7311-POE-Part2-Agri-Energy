using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Services;
using System;
using System.Threading.Tasks;
using AgriEnergyConnect.ViewModels;
using Microsoft.Extensions.Logging;

namespace AgriEnergyConnect.Controllers
{
    /// <summary>
    /// Controller responsible for handling employee-related actions and views.
    /// </summary>
    public class EmployeeController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IApplicationService applicationService,
            IProductService productService,
            IUserService userService,
            ILogger<EmployeeController> logger)
        {
            _applicationService = applicationService;
            _productService = productService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Displays the employee dashboard with filtered products.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Dashboard(string? userId, string? category, DateTime? startDate, DateTime? endDate, bool reset = false)
        {
            if (reset)
            {
                userId = null;
                category = null;
                startDate = null;
                endDate = null;
            }

            var viewModel = await _productService.GetFilteredProductsAsync(userId, category, startDate, endDate);
            return View(viewModel);
        }

        /// <summary>
        /// Displays the page for managing farmer applications.
        /// </summary>
        public async Task<ActionResult> ManageApplications()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return View(applications);
        }

        /// <summary>
        /// Updates the status of a farmer application.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var success = await _applicationService.ChangeApplicationStatusAsync(id, status);
            if (!success)
                return Json(new { success = false, message = "Failed to update status" });

            var updatedApplication = await _applicationService.GetApplicationByIdAsync(id);
            return Json(new { success = true, message = "Status updated successfully", newStatus = updatedApplication.Status });
        }

        /// <summary>
        /// Displays a list of all products.
        /// </summary>
        public async Task<IActionResult> ViewAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        /// <summary>
        /// Resets the dashboard filters and redirects to the Dashboard action.
        /// </summary>
        [HttpGet]
        public IActionResult ResetDashboard()
        {
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            TempData.Remove("ErrorMessage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User created successfully.";
                    _logger.LogInformation($"User created successfully: {model.Email}");
                    return RedirectToAction(nameof(ManageApplications));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogWarning($"Error creating user: {error.Description}");
                }
            }
            else
            {
                _logger.LogWarning($"Invalid model state: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
            }

            // If we got this far, something failed; redisplay form
            TempData["ErrorMessage"] = "Failed to create user. Please check the form and try again.";
            return View(model);
        }
    }
}