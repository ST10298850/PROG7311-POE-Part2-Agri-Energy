using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Repositories
{
    /// <summary>
    /// Repository class for managing farmer applications in the database.
    /// </summary>
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ApplicationRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all farmer applications from the database.
        /// </summary>
        /// <returns>A list of all farmer applications.</returns>
        public async Task<List<FarmerApplication>> GetAllAsync()
        {
            return await _context.FarmerApplications.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific farmer application by its ID.
        /// </summary>
        /// <param name="id">The ID of the farmer application to retrieve.</param>
        /// <returns>The farmer application if found, null otherwise.</returns>
        public async Task<FarmerApplication?> GetByIdAsync(int id)
        {
            return await _context.FarmerApplications.FindAsync(id);
        }

        /// <summary>
        /// Updates the status of a farmer application.
        /// </summary>
        /// <param name="id">The ID of the farmer application to update.</param>
        /// <param name="status">The new status to set.</param>
        public async Task UpdateStatusAsync(int id, string status)
        {
            var application = await _context.FarmerApplications.FindAsync(id);
            if (application != null)
            {
                application.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Submits a new farmer application to the database.
        /// </summary>
        /// <param name="application">The farmer application to submit.</param>
        /// <returns>True if the submission was successful, false otherwise.</returns>
        public async Task<bool> SubmitApplicationAsync(FarmerApplication application)
        {
            _context.FarmerApplications.Add(application);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}