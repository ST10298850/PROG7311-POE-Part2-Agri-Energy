using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Data;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Services;

namespace AgriEnergyConnect.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IApplicationService _applicationService;

        public EmployeeController(AppDbContext context, IApplicationService applicationService)
        {
            _context = context;
            _applicationService = applicationService;
        }

        // GET: EmployeeController
        public ActionResult Dashboard()
        {
            return View();
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
    }
}