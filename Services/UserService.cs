using System.Threading.Tasks;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Data;


namespace AgriEnergyConnect.Services
{
    /// <summary>
    /// Service class for managing user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppDbContext context,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Validates user credentials.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>True if the credentials are valid, false otherwise.</returns>
        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Succeeded;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user if found, null otherwise.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Attempts to log in a user with the provided credentials.
        /// </summary>
        /// <param name="model">The login view model containing the user's credentials.</param>
        /// <returns>A tuple indicating whether the login was successful and the user object if successful.</returns>
        public async Task<(bool success, User? user)> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return (false, null);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            return (result.Succeeded, result.Succeeded ? user : null);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserViewModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Role = "Farmer" // Explicitly set the Role to "Farmer"
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                try
                {
                    // Add the user to the "Farmer" role
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    var userDetail = new UserDetail
                    {
                        UserID = user.Id,
                        FullName = model.FullName,
                        Phone = model.Phone,
                        Address = model.Location // Assuming Location is the address
                    };
                    _context.UserDetails.Add(userDetail);

                    var farm = new Farm
                    {
                        UserID = user.Id,
                        FarmName = model.FarmName,
                        Location = model.Location,
                        FarmType = model.FarmType
                    };
                    _context.Farms.Add(farm);

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"User created successfully: {user.Email}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error creating user: {ex.Message}");
                    // If an error occurs after user creation, delete the user
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError { Description = "Error occurred while saving user details." });
                }
            }
            else
            {
                _logger.LogWarning($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }
    }
}