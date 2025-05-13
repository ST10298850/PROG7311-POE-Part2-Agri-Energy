using System.Linq;
using AgriEnergyConnect.Data;      // for AppDbContext
using AgriEnergyConnect.Models;   // for User
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}