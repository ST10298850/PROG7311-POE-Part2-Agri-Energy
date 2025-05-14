using AgriEnergyConnect.Services;
using AgriEnergyConnect.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using AppUser = AgriEnergyConnect.Models.User;

public class FarmerController : Controller
{
    private readonly IProductService _productService;
    private readonly IFarmService _farmService;
    private readonly UserManager<AppUser> _userManager;

    public FarmerController(
        IProductService productService,
        IFarmService farmService,
        UserManager<AppUser> userManager)
    {
        _productService = productService;
        _farmService = farmService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var farmId = await _farmService.GetFarmIdForUserAsync(User);
        var products = await _productService.GetProductsAsync(farmId);
        var vm = new AddProductViewModel
        {
            Products = products
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProduct([FromBody] ProductInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                errors = ModelState.ToDictionary(
                    kvp => kvp.Key.Replace("NewProduct.", ""),
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
            });
        }

        var farmId = await _farmService.GetFarmIdForUserAsync(User);
        var currentUser = await _userManager.GetUserAsync(User); // Use UserManager here

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var product = new Product
        {
            Name = input.Name,
            Description = string.IsNullOrEmpty(input.Description) ? "No Description" : input.Description,
            Category = input.Category,
            ProductionDate = input.ProductionDate,
            FarmID = farmId,
            UserId = currentUser.Id // Safer and cleaner
        };

        await _productService.AddProductAsync(product);

        return Ok(new
        {
            product = new
            {
                name = product.Name,
                description = product.Description,
                category = product.Category,
                productionDate = product.ProductionDate.ToString("yyyy-MM-dd")
            }
        });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var farmId = await _farmService.GetFarmIdForUserAsync(User);
        var result = await _productService.DeleteProductAsync(id, farmId);

        if (result)
        {
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}

