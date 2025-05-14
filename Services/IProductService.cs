using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync(int farmId);
    Task AddProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
}