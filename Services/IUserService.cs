using System.Threading.Tasks;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);


    }
}