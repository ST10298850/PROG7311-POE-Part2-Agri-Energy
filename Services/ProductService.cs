using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Service class for managing product-related operations.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the ProductService class.
    /// </summary>
    /// <param name="repo">The product repository.</param>
    /// <param name="userManager">The user manager for identity operations.</param>
    public ProductService(IProductRepository repo, UserManager<User> userManager)
    {
        _repo = repo;
        _userManager = userManager;
    }

    /// <summary>
    /// Retrieves products for a specific farm.
    /// </summary>
    /// <param name="farmId">The ID of the farm.</param>
    /// <returns>A list of products for the specified farm.</returns>
    public Task<List<Product>> GetProductsAsync(int farmId) =>
        _repo.GetProductsByFarmIdAsync(farmId);

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product to add.</param>
    public Task AddProductAsync(Product product) =>
        _repo.AddProductAsync(product);

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>A list of all products.</returns>
    public Task<List<Product>> GetAllProductsAsync() =>
        _repo.GetAllProductsAsync();

    /// <summary>
    /// Deletes a specific product from the database.
    /// </summary>
    /// <param name="productId">The ID of the product to delete.</param>
    /// <param name="farmId">The ID of the farm associated with the product.</param>
    /// <returns>True if the product was successfully deleted, false otherwise.</returns>
    public async Task<bool> DeleteProductAsync(int productId, int farmId)
    {
        return await _repo.DeleteProductAsync(productId, farmId);
    }

    /// <summary>
    /// Retrieves filtered products based on specified criteria.
    /// </summary>
    /// <param name="userId">The ID of the user (optional).</param>
    /// <param name="category">The category of products (optional).</param>
    /// <param name="startDate">The start date for filtering (optional).</param>
    /// <param name="endDate">The end date for filtering (optional).</param>
    /// <returns>A ProductFilterViewModel containing filtered products and related data.</returns>
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

    /// <summary>
    /// Retrieves a list of distinct product categories.
    /// </summary>
    /// <returns>A list of distinct product categories.</returns>
    public async Task<List<string>> GetDistinctCategoriesAsync()
    {
        var products = await _repo.GetAllProductsAsync();
        return products.Select(p => p.Category).Distinct().ToList();
    }

    /// <summary>
    /// Adds a new product to the database based on the input model.
    /// </summary>
    /// <param name="input">The input model containing product details.</param>
    /// <param name="farmId">The ID of the farm associated with the product.</param>
    /// <param name="userId">The ID of the user adding the product.</param>
    /// <returns>The newly created product.</returns>
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