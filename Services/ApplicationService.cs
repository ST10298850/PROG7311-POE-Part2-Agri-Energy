using System;
using System.Collections.Generic;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Repositories;
using AgriEnergyConnect.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Services
{
    /// <summary>
    /// Service class for managing farmer applications.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ApplicationService class.
        /// </summary>
        /// <param name="repository">The application repository.</param>
        /// <param name="userManager">The user manager for identity operations.</param>
        /// <param name="context">The database context.</param>
        public ApplicationService(
            IApplicationRepository repository,
            UserManager<User> userManager,
            AppDbContext context)
        {
            _repository = repository;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Changes the status of a farmer application and performs necessary actions if approved.
        /// </summary>
        /// <param name="id">The ID of the application to update.</param>
        /// <param name="status">The new status to set.</param>
        /// <returns>True if the status was successfully changed, false otherwise.</returns>
        public async Task<bool> ChangeApplicationStatusAsync(int id, string status)
        {
            var app = await _repository.GetByIdAsync(id);
            if (app == null || app.Status != "Pending")
                return false;

            // Update application status
            await _repository.UpdateStatusAsync(id, status);

            if (status == "Approved")
            {
                // Step 1: Create the user
                var user = new User
                {
                    UserName = app.Email,
                    Email = app.Email,
                    Role = "Farmer"
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!");
                if (!result.Succeeded) return false;

                // Step 2: Assign to Farmer role
                await _userManager.AddToRoleAsync(user, "Farmer");

                // Step 3: Create UserDetail
                var userDetail = new UserDetail
                {
                    UserID = user.Id,
                    FullName = app.FullName,
                    Phone = app.Phone,
                    Address = app.Location
                };

                _context.UserDetails.Add(userDetail);

                // Step 4: Create Farm
                var farm = new Farm
                {
                    UserID = user.Id,
                    FarmName = app.FarmName,
                    Location = app.Location,
                    FarmType = app.FarmType
                };

                _context.Farms.Add(farm);

                // Final save
                await _context.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Retrieves all farmer applications.
        /// </summary>
        /// <returns>A list of all farmer applications.</returns>
        public async Task<List<FarmerApplication>> GetAllApplicationsAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a specific farmer application by its ID.
        /// </summary>
        /// <param name="id">The ID of the application to retrieve.</param>
        /// <returns>The farmer application if found, null otherwise.</returns>
        public async Task<FarmerApplication> GetApplicationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Submits a new farmer application.
        /// </summary>
        /// <param name="model">The view model containing the application details.</param>
        /// <returns>True if the application was successfully submitted, false otherwise.</returns>
        public async Task<bool> SubmitApplicationAsync(FarmerApplicationViewModel model)
        {
            // Create a new FarmerApplication from the view model
            var application = new FarmerApplication
            {
                FarmName = model.FarmName,
                Location = model.Location,
                FarmType = model.FarmType,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Status = "Pending",
                SubmissionDate = DateTime.Now
            };

            // Use the repository to submit the application
            return await _repository.SubmitApplicationAsync(application);
        }

    }
}