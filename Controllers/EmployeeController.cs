using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Data;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Services;
using Microsoft.AspNetCore.Identity;
using AppUser = AgriEnergyConnect.Models.User;

namespace AgriEnergyConnect.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IApplicationService _applicationService;

        private readonly IProductService _productService;

        private readonly UserManager<AppUser> _userManager;

        public EmployeeController(AppDbContext context, IApplicationService applicationService, IProductService productService, UserManager<AppUser> userManager)
        {
            _context = context;
            _applicationService = applicationService;
            _productService = productService;
            _userManager = userManager;
        }

        // GET: EmployeeController
        [HttpGet]
        public async Task<IActionResult> Dashboard(string? userId, string? category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products
                .Include(p => p.Farm)
                .Include(p => p.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(p => p.UserId == userId);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (startDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.ProductionDate <= endDate.Value);

            var products = await query.ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                Products = products,
                Categories = await _context.Products.Select(p => p.Category).Distinct().ToListAsync(),
                Farmers = (await _userManager.GetUsersInRoleAsync("Farmer")).ToList(),
                UserId = userId,
                Category = category,
                StartDate = startDate,
                EndDate = endDate
            };

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

            var updatedApplication = await _context.FarmerApplications.FindAsync(id);
            return Json(new { success = true, message = "Status updated successfully", newStatus = updatedApplication.Status });
        }

        public async Task<IActionResult> ViewAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }
    }
}