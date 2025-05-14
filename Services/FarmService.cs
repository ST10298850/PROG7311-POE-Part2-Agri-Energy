using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Services;

/// <summary>
/// Service class for managing farm-related operations.
/// </summary>
public class FarmService : IFarmService
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the FarmService class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="userManager">The user manager for identity operations.</param>
    public FarmService(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Retrieves the farm ID associated with the given user.
    /// </summary>
    /// <param name="user">The ClaimsPrincipal representing the current user.</param>
    /// <returns>The farm ID if found; otherwise, throws an exception.</returns>
    /// <exception cref="Exception">Thrown when no farm is found for the user.</exception>
    public async Task<int> GetFarmIdForUserAsync(ClaimsPrincipal user)
    {
        var appUser = await _userManager.GetUserAsync(user);
        var farm = await _context.Farms.FirstOrDefaultAsync(f => f.UserID == appUser.Id);
        return farm?.FarmID ?? throw new Exception("Farm not found for user");
    }
}