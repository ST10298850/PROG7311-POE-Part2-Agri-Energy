using AgriEnergyConnect.Models;
using AgriEnergyConnect.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductService
{
    Task<List<Product>> GetProductsAsync(int farmId);
    Task AddProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
    Task<bool> DeleteProductAsync(int productId, int farmId);
    Task<ProductFilterViewModel> GetFilteredProductsAsync(string? userId, string? category, DateTime? startDate, DateTime? endDate);
    Task<List<string>> GetDistinctCategoriesAsync();
    Task<Product> AddProductAsync(ProductInputModel input, int farmId, string userId);

}