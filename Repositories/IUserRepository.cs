using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Repositories
{
    public interface IUserRepository
    {
        User? GetUserByEmail(string email);
    }
}