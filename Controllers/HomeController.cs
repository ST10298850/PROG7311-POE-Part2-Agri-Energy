using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Controllers;

/// <summary>
/// Controller responsible for handling home-related actions and views.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Initializes a new instance of the HomeController.
    /// </summary>
    /// <param name="logger">The logger used for logging information and errors.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Displays the home page of the application.
    /// </summary>
    /// <returns>The Index view.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the privacy policy page of the application.
    /// </summary>
    /// <returns>The Privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Handles and displays error information when an error occurs in the application.
    /// </summary>
    /// <returns>The Error view with error details.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}