using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeController
        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> ManageApplications()
        {
            var applications = await _context.FarmerApplications.ToListAsync();
            return View(applications);
        }
    }
}