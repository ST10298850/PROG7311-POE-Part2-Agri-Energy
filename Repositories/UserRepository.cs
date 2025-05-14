using System.Linq;
using AgriEnergyConnect.Data;      // for AppDbContext
using AgriEnergyConnect.Models;   // for User
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Repositories
{
    /// <summary>
    /// Repository class for managing user-related database operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user from the database by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}