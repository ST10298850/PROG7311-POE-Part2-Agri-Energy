using System;
using AppUser = AgriEnergyConnect.Models.User;


namespace AgriEnergyConnect.Models;

public class ProductFilterViewModel
{
    public string? UserId { get; set; }
    public string? Category { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public List<Product> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public List<AppUser> Farmers { get; set; } = new();
}
