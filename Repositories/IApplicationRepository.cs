using System;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace AgriEnergyConnect.Repositories;

public interface IApplicationRepository
{
    Task<List<FarmerApplication>> GetAllAsync();
    Task<FarmerApplication?> GetByIdAsync(int id);
    Task UpdateStatusAsync(int id, string status);
}
