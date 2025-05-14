using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Services;
using System;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IProductService _productService;

        public EmployeeController(IApplicationService applicationService, IProductService productService)
        {
            _applicationService = applicationService;
            _productService = productService;
        }

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

        public async Task<ActionResult> ManageApplications()
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var success = await _applicationService.ChangeApplicationStatusAsync(id, status);
            if (!success)
                return Json(new { success = false, message = "Failed to update status" });

            var updatedApplication = await _applicationService.GetApplicationByIdAsync(id);
            return Json(new { success = true, message = "Status updated successfully", newStatus = updatedApplication.Status });
        }

        public async Task<IActionResult> ViewAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult ResetDashboard()
        {
            return RedirectToAction("Dashboard");
        }
    }
}