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
        var categories = await _productService.GetDistinctCategoriesAsync();
        var vm = new AddProductViewModel
        {
            Products = products,
            Categories = categories
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
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var product = await _productService.AddProductAsync(input, farmId, currentUser.Id);

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

