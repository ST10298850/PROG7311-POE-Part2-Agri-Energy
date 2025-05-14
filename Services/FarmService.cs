using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Services;

public class FarmService : IFarmService
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public FarmService(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<int> GetFarmIdForUserAsync(ClaimsPrincipal user)
    {
        var appUser = await _userManager.GetUserAsync(user);
        var farm = await _context.Farms.FirstOrDefaultAsync(f => f.UserID == appUser.Id);
        return farm?.FarmID ?? throw new Exception("Farm not found for user");
    }
}