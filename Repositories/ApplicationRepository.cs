using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FarmerApplication>> GetAllAsync()
        {
            return await _context.FarmerApplications.ToListAsync();
        }

        public async Task<FarmerApplication?> GetByIdAsync(int id)
        {
            return await _context.FarmerApplications.FindAsync(id);
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var application = await _context.FarmerApplications.FindAsync(id);
            if (application != null)
            {
                application.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SubmitApplicationAsync(FarmerApplication application)
        {
            _context.FarmerApplications.Add(application);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}