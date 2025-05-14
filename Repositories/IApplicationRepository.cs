using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IApplicationRepository
{
    Task<List<FarmerApplication>> GetAllAsync();
    Task<FarmerApplication?> GetByIdAsync(int id);
    Task UpdateStatusAsync(int id, string status);
    Task<bool> SubmitApplicationAsync(FarmerApplication application);
}