using System;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AgriEnergyConnect.Services;

public interface IApplicationService
{
    Task<bool> ChangeApplicationStatusAsync(int id, string status);
    Task<List<FarmerApplication>> GetAllApplicationsAsync();
}
