using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Repository class for managing product-related database operations.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ProductRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all products associated with a specific farm.
    /// </summary>
    /// <param name="farmId">The ID of the farm.</param>
    /// <returns>A list of products for the specified farm.</returns>
    public async Task<List<Product>> GetProductsByFarmIdAsync(int farmId)
    {
        return await _context.Products
            .Include(p => p.Farm)
            .Include(p => p.User)
            .Where(p => p.FarmID == farmId)
            .ToListAsync();
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product to add.</param>
    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>A list of all products.</returns>
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Farm)
            .Include(p => p.User)
            .ToListAsync();
    }

    /// <summary>
    /// Deletes a specific product from the database.
    /// </summary>
    /// <param name="productId">The ID of the product to delete.</param>
    /// <param name="farmId">The ID of the farm associated with the product.</param>
    /// <returns>True if the product was successfully deleted, false otherwise.</returns>
    public async Task<bool> DeleteProductAsync(int productId, int farmId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductID == productId && p.FarmID == farmId);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}