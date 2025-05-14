using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AgriEnergyConnect.Services;

public interface IFarmService
{
    Task<int> GetFarmIdForUserAsync(ClaimsPrincipal user);
}