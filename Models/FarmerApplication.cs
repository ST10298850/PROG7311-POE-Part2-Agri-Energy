using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models;

public class FarmerApplication
{
    [Key]
    public int ApplicationID { get; set; }

    [Required]
    public string FarmName { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string FarmType { get; set; } = string.Empty; // e.g. Livestock, Crop, Mixed

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending"; // or Approved, Rejected

    public DateTime SubmissionDate { get; set; } = DateTime.Now;

    [ForeignKey("User")]
    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; }
}