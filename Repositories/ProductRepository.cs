using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    // public async Task<List<Product>> GetProductsByFarmIdAsync(int farmId)
    // {
    //     return await _context.Products
    //         .Where(p => p.FarmID == farmId)
    //         .ToListAsync();
    // }
    public async Task<List<Product>> GetProductsByFarmIdAsync(int farmId)
    {
        return await _context.Products
            .Include(p => p.Farm)
            .Include(p => p.User)
            .Where(p => p.FarmID == farmId)
            .ToListAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Farm)
            .Include(p => p.User)
            .ToListAsync();
    }
}