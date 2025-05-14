using AgriEnergyConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
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
}