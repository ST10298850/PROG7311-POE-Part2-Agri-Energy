using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductRepository
{
    Task<List<Product>> GetProductsByFarmIdAsync(int farmId);
    Task AddProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
}