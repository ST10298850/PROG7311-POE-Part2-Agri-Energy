using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

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