using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AgriEnergyConnect.Models;

public class Farm
{
    [Key]
    public int FarmID { get; set; }

    [ForeignKey("User")]
    public string UserID { get; set; } = string.Empty;

    [Required]
    public string FarmName { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string FarmType { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public List<Product> Products { get; set; } = new List<Product>();
}