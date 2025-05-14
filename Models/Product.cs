using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models;
public class Product
{
    [Key]
    public int ProductID { get; set; }

    [ForeignKey("Farm")]
    public int FarmID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Category { get; set; } // e.g. Vegetables, Livestock

    [Required]
    public DateTime ProductionDate { get; set; }

    public string Description { get; set; }

    // New: Reference to the farmer (User)
    [Required]
    [ForeignKey("User")]
    public string UserId { get; set; }

    public User User { get; set; }

    public Farm Farm { get; set; }
}