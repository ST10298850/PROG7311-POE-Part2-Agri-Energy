using System.Threading.Tasks;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<(bool success, User? user)> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> CreateUserAsync(CreateUserViewModel model);
    }
}