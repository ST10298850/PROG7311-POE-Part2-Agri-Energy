using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models;

public class User : IdentityUser
{
    [Key]
    public override string Id { get; set; } = string.Empty;

    public override string? Email { get; set; }

    public int LoginStreak { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;

    // Navigation properties
    public UserDetail? UserDetail { get; set; }

    public ICollection<Farm> Farms { get; set; } = new List<Farm>();
}