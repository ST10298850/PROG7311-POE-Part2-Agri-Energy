using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IApplicationService
{
    Task<List<FarmerApplication>> GetAllApplicationsAsync();
    Task<bool> ChangeApplicationStatusAsync(int id, string status);
    Task<FarmerApplication> GetApplicationByIdAsync(int id);
    Task<bool> SubmitApplicationAsync(FarmerApplicationViewModel model);
}