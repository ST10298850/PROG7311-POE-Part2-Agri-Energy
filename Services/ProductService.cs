using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriEnergyConnect.ViewModels;

using Microsoft.AspNetCore.Identity;


public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly UserManager<User> _userManager;

    public ProductService(IProductRepository repo, UserManager<User> userManager)
    {
        _repo = repo;
        _userManager = userManager;
    }

    public Task<List<Product>> GetProductsAsync(int farmId) =>
        _repo.GetProductsByFarmIdAsync(farmId);

    public Task AddProductAsync(Product product) =>
        _repo.AddProductAsync(product);

    public Task<List<Product>> GetAllProductsAsync() =>
        _repo.GetAllProductsAsync();
    public async Task<bool> DeleteProductAsync(int productId, int farmId)
    {
        return await _repo.DeleteProductAsync(productId, farmId);
    }

    public async Task<ProductFilterViewModel> GetFilteredProductsAsync(string? userId, string? category, DateTime? startDate, DateTime? endDate)
    {
        var query = await _repo.GetAllProductsAsync();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(p => p.UserId == userId).ToList();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category).ToList();

        if (startDate.HasValue)
            query = query.Where(p => p.ProductionDate >= startDate.Value).ToList();

        if (endDate.HasValue)
            query = query.Where(p => p.ProductionDate <= endDate.Value).ToList();

        var farmers = (await _userManager.GetUsersInRoleAsync("Farmer")).ToList();

        return new ProductFilterViewModel
        {
            Products = query,
            Categories = await GetDistinctCategoriesAsync(),
            Farmers = farmers,
            UserId = userId,
            Category = category,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    public async Task<List<string>> GetDistinctCategoriesAsync()
    {
        var products = await _repo.GetAllProductsAsync();
        return products.Select(p => p.Category).Distinct().ToList();
    }

    public async Task<Product> AddProductAsync(ProductInputModel input, int farmId, string userId)
    {
        var product = new Product
        {
            Name = input.Name,
            Description = string.IsNullOrEmpty(input.Description) ? "No Description" : input.Description,
            Category = input.Category,
            ProductionDate = input.ProductionDate,
            FarmID = farmId,
            UserId = userId
        };

        await _repo.AddProductAsync(product);
        return product;
    }

}
